import serial
from dataclasses import dataclass
import time
import struct
import signal


class RF:
    def __init__(self):
        self.__RF_ID = 0x02
        
        self.AHRS_Y = 0
        self.AHRS_Z = 0
        self.data1 = 0
        self.data2 = 0
        self.data3 = 0
        self.data4 = 0
        
        
    def Senddata(self, a, b, c, d):
        packet = [self.__gun_ID, 4, a, b, c, d]
        return bytearray(packet)
    
    def CheckPacket(self, data):
        if (len(data) == 8):
            self.AHRS_Y = (int(data[0]) << 4) + int(data[1])
            self.AHRS_Y = (int(data[2]) << 4) + int(data[3])
            self.data1 = int(data[4])
            self.data2 = int(data[5])
            self.data3 = int(data[6])
            self.data4 = int(data[7])
            return True
        return False


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
            print(data)
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
        teensy = serial.Serial('COM13', 500000)
    except IOError as e:
        print(e)
        print("Teensy undetect")
        exit()
    print("Teensy open")
    rf = RF()
    
    while(Sentry):
        packet = Read(teensy)
        if (rf.CheckPacket(packet)):
            print(rf.AHRS_Y, rf.AHRS_Z, rf.data1, rf.data2, rf.data3, rf.data4)
        
        
    
    print('Out of the while loop')
    print('Clean up Here')
    
    exit()