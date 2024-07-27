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
                            joy.axisX = int(event.value * 400 - 80)
                        elif event.value <= -0.2025:
                            joy.axisX = int(event.value * 400 + 80)
                elif (joy.button & 0x08) == 0x08:
                    if event.value > -0.25 and event.value < 0.25:
                        joy.axisX = int(0)
                    else:
                        if event.value >= 0.25:
                            joy.axisX = int(event.value * 20 - 4)
                        elif event.value <= -0.25:
                            joy.axisX = int(event.value * 20 + 4)
            if event.axis == 1:
                if (joy.button & 0x04) == 0x04:
                    if event.value > -0.22 and event.value < 0.22:
                        joy.axisY = int(0)
                    else:
                        if event.value >= 0.22 :
                            joy.axisY = int(event.value * 100 - 20)
                        elif event.value <= -0.22 :
                            joy.axisY = int(event.value * 100 + 20)
                elif (joy.button & 0x08) == 0x08:
                    if event.value > -0.3 and event.value < 0.3:
                        joy.axisY = int(0)
                    else:
                        if event.value >= 0.3 :
                            joy.axisY = int(event.value * 10 - 2)
                        elif event.value <= -0.3 :
                            joy.axisY = int(event.value * 10 + 2)
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
            elif event.button == 7:
                joy.button = (joy.button | 0x80)
        elif event.type == pygame.JOYBUTTONUP:
            if event.button == 0:
                print("Fire_Off")
                joy.button = (joy.button & ~(0x01))
            elif event.button == 1:
                print("release_Target")
                joy.button = (joy.button & ~(0x02))
            elif event.button == 7:
                joy.button = (joy.button & ~(0x80))
            
gamepad = init_gamepad()
        
