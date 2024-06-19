import tensorflow as tf
import tensorflow_hub as hub
import tensorflow_hub as keras
import cv2 as cv
import cv2
import numpy as np
import pickle
import time
import socket
import math
import struct

def RGB_pro(C_LR_, C_X_, C_Y_, RGB_Com_X_, RGB_Com_Y_, target_distance_, Button_Command_, request_access_,):
    #영상 송신 UDP 통신 서버, 클라이언트 주소 지정
    max_length = 65000
    #UDP_host_IP = "10.254.2.96"
    UDP_host_IP = "192.168.0.29"
    UDP_host_port = 9000
    #UDP_Client_IP = "10.254.1.3"
    UDP_Client_IP = "192.168.0.30"
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
    RGB_Cam_X_FoV = 13.5
    RGB_Cam_X_FoV = math.pi * (RGB_Cam_X_FoV / 2 / 180)

    RGB_Cam_Y_FoV = 6
    RGB_Cam_Y_FoV = math.pi * (RGB_Cam_Y_FoV / 2 / 180)

    #RGB 카메라와 TOF 센서간의 거리(cm)
    Distance_with_RGB_TOF = 5.5
    
    #하드웨어적으로 틀어져 있는 정도를 보정하기 위한 변수들
    #밑 변수들은 픽셀값 강제적으로 보정하는거라 웬만하면 사용 안하는 것이 좋음
    RGB_x_offset = 0
    RGB_y_offset = 0

    #카메라 틀어진 각도와 TOF 센서로 거리값 받아서 픽셀거리 계산해서 보정하는 변수들
    #웬만하면 밑 변수들로 보정하는 것이 좋음
    #RCWS를 위에서 봤을 때 시계방향으로 틀어졌으면 +, 반시계방향으로 틀어졌으면 -
    RGB_X_Tilt_Angle = 0
    RGB_X_Tilt_Angle = math.pi * (RGB_X_Tilt_Angle / 180)

    #위로 가면 +, 아래로 가면 -
    RGB_Y_Tilt_Angle = 0
    RGB_Y_Tilt_Angle = math.pi * (RGB_Y_Tilt_Angle / 180)

    #화면에 출력할 조준선 길이
    aim_lenth = 5

    #조준선 보정값 (픽셀 단위)
    RGB_Cam_aim_X = 0
    RGB_Cam_aim_Y = 0

    def Move_Aim(fr):
        global RGB_Cam_aim_X, RGB_Cam_aim_Y

        dist = target_distance_.value

        PD_X = (float)(math.tan(RGB_Cam_X_FoV)) * (float)(dist)
        PD_X = (float)(PD_X) / ((float)(RGB_Cam_Res_X) / (float)(2))

        PD_Y = (float)(math.tan(RGB_Cam_Y_FoV)) * (float)(dist)
        PD_Y = (float)(PD_Y) / ((float)(RGB_Cam_Res_Y) / (float)(2))
        
        if PD_X != 0:
            RGB_CAM_TILT_OFFSET_X = (float)(math.tan(RGB_X_Tilt_Angle)) * (float)(dist)
            RGB_CAM_TILT_OFFSET_X = (float)(RGB_CAM_TILT_OFFSET_X) / (float)(PD_X)

            if RGB_CAM_TILT_OFFSET_X != 0:
                RGB_Cam_aim_X = int((RGB_Cam_Res_X / 2) - 1 + ((float)(Distance_with_RGB_TOF) / (float)(PD_X)) - RGB_CAM_TILT_OFFSET_X + RGB_x_offset)
            else:
                RGB_Cam_aim_X = int((RGB_Cam_Res_X / 2) - 1 + ((float)(Distance_with_RGB_TOF) / (float)(PD_X)) + RGB_x_offset)
        elif PD_X == 0:
            RGB_Cam_aim_X = int((RGB_Cam_Res_X / 2) - 1 + RGB_x_offset)

        if PD_Y != 0:
            RGB_CAM_TILT_OFFSET_Y = (float)(math.tan(RGB_Y_Tilt_Angle)) * (float)(dist)
            RGB_CAM_TILT_OFFSET_Y = (float)(RGB_CAM_TILT_OFFSET_Y) / (float)(PD_Y)

            if RGB_CAM_TILT_OFFSET_Y != 0:
                RGB_Cam_aim_Y = int((RGB_Cam_Res_Y / 2) - 1 + RGB_CAM_TILT_OFFSET_Y - RGB_y_offset)
            else:
                RGB_Cam_aim_Y = int((RGB_Cam_Res_Y / 2) - 1 - RGB_y_offset)
        elif PD_Y == 0:
            RGB_Cam_aim_Y = int((RGB_Cam_Res_Y / 2) - 1 - RGB_y_offset)
        
        x1 = RGB_Cam_aim_X - (aim_lenth + 5)
        x2 = RGB_Cam_aim_X - 5
        x3 = RGB_Cam_aim_X + 5
        x4 = RGB_Cam_aim_X + (aim_lenth + 5)

        y1 = RGB_Cam_aim_Y - (aim_lenth + 5)
        y2 = RGB_Cam_aim_Y - 5
        y3 = RGB_Cam_aim_Y + 5
        y4 = RGB_Cam_aim_Y + (aim_lenth + 5)

        cv2.line(fr, (x1, RGB_Cam_aim_Y), (x2, RGB_Cam_aim_Y), (0, 0, 255), 2)
        cv2.line(fr, (x3, RGB_Cam_aim_Y), (x4, RGB_Cam_aim_Y), (0, 0, 255), 2)

        cv2.line(fr, (RGB_Cam_aim_X, y1), (RGB_Cam_aim_X, y2), (0, 0, 255), 2)
        cv2.line(fr, (RGB_Cam_aim_X, y3), (RGB_Cam_aim_X, y4), (0, 0, 255), 2)
    
        return (int)(RGB_Cam_aim_X), (int)(RGB_Cam_aim_Y)

    #RGB 카메라 열기
    cap1 = cv2.VideoCapture(RGB_Cam_Index)
    cap1.set(cv2.CAP_PROP_FRAME_WIDTH, RGB_Cam_Res_X)
    cap1.set(cv2.CAP_PROP_FRAME_HEIGHT, RGB_Cam_Res_Y)
    cap1.set(cv2.CAP_PROP_FPS, RGB_Cam_FPS)

    #Tensorflow 모델 불러오기, 함수 불러오기
    save_path = '/home/rcws/Downloads/GG/Tensor_Model'
    model = tf.saved_model.load(save_path)
    #model = hub.load('https://tfhub.dev/google/movenet/multipose/lightning/1')
    movenet = model.signatures['serving_default']

    def combine_overlapping_rectangles(rectangles):
        combined_rectangles = []
        rectangles = sorted(rectangles, key=lambda x: (x[2], x[3]))  # Sort by max_x and max_y
        current = rectangles[0]
        for rect in rectangles[1:]:
            # If the current rectangle overlaps with the next one, update the max_x and max_y
            if rect[0] <= current[2] and rect[1] <= current[3]:
                current = (current[0], current[1], max(current[2], rect[2]), max(current[3], rect[3]))
            else:
                combined_rectangles.append(current)
                current = rect
        combined_rectangles.append(current)
        return combined_rectangles
        
    def draw_bounding_box(frame, keypoints, confidence_threshold, color):
        y, x, c = frame.shape
        keypoints_array = np.array(keypoints)[:-1]  # 마지막 요소를 제외하여 배열을 재구성합니다.
        shaped = np.squeeze(np.multiply(keypoints_array.reshape(-1, 3), [y, x, 1]))
    
        # 'shaped' 배열의 각 요소를 반복하면서 'confidence_threshold'와 비교합니다.
        valid_points = [kp for kp in shaped if kp[2] > confidence_threshold]
        if not valid_points:
            return None

        min_x = min([kp[1] for kp in valid_points])
        min_y = min([kp[0] for kp in valid_points])
        max_x = max([kp[1] for kp in valid_points])
        max_y = max([kp[0] for kp in valid_points])

        #cv2.rectangle(frame, (int(min_x), int(min_y)), (int(max_x), int(max_y)), color, 2)
        return (min_x, min_y, max_x, max_y)

    def draw_bounding_box_(frame, keypoints,color):
        min_x = keypoints[0]
        min_y = keypoints[1]
        max_x = keypoints[2]
        max_y = keypoints[3]

        cv2.rectangle(frame, (int(min_x), int(min_y)), (int(max_x), int(max_y)), color, 2)
        return (min_x, min_y, max_x, max_y)

    # Loop through people and draw bounding boxes
    def loop_through_people(frame, keypoints_with_scores, confidence_threshold):
        rectangles = []
        combined_rectangles = []
        for person in keypoints_with_scores:
            # Draw bounding box only if the tracker is not active
            if tracker is None:
                rect = draw_bounding_box(frame, person, confidence_threshold, (255, 0, 0))
                if rect is not None:
                    rectangles.append(rect)

        if len(rectangles) > 0:
            combined_rectangles = combine_overlapping_rectangles(rectangles)
            for rect in combined_rectangles:
                draw_bounding_box_(frame, rect,(0, 0, 255))  # Red color for combined rectangles

        return combined_rectangles

    # Tracker 초기화
    tracker = None

    #마우스 클릭해서 트래킹 하는 함수인데 멀티 프로세스에서 안됌, 상황실 UI랑 통신해서 하면 되니까 걱정하지 않기
    # def mouse_callback(event, x, y, flags, param):
    #     if event == cv2.EVENT_LBUTTONDOWN:
    #         for rect in rectangles_info:
    #             min_x, min_y, max_x, max_y = rect
    #             if min_x <= x <= max_x and min_y <= y <= max_y:
    #                 tracker = cv2.TrackerCSRT_create()
    #                 bbox = (int(min_x), int(min_y), int(max_x - min_x), int(max_y - min_y))
    #                 tracker.init(frame1, bbox)
    #                 break
    #     elif event == cv2.EVENT_RBUTTONDOWN:
    #         tracker = None

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
    #cv2.namedWindow("Movenet Multipose")
    #cv2.setMouseCallback("Movenet Multipose", mouse_callback)

    RGB_pre_time = 0

    #카메라 열려 있어야 프로세스 동작
    while cap1.isOpened():
        #프레임 읽기
        success1, frame1 = cap1.read()
    
        #프레임 읽기 성공하면
        if success1:
            check_t = time.time()
            #캘리브레이션 데이터 받아서 왜곡 펴기
            frame1 = cv.undistort(frame1, RGB_cameraMatrix, RGB_dist, None)
            
            RGB_Cam_aim_X, RGB_Cam_aim_Y = Move_Aim(frame1)
            
            cv2.putText(frame1, str(math.floor(1 / (time.time() - RGB_pre_time))) + "FPS", (50,100), cv2.FONT_HERSHEY_SIMPLEX, 1, (0,0,255), 2)
            RGB_pre_time = time.time()

            if request_access_.value == 1:
                cv2.putText(frame1, "Access Request", (50,400), cv2.FONT_HERSHEY_SIMPLEX, 1, (0,0,255), 2)

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
       
                rectangles_info = loop_through_people(frame1, keypoints_with_scores, 0.8)
            
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
                    #if (Button_Command_.value & 0x10) == 0x10:
                    px = int((((x + x + w) / 2) - (RGB_Cam_aim_X)))
                    py = int((RGB_Cam_aim_Y - ((y + y + h) / 2)))

                    #print(px, ", ", py)

                    #목표와의 픽셀 단위 에러값으로 모터 구동
                    RGB_Com_X_.value = int(px * 0.65)
                    RGB_Com_Y_.value = int(py * 0.2)
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
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap1.release()
    cv2.destroyAllWindows()
