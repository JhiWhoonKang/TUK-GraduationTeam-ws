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

ACC_Z = 220
def get_gamepad_input(joystick):
    global joy

    for event in pygame.event.get():
        if event.type == pygame.JOYAXISMOTION:
            if event.axis == 0:
                if -0.2 < event.value < 0.2:
                    joy.axisX = int(0)
                    speed_mode_stop(3, ACC_Z)
                else:
                    if event.value > 0:  # CW
                        joy.axisX = int(event.value * 100)
                        RIGHT = joy.axisX
                        if RIGHT != PREV_RIGHT:
                            PREV_RIGHT = RIGHT
                            CURRENT_DIRECTION = 0  # 오른쪽 방향
                            CURRENT_SPEED = RIGHT
                            speed_mode(3, CURRENT_DIRECTION, RIGHT, ACC_Z)
                        
                    elif event.value < 0:  # CCW
                        joy.axisX = abs(int(event.value * 100))
                        LEFT = abs(joy.axisX)
                        if LEFT != PREV_LEFT:
                            PREV_LEFT = LEFT
                            CURRENT_DIRECTION = 1  # 왼쪽 방향
                            CURRENT_SPEED = LEFT
                            speed_mode(3, CURRENT_DIRECTION, LEFT, ACC_Z)
            if event.axis == 1:
                if event.value > -0.2 and event.value < 0.2:
                    # joystick - center
                    joy.axisY = int(0)
                    speed_mode_stop(3, ACC_Z)
                else:
                    # joystick - 기울어짐
                    if event.value > 0:
                        joy.axisY = int(event.value * 100)
                        DOWN = int(joy.axisY / 2)
                        if DOWN != PREV_DOWN:
                            PREV_DOWN = DOWN
                            CURRENT_DIRECTION = 0
                            speed_mode(3, CURRENT_DIRECTION, DOWN, ACC_Z)
                    
                    elif event.value > 0:
                        joy.axisY = abs(int(event.value * 100))
                        UP = int(joy.axisY / 2)
                        if UP != PREV_UP:
                            PREV_UP = UP
                            CURRENT_DIRECTION = 1
                            speed_mode(3, CURRENT_DIRECTION, UP, ACC_Z)

        if event.type == pygame.JOYBUTTONUP:
            if event.button == 0:
                read_pulse(3)                            

def speed_mode(canid, dir, speed, acc):
    upper_speed = (speed >> 4) & 0x0F
    lower_speed = speed & 0x0F
    byte2 = (dir << 7) | upper_speed
    crcbyte = (canid + 0xF6 + byte2 + lower_speed + acc) & 0xFF
    packet = [canid, 5, 0xF6, byte2, lower_speed, acc, crcbyte]
    teensy.write(bytearray(packet))

def speed_mode_stop(canid, acc):
    global LEFT_TURN, RIGHT_TURN
    crcbyte = (canid + 0xF6 + acc) & 0xFF
    packet = [canid, 5, 0xF6, 0, 0, acc, crcbyte]
    teensy.write(bytearray(packet))
    
def read_encoder(canid):
    byte1 = 31
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))    

def read_pulse(canid):
    byte1 = 33
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))
    
def go_home(canid):
    byte1 = 91
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))

def set_current_axis_to_zero(canid):
    byte1 = 92
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))
     
try:     
    gamepad = init_gamepad()
    if gamepad:
        while True:
            get_gamepad_input(gamepad)
            # print("DIR: {}, RIGHT: {}, LEFT: {}, ACC: {}, CURRENT SPEED: {}".format(CURRENT_DIRECTION, RIGHT, LEFT, ACC_Z, CURRENT_SPEED))
except KeyboardInterrupt:
    print("종료")