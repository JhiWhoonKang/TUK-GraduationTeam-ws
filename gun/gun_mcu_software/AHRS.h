#pragma once

#ifndef __AHRS
    #define __AHRS
#endif

#include "Arduino.h"
#include "imxrt.h"
#include "String.h"

#define AHRSSerial Serial2
#define DEBUG if(debug!=NULL)

struct float1 {
    bool use = false;
    float data;
};

struct float3 {
    bool use;
    float data[3];
};

struct float4 {
    bool use;
    float data[4];
};

struct float6 {
    bool use;
    float data[6];
};

class AHRS {
private:
    HardwareSerialIMXRT *ahrs;
    usb_serial_class *debug;
    bool sync;

    long long delta_T;
    int data_rate;

    float1 voltage;      // V
    float1 temperatere;  // C
    float3 accel;          // g
    float3 gyro;           // 
    float3 mag;            // uT
    float3 accel_g;        // m/s^2
    float3 euler;          // deg
    float4 quataniun;      
    float3 velocity;       // m/s
    float6 vibration;  // Hz

    void SaveData(float *storage, int size,  String data);
public:
    

    AHRS(HardwareSerialIMXRT *_ahrs);

    void Print(float data, bool end = true);
    void Print(float *data, int size, bool end = true);

    void SetDebugSerial(usb_serial_class *_debug);
    void Request(String data);

    
    bool Syncdata(uint8_t port, uint16_t cycle, uint16_t data);
    void AutoRequestAdd(String data);
    void AutoRequestDelete(String data);
    void AutoRate(int num);
    void AutoRequest();
    void AutoRead();
};