import serial
import Optical


if __name__=="__main__":

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
            
    def WriteAndWait(mcu:serial.Serial):
        Write(teensy)
        WaitData(teensy)
        while (True):
            packet = Read(teensy)
            if packet == [0, 0,0]:
                break
            opt.CheckPacket(packet)
        

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
    
    while(True):
        print("Input Table index")
        index = input()
        SendData.append(opt.GetZoomTableValue(index))
        SendData.append(opt.GetFocusTableValue(index))
        WriteAndWait(teensy)

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
        
        WriteAndWait(teensy)
        
            
            

