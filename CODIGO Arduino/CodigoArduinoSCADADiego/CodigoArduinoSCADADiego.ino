#include <Separador.h>
#include <EEPROM.h>
#include <OneWire.h>           // Libreria para comunicar con el arduino dispositivos de forma parasita
#include <DallasTemperature.h> // Libreria correspondiente al sensor de temperatura DS18B20
//Variables Sensor de Temepratura Tanque
const int pinDatosDQ = 9; // Pin de Conexion sensor de temperatura
OneWire oneWireObjeto(pinDatosDQ);
DallasTemperature sensorDS18B20(&oneWireObjeto);

//Variables Sensor de Voltaje ZMPT101B
#define sampling 300
#define Amplitud 411
#define RealV 233
int adc_max, adc_min;
int adc_vpp;
//Variable asignada libreria Separador comunicacion Serial
Separador s;

String inputString = "";         // a String to hold incoming data
bool stringComplete = false;  // whether the string is complete

int temp;
int oxi;
int ph;
int I1, I2, I3, Vol1, Vol2, Vol3, estadoMotor;


int OxigenoBase,PhBase,TemperaturaBase,HisteresisOxigeno, HisteresisTemp, HisteresisPh;
int OxigenoBase1,PhBase1,TemperaturaBase1,HisteresisOxigeno1, HisteresisTemp1, HisteresisPh1;

 float current1,current2,current3;

/* Variables Sensores
Sensibilidad del sensor en V/A
float SENSIBILITY = 0.185;   // Modelo 5A
float SENSIBILITY = 0.100; // Modelo 20A
float SENSIBILITY = 0.066; // Modelo 30A
int SAMPLESNUMBER = 100; 
 */
 float SENSIBILITY1 = 0.100; // Modelo 20A Sensor Corriente 1
 float SENSIBILITY2 = 0.100; // Modelo 20A Sensor corriente 2
 float SENSIBILITY3 = 0.100; // Modelo 20A Sensor Corriente 3
 int SAMPLESNUMBER = 100;

#define address1 5 
#define address2 10 
#define address3 15
#define address4 20 
#define address5 25 
#define address6 30

boolean alarmaOxi,alarmaTem,alarmaPh;
float thresholdOxi,thresholdTem,thresholdPh;

#define Oxigenador 13
void setup() {
  // put your setup code here, to run once:
 Serial.begin(9600);
 inputString.reserve(200);
  sensorDS18B20.begin();
 pinMode(Oxigenador,OUTPUT);
 digitalWrite(Oxigenador,LOW);
 EEPROM.get(address1, OxigenoBase1);
  EEPROM.get(address2, TemperaturaBase1);
  EEPROM.get(address3, PhBase1);
  EEPROM.get(address4, HisteresisOxigeno1);
  EEPROM.get(address5, HisteresisTemp1);
  EEPROM.get(address6, HisteresisPh1);
  
 AjusteOxigenador();
}

void loop() {
if (Serial.available()>0){
  while (Serial.read() != 'S');
  serialEvent();
  
}
 corriente1();
 Voltaje();
  oxi = map(analogRead(A0),0,1023,0,20);
 Motor();
Correccion();
 Enviar();
 
}

void serialEvent() {
String datosRecibidos = Serial.readString();
 String elemento1 = s.separa(datosRecibidos,',',1);
 String elemento2 = s.separa(datosRecibidos,',',2);
 String elemento3 = s.separa(datosRecibidos,',',3);
 String elemento4 = s.separa(datosRecibidos,',',4);
 String elemento5 = s.separa(datosRecibidos,',',5);
 String elemento6 = s.separa(datosRecibidos,',',6);
 
 OxigenoBase = elemento1.toInt();
 HisteresisOxigeno = elemento2.toInt();
 TemperaturaBase = elemento3.toInt();
 HisteresisTemp = elemento4.toInt();
 PhBase = elemento5.toInt();
 HisteresisPh = elemento6.toInt();
//EEPROM.put(address,planta);
  EEPROM.put(address1, OxigenoBase);
  
  EEPROM.put(address2, TemperaturaBase);
  
  EEPROM.put(address3, PhBase);
  
  EEPROM.put(address4, HisteresisOxigeno);
  
  EEPROM.put(address5, HisteresisTemp);
  
  EEPROM.put(address6, HisteresisPh);

}


void Enviar(){
  temp = random(0,100);
  //oxi = random(0,100);
  ph = random(0,15);
  Serial.print("O");Serial.println(oxi);
  Serial.print("T");Serial.println(temp);
  Serial.print("P");Serial.println(ph);
  Serial.print("I1");Serial.println(I1);
  Serial.print("I2");Serial.println(I2);
  Serial.print("I3");Serial.println(I3);
  Serial.print("v1");Serial.println(Vol1);
  Serial.print("v2");Serial.println(Vol2);
  Serial.print("v3");Serial.println(Vol3);
  Serial.print("m1");Serial.println(estadoMotor);
  delay(2000);
}

void Correccion(){
    if (oxi<=0){
    oxi=0;
  }
  if (Vol1<=0){
    Vol1=0;
  }
  if (Vol2<=0){
    Vol2=0;
  }
  if (Vol3<=0){
    Vol3=0;
  }
    if (I1<=0){
    I1=0;
  }
  if (I2<=0){
    I2=0;
  }
  if (I3<=0){
    I3=0;
  }
  if (ph<=0){
    ph=0;
  }
}
