import serial
from dataclasses import dataclass
import time
import struct
import signal



class Gun:
    def __init__(self):
        self.__gun_ID = 0x07
        self.__MODEMASK = 0x80
        self.__DEVICEMASK = 0x40
        self.__CAMMANMASK = 0x3F
        self.__AHRS = 0x40
        self.READ = 0x00
        self.WRITE = 0x80
        self.DEVICE = 0x00
        self.AHRS = 0x40
        
        self.ack = False
        
        self.trigger_status = 0 # open 0 , ready 2, on 3
        self.trigger_open_degree = 110
        self.trigger_single_time = 100
        self.trigger_ready_degree = 78
        self.trigger_on_degree = 60
        
        self.voltage = 0
        self.temperature = 0
        self.accel = [0,0,0]
        self.gyro = [0,0,0]
        self.mag = [0,0,0]
        self.euler = [0,0,0]
        self.quatanion = [0,0,0,0]
        
        
    
    def __read_int_from_bytes(self,data):
        # 바이트 데이터를 정수로 변환
        return struct.unpack('<I', data)[0] 
    
    def __DeviceData(self, data):
        if ((data[0] & self.__MODEMASK) >> 7) == 0x00:           # 읽기 모드
            com = data[0] & self.__CAMMANMASK
            if com == 0x00:        # 장치 확인 완료
                if data[1] == self.__gun_ID:
                    self.ack = True
                    print("[INFO] GUN::Gun Ack")
            elif com == 0x01:
                if data[1] == 0x00:
                    print("[INFO] GUN::Gun POWER Off")
                elif data[1] == 0x01:
                    print("[INFO] GUN::Gun POWER On")
            elif com == 0x10:
                if data[1] == 0x00:
                    self.trigger_status = 0
                    print("[INFO] GUN::Trigger Open")
                elif data[1] == 0x02:
                    self.trigger_status = 2
                    print("[INFO] GUN::Trigger Ready")
                elif data[1] == 0x03:
                    self.trigger_status = 3
                    print("[INFO] GUN::Trigger On")
            elif com == 0x14:
                self.trigger_open_degree = data[1]
                print("[INFO] GUN::Open Degree : ", self.trigger_open_degree)
            elif com == 0x15:
                self.trigger_single_time = self.__int_from_bytes(data[1])
                print("[INFO] GUN::Single time : ", self.trigger_single_time)
            elif com == 0x16:
                self.trigger_on_degree = data[1]
                print("[INFO] GUN::Single time : ", self.trigger_on_degree)
            elif com == 0x17:
                self.trigger_ready_degree = data[1]
                print("[INFO] GUN::Ready Degree : ", self.trigger_ready_degree)
                
    def __AHRSData(self, data):
        if ((data[0] & self.__MODEMASK) >> 7) == 0x00:           # 읽기 모드
            index = (data[0] & self.__CAMMANMASK)
            if (data[1] == 0xE6): # "p + v"
                self.voltage = self.__float_from_bytes(data[2:])
                print("[INFO] GUN::Voltage : ", self.voltage)
            if (data[1] == 0x74): # "t"
                self.temperature = self.__float_from_bytes(data[2:])
                print("[INFO] GUN::Temperature : ", self.temperature)
            elif data[1] == 0x61: # "a"
                self.accel[index] = self.__float_from_bytes(data[2:])
                if index == 2:
                    print("[INFO] GUN::Accel : ", self.accel)
            elif data[1] == 0x67: # "g"
                self.gyro[index] = self.__float_from_bytes(data[2:])
                if index == 2:
                    print("[INFO] GUN::Gyro : ", self.gyro)
            elif data[1] == 0x6D: # "m"
                self.mag[index] = self.__float_from_bytes(data[2:])
                if index == 2:
                    print("[INFO] GUN::Mag : ", self.mag)
            elif data[1] == 0x65: # "e"
                self.euler[index] = self.__float_from_bytes(data[2:])
                if index == 2:
                    print("[INFO] GUN::Euler : ", self.euler)
            elif data[1] == 0x71: # "q"
                self.quatanion[index] = self.__float_from_bytes(data[2:])
                if index == 3:
                    print("[INFO] GUN::Quatanion : ", self.quatanion)
            
            
            
    def CheckPacket(self, data):
        if ((data[0] & self.__DEVICEMASK) >> 6) == 0x00:     # 장치
            self.__DeviceData(data[2])
        elif ((data[0] & self.__DEVICEMASK) >> 6) == 0x01:     # AHRS
            self.__AHRSData(data[2])
            
    
    def __CHECKACK(self):
        if not self.ack:
            print("[WARN] GUN::Didn't Check the Device")
            return True
        return False
    
    def ReadGunPower(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__gun_ID, 1, 0x01]
        return bytearray(packet)
        
    def ReadTriggerStatus(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__gun_ID, 1, 0x10]
        return bytearray(packet)
    
    def ReadSingleTime(self):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__gun_ID, 1, 0x15]
        return bytearray(packet)
    
    def ReadTriggerDegree(self, data:str):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = list()
        if data=="ready":
            packet = [self.__gun_ID, 1, 0x17]
        elif data=="fire": # on
            packet = [self.__gun_ID, 1, 0x16]
        elif data=="open":
            packet = [self.__gun_ID, 1, 0x14]
        return bytearray(packet)
    
    def ReadAHRS(self, name:str):
        if self.__CHECKACK == True:
            return self.ACK()
        
        byte_data = name.encode('utf-8')
        if name == "pv" or name == "rd":
            byte_data = byte_data[0] + byte_data[1]
        else:
            byte_data = int.from_bytes(byte_data, 'big')
        packet = [self.__gun_ID, 2, self.__AHRS, byte_data]
        return bytearray(packet)
    
    def SetLaser(self, data:bool) :
        if self.__CHECKACK == True:
            return self.ACK()
        packet = [self.__gun_ID, 2, self.WRITE+self.DEVICE+0x01, data]
        return bytearray(packet)
    
    def SetGunPower(self, data:bool):
        return self.SetLaser(data)
    
    def Trigger(self, data:str) : ## open, single, ready, on ##
        if self.__CHECKACK == True:
            return self.ACK()
        packet = list()
        
        if data == "open":
            packet = [self.__gun_ID, 1, self.WRITE+self.DEVICE+0x10]
        elif data == "single":
            packet = [self.__gun_ID, 1, self.WRITE+self.DEVICE+0x11]
        elif data == "ready":
            packet = [self.__gun_ID, 1, self.WRITE+self.DEVICE+0x12]
        elif data == "fire": # on
            packet = [self.__gun_ID, 1, self.WRITE+self.DEVICE+0x13]
        else:
            packet = [0,0,0]
        
        return bytearray(packet)
    
    def SetTriggerDegree(self, data:str, data2:int):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = list()
        
        if data2 > 0xFF or data2 < 0:
            print("[ERROR] GUN::SetTriggerDegree degree value error")
            packet = [0,0,0]
        else: 
            if data == "open":
                packet = [self.__gun_ID, 2, self.WRITE+self.DEVICE+0x14]
            elif data == "ready":
                packet = [self.__gun_ID, 2, self.WRITE+self.DEVICE+0x17]
            elif data == "fire": # on
                packet = [self.__gun_ID, 2, self.WRITE+self.DEVICE+0x16]
            else:
                print("[ERROR] GUN::SetTriggerDegree name error")
                packet = [0,0,0]
        
        return bytearray(packet)
    
    def SetSingleTime(self, data:int):
        if self.__CHECKACK == True:
            return self.ACK()
        packet = list()
        packed_data = struct.pack('<i', data)
        if data < 0:
            print("[ERROR] GUN::SetSingleTime time isn't minus")
            packet = [0,0,0]
        else:
            packet = [self.__gun_ID, 5, self.WRITE+self.DEVICE+0x15]
            packet.append(packed_data[0])
            packet.append(packed_data[1])
            packet.append(packed_data[2])
            packet.append(packed_data[3])
            
        return bytearray(packet)
        
            
    def __int_from_bytes(self,data):
        return struct.unpack('<I', data)[0] 
    
    def __float_from_bytes(self,data):
        return struct.unpack('<f', data)[0] 
        
    def ACK(self):
        self.ack = False
        packet = [self.__gun_ID, 0x01, 0x00]
        return bytearray(packet)
    
    def Reset(self):
        packet = [self.__gun_ID, 0x01, 0xFF]
        return bytearray(packet)
    
    def Initialization(self,mcu):
        packet = list()
        while (True):
            mcu.write(self.ACK())
            time.sleep(0.5)
            if mcu.in_waiting > 3:
                id = int.from_bytes(mcu.read(1), 'big')
                len = int.from_bytes(mcu.read(1), 'big')
                data = mcu.read(len)
                packet = [id, len, data]
                self.CheckPacket(packet)
                if self.ack == True:
                    break
        
            
       
       
        

        

        
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
        teensy = serial.Serial('COM6', 500000)
    except IOError as e:
        print(e)
        print("Teensy undetect")
        exit()
    print("Teensy open")
    gun = Gun()
    
    # 장치 인식
    SendData.append(gun.ACK())          # 전송 목록에 데이터 추가 (bytearray)
    Write(teensy)                       # 전송 데이터 모두 처리
    
    ## Apply to all data processing
    WaitData(teensy)                    # 데이터가 올 때까지 대기 (예제 용)
    packet = Read(teensy)               # 데이터를 받아서 처리하기 편한 packet으로 변경 [id(int), len(int), data(bytearray)]   
    ##-----------------------------     # 데이터가 없는 경우 [0,0,0]반환
    gun.CheckPacket(packet)             # 데이터 처리 / 해당 데이터가 아닌 경우 return
    
    SendData.append(gun.ReadAHRS("pv")) # 전송 목록에 데이터 추가 (bytearray)
    SendData.append(gun.ReadAHRS("t")) # 전송 목록에 데이터 추가 (bytearray)
    SendData.append(gun.ReadAHRS("a")) # 전송 목록에 데이터 추가 (bytearray)
    SendData.append(gun.ReadAHRS("g")) # 전송 목록에 데이터 추가 (bytearray)
    SendData.append(gun.ReadAHRS("m")) # 전송 목록에 데이터 추가 (bytearray)
    SendData.append(gun.ReadAHRS("e")) # 전송 목록에 데이터 추가 (bytearray)
    SendData.append(gun.ReadAHRS("q")) # 전송 목록에 데이터 추가 (bytearray)
    Write(teensy)                       # 전송 데이터 모두 처리
    
    while(Sentry):
        packet = Read(teensy)
        gun.CheckPacket(packet)
        
    
    print('Out of the while loop')
    print('Clean up Here')
    
    SendData.append(gun.Reset())
    Write(teensy)  
    