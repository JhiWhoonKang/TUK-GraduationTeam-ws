import serial
from gun import Gun
import pandas as pd
import time 
import os

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

    def WaitData(mcu:serial.Serial, len=3):
        while(True):
            if mcu.in_waiting > len:
                break
            
    def Write(mcu:serial.Serial):
        while(True):
            if len(SendData) == 0:
                break
            mcu.write(SendData[0])
            SendData.pop(0)
            
    def WriteAndWait(mcu:serial.Serial, len=3):
        Write(teensy)
        WaitData(teensy, len)
        while (True):
            packet = Read(teensy)
            if packet == [0, 0, 0]:
                print("[ERROR] response empty")
                break
            RCWSgun.CheckPacket(packet[2])
    
    try:
        teensy = serial.Serial('/dev/ttyACM1', 500000)
    except IOError as e:
        print(e)
        print("Teensy undetect")
        exit()
    print("Teensy open")
    RCWSgun = Gun()
    
    # 장치 인식
    SendData.append(RCWSgun.ACK())          # 전송 목록에 데이터 추가 (bytearray)
    Write(teensy)                       # 전송 데이터 모두 처리
    
    ## Apply to all data processing
    WaitData(teensy)                    # 데이터가 올 때까지 대기 (예제 용)
    packet = Read(teensy)               # 데이터를 받아서 처리하기 편한 packet으로 변경 [id(int), len(int), data(bytearray)]   
    ##-----------------------------     # 데이터가 없는 경우 [0,0,0]반환
    RCWSgun.CheckPacket(packet[2])             # 데이터 처리 / 해당 데이터가 아닌 경우 return
    
    while(True):
        SendData.append(RCWSgun.ReadTriggerDegree("ready"))
        SendData.append(RCWSgun.ReadTriggerDegree("on"))
        SendData.append(RCWSgun.ReadTriggerDegree("open"))
        SendData.append(RCWSgun.ReadSingleTime())
        WriteAndWait(teensy,18)

        print("open : ", RCWSgun.trigger_open_degree,
              ", ready : ", RCWSgun.trigger_ready_degree,
              ", on : ", RCWSgun.trigger_on_degree,
              ", single time : ", RCWSgun.trigger_single_time,)
        
        print("Input Servo setting : [open/ready/on/single]")
        print("If you want done setting : out")
        com = input()
        if com == "out":
            break
        while (com == 'open' or 'ready' or 'on' or 'single'):
            print("write the value : ")
            value = int(input())
            
            if (com == 'single'):
                SendData.append(RCWSgun.SetSingleTime(value))
                Write(teensy)
                print(com , "Setting value ", value)
                
                #SendData.append(RCWSgun.ReadSingleTime())
                #WriteAndWait(teensy)
                print(com , "Setted value ", RCWSgun.trigger_single_time)
                
            else:
                SendData.append(RCWSgun.SetTriggerDegree(com, value))
                Write(teensy)
                print(com , "Setting value ", value)
                
                SendData.append(RCWSgun.ReadTriggerDegree(com))
                WriteAndWait(teensy)
                if (com == 'open'):
                    value = RCWSgun.trigger_open_degree
                elif (com == 'ready'):
                    value = RCWSgun.trigger_ready_degree
                elif (com == 'on'):
                    value = RCWSgun.trigger_on_degree
                print(com , "Setted value ", value)
            
            SendData.append(RCWSgun.Trigger(com))
            Write(teensy)
            print("Setting Done")
            
            print("if you want change another angle input : out")
            if (input() == 'out'):
                break
    
    table = [RCWSgun.trigger_open_degree,
             RCWSgun.trigger_ready_degree,
             RCWSgun.trigger_on_degree,
             RCWSgun.trigger_single_time]
    trigger_table = pd.DataFrame({'table':table })
    curret_dir =os.path.dirname(os.path.abspath(__file__))
    trigger_table.to_csv(os.path.join(curret_dir,'trigger_table.csv'), index=True)

    
    RCWSgun.TriggerTableSetup(teensy)
    SendData.append(RCWSgun.ReadTriggerDegree("ready"))
    SendData.append(RCWSgun.ReadTriggerDegree("on"))
    SendData.append(RCWSgun.ReadTriggerDegree("open"))
    SendData.append(RCWSgun.ReadSingleTime())
    WriteAndWait(teensy,18)

    print("open : ", RCWSgun.trigger_open_degree,
            ", ready : ", RCWSgun.trigger_ready_degree,
            ", on : ", RCWSgun.trigger_on_degree,
            ", single time : ", RCWSgun.trigger_single_time,)
    
    print("save parameter")

