/*
    This sketch sends a message to a TCP server

*/

#include <WiFi.h>
#include <WiFiMulti.h>

WiFiMulti WiFiMulti;

void setup()
{
  pinMode(5, OUTPUT);
  Serial.begin(115200);
  delay(10);

  // We start by connecting to a WiFi network
  if (true) {
    WiFiMulti.addAP("", "");
  }

  Serial.println();
  Serial.println();
  Serial.print("Waiting for WiFi... ");

  while (WiFiMulti.run() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  delay(500);
}

int leer(int puerto) {
  return digitalRead(puerto);
}

void escribir(int puerto) {
  if (leer(puerto) == 0) {
    digitalWrite(puerto, HIGH);
  } else {
    digitalWrite(puerto, LOW);
  }
}

void loop()
{
  //    const uint16_t port = 80;
  //    const char * host = "192.168.1.1"; // ip or dns
  const uint16_t port = 23;
  const char * host = "192.168.0.101"; // ip or dns

  char message[5] = {'1','2','3','4','5'};

  Serial.print("Connecting to ");
  Serial.println(host);

  // Use WiFiClient class to create TCP connections
  WiFiClient client;

  if (!client.connect(host, port)) {
    Serial.println("Connection failed.");
    Serial.println("Waiting 5 seconds before retrying...");
    delay(5000);
    return;
  }

  // This will send a request to the server
  //uncomment this line to send an arbitrary string to the server
  //client.print("Send this data to the server");
  //uncomment this line to send a basic document request to the server
  //client.print("GET /index.html HTTP/1.1\n\n");

  int maxloops = 0;

  //wait for the server's reply to become available
  while (!client.available() && maxloops < 1000)
  {
    maxloops++;
    delay(1); //delay 1 msec
  }
  if (client.available()>0)
  {
    //r5\n
    //w5\n
    //read back one line from the server
    String line = client.readStringUntil('\r');
    Serial.println(line);

    int port = line.substring(1).toInt();

    switch (line[0]) {
      case 'w':
        escribir(port);
        break;
      case 'r':
          Serial.println(leer(port));
        break;
    }
  }
  else
  {
    Serial.println("client.available() timed out ");
  }

  Serial.println("Closing connection.");
  client.stop();

  Serial.println("Waiting 5 seconds before restarting...");
  delay(5000);
}
