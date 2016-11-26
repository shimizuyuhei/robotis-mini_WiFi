#include <ESP8266WiFi.h>

/*アクセスポイント情報 host:192.168.4.1 */
const char *ssid = "wroom";
const char *password = "robotismini";
/*ポート設定*/
WiFiServer server(55555);

WiFiClient client;

void setup() {
  Serial.begin(115200);
  delay(10);
  
  /*アクセスポイントとして起動*/
  WiFi.softAP(ssid, password);
  IPAddress myIP = WiFi.softAPIP();
  Serial.println();
  //.Serial.print("AP IP address: ");
  //Serial.println(myIP);
  server.begin();
}

void loop() {
  // put your main code here, to run repeatedly:
  client = server.available();
  if (!client) {
    return;
  }

  //Serial.println("conected");
  while(client.connected())
  { 
    
    TCPread();
    
    //送信処理
    // This will send the request to the server
    TCPsend();
    //client.println("abc");
    //client.flush();  
  }
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
  char dataHL[3] = {0,0,'\n'};
  
  //改行コードを入れて送信  
  while(Serial.available()){
    data = Serial.read();
    dataHL[0] = (char)((data >> 4) + 0x10);
    dataHL[1] = (char)((data & 0x0f) + 0x20);
    client.print(dataHL); 
  }
   //client.print("adfsdf\n"); 
  //client.flush();
}
