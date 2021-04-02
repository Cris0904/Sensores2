#include <Wire.h>

void setup()
{
  Wire.begin(); // join i2c bus (address optional for master)
  Serial.begin(9600);
}
byte decToBcd(byte val) {
  return ((val/10*16) + (val%10));
}

void ReadTime(){
  Wire.requestFrom(DS1307, 7);
  second = bcdToDec(Wire.read());
  minute = bcdToDec(Wire.read());
  hour = bcdToDec(Wire.read());
}

void OnOff(){
  int y =-1;
  Wire.beginTransmission(104); // 
  int x = Wire.endTransmission();    // stop transmitting

  if(x != y){
    y = x;
    if(y == 0){
      Serial.println("Conectado");
    }
    if(y == 3){
      Serial.println("Desconectado");
    }
  } 
}



void loop()
{
  OnOff();
}
