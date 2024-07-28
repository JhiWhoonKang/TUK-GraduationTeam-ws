import serial
import math
import time

class M3:
    def __init__(self, M):
        self.MCU = M

        self.start_up_3 = 0

        #가속도
        self.ACC3 = 235

        #이전 스피드 기억하기
        self.pre_3_Speed = 0

        #명령 내린 방향 기억하기
        self.M3_state_dir = 0

        #모터 드라이버에서 송신한 모터 구동에 대한 데이터
        self.M3_Stop = b'\x03\x03\xf6\x02\xfb'
        self.M3_Move = b'\x03\x03\xf6\x01\xfa'
        self.M3_state = b'\x03\x03\xf6\x02\xfb'
        self.M3_error = b'\x03\x03\xf6\x00\xf9'

        #데이터 패킷
        self.CRCBYTE3 = (0x3 + 0xF6 + self.ACC3) & 0xFF
        self.packet3 = [3, 5, 0xF6, 0, 0, self.ACC3, self.CRCBYTE3]

        #3번 모터 속도 0으로 패킷 만들기
    def Go_to_Speed_Zero_3(self):
        self.CRCBYTE3 = (0x3 + 0xF6 + self.ACC3) & 0xFF
        self.packet3 = [3, 5, 0xF6, 0, 0, self.ACC3, self.CRCBYTE3]
    
    #3번 모터 특정 속도로 패킷 만들기
    def Go_to_target_Speed_3(self, Speed):
        if Speed > 0:
            DIR = 0
            upper_speed = (Speed >> 8) & 0x0F
            lower_speed = Speed & 0xFF
            BYTE2 = (DIR << 7) | upper_speed
            self.CRCBYTE3 = (0x3 + 0xF6 + BYTE2 + lower_speed + self.ACC3) & 0xFF
            self.packet3 = [3, 5, 0xF6, BYTE2, lower_speed, self.ACC3, self.CRCBYTE3]
        elif Speed < 0:
            DIR = 1
            upper_speed = (abs(Speed) >> 8) & 0x0F
            lower_speed = abs(Speed) & 0xFF
            BYTE2 = (DIR << 7) | upper_speed
            self.CRCBYTE3 = (0x3 + 0xF6 + BYTE2 + lower_speed + self.ACC3) & 0xFF
            self.packet3 = [3, 5, 0xF6, BYTE2, lower_speed, self.ACC3, self.CRCBYTE3]

    def position_control(self, MCU, canid, dir, speed, acc,  pulses):

        upper_speed = (speed >> 8) & 0x0F

        lower_speed = speed & 0xFF

        byte1 = 0xFD

        byte2 = (dir << 7) | upper_speed

        byte3 = lower_speed

        byte4 = acc

        byte5 = (pulses >> 16) & 0xFF

        byte6 = (pulses >> 8) & 0xFF

        byte7 = pulses & 0xFF

        byte8 = (canid + 0xFD + byte2 + byte3 + byte4 + byte5 + byte6 +byte7) & 0xFF

        packet = [canid, 8, byte1, byte2, byte3, byte4, byte5, byte6, byte7, byte8]

        MCU.write(bytearray(packet))

    #가감속 이상하게 안되게 만든 모터 구동 함수
    def Active_Motor_3(self, Speed):
        if Speed != self.pre_3_Speed or (Speed != 0 and self.M3_state == self.M3_Stop):
            self.start_up_3 = 1
            if Speed == 0:
                self.Go_to_Speed_Zero_3()
            elif Speed > 15:
                if (self.M3_state == self.M3_Stop) or self.M3_state_dir == 1:
                    self.Go_to_target_Speed_3(Speed)
                    self.M3_state_dir = 1
                elif self.M3_state_dir == -1:
                    self.Go_to_Speed_Zero_3()
            elif Speed > 0 and Speed <= 15:
                if (self.pre_3_Speed > 15) or (self.M3_state_dir == -1):
                    self.Go_to_Speed_Zero_3()
                elif self.M3_state == self.M3_Stop:
                    self.Go_to_target_Speed_3(Speed)
                    self.M3_state_dir = 1
            elif Speed < -15:
                if (self.M3_state == self.M3_Stop) or self.M3_state_dir == -1:
                    self.Go_to_target_Speed_3(Speed)
                    self.M3_state_dir = -1
                elif (self.M3_state_dir == 1):
                    self.Go_to_Speed_Zero_3()
            elif Speed < 0 and Speed >= -15:
                if (self.pre_3_Speed < -15) or (self.M3_state_dir == 1):
                    self.Go_to_Speed_Zero_3()
                elif self.M3_state == self.M3_Stop:
                    self.Go_to_target_Speed_3(Speed)
                    self.M3_state_dir = -1
            self.M3_state = self.M3_Move
            self.MCU.write(bytearray(self.packet3))
            self.pre_3_Speed = Speed
        elif Speed == 0 and self.M3_state == self.M3_Move and self.start_up_3 == 1:
            self.Go_to_Speed_Zero_3()
            self.MCU.write(bytearray(self.packet3))
            self.start_up_3 = 0

class M4:
    def __init__(self, M):
        self.MCU = M

        self.start_up_4 = 0

        #가속도
        self.ACC4 = 220

        #이전 스피드 기억하기
        self.pre_4_Speed = 0

        #명령 내린 방향 기억하기
        self.M4_state_dir = 0

        #모터 드라이버에서 송신한 모터 구동에 대한 데이터
        self.M4_Stop = b'\x04\x03\xf6\x02\xfc'
        self.M4_Move = b'\x04\x03\xf6\x01\xfb'
        self.M4_state = b'\x04\x03\xf6\x02\xfc'
        self.M4_error = b'\x04\x03\xf6\x00\xfa'

        #데이터 패킷
        self.CRCBYTE4 = (0x4 + 0xF6 + self.ACC4) & 0xFF
        self.packet4 = [4, 5, 0xF6, 0, 0, self.ACC4, self.CRCBYTE4]

    def position_control(self, canid, dir, speed, acc,  pulses):

        upper_speed = (speed >> 8) & 0x0F

        lower_speed = speed & 0xFF

        byte1 = 0xFD

        byte2 = (dir << 7) | upper_speed

        byte3 = lower_speed

        byte4 = acc

        byte5 = (pulses >> 16) & 0xFF

        byte6 = (pulses >> 8) & 0xFF

        byte7 = pulses & 0xFF

        byte8 = (canid + 0xFD + byte2 + byte3 + byte4 + byte5 + byte6 +byte7) & 0xFF

        packet = [canid, 8, byte1, byte2, byte3, byte4, byte5, byte6, byte7, byte8]

        self.MCU.write(bytearray(packet))

    #4번 모터 속도 0으로 패킷 만들기
    def Go_to_Speed_Zero_4(self):
        self.CRCBYTE4 = (0x4 + 0xF6 + self.ACC4) & 0xFF
        self.packet4 = [4, 5, 0xF6, 0, 0, self.ACC4, self.CRCBYTE4]
    
    #4번 모터 특정 속도로 패킷 만들기
    def Go_to_target_Speed_4(self, Speed):
        if Speed > 0:
            DIR = 0
            upper_speed = (Speed >> 8) & 0x0F
            lower_speed = Speed & 0xFF
            BYTE2 = (DIR << 7) | upper_speed
            self.CRCBYTE4 = (0x4 + 0xF6 + BYTE2 + lower_speed + self.ACC4) & 0xFF
            self.packet4 = [4, 5, 0xF6, BYTE2, lower_speed, self.ACC4, self.CRCBYTE4]
        elif Speed < 0:
            DIR = 1
            upper_speed = (abs(Speed) >> 8) & 0x0F
            lower_speed = abs(Speed) & 0xFF
            BYTE2 = (DIR << 7) | upper_speed
            self.CRCBYTE4 = (0x4 + 0xF6 + BYTE2 + lower_speed + self.ACC4) & 0xFF
            self.packet4 = [4, 5, 0xF6, BYTE2, lower_speed, self.ACC4, self.CRCBYTE4]

    #가감속 이상하게 안되게 만든 모터 구동 함수
    def Active_Motor_4(self, Speed):
        if Speed != self.pre_4_Speed or (Speed != 0 and self.M4_state == self.M4_Stop):
            self.start_up_4 = 1
            if Speed == 0:
                self.Go_to_Speed_Zero_4()
            elif Speed > 15:
                if (self.M4_state == self.M4_Stop) or (self.M4_state_dir == 1):
                    self.Go_to_target_Speed_4(Speed)
                    self.M4_state_dir = 1
                elif (self.M4_state_dir == -1):
                    self.Go_to_Speed_Zero_4()
            elif Speed > 0 and Speed <= 15:
                if (self.pre_4_Speed > 15) or (self.M4_state_dir == -1):
                    self.Go_to_Speed_Zero_4()
                elif self.M4_state == self.M4_Stop:
                    self.Go_to_target_Speed_4(Speed)
                    self.M4_state_dir = 1
            elif Speed < -15:
                if (self.M4_state == self.M4_Stop) or (self.M4_state_dir == -1):
                    self.Go_to_target_Speed_4(Speed)
                    self.M4_state_dir = -1
                elif (self.M4_state_dir == 1):
                    self.Go_to_Speed_Zero_4()
            elif Speed < 0 and Speed >= -15:
                if (self.pre_4_Speed < -15) or (self.M4_state_dir == 1):
                    self.Go_to_Speed_Zero_4()
                elif (self.M4_state == self.M4_Stop):
                    self.Go_to_target_Speed_4(Speed)
                    self.M4_state_dir = -1
            self.M4_state = self.M4_Move
            self.MCU.write(bytearray(self.packet4))
            self.pre_4_Speed = Speed
        elif Speed == 0 and self.M4_state == self.M4_Move and self.start_up_4 == 1:
            self.Go_to_Speed_Zero_4()
            self.MCU.write(bytearray(self.packet4))
            self.start_up_4 = 0

class M5:
    def __init__(self, M):
        self.MCU = M

        self.start_up_5 = 0

        #가속도
        self.ACC5 = 230

        #이전 스피드 기억하기
        self.pre_5_Speed = 0

        #명령 내린 방향 기억하기
        self.M5_state_dir = 0

        #모터 드라이버에서 송신한 모터 구동에 대한 데이터
        self.M5_Stop = b'\x05\x03\xf6\x02\xfd'
        self.M5_Move = b'\x05\x03\xf6\x01\xfc'
        self.M5_state = b'\x05\x03\xf6\x02\xfd'
        self.M5_error = b'\x05\x03\xf6\x00\xfb'

        #데이터 패킷
        self.CRCBYTE5 = (0x5 + 0xF6 + self.ACC5) & 0xFF
        self.packet5 = [5, 5, 0xF6, 0, 0, self.ACC5, self.CRCBYTE5]

        #5번 모터 속도 0으로 패킷 만들기
    def Go_to_Speed_Zero_5(self):
        self.CRCBYTE5 = (0x5 + 0xF6 + self.ACC5) & 0xFF
        self.packet5 = [5, 5, 0xF6, 0, 0, self.ACC5, self.CRCBYTE5]
    
    #3번 모터 특정 속도로 패킷 만들기
    def Go_to_target_Speed_5(self, Speed):
        if Speed > 0:
            DIR = 0
            upper_speed = (Speed >> 8) & 0x0F
            lower_speed = Speed & 0xFF
            BYTE2 = (DIR << 7) | upper_speed
            self.CRCBYTE5 = (0x5 + 0xF6 + BYTE2 + lower_speed + self.ACC5) & 0xFF
            self.packet5 = [5, 5, 0xF6, BYTE2, lower_speed, self.ACC5, self.CRCBYTE5]
        elif Speed < 0:
            DIR = 1
            upper_speed = (abs(Speed) >> 8) & 0x0F
            lower_speed = abs(Speed) & 0xFF
            BYTE2 = (DIR << 7) | upper_speed
            self.CRCBYTE5 = (0x5 + 0xF6 + BYTE2 + lower_speed + self.ACC5) & 0xFF
            self.packet5 = [5, 5, 0xF6, BYTE2, lower_speed, self.ACC5, self.CRCBYTE5]

    #가감속 이상하게 안되게 만든 모터 구동 함수
    def Active_Motor_5_Speed(self, Speed):
        if Speed != self.pre_5_Speed or (Speed != 0 and self.M5_state == self.M5_Stop):
            self.start_up_5 = 1
            if Speed == 0:
                self.Go_to_Speed_Zero_5()
            elif Speed > 15:
                if (self.M5_state == self.M5_Stop) or self.M5_state_dir == 1:
                    self.Go_to_target_Speed_5(Speed)
                    self.M5_state_dir = 1
                elif self.M5_state_dir == -1:
                    self.Go_to_Speed_Zero_5()
            elif Speed > 0 and Speed <= 15:
                if (self.pre_5_Speed > 15) or (self.M5_state_dir == -1):
                    self.Go_to_Speed_Zero_5()
                elif self.M5_state == self.M5_Stop:
                    self.Go_to_target_Speed_5(Speed)
                    self.M5_state_dir = 1
            elif Speed < -15:
                if (self.M5_state == self.M5_Stop) or self.M5_state_dir == -1:
                    self.Go_to_target_Speed_5(Speed)
                    self.M5_state_dir = -1
                elif (self.M5_state_dir == 1):
                    self.Go_to_Speed_Zero_5()
            elif Speed < 0 and Speed >= -15:
                if (self.pre_5_Speed < -15) or (self.M5_state_dir == 1):
                    self.Go_to_Speed_Zero_5()
                elif self.M5_state == self.M5_Stop:
                    self.Go_to_target_Speed_5(Speed)
                    self.M5_state_dir = -1
            self.M5_state = self.M5_Move
            self.MCU.write(bytearray(self.packet5))
            self.pre_5_Speed = Speed
        elif Speed == 0 and self.M5_state == self.M5_Move and self.start_up_5 == 1:
            self.Go_to_Speed_Zero_5()
            self.MCU.write(bytearray(self.packet5))
            self.start_up_5 = 0

    def angle_to_pulses(self, angle):
        offs = 0#0.00008
        pulses = int(angle / (0.0028125 + offs))
        pulses = abs(pulses)
        return pulses
    
    def position_control(self, MCU, canid, dir, speed, acc,  pulses):

        upper_speed = (speed >> 8) & 0x0F

        lower_speed = speed & 0xFF

        byte1 = 0xFD

        byte2 = (dir << 7) | upper_speed

        byte3 = lower_speed

        byte4 = acc

        byte5 = (pulses >> 16) & 0xFF

        byte6 = (pulses >> 8) & 0xFF

        byte7 = pulses & 0xFF

        byte8 = (canid + 0xFD + byte2 + byte3 + byte4 + byte5 + byte6 +byte7) & 0xFF

        packet = [canid, 8, byte1, byte2, byte3, byte4, byte5, byte6, byte7, byte8]

        MCU.write(bytearray(packet))
    
    def Active_Motor_5(self, MCU, Gun_angle, Angle):
        Error_Pulses = 0
        Dir = 0
        if Angle >= -20 and Angle <= 30:
            if Gun_angle >= Angle:
                Dir = 0
                Error_Angle = Gun_angle - Angle
                Error_Pulses = self.angle_to_pulses(Error_Angle)
            elif Gun_angle < Angle:
                Dir = 1
                Error_Angle = Angle - Gun_angle
                Error_Pulses = self.angle_to_pulses(Error_Angle)
            self.position_control(MCU, 5, Dir, 200, 150,  Error_Pulses)


#엔코더값 0으로 초기화 함수
def set_current_axis_to_zero(MCU, canid):
    byte1 = 0x92
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    MCU.write(bytearray(packet))

#호밍?
def go_home(MCU, canid):
    byte1 = 0x91
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    MCU.write(bytearray(packet))

#엔코더 데이터 보내라는 명령어
def read_encoder2(MCU, canid):
    byte1 = 0x31
    crcbyte = (canid + byte1) & 0xFF
    packet = [canid, 2, byte1, crcbyte]
    MCU.write(bytearray(packet))

#기어비, 엔코더 데이터로 모터 각도 계산     
def calculate_encoder(byttee, Gear_ratio):
    if byttee[1] == 0:
        ee = byttee[6] + (byttee[5] << 8)+ (byttee[4] << 16)+ (byttee[3] << 24)+ (byttee[2] << 32)
        ee = (ee * 360) / ((2 ** 14) * Gear_ratio)
        return ee
    elif byttee[1] == 255:
        ee = -((255 - byttee[6]) + ((255 - byttee[5]) << 8) + ((255 - byttee[4]) << 16) + ((255 - byttee[3]) << 24) + ((255 - byttee[2]) << 32))
        ee = (ee * 360) / ((2 ** 14) * Gear_ratio)
        return ee
    
#Degrees 각도, 카메라 정보, TOF 센서 데이터로 기총부 도착해야 할 각도 계산
def Cal_Anlge_Aming_Target(CAM_data, distance_tof, Optic_Angle):
    Offset_of_tof_OpticalCenter = 0

    #쌩으로 들어온 TOF 센서 데이터
    Initial_dist_of_optical = 0

    #RCWS 몸체 크기에 따른 X, Y 거리값 (cm)
    RCWS_X_lenth = 25
    RCWS_Y_lenth = 34.5

    if CAM_data == "RGB":
        #RGB 카메라면 CCD 센서 위치에 따른 거리 추가
        Initial_dist_of_optical = 3.55
    elif CAM_data == "IR":
        #IR 카메라면 CCD 센서 위치에 따른 거리 추가
        Initial_dist_of_optical = -3.72
        
    #광학부에서부터 최종적인 타겟까지의 거리
    dist_of_target = distance_tof + Initial_dist_of_optical + Offset_of_tof_OpticalCenter
    
    #광학부 각도 라디안값으로 변환
    Radian_Optic_Angle = math.radians(Optic_Angle)
    
    #광학부 라디안 값으로 광학부에서 부터 타겟까지 X, Y 거리 구하기
    X_target_dist_from_Opt = dist_of_target * math.cos(Radian_Optic_Angle)
    Y_target_dist_from_Opt = dist_of_target * math.sin(Radian_Optic_Angle)
        
    #기총부기준 타겟의 X, Y 거리 구하기
    X_target_dist_from_gun = RCWS_X_lenth + X_target_dist_from_Opt
    Y_target_dist_from_gun = RCWS_Y_lenth - Y_target_dist_from_Opt
    
    #degrees각도로 반환
    return -math.degrees(math.atan2(Y_target_dist_from_gun, X_target_dist_from_gun))

def Motor_Calibrate(MCU, MM3, MM4, MM5):
    MM3.position_control(MCU, 3, 0, 100, 100, 50000)
    time.sleep(10)

    go_home(MCU, 3)
    go_home(MCU, 4)
    go_home(MCU, 5)
    
    # #포토 센서로 이동할 때까지 기다리는건데 이거 나중에 처리하자
    time.sleep(20)

    # #캘리브레이션
    MM3.position_control(MCU, 3, 1, 50, 100, 9000)
    MM4.position_control(4, 1, 50, 100, 16222)#16370)
    MM5.position_control(MCU, 5, 1, 50, 100, 13450)#13175)#13650)#12760)

    #캘리 다 될 때까지 기다리는건데 이거 나중에 처리하자
    time.sleep(10)
    
    #캘리된 지점에서 엔코더 값 0으로 초기화 해주는 함수
    set_current_axis_to_zero(MCU, 3)
    set_current_axis_to_zero(MCU, 4)
    set_current_axis_to_zero(MCU, 5)

    time.sleep(1)