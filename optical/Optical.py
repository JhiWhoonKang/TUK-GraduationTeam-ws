import pygame
import serial
import time
from dataclasses import dataclass
import time
import struct

class Optical:
    def __init__(self, mcu):
        self.optical_ID = 0x06
        self.MODEMASK = 0x80
        self.DEVICEMASK = 0x40
        self.CAMMANMASK = 0x3F

        self.teensy = mcu
        
    def __ErrorCheck(self, data):
        if len(data) != 7:
            print("recived data length error")
            return False
        if self.optical_ID != data[0]:
            print("data length error")
            return False
        if data[1] != 5:
            print("data frame length error")
            return False
        return True
    
    def __WaitSerial(self):
        while(True):
            if self.teensy.readable():
                break
    
    def __read_int_from_bytes(self,data):
        # 바이트 데이터를 정수로 변환
        return struct.unpack('<I', data)[0] 

    def __Read(self, packet):
        self.teensy.write(bytearray(packet))
        self.__WaitSerial()
        res = self.teensy.read(7)
        if self.__ErrorCheck(res):
            return self.__read_int_from_bytes(res[2:6])
        
    def ACK(self):
        CRC = (self.optical_ID + 0x00 + 0x00) & 0xFF
        ACKPACKET = [self.optical_ID, 3, 0x00, 0x00, CRC]
        res = self.__Read(ACKPACKET)
        if res == self.optical_ID:
            print("ACK Success ID : ", res)
            
            
    def AutoOnTOF(self):
        CRC = (self.optical_ID + 0x82 + 0x00) & 0xFF
        ACKPACKET = [self.optical_ID, 3, 0x82, 0x00, CRC]
        res = self.__Read(ACKPACKET)
        if res == 1:
            print("suucess")
            
    def AutoOffTOF(self):
        CRC = (self.optical_ID + 0x81 + 0x00) & 0xFF
        ACKPACKET = [self.optical_ID, 3, 0x81, 0x00, CRC]
        res = self.__Read(ACKPACKET)
        if res == 1:
            print("suucess")
    
    def PollingDistance(self):
        CRC = (self.optical_ID + 0x01 + 0x00) & 0xFF
        ACKPACKET = [self.optical_ID, 3, 0x01, 0x00, CRC]
        res = self.__Read(ACKPACKET)
        print("distance : ", res)
        return res
        
    def PollingStrength(self):
        CRC = (self.optical_ID + 0x02 + 0x00) & 0xFF
        ACKPACKET = [self.optical_ID, 3, 0x02, 0x00, CRC]
        res = self.__Read(ACKPACKET)
        print("strength : ", res)
        return res
        
        
        
if __name__=="__main__":
    try:
        teensy = serial.Serial('COM34', 250000)
    except IOError as e:
        print(e)
        print("Teensy undetect")
        exit()
    print("Teensy open")
    optical = Optical(teensy)
    
    optical.ACK()
    time.sleep(1)
    
    while(True) :
        optical.PollingStrength()
        optical.PollingDistance()
        time.sleep(1)