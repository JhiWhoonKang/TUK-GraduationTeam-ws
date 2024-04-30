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

class joy_data:
    axisX:int = 0
    axisY:int = 0
    axisT:int = 0
    button:int = 0xfc000004
joy = joy_data()

try:
    teensy = serial.Serial('COM25', 115200)
except IOError as e:
    print(e)
    print("Teensy 어디")
    exit()


RIGHT = 0
LEFT = 0
UP = 0
DOWN = 0
STOP = 0
ACC_Z = 220
PREV_RIGHT = 0
PREV_LEFT = 0
PREV_STOP = 0
LEFT_TURN = 2
RIGHT_TURN = 2
def get_gamepad_input(joystick):
    global UP, DOWN, RIGHT, LEFT, STOP, joy, PREV_RIGHT, PREV_LEFT, PREV_STOP, LEFT_TURN, RIGHT_TURN

    for event in pygame.event.get():
        if event.type == pygame.JOYAXISMOTION:
            if event.axis == 0:
                if -0.2 < event.value < 0.2:
                    # 조이스틱 - 중앙
                    joy.axisX = int(0)
                    STOP = joy.axisX
                    speed_mode_stop(3, ACC_Z)                    
                    
                else:
                    # 조이스틱 - 기울어짐
                    if event.value > 0: # CW
                        RIGHT_TURN = 1 # 회전하고 있던 상태
                        joy.axisX = int(event.value * 100)
                        RIGHT = joy.axisX

                        if LEFT_TURN == 1:
                            if speed_mode_stop(3, ACC_Z):
                                speed_mode(3, 0, RIGHT, ACC_Z)
                                RIGHT_TURN = 1
                        else:
                            speed_mode(3, 0, RIGHT, ACC_Z)
                                
                    elif event.value < 0: # CCW
                        LEFT_TURN = 1
                        joy.axisX = abs(int(event.value * 100))
                        LEFT = abs(joy.axisX)

                        if RIGHT_TURN == 1:
                            if speed_mode_stop(3, ACC_Z):
                                speed_mode(3, 1, LEFT, ACC_Z)
                                LEFT_TURN = 1
                        else:
                            speed_mode(3, 1, LEFT, ACC_Z)

def speed_mode_stop(canid, acc):
    global LEFT_TURN, RIGHT_TURN
    crcbyte = (canid + 0xF6 + acc) & 0xFF
    packet = [canid, 0xF6, 0, 0, acc, crcbyte]
    teensy.write(bytearray(packet))
    while True:
        print("loop..")
        if teensy.in_waiting > 0:
            data = teensy.read(teensy.in_waiting)
            print(data)
            if data == b'\x03\xf6\x02\xfb':
                print("stoip")
                return True
            elif data == b'\x03\xf6\x00\xf9':
                print("help")
                LEFT_TURN = 0
                RIGHT_TURN = 0
                break
            elif data == b'\x03\xf6\x01\xfa':
                break
            else:
                teensy.write(bytearray(packet))
                break
        else:
            print("helpppppppppppppppppppppp")
            break

def speed_mode(canid, dir, speed, acc):
    upper_speed = (speed >> 4) & 0x0F
    lower_speed = speed & 0x0F
    byte2 = (dir << 7) | upper_speed
    crcbyte = (canid + 0xF6 + byte2 + lower_speed + acc) & 0xFF
    packet = [canid, 0xF6, byte2, lower_speed, acc, crcbyte]
    teensy.write(bytearray(packet))


gamepad = init_gamepad()
try:     
    if gamepad:
        while True:
            get_gamepad_input(gamepad)
            
            # if teensy.in_waiting > 0:
            #     data = teensy.read(teensy.in_waiting)
            #     print(data)                                
            
except KeyboardInterrupt:
    print("종료")