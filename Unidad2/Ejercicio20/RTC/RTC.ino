#include <Reloj.h>
#include <Wire.h>
//Sensor, hora, minutos, segundos, dia, mes, año, diasemana
//Reloj reloj(104,4,0,0,1,4,21,4);
Reloj reloj;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Wire.begin();
  reloj.RelojSet(104,4,0,0,1,4,21,4);
}

void loop() {
  // put your main code here, to run repeatedly:
  reloj.OnOff();
  reloj.ReadTime();
  
  delay(1000);
}
