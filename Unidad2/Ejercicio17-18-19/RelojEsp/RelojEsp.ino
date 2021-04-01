#include <Wire.h>

void setup()
{
  Wire.begin(); // join i2c bus (address optional for master)
  Serial.begin(9600);
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
