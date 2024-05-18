#include "AHRS.h"

AHRS::AHRS(HardwareSerialIMXRT *_ahrs)
: RequestQueue(50) {
    ahrs = _ahrs;
    sync = false;
    debug = NULL;
    delta_T = millis();
    data_rate = 10;
    auto_request = false;
    reset = false;
    CanSendQueue = NULL;
}

void AHRS::SetDebugSerial(usb_serial_class *_debug){
    debug = _debug;
    debug->println("AHRS::Set Debug");    
}

void AHRS::UnsetDebugSerial() {
    AHRSDEBUG debug->println("AHRS::Unset Debug");    
    debug = NULL;
}

void AHRS::SetDataQueue(DataQueue *_CanSendQueue) {
    CanSendQueue = _CanSendQueue;
}

void AHRS::Request(String data) {
    data = Check(data);
    if (data == "") return;
    data = data + "\n";
    ahrs->print(data);
}

void AHRS::RequestAdd(String data) {
    // Save
    data = Check(data);

    AHRSDEBUG debug->print("AHRS::RequestAdd : ");
    AHRSDEBUG debug->println(data);

    if (data == "") return;

    RequestQueue.push(data);

    if (reset) return; // ahrs reset 중

    // Activate
    if (RequestQueue.size() != 0) {
        String d = RequestQueue.use();
        if (d == "") return;
        if (d == "rd") reset = true;
        d = d+"\n";
        ahrs->print(d);
    }
}

bool AHRS::Syncdata(uint8_t port, uint16_t cycle, uint16_t data){
    if (port > 1) return false;
    if (cycle == 0 || cycle > 60000) return false;
    
    ahrs->print("so=0\n");
    if (!ahrs->available()){}
    String a = ahrs->readString();
    AHRSDEBUG debug->print("AHRS::Syncdata : ");
    AHRSDEBUG debug->println(a);

    ahrs->write(data);
    ahrs->print("\n");
    if (!ahrs->available()){}
    a = ahrs->readString();
    AHRSDEBUG debug->print("AHRS::Syncdata : ");
    AHRSDEBUG debug->println(a);
    
    // if ((data & 0x0001) == 0x0001) DEBUG_("0x0001");
    return true;
}

void AHRS::SaveData(float *storage, int size, String data) {
    AHRSDEBUG debug->print("AHRS::SaveData : ");
    AHRSDEBUG debug->println(data);
    int index = 0;
    data = data.substring(data.indexOf("="));
    AHRSDEBUG debug->print("AHRS::SaveData : ");
    AHRSDEBUG debug->println(data);
    for (int i = 0; i < size; i++) {
        int index1 = data.indexOf(',', index+1);
        String s1;
        if (index1 == -1)
            s1 = data.substring(index+1);
        else
            s1 = data.substring(index+1, index1+1);
        *(storage+i) = s1.toFloat();
        index = index1;
    }
}

void AHRS::Print(float data, bool end) {
    AHRSDEBUG {
        debug->print(data);
        debug->print(" ");
        if (end) debug->println(" ");
    }
}

void AHRS::Print(float *data, int size, bool end) {
    AHRSDEBUG {
        debug->print("(");
        for (int i = 0; i < size; i++)
            Print(*(data+i), false);
        debug->print(")");
        if (end) debug->println(" ");
    }
}

String AHRS::Check(String data) {
    if (data == "rd"){}
    else if (data == "pv"){}
    else if (data[0] == 0xE6) {data = "pv";}
    else if (data == "t"){}
    else if (data == "a"){}
    else if (data == "g"){}
    else if (data == "m"){}
    else if (data == "e"){}
    else if (data == "q"){}
    else if (data == "v"){}
    else if (data == "f"){}
    else {return "";}

    return data;
}

void AHRS::AutoRequestAdd(String data) {
    AHRSDEBUG debug->print("AHRS::AutoRequestAdd : ");
    AHRSDEBUG debug->println(data);

    if (data == "pv")       voltage.use = true;
    else if (data == "t")   temperatere.use = true;
    else if (data == "a")   accel.use = true;
    else if (data == "g")   gyro.use = true;
    else if (data == "m")   mag.use = true;
    else if (data == "e")   euler.use = true;
    else if (data == "q")   quataniun.use = true;
    else if (data == "v")   velocity.use = true;
    else if (data == "f")   vibration.use = true;
}

void AHRS::AutoRequestDelete(String data) {
    AHRSDEBUG debug->print("AHRS::AutoRequestDelete : ");
    AHRSDEBUG debug->println(data);
    
    if (data == "pv")       voltage.use = false;
    else if (data == "t")   temperatere.use = false;
    else if (data == "a")   accel.use = false;
    else if (data == "g")   gyro.use = false;
    else if (data == "m")   mag.use = false;
    else if (data == "e")   euler.use = false;
    else if (data == "q")   quataniun.use = false;
    else if (data == "v")   velocity.use = false;
    else if (data == "f")   vibration.use = false;
}

void AHRS::AutoRequestOn() {
    auto_request = true;
    AHRSDEBUG debug->printf("AHRS::AutoRequestOn\n");
}

void AHRS::AutoRequestOff() {
    auto_request = false;
    AHRSDEBUG debug->printf("AHRS::AutoRequestOff\n");
}
void AHRS::AutoRate(int num) {
    if (num < 10) return;
    AHRSDEBUG debug->printf("AHRS::AutoRate : %d\n", num);
    data_rate = num;
    return;
}

void AHRS::AutoRequest() {
    if (!auto_request) return;
    if (millis() - delta_T < data_rate) return;
    delta_T = millis();
    if (voltage.use) Request("pv");
    if (temperatere.use) Request("t");
    if (accel.use) Request("a");
    if (gyro.use) Request("g");
    if (mag.use) Request("m");
    if (euler.use) Request("e");
    if (quataniun.use) Request("q");
    if (velocity.use) Request("v");
    if (vibration.use) Request("f");
}

void AHRS::AutoRead() {
    if (!ahrs->available()) return;

    String a = ahrs->readStringUntil('\n');
    AHRSDEBUG debug->print("AHRS::AutoRead : ");
    AHRSDEBUG debug->println(a);

    int index = a.indexOf("=");
    String d;
    if (index != -1) d = a.substring(0,index);
    else if (a[0] == 254) {
        d = "rd";
        reset = false;
    }
    RequestQueue.popIndex(RequestQueue.findIndex(d));
    AHRSDEBUG if (RequestQueue.size() > 0) RequestQueue.printQueue();
    DataItem item;
    memset(&item, 0, sizeof(item));
    item.len = 6;
    switch (a[0]) {
        case 'p':
            if (a[1] != 'v') break;
            SaveData(&voltage.data, 1, a);
            if (CanSendQueue != NULL) {
                item.data[1] = ((uint8_t)'p') + ((uint8_t)'v');
                memcpy(&item.data[2], &voltage.data, 4);
                CanSendQueue->push(item);
            }
            Print(voltage.data);
        break;
        case 't':
            SaveData(&temperatere.data,1, a);
            if (CanSendQueue != NULL) {
                item.data[1] = (uint8_t)'t';
                memcpy(&item.data[2], &temperatere.data, 4);
                CanSendQueue->push(item);
            }
            Print(temperatere.data);
        break;
        case 'a':
            SaveData(accel.data, 3,a);
            if (CanSendQueue != NULL) {
                for (int i =0; i < 3; i++) {
                    item.data[0] = 0x40 + (uint8_t)i;
                    item.data[1] = (uint8_t)'a';
                    memcpy(&item.data[2], &accel.data[i], 4);
                    CanSendQueue->push(item);
                }
            }
            Print(accel.data, 3);
        break;
        case 'g':
            SaveData(gyro.data, 3, a);
            if (CanSendQueue != NULL) {
                for (int i =0; i < 3; i++) {
                    item.data[0] = 0x40 + (uint8_t)i;
                    item.data[1] = (uint8_t)'g';
                    memcpy(&item.data[2], &gyro.data[i], 4);
                    CanSendQueue->push(item);
                }
            }
            Print(gyro.data, 3);
        break;
        case 'm':
            SaveData(mag.data, 3,a);
            if (CanSendQueue != NULL) {
                for (int i =0; i < 3; i++) {
                    item.data[0] = 0x40 + (uint8_t)i;
                    item.data[1] = (uint8_t)'m';
                    memcpy(&item.data[2], &mag.data[i], 4);
                    CanSendQueue->push(item);
                }
            }
            Print(mag.data, 3);
        break;
        case 'e':
            SaveData(euler.data, 3,a);
            if (CanSendQueue != NULL) {
                for (int i =0; i < 3; i++) {
                    item.data[0] = 0x40 + (uint8_t)i;
                    item.data[1] = (uint8_t)'e';
                    memcpy(&item.data[2], &euler.data[i], 4);
                    CanSendQueue->push(item);
                }
            }
            Print(euler.data, 3);
        break;
        case 'q':
            SaveData(quataniun.data, 4,a);
            if (CanSendQueue != NULL) {
                for (int i =0; i < 4; i++) {
                    item.data[0] = 0x40 + (uint8_t)i;
                    item.data[1] = (uint8_t)'q';
                    memcpy(&item.data[2], &quataniun.data[i], 4);
                    CanSendQueue->push(item);
                }
            }
            Print(quataniun.data, 4);
        break;
    }
    return;
}