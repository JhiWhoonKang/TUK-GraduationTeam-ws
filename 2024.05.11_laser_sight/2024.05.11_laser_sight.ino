
#define lR_sink 45
#define Rl_sink 40
#define lRC_sink 35
#define RCl_sink 30

typedef struct Tx_data{
  uint8_t con_status; //수락 여부(0:disc. 1:request..., 2:conected, 3: failed)
  bool fire_status; //RCWS 발사 여부
  bool lockOn_status; // RCWS의 목표물 lockOn 여부
  uint8_t Tx_sink = lR_sink;
};

typedef struct Rx_data{ //reflex sight --> laser sight
  float gunX, gunY, gunZ;
  uint8_t Rx_sink = Rl_sink;
  uint8_t Dead1 = 0;
};
//-------------------------------------------------------------------------------
typedef struct Sendata{
  float gunY1, gunZ2; //기총의 방향각
  bool control_request; //제어권 요청 
  bool fire_request; //발사 요청
  uint8_t Tx_sink1 = lRC_sink;
  uint8_t Dead2 = 0;
};

typedef struct Recivedata{
  bool lockOn_status;
  bool permit_status;
  bool fire_status;
  bool dead1 = 0;
  uint8_t Rx_sink1 = RCl_sink;
};
//-------------------------------------------------------------------------------

unsigned char Sendbuf[4] = {NULL, };
unsigned char Recvbuf[16] = {NULL, };
unsigned char Sendbuf1[] = {NULL, };
unsigned char Recvbuf1[6] = {NULL, };
volatile bool commute_state = 0;
volatile bool commute_state1 = 1;

Rx_data rx_data;
Tx_data tx_data;

Sendata sendata;
Recivedata recivedata;

void setup() {
  
  Serial.begin(57600);
  Serial2.begin(57600); //laser와 RCWS간 서로 통신
  Serial3.begin(57600); //reflex sight간 서로 통신

  pinMode(2, INPUT_PULLUP);

}

void loop() {

//  if(commute_state1 == 0){ //RCWS로부터 데이터 수신
//    if(Serial2.available() > 0){ //RCWS로부터 데이터 수신
//      Serial2.readBytes(Recvbuf1, sizeof(Recvbuf1));
//      if(Recvbuf[4] == 30){
//        
//        commute_state1 = 1;
//      }
//    }
//  }else{ //RCWS에 데이터 전송
//    memcpy(Sendbuf1, &sendata, sizeof(sendata));
//    Serial2.write(Sendbuf1, sizeof(Sendbuf1));
//    commute_state1 = 0;
//  }
  
  if(commute_state == 1){
    memcpy(Sendbuf, &tx_data, sizeof(tx_data));
    Serial3.write(Sendbuf, sizeof(Sendbuf));
    commute_state = 0;
  }else{
    if(Serial3.available()>0){ //reflex sight로부터 데이터 수신
      Serial3.readBytes(Recvbuf, sizeof(Recvbuf));
      if(Recvbuf[12] == 40){
        memcpy(&rx_data, Recvbuf, sizeof(Recvbuf));
        Serial.print(rx_data.gunY);
        Serial.print(" ");
        Serial.println(rx_data.gunZ);
        commute_state = 1;
      }
    }
  }

  if(digitalRead(2) == LOW){
    tx_data.fire_status = 1;
  }else{
    tx_data.fire_status = 0;
  }

}
