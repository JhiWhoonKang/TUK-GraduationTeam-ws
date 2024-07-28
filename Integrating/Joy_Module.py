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
    axisT:int = 1
    button:int = 0xfc100000

joy = joy_data()

def get_gamepad_input(joystick):
    # 이벤트 처리
    for event in pygame.event.get():
        if event.type == pygame.JOYAXISMOTION:
            if event.axis == 0:
                if (joy.button & 0x04) == 0x04:
                    if event.value > -0.2025 and event.value < 0.2025:
                        joy.axisX = int(0)
                    else:
                        if event.value >= 0.2025:
                            joy.axisX = int((event.value * 400 - 80) * joy.axisT)
                        elif event.value <= -0.2025:
                            joy.axisX = int((event.value * 400 + 80) * joy.axisT)
            if event.axis == 1:
                if (joy.button & 0x04) == 0x04:
                    if event.value > -0.22 and event.value < 0.22:
                        joy.axisY = int(0)
                    else:
                        if event.value >= 0.22 :
                            joy.axisY = int((event.value * 100 - 20) * joy.axisT)
                        elif event.value <= -0.22 :
                            joy.axisY = int((event.value * 100 + 20) * joy.axisT)
            if event.axis == 3:
                joy.axisT = (abs(-event.value + 1) / float(2))
                print(joy.axisT)
        if event.type == pygame.JOYBUTTONDOWN:
            if event.button == 0:
                joy.button = (joy.button | 0x01)
                print("Fire_On")
            elif event.button == 1:
                joy.button = (joy.button | 0x02)
                print("Aming")
            elif event.button == 2:
                joy.button = (joy.button & ~(0x38))
                joy.button = (joy.button | 0x04)
                print("Fast_Manual_Activate")
            elif event.button == 3:
                joy.button = (joy.button & ~(0x34))
                joy.button = (joy.button | 0x08)
                print("Slow_Manual_Activate")
            elif event.button == 4:
                joy.button = (joy.button & ~(0x2c))
                joy.button = (joy.button | 0x10)
                print("Tracking_person")
            elif event.button == 5:
                joy.button = (joy.button & ~(0x1c))
                joy.button = (joy.button | 0x20)
                print("Tracking_lazer")
            elif event.button == 6:
                joy.button = (joy.button | 0x40)
            elif event.button == 7:
                joy.button = (joy.button | 0x80)
            elif event.button == 8:
                joy.button = (joy.button | 0x100)
            elif event.button == 9:
                joy.button = (joy.button | 0x200)
            elif event.button == 10:
                joy.button = (joy.button | 0x400)
            elif event.button == 11:
                joy.button = (joy.button | 0x800)
        elif event.type == pygame.JOYBUTTONUP:
            if event.button == 0:
                print("Fire_Off")
                joy.button = (joy.button & ~(0x01))
            elif event.button == 1:
                print("release_Target")
                joy.button = (joy.button & ~(0x02))
            elif event.button == 6:
                joy.button = (joy.button & ~(0x40))
            elif event.button == 7:
                joy.button = (joy.button & ~(0x80))
            elif event.button == 8:
                joy.button = (joy.button & ~(0x100))
            elif event.button == 9:
                joy.button = (joy.button & ~(0x200))
            elif event.button == 10:
                joy.button = (joy.button & ~(0x400))
            elif event.button == 11:
                joy.button = (joy.button & ~(0x800))
            
gamepad = init_gamepad()
        
