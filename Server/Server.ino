#include <ESP8266WiFi.h>

/*アクセスポイント情報 host:192.168.4.1 */
const char *ssid = "ESPap";
const char *password = "thereisnospoon";
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
    
    client.print("abc\n"); 
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
      Serial.print((char)((dataH << 4) | (dataL & 0x0f)),HEX);
      dataH = 0;
      dataL = 0;
    }
    
    //Serial.print(data[i],HEX);
  }
}
