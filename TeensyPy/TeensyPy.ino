#include "Motor_.h"

typedef struct Recive_data{
  volatile int axisX = 0;
  volatile int axisY = 0;
  volatile int axisT = 0;
  volatile unsigned int JButton = 0;
}Recive_data;

Recive_data joy_data;

#define RbuffSize 16

unsigned char Rbuff[RbuffSize] = {NULL, };

Motor Step1(4, 2, 3, 3200, 6);
Motor Step2(7, 5, 6, 3200, 9);

IntervalTimer myTimer;

void setup() {
  Serial.begin(115200);
  Serial.setTimeout(50);
  Step1.Setting();
  Step2.Setting();
  Step2.Max_Speed = 40;
  Step2.Speed_Limit = 40;
  
  myTimer.begin(Move_stepMotor, 10);
}

void Move_stepMotor() {
  if((joy_data.JButton & 4) == 4)
  {
    Step1.Manual_Active((float)joy_data.axisX);
    Step2.Manual_Active((float)joy_data.axisY);
  }
}

void loop() {
  if(Serial.available() > 0)
  {
    Serial.readBytes(Rbuff, RbuffSize);
    if(Rbuff[RbuffSize - 1] == 0xfc)
    {
      memcpy(&joy_data, Rbuff, RbuffSize);
      Serial.print(Step1.Speed);
      Serial.print(", ");
      Serial.print(Step1.c_Point);
      Serial.print(", ");
      Serial.println(joy_data.axisX);
    }
  }  
}