import cv2 as cv
import cv2
import numpy as np
import pickle
import math
import time

def IR_pro(IR_Com_X_, IR_Com_Y_, target_distance_, Button_Command_):
    #IR 카메라 캘리브레이션 데이터 불러오기
    with open("IR_calibration.pkl", "rb") as f:
        IR_cameraMatrix, IR_dist = pickle.load(f)
        
    #IR 카메라 해상도 640X480
    IR_Cam_Index = 0
    IR_Cam_Res_X = 640
    IR_Cam_Res_Y = 480
    IR_Cam_FPS = 30

    #IR 카메라 화각 라디안각으로 변환
    IR_Cam_X_FoV = 64.6
    IR_Cam_X_FoV = math.pi * (IR_Cam_X_FoV / 2 / 180)
    
    IR_Cam_Y_FoV = 30
    IR_Cam_Y_FoV = math.pi * (IR_Cam_Y_FoV / 2 / 180)

    #IR 카메라, TOF 센서간의 간격(cm)
    Distance_with_IR_TOF = 5
    
    #하드웨어적으로 틀어져 있는 정도를 보정하기 위한 변수들
    #밑 변수들은 그냥 강제적으로 픽셀 맞추는거라 웬만하면 안건드는게 좋음
    IR_x_offset = 0
    IR_y_offset = 0

    #밑 변수들은 카메라가 틀어진 각도와 TOF 센서를 사용해서 픽셀 거리값 계산해서 맞추기 위한 변수
    #웬만하면 밑 변수들만 변경해서 보정하는 것이 좋음
    #RCWS를 위에서 봤을 때 시계방향으로 틀어졌을 때 +, 반시계방향으로 틀어졌을 때 -
    IR_X_Tilt_Angle = 0
    IR_X_Tilt_Angle = math.pi * (IR_X_Tilt_Angle / 180)

    #위로 가면 +, 아래로 가면 -
    IR_Y_Tilt_Angle = 0
    IR_Y_Tilt_Angle = math.pi * (IR_Y_Tilt_Angle / 180)

    #화면에 출력할 조준선 길이
    aim_lenth = 5

    #조준선 보정값 (픽셀 단위)
    IR_Cam_aim_X = 0
    IR_Cam_aim_Y = 0

    def Move_Aim(fr):
        global IR_Cam_aim_X, IR_Cam_aim_Y

        dist = target_distance_.value

        PD_X = (float)(math.tan(IR_Cam_X_FoV)) * (float)(dist)
        PD_X = (float)(PD_X) / ((float)(IR_Cam_Res_X) / (float)(2))
        
        PD_Y = (float)(math.tan(IR_Cam_Y_FoV)) * (float)(dist)
        PD_Y = (float)(PD_Y) / ((float)(IR_Cam_Res_Y) / (float)(2))

        if PD_X != 0:
            IR_CAM_TILT_OFFSET_X = (float)(math.tan(IR_X_Tilt_Angle)) * (float)(dist)
            IR_CAM_TILT_OFFSET_X = (float)(IR_CAM_TILT_OFFSET_X) / (float)(PD_X)

            if IR_CAM_TILT_OFFSET_X != 0:
                IR_Cam_aim_X = int((IR_Cam_Res_X / 2) - 1 - ((float)(Distance_with_IR_TOF) / (float)(PD_X)) - IR_CAM_TILT_OFFSET_X + IR_x_offset)
            else:
                IR_Cam_aim_X = int((IR_Cam_Res_X / 2) - 1 - ((float)(Distance_with_IR_TOF) / (float)(PD_X)) + IR_x_offset)
        elif PD_X == 0:
            IR_Cam_aim_X = int((IR_Cam_Res_X / 2) - 1 + IR_x_offset)

        if PD_Y != 0:
            IR_CAM_TILT_OFFSET_Y = (float)(math.tan(IR_Y_Tilt_Angle)) * (float)(dist)
            IR_CAM_TILT_OFFSET_Y = (float)(IR_CAM_TILT_OFFSET_Y) / (float)(PD_Y)
            if IR_CAM_TILT_OFFSET_Y != 0:
                IR_Cam_aim_Y = int((IR_Cam_Res_Y / 2) - 1 + IR_CAM_TILT_OFFSET_Y - IR_y_offset)
            else:
                IR_Cam_aim_Y = int((IR_Cam_Res_Y / 2) - 1 - IR_y_offset)
        elif PD_Y == 0:
            IR_Cam_aim_Y = int((IR_Cam_Res_Y / 2) - 1 - IR_y_offset)

        return (int)(IR_Cam_aim_X), (int)(IR_Cam_aim_Y)

    #IR 카메라 열기
    cap2 = cv2.VideoCapture(IR_Cam_Index)
    cap2.set(cv2.CAP_PROP_FRAME_WIDTH, IR_Cam_Res_X)
    cap2.set(cv2.CAP_PROP_FRAME_HEIGHT, IR_Cam_Res_Y)
    cap2.set(cv2.CAP_PROP_FPS, IR_Cam_FPS)

    #블러치리할 영역(), 나눌 숫자
    kernel = np.ones((3, 3))/3**2

    #침식 범위
    #array = np.array([[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1]])
    array = np.array([[1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1]])

    IR_pre_time = 0

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
        
            IR_Cam_aim_X, IR_Cam_aim_Y = Move_Aim(dilate)

            white_pixel_coordinates = np.column_stack(np.nonzero(dilate))
            
            if len(white_pixel_coordinates) > 0:
                center = np.mean(white_pixel_coordinates, axis=0)
                center = tuple(map(int, center))  # 정수형으로 변환
                if (Button_Command_.value & 0x20) == 0x20:
                    x = int(center[1] - (IR_Cam_aim_X))
                    y = int(IR_Cam_aim_Y - center[0])

                    IR_Com_X_.value = int(x * 2)
                    IR_Com_Y_.value = int(y * 0.95)
            else:
                #레이저 놓치면 속도 0, 0으로
                IR_Com_X_.value = int(0)
                IR_Com_Y_.value = int(0)

            #조준선 그리기 위해 컬러로 다시 변환 지워도 됌 테스트 용
            dilate = cv2.cvtColor(dilate, cv2.COLOR_GRAY2BGR)
            
            x1 = IR_Cam_aim_X - (aim_lenth + 5)
            x2 = IR_Cam_aim_X - 5
            x3 = IR_Cam_aim_X + 5
            x4 = IR_Cam_aim_X + (aim_lenth + 5)

            y1 = IR_Cam_aim_Y - (aim_lenth + 5)
            y2 = IR_Cam_aim_Y - 5
            y3 = IR_Cam_aim_Y + 5
            y4 = IR_Cam_aim_Y + (aim_lenth + 5)

            cv2.line(dilate, (x1, IR_Cam_aim_Y), (x2, IR_Cam_aim_Y), (0, 0, 255), 2)
            cv2.line(dilate, (x3, IR_Cam_aim_Y), (x4, IR_Cam_aim_Y), (0, 0, 255), 2)

            cv2.line(dilate, (IR_Cam_aim_X, y1), (IR_Cam_aim_X, y2), (0, 0, 255), 2)
            cv2.line(dilate, (IR_Cam_aim_X, y3), (IR_Cam_aim_X, y4), (0, 0, 255), 2)

            cv2.putText(dilate, str(math.floor(1 / (time.time() - IR_pre_time))) + "FPS", (50,100), cv2.FONT_HERSHEY_SIMPLEX, 1, (0,0,255), 2)
            IR_pre_time = time.time()

            #필터링한 결과 영상 출력
            cv2.imshow('Camera Window1', dilate)
            #cv2.imshow('Camera Window2', frame2)

        #어차피 IR 카메라 FPS 30이어서 한 프레임 읽는데 30ms 넘게 걸림 waitkey 30
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
 
    cap2.release()
    cv2.destroyAllWindows()
