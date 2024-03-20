#include <usb_rawhid.h>

void setup() 
{
  Serial.begin(9600);
}

void loop() 
{
  uint8_t receiveBuffer[64];
  uint8_t sendBuffer[64];
  int recvSize;

  recvSize = RawHID.recv(receiveBuffer, 0);
  if (recvSize > 0) 
  {
    Serial.println("Received Data:");
    for (int i = 0; i < recvSize; i++) 
    {
      Serial.print(receiveBuffer[i], HEX);
      Serial.print(" ");
    }
    Serial.println();

    memset(sendBuffer, 1, sizeof(sendBuffer));

    RawHID.send(sendBuffer, 100);
  }

  delay(100);
}
