#include "Arduino.h"

#define RECVBUFSIZE 10
#define SENDBUFSIZE 6

#define RF_ID 0x02
#define COMMUNICATE 1
#define ERROR_RESORATION 2
#define WAIT_FOR_CONNECTION 0

#define CHECK 0xFC
#define THIS_ID 0x01

#define RFDEBUG if(debug!=NULL)debug->printf

struct RFRECIVEDATA {
  short gunY1; 
  short gunZ2; //기총의 방향각
  uint16_t deadzone1 = 0x0000;
  uint16_t S_Check = 0xf000;// 마지막 비트 제어권 요청, 마지막에서 두번째 발사요청
};

struct RFSENDDATA {
  unsigned int S_Check = 0x00000000; // 마지막 비트 락온, 마지막 두번째 권한, 마지막 세번째 발사
};


class RF{
private:
    HardwareSerialIMXRT *RFSerial;
    usb_serial_class *debug;

    uint8_t RFconnected; //0:ready, 1:communicate, 2:error
    uint8_t sendbuf[SENDBUFSIZE];
    
    long long timeouttimer;
    long long initialdelaytimer, initaldelay;
    long long sendtimer;
    long long waitsigneltimer, waitrate;

    bool readready;
    bool sendready;
public:
    RFRECIVEDATA recivedata;
    RFSENDDATA senddata;
    uint8_t recvbuf[RECVBUFSIZE];
    

    RF(HardwareSerialIMXRT *rf, long long initd, long long wait);

    void SetDebug(usb_serial_class *_debug);
    void ResetDebug();

    void RFhandler(int sendrate);
    void SerialReadByte(uint8_t *data, int size);
    void Communication(int sendrate);
    bool Readupdate(uint8_t buffer[RECVBUFSIZE-2]);
    void SendUpdate(uint8_t buffer[SENDBUFSIZE-2]);
    void Restoration();
    void Wait();
};