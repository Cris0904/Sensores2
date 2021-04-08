#include <WiFi.h>
#include <WiFiMulti.h>

WiFiMulti wifiMulti;

void setup() {
  Serial.begin(115200);
  wifiMulti.addAP("Cris","jhocrisan");
  Serial.println();
  Serial.println("Waiting for wifi...");
  while(wifiMulti.run() != WL_CONNECTED){
    Serial.print(".");
    delay(500);
  }
  Serial.println();
  Serial.println("Connected");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  delay(500);

}

void loop() {
  // put your main code here, to run repeatedly:

}
