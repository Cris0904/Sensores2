#include <Reloj.h>
#include <Wire.h>

#include <SPI.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>

#define BME_SCK 18
 #define BME_MISO 19
 #define BME_MOSI 23
 #define BME_CS 5

#define SEALEVELPRESSURE_HPA (1013.25)

Adafruit_BME280 bme(BME_CS);

static byte s[1] = {0x00};
//static int info[7] = {0,0,0,0,0,0,0};
static byte info[7] = {0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02};
static int DS1307 = 104;
bool setted;
bool rel;
Reloj reloj;

enum class State {firstByte, sevenCase, oneCase};
State state;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  bme.begin();  
  Wire.begin();
  setted = false;
  rel = false;
  state = State::firstByte;
}

byte bcdToDec(byte val) {
  return ((val / 16 * 10) + (val % 16));
}

void enviarFloat(float num){
  static uint8_t arr[4] = {0};
  memcpy(arr,(uint8_t *)&num,4);
  Serial.write(arr,4);
}

void TaskCom() {
  switch (state) {
    case State::firstByte:
      if (Serial.available() > 0) {
        Serial.readBytes(s, 1);
        if (s[0] == 0x4a) {
          state = State::sevenCase;
        } else if (s[0] == 0x3e) {
          state = State::oneCase;
        }
        s[0] = 0x00;
      }
      break;
    case State::oneCase:
      if (rel) {
        Serial.write(0x4a);
        reloj.ReadTime();  //Envia hora, minuto, segundos
        enviarFloat(bme.readTemperature());
        enviarFloat(bme.readAltitude(SEALEVELPRESSURE_HPA));
        enviarFloat(bme.readHumidity());
        
        state = State::firstByte;
      }
      break;
    case State:: sevenCase:
      if (Serial.available() > 6) {
        Serial.readBytes(info, 7);
        //Sensor, hora, minutos, segundos, dia, mes, año, diasemana
        reloj.RelojSet(DS1307, bcdToDec(info[0]),bcdToDec(info[1]),bcdToDec(info[2]),bcdToDec(info[3]),bcdToDec(info[4]),bcdToDec(info[5]),bcdToDec(info[6]));
        //reloj.RelojSet(DS1307, 4, 0, 0, 1, 4, 21, 4);
        setted = true;
        
        //Serial.print(reloj.hour);
        state = State::firstByte;
        break;
      }
  }


/*
  if (Serial.available() > 0) {
    Serial.readBytes(s, 1);

    if (s[0] == 0x4a) {
      if (Serial.available() > 6) {
        Serial.readBytes(info, 7);
        //Sensor, hora, minutos, segundos, dia, mes, año, diasemana
        //reloj.RelojSet(DS1307, bcdToDec(info[0]),bcdToDec(info[1]),bcdToDec(info[2]),bcdToDec(info[3]),bcdToDec(info[4]),bcdToDec(info[5]),bcdToDec(info[6]));
        reloj.RelojSet(DS1307, 4, 0, 0, 1, 4, 21, 4);
        setted = true;
        Serial.print(reloj.hour);
      }
    } else if (s[0] == 0x3e && rel) {
      Serial.write(0x4a);
      reloj.ReadTime();  //Envia hora, minuto, segundos, dia, mes, año, dia semana

    }
  }*/
}

void loop() {
  if (setted) {
    reloj.OnOff(DS1307);
    rel = true;
  }
  TaskCom();
}
