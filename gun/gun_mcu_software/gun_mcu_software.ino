#include <FlexCAN_T4.h>
#include "PWMServo.h"
#include "AHRS.h"

#define CANDEBUG  if(can_debug)
#define GUNDEBUG  if(gun_debug)
#define PYUSB     if(pythonusb)

#define DEBUGSerial Serial
#define AHRSSerial Serial2
#define PYTHONSerial  SerialUSB1
bool pythonusb = false;
bool Data_Read();
bool Data_Send();
bool Usb_Read();
void Usb_Send();

#define THIS_ID 0x07
#define MODE_MASK 0x80
#define DEVICE_MASK 0x40
#define COMMAN_MASK 0x3F
FlexCAN_T4<CAN1, RX_SIZE_256, TX_SIZE_16> Can0;
CANListener listener;
static CAN_message_t rxmsg, txmsg;
DataQueue CanSendQueue(50);

bool can_debug=false;
bool Can_Read();
void Can_Send();

PWMServo trigger;
#define TRIGGER_PIN 11
int TRIGGER_OPEN =110;
int TRIGGER_READY =78;
int TRIGGER_ON =60;
int TRIGGER_SINGLE =100;
long long trigger_single_time = 0;
uint8_t trigger_state = 1;
uint8_t last_trigger_state = 1;
void trigger_control();
bool gun_debug = false;

#define LASER_PIN 19

AHRS ahrs(&AHRSSerial);
bool ahrs_debug = false;

void setup() {
  // put your setup code here, to run once:
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN, HIGH);
  trigger.attach(TRIGGER_PIN);
  trigger.write(TRIGGER_OPEN);
  pinMode(LASER_PIN, OUTPUT);
  digitalWrite(LASER_PIN, LOW);

  DEBUGSerial.begin(115200);
  Can0.begin();
  Can0.setBaudRate(500000);
  Can0.attachObj(&listener);
  listener.attachGeneralHandler();
  txmsg.id = 7;
  txmsg.len = 5;
  rxmsg.id = 7;
  memset(txmsg.buf, 0, sizeof(txmsg.buf));
  memset(rxmsg.buf, 0, sizeof(rxmsg.buf));

  ahrs.SetDataQueue(&CanSendQueue);
  AHRSSerial.begin(921600);
  delay(1000);
}

void loop() {
  // put your main code here, to run repeatedly:
  if (DEBUGSerial.available()) {
    String com = DEBUGSerial.readString();
    com.trim();
    DEBUGSerial.println(com);
    if (com == "can") {
      can_debug = !can_debug;
      DEBUGSerial.printf("CAN::Debug % d\n",can_debug);
    }
    else if (com == "ahrs") {
      ahrs_debug = !ahrs_debug;
      if (ahrs_debug) ahrs.SetDebugSerial(&DEBUGSerial);
      else  ahrs.UnsetDebugSerial();
    }
    else if (com == "gun") {
      gun_debug = !gun_debug;
      DEBUGSerial.printf("GUN::Debug % d\n",can_debug);
    }
    else if (com == "usb") {
      pythonusb = !pythonusb;
      DEBUGSerial.printf("CAN::Change Communication Line %d\n",pythonusb);
    }
  }

  PYUSB Usb_Read();
  else  Can_Read();
  
  trigger_control();
  ahrs.AutoRequest();
  ahrs.AutoRead();

  PYUSB Usb_Send();
  else  Can_Send();
}

void trigger_control() {
  if (millis() - trigger_single_time < TRIGGER_SINGLE) return;
  if (trigger_state == last_trigger_state) return;

  last_trigger_state = trigger_state;

  if (last_trigger_state == 4) {
    trigger_single_time = millis();
    trigger_state = 2;
  }
  switch (last_trigger_state) {
    case 1:
      trigger.write(TRIGGER_OPEN);
      GUNDEBUG DEBUGSerial.printf("GUN::TriggerControl : TriggerOpen");
    break;
    case 2:
      trigger.write(TRIGGER_READY);
      GUNDEBUG DEBUGSerial.printf("GUN::TriggerControl : TriggerReady");
    break;
    case 3:
    case 4:
      trigger.write(TRIGGER_ON);
      GUNDEBUG DEBUGSerial.printf("GUN::TriggerControl : TriggerOn");
    break;
  }
}

bool Data_Read() {
  CANDEBUG {
  DEBUGSerial.println("---------------------------rx");
  DEBUGSerial.printf("%02X", rxmsg.id);
  for (int i = 0; i < 7; i++)
    DEBUGSerial.printf(" %02X", rxmsg.buf[i]);
  DEBUGSerial.printf(" | time:%d \n\r", millis());
  }
  bool returns;
  DataItem returndata;

  /* Write mode */
  if (((rxmsg.buf[0] & MODE_MASK) >> 7) == 0x01 ) {
    returns = false;
    /* device */
    if (((rxmsg.buf[0] & DEVICE_MASK) >> 6) == 0x00 ) {
      switch ((rxmsg.buf[0] & COMMAN_MASK)) {
        case 0x01:
          if((rxmsg.buf[1] == 0x00) || (rxmsg.buf[1] == 0x01) ) {
            digitalWrite(LASER_PIN, (rxmsg.buf[1] & 0x01));
            CANDEBUG DEBUGSerial.printf("Laser & Gun Power %d\n", digitalRead(LASER_PIN));
          }
        break;
        case 0x10:
          trigger_state = 1;
          CANDEBUG DEBUGSerial.printf("Trigger Open\n");
        break;
        case 0x11:
          trigger_state = 4;
          CANDEBUG DEBUGSerial.printf("Trigger Single\n");
        break;
        case 0x12:
          trigger_state = 2;
          CANDEBUG DEBUGSerial.printf("Trigger Ready\n");
        break;
        case 0x13:
          trigger_state = 3;
          CANDEBUG DEBUGSerial.printf("Trigger On\n");
        break;
        case 0x14:
          if (rxmsg.buf[1] > 180) break;
          TRIGGER_OPEN = (int)rxmsg.buf[1];
          CANDEBUG DEBUGSerial.printf("Trigger Open Angle : %d\n", TRIGGER_OPEN);
        break;
        case 0x15:
          memcpy(&TRIGGER_SINGLE, &rxmsg.buf[1], 4);
          CANDEBUG DEBUGSerial.printf("Trigger Single Time : %d\n", TRIGGER_SINGLE);
        break;
        case 0x16:
          if (rxmsg.buf[1] > 180) break;
          TRIGGER_ON = (int)rxmsg.buf[1];
          CANDEBUG DEBUGSerial.printf("Trigger On Angle : %d\n", TRIGGER_ON);
        break;
        case 0x17:
          if (rxmsg.buf[1] > 180) break;
          TRIGGER_READY = (int)rxmsg.buf[1];
          CANDEBUG DEBUGSerial.printf("Trigger Ready Angle : %d\n", TRIGGER_READY);
        break;
      }
    }
    /* AHRS */
    else {
      String data;
      int num = 0;
      switch ((rxmsg.buf[0] & COMMAN_MASK)) {
        case 0x01:
          ahrs.RequestAdd("rd");
          CANDEBUG DEBUGSerial.printf("AHRS::AHRS Restart\n");
        break;
        case 0x02:
          if (rxmsg.buf[1] == 0xE6) data = "pv";
          else data = rxmsg.buf[1];
          ahrs.AutoRequestAdd(data);
        break;
        case 0x03:
          if (rxmsg.buf[1] == 0xE6) data = "pv";
          else data = rxmsg.buf[1];
          ahrs.AutoRequestDelete(data);
        break;
        case 0x04:
          ahrs.AutoRequestOn();
        break;
        case 0x05:
          ahrs.AutoRequestOff();
        break;
        case 0x06:
          memcpy(&num, &rxmsg.buf[1], 4);
          ahrs.AutoRate(num);
        break;
      }
    }
  }
  /* Read */
  else {
    /* device */
    if (((rxmsg.buf[0] & DEVICE_MASK) >> 6) == 0x00 ) {
      returns = true;
      returndata.data[0] = rxmsg.buf[0];
      switch ((rxmsg.buf[0] & COMMAN_MASK)) {
        case 0x00:
          returndata.data[1] = THIS_ID;
          returndata.len = 2;
        break;
        case 0x01:
          returndata.data[1] = (uint8_t)digitalRead(LASER_PIN);
          returndata.len = 2;
        break;
        case 0x10:
          if (last_trigger_state == 1)      returndata.data[1] = 0;
          else if (last_trigger_state == 2) returndata.data[1] = 2;
          else if (last_trigger_state == 3) returndata.data[1] = 3;
          else if (last_trigger_state == 4) returndata.data[1] = 3;
          returndata.len = 2;
        break;
        case 0x14:
          returndata.data[1] = (uint8_t)TRIGGER_OPEN;
          returndata.len = 2;
        break;
        case 0x15:
          memcpy(&returndata.data[1], &TRIGGER_SINGLE, 4);
          returndata.len = 5;
        break;
        case 0x16:
          returndata.data[1] = (uint8_t)TRIGGER_ON;
          returndata.len = 2;
        break;
        case 0x17:
          returndata.data[1] = (uint8_t)TRIGGER_READY;
          returndata.len = 2;
        break;
      }
      CanSendQueue.push(returndata);
    }
    else {
      String data = (char)rxmsg.buf[1];
      ahrs.RequestAdd(data);
    }
  }

  CANDEBUG DEBUGSerial.println("---------------------------rx_");
  return returns;
}

bool Data_Send() {
  DataItem item;
  if (!CanSendQueue.pop(item)) return false;

  CANDEBUG DEBUGSerial.println("---------------------------tx");
  txmsg.len = item.len;
  memcpy(txmsg.buf, &item.data, txmsg.len);
  CANDEBUG DEBUGSerial.printf("CAN DATA FORMAT: ");
  CANDEBUG DEBUGSerial.printf("%02X %02X %02X %02X %02X %02X\n", txmsg.id, txmsg.buf[0],  txmsg.buf[1], txmsg.buf[2], txmsg.buf[3], txmsg.buf[4]);
  CANDEBUG DEBUGSerial.println("---------------------------tx_");
  return true;
}

bool Can_Read() {
  if (!Can0.read(rxmsg))    return true;
  if (rxmsg.id != THIS_ID)  return true;

  bool returns = Data_Read();
  return returns;
}

void Can_Send(){
  if(Data_Send())
    Can0.write(txmsg);
  return;
}

bool Usb_Read() {
  if (!PYTHONSerial.available()) return true;
  int ID = PYTHONSerial.read();
  int len = PYTHONSerial.read();
  if (ID < 0) return false;
  if ((uint8_t)ID != THIS_ID) return true;
  if (len > 8) return false;

  rxmsg.id = (uint8_t)THIS_ID;
  rxmsg.len = (uint8_t)len;
  PYTHONSerial.readBytes(rxmsg.buf, (int)rxmsg.len);

  bool returns = Data_Read();

  return returns;
}

void Usb_Send() {
  if(!Data_Send()) return;
  uint8_t buffer[10];
  buffer[0] = THIS_ID;
  buffer[1] = txmsg.len;
  memcpy(&buffer[2], txmsg.buf, txmsg.len);

  PYTHONSerial.write(buffer, txmsg.len);
  return;
}