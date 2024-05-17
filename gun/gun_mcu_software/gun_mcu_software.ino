#include <FlexCAN_T4.h>
#include "PWMServo.h"
#include "AHRS.h"

#define DEBUGSerial Serial
#define PYSerial
#define AHRSSerial Serial2

FlexCAN_T4<CAN1, RX_SIZE_256, TX_SIZE_16> Can0;
CANListener listener;
static CAN_message_t rxmsg, txmsg;

PWMServo trigger;
#define TRIGGER_PIN 11
#define TRIGGER_OPEN 124
#define TRIGGER_READY 78
#define TRIGGER_ON 30
#define SINGLE_TRIGGER 100
long long trigger_single_time = 0;
uint8_t trigger_state = 1;
uint8_t last_trigger_state = 1;
void trigger_control();

#define LASER_PIN 19

AHRS ahrs(&AHRSSerial);

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

  ahrs.SetDebugSerial(&DEBUGSerial);
  AHRSSerial.begin(921600);
  ahrs.AutoRequestAdd("a");
  ahrs.AutoRequestAdd("g");
  ahrs.AutoRate(2000);
  delay(1000);
}
long long tim = 0;

void loop() {
  // put your main code here, to run repeatedly:
  if (DEBUGSerial.available()) {
    char a = DEBUGSerial.read();
    if (a == 'q')
      trigger_state = 1;
    else if (a == 'w')
      trigger_state = 2;
    else if (a == 'e')
      trigger_state = 3;
    else if (a== 'r') {
      digitalToggle(LASER_PIN);
    } 
    else if (a == 'a') {
      trigger_state = 4;
    }
    DEBUGSerial.println(trigger.read());
  }
  
  trigger_control();
  ahrs.AutoRequest();
  ahrs.AutoRead();
}

void trigger_control() {
  if (millis() - trigger_single_time < SINGLE_TRIGGER) return;
  if (trigger_state == last_trigger_state) return;

  last_trigger_state = trigger_state;

  if (last_trigger_state == 4) {
    trigger_single_time = millis();
    trigger_state = 2;
  }
  switch (last_trigger_state) {
    case 1:
      trigger.write(TRIGGER_OPEN);
    break;
    case 2:
      trigger.write(TRIGGER_READY);
    break;
    case 3:
      trigger.write(TRIGGER_ON);
    break;
    case 4:
      trigger.write(TRIGGER_ON);
    break;
  }
}

