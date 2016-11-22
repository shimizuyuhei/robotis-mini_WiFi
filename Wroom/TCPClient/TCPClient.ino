/*
 *  This sketch sends data via HTTP GET requests to data.sparkfun.com service.
 *
 *  You need to get streamId and privateKey at data.sparkfun.com and paste them
 *  below. Or just customize this script to talk to other HTTP servers.
 *
 */

#include <ESP8266WiFi.h>

const char* ssid     = "WROOM-02";
const char* password = "DarwinMini";

const char* host = "192.168.4.1";
const int httpPort = 55555;

WiFiClient client;

void setup() {
  Serial.begin(115200);
  delay(10);

  // We start by connecting to a WiFi network

  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  
  WiFi.begin(ssid, password);
  
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");  
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}

int value = 0;

void loop() {

  //接続処理
  if (!client.connect(host, httpPort)) {
    Serial.println("connection failed");
    return;
  }
  
  while(client.connected())
  { 
    //送信処理
    TCPsend();
    
    //受信処理
    TCPread();
  }
  Serial.println();
  Serial.println("closing connection");
}

void TCPread()
{
  //受信処理
  /*DATA*/
  char data;
  char dataH;
  char dataL;
  
  // Read all the lines of the reply from server and print them to Serial
  while(client.available()){
    data = client.read();
    if((data & 0x10) == 0x10)
    {
      dataH = data;
    }
    else if((data & 0x20) == 0x20)
    {
      dataL = data;
    }
    
    if(dataH != 0 && dataL != 0)
    {
      //Serial.print((char)((dataH << 4) | (dataL & 0x0f)),HEX);
      Serial.print((char)((dataH << 4) | (dataL & 0x0f)));
      dataH = 0;
      dataL = 0;
    }
  }
}

void TCPsend()
{
  char data;
  char dataH;
  char dataL;
  
  //改行コードを入れて送信  
  while(Serial.available()){
    data = Serial.read();
    Conversion(data,&dataH,&dataL);
    client.print(dataH); 
    client.print(dataL);
  }
   //client.print("adfsdf\n"); 
  //client.flush();
}

/*送信するbyte列の分解*/
void Conversion(char data,char *dataH,char *dataL)
{
  *dataH = (char)((data >> 4) + 0x10);
  *dataL = (char)((data & 0x0f) + 0x20);
}
