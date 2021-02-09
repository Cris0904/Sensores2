int button = 19;
int led = 13;

void setup() {
  // put your setup code here, to run once:
  pinMode(led,OUTPUT);
  
  Serial.begin  (9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  
  digitalWrite(led,HIGH);
  delay(100);
}
