from multiprocessing import Process, Value
import tensorflow as tf
import tensorflow_hub as hub
import tensorflow_hub as keras
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
from Joy_Module import init_gamepad, joy, get_gamepad_input, gamepad
from Motor_Module import M3, M4, M5, set_current_axis_to_zero, go_home, read_encoder2, calculate_encoder, Cal_Anlge_Aming_Target, Motor_Calibrate
from Optical import Optical
from gun import Gun
from RF import RF
from RGB_Module import RGB_pro
from IR_Module import IR_pro

#멀티 프로세스에서 사용할 공유 변수들
#클릭 데이터
C_LR = Value('i', 3)
C_X = Value('i', 0)
C_Y = Value('i', 0)
#드래그 데이터
C_D_X_1 = Value('i', 0)
C_D_Y_1 = Value('i', 0)
C_D_X_2 = Value('i', 0)
C_D_Y_2 = Value('i', 0)

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

#모드 데이터 공유 변수
#0x00 수동조작
#0x01 오토스캔
#0x02 레이저 트래킹
#0x03 사람 트래킹
Mode_ = Value('i', 0)

#권한 요청 데이터 공유 변수
# 0 권한 요청 X
# 1 권한 요청 O
request_access = Value('i', 0)

#조준 완료 공유 변수
#0 조준 미완료
#1 조준 완료
aming_complete = Value('i', 0)

#사람 감지(트래킹 시작)공유 변수
#0 트래킹 X
#1 트래킹 시작
person_detected = Value('i', 0)

#RCWS 각도 정보 공유 변수
Optical_angle_ = Value('i', 0)
Body_angle_ = Value('i', 0)
Gun_angle_ = Value('i', 0)

#적외선 트래킹 때 조준됐는지에 대한 공유 변수
#0 조준 미완료
#1 조준 완료
IR_Armed = Value('i', 0)

#초병이랑 RF 통신 정보 공유 변수
#0x01 적외선 레이저 조사
#0x02 권한요청
#0x04 RCWS 가시광선 레이저 조사
#0x08 RCWS 실사격
Sentry_Communication = Value('i', 0)

#초병 방위각, 고각 공유 변수
Sentry_Azimuth = Value('i', 0)
Sentry_Elevation = Value('i', 0)

#줌 데이터 공유 변수
#0~4 줌 인덱스
Zoom = Value('i', 0)
################################################################################
#메인틴지 연결
try:
    teensy = serial.Serial('/dev/ttyACM2', 500000, timeout = 0)
except IOError as e:
    print(e)
    print("Teensy 어디")
    exit()
    
#클래스
#모터
m3 = M3(teensy)
m4 = M4(teensy)
m5 = M5(teensy)
#광학
optical = Optical()
#기총
gun = Gun()
#RF 통신
rf = RF()

#reset
optical.CameraTableSetup(teensy)
gun.TriggerTableSetup(teensy)

#RF 수신 데이터 클래스
class RF_R_D:
    Azimuth_Y:int = 0
    Azimuth_Z:int = 0
    dead:int = 0
    S_Check:int = 0

RF_R = RF_R_D()

#RF 송신 데이터
RF_S_D = 0x00

gun.Initialization(teensy)

#TOF 데이터 보내라는 명령어
def read_dist(canid):
    packet = optical.PollingDistance()
    teensy.write(packet)            

################################################################################
#엔코더값으로 각도 환산한 광학부, Z축, 기총부 각도 저장 전역변수
Optical_angle : float = 0
Body_angle : float = 0
Gun_angle : float = 0

#초병 권한
#0x00 초병 권한 X
#0x01 초병 권한 요청
#0x02 초병 권한 O
Sentry_Access = 0x00

#조준 여부 변수
#0x01 조준 완료
#그 외에 조준 미완료
Take_Aim = 0x00

#모드 저장 변수
#0x00 수동조작
#0x01 오토스캔
#0x02 레이저 트래킹
#0x03 사람 트래킹
Mode = 0x00

#TCP/IP로 각도 값 전해줄 때 변할 때만 보내주게 이전 각도 저장 전역 변수
pre_Optical_angle : float = 0
pre_Body_angle : float = 0
pre_Gun_angle : float = 0

#방아쇠 확인 변수
#1 방아쇠 당김
#0, 2 방아쇠 놓음
gun_trig = 0

#시퀀스 제어 때문에 사용하는 변수
aming_seq = 0

#수동/ 자동 조준
#0 자동 조준
#1 수동 조준
Manual_aming = 1

Manual_aming_seq = 0

#목표 기총 각도
Gun_targ_Angle = 0

#오토 스캔 관련 변수
#오토 스캔 시퀀스 제어 때문에 사용하는 변수
auto_scan_seq = 0
auto_scan_moving_seq = 0
auto_scan_dir = 0
auto_scan_angle = []
auto_scan_index = 0
auto_scan_stop_time = 0
auto_scan_complete_flag = 0
auto_tracking_flag = 0

#모드 변환 플래그
Mode_Change_Flag = 0

#RCWS 전원 변수
#0 전원 On
#1 전원 Off
Power_control = 0

#줌 플래그
Zoom_flag = 0
#줌, 포커스 인덱스 변수
Zoom_Focus_Index = 0

################################################################################
# TCP/IP로 클릭 데이터 받기 위한 클래스
class click_data:
    Left_or_right:int = 3
    X:int = 0
    Y:int = 0
Click = click_data()

#TCP/IP data
def Get_Send_data(client_socket1, addr1):
    global joy, Button_Command, Click, Optical_angle, pre_Optical_angle, Body_angle, pre_Body_angle
    send_time = 0
    while 1:
        try:
            data = client_socket1.recv(24, socket.MSG_DONTWAIT)
            if len(data) == 24:
                ds = struct.unpack('2iI6h', data)
                if ds[2] & 0xfc000000 == 4227858432:
                    joy.axisX = ds[0]
                    joy.axisY = ds[1]
                    joy.button = ds[2]
                    if (ds[2] & 0x00001000) == 0x00001000:
                        C_LR.value = 1
                    elif (ds[2] & 0x00002000) == 0x00002000:
                        C_LR.value = 2
                    C_X.value = ds[3]
                    C_Y.value = ds[4]
                    C_D_X_1.value = ds[5]
                    C_D_Y_1.value = ds[6]
                    C_D_X_2.value = ds[7]
                    C_D_Y_2.value = ds[8]
        except BlockingIOError:
            p = 1

        if (pre_Optical_angle != Optical_angle or pre_Body_angle != Body_angle) and (time.time() - send_time >= 0.3):
            try:
                sd = struct.pack('6f4B', float(round(Optical_angle, 2)), float(round(Gun_angle, 2)), float(round(Body_angle, 2)), float(round(Sentry_Azimuth.value, 2)), float(round(Sentry_Elevation.value, 2)), float(round(optical.distance, 2)), Sentry_Access, Take_Aim, gun_trig, Mode)
                client_socket1.sendall(sd)
                pre_Optical_angle = Optical_angle
                pre_Body_angle = Body_angle
            except:
                pre_Optical_angle = Optical_angle
                pre_Body_angle = Body_angle

            send_time = time.time()

    joy.axisX = 0
    joy.axisY = 0
    joy.axisT = 0
    joy.button = 0xfc100000
    m3.Active_Motor_3(0)
    m4.Active_Motor_4(0)
    m5.Active_Motor_5_Speed(0)
    time.sleep(1)
    socket.close()

def connect_tcp():
    #TCP/IP 설정
    TCP_host_IP = "192.168.0.30"
    TCP_host_port = 7000

    TCP = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    TCP.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 0)

    TCP.bind((TCP_host_IP, TCP_host_port))

    TCP.listen(1)
    while 1:
        client_socket1, addr1 = TCP.accept()
        print("success1")

        time.sleep(1)
    
        if client_socket1 is not None:
            t2= threading.Thread(target=Get_Send_data, args=(client_socket1, addr1))

            t2.daemon = True

            t2.start()
        
                     
        
################################################################################
################################################################################
#메인
if __name__ == '__main__':
    #프로세스 설정
    p1 = Process(target=RGB_pro, args=(C_LR, C_X, C_Y, RGB_Com_X, RGB_Com_Y, target_distance, Button_Command, request_access, aming_complete, person_detected, Optical_angle_, Body_angle_, Gun_angle_, Mode_, Zoom, C_D_X_1, C_D_Y_1, C_D_X_2, C_D_Y_2, ))
    p2 = Process(target=IR_pro, args=(IR_Com_X, IR_Com_Y, target_distance, Mode_, IR_Armed, Sentry_Communication, Sentry_Azimuth, Sentry_Elevation, Optical_angle_, Body_angle_, ))
    
    #프로세스 시작
    p1.start()
    p2.start()
    
    #엔코더, TOF 값들을 보내라는 명령을 보내는데 한 번에 받아서 데이터 꼬이지 않게 시퀀스 만들기 위한 변수
    get_data_pre_time = 0
    get_data_seq = 0
    
    RF_data_pre_time = 0
    
    #최종적인 모터를 구동하는 명령 변수
    Active_M3_command = 0
    Active_M4_command = 0
    Active_M5_command = 0

    t1= threading.Thread(target=connect_tcp, args=())

    t1.daemon = True

    t1.start()

    #초기에 캘리하기 위해 포토 센서로 이동
    aming_complete.value = 2
    #Motor_Calibrate(teensy, m3, m4, m5)
    aming_complete.value = 0

    
    #teensy.write(optical.UseFixedCameraTable(0))

    time.sleep(1)
    
    #teensy.write(gun.SetGunPower(1))
    
    #메인 틴지 통신 처리
    while 1:
        # 엔코더값, TOF 데이터 보내라는 명령어를 꼬이지 않게 순차적, 주기적으로 보냄(4ms 주기 요청)
        if time.time() - get_data_pre_time > 0.006 and Power_control == 0:
            if get_data_seq == 0:
                #CAN ID 3번 모터 엔코더 값 요청
                read_encoder2(teensy, 3)
                get_data_seq = 1
            elif get_data_seq == 1:
                #CAN ID 4번 모터 엔코더 값 요청
                read_encoder2(teensy, 4)
                get_data_seq = 2
            elif get_data_seq == 2:
                #CAN ID 5번 모터 엔코더 값 요청
                read_encoder2(teensy, 5)
                get_data_seq = 3
            elif get_data_seq == 3:
                #라이다값 요청
                read_dist(6)
                get_data_seq = 0
            get_data_pre_time = time.time()

        if time.time() - RF_data_pre_time > 0.1:
            RF_data_pre_time = time.time()
            
        #게임패드가 연결되어 있으면
        if gamepad:
            get_gamepad_input(gamepad)

        #멀티 프로세스로 넘기기 위해 저장
        Optical_angle_.value = int(Optical_angle)
        Body_angle_.value = int(Body_angle)
        Gun_angle_.value = int(Gun_angle)
        Button_Command.value = joy.button
        Mode_.value = Mode
        Zoom.value = Zoom_Focus_Index

        #사람 트래킹 명령이 있으면 트래킹 모드로 변환
        if person_detected.value == 1:
            Mode = 0x03

        #수동모드
        if Mode == 0x00:
            #모터 명령을 조이스틱 값으로 저장
            Active_M3_command = joy.axisX
            Active_M4_command = joy.axisY
            #RGB 카메라 기준으로 기총 목표 각도 계산
            Gun_targ_Angle = Cal_Anlge_Aming_Target("RGB", target_distance.value, Optical_angle)
        #오토스캔 모드
        #한 전체적인 시퀀스 지날 때 마다 인덱스 하나씩 늘리고
        #다 지나면 하나씩 빼고
        #반복
        elif Mode == 0x01:
            #오토스캔 인덱스가 0 이상일 때
            if auto_scan_index > 0:
                #해당 인덱스에 저장된 Z축 각도가 오른쪽에 있을 때
                if auto_scan_angle[auto_scan_moving_seq][0] - Body_angle > 0.1:
                    #Z축 속도 명령 15로
                    Active_M3_command = 15
                #해당 인덱스에 저장된 Z축 각도가 왼쪽에 있을 때
                elif auto_scan_angle[auto_scan_moving_seq][0] - Body_angle < -0.1:
                    #Z축 속도 명령 -15로
                    Active_M3_command = -15
                #해당 인덱스에 저장된 Z축 각도와 차이가 0.1도 내에 있을 때
                elif abs(auto_scan_angle[auto_scan_moving_seq][0] - Body_angle) <= 0.1:
                    #정지
                    Active_M3_command = 0
                    #다음 시퀀스로 넘기기 위해서 플래그
                    if auto_scan_complete_flag != 0x100:
                        auto_scan_complete_flag = (auto_scan_complete_flag | 0x001)

                #해당 인덱스에 저장된 Y축 각도가 아래에 있을 때
                if auto_scan_angle[auto_scan_moving_seq][1] - Optical_angle > 0.1:
                    #Y축 속도 명령 5로
                    Active_M4_command = 5
                #해당 인덱스에 저장된 Y축 각도가 위에 있을 때
                elif auto_scan_angle[auto_scan_moving_seq][1] - Optical_angle < -0.1:
                    #Y축 속도 명령 -5로
                    Active_M4_command = -5
                #해당 인덱스에 저장된 Y축 각도와 차이가 0.1도 내에 있을 때
                elif abs(auto_scan_angle[auto_scan_moving_seq][1] - Optical_angle) <= 0.1:
                    #정지
                    Active_M4_command = 0
                    #다음 시퀀스로 넘기기 위해서 플래그
                    if auto_scan_complete_flag != 0x100:
                        auto_scan_complete_flag = (auto_scan_complete_flag | 0x010)

                #Z축, Y축 플래그가 모두 생기면
                if (auto_scan_complete_flag & 0x11) == 0x11:
                    #플래그 시작 시간 저장
                    auto_scan_stop_time = time.time()
                    #다음 시퀀스로 넘어가기 위해서 플래그
                    auto_scan_complete_flag = (auto_scan_complete_flag & ~(0x011))
                    auto_scan_complete_flag = (auto_scan_complete_flag | 0x100)

                #시간 저장 후 2초 경과 후
                if(time.time() - auto_scan_stop_time) > 2 and auto_scan_complete_flag == 0x100:
                    #시계방향으로 가고 있고 저장된 오토스캔 좌표 개수가 현재 진행중인 인덱스보다 크면
                    if auto_scan_index - 1 > auto_scan_moving_seq and auto_scan_dir == 0:
                        #계속 가야되니까 다음 시퀀스 += 1
                        auto_scan_moving_seq += 1
                    #반시계방향으로 가고 있고 현재 진행중인 인덱스가 0보다 크면
                    elif auto_scan_moving_seq > 0 and auto_scan_dir == 1:
                        #계속 가야되니까 다음 시퀀스 -= 1
                        auto_scan_moving_seq -= 1
                    #시계 방향으로 가고 있고 저장된 오토스캔 좌표 개수랑 현재 진행중인 인덱스랑 같으면
                    #(시계방향으로 다 돌았으면)
                    elif auto_scan_index - 1 == auto_scan_moving_seq and auto_scan_dir == 0:
                        #거꾸로 돌아가야되니까 방향 바꾸기
                        auto_scan_dir = 1
                    #시계 방향으로 가고 있고 저장된 오토스캔 좌표 개수랑 현재 진행중인 인덱스랑 같으면
                    #(반시계방향으로 다 돌았으면)
                    elif auto_scan_moving_seq == 0 and auto_scan_dir == 1:
                        #거꾸로 돌아가야되니까 방향 바꾸기
                        auto_scan_dir = 0
                    #플래그 제거
                    auto_scan_complete_flag = (auto_scan_complete_flag & ~(0x100))
            #RGB 카메라 기준으로 기총 목표 각도 계산
            Gun_targ_Angle = Cal_Anlge_Aming_Target("RGB", target_distance.value, Optical_angle)
        #트래킹 모드
        elif Mode == 0x03:
            #영상처리로 나온 모터 제어 명령
            Active_M3_command = RGB_Com_X.value
            Active_M4_command = RGB_Com_Y.value
            #RGB 카메라 기준으로 기총 목표 각도 계산
            Gun_targ_Angle = Cal_Anlge_Aming_Target("RGB", target_distance.value, Optical_angle)
        elif Mode == 0x02:
            #영상처리로 나온 모터 제어 명령
            if target_distance.value >= 200:
                Active_M3_command = IR_Com_X.value
                Active_M4_command = IR_Com_Y.value
            else:
                Active_M3_command = 0
                Active_M4_command = 0
            #IR 카메라 기준으로 기총 목표 각도 계산
            Gun_targ_Angle = Cal_Anlge_Aming_Target("IR", target_distance.value, Optical_angle)

        #조이스틱 트리거 당겼을 때
        #초병 권한 있고 발사 명령 있을 때
        if (joy.button & 0x01) == 0x01 or (((Sentry_Communication.value & 0x04) == 0x04 or (Sentry_Communication.value & 0x08) == 0x08) and Mode == 0x02):
            #발사 명령 계속 보내지 않도록 하기 위해 gun_trig == 0일 때 (발사 중이 아닐 때)
            if gun_trig == 0:
                #RCWS 전원 켜져있을 때만
                if Power_control == 0:
                    #RCWS 레이저 발사
                    teensy.write(gun.SetGunPower(1))
                #조준이 됐고
                #조이스틱 조준 버튼이랑 동시에 눌렀거나
                #초병 권한이 있고 실사격 버튼 눌렀을 때만
                if ((joy.button & 0x02) == 0x02 or((Sentry_Communication.value & 0x08) == 0x08 and Mode == 0x02)) and Take_Aim == 0x01:
                    #RCWS 전원 켜져있을 때만
                    if Power_control == 0:
                        #실사격 방아쇠 당기기
                        teensy.write(gun.Trigger("fire"))
                #아니면
                else:
                    #RCWS 전원 켜져있을 때만
                    if Power_control == 0:
                        #실사격 방아쇠 놓기
                        teensy.write(gun.Trigger("ready"))
                #사격 중
                gun_trig = 1
            #사격 중에
            elif gun_trig == 1:
                #조준이 되지 않았으면
                if Take_Aim != 0x01:
                    #RCWS 전원 켜져있을 때만
                    if Power_control == 0:
                        #실사격 방아쇠 놓기
                        teensy.write(gun.Trigger("ready"))
                    #방아쇠 놓은 상태
                    gun_trig = 2
            #조준이 다시 됐으면 방아쇠를 다시 당기든 놓든 플래그처럼 0으로 설정
            elif gun_trig == 2 and Take_Aim == 0x01:
                #방아쇠 놓은 상태
                gun_trig = 0
        #조이스틱 트리거 놓았을 때
        elif (joy.button & 0x01) == 0x00:
            #발사중이거나 발사하다가 트리거 놓았을 때
            if gun_trig == 1 or gun_trig == 2:
                #RCWS 전원 켜져있을 때만
                if Power_control == 0:
                    #레이저 끄고 방아쇠 놓기
                    teensy.write(gun.SetGunPower(0))
                    teensy.write(gun.Trigger("ready"))
                gun_trig = 0

        #수동모드 버튼 눌렀을 때
        if (joy.button & 0x04) == 0x04 and (joy.button & 0x38) == 0x00:
            if Mode_Change_Flag == 0:
                #초병 권한 풀기
                Sentry_Access = 0x00
                #트래킹 풀기
                person_detected.value = 0
                #수동모드 전환
                Mode = 0x00
                Mode_Change_Flag = 1
        elif (joy.button & 0x08) == 0x08 and (joy.button & 0x34) == 0x00:
            if Mode_Change_Flag == 0:
                #초병 권한 풀기
                Sentry_Access = 0x00
                #트래킹 풀기
                person_detected.value = 0
                #오토스캔 모드 변환
                Mode = 0x01
                Mode_Change_Flag = 1
        elif (joy.button & 0x10) == 0x10 and (joy.button & 0x2c) == 0x00:
            if Mode_Change_Flag == 0:
                #초병 권한 풀기
                Sentry_Access = 0x00
                #트래킹 모드 변환
                Mode = 0x03
                Mode_Change_Flag = 1
        elif (joy.button & 0x20) == 0x20 and (joy.button & 0x1c) == 0x00:
            if Mode_Change_Flag == 0:
                #초병 권한 설정
                Sentry_Access = 0x02
                #트래킹 풀기
                person_detected.value = 0
                #초병 권한 모드 변환
                Mode = 0x02
                Mode_Change_Flag = 1
        else:
            Mode_Change_Flag = 0
        
        #자동 조준 모드 설정일 때
        if (joy.button & 0x40) == 0x40:
            if Manual_aming == 1:
                #수동 조준 -> 자동 조준
                Manual_aming = 0
        #수동 조준 모드 설정일 때
        elif (joy.button & 0x40) == 0x00:
            if Manual_aming == 0:
                #RCWS 전원 켜져있을 때만
                #자동 조준일 때 움직이던 명령이 있을 수 있어서 멈추는 명령 보내주기
                if Power_control == 0:
                    m5.Active_Motor_5_Speed(0)
                #자동 조준-> 수동 조준
                Manual_aming = 1

        #캘리브레이션 버튼 눌렀을 때
        if (joy.button & 0x80) == 0x80:
            aming_complete.value = 2
            #RCWS 전원 켜져있을 때만
            if Power_control == 0:
                #캘리브레이션 진행
                Motor_Calibrate(teensy, m3, m4, m5)
            aming_complete.value = 0


        #오토스캔 좌표 저장 버튼
        if (joy.button & 0x400) == 0x400:
            #플래그
            if auto_scan_seq == 0:
                #오토스캔 좌표 저장 변수에 리스트로 저장 [0]->Z축 좌표, [1]->Y축 좌표
                auto_scan_angle.append([])
                auto_scan_angle[auto_scan_index].append(Body_angle)
                auto_scan_angle[auto_scan_index].append(Optical_angle)
                #인덱스 크기 저장 변수 += 1
                auto_scan_index += 1
                #플래그
                auto_scan_seq = 1
        elif (joy.button & 0x400) == 0x000:
            #플래그
            auto_scan_seq = 0

        #오토스캔 좌표 초기화 버튼
        if (joy.button & 0x800) == 0x800:
            #오토스캔 좌표가 저장된게 있을 때 다 초기화
            if auto_scan_index > 0:
                auto_scan_angle = []
                auto_scan_index = 0
                auto_scan_moving_seq = 0
                auto_scan_complete_flag = 0x000
                Active_M3_command = 0
                Active_M4_command = 0

        #줌 인 버튼
        if (joy.button & 0x40000) == 0x40000 and Zoom_flag == 0:
            #최대 줌 인 인덱스보다 더 줌 인 할 수 있으면
            if Zoom_Focus_Index + 1 <= 4:
                #줌 인
                Zoom_Focus_Index += 1
                if Power_control == 0:
                    teensy.write(optical.UseFixedCameraTable(Zoom_Focus_Index))
                Zoom_flag = 1
        #줌 아웃 버튼
        elif (joy.button & 0x20000) == 0x20000 and Zoom_flag == 0:
            #최대 줌 아웃 인덱스보다 더 줌 아웃 할 수 있으면
            if Zoom_Focus_Index - 1 >= 0:
                #줌 아웃
                Zoom_Focus_Index -= 1
                if Power_control == 0:
                    teensy.write(optical.UseFixedCameraTable(Zoom_Focus_Index))
                Zoom_flag = 1
        elif (joy.button & 0x60000) == 0x00000:
            Zoom_flag = 0

        #RCWS 전원 키라는 명령
        if (joy.button & 0x100000) == 0x100000:
            #RCWS 전원 꺼져있을 때
            if Power_control == 1:
                #기총, 카메라 줌, 초점 초기 설정
                gun_trig = 0
                teensy.write(gun.SetGunPower(0))
                teensy.write(gun.Trigger("ready"))
                time.sleep(1)
                
                optical = Optical()
                optical.CameraTableSetup(teensy)
                time.sleep(1)
                Zoom_Focus_Index = 0
                teensy.write(optical.UseFixedCameraTable(0))
                time.sleep(3)
                
                #키라는 명령
                packet = [1, 1, 1]
                teensy.write(bytearray(packet))
                Power_control = 0
        #RCWS 전원 끄라는 명령
        elif (joy.button & 0x100000) == 0x000000:
            #RCWs 전원 켜져 있을 때
            if Power_control == 0:
                #기총, 카메라 줌, 초점 초기 설정
                gun_trig = 0
                teensy.write(gun.SetGunPower(0))
                teensy.write(gun.Trigger("ready"))
                time.sleep(1)
                
                optical = Optical()
                optical.CameraTableSetup(teensy)
                time.sleep(1)
                Zoom_Focus_Index = 0
                teensy.write(optical.UseFixedCameraTable(0))
                time.sleep(3)

                #끄라는 명령
                packet = [1, 1, 0]
                teensy.write(bytearray(packet))
                Power_control = 1

        #목표 기총 각도, 현재 기총 각도 차이가 0.03도보다 작을 때
        if abs(Gun_targ_Angle - Gun_angle) < 0.03 and aming_complete.value != 2:
            #조준 완료
            aming_complete.value = 1
            Take_Aim = 0x01
        #목표 기총 각도, 현재 기총 각도 차이가 0.03도보다 클 때
        elif abs(Gun_targ_Angle - Gun_angle) >= 0.03 and aming_complete.value != 2:
            #조준 미완료
            aming_complete.value = 0
            if Manual_aming == 1:
                Take_Aim = 0x02
            elif Manual_aming == 0:
                Take_Aim = 0x00
            
        #일단 2바이트 수신(CAN_ID, Length)
        data1 = teensy.read(2)
        if len(data1) == 2:
            #data length만큼 데이터 더 수신
            data2 = teensy.read(data1[1])
            ##To control Motor data
            #ID 3번
            if len(data2) == int(data1[1]):
                if data1[0] == 2:
                    #print(data2)
                    rf.CheckPacket(data2)
                    if len(data2) == 8:
                        ds_rf = struct.unpack('2h2H', data2)
                        #초병 Y,Z축 각도 저장
                        RF_R.Azimuth_Y = -ds_rf[0]
                        RF_R.Azimuth_Z = -ds_rf[1]
                        RF_R.dead = ds_rf[2]
                        RF_R.S_Check = ds_rf[3]

                        #멀티 프로세스에 초병 각도 넘겨주기 위해 저장
                        Sentry_Azimuth.value = RF_R.Azimuth_Z
                        Sentry_Elevation.value = RF_R.Azimuth_Y

                        #초병 상호작용 데이터 멀티 프로세스에 넘기기 위해 저장
                        Sentry_Communication.value = RF_R.S_Check

                        #초병 권한 요청할 때
                        if (RF_R.S_Check & 0x0002) == 0x0002:
                            #초병 권한 요청 변수에 저장
                            request_access.value = 1
                            Sentry_Access = 0x01
                        #초병 권한 요청 풀었을 때
                        elif (RF_R.S_Check & 0x0002) == 0x0000:
                            #초병 권한 요청 변수에 저장
                            request_access.value = 0
                            if Mode != 0x02:
                                Sentry_Access = 0x00
                            elif Mode == 0x02:
                                Sentry_Access = 0x02

                    #초병 권한이 아닐 때
                    if Mode != 0x02:
                        #홀로그램에 초병 권한 아니라는 데이터 넘겨주기 위해 RF 통신으로 권한이 없다는 것을 송신
                        RF_S_D = (RF_S_D & ~(0x01))
                    #초병 권한일 때
                    elif Mode == 0x02:
                        #홀로그램에 초병 권한이라는 데이터 넘겨주기 위해 RF 통신으로 권한이 있다는 것을 송신
                        RF_S_D = RF_S_D | 0x01

                    #기총 발사중일 때
                    if gun_trig == 1:
                        #홀로그램에 발사 중이라는 데이터 넘겨주기 위해 RF 통신으로 발사 중이라는 것을 송신
                        RF_S_D = RF_S_D | 0x02
                    #기총 발사중 아닐 때
                    elif gun_trig != 1:
                        #홀로그램에 발사 중 아니라는 데이터 넘겨주기 위해 RF 통신으로 발사 중 아니라는 것을 송신
                        RF_S_D = (RF_S_D & ~(0x02))

                    #기총 조준 완료 됐을 때
                    if IR_Armed.value == 1 and Take_Aim == 0x01:
                        #홀로그램에 기총 조준 완료됐다는 데이터 넘겨주기 위해 RF 통신으로 기총 조준 완료됐다는 것을 송신
                        RF_S_D = RF_S_D | 0x04
                    #기총 조준 미완료일 때
                    elif IR_Armed.value == 0:
                        #홀로그램에 기총 조준 미완료됐다는 데이터 넘겨주기 위해 RF 통신으로 기총 조준 미완료됐다는 것을 송신
                        RF_S_D = (RF_S_D & ~(0x04))

                    #명령 송신
                    sd_rf = struct.pack('6B', 2, 4, 0, 0, 0, RF_S_D)
                    teensy.write(bytearray(sd_rf))
                #ID 3번
                elif data1[0] == 3:
                    #모터 구동을 위한 데이터 처리
                    if data2 == b'\xf6\x02\xfb' or data2 == b'\xf6\x00\xf9':
                        m3.M3_state = m3.M3_Stop
                        m3.M3_state_dir = 0
                    #엔코더 데이터 처리
                    elif data1[1] == 8:
                        if calculate_encoder(data2, 40) is not None:
                            Body_angle = -calculate_encoder(data2, 40)
                #ID 4번
                elif data1[0] == 4:
                    #모터 구동을 위한 데이터 처리
                    if data2 == b'\xf6\x02\xfc' or data2 == b'\xf6\x00\xfa':
                        m4.M4_state = m4.M4_Stop
                        m4.M4_state_dir = 0
                    #엔코더 데이터 처리
                    elif data1[1] == 8:
                        if calculate_encoder(data2, 10) is not None:
                            Optical_angle = -calculate_encoder(data2, 10)
                #ID 5번
                elif data1[0] == 5:
                    #엔코더 데이터 처리
                    if data2 == b'\xf6\x02\xfd' or data2 == b'\xf6\x00\xfb':
                        m5.M5_state = m5.M5_Stop
                        m5.M5_state_dir = 0
                    if data1[1] == 8:
                        if calculate_encoder(data2, 20) is not None:
                            Gun_angle = calculate_encoder(data2, 20)
                ##To get ToF##
                #ID 6번
                elif data1[0] == 6:
                    optical.CheckPacket(data2)
                    if optical.distance >= 150 and optical.distance < 4500:
                        target_distance.value = optical.distance
        elif len(data1) != 2 and len(data1) != 0:
            print(data1)
                
        #RCWS 전원 On 일 때
        if Power_control == 0:
            #Z축 각도 제한 벗어나면 멈추기 시작
            if Body_angle < -55 and Active_M3_command < 0:
                m3.Active_Motor_3(0)
            elif Body_angle > 30 and Active_M3_command > 0:
                m3.Active_Motor_3(0)
            #안벗어 났으면 명령대로 모터 구동
            else:
                m3.Active_Motor_3(Active_M3_command)
            
            #광학부 각도 제한 벗어나면 멈추기 시작
            if Optical_angle < -2 and Active_M4_command < 0:
                m4.Active_Motor_4(0)
            elif Optical_angle > 30 and Active_M4_command > 0:
                m4.Active_Motor_4(0)
            #안벗어 났으면 명령대로 모터 구동
            else:
                m4.Active_Motor_4(Active_M4_command)

            #수동 조준 모드 아닐 때, 목표 거리가 100cm 이상일 때
            if Manual_aming == 0 and target_distance.value >= 100:
                #기총 모터 게인
                ga = -35
                #자료형 때문에 오류 방지
                if Gun_angle is not None:
                    #목표 기총 각도가 현재 각도보다 클 때
                    if float(Gun_targ_Angle) >= float(Gun_angle):
                        Active_M5_command = ga * (float(Gun_targ_Angle) - float(Gun_angle))
                    #목표 기총 각도가 현재 각도보다 작을 때
                    elif float(Gun_targ_Angle) < float(Gun_angle):
                        Active_M5_command = ga * (float(Gun_targ_Angle) - float(Gun_angle))

                    #목표 각도랑 차이가 0.03도 보다 작고 0.001도 보다 클 때
                    #게인 값이 크면 동작이 안돼서
                    #정밀제어하기 위해 최소 속도값으로 구동
                    if float(Gun_targ_Angle) - float(Gun_angle) <= 0.03 and float(Gun_targ_Angle) - float(Gun_angle) > 0.001:
                        Active_M5_command = -1
                    elif float(Gun_targ_Angle) - float(Gun_angle) >= -0.03 and float(Gun_targ_Angle) - float(Gun_angle) < -0.001:
                        Active_M5_command = 1

                    #최대, 최소 속도 제한
                    if Active_M5_command > 100:
                        Active_M5_command = 100
                    elif Active_M5_command < -100:
                        Active_M5_command = -100

                    #기총 각도 제한
                    if Gun_angle < -20 and Active_M5_command > 0:
                        m5.Active_Motor_5_Speed(0)
                    elif Gun_angle > 30 and Active_M5_command < 0:
                        m5.Active_Motor_5_Speed(0)
                    #안벗어 났으면 명령대로 모터 구동
                    else:
                        m5.Active_Motor_5_Speed(int(Active_M5_command))
                else:
                    m5.Active_Motor_5_Speed(0)
            #수동 조준 모드이고, 목표 거리가 100cm 이상일 때
            elif Manual_aming == 1 and target_distance.value >= 100:
                #자료형 때문에 오류 방지
                if Gun_angle is not None:
                    #조이스틱 조준 버튼 눌렀을 때
                    if (joy.button & 0x02) == 0x02 and aming_seq == 0:
                        #계산한 각도만큼 펄스 계산해서 구동 명령
                        m5.Active_Motor_5(teensy, Gun_angle, Gun_targ_Angle)
                        aming_seq = 1
                    elif (joy.button & 0x02) == 0x00 and aming_seq == 1:
                        aming_seq = 0
                else:
                    m5.Active_Motor_5_Speed(0)
            
socket.close()
# device.close() 
