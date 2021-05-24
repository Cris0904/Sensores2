
#include <WiFi.h>
#include <WiFiUdp.h>

const char* ssid = "";
const char* password = "";
WiFiUDP udpDevice;
uint16_t localUdpPort = 32001;
uint16_t UDPPort = 32002;
#define MAX_LEDSERVERS 3
const char* hosts[MAX_LEDSERVERS] = {"192.168.1.12", "?.?.?.?", "?.?.?.?"};
#define SERIALMESSAGESIZE 3
uint32_t previousMillis = 0;
#define ALIVE 1000
#define D0 5



//SPI   //////////
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
  Serial.begin(115200);
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  // Print the IP address
  Serial.println(WiFi.localIP());
  udpDevice.begin(localUdpPort);

  // SPI /////////
  bme.begin();
  Wire.begin();
  setted = false;
  rel = false;
  state = State::firstByte;
}

byte bcdToDec(byte val) {
  return ((val / 16 * 10) + (val % 16));
}
/*
  void networkTask() {

  udpDevice.beginPacket(hosts[0] , UDPPort);
  udpDevice.write(0x4a);
  udpDevice.endPacket();
  byte xxx[1];
  byte yyy[2];
  // UDP event:
  uint8_t packetSize = udpDevice.parsePacket();
  if (packetSize) {
    Serial.print("Data from: ");
    Serial.print(udpDevice.remoteIP());
    Serial.print(":");
    Serial.print(udpDevice.remotePort());
    Serial.println(' ');
    //for (uint8_t i = 0; i < packetSize; i++) {
    //Serial.write(udpDevice.read());
    //}
    udpDevice.read(xxx, 1);
    if (xxx[0] == 0x3e) {
      Serial.println("Es igual");
    }
    udpDevice.read(yyy, 2);
    Serial.write(xxx[0]);
    for (int i = 0; i < sizeof(yyy); i++) {
      Serial.write(yyy[i]);
    }
  }
  }
*/

void Clock() {
  switch (state) {
    case State::firstByte: {
        //Serial.println("Primer estado");
        uint8_t packetSize = udpDevice.parsePacket();
        if (packetSize) {
          Serial.print("llega");
          Serial.println(packetSize);
          udpDevice.read(s, 1);

          if (s[0] == 0x4a) {
            state = State::sevenCase;
            Serial.println("SevenCase");
          } else if (s[0] == 0x3e) {
            state = State::oneCase;
            Serial.println("OneCase");
          }
        }
      }

      /*
        if (Serial.available() > 0) {
          Serial.readBytes(s, 1);
          if (s[0] == 0x4a) {
            state = State::sevenCase;
          } else if (s[0] == 0x3e) {
            state = State::oneCase;
          }
          s[0] = 0x00;
        }*/
      break;
    case State::oneCase: {
        if (rel) {

          byte* datas;
          datas = reloj.ReadTime();
          Serial.println(*datas);
          udpDevice.beginPacket(hosts[0] , UDPPort);
          Serial.println("envia");
          udpDevice.write(0x4a);
          udpDevice.write(*(datas));
          udpDevice.write(*(datas + 1));
          udpDevice.write(*(datas + 2));
          udpDevice.endPacket();
          state = State::firstByte;
        }
      }
      /*
        if (rel) {
          Serial.write(0x4a);
          reloj.ReadTime();  //Envia hora, minuto, segundos
          enviarFloat(bme.readTemperature());
          enviarFloat(bme.readAltitude(SEALEVELPRESSURE_HPA));
          enviarFloat(bme.readHumidity());

          state = State::firstByte;
        }*/
      break;
    case State::sevenCase: {
        Serial.println("Lee");
        udpDevice.read(info, 7);
        if (!setted) {
          reloj.RelojSet(DS1307, bcdToDec(info[0]), bcdToDec(info[1]), bcdToDec(info[2]), bcdToDec(info[3]), bcdToDec(info[4]), bcdToDec(info[5]), bcdToDec(info[6]));
          setted = true;
        }
        state = State::firstByte;

      }
      break;
      /*
        if (Serial.available() > 6) {
          Serial.readBytes(info, 7);
          //Sensor, hora, minutos, segundos, dia, mes, año, diasemana
          reloj.RelojSet(DS1307, bcdToDec(info[0]), bcdToDec(info[1]), bcdToDec(info[2]), bcdToDec(info[3]), bcdToDec(info[4]), bcdToDec(info[5]), bcdToDec(info[6]));
          //reloj.RelojSet(DS1307, 4, 0, 0, 1, 4, 21, 4);
          setted = true;

          //Serial.print(reloj.hour);
          state = State::firstByte;
          break;
        }*/
  }
}


void loop() {
  if (setted) {
    reloj.OnOff(DS1307);
    rel = true;
  }
  Clock();

  //networkTask();
}
