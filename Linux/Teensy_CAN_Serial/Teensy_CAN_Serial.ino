// ******************************************************************************
// Speed range 0 ~ 3000
// Acc 0 ~ 255
// relAxis range -8388607 ~ 8388607
// Dir 0(CCW), 1(CW)
// CRC = (ID + byte1 + … + byte(n) ) & 0xFF
// CAN ID 3 = Body PAN
// CAN ID 4 = Optical TILT
// CAN ID 5 = Weapon
// ******************************************************************************
#include "RFSerial.h"
#include <FlexCAN_T4.h>

#define PCSerial SerialUSB1
#define DEBUGSerial Serial
#define RFSerial Serial2

#define DEBUG if(debug)
FlexCAN_T4<CAN1, RX_SIZE_256, TX_SIZE_16> Can0;
CANListener listener;

static CAN_message_t rxmsg, txmsg;

const int PACKET_SIZE = 10;
uint8_t PACKET[PACKET_SIZE];
int PACKET_INDEX = 0;

bool debug = false;
bool rfdebug = false;
bool teensydebug = false;

RF rf(&RFSerial, 1000, 500);

void DebugSetting();

void setup()
{
  RFSerial.begin(115200); //laser와 RCWS간 서로 통신
  RFSerial.setTimeout(2);

  PCSerial.begin(500000);
  PCSerial.setTimeout(1);
  DEBUGSerial.begin(115200);
  DEBUGSerial.setTimeout(1);
  delay(100);

  Can0.begin();
  Can0.setBaudRate(500000);
  Can0.attachObj(&listener);
  listener.attachGeneralHandler();

  txmsg.id = 1;
  txmsg.len = 5;
  memset(txmsg.buf, 0, 8);
}

void loop()
{
  DebugSetting();
  rf.RFhandler(500);

  memset(PACKET, 0, sizeof(PACKET));
  PACKET_INDEX = 0;
  while (PCSerial.available() > 0 && PACKET_INDEX < PACKET_SIZE)
  {
    PACKET[PACKET_INDEX++] = PCSerial.read();
  }
 
  if(PACKET_INDEX > 0)
  {    
    if(PACKET[0] == 1)
    {
      
    }
   
    if(PACKET[0] == 2)
    {
      rf.SendUpdate(&PACKET[2]);
    }

    else
    {
      txmsg.id = PACKET[0];
      txmsg.len = PACKET[1];
      memcpy(txmsg.buf, &PACKET[2], txmsg.len);
      DEBUG DEBUGSerial.printf("CAN::Read : %x [%x %x %x %x %x %x %x %x]\n",
        txmsg.id, txmsg.buf[0], txmsg.buf[1], txmsg.buf[2], txmsg.buf[3], 
        txmsg.buf[4], txmsg.buf[5], txmsg.buf[6], txmsg.buf[7]);

      Can0.write(txmsg);
    }
   }
  if (Can0.read(rxmsg))
  {
    byte message[10];
    message[0] = (byte)(rxmsg.id & 0xFF); // 하위 8비트만 사용
    message[1] = (byte)(rxmsg.len & 0xFF);
    for (int i = 0; i < rxmsg.len; i++)
    {
      message[i + 2] = rxmsg.buf[i];
    }
    DEBUG DEBUGSerial.printf("CAN::Send : %x [%x %x %x %x %x %x %x %x]\n",
      rxmsg.id, rxmsg.buf[0], rxmsg.buf[1], rxmsg.buf[2], rxmsg.buf[3],
      rxmsg.buf[4], rxmsg.buf[5], rxmsg.buf[6], rxmsg.buf[7]);

    PCSerial.write(message, rxmsg.len + 2);
  }

  uint8_t PACKET[RECVBUFSIZE];
  if (rf.Readupdate(&PACKET[2])) {
    
    PACKET[0] = 0x02;
    PACKET[1] = 0x08;
    PCSerial.write(PACKET, 10);
    DEBUG DEBUGSerial.printf("CAN::Send : %x [%x %x %x %x %x %x %x %x]\n",
      PACKET[0], PACKET[2], PACKET[3], PACKET[4], PACKET[5],
      PACKET[6], PACKET[7], PACKET[8], PACKET[9]);
  }
}

void DebugSetting() {
  if (! DEBUGSerial.available()) return;
  String com = DEBUGSerial.readString();
  com.trim();
  DEBUGSerial.println(com);
  if (com == "can") {
    debug = !debug;
    DEBUGSerial.printf("CAN::Debug %d\n",debug);
  }
  else if (com == "rf") {
    rfdebug = !rfdebug;
    if (rfdebug) rf.SetDebug(&DEBUGSerial);
    else  rf.ResetDebug();
  }
  else if (com == "teensy") {
    teensydebug = !teensydebug;
  }
}