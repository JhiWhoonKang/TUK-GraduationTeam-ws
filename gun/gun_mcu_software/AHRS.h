#pragma once

#ifndef __AHRS
    #define __AHRS
#endif

#include "Arduino.h"
#include "imxrt.h"
#include "String.h"
#include "StringQueue.h"
#include "CanQueue.h"

#define AHRSSerial Serial2
#define AHRSDEBUG if(debug!=NULL)

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
    DataQueue *CanSendQueue;
    bool reset;

    bool sync;

    long long delta_T;
    int data_rate;
    bool auto_request;

    CommandQueue RequestQueue;

    float1 voltage;         // V    // 0xD6
    float1 temperatere;     // C    // 't'
    float3 accel;          // g     // 'a'
    float3 gyro;           //       // 'g'
    float3 mag;            // uT    // 'm'
    float3 euler;          // deg   // 'e'
    float4 quataniun;               // 'q'
    float3 velocity;       // m/s   // 'v'
    float6 vibration;  // Hz        // 'p'

    void SaveData(float *storage, int size,  String data);
public:
    AHRS(HardwareSerialIMXRT *_ahrs);

    void Print(float data, bool end = true);
    void Print(float *data, int size, bool end = true);

    void SetDebugSerial(usb_serial_class *_debug);
    void UnsetDebugSerial();
    void SetDataQueue(DataQueue *_CanSendQueue);
    void Request(String data);

    void RequestAdd(String data = "");
    
    bool Syncdata(uint8_t port, uint16_t cycle, uint16_t data);
    
    String Check(String data);
    void AutoRequestAdd(String data);
    void AutoRequestDelete(String data);
    void AutoRequestOn();
    void AutoRequestOff();
    void AutoRate(int num);
    void AutoRequest();
    void AutoRead();
};