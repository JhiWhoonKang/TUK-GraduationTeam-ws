// ******************************************************************************
// RCWS Optical device MCU code
// You can use with CAN or USB Serial
// CRC = (ID + byte1 + … + byte(n) ) & 0xFF
// CAN ID 3 = Weapon
// CAN ID 4 = Optical TILT
// CAN ID 5 = Body PAN

// rx 2byte    tx 5byte
// ******************************************************************************
#include <FlexCAN_T4.h>
#include <PWMServo.h>
#include "CanQueue.h"
#include "Watchdog_t4.h"



#define CANDEBUG  if(can_debug)
#define TOFDEBUG  if(tof_debug)
#define CAMDEBUG  if(camera_debug)
#define PYUSB     if(pythonusb)
#define DEBUGSerial   Serial
#define TOFSerial     Serial2
#define PYTHONSerial  SerialUSB1

WDT_T4<WDT1> wdt;
void myCallback() {
  DEBUGSerial.println("FEED THE DOG SOON, OR RESET!");
}

bool pythonusb = false;
bool Data_Read();
bool Data_Send();
bool Usb_Read();
void Usb_Send();

FlexCAN_T4<CAN1, RX_SIZE_256, TX_SIZE_16> Can0;
CANListener listener;

#define THIS_ID 0x06
#define MODE_MASK 0x80
#define DEVICE_MASK 0x40
#define COMMAN_MASK 0x3F
#define FAILED 0xFFFFFFFF
#define ZOOM_TABLE 0x01
#define FOCUS_TABLE 0x02
#define ZOOM_CURRENT 0x03
#define FOCUS_CURRENT 0x04
bool can_debug = false;
static CAN_message_t rxmsg, txmsg;
bool Can_Read();
void Can_Send();
DataQueue Queue(50);


#define TOF_HEADER  0x59
bool tof_debug = false;
bool TOF_mode = false; // false : polling , true : auto
long long TOF_delay = 100;
long long TOF_timer = 0;
int dist = 0;
int strength = 0;
uint8_t buffer[6];
void Dist_Send();
void TOF_Read();
//void TOF_Send();

PWMServo Zoom;
PWMServo Focus;
#define ZOOM_MAX 180
#define ZOOM_MIN 4
#define FOCUS_MAX 180
#define FOCUS_MIN 1
bool camera_debug = false;
int focus_offset = 0;
int zoom_offset = 0;
int zoom_table[10] = {100,110,120,130,140,150,160,170,180,0};
int focus_table[10] = {10,17,30,40,50,60,75,90,100,0};
int zoom_pos = 100;
int focus_pos = 10;
bool focus_mode = false; // false : fixed , true : auto
void Auto_Focus(int Z, int dis);

void setup() 
{
  WDT_timings_t config;
  config.trigger = 2;
  config.timeout = 3;
  config.callback = myCallback;
  wdt.begin(config);

  DEBUGSerial.begin(115200);
  TOFSerial.begin(115200);
  PYTHONSerial.begin(250000);
  delay(100);

  Can0.begin();
  Can0.setBaudRate(500000);
  Can0.attachObj(&listener);
  listener.attachGeneralHandler();

  txmsg.id = THIS_ID;
  txmsg.len = 5;
  memset(txmsg.buf, 0, sizeof(txmsg.buf));

  memset(buffer, 0, sizeof(buffer));

  focus_pos = focus_table[0];
  zoom_pos = zoom_table[0];
  Focus.write(focus_pos);
  Zoom.write(zoom_pos);
  Focus.attach(1);
  Zoom.attach(0);
  
  
  DEBUGSerial.println("Optical Run");
  delay(100);
}

void loop() 
{
  wdt.feed();
  TOF_Read();

  if (millis() - TOF_timer >= TOF_delay && TOF_mode) {
    Dist_Send();
    TOF_timer = millis();
  }

  if (DEBUGSerial.available()) 
  {
    String com = DEBUGSerial.readString();
    com.trim();
    if (com == "can") can_debug = !can_debug;
    else if (com == "tof") tof_debug = !tof_debug;
    else if (com == "cam") camera_debug = !camera_debug;
    else if (com == "usb") pythonusb = !pythonusb;
    DEBUGSerial.println(com);
    PYUSB DEBUGSerial.println("start");
  }

  CAMDEBUG DEBUGSerial.printf("LIDAR::Dist : %d, F : %d, Z : %d\n", dist, Focus.read(), Zoom.read());

  PYUSB {
    Usb_Read();
    Usb_Send();
  }
  else {
    Can_Read();
    Can_Send();
  }
}

bool Data_Read() {
  CANDEBUG {
  DEBUGSerial.println("---------------------------rx");
  DEBUGSerial.printf("%02X", rxmsg.id);
  for (int i = 0; i < 8; i++)
    DEBUGSerial.printf(" %02X", rxmsg.buf[i]);
  DEBUGSerial.printf(" | time:%d \n\r", millis());
  }

  DataItem item;
  item.data[0] = rxmsg.buf[0];
  item.len = 5;

  uint8_t Mode = ((rxmsg.buf[0] & MODE_MASK) >> 7);
  uint8_t Device = ((rxmsg.buf[0] & DEVICE_MASK) >> 6);
  uint8_t com = (rxmsg.buf[0] & COMMAN_MASK);

  if (rxmsg.buf[0] == 0xFF) {
    DEBUGSerial.println("System::System Restart...");
    while(1) {}
  }
  /* Read Mode */
  if (Mode == 0x00 ) {
    /* TOF */
    if (Device == 0x00 ) {
      switch (com) {
      case 0x00 :
        if (rxmsg.buf[1] != 0) return false;
        item.data[1] = THIS_ID;
        item.len = 2;
        CANDEBUG DEBUGSerial.printf("THIS IS %d, %02X\n",THIS_ID ,THIS_ID); // ACK
      break;
      case 0x01:             // Read Tof distance
        memcpy(&item.data[1], &dist, 4);
        CANDEBUG DEBUGSerial.printf("Read Tof distance %d, %02X\n",dist ,dist);
      break;
      case 0x02:        // Read Tof Strength
        memcpy(&item.data[1], &strength, 4);
        CANDEBUG DEBUGSerial.printf("Read Tof Strength %d, %02X\n",strength ,strength);
      break;
      case 0x03:        // Read Tof Delay
      int de = (int)TOF_delay;
        memcpy(&item.data[1], &de, 4);
        CANDEBUG DEBUGSerial.printf("Read Tof Tof %d, %02X\n",TOF_delay ,TOF_delay);
      }
    }
    /* Camera */
    else {
      int num;
      switch (com) {
      case 0x01:     // Read zoom table
        if (rxmsg.buf[1] > 9) return false;
        num = zoom_table[rxmsg.buf[1]];
        item.data[1] = rxmsg.buf[1];
        memcpy(&item.data[2], &num, 4);
        CANDEBUG DEBUGSerial.printf("Read zoom table [%d] %d, %02X\n",rxmsg.buf[1], num ,num);
      break;
      case 0x02:      // Read focus table
        if (rxmsg.buf[1] > 9) return false;
        num = focus_table[rxmsg.buf[1]];
        item.data[1] = rxmsg.buf[1];
        memcpy(&item.data[2], &num, 4);
        CANDEBUG DEBUGSerial.printf("Read focus table [%d] %d, %02X\n",rxmsg.buf[1], num ,num);
      break;
      case 0x03:   // Read zoom
        num = Zoom.read();
        memcpy(&item.data[1], &num, 4);
        CANDEBUG DEBUGSerial.printf("Read zoom %d, %02X\n",Zoom.read() ,Zoom.read());
      break;
      case 0x04:    // Read focus
        num = Focus.read();
        memcpy(&item.data[1], &num, 4);
        CANDEBUG DEBUGSerial.printf("Read focus %d, %02X\n",Focus.read() ,Focus.read());
      break;
      }
    }
    Queue.push(item);
  }
  /* Write Mode */
  else {
    /* TOF */
    if (Device == 0x00 ) {
      switch (com) {
      case 0x03:             // Set tof_delay
        TOF_delay = (int)rxmsg.buf[1] * 10 + 10;
        CANDEBUG DEBUGSerial.printf("Set tof_delay %d, %02X\n",TOF_delay ,TOF_delay);
      break;
      case 0x02:            // auto Tof off
        TOF_mode = false;
        CANDEBUG DEBUGSerial.printf("auto Tof off %d, %02X\n",TOF_mode ,TOF_mode);
      break;
      case 0x01:            // auto Tof on
        TOF_mode = true;
        CANDEBUG DEBUGSerial.printf("auto Tof off %d, %02X\n",TOF_mode ,TOF_mode);
      break;
      }
    }
    /* Camera */
    else { 
      int8_t offset_ = 0;
      switch (com) {
      case 0x01:       // Set zoom table
        if (rxmsg.buf[1] > 9) return false;
        if (rxmsg.buf[2] < ZOOM_MIN || rxmsg.buf[2] > ZOOM_MAX) return false;
        zoom_table[rxmsg.buf[1]] = rxmsg.buf[2];
        CANDEBUG DEBUGSerial.printf("Set zoom table [%d] %d, %02X\n",rxmsg.buf[1], rxmsg.buf[2] ,rxmsg.buf[2]);
      break;
      case 0x02:  // Set focus table
        if (rxmsg.buf[1] > 9) return false;
        if (rxmsg.buf[2] < FOCUS_MIN || rxmsg.buf[2] > FOCUS_MAX) return false;
        zoom_table[rxmsg.buf[1]] = rxmsg.buf[2];
        CANDEBUG DEBUGSerial.printf("Set focus table [%d] %d, %02X\n",rxmsg.buf[1], rxmsg.buf[2] ,rxmsg.buf[2]);
      break;
      case 0x03: // Set zoom
        if (rxmsg.buf[2] < ZOOM_MIN || rxmsg.buf[2] > ZOOM_MAX) return false;
        focus_mode = false;
        zoom_pos = rxmsg.buf[2];
        CANDEBUG DEBUGSerial.printf("Set zoom %d, %02X\n",zoom_pos ,zoom_pos);
      break;
      case 0x04:  // Set focus
        if (rxmsg.buf[2] < FOCUS_MIN || rxmsg.buf[2] > FOCUS_MAX) return false;
        focus_mode = false;
        focus_pos = rxmsg.buf[2];
        CANDEBUG DEBUGSerial.printf("Set focus %d, %02X\n",focus_pos ,focus_pos);
      break;
      case 0x05:
        memcpy(&offset_, &rxmsg.buf[2], 1);
        zoom_offset = offset_;
        CANDEBUG DEBUGSerial.printf("Set zoom offset %d, %02X\n",zoom_offset ,zoom_offset);
      break;
      case 0x06:
        memcpy(&offset_, &rxmsg.buf[2], 1);
        focus_offset = offset_;
        CANDEBUG DEBUGSerial.printf("Set focus offset %d, %02X\n",focus_offset ,focus_offset);
      break;
      case 0x07:            // focus fixed
        if (rxmsg.buf[1] > 9) return false;
        zoom_pos = zoom_table[rxmsg.buf[1]];
        focus_pos = focus_table[rxmsg.buf[1]];
        focus_mode = false;
        CANDEBUG DEBUGSerial.printf("focus fixed %d, %02X\n",focus_mode ,focus_mode);
      break;
      case 0x08:            // focus auto
        focus_mode = true;
        CANDEBUG DEBUGSerial.printf("focus auto %d, %02X\n",focus_mode ,focus_mode);
      break;
      }
    }
  }
   CANDEBUG DEBUGSerial.println("---------------------------rx_");
  return true;
}

bool Data_Send() {
  DataItem item;
  if (!Queue.pop(item)) return false;
  
  CANDEBUG DEBUGSerial.println("---------------------------tx");
  txmsg.len = item.len;
  memset(txmsg.buf,0,8);
  memcpy(txmsg.buf, &item.data, txmsg.len);
  CANDEBUG DEBUGSerial.printf("CAN DATA FORMAT: ");
  CANDEBUG DEBUGSerial.printf("%02X :  %02X %02X %02X %02X %02X %02X %02X %02X\n", 
    txmsg.id, txmsg.buf[0],  txmsg.buf[1], txmsg.buf[2], txmsg.buf[3], txmsg.buf[4], txmsg.buf[5], txmsg.buf[6], txmsg.buf[7]);
  CANDEBUG DEBUGSerial.println("---------------------------tx_");
  return true;
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
  uint8_t buffer[10] = {0,};
  buffer[0] = THIS_ID;
  buffer[1] = txmsg.len;
  memcpy(&buffer[2], txmsg.buf, txmsg.len);

  CANDEBUG DEBUGSerial.printf("%02X %02X :  %02X %02X %02X %02X %02X %02X %02X %02X\n", 
    buffer[0], buffer[1], buffer[2], buffer[3], buffer[4],buffer[5],buffer[6],buffer[7],buffer[8],buffer[9]);

  PYTHONSerial.write(buffer,txmsg.len+2);
  return;
}

bool Can_Read() {
  if (!Can0.read(rxmsg))    return true;
  if (rxmsg.id != THIS_ID)  return true;

  bool returns = Data_Read();
  return returns;
}

void Can_Send() {
  if (Data_Send())
    Can0.write(txmsg);
  return;
}

/*
void TOF_Send(uint8_t lan, uint8_t data[9]) {
  for (int i = 0; i < lan; i++)
    TOFSerial.write(data[i]);

  uint8_t buffer[11] = {0,};
  int sum = 0;
  delay(500);
  for (int i = 0; i < lan; i++) {
    buffer[i] = TOFSerial.read();
    sum += buffer[i];
  }

  CANDEBUG DEBUGSerial.println("---------------------------tx");
  txmsg.len = 11;
  memcpy(txmsg.buf, buffer, lan);
  txmsg.buf[10] = (txmsg.id + sum) & 0xFF;
  CANDEBUG DEBUGSerial.printf("CAN DATA FORMAT: ");
  CANDEBUG DEBUGSerial.printf("%02X %02X %02X %02X %02X %02X\n", txmsg.id, txmsg.buf[0],  txmsg.buf[1], txmsg.buf[2], txmsg.buf[3], txmsg.buf[4]);
  CANDEBUG DEBUGSerial.println("---------------------------tx_");

  Can0.write(txmsg);
  return;
}
*/

void TOF_Read() {
  if (TOFSerial.available() < 9) return;

  if(TOFSerial.read() != TOF_HEADER) return;
  if(TOFSerial.read() != TOF_HEADER) return;

  int check = TOF_HEADER + TOF_HEADER;

  for (int i = 0; i < 6; i++) {
    buffer[i] = TOFSerial.read();
    check += (int)buffer[i];
  }
  if(TOFSerial.read() == (check & 0xff)) {
    dist = ((int)buffer[0] + (((int)buffer[1]) << 8));
    strength = ((int)buffer[2] + (((int)buffer[3]) << 8));
    Zoom.write(zoom_pos);
    Focus.write(focus_pos);
    TOFDEBUG DEBUGSerial.printf("Dist : %d, Strength : %d\n", dist, strength);
  }
  return;
}

void Dist_Send() {
  DataItem item;
  item.len = 5;
  item.data[0] = 0x01;
  memcpy(&item.data[1], &dist,5);
  Queue.push(item);
  return;
}

/*
void Auto_Focus(int Z, int dis) {
  switch (Z) {
  case 108:
    focus_pos = (21*pow(cbrt(10-3),2))/pow(cbrt(dis-3),2) + focus_offset;
    if (focus_pos > FOCUS_MAX) focus_pos = FOCUS_MAX;
    else if(focus_pos < FOCUS_MIN) focus_pos = FOCUS_MIN;
    Focus.write(focus_pos);
  break;
  case 0:
    focus_pos = 180/(sqrt(sqrt(sqrt(cbrt(pow(dis-150,11)))))) + 136 + focus_offset;
    if (focus_pos > FOCUS_MAX) focus_pos = FOCUS_MAX;
    else if(focus_pos < FOCUS_MIN) focus_pos = FOCUS_MIN;
    Focus.write(focus_pos);
  break;
  }
  
  return;
}
*/