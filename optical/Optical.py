import serial
from dataclasses import dataclass
import time
import struct
import signal

Sentry = True

def SignalHandler_SIGINT(SignalNumber,Frame):
    print('Ctrl+C Pheripheral Reset')
    print(f'Signal Number -> {SignalNumber} Frame -> {Frame}')
    global Sentry
    Sentry = False

signal.signal(signal.SIGINT,SignalHandler_SIGINT)


class Optical:
    def __init__(self):
        self.__optical_ID = 0x06
        self.__MODEMASK = 0x80
        self.__DEVICEMASK = 0x40
        self.__CAMMANMASK = 0x3F
        self.__DUMMY = [0,0,0]
        
        self.ack = False
        
        self.fixed_focus_table = [0,0,0,0,0,0,0,0,0,0]
        self.fixed_zoom_table = [0,0,0,0,0,0,0,0,0,0]
        self.current_focus = 0
        self.current_zoom = 0
        self.zoom_offset = 0
        self.zoom_offset = 0
        
        self.distance = 0
        self.strength = 0

        
    def __int_from_bytes(self,data):
        # 바이트 데이터를 정수로 변환
        return struct.unpack('<I', data)[0] 
    
    def __CHECKACK(self):
        if not self.ack:
            print("[WARN] OPT::Didn't Check the Device")
            return True
        return False
    
    def __CameraData(self,data):
        if ((data[0] & self.__MODEMASK) >> 7) == 0x00:           # 읽기 모드
            com = (data[0] & self.__CAMMANMASK)
            if com == 0x01: # 고정초점 값
                index = int.from_bytes(data[1], 'big')
                self.fixed_focus_table[index] = self.__int_from_bytes(data[2:])
                print("[INFO] OPT::Fixed table ", index, " : ", self.fixed_focus_table[index])
            if com == 0x02: # 고정배율 값
                index = int.from_bytes(data[1], 'big')
                self.fixed_zoom_table[index] = self.__int_from_bytes(data[2:])
                print("[INFO] OPT::Zoom table ", index, " : ", self.fixed_zoom_table[index])
        
    def __TOFData(self, data):
        if ((data[0] & self.__MODEMASK) >> 7) == 0x00:           # 읽기 모드
            com = (data[0] & self.__CAMMANMASK)
            if com == 0x00: 
                if (data[1] == self.__optical_ID):
                    self.ack = True
                    print("INFO] OPT::Optical Ack")
            if com == 0x01: 
                self.distance = self.__int_from_bytes(data[1:])
                print("INFO] OPT::Distance : ", self.distance)
                
                
    def CheckPacket(self, data):
        if data[0] != self.__optical_ID:
            return
        length = data[1]
        #print(data[2])
        if ((data[2][0] & self.__DEVICEMASK) >> 6) == 0x00:     # tof
           # print("tof")
            self.__TOFData(data[2])
        elif ((data[2][0] & self.__DEVICEMASK) >> 6) == 0x01:     # camera
            self.__CameraData(data[2])
          #  print("camera")
    
    def ACK(self):
        self.ack = False
        packet = [self.__optical_ID, 3, 0x00, 0x00]
        return bytearray(packet)
            
            
    def AutoOnTOF(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 1, 0x81]
        return bytearray(packet)
            
    def AutoOffTOF(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 1, 0x82]
        return bytearray(packet)
    
    def SetAutoTimeTOF(self, time):
        if self.__CHECKACK == True:
            return self.ACK()
        time = time - 10
        if time < 0:
            time = -time
        time = int(time / 10)
        if time > 0xFF:
            time = 0xFF
        print(time)
        packet = [self.__optical_ID, 2, 0x83, time]
        return bytearray(packet)
    
    def PollingDistance(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 1, 0x01]
        return bytearray(packet)
        
    def PollingStrength(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 1, 0x02]
        return bytearray(packet)
    
    def ReadFZoomTable(self, index, value):
        if self.__CHECKACK == True:
            return self.ACK()
        if (index > 10) or (index < 0):
            return self.__DUMMY
        if (value < 0) or (value > 180):
            return self.__DUMMY
        packet = [self.__optical_ID, 3, 0xC1, index, value]
        return bytearray(packet)
    
    def Reset(self):
        packet = [self.__optical_ID, 1, 0xFF]
        return bytearray(packet)
        
        
        
SendData = list()


def Read(mcu:serial.Serial):
    if mcu.in_waiting > 3:
        id = int.from_bytes(mcu.read(1), 'big')
        len = int.from_bytes(mcu.read(1), 'big')
        data = mcu.read(len)
        return [id, len, data]
    return [0,0,0]

def WaitData(mcu:serial.Serial):
    while(True):
        if mcu.in_waiting > 3:
            break
        
def Write(mcu:serial.Serial):
    while(True):
        if len(SendData) == 0:
            break
        mcu.write(SendData[0])
        SendData.pop(0)
        

        
if __name__=="__main__":
    try:
        teensy = serial.Serial('COM8', 500000)
    except IOError as e:
        print(e)
        print("Teensy undetect")
        exit()
    print("Teensy open")
    opt = Optical()
    
    # 장치 인식
    SendData.append(opt.ACK())          # 전송 목록에 데이터 추가 (bytearray)
    Write(teensy)                       # 전송 데이터 모두 처리
    
    ## Apply to all data processing
    WaitData(teensy)                    # 데이터가 올 때까지 대기 (예제 용)
    packet = Read(teensy)               # 데이터를 받아서 처리하기 편한 packet으로 변경 [id(int), len(int), data(bytearray)]   
    ##-----------------------------     # 데이터가 없는 경우 [0,0,0]반환
    opt.CheckPacket(packet)             # 데이터 처리 / 해당 데이터가 아닌 경우 return
    
    
    packet = opt.PollingDistance()
    SendData.append(packet) # 전송 목록에 데이터 추가 (bytearray)

    
    SendData.append(opt.SetAutoTimeTOF(10)) # 자동 전송 100ms로 설정
    SendData.append(opt.AutoOnTOF())

    
    
    while(Sentry):
        Write(teensy)                       # 전송 데이터 모두 처리 , 데이터가 빈경우는 전송X
        packet = Read(teensy)
        opt.CheckPacket(packet)


    print('Out of the while loop')
    print('Clean up Here')
    
    SendData.append(opt.Reset())
    Write(teensy)  