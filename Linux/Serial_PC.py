import pygame
import serial
import time
from dataclasses import dataclass

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

start_up_3 = 0

ACC3 = 240

pre_3_Speed = 0

M3_state_dir = 0

M3_Stop = b'\x03\x03\xf6\x02\xfb'
M3_Move = b'\x03\x03\xf6\x01\xfa'
M3_state = b'\x03\x03\xf6\x02\xfb'

CRCBYTE3 = (0x3 + 0xF6 + ACC3) & 0xFF
packet3 = [3, 5, 0xF6, 0, 0, ACC3, CRCBYTE3]


start_up_4 = 0

ACC4 = 100

pre_4_Speed = 0

M4_state_dir = 0

M4_Stop = b'\x04\x03\xf6\x02\xfc'
M4_Move = b'\x04\x03\xf6\x01\xfb'
M4_state = b'\x04\x03\xf6\x02\xfc'

CRCBYTE4 = (0x4 + 0xF6 + ACC4) & 0xFF
packet4 = [4, 5, 0xF6, 0, 0, ACC4, CRCBYTE4]

class joy_data:
    axisX:int = 0
    axisY:int = 0
    axisT:int = 0
    button:int = 0xfc000004
joy = joy_data()

try:
    teensy = serial.Serial('/dev/ttyACM0', 115200, timeout = 0)
except IOError as e:
    print(e)
    print("Teensy 어디")
    exit()
    
def Go_to_Speed_Zero_3():
    global ACC3, CRCBYTE3, packet3
    CRCBYTE3 = (0x3 + 0xF6 + ACC3) & 0xFF
    packet3 = [3, 5, 0xF6, 0, 0, ACC3, CRCBYTE3]
    
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

def Active_Motor_3(Speed):
    global ACC3, pre_3_Speed, M3_state_dir, M3_state, M3_Stop, M3_Move, CRCBYTE3, packet3, start_up_3
    if Speed != pre_3_Speed or (Speed != 0 and M3_state == M3_Stop):
        start_up_3 = 1
        if Speed == 0:
            Go_to_Speed_Zero_3()
        elif Speed > 15:
            if (M3_state == M3_Stop) or (M3_state_dir == 1 and M3_state == M3_Move):
                Go_to_target_Speed_3(Speed)
                M3_state_dir = 1
            elif (M3_state == M3_Move and M3_state_dir == -1) or (M3_state == M3_Stop and M3_state_dir == -1):
                Go_to_Speed_Zero_3()
        elif Speed > 0 and Speed <= 15:
            if (pre_3_Speed > 15) or (M3_state == M3_Move and M3_state_dir == -1) or (M3_state == M3_Stop and M3_state_dir == -1):
                Go_to_Speed_Zero_3()
            elif (M3_state == M3_Stop) or (M3_state_dir == 1 and M3_state == M3_Move):
                Go_to_target_Speed_3(Speed)
                M3_state_dir = 1
        elif Speed < -15:
            if (M3_state == M3_Stop) or (M3_state_dir == -1 and M3_state == M3_Move):
                Go_to_target_Speed_3(Speed)
                M3_state_dir = -1
            elif (M3_state == M3_Move and M3_state_dir == 1) or (M3_state == M3_Stop and M3_state_dir == 1):
                Go_to_Speed_Zero_3()
        elif Speed < 0 and Speed >= -15:
            if (pre_3_Speed < -15) or (M3_state == M3_Move and M3_state_dir == 1) or (M3_state == M3_Stop and M3_state_dir == 1):
                Go_to_Speed_Zero_3()
            elif (M3_state == M3_Stop) or (M3_state_dir == -1 and M3_state == M3_Move):
                Go_to_target_Speed_3(Speed)
                M3_state_dir = -1
        teensy.write(bytearray(packet3))
        pre_3_Speed = Speed
    elif Speed == 0 and M3_state == M3_Move and start_up_3 == 1:
        Go_to_Speed_Zero_3()
        teensy.write(bytearray(packet3))
        start_up_3 = 0
            
       
def Go_to_Speed_Zero_4():
    global ACC4, CRCBYTE4, packet4
    CRCBYTE4 = (0x4 + 0xF6 + ACC4) & 0xFF
    packet4 = [4, 5, 0xF6, 0, 0, ACC4, CRCBYTE4]
    
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

def Active_Motor_4(Speed):
    global ACC4, pre_4_Speed, M4_state_dir, M4_state, M4_Stop, M4_Move, CRCBYTE4, packet4, start_up_4
    if Speed != pre_4_Speed or (Speed != 0 and M4_state == M4_Stop):
        start_up_4 = 1
        if Speed == 0:
            Go_to_Speed_Zero_4()
        elif Speed > 15:
            if (M4_state == M4_Stop) or (M4_state_dir == 1 and M4_state == M4_Move):
                Go_to_target_Speed_4(Speed)
                M4_state_dir = 1
            elif (M4_state == M4_Move and M4_state_dir == -1) or (M4_state == M4_Stop and M4_state_dir == -1):
                Go_to_Speed_Zero_4()
        elif Speed > 0 and Speed <= 15:
            if (pre_4_Speed > 15) or (M4_state == M4_Move and M4_state_dir == -1) or (M4_state == M4_Stop and M4_state_dir == -1):
                Go_to_Speed_Zero_4()
            elif (M4_state == M4_Stop) or (M4_state_dir == 1 and M4_state == M4_Move):
                Go_to_target_Speed_4(Speed)
                M4_state_dir = 1
        elif Speed < -15:
            if (M4_state == M4_Stop) or (M4_state_dir == -1 and M4_state == M4_Move):
                Go_to_target_Speed_4(Speed)
                M4_state_dir = -1
            elif (M4_state == M4_Move and M4_state_dir == 1) or (M4_state == M4_Stop and M4_state_dir == 1):
                Go_to_Speed_Zero_4()
        elif Speed < 0 and Speed >= -15:
            if (pre_4_Speed < -15) or (M4_state == M4_Move and M4_state_dir == 1) or (M4_state == M4_Stop and M4_state_dir == 1):
                Go_to_Speed_Zero_4()
            elif (M4_state == M4_Stop) or (M4_state_dir == -1 and M4_state == M4_Move):
                Go_to_target_Speed_4(Speed)
                M4_state_dir = -1
        teensy.write(bytearray(packet4))
        pre_4_Speed = Speed
    elif Speed == 0 and M4_state == M4_Move and start_up_4 == 1:
        Go_to_Speed_Zero_4()
        teensy.write(bytearray(packet4))
        start_up_4 = 0
        

def get_gamepad_input(joystick):
    # 이벤트 처리
    for event in pygame.event.get():
        if event.type == pygame.JOYAXISMOTION:
            if event.axis == 0:                
                if event.value > -0.2025 and event.value < 0.2025:
                    joy.axisX = int(0)
                else:
                    if event.value >= 0.2025:
                        joy.axisX = int(event.value * 400 - 80)
                    elif event.value <= -0.2025:
                        joy.axisX = int(event.value * 400 + 80)
            if event.axis == 1:                
                if event.value > -0.22 and event.value < 0.22:
                    joy.axisY = int(0)
                else:
                    if event.value > 0.22 :
                        joy.axisY = int(event.value * 50 - 10)
                    elif event.value < -0.22 :
                        joy.axisY = int(event.value * 50 + 10)
        if event.type == pygame.JOYBUTTONDOWN:
            if event.button == 0:
                joy.axisX = int(10)
                print("BB")
            

gamepad = init_gamepad()

while True:
    if gamepad:
        get_gamepad_input(gamepad)
        
    if teensy.in_waiting > 0:
        data = teensy.read(teensy.in_waiting)
        print(data)
        if (data == M3_Stop) or (data == M3_Move):
            M3_state = data
            if data == M3_Stop:
                M3_state_dir = 0
        elif (data == M4_Stop) or (data == M4_Move):
            M4_state = data
            if data == M4_Stop:
                M4_state_dir = 0
        
    
    Active_Motor_3(joy.axisX)
    Active_Motor_4(joy.axisY)
    #time.sleep(0.001)
        
device.close()