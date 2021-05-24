
#include <WiFi.h>
#include <WiFiUdp.h>

const char* ssid = "";
const char* password = "";
WiFiUDP udpDevice;
uint16_t localUdpPort = 32003;
uint16_t UDPPort = 32004;
#define MAX_LEDSERVERS 3
const char* hosts[MAX_LEDSERVERS] = {"192.168.1.12", "?.?.?.?", "?.?.?.?"};
#define SERIALMESSAGESIZE 3
uint32_t previousMillis = 0;
#define ALIVE 1000



//SPI   //////////
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

enum class State {firstByte, oneCase};
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
  state = State::firstByte;
}

byte bcdToDec(byte val) {
  return ((val / 16 * 10) + (val % 16));
}


void enviarFloat(float num) {
  static uint8_t arr[4] = {0};
  memcpy(arr, (uint8_t *)&num, 4);
  //Serial.write(arr,4);
  udpDevice.beginPacket(hosts[0] , UDPPort);
  Serial.print("envia un flotante");
  Serial.println(num);
  udpDevice.write(arr, 4);
  udpDevice.endPacket();

}

void Clock() {
  switch (state) {
    case State::firstByte: {
        //Serial.println("Primer estado");
        uint8_t packetSize = udpDevice.parsePacket();
        if (packetSize) {
          Serial.print("llega");
          Serial.println(packetSize);
          udpDevice.read(s, 1);

          if (s[0] == 0x3e) {
            state = State::oneCase;
            Serial.println("OneCase");
          }
        }
      }
      break;
    case State::oneCase: {
        enviarFloat(bme.readTemperature());
        enviarFloat(bme.readAltitude(SEALEVELPRESSURE_HPA));
        enviarFloat(bme.readHumidity());
        state = State::firstByte;
        break;
      }
  }
}


  void loop() {
    Clock();
  }
