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
PREV_LEFT = 0
PREV_RIGHT = 0
PREV_UP = 0
PREV_DOWN = 0
def get_gamepad_input(joystick):
    global joy
    global PREV_DOWN, PREV_LEFT, PREV_RIGHT, PREV_UP
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
                    speed_mode_stop(4, ACC_Z)
                    speed_mode_stop(5, ACC_Z)
                else:
                    # joystick - 기울어짐
                    if event.value > 0:
                        joy.axisY = int(event.value * 100)
                        DOWN = int(joy.axisY / 2)
                        if DOWN != PREV_DOWN:
                            PREV_DOWN = DOWN
                            CURRENT_DIRECTION = 0
                            speed_mode(4, CURRENT_DIRECTION, DOWN, ACC_Z)
                            CURRENT_DIRECTION = 1
                            speed_mode(5, CURRENT_DIRECTION, DOWN, ACC_Z)                    
                    elif event.value < 0:
                        joy.axisY = abs(int(event.value * 100))
                        UP = int(joy.axisY / 2)
                        if UP != PREV_UP:
                            PREV_UP = UP
                            CURRENT_DIRECTION = 1
                            speed_mode(4, CURRENT_DIRECTION, UP, ACC_Z)
                            CURRENT_DIRECTION = 0
                            speed_mode(5, CURRENT_DIRECTION, UP, ACC_Z)
        if event.type == pygame.JOYBUTTONUP:
            if event.button == 0:
                go_home(3)
                #go_home(4)
                #go_home(5)
            if event.button == 2:
                set_current_axis_to_zero(3)
            if event.button == 3:
                read_encoder(3)
                #read_encoder(4)
                #read_encoder(5)
            if event.button == 4:
                position_mode(3, 0, 10, 3, 1)    
            if event.button == 11:
                power_on_off()
            
                                
def power_on_off():
    go_home(3)
    go_home(4)
    go_home(5)

def speed_mode(canid, dir, speed, acc):
    upper_speed = (speed >> 8) & 0xFF
    lower_speed = speed & 0xFF
    byte2 = (dir << 7) | upper_speed
    crcbyte = (canid + 0xF6 + byte2 + lower_speed + acc) & 0xFF
    packet = [canid, 5, 0xF6, byte2, lower_speed, acc, crcbyte]
    print(packet)
    teensy.write(bytearray(packet))

def speed_mode_stop(canid, acc):
    global LEFT_TURN, RIGHT_TURN
    crcbyte = (canid + 0xF6 + acc) & 0xFF
    packet = [canid, 5, 0xF6, 0, 0, acc, crcbyte]
    teensy.write(bytearray(packet))
    
def position_mode(canid, dir, speed, acc, pulse):
    upper_speed = (speed >> 8) & 0xFF
    lower_speed = speed & 0xFF
    byte2 = (dir << 7) | upper_speed
    pulsebyte1 = (pulse >> 16) & 0xFF
    pulsebyte2 = (pulse >> 8) & 0xFF
    pulsebyte3 = pulse & 0xFF
    crcbyte = (canid + 0xFD + byte2 + lower_speed + acc + pulsebyte1 + pulsebyte2 + pulsebyte3) & 0xFF
    packet = [canid, 8, 0xFD, byte2, lower_speed, acc, pulsebyte1, pulsebyte2, pulsebyte3, crcbyte]
    print(packet)
    teensy.write(bytearray(packet))

def read_encoder(canid):
    byte1 = 0x31
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))    

def read_pulse(canid):
    byte1 = 0x33
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))

def go_home(canid):
    byte1 = 0x91
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))

def set_current_axis_to_zero(canid):
    byte1 = 0x92
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))

try:     
    gamepad = init_gamepad()
    if gamepad:
        while True:
            get_gamepad_input(gamepad)
            #print("RIGHT: {}, LEFT: {}, ACC: {}".format(PREV_RIGHT, PREV_LEFT, ACC_Z))
            #read_encoder(3)
            #read_encoder(4)
            #read_encoder(5)
            
except KeyboardInterrupt:
    print("종료")