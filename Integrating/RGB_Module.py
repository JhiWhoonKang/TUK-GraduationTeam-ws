import tensorflow as tf
import tensorflow_hub as hub
import tensorflow_hub as keras
import cv2 as cv
import cv2,sys,os
import numpy as np
import pickle
import time
import socket
import math
import struct

def RGB_pro(C_LR_, C_X_, C_Y_, RGB_Com_X_, RGB_Com_Y_, target_distance_, Button_Command_, request_access_,aming_complete_, person_detected_, Optical_angle_, Body_angle_, Gun_angle_, Mode__, Zoom_, C_D_X_1_, C_D_Y_1_, C_D_X_2_, C_D_Y_2_):
    global tracker
    #영상 송신 UDP 통신 서버, 클라이언트 주소 지정
    max_length = 65000
    #UDP_host_IP = "10.254.2.96"
    UDP_host_IP = "192.168.0.30"
    UDP_host_port = 9000
    #UDP_Client_IP = "10.254.1.3"
    UDP_Client_IP = "192.168.0.52"
    UDP_Client_port = 8000

    #UDP 통신 환경 설정
    UDP = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    UDP.bind((UDP_host_IP, UDP_host_port))
    
    #RGB 카메라 해상도 640X480
    RGB_Cam_Index = 2
    RGB_Cam_Res_X = 640
    RGB_Cam_Res_Y = 480
    RGB_Cam_FPS = 60

    #RGB 카메라 화각 라디안각으로 변환
    #뒤에 숫자는 줌, 포커스 인덱스
    RGB_Cam_X_FoV_0 = 13.984#14.112
    RGB_Cam_X_FoV_1 = 13.774#13.309
    RGB_Cam_X_FoV_2 = 10.428#10.392
    RGB_Cam_X_FoV_3 = 7.317#7.218
    RGB_Cam_X_FoV_4 = 7.383#7.722

    RGB_Cam_Y_FoV_0 = RGB_Cam_X_FoV_0 / 4.0 * 3.0
    RGB_Cam_Y_FoV_1 = RGB_Cam_X_FoV_1 / 4.0 * 3.0
    RGB_Cam_Y_FoV_2 = RGB_Cam_X_FoV_2 / 4.0 * 3.0
    RGB_Cam_Y_FoV_3 = RGB_Cam_X_FoV_3 / 4.0 * 3.0
    RGB_Cam_Y_FoV_4 = RGB_Cam_X_FoV_4 / 4.0 * 3.0

    Distance_with_RGB_TOF = 5.5
    
    #하드웨어적으로 틀어져 있는 정도를 보정하기 위한 변수들
    RGB_x_offset = 0
    RGB_y_offset = 0

    RGB_X_Tilt_Angle = 0.1#0.2#-0.75
    RGB_X_Tilt_Angle = math.pi * (RGB_X_Tilt_Angle / 180)

    RGB_Y_Tilt_Angle = -0.55#0.4
    RGB_Y_Tilt_Angle = math.pi * (RGB_Y_Tilt_Angle / 180)

    #화면에 출력할 조준선 길이
    aim_lenth_X = 130
    aim_lenth_Y = 70

    #조준선 보정값 (픽셀 단위)
    RGB_Cam_aim_X = 0
    RGB_Cam_aim_Y = 0

    #조준선 움직이는 함수
    def Move_Aim(fr, RGB_Color, thickness, M):
        #최종 조준선 좌표 위치
        global RGB_Cam_aim_X, RGB_Cam_aim_Y

        #목표 대상 거리값
        dist = target_distance_.value
        
        #화각 저장 변수
        RGB_Cam_X_FoV = 0
        RGB_Cam_Y_FoV = 0

        #줌 별로 화각 계산
        if Zoom_.value == 0:
            RGB_Cam_X_FoV = math.pi * (RGB_Cam_X_FoV_0 / 2 / 180)
            RGB_Cam_Y_FoV = math.pi * (RGB_Cam_Y_FoV_0 / 2 / 180)
        elif Zoom_.value == 1:
            RGB_Cam_X_FoV = math.pi * (RGB_Cam_X_FoV_1 / 2 / 180)
            RGB_Cam_Y_FoV = math.pi * (RGB_Cam_Y_FoV_1 / 2 / 180)
        elif Zoom_.value == 2:
            RGB_Cam_X_FoV = math.pi * (RGB_Cam_X_FoV_2 / 2 / 180)
            RGB_Cam_Y_FoV = math.pi * (RGB_Cam_Y_FoV_2 / 2 / 180)
        elif Zoom_.value == 3:
            RGB_Cam_X_FoV = math.pi * (RGB_Cam_X_FoV_3 / 2 / 180)
            RGB_Cam_Y_FoV = math.pi * (RGB_Cam_Y_FoV_3 / 2 / 180)
        elif Zoom_.value == 4:
            RGB_Cam_X_FoV = math.pi * (RGB_Cam_X_FoV_4 / 2 / 180)
            RGB_Cam_Y_FoV = math.pi * (RGB_Cam_Y_FoV_4 / 2 / 180)

        #X축 픽셀당 거리값 계산
        PD_X = float(math.tan(RGB_Cam_X_FoV)) * float(dist)
        PD_X = float(PD_X) / (float(RGB_Cam_Res_X) / float(2))

        #Y축 픽셀당 거리값 계산
        PD_Y = float(math.tan(RGB_Cam_Y_FoV)) * float(dist)
        PD_Y = float(PD_Y) / (float(RGB_Cam_Res_Y) / float(2))
        
        #X축 픽셀당 거리값이 0이 아닐 때
        if PD_X != 0:
            #하드웨어적으로 틀어진 것 보정하기 위한 변수, 계산
            RGB_CAM_TILT_OFFSET_X = float(math.tan(RGB_X_Tilt_Angle)) * float(dist)
            RGB_CAM_TILT_OFFSET_X = float(RGB_CAM_TILT_OFFSET_X) / float(PD_X)

            #하드웨어적으로 틀어진 것 보정하기 위한 값이 0이 아닐 때
            if RGB_CAM_TILT_OFFSET_X != 0:
                #조준선 계산
                RGB_Cam_aim_X = int((RGB_Cam_Res_X / 2) - 1 + (float(Distance_with_RGB_TOF) / float(PD_X)) - RGB_CAM_TILT_OFFSET_X + RGB_x_offset)
            else:
                #조준선 계산
                RGB_Cam_aim_X = int((RGB_Cam_Res_X / 2) - 1 + (float(Distance_with_RGB_TOF) / float(PD_X)) + RGB_x_offset)
        elif PD_X == 0:
            #조준선 계산
            RGB_Cam_aim_X = int((RGB_Cam_Res_X / 2) - 1 + RGB_x_offset)

        #Y축 픽셀당 거리값이 0이 아닐 때
        if PD_Y != 0:
            #하드웨어적으로 틀어진 것 보정하기 위한 변수, 계산
            RGB_CAM_TILT_OFFSET_Y = float(math.tan(RGB_Y_Tilt_Angle)) * float(dist)
            RGB_CAM_TILT_OFFSET_Y = float(RGB_CAM_TILT_OFFSET_Y) / float(PD_Y)

            #하드웨어적으로 틀어진 것 보정하기 위한 값이 0이 아닐 때
            if RGB_CAM_TILT_OFFSET_Y != 0:
                #조준선 계산
                RGB_Cam_aim_Y = int((RGB_Cam_Res_Y / 2) - 1 + RGB_CAM_TILT_OFFSET_Y - RGB_y_offset)
            else:
                #조준선 계산
                RGB_Cam_aim_Y = int((RGB_Cam_Res_Y / 2) - 1 - RGB_y_offset)
        elif PD_Y == 0:
            #조준선 계산
            RGB_Cam_aim_Y = int((RGB_Cam_Res_Y / 2) - 1 - RGB_y_offset)

        #줌 했을 때 이미지 가운데가 움직이는거 보정하기 위한 계산
        if Zoom_.value == 0 or Zoom_.value == 1:
            RGB_Cam_aim_X += 6
            RGB_Cam_aim_Y += 7
        elif Zoom_.value == 2:
            RGB_Cam_aim_X += 4
            RGB_Cam_aim_Y += 5
      
        #이 밑에는 조준선 디자인
        x1 = RGB_Cam_aim_X - (aim_lenth_X + 5)
        x2 = RGB_Cam_aim_X - 10
        x3 = RGB_Cam_aim_X + 10
        x4 = RGB_Cam_aim_X + (aim_lenth_X + 5)

        y1 = RGB_Cam_aim_Y - (aim_lenth_Y + 5)
        y2 = RGB_Cam_aim_Y - 10
        y3 = RGB_Cam_aim_Y + 10
        y4 = RGB_Cam_aim_Y + (aim_lenth_Y + 5)

        cv2.line(fr, (RGB_Cam_aim_X - 1, RGB_Cam_aim_Y), (RGB_Cam_aim_X + 1, RGB_Cam_aim_Y), RGB_Color, 1)
        cv2.line(fr, (RGB_Cam_aim_X, RGB_Cam_aim_Y - 1), (RGB_Cam_aim_X, RGB_Cam_aim_Y + 1), RGB_Color, 1)

        cv2.line(fr, (x1, RGB_Cam_aim_Y), (x2, RGB_Cam_aim_Y), RGB_Color, thickness)
        cv2.line(fr, (x3, RGB_Cam_aim_Y), (x4, RGB_Cam_aim_Y), RGB_Color, thickness)

        cv2.line(fr, (RGB_Cam_aim_X, y1), (RGB_Cam_aim_X, y2), RGB_Color, thickness)
        cv2.line(fr, (RGB_Cam_aim_X, y3), (RGB_Cam_aim_X, y4), RGB_Color, thickness)

        cv2.line(fr, (x1, RGB_Cam_aim_Y - 10), (x1, RGB_Cam_aim_Y + 10), RGB_Color, thickness)
        cv2.putText(fr, str(round(float(RGB_Cam_aim_X - x1) * PD_X, 1)), (x1 - 13,RGB_Cam_aim_Y + 20), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        cv2.line(fr, (int(x1 + (x2 - x1) / 3), RGB_Cam_aim_Y - 10), (int(x1 + (x2 - x1) / 3), RGB_Cam_aim_Y + 10), RGB_Color, thickness)
        cv2.putText(fr, str(round((RGB_Cam_aim_X - (x1 + (x2 - x1) / 3)) * PD_X, 1)), (int((x1 + (x2 - x1) / 3) - 13),RGB_Cam_aim_Y + 20), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        cv2.line(fr, (int(x1 + (x2 - x1) * 2 / 3), RGB_Cam_aim_Y - 10), (int(x1 + (x2 - x1) * 2 / 3), RGB_Cam_aim_Y + 10), RGB_Color, thickness)
        cv2.putText(fr, str(round((RGB_Cam_aim_X - (x1 + (x2 - x1) * 2 / 3)) * PD_X, 1)), (int((x1 + (x2 - x1) * 2 / 3) - 13),RGB_Cam_aim_Y + 20), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
            
        cv2.line(fr, (x4, RGB_Cam_aim_Y - 10), (x4, RGB_Cam_aim_Y + 10), RGB_Color, thickness)
        cv2.putText(fr, str(round((x4 - RGB_Cam_aim_X) * PD_X, 1)), (x4 - 13,RGB_Cam_aim_Y + 20), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        cv2.line(fr, (int(x4 - ((x4 - x3) / 3)), RGB_Cam_aim_Y - 10), (int(x4 - ((x4 - x3) / 3)), RGB_Cam_aim_Y + 10), RGB_Color, thickness)
        cv2.putText(fr, str(-round((RGB_Cam_aim_X - (x4 - ((x4 - x3) / 3))) * PD_X, 1)), (int((x4 - ((x4 - x3) / 3)) - 13),RGB_Cam_aim_Y + 20), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        cv2.line(fr, (int(x4 - ((x4 - x3) * 2 / 3)), RGB_Cam_aim_Y - 10), (int(x4 - ((x4 - x3) * 2 / 3)), RGB_Cam_aim_Y + 10), RGB_Color, thickness)
        cv2.putText(fr, str(-round((RGB_Cam_aim_X - (x4 - ((x4 - x3) * 2 / 3))) * PD_X, 1)), (int((x4 - ((x4 - x3) * 2 / 3)) - 13),RGB_Cam_aim_Y + 20), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)

            
        cv2.line(fr, (RGB_Cam_aim_X - 7, y1), (RGB_Cam_aim_X + 7, y1), RGB_Color, thickness)
        if (RGB_Cam_aim_Y - y1) * PD_Y >= 10:
            cv2.putText(fr, str(round((RGB_Cam_aim_Y - y1) * PD_Y, 1)), (RGB_Cam_aim_X - 15, y1 - 5), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        else:
            cv2.putText(fr, str(round((RGB_Cam_aim_Y - y1) * PD_Y, 1)), (RGB_Cam_aim_X - 10, y1 - 5), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        cv2.line(fr, (RGB_Cam_aim_X - 7, int(y1 + (y2 - y1) / 2)), (RGB_Cam_aim_X + 7, int(y1 + (y2 - y1) / 2)), RGB_Color, thickness)
        cv2.putText(fr, str(round((RGB_Cam_aim_Y - (y1 + (y2 - y1) / 2)) * PD_Y, 1)), (RGB_Cam_aim_X + 10, int(y1 + (y2 - y1) / 2) + 5), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)

        cv2.line(fr, (RGB_Cam_aim_X - 7, y4), (RGB_Cam_aim_X + 7, y4), RGB_Color, thickness)
        if (RGB_Cam_aim_Y - y1) * PD_Y >= 10:
            cv2.putText(fr, str(round((y4 - RGB_Cam_aim_Y) * PD_Y, 1)), (RGB_Cam_aim_X - 15, y4 + 12), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        else:
            cv2.putText(fr, str(round((y4 - RGB_Cam_aim_Y) * PD_Y, 1)), (RGB_Cam_aim_X - 10, y4 + 12), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        cv2.line(fr, (RGB_Cam_aim_X - 7, int(y4 - (y4 - y3) / 2)), (RGB_Cam_aim_X + 7, int(y4 - (y4 - y3) / 2)), RGB_Color, thickness)
        cv2.putText(fr, str(round(((y4 - (y4 - y3) / 2) - RGB_Cam_aim_Y) * PD_Y, 1)), (RGB_Cam_aim_X + 10, int(y4 - (y4 - y3) / 2) + 5), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)

        cv2.putText(fr, str("Dist ") + str(round(target_distance_.value * 0.01, 1)), (x4 - 50, y1), cv2.FONT_HERSHEY_SIMPLEX, 0.4, RGB_Color, 1)
        if aming_complete_.value == 1:
            cv2.putText(fr, str("Ready to Fire"), (250, 120), cv2.FONT_HERSHEY_SIMPLEX, 0.8, RGB_Color, 2)

        cv2.putText(fr, str("Mode:"), (5, 470), cv2.FONT_HERSHEY_SIMPLEX, 0.8, (0, 0, 0), 2)
        if M == 1:
            cv2.putText(fr, str("Manual"), (85, 470), cv2.FONT_HERSHEY_SIMPLEX, 0.8, RGB_Color, 2)
        elif M == 2:
            cv2.putText(fr, str("Auto Scan"), (85, 470), cv2.FONT_HERSHEY_SIMPLEX, 0.8, RGB_Color, 2)
        elif M == 3:
            cv2.putText(fr, str("Tracking"), (85, 470), cv2.FONT_HERSHEY_SIMPLEX, 0.8, RGB_Color, 2)
        elif M == 4:
            cv2.putText(fr, str("Laser Track"), (85, 470), cv2.FONT_HERSHEY_SIMPLEX, 0.8, RGB_Color, 2)
        
        return int(RGB_Cam_aim_X), int(RGB_Cam_aim_Y)
        

    #RGB 카메라 열기
    cap1 = cv2.VideoCapture(RGB_Cam_Index)
    cap1.set(cv2.CAP_PROP_FRAME_WIDTH, RGB_Cam_Res_X)
    cap1.set(cv2.CAP_PROP_FRAME_HEIGHT, RGB_Cam_Res_Y)
    cap1.set(cv2.CAP_PROP_FPS, RGB_Cam_FPS)

    #Tensorflow 모델 불러오기, 함수 불러오기
    save_path = '/home/rcws/Downloads/GG/Tensor_Model'
    model = tf.saved_model.load(save_path)
    movenet = model.signatures['serving_default']

    def combine_overlapping_rectangles(rectangles):
        combined_rectangles = []
        rectangles = sorted(rectangles, key=lambda x: (x[2], x[3]))
        current = rectangles[0]
        for rect in rectangles[1:]:
            if rect[0] <= current[2] and rect[1] <= current[3]:
                current = (current[0], current[1], max(current[2], rect[2]), max(current[3], rect[3]))
            else:
                combined_rectangles.append(current)
                current = rect
        combined_rectangles.append(current)
        return combined_rectangles
        
    def draw_bounding_box(frame, keypoints, confidence_threshold, color):
        y, x, c = frame.shape
        keypoints_array = np.array(keypoints)[:-1]
        shaped = np.squeeze(np.multiply(keypoints_array.reshape(-1, 3), [y, x, 1]))
    
        valid_points = [kp for kp in shaped if kp[2] > confidence_threshold]
        if not valid_points:
            return None

        min_x = min([kp[1] for kp in valid_points])
        min_y = min([kp[0] for kp in valid_points])
        max_x = max([kp[1] for kp in valid_points])
        max_y = max([kp[0] for kp in valid_points])

        return (min_x, min_y, max_x, max_y)

    def draw_bounding_box_(frame, keypoints,color):
        min_x = keypoints[0]
        min_y = keypoints[1]
        max_x = keypoints[2]
        max_y = keypoints[3]

        cv2.rectangle(frame, (int(min_x), int(min_y)), (int(max_x), int(max_y)), color, 2)
        return (min_x, min_y, max_x, max_y)

    def loop_through_people(frame, keypoints_with_scores, confidence_threshold):
        rectangles = []
        combined_rectangles = []
        for person in keypoints_with_scores:
            if tracker is None:
                rect = draw_bounding_box(frame, person, confidence_threshold, (255, 0, 0))
                if rect is not None:
                    rectangles.append(rect)

        if len(rectangles) > 0:
            combined_rectangles = combine_overlapping_rectangles(rectangles)
            for rect in combined_rectangles:
                draw_bounding_box_(frame, rect,(255, 0, 0))

        return combined_rectangles

    # Tracker 초기화
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
    with open("RGB_calibration1.pkl", "rb") as a:
        RGB_cameraMatrix0, RGB_dist = pickle.load(a)
    with open("RGB_calibration1.pkl", "rb") as a:
        RGB_cameraMatrix1, RGB_dist = pickle.load(a)
    with open("RGB_calibration2.pkl", "rb") as b:
        RGB_cameraMatrix2, RGB_dist = pickle.load(b)
    with open("RGB_calibration3.pkl", "rb") as c:
        RGB_cameraMatrix3, RGB_dist = pickle.load(c)
    with open("RGB_calibration4.pkl", "rb") as d:
        RGB_cameraMatrix4, RGB_dist = pickle.load(d)

    #창 띄우기
    #cv2.namedWindow("Movenet Multipose")
    #cv2.setMouseCallback("Movenet Multipose", mouse_callback)
    Zoom_0_Gain = 1
    Zoom_1_Gain = 1
    Zoom_2_Gain = 0.6
    Zoom_3_Gain = 0.3
    Zoom_4_Gain = 0.3

    X_P_Gain = 0.7
    X_I_Gain = 0
    X_D_Gain = 0.01

    Y_P_Gain = 0.15
    Y_I_Gain = 0
    Y_D_Gain = 0.001

    RGB_pre_time = 0

    X_IControl = 0
    Y_IControl = 0

    X_Pre_Error = 0
    Y_Pre_Error = 0

    #카메라 열려 있어야 프로세스 동작
    while cap1.isOpened():
        #프레임 읽기
        success1, frame1 = cap1.read()
    
        #프레임 읽기 성공하면
        if success1:
            check_t = time.time()
            #캘리브레이션 데이터 받아서 왜곡 펴기
            if Zoom_.value == 0:
                frame1 = cv.undistort(frame1, RGB_cameraMatrix0, RGB_dist, None)
            elif Zoom_.value == 1:
                frame1 = cv.undistort(frame1, RGB_cameraMatrix1, RGB_dist, None)
            elif Zoom_.value == 2:
                frame1 = cv.undistort(frame1, RGB_cameraMatrix2, RGB_dist, None)
            elif Zoom_.value == 3:
                frame1 = cv.undistort(frame1, RGB_cameraMatrix3, RGB_dist, None)
            elif Zoom_.value == 4:
                frame1 = cv.undistort(frame1, RGB_cameraMatrix4, RGB_dist, None)
            
            if Mode__.value == 0x00 and aming_complete_.value != 2:
                Manual_Mode = 1
                RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (0, 255, 0), 2, Manual_Mode)
            elif Mode__.value == 0x01:
                Auto_scan_mode = 2
                RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (0, 255, 0), 2, Auto_scan_mode)
            elif Mode__.value == 0x03:
                Human_Tracking_Mode = 3
                RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (0, 255, 0), 2, Human_Tracking_Mode)
            elif Mode__.value == 0x02:
                Lazer_Tracking_Mode = 4
                RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (255, 0, 0), 2, Lazer_Tracking_Mode)

            if request_access_.value == 1:
                cv2.putText(frame1, "Access Request", (50,400), cv2.FONT_HERSHEY_SIMPLEX, 1, (0,0,255), 2)

            if aming_complete_.value == 1:
                if Mode__.value == 0x00:
                    Manual_Mode = 1
                    RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (0,0,255), 2, Manual_Mode)
                elif Mode__.value == 0x01:
                    Auto_scan_mode = 2
                    RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (0,0,255), 2, Auto_scan_mode)
                elif Mode__.value == 0x03:
                    Human_Tracking_Mode = 3
                    RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (0,0,255), 2, Human_Tracking_Mode)
                elif Mode__.value == 0x02:
                    Lazer_Tracking_Mode = 4
                    RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1, (0,0,255), 2, Lazer_Tracking_Mode)
            elif aming_complete_.value == 2:
                cv2.putText(frame1, "Calibrating", (240,220), cv2.FONT_HERSHEY_SIMPLEX, 1, (0,0,255), 2)

            #마우스 오른쪽 클릭 시
            if C_LR_.value == 2:
                tracker = None
                person_detected_.value = 0
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
       
                rectangles_info = loop_through_people(frame1, keypoints_with_scores, 0.7)

                #if len(rectangles_info) > 0:
                    #print(rectangles_info[0])
                #마우스 왼 쪽 클릭 시 트래커 생성
                if (C_LR_.value == 1 and (C_D_X_1_.value == C_D_X_2_.value) and (C_D_Y_1_.value == C_D_Y_2_.value)):
                    for rect in rectangles_info:
                        min_x, min_y, max_x, max_y = rect
                        if (min_x <= C_X_.value / 1.8 <= max_x and min_y <= C_Y_.value / 1.8 <= max_y) and (max_x - min_x > 30) and (max_y - min_y > 30):
                            person_detected_.value = 1
                            tracker = cv2.TrackerCSRT_create()

                            min_x = min_x - 10
                            if min_x < 0:
                                min_x = 0
                            max_x = max_x + 10
                            if max_x > 639:
                                max_x = 639
                            min_y = min_y - 30
                            if min_y < 0:
                                min_y = 0
                            max_y = max_y + 60
                            if max_y > 479:
                                max_y = 479

                            bbox = (int(min_x), int(min_y), int(max_x - min_x), int(max_y - min_y))
                            C_LR_.value = 3
                            C_X_.value = 0
                            C_Y_.value = 0
                            success1, frame1 = cap1.read()
                            tracker.init(frame1, bbox)
                            break
                #UI에서 드래그 했을 때 트래킹 하기 위해
                elif (C_LR_.value == 1 and (C_D_X_1_.value != C_D_X_2_.value) and (C_D_Y_1_.value != C_D_Y_2_.value)):
                    min_x = 0
                    min_y = 0
                    max_x = 0
                    max_y = 0

                    if C_D_X_1_.value >= C_D_X_2_.value:
                        min_x = int(C_D_X_2_.value / 1.8)
                        max_x = int(C_D_X_1_.value / 1.8)
                    else:
                        max_x = int(C_D_X_2_.value / 1.8)
                        min_x = int(C_D_X_1_.value / 1.8)

                    if C_D_Y_1_.value >= C_D_Y_2_.value:
                        min_y = int(C_D_Y_2_.value / 1.8)
                        max_y = int(C_D_Y_1_.value / 1.8)
                    else:
                        min_y = int(C_D_Y_1_.value / 1.8)
                        max_y = int(C_D_Y_2_.value / 1.8)

                    if (min_x > 0 and min_x < 640) and (max_x > 0 and max_x < 640) and (min_y > 0 and min_y < 640) and (max_y > 0 and max_y < 640):
                        person_detected_.value = 1
                        tracker = cv2.TrackerCSRT_create()
                        bbox = (int(min_x), int(min_y), int(max_x - min_x), int(max_y - min_y))
                        C_LR_.value = 3
                        C_D_X_1_.value = 0
                        C_D_Y_1_.value = 0
                        C_D_X_2_.value = 0
                        C_D_Y_2_.value = 0
                        success1, frame1 = cap1.read()
                        tracker.init(frame1, bbox)
                #오토스캔 모드이고 사람 자동 트래킹 모드 켜져 있을 때
                elif Mode__.value == 0x01 and (Button_Command_.value & 0x10000) == 0x10000:
                    if len(rectangles_info) > 0:
                        min_x, min_y, max_x, max_y = rectangles_info[0]
                        if (min_x > 0 and min_x < 640) and (max_x > 0 and max_x < 640) and (min_y > 0 and min_y < 640) and (max_y > 0 and max_y < 640) and (max_x - min_x > 40) and (max_y - min_y > 40):
                            person_detected_.value = 1
                            tracker = cv2.TrackerCSRT_create()

                            min_x = min_x - 25
                            if min_x < 0:
                                min_x = 0
                            max_x = max_x + 25
                            if max_x > 639:
                                max_x = 639
                            min_y = min_y - 30
                            if min_y < 0:
                                min_y = 0
                            max_y = max_y + 60
                            if max_y > 479:
                                max_y = 479

                            bbox = (int(min_x), int(min_y), int(max_x - min_x), int(max_y - min_y))
                            success1, frame1 = cap1.read()
                            tracker.init(frame1, bbox)

                #사람 추적이 아니라 인식 때에는 모터 동작 X
                RGB_Com_X_.value = int(0)
                RGB_Com_Y_.value = int(0)
                X_IControl = 0
                X_Pre_Error = 0
                Y_IControl = 0
                Y_Pre_Error = 0

                RGB_pre_time = 0

                PID_previous_time = time.time()
            #사람 추적이 생성되면
            elif tracker is not None:
                #트래커 업데이트
                ret, bbox = tracker.update(frame1)
                #성공하면 PID 제어기 거쳐서 명령값 생성
                if ret:
                    x, y, w, h = [int(v) for v in bbox]
                    cv2.rectangle(frame1, (x, y), (x + w, y + h), (0, 0, 255), 2)
                    #if (Button_Command_.value & 0x10) == 0x10:
                    px = int((((x + x + w) / 2) - (RGB_Cam_aim_X)))
                    py = int((RGB_Cam_aim_Y - ((y + y + h) / 2)))

                    X_PControl = px * X_P_Gain
                    Y_PControl = py * Y_P_Gain

                    X_DControl = 0
                    Y_DControl = 0

                    if RGB_pre_time != 0:
                        X_IControl = X_IControl + (X_I_Gain * px * (time.time() - RGB_pre_time))
                        Y_IControl = Y_IControl + (Y_I_Gain * py * (time.time() - RGB_pre_time))

                        X_DControl = X_D_Gain * ((px - X_Pre_Error) / (time.time() - RGB_pre_time))
                        Y_DControl = Y_D_Gain * ((py - Y_Pre_Error) / (time.time() - RGB_pre_time))

                        
                    
                    RGB_pre_time = time.time()
                    X_Pre_Error = px
                    Y_Pre_Error = py

                    X_PIDControl = X_PControl + X_IControl + X_DControl
                    Y_PIDControl = Y_PControl + Y_IControl + Y_DControl

                    #목표와의 픽셀 단위 에러값으로 모터 구동
                    if Zoom_.value == 0:
                        RGB_Com_X_.value = int(X_PIDControl * Zoom_0_Gain)
                        RGB_Com_Y_.value = int(Y_PIDControl * Zoom_0_Gain)
                    elif Zoom_.value == 1:
                        RGB_Com_X_.value = int(X_PIDControl * Zoom_1_Gain)
                        RGB_Com_Y_.value = int(Y_PIDControl * Zoom_1_Gain)
                    elif Zoom_.value == 2:
                        RGB_Com_X_.value = int(X_PIDControl * Zoom_2_Gain)
                        RGB_Com_Y_.value = int(Y_PIDControl * Zoom_2_Gain)
                    elif Zoom_.value == 3:
                        RGB_Com_X_.value = int(X_PIDControl * Zoom_3_Gain)
                        RGB_Com_Y_.value = int(Y_PIDControl * Zoom_3_Gain)
                    elif Zoom_.value == 4:
                        RGB_Com_X_.value = int(X_PIDControl * Zoom_4_Gain)
                        RGB_Com_Y_.value = int(Y_PIDControl * Zoom_4_Gain)
                    
                    # RGB_Com_X_.value = int(X_PIDControl)
                    # RGB_Com_Y_.value = int(Y_PIDControl)
                # 트래킹 실패 시 트래커 리셋
                else:
                    #트래킹 실패 시 속도 0, 0으로 멈춤
                    X_IControl = 0
                    X_Pre_Error = 0
                    Y_IControl = 0
                    Y_Pre_Error = 0
                    
                    RGB_pre_time = 0
                    
                    person_detected_.value = 0
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
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap1.release()
    cv2.destroyAllWindows()
