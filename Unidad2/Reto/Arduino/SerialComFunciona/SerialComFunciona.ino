#include <Reloj.h>
#include <Wire.h>

static byte s[1] = {0x00};
//static int info[7] = {0,0,0,0,0,0,0};
static byte info[7] = {0x02,0x02,0x02,0x02,0x02,0x02,0x02};
static int DS1307 = 104;
bool setted;
Reloj reloj;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Wire.begin();
  setted = false;
}

byte bcdToDec(byte val) {
  return ((val/16*10) + (val%16));
}

void TaskCom(){
  if(Serial.available()>0){
      Serial.readBytes(s,1);
      reloj.RelojSet(DS1307, 4,0,0,1,4,21,4);
      setted = true;
          
      if(s[0] == 0x4a){
        if(Serial.available()>6){
          Serial.readBytes(info,7);
          //Sensor, hora, minutos, segundos, dia, mes, año, diasemana
          //reloj.RelojSet(DS1307, bcdToDec(info[0]),bcdToDec(info[1]),bcdToDec(info[2]),bcdToDec(info[3]),bcdToDec(info[4]),bcdToDec(info[5]),bcdToDec(info[6]));
          Serial.print("XD");
          //reloj.setTime();
          
          //reloj.RelojSet(DS1307,info[0],info[1],info[2],info[3],info[4],info[5],info[6]);
        }
      }else if(s[0] == 0x3e){
        
        //reloj.RelojSet(DS1307,bcdToDec(info[0]),bcdToDec(info[1]),bcdToDec(info[2]),bcdToDec(info[3]),bcdToDec(info[4]),bcdToDec(info[5]),bcdToDec(info[6]));
        Serial.write(0x4a);
        reloj.ReadTime();  //Envia hora, minuto, segundos, dia, mes, año, dia semana 
        s[0]= 0x00;       
      }
      
  }
}

void loop() {
  if(setted){
    reloj.OnOff(DS1307);
  }
  TaskCom();
}
