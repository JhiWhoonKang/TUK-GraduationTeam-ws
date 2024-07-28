import serial
from Optical import Optical
import threading
import cv2
import time
import pandas as pd
import os

def camera():
    print("camera")
    cap = cv2.VideoCapture(2)
    while(True):
        ret, frame = cap.read()

        if ret:
            cv2.imshow("camera test", frame)
            cv2.waitKey(2)
        


if __name__=="__main__":

    SendData = list()

    def Read(mcu:serial.Serial):
        if mcu.in_waiting > 3:
            id = int.from_bytes(mcu.read(1), 'big')
            len = int.from_bytes(mcu.read(1), 'big')
            data = mcu.read(len)
            # print("ID : ", id, "len : ", len, "data : ", data)
            return [id, len, data]
        return [0,0,0]

    def WaitData(mcu:serial.Serial, len):
        while(True):
            if mcu.in_waiting > len:
                break
            
    def Write(mcu:serial.Serial):
        while(True):
            if len(SendData) == 0:
                break
            mcu.write(SendData[0])
            SendData.pop(0)
            
    def WriteAndWait(mcu:serial.Serial, len):
        Write(teensy)
        WaitData(teensy, len)
        while (True):
            packet = Read(teensy)
            if packet == [0, 0, 0]:
                print("[ERROR] response empty")
                break
            opt.CheckPacket(packet[2])
        

    try:
        teensy = serial.Serial('/dev/ttyACM1', 500000)
    except IOError as e:
        print(e)
        print("Teensy undetect")
        exit()
    print("Teensy open")

    camera_thread = threading.Thread(target = camera)
    camera_thread.start()
    time.sleep(3)

    opt = Optical()
    
    # 장치 인식
    SendData.append(opt.ACK())          # 전송 목록에 데이터 추가 (bytearray)
    Write(teensy)                       # 전송 데이터 모두 처리
    
    ## Apply to all data processing
    WaitData(teensy, 1)                    # 데이터가 올 때까지 대기 (예제 용)
    packet = Read(teensy)               # 데이터를 받아서 처리하기 편한 packet으로 변경 [id(int), len(int), data(bytearray)]   
    ##-----------------------------     # 데이터가 없는 경우 [0,0,0]반환
    opt.CheckPacket(packet[2])             # 데이터 처리 / 해당 데이터가 아닌 경우 return
    
    for i in range(10):
        SendData.append(opt.GetZoomTableValue(i))
        SendData.append(opt.GetFocusTableValue(i))
        WriteAndWait(teensy, 12)

    while(True):
        print("Input Table index / if you want done : -1")
        index = int(input())
        if (index == -1):
            break

        SendData.append(opt.GetZoomTableValue(index))
        SendData.append(opt.GetFocusTableValue(index))
        WriteAndWait(teensy, 12)
        print(index, " zoom  value : ", opt.fixed_zoom_table[index])
        print(index, " focus value : ", opt.fixed_focus_table[index])
        
        print("Type that you want change value : [zoom/focus]")
        com = input()
        if (com == 'zoom'):
            print("write the value : ")
            value = int(input())
            SendData.append(opt.SetZoomTableValue(index, value))
            print(com , " value from ", opt.fixed_zoom_table[index], " to ", value)
        elif (com == 'focus'):
            print("write the value : ")
            value = int(input())
            SendData.append(opt.SetFocusTableValue(index, value))
            print(com , " value from ", opt.fixed_focus_table[index], " to ", value)
        
        Write(teensy)
        SendData.append(opt.GetZoomTableValue(index))
        SendData.append(opt.GetFocusTableValue(index))
        WriteAndWait(teensy, 14)
        print(index, " zoom  value : ", opt.fixed_zoom_table[index])
        print(index, " focus value : ", opt.fixed_focus_table[index])
        
        print("Setting Done\n\n")
    
    print("zoom tabe : ", opt.fixed_zoom_table)
    print("focus tabe : ", opt.fixed_focus_table)
    
    
    camera_table = pd.DataFrame({'zoom_table':opt.fixed_zoom_table,
                                 'focus_table':opt.fixed_focus_table})
    curret_dir =os.path.dirname(os.path.abspath(__file__))
    camera_table.to_csv(os.path.join(curret_dir,'camera_table.csv'), index=True)

    print("save parameter")