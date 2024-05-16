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
WEL_U = 0
WEL_L = 0
pulse_cnt = 0
cali_Flag = 0 # 캘리 했으면 1, 캘리 안했으면 0
def get_gamepad_input(joystick):
    global joy
    global PREV_DOWN, PREV_LEFT, PREV_RIGHT, PREV_UP, pulse_cnt, WEL_U, WEL_L, cali_Flag
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
                    read_encoder(5)
                    joy.axisY = int(0)
                    speed_mode_stop(4, ACC_Z)
                    speed_mode_stop(5, ACC_Z)
                else:
                    # joystick - 기울어짐
                    read_encoder(5)
                    if event.value > 0:
                        if WEL_U == 1:
                            joy.axisY = int(event.value * 100) 
                            DOWN = int(joy.axisY / 2)
                            if DOWN != PREV_DOWN:
                                PREV_DOWN = DOWN
                                CURRENT_DIRECTION = 0
                                speed_mode(4, CURRENT_DIRECTION, DOWN, ACC_Z)
                                CURRENT_DIRECTION = 1 # 기총 위 방향
                                speed_mode(5, CURRENT_DIRECTION, DOWN, ACC_Z)                    
                                print("HIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHIHI")
                    elif event.value < 0:
                        if WEL_L == 1:
                            joy.axisY = abs(int(event.value * 100))
                            UP = int(joy.axisY / 2)
                            if UP != PREV_UP:
                                PREV_UP = UP
                                CURRENT_DIRECTION = 1
                                speed_mode(4, CURRENT_DIRECTION, UP, ACC_Z)
                                CURRENT_DIRECTION = 0 # 기총 아래 방향
                                speed_mode(5, CURRENT_DIRECTION, UP, ACC_Z)
                    # else:
                    #     print("STOP..")
                    #     speed_mode_stop(5, ACC_Z)
                                
        if event.type == pygame.JOYBUTTONUP:            
            if event.button == 2:
                set_current_axis_to_zero(5)
                pulse_cnt = 0
                read_encoder(5)
            if event.button == 3: #4번
                #read_encoder(3)
                #read_encoder(4)
                read_encoder(5)
            if event.button == 6:
                set_current_axis_to_zero(5)
            if event.button == 8: # 기총 위 방향
                position_mode(5, 1, 10, 3, 100)
                pulse_cnt += 1
                print(pulse_cnt)
                read_encoder(5)
            if event.button == 9: #기총 아래 방향
                position_mode(5, 0, 10, 3, 100)
                pulse_cnt -= 1
                print(pulse_cnt)
                read_encoder(5)
            if event.button == 10:
                #go_home(3)
                #go_home(4)
                go_home(5)  
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
    # print(packet)
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
    # print(packet)
    teensy.write(bytearray(packet))

pattern = b'\x05\x31'
pattern_length = len(pattern)
data_length = 9
UPPER_LIMIT = '000000008ddd'
LOWER_LIMIT = 'ffffffff8a6d'
ZERO1 = 0
ZERO2 = 'ffffffffffff'
upper_limit_int = int('000000008ddd', 16)
lower_limit_int = int('ffffffff8a6d', 16)
zero1_int = int('0', 16)
zero2_int = int('ffffffffffff', 16)
def read_encoder(canid):
    global WEL_U, WEL_L
    print("리드요")
    byte1 = 0x31
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))
    if teensy.in_waiting > 0:
        data = teensy.read(teensy.in_waiting)
        #print("Origin Data: ", data.hex())
        for i in range(len(data)):
            if data[i:i+pattern_length] == pattern:
                filtered_data = data[i:i+data_length]
                filtered_data_hex = filtered_data.hex()
                weapon_encoder_data = filtered_data_hex[4:-2]
                weapon_encoder_data_int = int(weapon_encoder_data, 16)
                print("Filtered Data: ", weapon_encoder_data)                
                print("zero1_int: ", zero1_int)
                print("upper limit: ", upper_limit_int)
                print("lower_limit: ", lower_limit_int)
                print("zero2_int: ", zero2_int)
                print("moter encoder int: ", weapon_encoder_data_int)                            
                if zero1_int <= weapon_encoder_data_int <= upper_limit_int:
                    WEL_U = 1
                    WEL_L = 1
                    print("WEL 1 - Within upper limits")
                    print("wel u 사ㅇ태: {0}, wel l 상태: {1}".format(WEL_U, WEL_L) )
                
                elif lower_limit_int  <= weapon_encoder_data_int <= zero2_int:
                    WEL_U = 1
                    WEL_L = 1
                    print("WEL 1 - Within lower limits")
                    print("wel u 사ㅇ태: {0}, wel l 상태: {1}".format(WEL_U, WEL_L) )
                elif lower_limit_int > weapon_encoder_data_int > upper_limit_int:
                    WEL_U = 0
                    WEL_L = 1
                    print("WEL 0 - 완전나감1")
                    print("wel u 사ㅇ태: {0}, wel l 상태: {1}".format(WEL_U, WEL_L) )
                elif upper_limit_int < weapon_encoder_data_int < lower_limit_int:
                    WEL_U = 1
                    WEL_L = 0
                    print("WEL 0 - 완전나감2")
                    print("wel u 사ㅇ태: {0}, wel l 상태: {1}".format(WEL_U, WEL_L) )
                else:
                    WEL_U = WEL_L = 0
                    print("WEL 0 - Out of limits")
                    print("wel u 사ㅇ태: {0}, wel l 상태: {1}".format(WEL_U, WEL_L) )
                break
    
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
    global cali_Flag
    byte1 = 0x92
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    teensy.write(bytearray(packet))
    cali_Flag = 1
    read_encoder(5)

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