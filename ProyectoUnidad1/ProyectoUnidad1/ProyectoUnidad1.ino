int button = 19;
int led = 13;

void setup() {
  // put your setup code here, to run once:
  pinMode(led, OUTPUT);
  pinMode(button, INPUT);
  
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  int state;
  digitalWrite(led,HIGH);
  state = digitalRead(button);

  Serial.println(state);
  
  
  delay(100);
}
