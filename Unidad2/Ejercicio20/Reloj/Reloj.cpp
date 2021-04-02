#include "Arduino.h"
#include "Reloj.h"
#include <Wire.h>

Reloj::Reloj(int _DS1307, int hora, int min, int sec, int dia, int mes, int ano, int sdia)
{
    DS1307= _DS1307;
    hour = hora;
    minute = min;
    second = sec;
    year = ano;
    month = mes;
    monthday = dia;
    weekday = sdia;
} 

byte Reloj::decToBcd(byte val) {
  return ((val/10*16) + (val%10));
}

byte Reloj::bcdToDec(byte val) {
  return ((val/16*10) + (val%16));
}

void Reloj::ReadTime(){
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

void Reloj::setTime(){    
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

void Reloj::OnOff(){
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
