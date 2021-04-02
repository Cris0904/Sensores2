#include <Reloj.h>
#include <Wire.h>

Reloj reloj(104,4,0,0,1,4,21,4);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Wire.begin();
}

void loop() {
  // put your main code here, to run repeatedly:
  reloj.OnOff();
  reloj.ReadTime();
  delay(1000);
}
