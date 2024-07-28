import serial
from dataclasses import dataclass
import time
import struct
import signal
import pandas as pd
import os



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
        self.autodelay = 100
        
        self._info = False

        
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
            if com == 0x01: # 고정 zoom 값
                index = int(data[1])
                self.fixed_zoom_table[index] = self.__int_from_bytes(data[2:])
                if self._info:
                    print("[INFO] OPT::Fixed Zoom table ", index, " : ", self.fixed_zoom_table[index])
            if com == 0x02: # 고정 focus 값
                index = int(data[1])
                self.fixed_focus_table[index] = self.__int_from_bytes(data[2:])
                if self._info:
                    print("[INFO] OPT::Fixed Focus table ", index, " : ", self.fixed_focus_table[index])
            if com == 0x03: # 현재 배율값
                value = self.__int_from_bytes(data[2:])
                if self._info:
                    print("[INFO] OPT::Current Zoom Value : ", value)
            if com == 0x04: # 현재 초점값
                value = self.__int_from_bytes(data[2:])
                if self._info:
                    print("[INFO] OPT::Current Focus Value : ", value)
        
    def __TOFData(self, data):
        if ((data[0] & self.__MODEMASK) >> 7) == 0x00:           # 읽기 모드
            com = (data[0] & self.__CAMMANMASK)
            if com == 0x00: 
                if (data[1] == self.__optical_ID):
                    self.ack = True
                    if self._info:
                        print("[INFO] OPT::Optical Ack")
            if com == 0x01: 
                self.distance = self.__int_from_bytes(data[1:])
                if self._info: 
                    print("[INFO] OPT::Distance : ", self.distance)
            if com == 0x02: 
                self.autodelay = self.__int_from_bytes(data[1:])
                if self._info:
                    print("[INFO] OPT::AutoDelay : ", self.autodelay)
                
    def Info(self, bool:bool):
        self._info = bool
    
    def CheckPacket(self, data):
        if ((data[0] & self.__DEVICEMASK) >> 6) == 0x00:     # tof
            self.__TOFData(data)
        elif ((data[0] & self.__DEVICEMASK) >> 6) == 0x01:     # camera
            self.__CameraData(data)
    
    def ACK(self):
        self.ack = False
        packet = [self.__optical_ID, 3, 0x00, 0x00]
        return bytearray(packet)
            
            
    def SetAutoTOF(self, data:str):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = list()
        if data == "on":
            packet = [self.__optical_ID, 1, 0x81]
        elif data == "off":
            packet = [self.__optical_ID, 1, 0x82]
        else:
            packet = [0,0,0]
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
    
    def ReadAutoTimeTOF(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 1, 0x03]
        return bytearray(packet)
    
    def SetZoomTableValue(self, index, value):
        if self.__CHECKACK == True:
            return self.ACK()
        if (index > 10) or (index < 0):
            return self.__DUMMY
        if (value < 0) or (value > 180):
            return self.__DUMMY
        packet = [self.__optical_ID, 3, 0xC1, index, value]
        return bytearray(packet)
    
    def SetFocusTableValue(self, index, value):
        if self.__CHECKACK == True:
            return self.ACK()
        if (index > 10) or (index < 0):
            return self.__DUMMY
        if (value < 0) or (value > 180):
            return self.__DUMMY
        packet = [self.__optical_ID, 3, 0xC2, index, value]
        return bytearray(packet)
    
    def SetZoomCurrent(self, value):
        if self.__CHECKACK == True:
            return self.ACK()
        if (value < 0) or (value > 180):
            return self.__DUMMY
        packet = [self.__optical_ID, 3, 0xC3, 0x00, value]
        return bytearray(packet)
    
    def SetFocusCurrent(self, value):
        if self.__CHECKACK == True:
            return self.ACK()
        if (value < 0) or (value > 180):
            return self.__DUMMY
        packet = [self.__optical_ID, 3, 0xC4, 0x00, value]
        return bytearray(packet)
    
    def SetAutoZoom(self, value):
        if self.__CHECKACK == True:
            return self.ACK()
        if (value < 0) or (value > 180):
            return self.__DUMMY
        packet = [self.__optical_ID, 3, 0xC5, 0x00, value]
        return bytearray(packet)
    
    def SetAutoFocus(self, value):
        if self.__CHECKACK == True:
            return self.ACK()
        if (value < 0) or (value > 180):
            return self.__DUMMY
        packet = [self.__optical_ID, 3, 0xC6, 0x00, value]
        return bytearray(packet)
    
    def UseFixedCameraTable(self, index):
        if self.__CHECKACK == True:
            return self.ACK()
        if (index > 10) or (index < 0):
            return self.__DUMMY
        packet = [self.__optical_ID, 2, 0xC7, index]
        return bytearray(packet)
    
    def UseAutoFocus(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 1, 0xC8]
        return bytearray(packet)
    
    def GetZoomTableValue(self, index):
        if self.__CHECKACK == True:
            return self.ACK()
        if (index > 10) or (index < 0):
            if self._info:
                print("[ERROR] index error")
            return self.__DUMMY
        packet = [self.__optical_ID, 0x2, 0x41, index]
        return bytearray(packet)
    
    def GetFocusTableValue(self, index):
        if self.__CHECKACK == True:
            return self.ACK()
        if (index > 10) or (index < 0):
            return self.__DUMMY
        packet = [self.__optical_ID, 0x2, 0x42, index]
        return bytearray(packet)
    
    def GetZoomCurrent(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 2, 0x43]
        return bytearray(packet)
    
    def GetFocusCurrent(self,):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__optical_ID, 2, 0x44]
        return bytearray(packet)
    
    def Reset(self):
        packet = [self.__optical_ID, 1, 0xFF]
        return bytearray(packet)
    
    def CameraTableSetup(self, mcu:serial):
        curret_dir =os.path.dirname(os.path.abspath(__file__))
        camera_table =pd.read_csv(os.path.join(curret_dir,'camera_table.csv'))
        zoom_table = camera_table['zoom_table'].tolist()[:10]
        focus_table = camera_table['focus_table'].tolist()[:10]
        
        for index in range(10):
            packet = self.SetZoomTableValue(index, zoom_table[index])
            mcu.write(packet)
            packet = self.SetFocusTableValue(index, focus_table[index])
            mcu.write(packet)

        mcu.write(self.UseFixedCameraTable(0))
        
        

        

        
if __name__=="__main__":
    Sentry = True

    def SignalHandler_SIGINT(SignalNumber,Frame):
        print('Ctrl+C Pheripheral Reset')
        print(f'Signal Number -> {SignalNumber} Frame -> {Frame}')
        global Sentry
        Sentry = False

    signal.signal(signal.SIGINT,SignalHandler_SIGINT)

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
    opt.CheckPacket(packet[2])             # 데이터 처리 / 해당 데이터가 아닌 경우 return
    
    
    packet = opt.PollingDistance()
    SendData.append(packet) # 전송 목록에 데이터 추가 (bytearray)

    
    SendData.append(opt.SetAutoTimeTOF(10)) # 자동 전송 100ms로 설정
    SendData.append(opt.AutoOnTOF())

    
    
    while(Sentry):
        Write(teensy)                       # 전송 데이터 모두 처리 , 데이터가 빈경우는 전송X
        packet = Read(teensy)
        opt.CheckPacket(packet[2])


    print('Out of the while loop')
    print('Clean up Here')
    
    SendData.append(opt.Reset())
    Write(teensy)  
