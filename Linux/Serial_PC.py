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
ACC_Z = 220
X_FLAG = 0
Y_FLAG = 0
CNT = 0

class joy_data:
    axisX:int = 0
    axisY:int = 0
    axisT:int = 0
    button:int = 0xfc000004
joy = joy_data()

try:
    teensy = serial.Serial('/dev/ttyACM0', 115200)
except IOError as e:
    print(e)
    print("Teensy 어디")
    exit()


PREV_RIGHT = 0
PREV_LEFT = 0
PREV_UP = 0
PREV_DOWN = 0
def get_gamepad_input(joystick):
    # 이벤트 처리
    global UP, DOWN, RIGHT, LEFT, DIR, ACC, X_FLAG, Y_FLAG, CNT, joy, PREV_RIGHT, PREV_LEFT, PREV_UP, PREV_DOWN

    for event in pygame.event.get():
        if event.type == pygame.JOYAXISMOTION:
            if event.axis == 0:                
                if event.value > -0.2 and event.value < 0.2:
                    # joystick - center
                    joy.axisX = int(0)
                    RIGHT = 0
                    LEFT = 0
                    CRCBYTE = (0x3 + 0xF6 + ACC_Z) & 0xFF
                    if X_FLAG == 1:
                        print("{} 정지 {}", CNT, LEFT)
                        packet = [3, 0xF6, 0, 0, ACC_Z, CRCBYTE]
                        teensy.write(bytearray(packet))
                        CNT +=1 
                    X_FLAG = 0

                else:
                    # joystick - 기울어짐
                    X_FLAG = 1
                    if event.value > 0:
                        joy.axisX = int(event.value * 100)
                        if (RIGHT != joy.axisX and joy.axisX != 0):                            
                            RIGHT = joy.axisX
                            if RIGHT != PREV_RIGHT:
                                PREV_RIGHT = RIGHT
                                
                                DIR = 0
                                upper_speed = (RIGHT >> 4) & 0x0F
                                lower_speed = RIGHT & 0x0F
                                BYTE2 = (DIR << 7) | upper_speed
                                CRCBYTE = (0x3 + 0xF6 + BYTE2 + lower_speed + ACC_Z) & 0xFF
                                print("우키",CNT, RIGHT)
                                
                                packet = [3, 0xF6, BYTE2, lower_speed, ACC_Z, CRCBYTE]
                                teensy.write(bytearray(packet))
                                CNT += 1

                    elif event.value < 0 :
                        joy.axisX = abs(int(event.value * 100))
                        if (LEFT != joy.axisX and joy.axisX != 0):
                            LEFT = abs(joy.axisX)
                            if LEFT != PREV_LEFT:
                                PREV_LEFT = LEFT

                                DIR = 1
                                upper_speed = (LEFT >> 4) & 0x0F
                                lower_speed = LEFT & 0x0F
                                BYTE2 = (DIR << 7) | upper_speed
                                CRCBYTE = (0x3 + 0xF6 + BYTE2 + lower_speed + ACC_Z) & 0xFF
                                print("왼키", CNT, LEFT)
                                packet = [3, 0xF6, BYTE2, lower_speed, ACC_Z, CRCBYTE]
                                teensy.write(bytearray(packet))
                                CNT += 1
            if event.axis == 1:                
                if event.value > -0.2 and event.value < 0.2:
                    # joystick - center
                    joy.axisY = int(0)
                    UP = 0
                    DOWN = 0
                    CRCBYTE = (0x4 + 0xF6 + ACC) & 0xFF
                    if Y_FLAG == 1:
                        ACC = 5
                        print("정지", CNT, UP)
                        packet = [4, 0xF6, 0, 0, ACC, CRCBYTE]
                        teensy.write(bytearray(packet))

                        CRCBYTE = (0x5 + 0xF6 + ACC) & 0xFF
                        packet = [5, 0xF6, 0, 0, ACC, CRCBYTE]
                        teensy.write(bytearray(packet))
                        CNT +=1 
                    Y_FLAG = 0

                else:
                    # joystick - 기울어짐
                    Y_FLAG = 1
                    if event.value > 0 :
                        joy.axisY = int(event.value * 100)
                        if (DOWN != joy.axisY and joy.axisY != 0):                            
                            DOWN = int(joy.axisY / 2)
                            if DOWN != PREV_DOWN:
                                PREV_DOWN = DOWN
                                
                                DIR = 0
                                upper_speed = (DOWN >> 4) & 0x0F
                                lower_speed = DOWN & 0x0F
                                BYTE2 = (DIR << 7) | upper_speed
                                CRCBYTE = (0x4 + 0xF6 + BYTE2 + lower_speed + ACC) & 0xFF
                                print("아럐",CNT, DOWN)
                                
                                packet = [4, 0xF6, BYTE2, lower_speed, ACC, CRCBYTE]
                                teensy.write(bytearray(packet))

                                DIR = 1
                                upper_speed = (DOWN >> 4) & 0x0F
                                lower_speed = DOWN & 0x0F
                                BYTE2 = (DIR << 7) | upper_speed
                                CRCBYTE = (0x5 + 0xF6 + BYTE2 + lower_speed + ACC) & 0xFF
                                packet = [5, 0xF6, BYTE2, lower_speed, ACC, CRCBYTE]
                                teensy.write(bytearray(packet))
                                CNT += 1

                    elif event.value < 0 :
                        joy.axisY = abs(int(event.value * 100))
                        if (UP != joy.axisY and joy.axisY != 0):
                            UP = int(abs(joy.axisY) / 2)
                            if UP != PREV_UP:
                                PREV_UP = UP

                                DIR = 1
                                upper_speed = (UP >> 4) & 0x0F
                                lower_speed = UP & 0x0F
                                BYTE2 = (DIR << 7) | upper_speed
                                CRCBYTE = (0x4 + 0xF6 + BYTE2 + lower_speed + ACC) & 0xFF
                                print("위", CNT, UP)
                                packet = [4, 0xF6, BYTE2, lower_speed, ACC, CRCBYTE]
                                teensy.write(bytearray(packet))

                                DIR = 0
                                upper_speed = (UP >> 4) & 0x0F
                                lower_speed = UP & 0x0F
                                BYTE2 = (DIR << 7) | upper_speed
                                CRCBYTE = (0x5 + 0xF6 + BYTE2 + lower_speed + ACC) & 0xFF
                                packet = [5, 0xF6, BYTE2, lower_speed, ACC, CRCBYTE]
                                teensy.write(bytearray(packet))
                                CNT += 1                            
            

gamepad = init_gamepad()

while True:
    if gamepad:
        get_gamepad_input(gamepad)   
device.close()