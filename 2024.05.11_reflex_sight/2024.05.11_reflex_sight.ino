//#include "Oled_display.h"

#define Rl_sink 40
#define lR_sink 45

String str_ahrs;
char char_ahrs;
float x, y, z;
int idx1, idx2, idx3;

typedef struct Tx_data{ //reflex sight --> laser sight
  float gunX, gunY, gunZ;
  uint8_t Tx_sink = Rl_sink;
  uint8_t Dead1 = 0;
};

typedef struct Rx_data{ //reflex sight <-- laser sight
  uint8_t con_status; //수락 여부(0:disc. 1:request..., 2:conected, 3: failed)
  boolean fire_status; //RCWS 발사 여부
  boolean lockOn_status; // RCWS의 목표물 lockOn 여부
  uint8_t Rx_sink = lR_sink;
};

unsigned char Recvbuf[4] = {NULL, };
unsigned char Sendbuf[16] = {NULL, };
volatile bool commute_state = 1;


Tx_data tx_data;
Rx_data rx_data;

void setup() {

  Serial.begin(57600);
  Serial2.begin(57600); //laser sight간 서로 통신
  Serial3.begin(57600); //AHRS와 서로 통신


  pinMode(2, INPUT_PULLUP);

  
}

void loop() {

  if(commute_state == 1){ //commute_state가 1이면 송신 0이면 수신
    memcpy(Sendbuf, &tx_data, sizeof(tx_data)); //데이터를 전송하기 위한 메모리 복사
    Serial2.write(Sendbuf, sizeof(Sendbuf)); //레이저 사이트에 데이터 전송
    commute_state = 0;
  }else{
    if(Serial2.available() > 0){
      Serial2.readBytes(Recvbuf, sizeof(Recvbuf));
      if(Recvbuf[3] == 45){
        memcpy(&rx_data, Recvbuf, sizeof(Recvbuf));
        commute_state = 1;
        Serial.println(rx_data.fire_status);
      }
    }   
  }
  if(digitalRead(2) == LOW){
    
  }
    
  RFZ_fun(); //AHRS로부터 데이터 불러들이기
//  Serial.print(tx_data.gunY);
//  Serial.print(" ");
//  Serial.println(tx_data.gunZ);
  

}

void RFZ_fun(){ //AHRS로부터 데이터 수신 받는 함수
  if(Serial3.available() > 0){
    char_ahrs = Serial3.read();
    if(char_ahrs == '\n'){
      idx1 = str_ahrs.indexOf(',');
      idx2 = str_ahrs.indexOf(',', idx1+1);
      idx3 = str_ahrs.length();

      tx_data.gunX = str_ahrs.substring(0, idx1).toFloat();
      tx_data.gunY = str_ahrs.substring(idx1+1, idx2).toFloat();
      tx_data.gunZ = str_ahrs.substring(idx2+1, idx3).toFloat();

      str_ahrs = "";
    }else{
      str_ahrs += char_ahrs;
    }
  }
}
