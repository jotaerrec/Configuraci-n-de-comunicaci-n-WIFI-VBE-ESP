#include <ESP8266WiFi.h>

//IPAddress ip(192, 168, 43, 150);
//IPAddress gateway(192, 168, 43, 1);
//IPAddress subnet(255, 255, 255, 0);
//IPAddress dns(192, 168, 43, 1);

int led1 = 2;
const char* ssid     = "TeleCentro-9d10";
const char* password = "KZM4EWYJRTM5";

WiFiServer server(80);

void setup()
{
  Serial.begin(9600);
  delay(10);

  pinMode(led1, OUTPUT);
  Serial.println();
  Serial.println();
  Serial.print("Conectando a: ");
  Serial.println(ssid);

  // WiFi.config(ip,dns,gateway,subnet);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi Conectado.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  server.begin();

}


void loop() {
  WiFiClient client = server.available();


  if (client) {
    Serial.println("Nuevo cliente.");


    while (client.connected()) {
      String request = client.readStringUntil('\r');
      Serial.print(request);

      if (request.indexOf("led1") != -1 ) {
        int ind = request.indexOf(':');
        int dataLength = request.length();
        String rta = request.substring(ind+1, dataLength);
        if(rta == "HIGH")
          digitalWrite(led1, HIGH);
        else 
          digitalWrite(led1, LOW);
      } 

      //Caso de querer elegir el pin de manera de dinamica
      /*else if (request.indexOf("pin" != -1)) {
        int ind = request.indexOf(':');
        int dataLength = request.length();
        led1 = request.substring(ind+1, dataLength).toInt();
        pinMode(led1, OUTPUT);
      }*/

      if (led1 != -1) {
        client.print("Led 1 = ");
        client.print(digitalRead(led1));
      }
    }
    client.println("HTTP/1.1 200 OK");
    client.println("Content-type:text/html");
    client.println("");
    //client.println("<meta http-equiv=\"refresh\" content=\"5\" >");
    client.print(",");

    delay(1);
    client.stop();
    Serial.println("Cliente desconectado");

  }
}
