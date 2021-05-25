float getCorriente1(int samplesNumber)
{
   float voltage1;
   float corrienteSum1 = 0;
   for (int i = 0; i < samplesNumber; i++)
   {
      voltage1 = analogRead(A6) * 5.0 / 1023.0;
      corrienteSum1 += (voltage1 - 2.5) / SENSIBILITY1;
   }
   return(corrienteSum1 / samplesNumber);
   Vol1=voltage1;
}
float getCorriente2(int samplesNumber)
{
  float corrienteSum2 = 0;
  float voltage2;
   for (int i = 0; i < samplesNumber; i++)
   {
      voltage2 = analogRead(A7) * 5.0 / 1023.0;
      corrienteSum2 += (voltage2 - 2.5) / SENSIBILITY2;
   }
   return(corrienteSum2 / samplesNumber);
}

float getCorriente3(int samplesNumber)
{
  float corrienteSum3 = 0;
  float voltage3;
   for (int i = 0; i < samplesNumber; i++)
   {
      voltage3 = analogRead(A8) * 5.0 / 1023.0;
      corrienteSum3 += (voltage3 - 2.5) / SENSIBILITY3;
   }
   return(corrienteSum3 / samplesNumber);
}

void corriente1(){
  current1 = getCorriente1(SAMPLESNUMBER);
  I1 = 0.707 * current1;
  current2 = getCorriente2(SAMPLESNUMBER);
  I2 = 0.707 * current2;
  current3 = getCorriente3(SAMPLESNUMBER);
  I3 = 0.707 * current3;
   //printMeasure("Intensidad: ", current, "A ,");
   //printMeasure("Irms: ", currentRMS, "A ,");
   //printMeasure("Potencia: ", power, "W");
  // delay(1000);
}
/*
void Voltage1(){
  int cnt;
  adc_max=0;
  adc_min=1024;
  for(cnt=0;cnt<300;cnt++)
  {
    int adc = analogRead(A3);
    
    if(adc>adc_max)
    {
      adc_max =adc;
    }
    if(adc<adc_min)
    {
      adc_min = adc;
    }
    delay(10);
  }
adc_vpp1 = adc_max-adc_min;
  
    }

    void Voltage2(){
  int cnt;
  adc_max=0;
  adc_min=1024;
  for(cnt=0;cnt<300;cnt++)
  {
    int adc = analogRead(A4);
    
    if(adc>adc_max)
    {
      adc_max =adc;
    }
    if(adc<adc_min)
    {
      adc_min = adc;
    }
    delay(10);
  }
adc_vpp2 = adc_max-adc_min;
  
    }

void Voltage3(){
  int cnt;
  adc_max=0;
  adc_min=1024;
  for(cnt=0;cnt<300;cnt++)
  {
    int adc = analogRead(A5);
    
    if(adc>adc_max)
    {
      adc_max =adc;
    }
    if(adc<adc_min)
    {
      adc_min = adc;
    }
    delay(10);
  }
adc_vpp3 = adc_max-adc_min;
  
    }
    */
void Motor(){
 
  if (oxi > thresholdOxi){
    if (alarmaOxi == 0){
    thresholdOxi = OxigenoBase1 - HisteresisOxigeno1;
     digitalWrite(Oxigenador, LOW);
     alarmaOxi =1;
     estadoMotor = 0;
    }
  }
  else {
    if (alarmaOxi ==1){
      digitalWrite(Oxigenador, HIGH);
      alarmaOxi = 0;
      thresholdOxi = OxigenoBase1 + HisteresisOxigeno1;
      estadoMotor = 1;
  }
}
}

void AjusteOxigenador(){
  oxi = map(analogRead(A0),0,1023,0,20);
  if (oxi<=0){
    oxi=0;
  }
 if (oxi > OxigenoBase1){
    alarmaOxi = 1;
    thresholdOxi = OxigenoBase1 - HisteresisOxigeno1;
     digitalWrite(Oxigenador, LOW);
  }
  else {
    alarmaOxi = 0;
    thresholdOxi = OxigenoBase1 + HisteresisOxigeno1;
    digitalWrite(Oxigenador, HIGH);
  }
}

int ReadTemperatura(){
    sensorDS18B20.requestTemperatures();
    //temp = sensorDS18B20.getTempCByIndex(0);
}
void Voltaje(){
  Vol1 = map(analogRead(A3),0,1023,0,220);
   Vol2 = map(analogRead(A4),0,1023,0,220);
   Vol3 = map(analogRead(A5),0,1023,0,220);
}
