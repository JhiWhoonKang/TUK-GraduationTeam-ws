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

UP = 0
DOWN = 0
RIGHT = 0
LEFT = 0
DIR = 0
ACC = 1
ACC_Z = 240

packet3 = [0, 0x00, 0, 0, ACC_Z, (0x3 + 0xF6 + ACC_Z) & 0xFF]
pre_packet3 = [0, 0x00, 0, 0, ACC_Z, (0x3 + 0xF6 + ACC_Z) & 0xFF]
packet4 = [0, 0x00, 0, 0, ACC_Z, (0x3 + 0xF6 + ACC_Z) & 0xFF]
pre_packet4 = [0, 0x00, 0, 0, ACC_Z, (0x3 + 0xF6 + ACC_Z) & 0xFF]

M3_state_dir = 0
M4_state_dir = 0

M3_Stop = b'\x03\x03\xf6\x02\xfb'
M3_Move = b'\x03\x03\xf6\x01\xfa'
M3_state = b'\x03\x03\xf6\x02\xfb'

M4_Stop = b'\x04\x03\xf6\x02\xfc'
M4_Move = b'\x04\x03\xf6\x01\xfb'
M4_state = b'\x04\x03\xf6\x02\xfc'

PREV_RIGHT = 0
PREV_LEFT = 0
PREV_UP = 0
PREV_DOWN = 0        

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
   
pre_3_Speed = 0
def Active_Motor_3(Speed):
    global ACC_Z, pre_3_Speed, M3_state_dir, M3_state, M3_Stop, M3_Move
    if Speed != pre_3_Speed or (Speed != 0 and M3_state == M3_Stop):
        #print(Speed)
        if Speed == 0:
            CRCBYTE = (0x3 + 0xF6 + ACC_Z) & 0xFF
            packet = [3, 5, 0xF6, 0, 0, ACC_Z, CRCBYTE]
            teensy.write(bytearray(packet))
        elif Speed > 0:
            if (M3_state == M3_Stop) or (M3_state_dir == 1 and M3_state == M3_Move):
                RIGHT = Speed
                DIR = 0
                upper_speed = (RIGHT >> 8) & 0x0F
                lower_speed = RIGHT & 0xFF
                BYTE2 = (DIR << 7) | upper_speed
                CRCBYTE = (0x3 + 0xF6 + BYTE2 + lower_speed + ACC_Z) & 0xFF
                packet = [3, 5, 0xF6, BYTE2, lower_speed, ACC_Z, CRCBYTE]
                teensy.write(bytearray(packet))
                M3_state_dir = 1
            elif (M3_state == M3_Move and M3_state_dir == -1) or (M3_state == M3_Stop and M3_state_dir == -1):
                CRCBYTE = (0x3 + 0xF6 + ACC_Z) & 0xFF
                packet = [3, 5, 0xF6, 0, 0, ACC_Z, CRCBYTE]
                teensy.write(bytearray(packet))
        elif Speed < 0:
            if (M3_state == M3_Stop) or (M3_state_dir == -1 and M3_state == M3_Move):
                LEFT = abs(Speed)
                DIR = 1
                upper_speed = (LEFT >> 8) & 0x0F
                lower_speed = LEFT & 0xFF
                BYTE2 = (DIR << 7) | upper_speed
                CRCBYTE = (0x3 + 0xF6 + BYTE2 + lower_speed + ACC_Z) & 0xFF
                packet = [3, 5, 0xF6, BYTE2, lower_speed, ACC_Z, CRCBYTE]
                teensy.write(bytearray(packet))
                M3_state_dir = -1
            elif (M3_state == M3_Move and M3_state_dir == 1) and (M3_state == M3_Stop and M3_state_dir == 1):
                CRCBYTE = (0x3 + 0xF6 + ACC_Z) & 0xFF
                packet = [3, 5, 0xF6, 0, 0, ACC_Z, CRCBYTE]
                teensy.write(bytearray(packet))
        pre_3_Speed = Speed
           
       
pre_4_Speed = 0
def Active_Motor_4(Speed):
    global ACC, pre_4_Speed, M4_state_dir, M4_state, M4_Stop, M4_Move
    if Speed != pre_4_Speed or (Speed != 0 and M4_state == M4_Stop):
        if Speed == 0:
            ACC = 5
            CRCBYTE = (0x4 + 0xF6 + ACC) & 0xFF
            packet = [4, 0xF6, 0, 0, ACC, CRCBYTE]
            teensy.write(bytearray(packet))
        elif Speed > 0:
            if (M4_state == M4_Stop) or (M4_state_dir == 1 and M4_state == M4_Move):
                DOWN = Speed
                DIR = 0
                upper_speed = (DOWN >> 8) & 0x0F
                lower_speed = DOWN & 0xFF
                BYTE2 = (DIR << 7) | upper_speed
                CRCBYTE = (0x4 + 0xF6 + BYTE2 + lower_speed + ACC) & 0xFF
                packet = [4, 0xF6, BYTE2, lower_speed, ACC, CRCBYTE]
                teensy.write(bytearray(packet))
                M4_state_dir = 1
            elif (M4_state == M4_Move and M4_state_dir == -1) and (M4_state == M4_Stop and M4_state_dir == -1):
                ACC = 5
                CRCBYTE = (0x4 + 0xF6 + ACC) & 0xFF
                packet = [4, 0xF6, 0, 0, ACC, CRCBYTE]
                teensy.write(bytearray(packet))
        elif Speed < 0:
            if (M4_state == M4_Stop) or (M4_state_dir == -1 and M4_state == M4_Move):
                UP = abs(Speed)
                DIR = 1
                upper_speed = (UP >> 8) & 0x0F
                lower_speed = UP & 0xFF
                BYTE2 = (DIR << 7) | upper_speed
                CRCBYTE = (0x4 + 0xF6 + BYTE2 + lower_speed + ACC) & 0xFF
                packet = [4, 0xF6, BYTE2, lower_speed, ACC, CRCBYTE]
                teensy.write(bytearray(packet))
                M4_state_dir = -1
            elif (M4_state == M4_Move and M4_state_dir == 1) and (M4_state == M4_Stop and M4_state_dir == 1):
                ACC = 5
                CRCBYTE = (0x4 + 0xF6 + ACC) & 0xFF
                packet = [4, 0xF6, 0, 0, ACC, CRCBYTE]
                teensy.write(bytearray(packet))
        pre_4_Speed = Speed
       

def get_gamepad_input(joystick):
    # 이벤트 처리
    for event in pygame.event.get():
        if event.type == pygame.JOYAXISMOTION:
            if event.axis == 0:                
                if event.value > -0.2 and event.value < 0.2:
                    joy.axisX = int(0)
                else:
                    if event.value >= 0.2025:
                        joy.axisX = int(event.value * 400 - 80)
                    elif event.value <= -0.2025:
                        joy.axisX = int(event.value * 400 + 80)
            if event.axis == 1:                
                if event.value > -0.2 and event.value < 0.2:
                    joy.axisY = int(0)
                else:
                    if event.value > 0 :
                        joy.axisY = int(event.value * 50)
                    elif event.value < 0 :
                        joy.axisY = int(event.value * 50)
        if event.type == pygame.JOYBUTTONDOWN:
            if event.button == 0:
                print("BB")
           

gamepad = init_gamepad()

while True:
    if gamepad:
        get_gamepad_input(gamepad)
       
    if teensy.in_waiting > 0:
        data = teensy.read(teensy.in_waiting)
        if (data == M3_Stop) or (data == M3_Move):
            M3_state = data
            if data == M3_Stop:
                M3_state_dir = 0
        elif (data == M4_Stop) or (data == M4_Move):
            M4_state = data
            if data == M4_Stop:
                M4_state_dir = 0
        if data[:2] == b'\x03\x03':
            print(data)
       
   
    Active_Motor_3(joy.axisX)
    #Active_Motor_4(joy.axisY)
    #time.sleep(0.001)
       
device.close()