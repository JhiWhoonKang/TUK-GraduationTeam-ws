  #pragma once
#include <math.h>

class Motor {
public:
  uint8_t En;
  uint8_t Dir;
  uint8_t PUL;

  bool Dir_Cali = 0;

  float Ab_Angle = 0;

  float tmp = 0;

  float accel = 0.002;
  float Speed = 0;
  float Max_Speed = 150;
  float Speed_Limit = 150;

  float c_Point = 0;

  float Ab_Pulse = 0;

  Motor(uint8_t E, uint8_t D, uint8_t P, float PR, float Gear_Ratio)
  {
    En = E;
    Dir = D;
    PUL = P;
    tmp = PR * Gear_Ratio / (float)360;
  }

  void Setting()
  {
    pinMode(En, OUTPUT);
    digitalWrite(En, LOW);
    pinMode(Dir, OUTPUT);
    pinMode(PUL, OUTPUT);
  }

  void Manual_Active(float target_speed)  //수동 동작
  {
    if(target_speed > Speed)
    {
      Speed += accel;
    }
    else if(target_speed < Speed)
    {
      Speed -= accel;
    }

    if(target_speed == 0 && Speed < 0.1 && Speed > -0.1)
    {
      Speed = 0;
    }

    Speed = constrain(Speed, -Speed_Limit, Speed_Limit);
    
    c_Point += Speed;

    if(c_Point >= Max_Speed)
    {
      if(Dir_Cali == 1)
      {
        digitalWrite(Dir, HIGH);
      }
      else if(Dir_Cali == 0)
      {
        digitalWrite(Dir, LOW);
      }
      Ab_Pulse += 1;
      digitalWrite(PUL, HIGH);
      delayMicroseconds(5);
      digitalWrite(PUL, LOW);
      c_Point -= Max_Speed;
    }
    else if(c_Point <= -Max_Speed)
    {
      if(Dir_Cali == 1)
      {
        digitalWrite(Dir, LOW);
      }
      else if(Dir_Cali == 0)
      {
        digitalWrite(Dir, HIGH);
      }
      Ab_Pulse -= 1;
      digitalWrite(PUL, HIGH);
      delayMicroseconds(5);
      digitalWrite(PUL, LOW);
      c_Point += Max_Speed;
    }
    Ab_Angle = Ab_Pulse / tmp;
  }

  float Angle_error = 2;

  void Contol_by_Angle(float Angle)
  {
    if(Angle > Angle_error)
    {
      if(Ab_Angle < Angle - Angle_error)
      {
        if(Dir_Cali == 1)
        {
          digitalWrite(Dir, HIGH);
        }
        else if(Dir_Cali == 0)
        {
          digitalWrite(Dir, LOW);
        }
        Ab_Pulse += 1;
        digitalWrite(PUL, HIGH);
        delayMicroseconds(10);
        digitalWrite(PUL, LOW);
      }
      else if(Ab_Angle > Angle + Angle_error)
      {
        if(Dir_Cali == 1)
        {
          digitalWrite(Dir, LOW);
        }
        else if(Dir_Cali == 0)
        {
          digitalWrite(Dir, HIGH);
        }
        Ab_Pulse -= 1;
        digitalWrite(PUL, HIGH);
        delayMicroseconds(10);
        digitalWrite(PUL, LOW);
      }
    }
    else if(Angle < -Angle_error)
    {
      if(Ab_Angle > Angle + Angle_error)
      {
        if(Dir_Cali == 1)
        {
          digitalWrite(Dir, LOW);
        }
        else if(Dir_Cali == 0)
        {
          digitalWrite(Dir, HIGH);
        }
        Ab_Pulse -= 1;
        digitalWrite(PUL, HIGH);
        delayMicroseconds(10);
        digitalWrite(PUL, LOW);
      }
      else if(Ab_Angle < Angle - Angle_error)
      {
        if(Dir_Cali == 1)
        {
          digitalWrite(Dir, HIGH);
        }
        else if(Dir_Cali == 0)
        {
          digitalWrite(Dir, LOW);
        }
        Ab_Pulse += 1;
        digitalWrite(PUL, HIGH);
        delayMicroseconds(10);
        digitalWrite(PUL, LOW);
      }
    }
    Ab_Angle = Ab_Pulse / tmp;
  }
};
