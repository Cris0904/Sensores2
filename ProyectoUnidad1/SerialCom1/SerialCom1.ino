int button = 19;
int led = 5;
byte msg[1] = {0x3D};
byte act[1] = {0x4A};
unsigned long localTime;
unsigned long lastTime = 0;
static int x = HIGH;
void setup() {
  // put your setup code here, to run once:
  pinMode(led, OUTPUT);
  pinMode(button, INPUT);
  Serial.begin(9600);
}

void taskcom() {
  enum class State {activado, desactivado};
  static State state = State::desactivado;
  static byte r[1] = {0x4A};
  static byte s[1] = {0x4A};
  static byte e[1] = {0x3E};

  switch (state) {
    case State::activado: //Boton para pausar y despausar el juego
      if (Serial.available() > 0) {
        Serial.readBytes(s, 1);
        if (s[0] == 0x4a) {
          state = State::desactivado;
          Serial.write(e, 1); //Mando 3e para decir que ya esta activo
        }
      
      }

    
      x = digitalRead(button);
      if (x == LOW) {
        localTime = millis() - lastTime;
      } else {
        lastTime = millis();
      }

      if (localTime > 500) {        
        Serial.write(msg, 1); //Mando 3D
        localTime = 0;
        lastTime = millis();

        if (digitalRead(led) == HIGH) {
          digitalWrite(led, LOW);
        } else {
          digitalWrite(led, HIGH);
        }
      }


      break;

    case State::desactivado:  
      if (Serial.available() > 0) {
        Serial.readBytes(s, 1);
        if (s[0] == 0x3E) {
          Serial.write(r, 1); //Mando 4a para decir que ya esta activo
          state = State::activado;
        }
      }
      break;
  }
}



void loop() {
  taskcom();
}
