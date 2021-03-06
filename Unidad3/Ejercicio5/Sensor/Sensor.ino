 #include <WiFi.h>
 #include <WiFiUdp.h>

 const char* ssid = "UNE_HFC_B9E0";
 const char* password = "ADBADDEF";
 WiFiUDP udpDevice;
 uint16_t localUdpPort = 32001;
 uint32_t previousMillis = 0;
 #define ALIVE 10000
 #define D0 5
 #define D8 18

 void setup() {
     pinMode(D0, OUTPUT);     // Initialize the LED_BUILTIN pin as an output
     digitalWrite(D0, HIGH);
     pinMode(D8, INPUT);
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
 }


 void networkTask() {
   int info = digitalRead(D8);

   if(info == HIGH){   
   Serial.println("XD..");
   udpDevice.beginPacket(udpDevice.remoteIP(), udpDevice.remotePort());
   udpDevice.write('S');
   udpDevice.write('e');
   udpDevice.write('n');
   udpDevice.write('s');
   udpDevice.write('o');
   udpDevice.write('r');
   udpDevice.write(' ');
   udpDevice.write('2');
   
   udpDevice .endPacket();
   }else{
   }

   
   
     
 }


 void loop() {
     networkTask();
     delay(100);
 }
