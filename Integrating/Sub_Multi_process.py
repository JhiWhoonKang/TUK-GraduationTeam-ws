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

#사람 추적에서 픽셀 단위 에러값
RGB_Com_X = Value('i', 0)
RGB_Com_Y = Value('i', 0)

#레이저 추적에서 픽셀 단위 에러값
IR_Com_X = Value('i', 0)
IR_Com_Y = Value('i', 0)

#TOF 거리 데이터 공유 변수
target_distance = Value('i', 0)

#조이스틱 버튼 데이터 공유 변수
Button_Command = Value('i', 0)

#권한 데이터 공유 변수
request_access = Value('i', 0)

################################################################################
#메인틴지 연결
try:
    teensy = serial.Serial('/dev/ttyACM1', 500000, timeout = 0)
except IOError as e:
    print(e)
    print("Teensy 어디")
    exit()
    
#클래스
#모터
m3 = M3(teensy)
m4 = M4(teensy)
m5 = M5()
#광학
optical = Optical()
#기총
gun = Gun()
#RF통신모듈
rf = RF()
gun.Initialization(teensy)

#TOF 데이터 보내라는 명령어
def read_dist(canid):
    packet = optical.PollingDistance()
    teensy.write(packet)            

################################################################################
#엔코더값으로 각도 환산한 광학부, Z축, 기총부 각도 저장 전역변수
Optical_angle = 0
Body_angle = 0
Gun_angle = 0

#TCP/IP로 각도 값 전해줄 때 변할 때만 보내주게 이전 각도 저장 전역 변수
pre_Optical_angle = 0
pre_Body_angle = 0

#기총발사 버튼 한 번에 중복 안되게 하는 변수
gun_trig = 0
#기총 조준 시퀀스 조절 변수
aming_seq = 0
    
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
            data = client_socket1.recv(16, socket.MSG_DONTWAIT)
            if len(data) == 16:
                ds = struct.unpack('2iI2h', data)
                if ds[2] & 0xfc000000 == 4227858432:
                    joy.axisX = ds[0]
                    joy.axisY = ds[1]
                    joy.button = ds[2]
                    if (ds[2] & 0x00000100) == 0x00000100:
                        C_LR.value = 1
                    elif (ds[2] & 0x00000200) == 0x00000200:
                        C_LR.value = 2
                    C_X.value = ds[3]
                    C_Y.value = ds[4]
                print(joy.axisX, ", ", joy.axisY, ", ", joy.axisT, ", ", joy.button)
        except BlockingIOError:
            p = 1

        if (pre_Optical_angle != Optical_angle or pre_Body_angle != Body_angle) and (time.time() - send_time >= 0.1):
            try:
                sd = struct.pack('4f', float(Optical_angle), Click.Left_or_right, joy.axisT, float(Body_angle))
                client_socket1.sendall(sd)
                pre_Optical_angle = Optical_angle
                pre_Body_angle = Body_angle
            except:
                pre_Optical_angle = Optical_angle
                pre_Body_angle = Body_angle
                print("failed Sending")

            send_time = time.time()

    joy.axisX = 0
    joy.axisY = 0
    joy.axisT = 0
    joy.button = 0xfc000004
    m3.Active_Motor_3(0)
    m4.Active_Motor_4(0)
    socket.close()

#TCP/IP 통신 대기 스레드
def connect_tcp():
    #TCP/IP 설정
    #TCP_host_IP = "10.254.2.96"
    TCP_host_IP = "192.168.0.29"
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
    p1 = Process(target=RGB_pro, args=(C_LR, C_X, C_Y, RGB_Com_X, RGB_Com_Y, target_distance, Button_Command, request_access,))
    p2 = Process(target=IR_pro, args=(IR_Com_X, IR_Com_Y, target_distance, Button_Command,))
    
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

    #TCP/IP 통신 대기 스레드 시작
    t1= threading.Thread(target=connect_tcp, args=())

    t1.daemon = True

    t1.start()
    
    #초기에 캘리하기 위해 포토 센서로 이동
    Motor_Calibrate(teensy, m4, m5)
    
    #메인 틴지 통신 처리
    while 1:
        # 엔코더값, TOF 데이터 보내라는 명령어를 꼬이지 않게 순차적, 주기적으로 보냄
        if time.time() - get_data_pre_time > 0.004:
            if get_data_seq == 0:
                read_encoder2(teensy, 3)
                get_data_seq = 1
            elif get_data_seq == 1:
                read_encoder2(teensy, 4)
                get_data_seq = 2
            elif get_data_seq == 2:
                read_encoder2(teensy, 5)
                get_data_seq = 3
            elif get_data_seq == 3:
                read_dist(6)
                get_data_seq = 0
            get_data_pre_time = time.time()

        if time.time() - RF_data_pre_time > 0.1:
            RF_data_pre_time = time.time()
            
        #게임패드가 연결되어 있으면
        if gamepad:
            get_gamepad_input(gamepad)
            
        #게임패드 버튼에 따른 모드 구분
        Button_Command.value = joy.button

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
        if (joy.button & 0x02) == 0x02 and aming_seq == 0:
            Gun_targ_Angle = Cal_Anlge_Aming_Target("RGB", target_distance.value, Optical_angle)
            m5.Active_Motor_5(teensy, Gun_angle, Gun_targ_Angle)
            aming_seq = 1

        if (joy.button & 0x02) == 0x00 and aming_seq == 1:
            aming_seq = 0

        if (joy.button & 0x01) == 0x01:
            if gun_trig == 0:
                teensy.write(gun.SetGunPower(1))
                teensy.write(gun.Trigger("fire"))
                gun_trig = 1

        if (joy.button & 0x01) == 0x00:
            if gun_trig == 1:
                teensy.write(gun.SetGunPower(0))
                teensy.write(gun.Trigger("ready"))
                gun_trig = 0

        if (joy.button & 0x80) == 0x80:
            Motor_Calibrate(teensy, m4, m5)
            
        #일단 2바이트 수신(CAN_ID, Length)
        data1 = teensy.read(2)
        if len(data1) > 0:
            #data length만큼 데이터 더 수신
            data2 = teensy.read(data1[1])
            ##To control Motor data
            #ID 3번
            if data1[0] == 2:
                rf.CheckPacket(data2)
                if rf.data3 == 2:
                    request_access.value = 1
                elif rf.data3 == 0:
                    request_access.value = 0
            elif data1[0] == 3:
                #모터 구동을 위한 데이터 처리
                if data2 == b'\xf6\x02\xfb' or data2 == b'\xf6\x00\xf9':
                    m3.M3_state = m3.M3_Stop
                    m3.M3_state_dir = 0
                #엔코더 데이터 처리
                elif data1[1] == 8:
                    Body_angle = -calculate_encoder(data2, 43.33333)
            #ID 4번
            elif data1[0] == 4:
                #모터 구동을 위한 데이터 처리
                if data2 == b'\xf6\x02\xfc' or data2 == b'\xf6\x00\xfa':
                    m4.M4_state = m4.M4_Stop
                    m4.M4_state_dir = 0
                #엔코더 데이터 처리
                elif data1[1] == 8:
                    Optical_angle = -calculate_encoder(data2, 10)
            #ID 5번
            elif data1[0] == 5:
                #엔코더 데이터 처리
                if data1[1] == 8:
                    Gun_angle = calculate_encoder(data2, 20)
                    #print(Gun_angle)
                else :
                    print(data2)
                    #\xfd\x01\x03 moving
                    #\xfd\x02\x04 complete
            ##To get ToF##
            #ID 6번
            elif data1[0] == 6:
                optical.CheckPacket(data2)
                target_distance.value = optical.distance
                
                
        #Z축 각도 -90 ~ 90 벗어나면 멈추기 시작
        if Body_angle < -90 and Active_M3_command < 0:
            m3.Active_Motor_3(0)
        elif Body_angle > 90 and Active_M3_command > 0:
            m3.Active_Motor_3(0)
        #안벗어 났으면 명령대로 모터 구동
        else:
            m3.Active_Motor_3(Active_M3_command)
            
        #광학부 각도 -30 ~ 30 벗어나면 멈추기 시작
        if Optical_angle < -30 and Active_M4_command < 0:
            m4.Active_Motor_4(0)
        elif Optical_angle > 30 and Active_M4_command > 0:
            m4.Active_Motor_4(0)
        #안벗어 났으면 명령대로 모터 구동
        else:
            m4.Active_Motor_4(Active_M4_command)
            
socket.close()
# device.close() 
