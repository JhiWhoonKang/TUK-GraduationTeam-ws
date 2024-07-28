import cv2 as cv
import cv2
import numpy as np
import pickle
import math
import time

def IR_pro(IR_Com_X_, IR_Com_Y_, target_distance_, Mode__, IR_Armed_, Sentry_Communication_, Sentry_Azimuth_, Sentry_Elevation_, Optical_angle_, Body_angle_):
    #IR 카메라 캘리브레이션 데이터 불러오기
    with open("IR_calibration.pkl", "rb") as f:
        IR_cameraMatrix, IR_dist = pickle.load(f)
        
    #IR 카메라 해상도 640X480
    IR_Cam_Index = 0 
    IR_Cam_Res_X = 640
    IR_Cam_Res_Y = 480
    IR_Cam_FPS = 30

    #RGB 카메라 화각 라디안각으로 변환
    IR_Cam_X_FoV = 63.19
    IR_Cam_X_FoV = math.pi * (IR_Cam_X_FoV / 2 / 180)
    
    IR_Cam_Y_FoV = 47.9
    IR_Cam_Y_FoV = math.pi * (IR_Cam_Y_FoV / 2 / 180)

    Distance_with_IR_TOF = 5
    
    #하드웨어적으로 틀어져 있는 정도를 보정하기 위한 변수들
    IR_x_offset = 0
    IR_y_offset = 10

    IR_X_Tilt_Angle = 0
    IR_X_Tilt_Angle = math.pi * (IR_X_Tilt_Angle / 180)

    IR_Y_Tilt_Angle = 0
    IR_Y_Tilt_Angle = math.pi * (IR_Y_Tilt_Angle / 180)

    #화면에 출력할 조준선 길이
    aim_lenth = 5

    #조준선 보정값 (픽셀 단위)
    IR_Cam_aim_X = 0
    IR_Cam_aim_Y = 0

    #초병 조준 각도 서치, 스캔하기 위한 플래그, 속도 변수
    Scan_Flag1_x = 0
    Scan_Flag1_y = 0

    Scan_Flag2_x = 0
    Scan_Flag2_y = 0
    
    Scan_Flag3_x = 0
    Scan_Flag3_y = 0

    Scan_Speed_x = 20
    Scan_Speed_y = 20
    
    Scan_Search_Speed_x = 200
    Scan_Search_Speed_y = 100

    #초병 조준 각도 +-로 서칭하기 위한 범위 설정 (각도)
    Search_Range_x = 10
    Search_Range_y = 5

    #임계값 가변하기 위한 변수
    Trans_Thresh = 200

    #조준선 움직이기 위한 함수
    def Move_Aim(fr):
        #최종 IR 조준선 변수들
        global IR_Cam_aim_X, IR_Cam_aim_Y

        #목표 대상 거리값 변수 저장
        dist = target_distance_.value

        #X축 픽셀당 거리 계산
        PD_X = (float)(math.tan(IR_Cam_X_FoV)) * (float)(dist)
        PD_X = (float)(PD_X) / ((float)(IR_Cam_Res_X) / (float)(2))
        
        #Y축 픽셀당 거리 계산
        PD_Y = (float)(math.tan(IR_Cam_Y_FoV)) * (float)(dist)
        PD_Y = (float)(PD_Y) / ((float)(IR_Cam_Res_Y) / (float)(2))

        #X축 픽셀 하나 크기가 0이 아닐 때
        if PD_X != 0:
            #하드웨어적으로 틀어진거 보정 변수
            IR_CAM_TILT_OFFSET_X = (float)(math.tan(IR_X_Tilt_Angle)) * (float)(dist)
            IR_CAM_TILT_OFFSET_X = (float)(IR_CAM_TILT_OFFSET_X) / (float)(PD_X)

            #하드웨어적으로 틀어진거 보정 변수가 0이 아닐 때
            if IR_CAM_TILT_OFFSET_X != 0:
                #조준선 계산
                IR_Cam_aim_X = int((IR_Cam_Res_X / 2) - 1 - ((float)(Distance_with_IR_TOF) / (float)(PD_X)) - IR_CAM_TILT_OFFSET_X + IR_x_offset)
            else:
                #조준선 계산
                IR_Cam_aim_X = int((IR_Cam_Res_X / 2) - 1 - ((float)(Distance_with_IR_TOF) / (float)(PD_X)) + IR_x_offset)
        #X축 픽셀 크기가 0이면 이미지 가운데로 조준선 조정
        elif PD_X == 0:
            IR_Cam_aim_X = int((IR_Cam_Res_X / 2) - 1 + IR_x_offset)

        #Y축 픽셀 하나 크기가 0이 아닐 때
        if PD_Y != 0:
            #하드웨어적으로 틀어진거 보정 변수
            IR_CAM_TILT_OFFSET_Y = (float)(math.tan(IR_Y_Tilt_Angle)) * (float)(dist)
            IR_CAM_TILT_OFFSET_Y = (float)(IR_CAM_TILT_OFFSET_Y) / (float)(PD_Y)
            
            #하드웨어적으로 틀어진거 보정 변수가 0이 아닐 때
            if IR_CAM_TILT_OFFSET_Y != 0:
                #조준선 계산
                IR_Cam_aim_Y = int((IR_Cam_Res_Y / 2) - 1 + IR_CAM_TILT_OFFSET_Y - IR_y_offset)
            else:
                #조준선 계산
                IR_Cam_aim_Y = int((IR_Cam_Res_Y / 2) - 1 - IR_y_offset)
        #Y축 픽셀 크기가 0이면 이미지 가운데로 조준선 조정
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

    #침식, 팽창 범위
    #array = np.array([[1, 1, 1],[1, 1, 1],[1, 1, 1]])
    #array = np.array([[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1],[1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1],[1, 1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1],[1, 1, 1, 1, 1, 1]])
    array = np.array([[1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1]])
    #array = np.array([[1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], 
                    #   [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], 
                    #   [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], 
                    #   [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], 
                    #   [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], 
                    #   [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1], 
                    #   [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]])
    
    #적외선 레이저랑 맞추기 위해서 카메라 조준선을 조정

    IR_pre_time = 0

    #IR 카메라 연결되어 있을 때
    while cap2.isOpened():
        #한 프레임 읽기
        success2, frame2 = cap2.read()
        if success2:
            #캘리브레이션 데이터로 왜곡 펴기
            frame2 = cv.undistort(frame2, IR_cameraMatrix, IR_dist, None)
            cv2.imshow('Camera Window1', frame2)

            #흑백 변환
            frame2_gray = cv2.cvtColor(frame2, cv2.COLOR_BGR2GRAY)
            
            #이미지에서 가장 밝은 픽셀 찾기
            (min_val, max_val, min_loc, max_loc) = cv2.minMaxLoc(frame2_gray)

            #이미지 복사
            otput = frame2_gray

            #가장 밝은 픽셀 밝기가 70 이상일 때
            if max_val >= 150:
                binary_image = np.zeros_like(otput, dtype=np.uint8)
                #가장 밝은 픽셀 30X30 픽셀 이내에서
                for i in range(-15, 16):
                    for j in range(-15, 16):
                        x, y = max_loc[0] + i, max_loc[1] + j
                        if 0 <= x < otput.shape[1] and 0 <= y < otput.shape[0]:
                            #밝기가 60 이상인 픽셀들은 255로 임계값을 넘을 수 있도록 밝게 만들기
                            if int(otput[y, x]) > 60:
                                otput[y, x] = 255

            #블러처리
            blured = cv2.filter2D(frame2_gray, -1, kernel)

            #이진화
            #우리 눈에는 안보여도 60 아래에 밝기의 잡음 픽셀이 있을 수 있어서 처리
            if max_val >= 150:
                Trans_Thresh = max_val
            else:
                Trans_Thresh = 150

            ret_set_binary, set_binary = cv2.threshold(frame2_gray, Trans_Thresh, 255, cv2.THRESH_BINARY)
            #ret_set_binary, set_binary = cv2.threshold(blured, ma_val - 5, 255, cv2.THRESH_BINARY)
        
            #erode = cv2.erode(set_binary, array) #침식
            #dilate = cv2.dilate(erode, array) #팽창
            #erode = cv2.erode(set_binary, array) #침식
            dilate = cv2.dilate(set_binary, array) #팽창
            
            #cv2.imshow('Camera Window9', dilate)
        
            IR_Cam_aim_X, IR_Cam_aim_Y = Move_Aim(dilate)

            #print(max_val)

            #흰 부분을 그룹으로 묶어서 처리하기 위해 처리
            num_labels, labels, stats, centroids = cv2.connectedComponentsWithStats(dilate)

            #white_pixel_coordinates = np.column_stack(np.nonzero(dilate))
            #모터 제어 게인
            X_gain = 2
            Y_gain = 0.95
            #cv2.imshow('Camera Window1', dilate)

            #흰 부분 그룹이 있고 초병이 레이저를 조사했을 때
            if num_labels - 1 > 0 and (Sentry_Communication_.value & 0x0001) == 0x0001:
                #적외선 레이저 서치 플래그 초기화
                Scan_Flag1_x = 0
                Scan_Flag2_x = 0
                Scan_Flag3_x = 0
                Scan_Flag1_y = 0
                Scan_Flag2_y = 0
                Scan_Flag3_y = 0
                #흰 부분이 하나 있을 때
                if num_labels - 1 == 1:
                    #그룹의 중앙 픽셀 추출
                    white_pixel_coordinates = np.column_stack(np.nonzero(dilate))
                    center = np.mean(white_pixel_coordinates, axis=0)
                    center = tuple(map(int, center))  # 정수형으로 변환
                    #초병 권한 모드일 때
                    if Mode__.value == 0x02:
                        #픽셀 차이 계산
                        x = int(center[1] - (IR_Cam_aim_X))
                        y = int(IR_Cam_aim_Y - center[0])

                        #조준이 2 픽셀 내로 됐을 때
                        #초병 홀로그램에 띄우기 위한 데이터 저장
                        if abs(x) <= 2 and abs(y) <= 2:
                            IR_Armed_.value = 1
                        else:
                            IR_Armed_.value = 0

                        #적외선 레이저 트래킹 모터 명령 변수에 저장
                        IR_Com_X_.value = int(x * X_gain)
                        IR_Com_Y_.value = int(y * Y_gain)
            #인식된 흰 그룹이 없는데 초병이 레이저를 조사 중일 때
            elif num_labels - 1 == 0 and (Sentry_Communication_.value & 0x0001) == 0x0001:
                #초병 Z축 각도가 RCWS Z축보다 왼쪽에 있을 때 서치
                if Body_angle_.value > Sentry_Azimuth_.value + 5 and Scan_Flag1_x == 0:
                    #RCWS 왼쪽으로 돌리기
                    IR_Com_X_.value = -Scan_Search_Speed_x
                    #다음 시퀀스에서 왼쪽으로 움직이고 있는거 확인하기 위해 플래그
                    Scan_Flag2_x = 0
                    Scan_Flag3_x = 0
                #초병 Z축 각도가 RCWS Z축보다 오른쪽에 있을 때
                elif Body_angle_.value < Sentry_Azimuth_.value - 5 and Scan_Flag1_x == 0:
                    #RCWS 오른쪽으로 돌리기
                    IR_Com_X_.value = Scan_Search_Speed_x
                    #다음 시퀀스에서 오른쪽으로 움직이고 있는거 확인하기 위해 플래그
                    Scan_Flag2_x = 1
                    Scan_Flag3_x = 1
                #초병 각도에 도달 했을 때 서치->스캔으로 가기 위해
                elif abs(Body_angle_.value - Sentry_Azimuth_.value) <= 5 and Scan_Flag1_x == 0:
                    #서치 완료 플래그
                    Scan_Flag1_x = 1
                    #일단 모터 정지 명령
                    IR_Com_X_.value = 0

                #초병 Y축 각도가 RCWS Y축보다 아래쪽에 있을 때 서치
                if Optical_angle_.value > Sentry_Elevation_.value + 5 and Scan_Flag1_y == 0:
                    #광학부 아래로 내리기
                    IR_Com_Y_.value = -Scan_Search_Speed_y
                    #다음 시퀀스에서 아래쪽으로 움직이고 있는거 확인하기 위해 플래그
                    Scan_Flag2_y = 0
                    Scan_Flag3_y = 0
                #초병 Y축 각도가 RCWS Y축보다 위쪽에 있을 때 서치
                elif Optical_angle_.value < Sentry_Elevation_.value - 5 and Scan_Flag1_y == 0:
                    #광학부 위로 올리기
                    IR_Com_Y_.value = Scan_Search_Speed_y
                    #다음 시퀀스에서 위쪽으로 움직이고 있는거 확인하기 위해 플래그
                    Scan_Flag2_y = 1
                    Scan_Flag3_y = 1
                #초병 각도에 도달 했을 때 서치->스캔으로 가기 위해
                elif abs(Optical_angle_.value - Sentry_Elevation_.value) <= 5 and Scan_Flag1_y == 0:
                    #서치 완료 플래그
                    Scan_Flag1_y = 1
                    #일단 모터 정지 명령
                    IR_Com_Y_.value = 0

                #서치 완료했고 왼쪽으로 가고 있는데
                if Scan_Flag1_x == 1 and Scan_Flag2_x == 1 and Scan_Flag3_x == 1:
                    #아직 초병 방위각 + Search_Range_x 범위까지 도달하지 못했으면
                    if Body_angle_.value < Sentry_Azimuth_.value + Search_Range_x:
                        #오른쪽으로 움직이기
                        IR_Com_X_.value = Scan_Speed_x
                    #도달했으면
                    else:
                        #방향 전환 플래그(왼쪽으로)
                        Scan_Flag3_x = 0
                #서치 완료했고 초병 방위각 + Search_Range_x 범위까지 도달했으면 다시 왼쪽으로 보내기
                elif Scan_Flag1_x == 1 and Scan_Flag2_x == 1 and Scan_Flag3_x == 0:
                    #아직 초병 방위각 - Search_Range_x 범위까지 도달하지 못했으면
                    if Body_angle_.value > Sentry_Azimuth_.value - Search_Range_x:
                        #왼쪽으로 움직이기
                        IR_Com_X_.value = -Scan_Speed_x
                    #도달했으면
                    else:
                        #방향 전환 플래그(오른쪽으로)
                        Scan_Flag3_x = 1
                #서치 완료했고 오른쪽으로 가고 있는데
                elif Scan_Flag1_x == 1 and Scan_Flag2_x == 0 and Scan_Flag3_x == 0:
                    #아직 초병 방위각 - Search_Range_x 범위까지 도달하지 못했으면
                    if Body_angle_.value > Sentry_Azimuth_.value - Search_Range_x:
                        #왼쪽으로 움직이기
                        IR_Com_X_.value = -Scan_Speed_x
                    #도달했으면
                    else:
                        #방향 전환 플래그(오른쪽으로)
                        Scan_Flag3_x = 1
                #서치 완료했고 초병 방위각 - Search_Range_x 범위까지 도달했으면 다시 오른쪽으로 보내기
                elif Scan_Flag1_x == 1 and Scan_Flag2_x == 0 and Scan_Flag3_x == 1:
                    #아직 초병 방위각 + Search_Range_x 범위까지 도달하지 못했으면
                    if Body_angle_.value < Sentry_Azimuth_.value + Search_Range_x:
                        #오른쪽으로 움직이기
                        IR_Com_X_.value = Scan_Speed_x
                    #도달했으면
                    else:
                        #방향 전환 플래그(왼쪽으로)
                        Scan_Flag3_x = 0
                
                #Y축도 마찬가지
                if Scan_Flag1_y == 1 and Scan_Flag2_y == 1 and Scan_Flag3_y == 1:
                    if Optical_angle_.value < Sentry_Elevation_.value + Search_Range_y:
                        IR_Com_Y_.value = Scan_Speed_y
                    else:
                        Scan_Flag3_y = 0
                elif Scan_Flag1_y == 1 and Scan_Flag2_y == 1 and Scan_Flag3_y == 0:
                    if Optical_angle_.value > Sentry_Elevation_.value - Search_Range_y:
                        IR_Com_Y_.value = -Scan_Speed_y
                    else:
                        Scan_Flag3_y = 1
                elif Scan_Flag1_y == 1 and Scan_Flag2_y == 0 and Scan_Flag3_y == 0:
                    if Optical_angle_.value > Sentry_Elevation_.value - Search_Range_y:
                        IR_Com_Y_.value = -Scan_Speed_y
                    else:
                        Scan_Flag3_y = 1
                elif Scan_Flag1_y == 1 and Scan_Flag2_y == 0 and Scan_Flag3_y == 1:
                    if Optical_angle_.value < Sentry_Elevation_.value + Search_Range_y:
                        IR_Com_Y_.value = Scan_Speed_y
                    else:
                        Scan_Flag3_y = 0
            #흰 색 그룹이 인식 됐는데 초병이 레이저 쏜게 아니면
            elif num_labels - 1 > 0 and (Sentry_Communication_.value & 0x0001) == 0x0000:
                Scan_Flag1_x = 0
                Scan_Flag2_x = 0
                Scan_Flag3_x = 0
                Scan_Flag1_y = 0
                Scan_Flag2_y = 0
                Scan_Flag3_y = 0
                #모터 멈추기
                IR_Com_X_.value = int(0)
                IR_Com_Y_.value = int(0)
            else:
                Scan_Flag1_x = 0
                Scan_Flag2_x = 0
                Scan_Flag3_x = 0
                Scan_Flag1_y = 0
                Scan_Flag2_y = 0
                Scan_Flag3_y = 0
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
            cv2.imshow('Camera Window2', dilate)
            #cv2.imshow('Camera Window3', frame2)

        #어차피 IR 카메라 FPS 30이어서 한 프레임 읽는데 30ms 넘게 걸림 waitkey 30
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
 
    cap2.release()
    cv2.destroyAllWindows()
