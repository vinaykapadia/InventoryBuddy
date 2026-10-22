#include <Arduino.h>
#include <BluetoothSerial.h>
#include <WiFi.h>
#include <cstdint>

#define ADD_INV_CODE "000000000001"
#define END_ADD_CODE "000000000002"
#define SET_ZERO_CODE "000000000003"

#define WIFI_SSID "kapadianet"
#define WIFI_PASS "12345abcde"

#define SCANNER_ADDRESS {0xDC, 0x0D, 0x30, 0x7D, 0x2D, 0x80};
#define SCANNER_END_CODE 9

#define PIN_R 27
#define PIN_G 25
#define PIN_B 32

#define HOST_NAME "10.1.1.52"
#define HTTP_PORT 32323
#define PASSKEY "thisisapasskey!"

BluetoothSerial SerialBT;
WiFiClient client;
int inventoryPolarity = -1;
uint8_t address[6] = SCANNER_ADDRESS;

void setColor(int, int, int);
void red();
void green();
void blue();
void yellow();
void makeCall(int, String);

void setup()
{
  pinMode(PIN_R, OUTPUT);
  pinMode(PIN_G, OUTPUT);
  pinMode(PIN_B, OUTPUT);
  red();

  Serial.begin(9600);
  SerialBT.begin("ESP32", true);
  SerialBT.setTimeout(100);

  WiFi.mode(WIFI_STA);
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  while(!Serial || WiFi.status() != WL_CONNECTED);
}

void loop()
{
  // maintain connection
  if (!SerialBT.connected() || WiFi.status() != WL_CONNECTED)
  {
    red();
    if (!SerialBT.connected())
    {
      Serial.println("Bluetooth Disconnected");
      SerialBT.connect(address);
      Serial.println("Bluetooth Connected");
    }
    if (WiFi.status() != WL_CONNECTED)
    {
      Serial.println("WiFI Disconnected");
      WiFi.reconnect();
      while(!Serial || WiFi.status() != WL_CONNECTED);
      Serial.println("WiFi Connected");
    }
    inventoryPolarity = -1;
  }
  else
  {
    if (inventoryPolarity == 1)
    {
      blue();
    }
    else if (inventoryPolarity == -1)
    {
      green();
    }
    else
    {
      yellow();
    }
  }

  // read barcode
  String barcode = SerialBT.readStringUntil(SCANNER_END_CODE);

  if (barcode == ADD_INV_CODE)
  {
    Serial.println("Read Add Inv");
    inventoryPolarity = 1;
  }
  else if (barcode == END_ADD_CODE)
  {
    Serial.println("Read End Add");
    inventoryPolarity = -1;
  }
  else if (barcode == SET_ZERO_CODE)
  {
    Serial.println("Read Set Zero");
    inventoryPolarity = 0;
  }
  else if (barcode != "")
  {
    Serial.println("Read Barcode " + barcode);
    makeCall(inventoryPolarity, barcode);
  }
}

void makeCall(int polarity, String barcode)
{
  String url = "http://" + String(HOST_NAME) + ":" + HTTP_PORT + "/inventory/" + PASSKEY + "/" + barcode + "/" + polarity;

  if(client.connect(HOST_NAME, HTTP_PORT)) {
    Serial.println("Connected to Server");
    client.println("GET " + url + " HTTP/1.1");
    client.println("Host: " + String(HOST_NAME) + ":" + HTTP_PORT);
    client.println("Connection: close");
    client.println(); // end HTTP request header
    
    while(client.available())
    {
      char c = client.read();
      Serial.print("." + c);
    }

    client.stop();
    Serial.println("Called" + url);
  }
  else
  {
    Serial.println("Connection Failed");
  }
}

void red() { setColor(255, 0, 0); }
void green() { setColor(0, 255, 0); }
void blue() { setColor(0, 0, 255); }
void yellow() { setColor(255, 255, 0); }

void setColor(int r, int g, int b)
{
  analogWrite(PIN_R, r);
  analogWrite(PIN_G, g);
  analogWrite(PIN_B, b);
}