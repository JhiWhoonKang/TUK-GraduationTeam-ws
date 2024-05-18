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


#define CANDEBUG  if(can_debug)
#define TOFDEBUG  if(tof_debug)
#define CAMDEBUG  if(camera_debug)
#define PYUSB     if(pythonusb)
#define DEBUGSerial   Serial
#define TOFSerial     Serial2
#define PYTHONSerial  SerialUSB1
bool pythonusb = false;
bool Data_Read(int &return_val);
void Data_Send(int value);
bool Usb_Read();
void Uab_Send(int value);

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
void Can_Send(int value);


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
  DEBUGSerial.begin(115200);
  TOFSerial.begin(115200);
  PYTHONSerial.begin(250000);
  delay(1000);

  Can0.begin();
  Can0.setBaudRate(500000);
  Can0.attachObj(&listener);
  listener.attachGeneralHandler();

  txmsg.id = THIS_ID;
  txmsg.len = 5;
  memset(txmsg.buf, 0, sizeof(txmsg.buf));

  memset(buffer, 0, sizeof(buffer));

  Focus.attach(1);
  Zoom.attach(0);
  focus_pos = focus_table[0];
  zoom_pos = zoom_table[0];
  Focus.write(focus_pos);
  Zoom.write(zoom_pos);
  DEBUGSerial.println("Optical Run");
}

void loop() 
{
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

  CAMDEBUG DEBUGSerial.printf("Dist : %d, F : %d, Z : %d\n", dist, Focus.read(), Zoom.read());

  PYUSB {
    if (Usb_Read() == false) {
      Usb_Send(FAILED);
    }
  }
  else {
    if (Can_Read() == false) {
      Can_Send(FAILED);
    }
  }
}

bool Data_Read(int &return_val) {
  CANDEBUG {
  DEBUGSerial.println("---------------------------rx");
  DEBUGSerial.printf("%02X", rxmsg.id);
  for (int i = 0; i < 8; i++)
    DEBUGSerial.printf(" %02X", rxmsg.buf[i]);
  DEBUGSerial.printf(" | time:%d \n\r", millis());
  }

  if (rxmsg.len == 3) {
    /* Read Mode */
    if (((rxmsg.buf[0] & MODE_MASK) >> 7) == 0x00 ) {
      /* TOF */
      if (((rxmsg.buf[0] & DEVICE_MASK) >> 6) == 0x00 ) {

        if ((rxmsg.buf[0] & COMMAN_MASK) == 0x00 ) {
          if (rxmsg.buf[1] != 0) return false;
          return_val = THIS_ID;
          CANDEBUG DEBUGSerial.printf("THIS IS %d, %02X\n",return_val ,return_val); // ACK
        }
        else if ((rxmsg.buf[0] & COMMAN_MASK) == 0x01 ) {             // Read Tof distance
          return_val = dist;
          CANDEBUG DEBUGSerial.printf("Read Tof distance %d, %02X\n",return_val ,return_val);
        }
        else if ((rxmsg.buf[0] & COMMAN_MASK) == 0x02 ) {        // Read Tof Strength
          return_val = strength;
          CANDEBUG DEBUGSerial.printf("Read Tof Strength %d, %02X\n",return_val ,return_val);
        }
      }
      /* Camera */
      else {
        if ((rxmsg.buf[0] & COMMAN_MASK) == ZOOM_TABLE ) {      // Read zoom table
          if (rxmsg.buf[1] > 9) return false;
          return_val = zoom_table[rxmsg.buf[1]];
          CANDEBUG DEBUGSerial.printf("Read zoom table [%d] %d, %02X\n",rxmsg.buf[1], return_val ,return_val);
        }
        else if ((rxmsg.buf[0] & COMMAN_MASK) == FOCUS_TABLE ) {// Read focus table
          if (rxmsg.buf[1] > 9) return false;
          return_val = focus_table[rxmsg.buf[1]];
          CANDEBUG DEBUGSerial.printf("Read focus table [%d] %d, %02X\n",rxmsg.buf[1], return_val ,return_val);
        }
        else if ((rxmsg.buf[0] & COMMAN_MASK) == FOCUS_TABLE ) { // Read zoom
          return_val = strength;
          CANDEBUG DEBUGSerial.printf("Read zoom %d, %02X\n",return_val ,return_val);
        }
        else if ((rxmsg.buf[0] & COMMAN_MASK) == FOCUS_TABLE ) { // Read focus
          return_val = strength;
          CANDEBUG DEBUGSerial.printf("Read focus %d, %02X\n",return_val ,return_val);
        }
      }
    }
    /* Use Mode */
    else {
      /* TOF */
      if (((rxmsg.buf[0] & DEVICE_MASK) >> 6) == 0x00 ) {
        if ((rxmsg.buf[0] & COMMAN_MASK) == 0x01 ) {            // auto Tof off
          TOF_mode = false;
          return_val = 0;
          CANDEBUG DEBUGSerial.printf("auto Tof off %d, %02X\n",return_val ,return_val);
        }
        if ((rxmsg.buf[0] & COMMAN_MASK) == 0x02 ) {            // auto Tof on
          TOF_mode = true;
          return_val = 1;
          CANDEBUG DEBUGSerial.printf("auto Tof off %d, %02X\n",return_val ,return_val);
        }
      }
      /* Camera */
      else { 
        if ((rxmsg.buf[0] & COMMAN_MASK) == 0x01 ) {            // focus fixed
          if (rxmsg.buf[1] > 9) return false;
          zoom_pos = zoom_table[rxmsg.buf[1]];
          focus_pos = focus_table[rxmsg.buf[1]];
          focus_mode = false;
          return_val = zoom_pos + focus_pos;
          CANDEBUG DEBUGSerial.printf("focus fixed %d, %02X\n",return_val ,return_val);
        }
        if ((rxmsg.buf[0] & COMMAN_MASK) == 0x02 ) {            // focus auto
          focus_mode = true;
          return_val = 0;
          CANDEBUG DEBUGSerial.printf("focus auto %d, %02X\n",return_val ,return_val);
        }
      }
    }
  }
  /* Write Mode */
  else if (rxmsg.len == 4) {
    /* TOF */
    if (((rxmsg.buf[0] & DEVICE_MASK) >> 6) == 0x00 ) {
      if ((rxmsg.buf[0] & COMMAN_MASK) == 0x01 ) {              // Set tof_delay
        TOF_delay = rxmsg.buf[2] * 10;
        return_val = TOF_delay;
        CANDEBUG DEBUGSerial.printf("Set tof_delay %d, %02X\n",return_val ,return_val);
      }
    }
    /* Camera */
    else {
      if ((rxmsg.buf[0] & COMMAN_MASK) == ZOOM_TABLE ) {        // Set zoom table
        if (rxmsg.buf[1] > 9) return false;
        if (rxmsg.buf[2] < ZOOM_MIN || rxmsg.buf[2] > ZOOM_MAX) return false;
        zoom_table[rxmsg.buf[1]] = rxmsg.buf[2];
        return_val = zoom_table[rxmsg.buf[1]];
        CANDEBUG DEBUGSerial.printf("Set zoom table [%d] %d, %02X\n",rxmsg.buf[1], return_val ,return_val);
      }
      else if ((rxmsg.buf[0] & COMMAN_MASK) == FOCUS_TABLE ) {  // Set focus table
        if (rxmsg.buf[1] > 9) return false;
        if (rxmsg.buf[2] < FOCUS_MIN || rxmsg.buf[2] > FOCUS_MAX) return false;
        zoom_table[rxmsg.buf[1]] = rxmsg.buf[2];
        return_val = zoom_table[rxmsg.buf[1]];
        CANDEBUG DEBUGSerial.printf("Set focus table [%d] %d, %02X\n",rxmsg.buf[1], return_val ,return_val);
      }
      else if ((rxmsg.buf[0] & COMMAN_MASK) == ZOOM_CURRENT ) { // Set zoom
        if (rxmsg.buf[2] < ZOOM_MIN || rxmsg.buf[2] > ZOOM_MAX) return false;
        focus_mode = false;
        zoom_pos = rxmsg.buf[2];
        return_val = zoom_pos;
        CANDEBUG DEBUGSerial.printf("Set zoom %d, %02X\n",return_val ,return_val);
      }
      else if ((rxmsg.buf[0] & COMMAN_MASK) == FOCUS_CURRENT ) {  // Set focus
        if (rxmsg.buf[2] < FOCUS_MIN || rxmsg.buf[2] > FOCUS_MAX) return false;
        focus_mode = false;
        focus_pos = rxmsg.buf[2];
        return_val = focus_pos;
        CANDEBUG DEBUGSerial.printf("Set focus %d, %02X\n",return_val ,return_val);
      }
    }
  }
  // else if (rxmsg.len == 11) {
  //   if (rxmsg.buf[0] > 9) return false;
  //   uint8_t buffer[9] = {0,};
  //   memcpy(buffer,&rxmsg.buf[1],rxmsg.buf[0]);
  //   TOF_Send(rxmsg.buf[0], buffer);
  // }
  
   CANDEBUG DEBUGSerial.println("---------------------------rx_");
  return true;
}

void Data_Send(int value) {
  CANDEBUG DEBUGSerial.println("---------------------------tx");
  txmsg.len = 5;
  memcpy(txmsg.buf, &value, 4);
  txmsg.buf[4] = (txmsg.id + txmsg.buf[0] + txmsg.buf[1] + txmsg.buf[2] + txmsg.buf[3]) & 0xFF;
  CANDEBUG DEBUGSerial.printf("CAN DATA FORMAT: ");
  CANDEBUG DEBUGSerial.printf("%02X %02X %02X %02X %02X %02X\n", txmsg.id, txmsg.buf[0],  txmsg.buf[1], txmsg.buf[2], txmsg.buf[3], txmsg.buf[4]);
  CANDEBUG DEBUGSerial.println("---------------------------tx_");
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

  int return_val;
  bool returns = Data_Read(return_val);

  if(returns) Usb_Send(return_val);
  return returns;
}

void Usb_Send(int value) {
  Data_Send(value);
  uint8_t buffer[7];
  buffer[0] = THIS_ID;
  buffer[1] = 5;
  memcpy(&buffer[2], txmsg.buf, 5);

  PYTHONSerial.write(buffer,sizeof(buffer));
}

bool Can_Read() {
  if (!Can0.read(rxmsg))    return true;
  if (rxmsg.id != THIS_ID)  return true;

  int return_val;
  bool returns = Data_Read(return_val);

  if(returns) Can_Send(return_val);
  return returns;
}

void Can_Send(int value) {
  Data_Send(value);
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
  PYUSB Usb_Send(dist);
  else Can_Send(dist);
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