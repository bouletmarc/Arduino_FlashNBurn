//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################
//
// EEPROM Programmer - code for an Arduino Mega 2560 - Written by BM Devs (Bouletmarc)
//
// This software presents a 9600-8N1 serial port. It is made to Read/Write Data from
// EEPROM directly with the Arduino! It Doesnt need any extrenal equipement except
// Wires and the Chip itself. Default Serial Timeout is set to 50ms to read the received
// bytes. This program is made to read 28pins Chips such as (27cXXX, 29cXXX, 27sf512).
// 
// For more informations visit : https://www.github.com/bouletmarc/Arduino_FlashNBurn/
//
//
//
//
//
//---------------------------------------------------------------------------------------------------------------------------------------------------------
// COMMANDS :                                         |   OUTPUTS :               |   DESC :
//---------------------------------------------------------------------------------------------------------------------------------------------------------
// R + 2 + "MSB" + "LSB"                              | "B1" + "B2" + "...B256"   |   Reads 256 bytes of data from the EEPROM at address "MSB" + "LSB"  ... Only for No-SST Chip
// R + 5 + "MSB" + "LSB"                              | "B1" + "B2" + "...B256"   |   Reads 256 bytes of data from the EEPROM at address "MSB" + "LSB"  ... Only for SST Chip
// W + 2 + "MSB" + "LSB" + "B1" + "B2" + "..."        | "O"                       |   Writes 256 bytes of data to the EEPROM at address "MSB" + "LSB", This output 'O' (0x79) for telling 'OK'  ... Only for No-SST Chip
// W + 5 + "MSB" + "LSB" + "B1" + "B2" + "..."        | "O"                       |   Writes 256 bytes of data to the EEPROM at address "MSB" + "LSB", This output 'O' (0x79) for telling 'OK'  ... Only for SST Chip
// E + 5                                              | "O"                       |   Erase all the data on the SST Chip
// V + V                                              | "V1" + "V2" + "V3"        |   Prints the version bytes (Ex: V1.2.5 = 0x01, 0x02, 0x05)
//---------------------------------------------------------------------------------------------------------------------------------------------------------
//
//  *** You can directly send the bytes that represent the char ***
//  *** R = DEC 82 // HEX 0x52 ***
//  *** V = DEC 86 // HEX 0x56 ***
//  *** W = DEC 87 // HEX 0x57 ***
//
//---------------------------------------------------------------------------------------------------------------------------------------------------------
//
//
//
//
//
// Exemple of Usage :
//    [0x86 + 0x86]                                        = Read The Version                           ---> (0x86 = V  | 0x86 = V)
//    [0x52 + 0x05 + 0x0F + 0x45]                          = Read 256 bytes from SST Chip at 0x0F45     ---> (0x52 = R  | 0x05 = SST    |  0x0F = "MSB"  |  0x45 = "LSB")
//    [0x57 + 0x02 + 0x53 + 0xD4 + 0x01 + 0x02 + "..."]    = Write 256 bytes from No-SST Chip at 0x53D4 ---> (0x57 = W  | 0x02 = No-SST |  0x0F = "MSB"  |  0x45 = "LSB"  | "++ 256 bytes ++")
//
//
//
//
//
//
//
//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################






int SerialTimeout = 5;


#include <avr/pgmspace.h>

byte VerisonBytes[] = {
  1, 1, 5
};

unsigned int chipType;
byte g_cmd[260];
byte buffer256[256];

#define CHIPOTHERS 2
#define CHIP27SF512 5

//#define PIN_A14 25
#define PIN_A14 22
#define PIN_A12 24
#define PIN_A7  26
#define PIN_A6  28
#define PIN_A5  30
#define PIN_A4  32
#define PIN_A3  34
#define PIN_A2  36
#define PIN_A1  38
#define PIN_A0  40
#define PIN_D0  42
#define PIN_D1  44
#define PIN_D2  46

//#define PIN_nWE 22
#define PIN_nWE 25
#define PIN_A13 27
#define PIN_A8  29
#define PIN_A9  31
#define PIN_A11 33
#define PIN_nOE 35
#define PIN_A10 37
#define PIN_nCE 39
#define PIN_D7  41
#define PIN_D6  43
#define PIN_D5  45
#define PIN_D4  47
#define PIN_D3  49

//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################

void setup()
{
  chipType = 2;
  
  Serial.begin(9600);
  Serial.setTimeout(SerialTimeout);
  //Serial.begin(115200);
  //Serial.begin(921600);

  // Define address lines (outputs)
  pinMode(PIN_A0,  OUTPUT);
  pinMode(PIN_A1,  OUTPUT);
  pinMode(PIN_A2,  OUTPUT);
  pinMode(PIN_A3,  OUTPUT);
  pinMode(PIN_A4,  OUTPUT);
  pinMode(PIN_A5,  OUTPUT);
  pinMode(PIN_A6,  OUTPUT);
  pinMode(PIN_A7,  OUTPUT);
  pinMode(PIN_A8,  OUTPUT);
  pinMode(PIN_A9,  OUTPUT);
  pinMode(PIN_A10, OUTPUT);
  pinMode(PIN_A11, OUTPUT);
  pinMode(PIN_A12, OUTPUT);
  pinMode(PIN_A13, OUTPUT);
  pinMode(PIN_A14, OUTPUT);

  // Define control lines (outputs)
  pinMode(PIN_nCE, OUTPUT); //digitalWrite(PIN_nCE, LOW);
  pinMode(PIN_nOE, OUTPUT); //digitalWrite(PIN_nOE, LOW);
  pinMode(PIN_nWE, OUTPUT); //digitalWrite(PIN_nWE, LOW);
}

//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################

void loop()
{
  while (true)
  {
    //Get Commands
    ReadString();

    //################################
    //################################
    //Send Version
    if (g_cmd[0] == 'V' & g_cmd[1] == 'V')
    {
      Serial.write(VerisonBytes, sizeof(VerisonBytes));
    }

    //################################
    //################################
    //Read Chips
    if (g_cmd[0] == 'R')
    {
      //Set Pinout
      SetDataLinesAsInputs();
      set_ce(LOW);  //enable chip select
      set_oe(LOW);  //enable output
      set_we(HIGH); //disable write
      delay(5);

      //Set Chip Type
      chipType = g_cmd[1];
      
      //Get/Reset Address
      long addr = (g_cmd[2] * 256) + g_cmd[3];
      addr = ResetAddress(addr);
      
      //Read
      ReadIntoBuffer(addr);
      //Send
      Serial.write(buffer256, sizeof(buffer256));

      // Reset Pinout
      set_ce(HIGH); //disable chip select
      set_oe(HIGH); //disable output
    }
    
    //################################
    //################################
    //Write
    if (g_cmd[0] == 'W')
    {
      //Set Pinout
      set_oe(HIGH); //disable output
      set_we(HIGH); //disables write
      SetDataLinesAsOutputs();
      delay(5);

      //Set Chip Type
      chipType = g_cmd[1];
      
      //Get/Reset Address
      long addr = (g_cmd[2] * 256) + g_cmd[3];
      addr = ResetAddress(addr);

      //Add the 256bytes to write to the buffer256
      int LoopIndex = 0;
      while (LoopIndex < 256)
      {
        buffer256[LoopIndex] = g_cmd[LoopIndex + 4];
        LoopIndex++;
      }

      //Write
      WriteBufferToEEPROM(addr);
      //Send 'O' --> for 'OK'
      byte EndByte = 79;
      Serial.write(EndByte);

      //Reset Pinout
      SetDataLinesAsInputs();
    }
    
    //################################
    //################################
    //Erase SST
    if (g_cmd[0] == 'E' & g_cmd[1] == 5)
    {
      set_ce(HIGH);
      set_oe(HIGH);
      set_vpp(HIGH);
      delay(1);
      
      //erase pulse
      set_ce(LOW);
      delay(150);
      set_ce(HIGH);
      delayMicroseconds(1);
      set_vpp(LOW);
      delayMicroseconds(1);

      //Send 'O' --> for 'OK'
      byte EndByte = 79;
      Serial.write(EndByte);
    }
    //################################
    //################################
    
  }
}

//####################################################################################################################################
//####################################################################################################################################
//####################################################################################################################################

void ReadString()
{
  int i = 0;
  byte c;

  //Reset Array Commands
  if (g_cmd[0] != 0) {
    for (i = 0; i < sizeof(g_cmd); i++)
    {
      g_cmd[i] = 0;
    }
  }
        
  //Get Serial Commands if Found
  if (Serial.available())
  {
    Serial.readBytes(g_cmd, sizeof(g_cmd));
  }
}

//##############################################################################
//##############################################################################
//##############################################################################

void SetDataLinesAsInputs()
{
  pinMode(PIN_D0, INPUT);
  pinMode(PIN_D1, INPUT);
  pinMode(PIN_D2, INPUT);
  pinMode(PIN_D3, INPUT);
  pinMode(PIN_D4, INPUT);
  pinMode(PIN_D5, INPUT);
  pinMode(PIN_D6, INPUT);
  pinMode(PIN_D7, INPUT);
}

void SetDataLinesAsOutputs()
{
  pinMode(PIN_D0, OUTPUT);
  pinMode(PIN_D1, OUTPUT);
  pinMode(PIN_D2, OUTPUT);
  pinMode(PIN_D3, OUTPUT);
  pinMode(PIN_D4, OUTPUT);
  pinMode(PIN_D5, OUTPUT);
  pinMode(PIN_D6, OUTPUT);
  pinMode(PIN_D7, OUTPUT);
}

void SetAddress(long a)
{
  digitalWrite(PIN_A0,  (a&1)?HIGH:LOW    );
  digitalWrite(PIN_A1,  (a&2)?HIGH:LOW    );
  digitalWrite(PIN_A2,  (a&4)?HIGH:LOW    );
  digitalWrite(PIN_A3,  (a&8)?HIGH:LOW    );
  digitalWrite(PIN_A4,  (a&16)?HIGH:LOW   );
  digitalWrite(PIN_A5,  (a&32)?HIGH:LOW   );
  digitalWrite(PIN_A6,  (a&64)?HIGH:LOW   );
  digitalWrite(PIN_A7,  (a&128)?HIGH:LOW  );
  digitalWrite(PIN_A8,  (a&256)?HIGH:LOW  );
  digitalWrite(PIN_A9,  (a&512)?HIGH:LOW  );
  digitalWrite(PIN_A10, (a&1024)?HIGH:LOW );
  digitalWrite(PIN_A11, (a&2048)?HIGH:LOW );
  digitalWrite(PIN_A12, (a&4096)?HIGH:LOW );
  digitalWrite(PIN_A13, (a&8192)?HIGH:LOW );
  digitalWrite(PIN_A14, (a&16384)?HIGH:LOW);
}

void SetData(byte b)
{
  digitalWrite(PIN_D0, (b&1)?HIGH:LOW  );
  digitalWrite(PIN_D1, (b&2)?HIGH:LOW  );
  digitalWrite(PIN_D2, (b&4)?HIGH:LOW  );
  digitalWrite(PIN_D3, (b&8)?HIGH:LOW  );
  digitalWrite(PIN_D4, (b&16)?HIGH:LOW );
  digitalWrite(PIN_D5, (b&32)?HIGH:LOW );
  digitalWrite(PIN_D6, (b&64)?HIGH:LOW );
  digitalWrite(PIN_D7, (b&128)?HIGH:LOW);
}

byte ReadData()
{
  byte b = 0;

  if (digitalRead(PIN_D0) == HIGH) b |= 1;
  if (digitalRead(PIN_D1) == HIGH) b |= 2;
  if (digitalRead(PIN_D2) == HIGH) b |= 4;
  if (digitalRead(PIN_D3) == HIGH) b |= 8;
  if (digitalRead(PIN_D4) == HIGH) b |= 16;
  if (digitalRead(PIN_D5) == HIGH) b |= 32;
  if (digitalRead(PIN_D6) == HIGH) b |= 64;
  if (digitalRead(PIN_D7) == HIGH) b |= 128;

  return(b);
}

// converts one character of a HEX value into its absolute value (nibble)
/*byte HexToVal(byte b)
{
  if (b >= '0' && b <= '9') return(b - '0');
  if (b >= 'A' && b <= 'F') return((b - 'A') + 10);
  if (b >= 'a' && b <= 'f') return((b - 'a') + 10);
  return(0);
}*/

//##############################################################################
//##############################################################################
//##############################################################################
long ResetAddress(long addr)
{
  byte hi, low;

  //get high (MSB) - byte of 16 bit address ... the 27x512 has different address pin wiring
  if (chipType == CHIP27SF512) {
    hi = (addr >> 8) & 0x3F;
    hi |= (addr >> 9) & 0x40;
    
    //the 27x512 doesn't use WE, instead it's bit A14
    digitalWrite(PIN_nWE, addr & 0x4000 ? HIGH : LOW);
  } else {
    hi = addr >> 8;
  }

  //get low (LSB) - byte of 16 bit address
  low = addr & 0xff;

  //Remake to Address
  long ToReturn = (hi * 256) + low;

  return ToReturn;
}

inline void set_we (byte state)
{
  //the 27x512 doesn't have a WE pin
  switch (chipType) {
    case CHIP27SF512:
      break;
    default:
      digitalWrite(PIN_nWE, state);
    }
}

void set_vpp (byte state)
{
  switch (chipType) {
  case CHIP27SF512:
    digitalWrite(PIN_nOE, state);
    break;
  default:
    break;
  }
}

inline void set_oe (byte state)
{
  digitalWrite(PIN_nOE, state);
}
 
inline void set_ce (byte state)
{
  digitalWrite(PIN_nCE, state);
}

//############################################################################

void WriteBufferToEEPROM(long addr)
{
  for (byte x = 0; x < 256; ++x)
  {
    SetAddress(addr + x);
    SetData(buffer256[x]);

    switch (chipType) {
    case CHIP27SF512:
      delayMicroseconds(1);
      set_ce(LOW);  //strobe ce with programming pulse
      delayMicroseconds(20); // for 27SF512 (or 100 micro)
      set_ce(HIGH);
      delayMicroseconds(1);
      break;
    default:
      set_ce(LOW);  //enable chip select
      delayMicroseconds(1);
      set_we(LOW);  // enable write
      delayMicroseconds(1);
      //delay(10);
      set_we(HIGH); // disable write
      set_ce(HIGH); //disable chip select
      break;
    }
  }
}

void ReadIntoBuffer(long addr)
{
  for (int x = 0; x < 256; ++x)
  {
    SetAddress(addr + x);
    delayMicroseconds(100);
    buffer256[x] = ReadData();
  }
}

