from multiprocessing import Process, Value
import tensorflow as tf
import tensorflow_hub as hub
import cv2 as cv
import cv2
import numpy as np
import pickle
import time
import pygame
import serial
import socket
import argparse
import os
import sys
import math
import struct
import threading
from dataclasses import dataclass

#C#UI에서 사람 클릭하면 추적을 하기 위한 클래스
class click_data:
    Left_or_right:int = 3
    X:int = 0
    Y:int = 0
Click = click_data()

#멀티 스레드에 사용할 공유변수
###########################멀티 스레드에 사용할 공유변수##########################
#C#클릭 데이터
C_LR = Value('i', 3)
C_X = Value('i', 0)
C_Y = Value('i', 0)

#사람 트래킹 에러값
RGB_Com_X = Value('i', 0)
RGB_Com_Y = Value('i', 0)

#레이저 트래킹 에러값
IR_Com_X = Value('i', 0)
IR_Com_Y = Value('i', 0)

#tof센서값
target_distance = Value('i', 0)

#모드 변환 정보 데이터(조이스틱 버튼 데이터)
Button_Command = Value('i', 0)
################################################################################

##############################사람 트래킹 프로세스################################
def RGB_pro(C_LR_, C_X_, C_Y_, RGB_Com_X_, RGB_Com_Y_, target_distance_, Button_Command_):
    #영상 송신을 위한 UDP 통신 설정
    max_length = 65000
    UDP_host_IP = "10.254.1.20"
    UDP_host_port = 9000
    UDP_Client_IP = "10.254.2.172"
    UDP_Client_port = 8000

    UDP = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    UDP.bind((UDP_host_IP, UDP_host_port))
    
    #RGB 카메라 해상도
    RGB_Cam_Res_X = 640
    RGB_Cam_Res_Y = 480

    #RGB 카메라 화각 라디안각 변환
    RGB_CAM_FoV = math.pi * (13.5 / 2 / 180)
    
    #캘리할 때 보정하기 위한 값
    RGB_Cam_tilt = math.pi * (0 / 180)
    RGB_Cam_tilt_offset = int(-((0 / 6.75) * 320))
    tof_tilt = math.pi * (0 / 180)

    #화면에 표시할 조준선 길이
    aim_lenth = 10

    #거리에 따른 조준선 보정 정도(픽셀 단위)
    RGB_Cam_aim_X = 0
    RGB_Cam_aim_Y = 0
    
    #RGB 카메라
    cap1 = cv2.VideoCapture(0)
    cap1.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    cap1.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
    cap1.set(cv2.CAP_PROP_FPS, 30)

    #tensorflow 모델 및 함수 불러오기
    model = hub.load('https://tfhub.dev/google/movenet/multipose/lightning/1')
    movenet = model.signatures['serving_default']

    #사람 인식 되는 부분 바운딩 박스 표시 함수
    def draw_bounding_box(frame, keypoints, confidence_threshold):
        y, x, c = frame.shape
        shaped = np.squeeze(np.multiply(keypoints, [y, x, 1]))
        valid_points = [kp for kp in shaped if kp[2] > confidence_threshold]
        if not valid_points:
            return None

        min_x = min([kp[1] for kp in valid_points])
        min_y = min([kp[0] for kp in valid_points])
        max_x = max([kp[1] for kp in valid_points])
        max_y = max([kp[0] for kp in valid_points])

        cv2.rectangle(frame, (int(min_x), int(min_y)), (int(max_x), int(max_y)), (255, 0, 0), 2)
        return (min_x, min_y, max_x, max_y)

    def loop_through_people(frame, keypoints_with_scores, confidence_threshold):
        rectangles = []
        for person in keypoints_with_scores:
            # 트래커가 활성화되지 않았을 때만 바운딩 박스를 그립니다.
            if tracker is None:
                rect = draw_bounding_box(frame, person, confidence_threshold)
                if rect is not None:
                    rectangles.append(rect)
        return rectangles

    # Tracker 초기화
    tracker = None

    # 멀티 프로세스에서 동작 안되는 사람 클릭하면 트래킹할 수 있게 하는 이벤트 함수
    # C#에서 클릭하면 동작되므로 신경쓰지 않기
    def mouse_callback(event, x, y, flags, param):
        if event == cv2.EVENT_LBUTTONDOWN:
            for rect in rectangles_info:
                min_x, min_y, max_x, max_y = rect
                if min_x <= x <= max_x and min_y <= y <= max_y:
                    tracker = cv2.TrackerCSRT_create()
                    bbox = (int(min_x), int(min_y), int(max_x - min_x), int(max_y - min_y))
                    tracker.init(frame1, bbox)
                    break
        elif event == cv2.EVENT_RBUTTONDOWN:
            tracker = None

    #tensorflow GPU로 돌리기
    gpus = tf.config.list_physical_devices('GPU')
    if gpus:
      # 텐서플로가 첫 번째 GPU만 사용하도록 제한
      try:
        tf.config.set_visible_devices(gpus[0], 'GPU')
      except RuntimeError as e:
        # 프로그램 시작시에 접근 가능한 장치가 설정되어야만 합니다
        print(e)

    #카메라 캘리브레이션 정보 저장
    with open("RGB_calibration.pkl", "rb") as f:
        RGB_cameraMatrix, RGB_dist = pickle.load(f)

    #창 띄우기
    cv2.namedWindow("Movenet Multipose")
    cv2.setMouseCallback("Movenet Multipose", mouse_callback)

    #카메라 연결되어 있으면 동작
    while cap1.isOpened():
        success1, frame1 = cap1.read()
    
        if success1:
            #카메라 캘리브레이션 정보로 왜곡 펴기
            frame1 = cv.undistort(frame1, RGB_cameraMatrix, RGB_dist, None)
            
            #tof 센서 데이터 쓰기 편하게 지역변수 하나 추가
            dist = target_distance_.value
            
            #거리 값에 따른 조준선 정렬을 위한 계산
            pixel_distance = math.tan(RGB_CAM_FoV) * dist
            pixel_distance = pixel_distance / (RGB_Cam_Res_X / 2)
            if pixel_distance != 0:
                #최종 보정해야 할 픽셀단위 값 계산
                RGB_Cam_aim_X = int(5.5 / pixel_distance)
                
            #조준선 정렬
            cv2.line(frame1, (int(RGB_Cam_Res_X / 2) - 1 - aim_lenth + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1), (int(RGB_Cam_Res_X / 2) - 1  + aim_lenth + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1), (0, 0, 255), 2)
            cv2.line(frame1, (int(RGB_Cam_Res_X / 2) - 1  + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1 - aim_lenth), (int(RGB_Cam_Res_X / 2) - 1 + RGB_Cam_aim_X + RGB_Cam_tilt_offset , int(RGB_Cam_Res_Y / 2) - 1 + aim_lenth), (0, 0, 255), 2)

            #오른쪽 마우스 클릭 시 사람 트래킹 Release 해제
            #밑에 C_로 시작하는 변수들은 tcp/ip 통신으로 수신 받은 데이터
            if C_LR_.value == 2:
                tracker = None
                C_LR_.value = 3
                C_X_.value = 0
                C_Y_.value = 0

            #사람 트래킹 중이 아니면 텐서플로우 동작
            if tracker is None:
                img = frame1.copy()
                img = tf.image.resize_with_pad(tf.expand_dims(img, axis=0), 480, 640)
                input_img = tf.cast(img, dtype=tf.int32)
         
                #사람 인식하는 함수
                results = movenet(input_img)
         
                keypoints_with_scores = results['output_0'].numpy()[:,:,:51].reshape((6,17,3))
       
                rectangles_info = loop_through_people(frame1, keypoints_with_scores, 0.5)

                #마우스 왼쪽 클릭 시 사람 트래킹 시작
                if C_LR_.value == 1:
                    for rect in rectangles_info:
                        min_x, min_y, max_x, max_y = rect
                        if min_x <= C_X_.value <= max_x and min_y <= C_Y_.value <= max_y:
                            tracker = cv2.TrackerCSRT_create()
                            bbox = (int(min_x), int(min_y), int(max_x - min_x), int(max_y - min_y))
                            C_LR_.value = 3
                            C_X_.value = 0
                            C_Y_.value = 0
                            success1, frame1 = cap1.read()
                            tracker.init(frame1, bbox)
                            break
            #트래킹 중이면
            elif tracker is not None:
                #트래킹 정보 업데이트
                ret, bbox = tracker.update(frame1)
                if ret:
                    x, y, w, h = [int(v) for v in bbox]
                    cv2.rectangle(frame1, (x, y), (x + w, y + h), (0, 255, 0), 2)
                    #조준선과 추적 중인 사람 간의 에러값 계산
                    if (Button_Command_.value & 0x10) == 0x10:
                        RGB_Com_X_.value = int(((x + x + w) / 2) - (320 + RGB_Cam_aim_X))
                        RGB_Com_Y_.value = 240 - int(((y + y + h) / 2))
                else:
                    # 트래킹 실패 시 트래커 리셋
                    tracker = None

            #RGB 카메라 영상 출력
            cv2.imshow('Movenet Multipose', frame1)

        ##UDP 영상 송신##
        retval, buffer = cv2.imencode(".jpg", frame1)
        if retval:
            buffer = buffer.tobytes()
            buffer_size = len(buffer)

            num_of_packs = 1
            if buffer_size > max_length:
                num_of_packs = math.ceil(buffer_size/max_length)

            frame_info = {"packs":num_of_packs}

            left = 0
            right = max_length

            for i in range(num_of_packs):
                data = buffer[left:right]
                left = right
                right += max_length
                UDP.sendto(data, (UDP_Client_IP, UDP_Client_port))
        
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap1.release()
    cv2.destroyAllWindows()
################################################################################


###############################IR 카메라 프로세스################################
def IR_pro(IR_Com_X_, IR_Com_Y_, target_distance_, Button_Command_):
    #캘리브레이션 정보 저장
    with open("IR_calibration.pkl", "rb") as f:
        IR_cameraMatrix, IR_dist = pickle.load(f)
        
    #IR 카메라 해상도
    IR_Cam_Res_X = 640
    IR_Cam_Res_Y = 480

    #IR 카메라 화각 라디안 각으로 변환
    IR_CAM_FoV = -math.pi * (64.6 / 2 / 180)
    
    #캘리브레이션으로 보정할 값들 변수
    IR_Cam_tilt = math.pi * (0 / 180)
    IR_Cam_tilt_offset = int(-((0 / 6.75) * 320))
    tof_tilt = math.pi * (0 / 180)

    #영상에 출력할 조준선 길이
    aim_lenth = 10

    #조준선 보정 값(픽셀 단위)
    IR_Cam_aim_X = 0
    IR_Cam_aim_Y = 0

    #IR 카메라
    cap2 = cv2.VideoCapture(2)
    cap2.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    cap2.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
    cap2.set(cv2.CAP_PROP_FPS, 30)

    #나중에 데모 환경에 맞춰서 변경할 수 있는 파라메타
    #블러치리할 영역(), 나눌 숫자
    kernel = np.ones((3, 3))/3**2

    #침식 범위
    array = np.array([[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1]])

    #IR 카메라 연결 되어 있을 때
    while cap2.isOpened():
        success2, frame2 = cap2.read()
        if success2:
            #캘리브레이션으로 왜곡 펴기
            frame2 = cv.undistort(frame2, IR_cameraMatrix, IR_dist, None)
    
            frame2_gray = cv2.cvtColor(frame2, cv2.COLOR_BGR2GRAY)

            blured = cv2.filter2D(frame2_gray, -1, kernel) #블러처리

            ret_set_binary, set_binary = cv2.threshold(blured, 200, 255, cv2.THRESH_BINARY) #이진화
        
            erode = cv2.erode(set_binary, array) #침식
            dilate = cv2.dilate(erode, array) #팽창
        
            white_pixel_coordinates = np.column_stack(np.nonzero(dilate))
            
            #Tof 센서 값 지역 변수 하나 추가
            dist = target_distance_.value
            
            #조준선 정렬 보정 값 계산
            pixel_distance = math.tan(IR_CAM_FoV) * dist
            pixel_distance = pixel_distance / (IR_Cam_Res_X / 2)
            if pixel_distance != 0:
                IR_Cam_aim_X = int(5 / pixel_distance)

            #흰 색 부분이 있을 때
            if len(white_pixel_coordinates) > 0:
                center = np.mean(white_pixel_coordinates, axis=0)
                center = tuple(map(int, center))  # 정수형으로 변환
                #정렬된 조준선과 레이저 에러값 계산
                if (Button_Command_.value & 0x20) == 0x20:
                    IR_Com_Y_.value = int(240 - center[0])
                    IR_Com_X_.value = int(center[1] - (320 + IR_Cam_aim_X))
                    
            dilate = cv2.cvtColor(dilate, cv2.COLOR_GRAY2BGR)
            cv2.line(dilate, (int(IR_Cam_Res_X / 2) - 1 - aim_lenth + IR_Cam_aim_X + IR_Cam_tilt_offset, int(IR_Cam_Res_Y / 2) - 1), (int(IR_Cam_Res_X / 2) - 1  + aim_lenth + IR_Cam_aim_X + IR_Cam_tilt_offset, int(IR_Cam_Res_Y / 2) - 1), (0, 0, 255), 2)
            cv2.line(dilate, (int(IR_Cam_Res_X / 2) - 1  + IR_Cam_aim_X + IR_Cam_tilt_offset, int(IR_Cam_Res_Y / 2) - 1 - aim_lenth), (int(IR_Cam_Res_X / 2) - 1 + IR_Cam_aim_X + IR_Cam_tilt_offset , int(IR_Cam_Res_Y / 2) - 1 + aim_lenth), (0, 0, 255), 2)

            cv2.imshow('Camera Window1', dilate)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
 
    cap2.release()
    cv2.destroyAllWindows()
################################################################################
    
    
################################################################################
# 조이스틱 초기화 함수
def init_gamepad():
    pygame.init()
    pygame.joystick.init()
    joystick_count = pygame.joystick.get_count()
    if joystick_count == 0:
        print("게임 패드가 연결되어 있지 않습니다.")
        return None

    joystick = pygame.joystick.Joystick(0)
    joystick.init()
    print(f"{joystick.get_name()} 게임 패드가 연결되었습니다.")
    return joystick

#CAN_ID 3번 모터를 위한 변수들
#시동
start_up_3 = 0

# 가속도값
ACC3 = 210

# 이전 속도 기억하기
pre_3_Speed = 0

# 방향 정보
M3_state_dir = 0

#CAN 통신으로 respond되는 데이터들
M3_Stop = b'\x03\x03\xf6\x02\xfb'
M3_Move = b'\x03\x03\xf6\x01\xfa'
M3_state = b'\x03\x03\xf6\x02\xfb'
M3_error = b'\x03\x03\xf6\x00\xf9'

CRCBYTE3 = (0x3 + 0xF6 + ACC3) & 0xFF
packet3 = [3, 5, 0xF6, 0, 0, ACC3, CRCBYTE3]


#CAN_ID 3번 모터를 위한 변수들
#시동
start_up_4 = 0

# 가속도값
ACC4 = 100

# 이전 속도 기억하기
pre_4_Speed = 0

# 방향 정보
M4_state_dir = 0

#CAN 통신으로 respond되는 데이터들
M4_Stop = b'\x04\x03\xf6\x02\xfc'
M4_Move = b'\x04\x03\xf6\x01\xfb'
M4_state = b'\x04\x03\xf6\x02\xfc'
M4_error = b'\x04\x03\xf6\x00\xfa'

CRCBYTE4 = (0x4 + 0xF6 + ACC4) & 0xFF
packet4 = [4, 5, 0xF6, 0, 0, ACC4, CRCBYTE4]

#조이스틱 데이터 저장을 위한 클래스
class joy_data:
    axisX:int = 0
    axisY:int = 0
    axisT:int = 0
    button:int = 0xfc000004
joy = joy_data()

# 메인 틴지 연결
try:
    teensy = serial.Serial('/dev/ttyACM0', 500000, timeout = 0)
except IOError as e:
    print(e)
    print("Teensy 어디")
    exit()
    
# CAN통신으로 엔코더 값 불러오라고 하는 명령 보내는 함수
def read_encoder2(canid):
    byte1 = 0x31
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))

# 받은 엔코더 값을 각도로 변환, 받은 데이터, 기어비 함수 인자
def calculate_encoder(byttee, Gear_ratio):
    if byttee[1] == 0:
        ee = byttee[6] + (byttee[5] << 8)+ (byttee[4] << 16)+ (byttee[3] << 24)+ (byttee[2] << 32)
        ee = (ee * 360) / ((2 ** 14) * Gear_ratio)
        return ee
    elif byttee[1] == 255:
        ee = -((255 - byttee[6]) + ((255 - byttee[5]) << 8) + ((255 - byttee[4]) << 16) + ((255 - byttee[3]) << 24) + ((255 - byttee[2]) << 32))
        ee = (ee * 360) / ((2 ** 14) * Gear_ratio)
        return ee
                
#CAN통신으로 Tof 값 불러오라고 하는 명령 보내는 함수
def read_dist(canid):
    CRC = (canid + 0x01 + 0x00) & 0xFF
    ACKPACKET = [canid, 3, 0x01, 0x00, CRC]
    teensy.write(bytearray(ACKPACKET))
    
#CAN_ID 3번 모터 속도 0 보내는 함수
def Go_to_Speed_Zero_3():
    global ACC3, CRCBYTE3, packet3
    CRCBYTE3 = (0x3 + 0xF6 + ACC3) & 0xFF
    packet3 = [3, 5, 0xF6, 0, 0, ACC3, CRCBYTE3]
    
#CAN_ID 3번 모터 지정 속도 보내는 함수
def Go_to_target_Speed_3(Speed):
    global ACC3, CRCBYTE3, packet3
    if Speed > 0:
        DIR = 0
        upper_speed = (Speed >> 8) & 0x0F
        lower_speed = Speed & 0xFF
        BYTE2 = (DIR << 7) | upper_speed
        CRCBYTE3 = (0x3 + 0xF6 + BYTE2 + lower_speed + ACC3) & 0xFF
        packet3 = [3, 5, 0xF6, BYTE2, lower_speed, ACC3, CRCBYTE3]
    elif Speed < 0:
        DIR = 1
        upper_speed = (abs(Speed) >> 8) & 0x0F
        lower_speed = abs(Speed) & 0xFF
        BYTE2 = (DIR << 7) | upper_speed
        CRCBYTE3 = (0x3 + 0xF6 + BYTE2 + lower_speed + ACC3) & 0xFF
        packet3 = [3, 5, 0xF6, BYTE2, lower_speed, ACC3, CRCBYTE3]

#CAN_ID 3번 모터 가감속하기 위해 짠 함수
def Active_Motor_3(Speed):
    global ACC3, pre_3_Speed, M3_state_dir, M3_state, M3_Stop, M3_Move, CRCBYTE3, packet3, start_up_3
    if Speed != pre_3_Speed or (Speed != 0 and M3_state == M3_Stop):
        start_up_3 = 1
        if Speed == 0:
            Go_to_Speed_Zero_3()
        elif Speed > 15:
            if (M3_state == M3_Stop) or M3_state_dir == 1:
                Go_to_target_Speed_3(Speed)
                M3_state_dir = 1
            elif M3_state_dir == -1:
                Go_to_Speed_Zero_3()
        elif Speed > 0 and Speed <= 15:
            if (pre_3_Speed > 15) or (M3_state_dir == -1):
                Go_to_Speed_Zero_3()
            elif M3_state == M3_Stop:
                Go_to_target_Speed_3(Speed)
                M3_state_dir = 1
        elif Speed < -15:
            if (M3_state == M3_Stop) or M3_state_dir == -1:
                Go_to_target_Speed_3(Speed)
                M3_state_dir = -1
            elif (M3_state_dir == 1):
                Go_to_Speed_Zero_3()
        elif Speed < 0 and Speed >= -15:
            if (pre_3_Speed < -15) or (M3_state_dir == 1):
                Go_to_Speed_Zero_3()
            elif M3_state == M3_Stop:
                Go_to_target_Speed_3(Speed)
                M3_state_dir = -1
        M3_state = M3_Move
        teensy.write(bytearray(packet3))
        pre_3_Speed = Speed
    elif Speed == 0 and M3_state == M3_Move and start_up_3 == 1:
        Go_to_Speed_Zero_3()
        teensy.write(bytearray(packet3))
        start_up_3 = 0
            
#CAN_ID 4번 모터 속도 0 보내는 함수
def Go_to_Speed_Zero_4():
    global ACC4, CRCBYTE4, packet4
    CRCBYTE4 = (0x4 + 0xF6 + ACC4) & 0xFF
    packet4 = [4, 5, 0xF6, 0, 0, ACC4, CRCBYTE4]
    
#CAN_ID 4번 모터 지정 속도 보내는 함수
def Go_to_target_Speed_4(Speed):
    global ACC4, CRCBYTE4, packet4
    if Speed > 0:
        DIR = 0
        upper_speed = (Speed >> 8) & 0x0F
        lower_speed = Speed & 0xFF
        BYTE2 = (DIR << 7) | upper_speed
        CRCBYTE4 = (0x4 + 0xF6 + BYTE2 + lower_speed + ACC4) & 0xFF
        packet4 = [4, 5, 0xF6, BYTE2, lower_speed, ACC4, CRCBYTE4]
    elif Speed < 0:
        DIR = 1
        upper_speed = (abs(Speed) >> 8) & 0x0F
        lower_speed = abs(Speed) & 0xFF
        BYTE2 = (DIR << 7) | upper_speed
        CRCBYTE4 = (0x4 + 0xF6 + BYTE2 + lower_speed + ACC4) & 0xFF
        packet4 = [4, 5, 0xF6, BYTE2, lower_speed, ACC4, CRCBYTE4]

#CAN_ID 4번 모터 가감속하기 위해 짠 함수
def Active_Motor_4(Speed):
    global ACC4, pre_4_Speed, M4_state_dir, M4_state, M4_Stop, M4_Move, CRCBYTE4, packet4, start_up_4
    if Speed != pre_4_Speed or (Speed != 0 and M4_state == M4_Stop):
        start_up_4 = 1
        if Speed == 0:
            Go_to_Speed_Zero_4()
        elif Speed > 15:
            if (M4_state == M4_Stop) or (M4_state_dir == 1):
                Go_to_target_Speed_4(Speed)
                M4_state_dir = 1
            elif (M4_state_dir == -1):
                Go_to_Speed_Zero_4()
        elif Speed > 0 and Speed <= 15:
            if (pre_4_Speed > 15) or (M4_state_dir == -1):
                Go_to_Speed_Zero_4()
            elif M4_state == M4_Stop:
                Go_to_target_Speed_4(Speed)
                M4_state_dir = 1
        elif Speed < -15:
            if (M4_state == M4_Stop) or (M4_state_dir == -1):
                Go_to_target_Speed_4(Speed)
                M4_state_dir = -1
            elif (M4_state_dir == 1):
                Go_to_Speed_Zero_4()
        elif Speed < 0 and Speed >= -15:
            if (pre_4_Speed < -15) or (M4_state_dir == 1):
                Go_to_Speed_Zero_4()
            elif (M4_state == M4_Stop):
                Go_to_target_Speed_4(Speed)
                M4_state_dir = -1
        M4_state = M4_Move
        teensy.write(bytearray(packet4))
        pre_4_Speed = Speed
    elif Speed == 0 and M4_state == M4_Move and start_up_4 == 1:
        Go_to_Speed_Zero_4()
        teensy.write(bytearray(packet4))
        start_up_4 = 0
        
#조이스틱 정보 처리 함수(이벤트)
def get_gamepad_input(joystick):
    # 이벤트 처리
    for event in pygame.event.get():
        if event.type == pygame.JOYAXISMOTION:
            if event.axis == 0:
                if (joy.button & 0x04) == 0x04:
                    if event.value > -0.2025 and event.value < 0.2025:
                        joy.axisX = int(0)
                    else:
                        if event.value >= 0.2025:
                            joy.axisX = int(event.value * 400 - 80)
                        elif event.value <= -0.2025:
                            joy.axisX = int(event.value * 400 + 80)
                elif (joy.button & 0x08) == 0x08:
                    if event.value > -0.25 and event.value < 0.25:
                        joy.axisX = int(0)
                    else:
                        if event.value >= 0.25:
                            joy.axisX = int(event.value * 20 - 4)
                        elif event.value <= -0.25:
                            joy.axisX = int(event.value * 20 + 4)
            if event.axis == 1:
                if (joy.button & 0x04) == 0x04:
                    if event.value > -0.22 and event.value < 0.22:
                        joy.axisY = int(0)
                    else:
                        if event.value >= 0.22 :
                            joy.axisY = int(event.value * 50 - 10)
                        elif event.value <= -0.22 :
                            joy.axisY = int(event.value * 50 + 10)
                elif (joy.button & 0x08) == 0x08:
                    if event.value > -0.3 and event.value < 0.3:
                        joy.axisY = int(0)
                    else:
                        if event.value >= 0.3 :
                            joy.axisY = int(event.value * 10 - 2)
                        elif event.value <= -0.3 :
                            joy.axisY = int(event.value * 10 + 2)
        if event.type == pygame.JOYBUTTONDOWN:
            if event.button == 0:
                joy.button = (joy.button | 0x01)
                print("Fire_On")
            elif event.button == 1:
                joy.button = (joy.button | 0x02)
                print("Get_Target")
            elif event.button == 2:
                joy.button = (joy.button & ~(0x38))
                joy.button = (joy.button | 0x04)
                print("Fast_Manual_Activate")
            elif event.button == 3:
                joy.button = (joy.button & ~(0x34))
                joy.button = (joy.button | 0x08)
                print("Slow_Manual_Activate")
            elif event.button == 4:
                joy.button = (joy.button & ~(0x2c))
                joy.button = (joy.button | 0x10)
                print("Tracking_person")
            elif event.button == 5:
                joy.button = (joy.button & ~(0x1c))
                joy.button = (joy.button | 0x20)
                print("Tracking_lazer")
            Button_Command.value = joy.button
        elif event.type == pygame.JOYBUTTONUP:
            if event.button == 0:
                print("Fire_Off")
            elif event.button == 1:
                print("release_Target")
            Button_Command.value = joy.button
            

gamepad = init_gamepad()
################################################################################

# 엔코더 값에 따른 모터 각도 저장 변수
Optical_angle = 0
Body_angle = 0

# 엔코더 값에 따른 이전 모터 각도 저장 변수
pre_Optical_angle = 0
pre_Body_angle = 0
    
##############################TCP/IP 통신을 위한 설정, 쓰레드######################
TCP_host_IP = "10.254.1.20"
TCP_host_port = 7000

TCP = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
TCP.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 0)

TCP.bind((TCP_host_IP, TCP_host_port))

TCP.listen(2)

def Get_Joystick(client_socket, addr):
    global joy, Button_Command
    while 1:    
        data = client_socket.recv(16)
        ds = struct.unpack('3iI', data)
        if ds[3] & 0xfc000000 == 4227858432:
            joy.axisX = ds[0]
            joy.axisY = ds[1]
            joy.axisT = ds[2]
            joy.button = ds[3]
            Button_Command.value = joy.button
        time.sleep(0.001)
        
def Get_Clickdata(client_socket, addr):
    global Click, Optical_angle, pre_Optical_angle, Body_angle, pre_Body_angle
    while 1:
        #여기 받는 쪽에서 블로킹 됌 처리 해야됌
        data = client_socket.recv(12)
        ds = struct.unpack('3i', data)
        
        if ds[0] == 1 or ds[0] == 2:
            C_LR.value = ds[0]
            C_X.value = ds[1]
            C_Y.value = ds[2]
            
        if pre_Optical_angle != Optical_angle or pre_Body_angle != Body_angle:
            sd = struct.pack('3iI', int(Optical_angle), int(Body_angle), joy.axisT, Click.Left_or_right)
            client_socket.sendall(sd)
            pre_Optical_angle = Optical_angle
            pre_Body_angle = Body_angle
            
        time.sleep(0.001)
        
################################################################################


################################################################################
    
if __name__ == '__main__':
    #프로세스 설정, 함수 이름, 공유 변수가 함수 인자
    p1 = Process(target=RGB_pro, args=(C_LR, C_X, C_Y, RGB_Com_X, RGB_Com_Y, target_distance, Button_Command,))
    p2 = Process(target=IR_pro, args=(IR_Com_X, IR_Com_Y, target_distance, Button_Command,))
    
    #프로세스 시작
    p1.start()
    p2.start()
    
    #엔코더값, tof 센서값 보내라는 명령어 송신 시퀀스 제어 변수
    get_data_pre_time = 0
    get_data_seq = 0
    
    #최종 모터 동작 명령 변수(조이스틱, 사람추적, 레이저추적)
    Active_M3_command = 0
    Active_M4_command = 0
    
    #게임패드 연결 안되어 있으면 TCP/IP 통신 2개 들어올 때까지 기다림
    #C# UI 먼저, 조이스틱 파이썬 코드 두 번째로 연결해야 함
    if gamepad is None:
        client_socket1, addr1 = TCP.accept()
        print("success1")
        client_socket2, addr2 = TCP.accept()
        print("success2")
    
        #C#TCP/IP 쓰레드 시작
        t1= threading.Thread(target=Get_Clickdata, args=(client_socket1, addr1))

        t1.daemon = True

        t1.start() 
    
        #파이썬 TCP/IP 쓰레드 시작(조이스틱)
        t2 = threading.Thread(target=Get_Joystick, args=(client_socket2, addr2))

        t2.daemon = True

        t2.start() 
        
    time.sleep(1)
    #메인 Teensy 데이터 처리, 모터 동작
    while 1:
        #특정 주기로 엔코더값, tof값 보내라는 명령 보내기
        if time.time() - get_data_pre_time > 0.005:
            if get_data_seq == 0:
                read_encoder2(3)
                get_data_seq = 1
            elif get_data_seq == 1:
                read_encoder2(4)
                get_data_seq = 2
            elif get_data_seq == 2:
                read_dist(6)
                get_data_seq = 0
            get_data_pre_time = time.time()
            
        if gamepad:
            get_gamepad_input(gamepad)
            
        #조이스틱 변수에 따른 모드 변환
        if (joy.button & 0x04) == 0x04 or (joy.button & 0x08) == 0x08:
            Active_M3_command = joy.axisX
            Active_M4_command = joy.axisY
        elif (joy.button & 0x10) == 0x10:
            Active_M3_command = RGB_Com_X.value
            Active_M4_command = RGB_Com_Y.value
        elif (joy.button & 0x20) == 0x20:
            Active_M3_command = IR_Com_X.value
            Active_M4_command = IR_Com_Y.value
            
        #일단 Teensy에서 두 바이트 읽고 (CAN ID, Data Length)
        data1 = teensy.read(2)
        if len(data1) > 0:
            #Data Length만큼 또 읽음
            data2 = teensy.read(data1[1])
            ##To control Motor data##
            #CAN ID가 3일 때
            if data1[0] == 3:
                #모터 동작 때문에 처리
                if data2 == b'\xf6\x02\xfb' or data2 == b'\xf6\x00\xf9':
                    M3_state = M3_Stop
                    M3_state_dir = 0
                #엔코더값 수신
                elif data1[1] == 8:
                    Body_angle = calculate_encoder(data2, 43.33333)
            #CAN ID가 4일 때
            elif data1[0] == 4:
                #모터 동작 때문에 처리
                if data2 == b'\xf6\x02\xfc' or data2 == b'\xf6\x00\xfa':
                    M4_state = M4_Stop
                    M4_state_dir = 0
                #엔코더값 수신
                elif data1[1] == 8:
                    Optical_angle = calculate_encoder(data2, 10)
            ##To get ToF##
            #TOF 값 수신
            elif data1[0] == 6 and data1[1] == 5:
                target_distance.value = data2[0] + (data2[1] << 8)
                
        #Z축 제한각도 -55 ~ 55, 가속도 때문에 정확히 거기서 멈추는거 아님
        if Body_angle < -55 and Active_M3_command > 0:
            Active_Motor_3(0)
        elif Body_angle > 55 and Active_M3_command < 0:
            Active_Motor_3(0)
        else:
            Active_Motor_3(Active_M3_command)
            
        #광학 제한각도 -30 ~ 30, 가속도 때문에 정확히 거기서 멈추는거 아님
        if Optical_angle < -30 and Active_M4_command > 0:
            Active_Motor_4(0)
        elif Optical_angle > 30 and Active_M4_command < 0:
            Active_Motor_4(0)
        else:
            Active_Motor_4(Active_M4_command)
            
    
device.close() 
