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

#include <FlexCAN_T4.h>

FlexCAN_T4<CAN1, RX_SIZE_256, TX_SIZE_16> Can0;
CANListener listener;

static CAN_message_t rxmsg, txmsg;

const int PACKET_SIZE = 10;
uint8_t PACKET[PACKET_SIZE];
int PACKET_INDEX = 0;
bool canflag = true;

void setup() 
{
  Serial.begin(115200);
  Serial.setTimeout(1);
  delay(1000);

  Can0.begin();
  Can0.setBaudRate(1000000);
  Can0.attachObj(&listener);
  listener.attachGeneralHandler();

  txmsg.id = 1;
  txmsg.len = 5;
  txmsg.buf[0] = 00;
  txmsg.buf[1] = 00;
  txmsg.buf[2] = 00;
  txmsg.buf[3] = 00;
  txmsg.buf[4] = 00;
  txmsg.buf[5] = 00;
  txmsg.buf[6] = 00;
  txmsg.buf[7] = 00;
}

bool sendCANMessage(CAN_message_t &message, unsigned long timeoutMillis = 100) 
{
  unsigned long startTime = millis();
  while (!Can0.write(message)) 
  {
    if (millis() - startTime > timeoutMillis) 
    {
      return false;
    }
  }
  return true;
}

void loop() 
{
  memset(PACKET, 0, sizeof(PACKET));
  PACKET_INDEX = 0;
  while (Serial.available() > 0 && PACKET_INDEX < PACKET_SIZE) 
  {
    PACKET[PACKET_INDEX++] = Serial.read();
  }
  
  if(PACKET_INDEX > 0)
  {
    // Speed Mode
    if (PACKET[0] == 3 || PACKET[0] == 4 || PACKET[0] == 5) 
    {
      txmsg.id = PACKET[0];
      txmsg.len = 5;
      for (int i = 0; i < txmsg.len; ++i) 
      {
        txmsg.buf[i] = PACKET[i+1];
      }

      if (!sendCANMessage(txmsg)) 
      {
        txmsg.id = 5;
        txmsg.len = 6;
        txmsg.buf[0] = 0xF6;
        txmsg.buf[1] = 0x00;
        txmsg.buf[2] = 0x00;
        txmsg.buf[3] = 0x00;
        txmsg.buf[4] = 0x64; // 100
        txmsg.buf[5] = (txmsg.id + txmsg.buf[0] + txmsg.buf[4]) & 0xFF;
        
        sendCANMessage(txmsg);
      }
    }
  }

  if (Can0.read(rxmsg)) 
  {
    byte message[9];
    message[0] = (byte)(rxmsg.id & 0xFF); // 하위 8비트만 사용
  
    for (int i = 0; i < rxmsg.len; i++)
    {
      message[i + 1] = rxmsg.buf[i];
    }
    Serial.write(message, rxmsg.len + 1);
  }
}