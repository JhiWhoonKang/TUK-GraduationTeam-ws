#include "RFSerial.h"

RF::RF(HardwareSerialIMXRT *rf, long long initd, long long wait) 
    : RFSerial(rf), sendbuf({CHECK, 0,0,0,0, CHECK}),
    RFconnected(0),
    initaldelay(initd), waitrate(wait)
{
    debug = NULL;
    sendtimer = 0;
    timeouttimer = 0;
    initialdelaytimer = 0;
    waitsigneltimer = 0;
    readready = false;
    sendready = false;
}

void RF::SetDebug(usb_serial_class *_debug) {
    debug = _debug;
    RFDEBUG("RF::Strat debug\n");
}

void RF::ResetDebug() {
    RFDEBUG("RF::End debug\n");
    debug = NULL;
}

void RF::SerialReadByte(uint8_t *data, int size) {
  for (int i = 0; i < size; i++)
    RFDEBUG("%x ",*(data+i));
  RFDEBUG("\n");
}

void RF::RFhandler(int sendrate) {
  if (millis() - timeouttimer > 3000) {
    RFconnected = 0;
    RFDEBUG("RF::RFconnected false timeout\n");
    timeouttimer = millis();
  }
  switch (RFconnected) {
    case COMMUNICATE:
      Communication(sendrate);
    break;
    case ERROR_RESORATION:
      Restoration();
    break;
    case WAIT_FOR_CONNECTION:
      Wait();
    break;
  }
}

void RF::Communication(int sendrate) {
  if (millis() - initialdelaytimer < 1000) return; 

  if (RFSerial->available() >= RECVBUFSIZE) {
    timeouttimer = millis();

    RFSerial->readBytes(recvbuf, RECVBUFSIZE);
    //SerialReadByte(recvbuf, RECVBUFSIZE);

    if (recvbuf[0] == CHECK && recvbuf[RECVBUFSIZE-1] == CHECK)
    {
      memcpy(&recivedata, &recvbuf[1], RECVBUFSIZE-2);
      readready = true;
    }
    else {
      RFconnected = 2;
      RFDEBUG("RF::Recv data Not correct\n");
    }
  }
  if(millis() - sendtimer > sendrate || sendready)
  {
    RFSerial->write(sendbuf, SENDBUFSIZE);
    sendtimer = millis();
    sendready = false;
  }
}

bool RF::Readupdate(uint8_t buffer[RECVBUFSIZE-2]){
    if (readready) {
        memcpy(buffer, &recvbuf[1], RECVBUFSIZE-2);
        readready = false;
        return true;
    }
    return false;
}

void RF::SendUpdate(uint8_t buffer[SENDBUFSIZE-2]) {
    memcpy(&sendbuf[1], buffer, SENDBUFSIZE-2);
    RFDEBUG("RF::Send data update\n");
    sendready = true;
}

void RF::Restoration() {
  if (!RFSerial->available()) return;

  uint8_t data = RFSerial->read();
  
  if (data == CHECK) {
    RFDEBUG("RF::Read byte : %x\n", data);
    RFconnected = 1;
  }
}
void RF::Wait(){

  if (RFSerial->available() == 0) return;

  uint8_t id = RFSerial->read();
  
  if (millis() - waitsigneltimer > waitrate) {
    RFSerial->write(THIS_ID);
    waitsigneltimer = millis();
  }

  if (id == RF_ID) {
    RFDEBUG("RF::ID is correct : %x\n",id);
    id = RFSerial->read();
    
    if (id == RF_ID) {
      RFDEBUG("RF::ID is double correct : %x\n",id);
      RFconnected = 1;
      initialdelaytimer = millis();
    }
    else RFDEBUG("RF::ID is uncorrect : %x\n",id);
  }
  else if (id == CHECK) {
    RFDEBUG("RF::Module Send data Change RFconnceted mode 2 : %x\n",id);
    RFconnected = 2;
  }
  else RFDEBUG("RF::ID is uncorrect : %x\n",id);
}