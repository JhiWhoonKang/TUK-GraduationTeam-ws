void setup() {
    pinMode(19, OUTPUT);
}

void loop() {
  if(Serial.available()>0)
  {
    char input = Serial.read();
    if(input == 'a')
    {
      digitalWrite(19, HIGH);
    }
    if(input == 'b')
    {
      digitalWrite(19, LOW);
    }
  }
}
