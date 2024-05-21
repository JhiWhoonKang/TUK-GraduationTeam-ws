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
from Optical import Optical

# TCP/IP로 클릭 데이터 받기 위한 클래스
class click_data:
    Left_or_right:int = 3
    X:int = 0
    Y:int = 0
Click = click_data()

#멀티 프로세스에서 사용할 공유 변수들
#클릭 데이터
C_LR = Value('i', 3)
C_X = Value('i', 0)
C_Y = Value('i', 0)

#사람 추적에서 픽셀 단위 에러값
RGB_Com_X = Value('i', 0)
RGB_Com_Y = Value('i', 0)

#레이저 추적에서 픽셀 단위 에러값
IR_Com_X = Value('i', 0)
IR_Com_Y = Value('i', 0)

#TOF 거리 데이터 공유 변수
target_distance = Value('i', 0)

#조이스틱 데이터 공유 변수
Button_Command = Value('i', 0)

###############################RGB 카메라 프로세스###############################
def RGB_pro(C_LR_, C_X_, C_Y_, RGB_Com_X_, RGB_Com_Y_, target_distance_, Button_Command_):
    #영상 송신 UDP 통신 서버, 클라이언트 주소 지정
    max_length = 65000
    UDP_host_IP = "10.254.1.20"
    UDP_host_port = 9000
    UDP_Client_IP = "10.254.2.172"
    UDP_Client_port = 8000

    #UDP 통신 환경 설정
    UDP = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    UDP.bind((UDP_host_IP, UDP_host_port))
    
    #RGB 카메라 해상도 640X480
    RGB_Cam_Res_X = 640
    RGB_Cam_Res_Y = 480

    #RGB 카메라 화각 라디안각으로 변환
    RGB_CAM_FoV = math.pi * (13.5 / 2 / 180)
    
    #하드웨어적으로 틀어져 있는 정도를 보정하기 위한 변수들
    RGB_Cam_tilt = math.pi * (0 / 180)
    RGB_Cam_tilt_offset = int(-((0 / 6.75) * 320))
    tof_tilt = math.pi * (0 / 180)

    #화면에 출력할 조준선 길이
    aim_lenth = 5

    #조준선 보정값 (픽셀 단위)
    RGB_Cam_aim_X = 0
    RGB_Cam_aim_Y = 0
    
    #RGB 카메라 열기
    cap1 = cv2.VideoCapture(0)
    cap1.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    cap1.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
    cap1.set(cv2.CAP_PROP_FPS, 60)

    #Tensorflow 모델 불러오기, 함수 불러오기
    model = hub.load('https://tfhub.dev/google/movenet/multipose/lightning/1')
    movenet = model.signatures['serving_default']

    ###PID 제어기 파라미터###
    previous_Speedx = 0
    previous_Speedy = 0

    errorx = 0
    errory = 0

    previous_errorx = 0
    previous_errory = 0

    PID_time = 0
    PID_previous_time = 0

    PIDControlx = 0
    PControlx = 0
    IControlx = 0
    DControlx = 0

    PIDControly = 0
    PControly = 0
    IControly = 0
    DControly = 0

    Kp = 0.3
    Ki = 0
    Kd = 0

    Kp_y = 0.3
    Ki_y = 0
    Kd_y = 0
    ######

    #사람 있는 곳에 바운딩 박스 그리는 함수
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

    #특정 수치 이상의 사람을 인식하고 바운딩 박스 그리기
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

    #마우스 클릭해서 트래킹 하는 함수인데 멀티 프로세스에서 안됌, 상황실 UI랑 통신해서 하면 되니까 걱정하지 않기
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

    #Tensorflow GPU로 돌리기 위한 설정
    gpus = tf.config.list_physical_devices('GPU')
    if gpus:
      # 텐서플로가 첫 번째 GPU만 사용하도록 제한
      try:
        tf.config.set_visible_devices(gpus[0], 'GPU')
      except RuntimeError as e:
        # 프로그램 시작시에 접근 가능한 장치가 설정되어야만 합니다
        print(e)

    #카메라 캘리브레이션 정보 읽기
    with open("RGB_calibration.pkl", "rb") as f:
        RGB_cameraMatrix, RGB_dist = pickle.load(f)

    #창 띄우기
    cv2.namedWindow("Movenet Multipose")
    cv2.setMouseCallback("Movenet Multipose", mouse_callback)

    #카메라 열려 있어야 프로세스 동작
    while cap1.isOpened():
        #프레임 읽기
        success1, frame1 = cap1.read()
    
        #프레임 읽기 성공하면
        if success1:
            #캘리브레이션 데이터 받아서 왜곡 펴기
            frame1 = cv.undistort(frame1, RGB_cameraMatrix, RGB_dist, None)
            
            #TOF 거리 데이터 저장 지역 변수
            dist = target_distance_.value
            
            #화각, 거리로 조준선 보정 값 계산
            # 한 픽셀 당 거리 계산
            pixel_distance = math.tan(RGB_CAM_FoV) * dist
            pixel_distance = pixel_distance / (RGB_Cam_Res_X / 2)
            if pixel_distance != 0:
                RGB_Cam_aim_X = int(5.5 / pixel_distance)

            #조준선 그리기
            cv2.line(frame1, (int(RGB_Cam_Res_X / 2) - 1 - (aim_lenth + 5) + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1), (int(RGB_Cam_Res_X / 2) - 1  - 5 + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1), (0, 0, 255), 2)
            cv2.line(frame1, (int(RGB_Cam_Res_X / 2) - 1 + 5 + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1), (int(RGB_Cam_Res_X / 2) - 1  + (aim_lenth + 5) + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1), (0, 0, 255), 2)
            
            cv2.line(frame1, (int(RGB_Cam_Res_X / 2) - 1  + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1 - (aim_lenth + 5)), (int(RGB_Cam_Res_X / 2) - 1 + RGB_Cam_aim_X + RGB_Cam_tilt_offset , int(RGB_Cam_Res_Y / 2) - 1 - 5), (0, 0, 255), 2)
            cv2.line(frame1, (int(RGB_Cam_Res_X / 2) - 1  + RGB_Cam_aim_X + RGB_Cam_tilt_offset, int(RGB_Cam_Res_Y / 2) - 1 + 5), (int(RGB_Cam_Res_X / 2) - 1 + RGB_Cam_aim_X + RGB_Cam_tilt_offset , int(RGB_Cam_Res_Y / 2) - 1 + (aim_lenth + 5)), (0, 0, 255), 2)

            #마우스 오른쪽 클릭 시
            if C_LR_.value == 2:
                tracker = None
                C_LR_.value = 3
                C_X_.value = 0
                C_Y_.value = 0

            #사람 트래킹 중이라면 Tensorflow(사람 인식) 끄기
            if tracker is None:
                img = frame1.copy()
                img = tf.image.resize_with_pad(tf.expand_dims(img, axis=0), 480, 640)
                input_img = tf.cast(img, dtype=tf.int32)
         
                #tensorflow 함수
                results = movenet(input_img)
         
                keypoints_with_scores = results['output_0'].numpy()[:,:,:51].reshape((6,17,3))
       
                rectangles_info = loop_through_people(frame1, keypoints_with_scores, 0.5)
            
                #마우스 왼 쪽 클릭 시 트래커 생성
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

                #사람 추적이 아니라 인식 때에는 모터 동작 X
                RGB_Com_X_.value = int(0)
                RGB_Com_Y_.value = int(0)
                
                PID_previous_time = time.time()
            #사람 추적이 생성되면
            elif tracker is not None:
                #트래커 업데이트
                ret, bbox = tracker.update(frame1)
                #성공하면 PID 제어기 거쳐서 명령값 생성
                if ret:
                    x, y, w, h = [int(v) for v in bbox]
                    cv2.rectangle(frame1, (x, y), (x + w, y + h), (0, 255, 0), 2)
                    if (Button_Command_.value & 0x10) == 0x10:
                        #RGB_Com_X_.value = int((((x + x + w) / 2) - (320 + RGB_Cam_aim_X)) / 5)
                        #RGB_Com_Y_.value = int((240 - ((y + y + h) / 2)) / 5)
                        px = int((((x + x + w) / 2) - (320 + RGB_Cam_aim_X)))
                        py = int((240 - ((y + y + h) / 2)))

                        ##PID 제어기##
                        PID_time = time.time() - PID_previous_time
                        PID_previous_time = time.time()

                        errorx = px - previous_Speedx
                        
                        PControlx = Kp * errorx
                        IControlx += Ki * errorx * PID_time
                        if IControlx > 3 or IControlx < -3:
                            IControlx = 0
                        DControlx = Kd * (errorx - previous_errorx) * PID_time
                        
                        PIDControlx = PControlx + IControlx + DControlx
                        
                        previous_Speedx = PIDControlx
                        previous_errorx = errorx
                        
                        errory = py - previous_Speedy
                        
                        PControly = Kp_y * errory 
                        IControly += Ki_y * errory * PID_time
                        if IControly > 3 or IControly < -3:
                            IControly = 0
                        DControly = Kd_y * (errory - previous_errory) * PID_time
                        
                        PIDControly = PControly + IControly + DControly
                        
                        previous_Speedy = PIDControly
                        previous_errory = errory

                        #목표와의 픽셀 단위 에러값으로 모터 구동
                        RGB_Com_X_.value = int(PIDControlx)
                        RGB_Com_Y_.value = int(PIDControly)
                # 트래킹 실패 시 트래커 리셋
                else:
                    #트래킹 실패 시 속도 0, 0으로 멈춤
                    RGB_Com_X_.value = int(0)
                    RGB_Com_Y_.value = int(0)
                    tracker = None
    
            # 영상 출력
            cv2.imshow('Movenet Multipose', frame1)

            #UDP 통신으로 영상 송신하기 위한 버퍼
            retval, buffer = cv2.imencode(".jpg", frame1)
            #영상 송신
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
        
        #어차피 RGB 카메라 fps 60이라 한 프레임 읽어올 때 빨라도 15ms임
        if cv2.waitKey(15) & 0xFF == ord('q'):
            break

    cap1.release()
    cv2.destroyAllWindows()
################################################################################


################################################################################
################################IR 카메라 프로세스###############################
def IR_pro(IR_Com_X_, IR_Com_Y_, target_distance_, Button_Command_):
    #IR 카메라 캘리브레이션 데이터 불러오기
    with open("IR_calibration.pkl", "rb") as f:
        IR_cameraMatrix, IR_dist = pickle.load(f)
        
    #IR 카메라 해상도
    IR_Cam_Res_X = 640
    IR_Cam_Res_Y = 480

    #IR 카메라 화각 라디안 값으로 변환
    IR_CAM_FoV = -math.pi * (64.6 / 2 / 180)
    
    #IR 카메라 틀어진 정도 보정하기 위한 파라미터
    IR_Cam_tilt = math.pi * (0 / 180)
    IR_Cam_tilt_offset = int(-((0 / 6.75) * 320))
    tof_tilt = math.pi * (0 / 180)

    #조준선 길이
    aim_lenth = 10

    #보정된 값(픽셀 단위)
    IR_Cam_aim_X = 0
    IR_Cam_aim_Y = 0

    #IR 카메라 열기
    cap2 = cv2.VideoCapture(2)
    cap2.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    cap2.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
    cap2.set(cv2.CAP_PROP_FPS, 30)

    #블러치리할 영역(), 나눌 숫자
    kernel = np.ones((3, 3))/3**2

    #침식 범위
    array = np.array([[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1]])

    ###PID 제어기 파라미터###
    L_previous_Speedx = 0
    L_previous_Speedy = 0

    L_errorx = 0
    L_errory = 0

    L_previous_errorx = 0
    L_previous_errory = 0

    L_PID_time = 0
    L_PID_previous_time = 0

    L_PIDControlx = 0
    L_PControlx = 0
    L_IControlx = 0
    L_DControlx = 0

    L_PIDControly = 0
    L_PControly = 0
    L_IControly = 0
    L_DControly = 0

    L_Kp = 0.3
    L_Ki = 0
    L_Kd = 0

    L_y_Kp = 0.3
    L_y_Ki = 0
    L_y_Kd = 0
    ######

    #IR 카메라 연결되어 있을 때
    while cap2.isOpened():
        #한 프레임 읽기
        success2, frame2 = cap2.read()
        if success2:
            #캘리브레이션 데이터로 왜곡 펴기
            frame2 = cv.undistort(frame2, IR_cameraMatrix, IR_dist, None)
    
            #흑백 변환
            frame2_gray = cv2.cvtColor(frame2, cv2.COLOR_BGR2GRAY)

            #블러처리
            blured = cv2.filter2D(frame2_gray, -1, kernel)

            #이진화
            ret_set_binary, set_binary = cv2.threshold(blured, 200, 255, cv2.THRESH_BINARY)
        
            erode = cv2.erode(set_binary, array) #침식
            dilate = cv2.dilate(erode, array) #팽창
        
            white_pixel_coordinates = np.column_stack(np.nonzero(dilate))
            
            dist = target_distance_.value

            #화각, 거리로 조준선 보정 정도 계산
            pixel_distance = math.tan(IR_CAM_FoV) * dist
            pixel_distance = pixel_distance / (IR_Cam_Res_X / 2)
            if pixel_distance != 0:
                IR_Cam_aim_X = int(5 / pixel_distance)

            if len(white_pixel_coordinates) > 0:
                center = np.mean(white_pixel_coordinates, axis=0)
                center = tuple(map(int, center))  # 정수형으로 변환
                if (Button_Command_.value & 0x20) == 0x20:
                    #IR_Com_Y_.value = int(240 - center[0])
                    #IR_Com_X_.value = int(center[1] - (320 + IR_Cam_aim_X))
                    
                    ##PID 제어기##
                    L_PID_time = time.time() - L_PID_previous_time
                    L_PID_previous_time = time.time()
                    
                    x = int(center[1] - (320 + IR_Cam_aim_X))
                    y = int(240 - center[0])

                    L_errorx = x - L_previous_Speedx
                        
                    L_PControlx = L_Kp * L_errorx
                    L_IControlx += L_Ki * L_errorx * L_PID_time
                    if L_IControlx > 0.1 or L_IControlx < -0.1:
                        L_IControlx = 0
                    L_DControlx = L_Kd * (L_errorx - L_previous_errorx) * L_PID_time
                    
                    L_PIDControlx = L_PControlx + L_IControlx + L_DControlx
                      
                    L_previous_Speedx = L_PIDControlx
                    L_previous_errorx = L_errorx
                       
                    L_errory = y - L_previous_Speedy
                       
                    L_PControly = L_y_Kp * L_errory
                    L_IControly += L_y_Ki * L_errory * L_PID_time
                    if L_IControly > 0.1 or L_IControly < -0.1:
                        L_IControly = 0
                    L_DControly = L_y_Kd * (L_errory - L_previous_errory) * L_PID_time
                        
                    L_PIDControly = L_PControly + L_IControly + L_DControly
                        
                    L_previous_Speedy = L_PIDControly
                    L_previous_errory = L_errory

                    #레이저와의 픽셀 단위 에러값 모터 구동 데이터 저장
                    IR_Com_X_.value = int(L_PIDControlx)
                    IR_Com_Y_.value = int(L_PIDControly)
            else:
                #레이저 놓치면 속도 0, 0으로
                IR_Com_X_.value = int(0)
                IR_Com_Y_.value = int(0)
                L_PID_previous_time = time.time()

            #조준선 그리기 위해 컬러로 다시 변환 지워도 됌 테스트 용
            dilate = cv2.cvtColor(dilate, cv2.COLOR_GRAY2BGR)
            #조준선 그리기 지워도 됌 테스트 용
            cv2.line(dilate, (int(IR_Cam_Res_X / 2) - 1 - aim_lenth + IR_Cam_aim_X + IR_Cam_tilt_offset, int(IR_Cam_Res_Y / 2) - 1), (int(IR_Cam_Res_X / 2) - 1  + aim_lenth + IR_Cam_aim_X + IR_Cam_tilt_offset, int(IR_Cam_Res_Y / 2) - 1), (0, 0, 255), 2)
            cv2.line(dilate, (int(IR_Cam_Res_X / 2) - 1  + IR_Cam_aim_X + IR_Cam_tilt_offset, int(IR_Cam_Res_Y / 2) - 1 - aim_lenth), (int(IR_Cam_Res_X / 2) - 1 + IR_Cam_aim_X + IR_Cam_tilt_offset , int(IR_Cam_Res_Y / 2) - 1 + aim_lenth), (0, 0, 255), 2)

            #필터링한 결과 영상 출력
            cv2.imshow('Camera Window1', dilate)

        #어차피 IR 카메라 FPS 30이어서 한 프레임 읽는데 30ms 넘게 걸림 waitkey 30
        if cv2.waitKey(30) & 0xFF == ord('q'):
            break
 
    cap2.release()
    cv2.destroyAllWindows()
################################################################################
    
    
################################################################################
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

#CAN_ID 3번 모터 구동 변수, 함수#
#시동
start_up_3 = 0

#가속도
ACC3 = 210

#이전 스피드 기억하기
pre_3_Speed = 0

#명령 내린 방향 기억하기
M3_state_dir = 0

#모터 드라이버에서 송신한 모터 구동에 대한 데이터
M3_Stop = b'\x03\x03\xf6\x02\xfb'
M3_Move = b'\x03\x03\xf6\x01\xfa'
M3_state = b'\x03\x03\xf6\x02\xfb'
M3_error = b'\x03\x03\xf6\x00\xf9'

#데이터 패킷
CRCBYTE3 = (0x3 + 0xF6 + ACC3) & 0xFF
packet3 = [3, 5, 0xF6, 0, 0, ACC3, CRCBYTE3]

#CAN_ID 3번 모터 구동 변수, 함수#
#시동
start_up_4 = 0

#가속도
ACC4 = 100

#이전 스피드 기억하기
pre_4_Speed = 0

#명령 내린 방향 기억하기
M4_state_dir = 0

#모터 드라이버에서 송신한 모터 구동에 대한 데이터
M4_Stop = b'\x04\x03\xf6\x02\xfc'
M4_Move = b'\x04\x03\xf6\x01\xfb'
M4_state = b'\x04\x03\xf6\x02\xfc'
M4_error = b'\x04\x03\xf6\x00\xfa'

#데이터 패킷
CRCBYTE4 = (0x4 + 0xF6 + ACC4) & 0xFF
packet4 = [4, 5, 0xF6, 0, 0, ACC4, CRCBYTE4]

#CAN_ID 5번 모터 구동 변수, 함수#
#시동
start_up_5 = 0

#가속도
ACC5 = 100

#이전 스피드 기억하기
pre_5_Speed = 0

#명령 내린 방향 기억하기
M5_state_dir = 0

#모터 드라이버에서 송신한 모터 구동에 대한 데이터
M5_Stop = b'\x05\x03\xf6\x02\xfd'
M5_Move = b'\x05\x03\xf6\x01\xfc'
M5_state = b'\x05\x03\xf6\x02\xfd'
M5_error = b'\x05\x03\xf6\x00\xfb'

#데이터 패킷
CRCBYTE5 = (0x5 + 0xF6 + ACC5) & 0xFF
packet5 = [5, 5, 0xF6, 0, 0, ACC5, CRCBYTE5]

#조이스틱 데이터에 대한 클래스
class joy_data:
    axisX:int = 0
    axisY:int = 0
    axisT:int = 0
    button:int = 0xfc000004
joy = joy_data()

#메인틴지 연결
try:
    teensy = serial.Serial('/dev/ttyACM0', 500000, timeout = 0)
except IOError as e:
    print(e)
    print("Teensy 어디")
    exit()
    
#광학부 클래스
optical = Optical()

#엔코더값 0으로 초기화 함수
def set_current_axis_to_zero(canid):
    byte1 = 0x92
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))

#Degrees to Pulses
def angle_to_pulses(angle):
    pulses = int(angle / 0.0003515625)
    pulses = abs(pulses)
    return pulses

#호밍?
def go_home(canid):

    byte1 = 0x91

    crcbyte = (canid + byte1) & 0xFF

    packet = [canid, 2, byte1, crcbyte]

    teensy.write(bytearray(packet))

#펄스로 모터 제어
def position_control(canid, dir, speed, acc,  pulses):

    upper_speed = (speed >> 8) & 0x0F

    lower_speed = speed & 0xFF

    byte1 = 0xFD

    byte2 = (dir << 7) | upper_speed

    byte3 = lower_speed

    byte4 = acc

    byte5 = (pulses >> 16) & 0xFF

    byte6 = (pulses >> 8) & 0xFF

    byte7 = pulses & 0xFF

    byte8 = (canid + 0xFD + byte2 + byte3 + byte4 + byte5 + byte6 +byte7) & 0xFF

    packet = [canid, 8, byte1, byte2, byte3, byte4, byte5, byte6, byte7, byte8]

    teensy.write(bytearray(packet))

#엔코더 데이터 보내라는 명령어
def read_encoder2(canid):
    byte1 = 0x31
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))

#기어비, 엔코더 데이터로 모터 각도 계산     
def calculate_encoder(byttee, Gear_ratio):
    if byttee[1] == 0:
        ee = byttee[6] + (byttee[5] << 8)+ (byttee[4] << 16)+ (byttee[3] << 24)+ (byttee[2] << 32)
        ee = (ee * 360) / ((2 ** 14) * Gear_ratio)
        return ee
    elif byttee[1] == 255:
        ee = -((255 - byttee[6]) + ((255 - byttee[5]) << 8) + ((255 - byttee[4]) << 16) + ((255 - byttee[3]) << 24) + ((255 - byttee[2]) << 32))
        ee = (ee * 360) / ((2 ** 14) * Gear_ratio)
        return ee

#TOF 데이터 보내라는 명령어
def read_dist(canid):
    packet = optical.PollingDistance()
    teensy.write(packet)
    
#3번 모터 속도 0으로 패킷 만들기
def Go_to_Speed_Zero_3():
    global ACC3, CRCBYTE3, packet3
    CRCBYTE3 = (0x3 + 0xF6 + ACC3) & 0xFF
    packet3 = [3, 5, 0xF6, 0, 0, ACC3, CRCBYTE3]
    
#3번 모터 특정 속도로 패킷 만들기
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

#가감속 이상하게 안되게 만든 모터 구동 함수
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
            
       
#4번 모터 속도 0으로 패킷 만들기
def Go_to_Speed_Zero_4():
    global ACC4, CRCBYTE4, packet4
    CRCBYTE4 = (0x4 + 0xF6 + ACC4) & 0xFF
    packet4 = [4, 5, 0xF6, 0, 0, ACC4, CRCBYTE4]
    
#4번 모터 특정 속도로 패킷 만들기
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

#가감속 이상하게 안되게 만든 모터 구동 함수
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
        
#5번 모터 속도 0으로 패킷 만들기
def Go_to_Speed_Zero_5():
    global ACC5, CRCBYTE5, packet5
    CRCBYTE5 = (0x5 + 0xF6 + ACC5) & 0xFF
    packet5 = [5, 5, 0xF6, 0, 0, ACC5, CRCBYTE5]
    
#4번 모터 특정 속도로 패킷 만들기
def Go_to_target_Speed_5(Speed):
    global ACC5, CRCBYTE5, packet5
    if Speed > 0:
        DIR = 0
        upper_speed = (Speed >> 8) & 0x0F
        lower_speed = Speed & 0xFF
        BYTE2 = (DIR << 7) | upper_speed
        CRCBYTE5 = (0x5 + 0xF6 + BYTE2 + lower_speed + ACC5) & 0xFF
        packet5 = [5, 5, 0xF6, BYTE2, lower_speed, ACC5, CRCBYTE5]
    elif Speed < 0:
        DIR = 1
        upper_speed = (abs(Speed) >> 8) & 0x0F
        lower_speed = abs(Speed) & 0xFF
        BYTE2 = (DIR << 7) | upper_speed
        CRCBYTE5 = (0x5 + 0xF6 + BYTE2 + lower_speed + ACC5) & 0xFF
        packet5 = [5, 5, 0xF6, BYTE2, lower_speed, ACC5, CRCBYTE5]

#그냥 속도 데이터로 동작 확인용 함수
def Active_Motor_5(Angle):
    global Gun_angle
    Upper_Angle_Limit = 40
    Under_Angle_Limit = -40
    Error_Pulses = 0
    Dir = 0
    if Gun_angle >= Angle:
        if Gun_angle + Angle < Under_Angle_Limit:
            Dir = 0
            Error_Angle = Under_Angle_Limit - Gun_angle
            Error_Pulses = angle_to_pulses(Error_Angle)
            print("Angle Limit1")
        else:
            Dir = 0
            Error_Angle = Gun_angle - Angle
            Error_Pulses = angle_to_pulses(Error_Angle)
    elif Gun_angle < Angle:
        if Gun_angle + Angle > Upper_Angle_Limit:
            Dir = 1
            Error_Angle = Upper_Angle_Limit - Gun_angle
            Error_Pulses = angle_to_pulses(Error_Angle)
            print("Angle Limit2")
        else:
            Dir = 1
            Error_Angle = Angle - Gun_angle
            Error_Pulses = angle_to_pulses(Error_Angle)
    position_control(5, Dir, 200, 150,  Error_Pulses)

#Degrees 각도, 카메라 정보, TOF 센서 데이터로 기총부 도착해야 할 각도 계산
def Cal_Anlge_Aming_Target(CAM_data, distance_tof, Optic_Angle):
    Offset_of_tof_OpticalCenter = 0

    #쌩으로 들어온 TOF 센서 데이터
    Initial_dist_of_optical = 0

    #RCWS 몸체 크기에 따른 X, Y 거리값 (cm)
    RCWS_X_lenth = 25
    RCWS_Y_lenth = 34.5

    if CAM_data == "RGB":
        #RGB 카메라면 CCD 센서 위치에 따른 거리 추가
        Initial_dist_of_optical = 3.55
    elif CAM_data == "IR":
        #IR 카메라면 CCD 센서 위치에 따른 거리 추가
        Initial_dist_of_optical = -3.72
        
    #광학부에서부터 최종적인 타겟까지의 거리
    dist_of_target = distance_tof + Initial_dist_of_optical + Offset_of_tof_OpticalCenter
    
    #광학부 각도 라디안값으로 변환
    Radian_Optic_Angle = math.radians(Optic_Angle)
    
    #광학부 라디안 값으로 광학부에서 부터 타겟까지 X, Y 거리 구하기
    X_target_dist_from_Opt = dist_of_target * math.cos(Radian_Optic_Angle)
    Y_target_dist_from_Opt = dist_of_target * math.sin(Radian_Optic_Angle)
        
    #기총부기준 타겟의 X, Y 거리 구하기
    X_target_dist_from_gun = RCWS_X_lenth + X_target_dist_from_Opt
    Y_target_dist_from_gun = RCWS_Y_lenth - Y_target_dist_from_Opt
    
    #degrees각도로 반환
    return -math.degrees(math.atan2(Y_target_dist_from_gun, X_target_dist_from_gun))    

#조이스틱 이벤트 함수
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
                if (joy.button & 0x20) == 0x20:
                    Gun_M_Angle = Cal_Anlge_Aming_Target("IR", target_distance.value, Optical_angle)
                    Active_Motor_5(Gun_M_Angle)
                else:
                    Gun_M_Angle = Cal_Anlge_Aming_Target("RGB", target_distance.value, Optical_angle)
                    Active_Motor_5(Gun_M_Angle)
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
            elif event.button == 7:
                Go_to_target_Speed_5(25)
                teensy.write(bytearray(packet5))
            elif event.button == 9:
                Go_to_Speed_Zero_5()
                teensy.write(bytearray(packet5))
            elif event.button == 11:
                Go_to_target_Speed_5(-25)
                teensy.write(bytearray(packet5))
            Button_Command.value = joy.button
        elif event.type == pygame.JOYBUTTONUP:
            if event.button == 0:
                print("Fire_Off")
            elif event.button == 1:
                print("release_Target")
            Button_Command.value = joy.button
            
#조이스틱 초기화 함수
gamepad = init_gamepad()
################################################################################
#엔코더값으로 각도 환산한 광학부, Z축, 기총부 각도 저장 전역변수
Optical_angle = 0
Body_angle = 0
Gun_angle = 0

#TCP/IP로 각도 값 전해줄 때 변할 때만 보내주게 이전 각도 저장 전역 변수
pre_Optical_angle = 0
pre_Body_angle = 0
    
################################################################################
#TCP/IP 설정
TCP_host_IP = "10.254.1.20"
TCP_host_port = 7000

TCP = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
TCP.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 0)

TCP.bind((TCP_host_IP, TCP_host_port))

TCP.listen(2)

#TCP/IP로 조이스틱 데이터 수신(상황실 Python과 통신)
def Get_Joystick(client_socket, addr):
    global joy, Button_Command
    while 1:    
        data = client_socket.recv(16)
        if len(data) == 16:
            ds = struct.unpack('3iI', data)
            if ds[3] & 0xfc000000 == 4227858432:
                joy.axisX = ds[0]
                joy.axisY = ds[1]
                joy.axisT = ds[2]
                joy.button = ds[3]
                Button_Command.value = joy.button
        
#TCP/IP로 클릭 데이터 수신(상황실 C#과 통신)
def Get_Clickdata(client_socket, addr):
    global Click, Optical_angle, pre_Optical_angle, Body_angle, pre_Body_angle
    while 1:
        data = client_socket.recv(12)
        ds = struct.unpack('3i', data)
        
        if ds[0] == 1 or ds[0] == 2:
            C_LR.value = ds[0]
            C_X.value = ds[1]
            C_Y.value = ds[2]
            
        #이전 각도와 다른 각도로 변하면 Z축, 광학부 각도 전송
        if pre_Optical_angle != Optical_angle or pre_Body_angle != Body_angle:
            sd = struct.pack('3iI', int(Optical_angle), int(Body_angle), joy.axisT, Click.Left_or_right)
            client_socket.sendall(sd)
            pre_Optical_angle = Optical_angle
            pre_Body_angle = Body_angle
            
        time.sleep(0.001)
        
################################################################################


################################################################################
#메인
if __name__ == '__main__':
    #프로세스 설정
    p1 = Process(target=RGB_pro, args=(C_LR, C_X, C_Y, RGB_Com_X, RGB_Com_Y, target_distance, Button_Command,))
    p2 = Process(target=IR_pro, args=(IR_Com_X, IR_Com_Y, target_distance, Button_Command,))
    
    #프로세스 시작
    p1.start()
    p2.start()
    
    #엔코더, TOF 값들을 보내라는 명령을 보내는데 한 번에 받아서 데이터 꼬이지 않게 시퀀스 만들기 위한 변수
    get_data_pre_time = 0
    get_data_seq = 0
    
    #최종적인 모터를 구동하는 명령 변수
    Active_M3_command = 0
    Active_M4_command = 0
    
    #조이스틱 연결이 안되어 있으면 TCP/IP 연결
    #조이스틱 연결이 되어 있으면 TCP/IP 연결 안함
    if gamepad is None:
        client_socket1, addr1 = TCP.accept()
        print("success1")
        client_socket2, addr2 = TCP.accept()
        print("success2")
    
    
        t1= threading.Thread(target=Get_Clickdata, args=(client_socket1, addr1))

        t1.daemon = True

        t1.start() 
    
        t2 = threading.Thread(target=Get_Joystick, args=(client_socket2, addr2))

        t2.daemon = True

        t2.start() 
        
    #초기에 캘리하기 위해 포토 센서로 이동
    go_home(4)
    go_home(5)
    
    #포토 센서로 이동할 때까지 기다리는건데 이거 나중에 처리하자
    time.sleep(25)

    #캘리브레이션
    position_control(4, 1, 50, 100, 8062)
    position_control(5, 1, 50, 100, 8190)

    #캘리 다 될 때까지 기다리는건데 이거 나중에 처리하자
    time.sleep(10)
    
    #캘리된 지점에서 엔코더 값 0으로 초기화 해주는 함수
    set_current_axis_to_zero(4)
    set_current_axis_to_zero(5)

    time.sleep(1)
    
    #메인 틴지 통신 처리
    while 1:
        # 엔코더값, TOF 데이터 보내라는 명령어를 꼬이지 않게 순차적, 주기적으로 보냄
        if time.time() - get_data_pre_time > 0.004:
            if get_data_seq == 0:
                read_encoder2(3)
                get_data_seq = 1
            elif get_data_seq == 1:
                read_encoder2(4)
                get_data_seq = 2
            elif get_data_seq == 2:
                read_encoder2(5)
                get_data_seq = 3
            elif get_data_seq == 3:
                read_dist(6)
                get_data_seq = 0
            get_data_pre_time = time.time()
            
        #게임패드가 연결되어 있으면
        if gamepad:
            get_gamepad_input(gamepad)
            
        #게임패드 버튼에 따른 모드 구분
        if (joy.button & 0x04) == 0x04 or (joy.button & 0x08) == 0x08:
            Active_M3_command = joy.axisX
            Active_M4_command = joy.axisY
        elif (joy.button & 0x10) == 0x10:
            Active_M3_command = RGB_Com_X.value
            Active_M4_command = RGB_Com_Y.value
        elif (joy.button & 0x20) == 0x20:
            Active_M3_command = IR_Com_X.value
            Active_M4_command = IR_Com_Y.value

        #버튼 누르면 기총부 조준(aming)
        if (joy.button & 0x02) == 0x02:
            if (joy.button & 0x20) == 0x20:
                Gun_M_Angle = Cal_Anlge_Aming_Target("IR", target_distance.value, Optical_angle)
                Active_Motor_5(Gun_M_Angle)
            else:
                Gun_M_Angle = Cal_Anlge_Aming_Target("RGB", target_distance.value, Optical_angle)
                Active_Motor_5(Gun_M_Angle)
            
        #일단 2바이트 수신(CAN_ID, Length)
        data1 = teensy.read(2)
        if len(data1) > 0:
            #data length만큼 데이터 더 수신
            data2 = teensy.read(data1[1])
            #print(data2)
            ##To control Motor data
            #ID 3번
            if data1[0] == 3:
                #모터 구동을 위한 데이터 처리
                if data2 == b'\xf6\x02\xfb' or data2 == b'\xf6\x00\xf9':
                    M3_state = M3_Stop
                    M3_state_dir = 0
                #엔코더 데이터 처리
                elif data1[1] == 8:
                    Body_angle = -calculate_encoder(data2, 43.33333)
            #ID 4번
            elif data1[0] == 4:
                #모터 구동을 위한 데이터 처리
                if data2 == b'\xf6\x02\xfc' or data2 == b'\xf6\x00\xfa':
                    M4_state = M4_Stop
                    M4_state_dir = 0
                #엔코더 데이터 처리
                elif data1[1] == 8:
                    Optical_angle = -calculate_encoder(data2, 10)
            #ID 5번
            elif data1[0] == 5:
                #엔코더 데이터 처리
                if data1[1] == 8:
                    Gun_angle = calculate_encoder(data2, 20)
            ##To get ToF##
            #ID 6번
            elif data1[0] == 6:
                optical.CheckPacket(data2)
                target_distance.value = optical.distance
                
        #Z축 각도 -55 ~ 55 벗어나면 멈추기 시작
        if Body_angle < -55 and Active_M3_command > 0:
            Active_Motor_3(0)
        elif Body_angle > 55 and Active_M3_command < 0:
            Active_Motor_3(0)
        #안벗어 났으면 명령대로 모터 구동
        else:
            Active_Motor_3(Active_M3_command)
            
        #광학부 각도 -30 ~ 30 벗어나면 멈추기 시작
        if Optical_angle < -30 and Active_M4_command > 0:
            Active_Motor_4(0)
        elif Optical_angle > 30 and Active_M4_command < 0:
            Active_Motor_4(0)
        #안벗어 났으면 명령대로 모터 구동
        else:
            Active_Motor_4(Active_M4_command)
            
    
# device.close() 
