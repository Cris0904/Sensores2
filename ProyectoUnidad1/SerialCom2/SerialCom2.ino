  int pinRes = A0;
int pinLed = 8;


void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(pinRes, INPUT);
  pinMode(pinLed, OUTPUT);
}

void taskcom() {
  enum class State {activado, desactivado};
  static State state = State::desactivado;
  static int res = analogRead(pinRes);
  static int resBfr = analogRead(pinRes);
  
  switch (state) {
    case State::desactivado:
      if (Serial.available() > 0) {
        String rcv = Serial.readStringUntil('\n');
        if (rcv.equals("on")) {
          Serial.println("on");
          state = State::activado;
        }
      }
      break;
    case State::activado:
      if (Serial.available() > 0) {
        String rcv = Serial.readStringUntil('\n');
        if (rcv.equals("off")) {
          Serial.print("off\n");
          state = State::desactivado;
        }         
      }
  
      res = analogRead(pinRes);

      if(res-resBfr > 15 || res -resBfr < -15){
        Serial.print("n");
        Serial.println(res);
        resBfr = res;
      }      
      break;
  }
}


void loop() {
  taskcom();
}
