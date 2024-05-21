import serial
import time
from dataclasses import dataclass
import time
import struct


class AHRS:
    def __init__(self, mcu):
        self.ahrs = mcu
        self.temperature = 0
        
    def __p_to_c__(self, packet_):
        packet = packet_ + '\n'
        hes = list()
        for char in packet:
            asci = ord(char)
            hes.append(asci)
        return hes

    def __tofloat__(self,data: str):
        decoded_data = data.decode('utf-8')
        split_data = decoded_data.split(',')
        float_list = [float(item) for item in split_data]
        return float_list
        
    def BufClear(self):
        self.ahrs.read_all()
        
    def Read(self, com):
        self.ahrs.write(bytearray(self.__p_to_c__(com)))
        result = self.ahrs.read_until()[len(com)+1:-2]
        return self.__tofloat__(result)
        
    def Write(self, com, data):
        packet = com + '=' + str(data)
        self.ahrs.write(bytearray(self.__p_to_c__(packet)))
        print(self.ahrs.read_until()[2:-2])
        
    def Writ(self, com):
        packet = com
        self.ahrs.write(bytearray(self.__p_to_c__(packet)))
        print(self.ahrs.read_until()[2:-2])
        
    
        
            
            
        
        
        
if __name__=="__main__":
    try:
        teensy = serial.Serial('COM5', 115200)
    except IOError as e:
        print(e)
        print("ahrs undetect")
        exit()
    print("ahrs open")
    ahrs = AHRS(teensy)
    time.sleep(1)
    ahrs.Write('b1', 921600)
    ahrs.Writ('fw')
    ahrs.Writ('rd')
    time.sleep(3)
    ahrs.BufClear()
    while(True) :
        print(ahrs.Read('e'))
        time.sleep(1)