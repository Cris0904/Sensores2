int button = 19;
int led = 13;
int motor = 9;

void setup() {
  // put your setup code here, to run once:
  pinMode(led, OUTPUT);
  pinMode(button, INPUT);
  pinMode(motor, OUTPUT);
  //Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  int state;
  digitalWrite(led,HIGH);
  state = digitalRead(button);
  digitalWrite(motor,HIGH);
  //Serial.println(state);
  
  
  delay(100);
}
