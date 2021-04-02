#include <Wire.h>

int DS1307 = 104;
int hour;
int minute;
int second;
int year;
int month;
int monthday;
int weekday;


void setup()
{
  Wire.begin(); // join i2c bus (address optional for master)
  Serial.begin(9600);
}
byte decToBcd(byte val) {
  return ((val/10*16) + (val%10));
}
byte bcdToDec(byte val) {
  return ((val/16*10) + (val%16));
}

void ReadTime(){
  Wire.beginTransmission(DS1307);
  Wire.write(byte(0));
  Wire.endTransmission();
  Wire.requestFrom(DS1307, 7);
  byte second = bcdToDec(Wire.read());
  byte minute = bcdToDec(Wire.read());
  byte hour = bcdToDec(Wire.read());
  byte weekday = bcdToDec(Wire.read());
  byte monthday = bcdToDec(Wire.read());
  byte month = bcdToDec(Wire.read());
  byte year = bcdToDec(Wire.read());
  Wire.endTransmission();
  Serial.print(hour);
  Serial.print(":");
  Serial.print(minute);
  Serial.print(":");
  Serial.println(second);

  Serial.print(monthday);
  Serial.print("/");
  Serial.print(month);
  Serial.print("/");
  Serial.print(year);
  Serial.print("    ");
  Serial.println(weekday);
  
   
}

void setTime(){    
  Wire.beginTransmission(DS1307);
  Wire.write(byte(0));
  Wire.write(decToBcd(second));
  Wire.write(decToBcd(minute));
  Wire.write(decToBcd(hour));
  Wire.write(decToBcd(weekday));
  Wire.write(decToBcd(monthday));
  Wire.write(decToBcd(month));
  Wire.write(decToBcd(year));
  Wire.write(byte(0));
  Wire.endTransmission();
}

void OnOff(){
  static int y =-1;
  Wire.beginTransmission(DS1307); // 
  int x = Wire.endTransmission();    // stop transmitting

  if(x != y){
    y = x;
    if(y == 0){
      Serial.println("Conectado");
      setTime();
    }
    if(y == 3){
      Serial.println("Desconectado");
    }
  } 
}

void loop(){
  OnOff();
  ReadTime();
  delay(1000);
}
