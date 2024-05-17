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

#define USBSerial Serial
#define RFSerial Serial2
#define FAILED 0xFFFFFFFF

FlexCAN_T4<CAN1, RX_SIZE_256, TX_SIZE_16> Can0;
CANListener listener;
static CAN_message_t rxmsg, txmsg;


bool sendCANMessage(CAN_message_t &message, unsigned long timeoutMillis = 100);
bool Usb_Read_Can_Send();
bool Can_Read_Usb_Send();

void RF_Send_handler(int len, uint8_t *data);
void RF_Read_handler();

void Teensy_handler(int len, uint8_t *data);

void setup() 
{
  USBSerial.begin(115200);
  USBSerial.setTimeout(1);
  delay(1000);

  RFSerial.begin(115200);
  RFSerial.setTimeout(1);
  delay(1000);

  Can0.begin();
  Can0.setBaudRate(1000000);
  Can0.attachObj(&listener);
  listener.attachGeneralHandler();

  txmsg.id = 1;
  txmsg.len = 5;
  memset(txmsg.buf,0,8);
}

void loop() 
{
  Usb_Read_Can_Send();
  Can_Read_Usb_Send();
}

bool sendCANMessage(CAN_message_t &message, unsigned long timeoutMillis = 100) 
{
  unsigned long startTime = millis();
  while (!Can0.write(message)) 
  {
    if (millis() - startTime > timeoutMillis) return false;
  }
  return true;
}

bool Usb_Read_Can_Send() {
  if (!USBSerial.available()) return true;
  int ID = USBSerial.read();
  int len = USBSerial.read();
  if (ID < 0) return false;
  if (len > 8) return false;

  memset(txmsg.buf,0,8);

  txmsg.id = (uint8_t)ID;
  txmsg.len = (uint8_t)len;
  USBSerial.readBytes(txmsg.buf, (int)txmsg.len);

  switch (ID) {
    case 1:
      Teensy_handler(len, txmsg.buf);
    break;
    case 2:
      RF_Send_handler(len, txmsg.buf);
    break;
    default:
    if (!sendCANMessage(txmsg)) return false;
  }
    
  return true;
}

bool Can_Read_Usb_Send() {
  if (!Can0.read(rxmsg)) return true;
  uint8_t buffer[10] = {0,};
  buffer[0] = (rxmsg.id & 0xFF);
  buffer[1] = rxmsg.len;
  memcpy(&buffer[2], rxmsg.buf, rxmsg.len);

  USBSerial.write(buffer, (int)rxmsg.len + 2);
  memset(rxmsg.buf,0,8);
}

void RF_Send_handler(int len, uint8_t *data) {
  RFSerial.write(data, len);
}

void RF_Read_handler() {
  RFSerial.read();
}

void Teensy_handler(int len, uint8_t *data) {

}