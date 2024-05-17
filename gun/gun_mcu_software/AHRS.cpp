#include "AHRS.h"

AHRS::AHRS(HardwareSerialIMXRT *_ahrs) {
    ahrs = _ahrs;
    sync = false;
    debug = NULL;
    delta_T = millis();
    data_rate = 10;
}

void AHRS::SetDebugSerial(usb_serial_class *_debug){
    debug = _debug;
}

void AHRS::Request(String data) {
    data = data + "\n";
    ahrs->print(data);
}

bool AHRS::Syncdata(uint8_t port, uint16_t cycle, uint16_t data){
    if (port > 1) return false;
    if (cycle == 0 || cycle > 60000) return false;
    
    ahrs->print("so=0\n");
    if (!ahrs->available()){}
    String a = ahrs->readString();
    Serial.println(a);

    ahrs->write(data);
    ahrs->print("\n");
    if (!ahrs->available()){}
    a = ahrs->readString();
    Serial.println(a);
    
    // if ((data & 0x0001) == 0x0001) DEBUG_("0x0001");
    return true;
}

void AHRS::SaveData(float *storage, int size, String data) {
    int index = 0;
    data = data.substring(2);
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
    debug->print(data);
    debug->print(" ");
    if (end) debug->println(" ");
}

void AHRS::Print(float *data, int size, bool end) {
    debug->print("(");
    for (int i = 0; i < size; i++)
        Print(*(data+i), false);
    debug->print(")");
    if (end) debug->println(" ");
}

void AHRS::AutoRequestAdd(String data) {
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

void AHRS::AutoRate(int num) {
    if (num < 10) return;
    data_rate = num;
}

void AHRS::AutoRequest() {
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
    //debug->println(a);

    switch (a[0]) {
        case 'p':
            if (a[1] != 'v') break;
            SaveData(&voltage.data, 1, a);
            DEBUG Print(voltage.data);
        break;
        case 't':
            SaveData(&temperatere.data,1, a);
            DEBUG Print(temperatere.data);
        break;
        case 'a':
            SaveData(accel.data, 3,a);
            DEBUG Print(accel.data, 3);
        break;
        case 'g':
            SaveData(gyro.data, 3,a);
            DEBUG Print(gyro.data, 3);
        break;
        case 'm':
            SaveData(mag.data, 3,a);
            DEBUG Print(mag.data, 3);
        break;
        case 'e':
            SaveData(euler.data, 3,a);
            DEBUG Print(euler.data, 3);
        break;
        case 'q':
            SaveData(quataniun.data, 4,a);
            DEBUG Print(quataniun.data, 4);
        break;
    }
    return;
}