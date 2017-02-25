using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace ArduinoEPROM
{
    public partial class Form1 : Form
    {
        //Settings
        public string Version = "V2.0.0";
        public int SerialWaitTimeout = 500;                                 //max millis to wait if 'BytesToRead' is not reached

        //NOT Settings Values
        public static SerialPort serial;                                    //Create a new serial slot
        public static List<string> AvailablePorts = new List<string>();     //Available COM Ports List
        public bool SerialConnected = false;                                //is serial connected ?
        public string SerialHardware = "";                                  //Serial hardware (arduino or other serial controller)
        public string SerialVersion = "";                                   //Serial hardware version (arduino .ino code version)
        public bool SerialTry = false;                                      //Tried to connect and detect Serial Controller (Yes/No)

        public int ChipSize = 0;                                            //Chip size (32768 = 32kb)
        public string ChipModel = "";                                       //Chip model (ex: 29C256)
        public byte[] Buffer;                                               //Bytes Buffer
        public bool ReadOnly = false;                                       //Is CHip ReadOnly (Yes/No)

        //Loop (Updates) Settings
        public int StartValue = 0;                                          //Start index ... exemple starting at 32768 (0x8000 to 0xFFFF)
        public int CurrentIndex = 0;                                        //Current Index (Over the LoopIndex) ... exemple : 5345 on 32768
        public int LoopIndex = 0;                                           //Number of Index to Loop ... exemple 32768 for 32kb
        public bool Running = false;                                        //Running any Function Loop (Yes/No)
        public string Mode = "";                                            //Running Mode (Read, Write, Verify, Blank, Erase, etc)

        private System.Windows.Forms.Timer LoopTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();

            //Set Loaded
            Log_This("Arduino Flash&Burn Interface Loaded " + Version);

            //Disable Button until Chip is Selected !
            button_SaveBuffer.Enabled = false;
            button_LoadBuffer.Enabled = false;
            button_Blank.Enabled = false;
            button_Edit.Enabled = false;
            button_Erase.Enabled = false;
            button_Read.Enabled = false;
            button_Verify.Enabled = false;
            button_Write.Enabled = false;
            button1.Enabled = false;

            //Start Update Loop
            LoopTimer.Tick += DoThisAllTheTime;
            LoopTimer.Start();
        }

        void DoThisAllTheTime(object sender, EventArgs e)
        {
            Thread.Sleep(1);

            //First Connect
            if (!SerialTry)
            {
                SerialConnect();
                if (!SerialConnected)
                {
                    //Set Anything Disabled
                    listBox_Chips.Enabled = false;
                    textBox_ChipStart.Enabled = false;
                    textBox_ChipEnd.Enabled = false;
                    textBox_BufferStart.Enabled = false;
                    textBox_BufferEnd.Enabled = false;

                    //Set Logs
                    Log_This("Hardware : CAN'T DETECT DEVICE");
                    Log_This("RESTART THE APPLICATION");
                }
                else
                {
                    Log_This("Hardware : " + SerialHardware + " V" + SerialVersion + " CONNECTED");
                    Log_This("Select your Chip !");
                }

                SerialTry = true;
            }

            //###########################
            if (Running)
            {
                //Set Cancel Button
                button1.Enabled = true;

                //Disable Control Button
                button_SaveBuffer.Enabled = false;
                button_LoadBuffer.Enabled = false;
                button_Blank.Enabled = false;
                button_Edit.Enabled = false;
                button_Erase.Enabled = false;
                button_Read.Enabled = false;
                button_Verify.Enabled = false;
                button_Write.Enabled = false;

                //Set Textbox Disabled
                listBox_Chips.Enabled = false;
                textBox_ChipStart.Enabled = false;
                textBox_ChipEnd.Enabled = false;
                textBox_BufferStart.Enabled = false;
                textBox_BufferEnd.Enabled = false;


                if (Mode == "Read")
                {
                    if (CurrentIndex < LoopIndex)
                    {
                        //Set ProgressBar
                        progressBar1.Value = (CurrentIndex * 100) / LoopIndex;

                        //Set MSB/LSB
                        string HexAddress = (CurrentIndex + StartValue).ToString("X4");
                        byte MSB = byte.Parse((HexAddress[0].ToString() + HexAddress[1].ToString()), System.Globalization.NumberStyles.HexNumber);
                        byte LSB = byte.Parse((HexAddress[2].ToString() + HexAddress[3].ToString()), System.Globalization.NumberStyles.HexNumber);
                        //Console.WriteLine("0x" + MSB.ToString("X2") + LSB.ToString("X2") + " = " + ((MSB * 256) + LSB));

                        //Set Commands
                        byte[] Cmds = new byte[4];
                        Cmds[0] = Convert.ToByte('R');
                        Cmds[1] = 2;
                        Cmds[2] = MSB;
                        Cmds[3] = LSB;

                        //Reset 0x02 to 0x05 for SST
                        if (ChipModel == "27SF512 (SST)")
                            Cmds[1] = 5;

                        //Send Commands
                        Write(Cmds);

                        //Receive Bytes
                        byte[] ReceivedBytes = new byte[256];
                        ReceivedBytes = ReadBytes(256);

                        //Add bytes to Buffer
                        for (int i2 = 0; i2 < 256; i2++)
                            Buffer[CurrentIndex + i2] = ReceivedBytes[i2];

                        //Adv Logs
                        if (checkBox1.Checked)
                        {
                            string LogsStr = "Received :" + Environment.NewLine;
                            int LineIndex = 1;
                            for (int i2 = 0; i2 < 256; i2++)
                            {
                                //Add Bytes
                                LogsStr += ReceivedBytes[i2].ToString("X2");

                                //Add Comma
                                if (i2 < 255)
                                    LogsStr += ", ";

                                //Newline each 16bytes
                                if (LineIndex != 16)
                                    LineIndex++;
                                else
                                {
                                    if (i2 < 255)
                                        LogsStr += Environment.NewLine;

                                    LineIndex = 1;
                                }
                            }

                            //Send Logs
                            Log_This(LogsStr);
                        }

                        Log_This("Read successful at : " + "0x" + (StartValue + CurrentIndex).ToString("X4"));

                        CurrentIndex += 256;
                    }
                    else
                    {
                        DisableRunning();
                    }
                }

                //###################################

                if (Mode == "Write")
                {
                    if (CurrentIndex < LoopIndex)
                    {
                        //Set ProgressBar
                        progressBar1.Value = (CurrentIndex * 100) / LoopIndex;

                        //Set MSB/LSB
                        string HexAddress = (CurrentIndex + StartValue).ToString("X4");
                        byte MSB = byte.Parse((HexAddress[0].ToString() + HexAddress[1].ToString()), System.Globalization.NumberStyles.HexNumber);
                        byte LSB = byte.Parse((HexAddress[2].ToString() + HexAddress[3].ToString()), System.Globalization.NumberStyles.HexNumber);

                        //Set Commands
                        byte[] Cmds = new byte[260];
                        Cmds[0] = Convert.ToByte('W');
                        Cmds[1] = 2;
                        Cmds[2] = MSB;
                        Cmds[3] = LSB;

                        //Reset 0x02 to 0x05 for SST
                        if (ChipModel == "27SF512 (SST)")
                            Cmds[1] = 5;

                        //Add 256bytes of the buffer to Commands
                        for (int i2 = 0; i2 < 256; i2++)
                        {
                            int BufferStartValue = int.Parse(textBox_BufferStart.Text, System.Globalization.NumberStyles.HexNumber);
                            Cmds[i2 + 4] = Buffer[CurrentIndex + BufferStartValue];
                        }

                        //Send Commands
                        Write(Cmds);

                        //Receive 79 'O'
                        if (ReadByte() != 79)
                        {
                            Log_This("Chip NOT WRITED");
                            Log_This("Stopped at : " + "0x" + (StartValue + CurrentIndex).ToString("X4"));

                            DisableRunning();
                        }
                        else
                        {
                            Log_This("Write successful at : " + "0x" + (StartValue + CurrentIndex).ToString("X4"));
                        }

                        CurrentIndex += 256;
                    }
                    else
                    {
                        DisableRunning();
                    }
                }

                //###################################

                if (Mode == "Erase")
                {
                    if (ChipModel == "27SF512 (SST)")
                    {
                        byte[] Cmds = new byte[2];
                        Cmds[0] = Convert.ToByte('E');
                        Cmds[1] = 5;
                        Write(Cmds);
                        if (ReadByte() != 79)
                            Log_This("Chip NOT ERASED");
                        else
                            Log_This("Erase successful");

                        DisableRunning();
                    }
                    else
                    {
                        if (CurrentIndex < LoopIndex)
                        {
                            //Set ProgressBar
                            progressBar1.Value = (CurrentIndex * 100) / LoopIndex;

                            //Set MSB/LSB
                            string HexAddress = (CurrentIndex + StartValue).ToString("X4");
                            byte MSB = byte.Parse((HexAddress[0].ToString() + HexAddress[1].ToString()), System.Globalization.NumberStyles.HexNumber);
                            byte LSB = byte.Parse((HexAddress[2].ToString() + HexAddress[3].ToString()), System.Globalization.NumberStyles.HexNumber);

                            //Set Commands
                            byte[] Cmds = new byte[260];
                            Cmds[0] = Convert.ToByte('W');
                            Cmds[1] = 2;
                            Cmds[2] = MSB;
                            Cmds[3] = LSB;

                            //Reset 0x02 to 0x05 for SST
                            if (ChipModel == "27SF512 (SST)")
                                Cmds[1] = 5;

                            //Add 256bytes of Empty DATA to Commands
                            for (int i2 = 0; i2 < 256; i2++)
                                Cmds[i2 + 4] = 255;

                            //Send Commands
                            Write(Cmds);

                            //Receive 79 'O'
                            if (ReadByte() != 79)
                            {
                                Log_This("Chip NOT ERASED");
                                Log_This("Stopped at : " + "0x" + (StartValue + CurrentIndex).ToString("X4"));

                                DisableRunning();
                            }
                            else
                            {
                                Log_This("Erase successful at : " + "0x" + (StartValue + CurrentIndex).ToString("X4"));
                            }

                            CurrentIndex += 256;
                        }
                        else
                        {
                            DisableRunning();
                        }
                    }
                }

                //###################################

                if (Mode == "Blank")
                {
                    if (CurrentIndex < LoopIndex)
                    {
                        //Set ProgressBar
                        progressBar1.Value = (CurrentIndex * 100) / LoopIndex;

                        //Set MSB/LSB
                        string HexAddress = (CurrentIndex + StartValue).ToString("X4");
                        byte MSB = byte.Parse((HexAddress[0].ToString() + HexAddress[1].ToString()), System.Globalization.NumberStyles.HexNumber);
                        byte LSB = byte.Parse((HexAddress[2].ToString() + HexAddress[3].ToString()), System.Globalization.NumberStyles.HexNumber);

                        //Set Commands
                        byte[] Cmds = new byte[4];
                        Cmds[0] = Convert.ToByte('R');
                        Cmds[1] = 2;
                        Cmds[2] = MSB;
                        Cmds[3] = LSB;

                        //Reset 0x02 to 0x05 for SST
                        if (ChipModel == "27SF512 (SST)")
                            Cmds[1] = 5;

                        //Send Commands
                        Write(Cmds);

                        //Receive Bytes
                        byte[] ReceivedBytes = new byte[256];
                        ReceivedBytes = ReadBytes(256);

                        //Check for Bytes
                        bool Success = true;
                        int ErrorIndex = 0;
                        for (int i2 = 0; i2 < 256; i2++)
                        {
                            if (ReceivedBytes[i2] != 0)
                            {
                                ErrorIndex = StartValue + CurrentIndex + i2;
                                Success = false;
                            }
                        }

                        if (Success)
                            Log_This("Blank successful at : " + "0x" + (StartValue + CurrentIndex).ToString("X4"));
                        else
                        {
                            Log_This("Chip NOT BLANK");
                            Log_This("Stopped at : " + "0x" + (ErrorIndex).ToString("X4"));
                            CurrentIndex = LoopIndex;
                            DisableRunning();
                        }

                        CurrentIndex += 256;
                    }
                    else
                    {
                        DisableRunning();
                    }
                }

                //###################################

                if (Mode == "Verify")
                {
                    if (CurrentIndex < LoopIndex)
                    {
                        //Set ProgressBar
                        progressBar1.Value = (CurrentIndex * 100) / LoopIndex;

                        //Set MSB/LSB
                        string HexAddress = (CurrentIndex + StartValue).ToString("X4");
                        byte MSB = byte.Parse((HexAddress[0].ToString() + HexAddress[1].ToString()), System.Globalization.NumberStyles.HexNumber);
                        byte LSB = byte.Parse((HexAddress[2].ToString() + HexAddress[3].ToString()), System.Globalization.NumberStyles.HexNumber);

                        //Set Commands
                        byte[] Cmds = new byte[4];
                        Cmds[0] = Convert.ToByte('R');
                        Cmds[1] = 2;
                        Cmds[2] = MSB;
                        Cmds[3] = LSB;

                        //Reset 0x02 to 0x05 for SST
                        if (ChipModel == "27SF512 (SST)")
                            Cmds[1] = 5;

                        //Send Commands
                        Write(Cmds);

                        //Receive Bytes
                        byte[] ReceivedBytes = new byte[256];
                        ReceivedBytes = ReadBytes(256);

                        //Compare Bytes with Buffer
                        bool Success = true;
                        int ErrorIndex = 0;
                        for (int i2 = 0; i2 < 256; i2++)
                        {
                            if (Buffer[CurrentIndex + i2] != ReceivedBytes[i2])
                            {
                                ErrorIndex = StartValue + CurrentIndex + i2;
                                Success = false;
                            }
                        }

                        //Adv Logs
                        if (checkBox1.Checked)
                        {
                            string LogsStr = "Received :" + Environment.NewLine;
                            int LineIndex = 1;
                            for (int i2 = 0; i2 < 256; i2++)
                            {
                                //Add Bytes
                                LogsStr += ReceivedBytes[i2].ToString("X2");

                                //Add Comma
                                if (i2 < 255)
                                    LogsStr += ", ";

                                //Newline each 16bytes
                                if (LineIndex != 16)
                                    LineIndex++;
                                else
                                {
                                    if (i2 < 255)
                                        LogsStr += Environment.NewLine;

                                    LineIndex = 1;
                                }
                            }
                        }

                        if (Success)
                            Log_This("Verify successful at : " + "0x" + (StartValue + CurrentIndex).ToString("X4"));
                        else
                        {
                            Log_This("Chip NOT SAME AS BUFFER");
                            Log_This("Stopped at : " + "0x" + (ErrorIndex).ToString("X4"));
                            CurrentIndex = LoopIndex;
                            DisableRunning();
                        }

                        CurrentIndex += 256;
                    }
                    else
                    {
                        DisableRunning();
                    }
                }

            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void SerialConnect()
        {
            serial = new SerialPort();

            GetPortName();

            int Index = 0;
            if (AvailablePorts.Count > 0)
            {
                //Loop through all COM Ports until we find the proper one
                while (!SerialConnected & Index < AvailablePorts.Count)
                {
                    //Close before Setting Values
                    if (serial.IsOpen)
                    {
                        serial.Close();
                        serial.Dispose();
                    }

                    //Set Settings
                    serial.PortName = AvailablePorts[Index];
                    serial.BaudRate = 9600;
                    //serial.BaudRate = 115200;
                    serial.ReadTimeout = 300;
                    serial.WriteTimeout = 500;

                    //Connect and Get Version
                    try
                    {
                        serial.Open();
                        Log_This("Connected on " + serial.PortName);
                        GetVersion();

                        //If still not connected, then increase Index
                        if (!SerialConnected)
                            Index++;
                    }
                    catch (Exception)
                    {
                        Log_This("Device NOT RECONIZED on " + serial.PortName);
                        Index++;
                    }
                }
            }
        }

        private void GetVersion()
        {
            //Set Commands
            byte[] Commands = new byte[2];
            Commands[0] = Convert.ToByte('V');
            Commands[1] = Convert.ToByte('V');

            //Send
            Write(Commands);

            //Receive
            byte[] VersionBytes = new byte[3];
            VersionBytes = ReadBytes(3);

            //Check for Available Devices
            if (VersionBytes[0] != 0 & VersionBytes[0] != 255)
            {
                SerialHardware = "Arduino Mega 2560";
                SerialVersion = VersionBytes[0] + "." + VersionBytes[1] + "." + VersionBytes[2];

                SerialConnected = true;
            }
            else
            {
                Log_This("Device NOT RECONIZED on " + serial.PortName);
                serial.Close();
            }
        }

        public void GetPortName()
        {
            AvailablePorts.Clear();

            Log_This("Getting available ports list");
            foreach (string s in SerialPort.GetPortNames())
            {
                AvailablePorts.Add(s);
                Log_This("    " + s + " available");
            }
        }

        /*public void Sleep(int TimeToWait)
        {
            //Log_This("Waiting " + (TimeToWait / 1000) + "s" + " before next Serial Task");
            //Thread.Sleep(TimeToWait);

            while (TimeToWait > 0)
            {
                Thread.Sleep(10);
                TimeToWait -= 10;
            }
        }*/

        private void DisableRunning()
        {
            if (Running)
            {
                progressBar1.Value = 0;
                Running = false;

                //Enable Control Button
                button_SaveBuffer.Enabled = true;
                button_LoadBuffer.Enabled = true;
                button_Blank.Enabled = true;
                //button_Edit.Enabled = true;
                button_Erase.Enabled = true;
                button_Read.Enabled = true;
                button_Verify.Enabled = true;
                button_Write.Enabled = true;

                //Set Textbox Enable
                listBox_Chips.Enabled = true;
                textBox_ChipStart.Enabled = true;
                textBox_ChipEnd.Enabled = true;
                textBox_BufferStart.Enabled = true;
                textBox_BufferEnd.Enabled = true;

                if (ReadOnly)
                {
                    button_Write.Enabled = false;
                    button_Erase.Enabled = false;
                }
            }
        }

        private void Write(byte[] Bytes)
        {
            try
            {
                serial.Write(Bytes, 0, Bytes.Length);
            }
            catch (TimeoutException)
            {
                Log_This("Write Timeout Exception");

                DisableRunning();

                try
                {
                    serial.DiscardInBuffer();
                    serial.DiscardOutBuffer();
                }
                catch
                {
                    Log_This("Emulator SP.Discard failed");
                }
            }
        }

        private byte ReadByte()
        {
            int Timeout = 0;

            try
            {
                //Timeout Loop if bytes is not availables
                while (serial.BytesToRead < 1 & Timeout < 10000)
                {
                    Thread.Sleep(1);
                    Timeout++;
                }

                //Check Timeout is out of time
                if (Timeout >= 10000)
                {
                    DisableRunning();
                    Log_This("Read Timeout Exception");
                    return 0xFF;
                }
                else
                {
                    return (byte)serial.ReadByte();
                }
            }
            catch (TimeoutException)
            {
                DisableRunning();
                Log_This("Read Timeout Exception");
                return 0xFF;
            }
        }

        private byte[] ReadBytes(int size)
        {
            byte[] Temp = new byte[0];

            int Timeout = 0;

            try
            {
                //Timeout Loop if bytes is not availables
                while (serial.BytesToRead < size & Timeout < 10000)
                {
                    Thread.Sleep(1);
                    Timeout++;
                }

                //Check Timeout is out of time
                if (Timeout >= 10000)
                {
                    DisableRunning();
                    Log_This("Read Timeout Exception");
                }
                else
                {
                    Temp = new byte[size];
                    serial.Read(Temp, 0, size);
                }
            }
            catch (TimeoutException)
            {
                DisableRunning();
                Log_This("Read Timeout Exception");
            }
            return Temp;
        }

        //#####################################################################################################################################
        //#####################################################################################################################################
        //#####################################################################################################################################

        private void listBox_Chips_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChipModel = listBox_Chips.SelectedItem.ToString();

            ReadOnly = false;

            Log_This(ChipModel + " Chip Selected");

            //Enable Button since Chip is Selected !
            button_SaveBuffer.Enabled = true;
            button_LoadBuffer.Enabled = true;
            button_Blank.Enabled = true;
            //button_Edit.Enabled = true;
            button_Erase.Enabled = true;
            button_Read.Enabled = true;
            button_Verify.Enabled = true;
            button_Write.Enabled = true;

            //27C256 (Read-Only) or 29C256
            if (listBox_Chips.SelectedItem.ToString() == "27C256 (Read-Only)" | listBox_Chips.SelectedItem.ToString() == "29C256")
            {
                textBox_ChipStart.Text = "0000";
                textBox_ChipEnd.Text = "7FFF";
                ChipSize = 32768;

                //Set Read-Only
                if (listBox_Chips.SelectedItem.ToString() == "27C256 (Read-Only)")
                {
                    ReadOnly = true;
                    button_Write.Enabled = false;
                    button_Erase.Enabled = false;
                }
            }

            //27C512 or 29C512 or 27SF512 (SST)
            if (listBox_Chips.SelectedItem.ToString() == "27C512" | listBox_Chips.SelectedItem.ToString() == "29C512" | listBox_Chips.SelectedItem.ToString() == "27SF512 (SST)")
            {
                textBox_ChipStart.Text = "0000";
                textBox_ChipEnd.Text = "FFFF";
                ChipSize = 32768 * 2;
            }

            //Check with Buffer
            if (GetBufferAddressingLoop() > ChipSize)
            {
                Log_This("ERROR : Buffer is bigger than Chip");
            }
        }

        private void button_LoadBuffer_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Buffer = File.ReadAllBytes(openFileDialog1.FileName);

                //Set Buffer Addressing
                textBox_BufferStart.Text = "0000";
                textBox_BufferEnd.Text = (Buffer.Length - 1).ToString("X4");

                //Reset Chip Addressing wih Buffer Size
                textBox_ChipStart.Text = (ChipSize - Buffer.Length).ToString("X4");

                //Set Loaded
                Log_This("File Loaded : " + Path.GetFileName(openFileDialog1.FileName));
                Log_This("File Size in bytes (Integer/Hex) : " + Buffer.Length + "/0x" + (Buffer.Length).ToString("X4"));
            }
        }

        private void Log_This(string Message)
        {
            textBox_Logs.AppendText(Message);
            textBox_Logs.AppendText(Environment.NewLine);
        }

        private void button_SaveBuffer_Click(object sender, EventArgs e)
        {
            //Open Dialog
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Save
                File.Create(saveFileDialog1.FileName).Dispose();
                File.WriteAllBytes(saveFileDialog1.FileName, Buffer);

                //Set Saved
                Log_This("Saved : " + Path.GetFileName(saveFileDialog1.FileName));
            }
        }

        private int GetChipAddressingLoop()
        {
            int StartValue = int.Parse(textBox_ChipStart.Text, System.Globalization.NumberStyles.HexNumber);
            int EndValue = int.Parse(textBox_ChipEnd.Text, System.Globalization.NumberStyles.HexNumber);

            return EndValue - StartValue;
        }

        private int GetBufferAddressingLoop()
        {
            int StartValue = int.Parse(textBox_BufferStart.Text, System.Globalization.NumberStyles.HexNumber);
            int EndValue = int.Parse(textBox_BufferEnd.Text, System.Globalization.NumberStyles.HexNumber);

            return EndValue - StartValue;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Running = false;

            if (serial.IsOpen)
                serial.Close();

            this.Close();
            Application.Exit();
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Running = false;

            if (serial.IsOpen)
                serial.Close();
        }

        //#############################################################################################################################################
        //#############################################################################################################################################
        //#############################################################################################################################################

        private bool CheckCompatibility()
        {
            bool Compatible = true;

            if (ChipModel == "")
            {
                Log_This("ERROR : Supported Chip NOT SELECTED");
                Compatible = false;
            }
            if (GetBufferAddressingLoop() > ChipSize)
            {
                Log_This("ERROR : Buffer is bigger than Chip");
                Compatible = false;
            }
            if (GetChipAddressingLoop() == 0)
            {
                Log_This("ERROR : Chip addressing is Empty");
                Compatible = false;
            }

            return Compatible;
        }

        private void button_Write_Click(object sender, EventArgs e)
        {
            if (CheckCompatibility())
            {
                LoopIndex = GetBufferAddressingLoop();
                StartValue = int.Parse(textBox_ChipStart.Text, System.Globalization.NumberStyles.HexNumber);

                bool Error = false;

                //Erase SST Before Writing
                if (ChipModel == "27SF512 (SST)")
                {
                    Log_This("Erasing Chip...");
                    byte[] Cmds = new byte[2];
                    Cmds[0] = Convert.ToByte('E');
                    Cmds[1] = 5;
                    Write(Cmds);
                    if (ReadByte() != 79)
                    {
                        Log_This("Chip NOT ERASED");
                        Error = true;
                    }
                    else
                        Log_This("Erase successful");
                }

                //If no errors, then write data
                if (!Error)
                {
                    Log_This("Writing Chip...");

                    //Run Loop
                    Running = true;
                    Mode = "Write";
                    CurrentIndex = 0;
                }
            }
        }

        private void button_Read_Click(object sender, EventArgs e)
        {
            if (CheckCompatibility())
            {
                //Get Chip Addressing (how many bytes to read)
                LoopIndex = GetChipAddressingLoop();
                StartValue = int.Parse(textBox_ChipStart.Text, System.Globalization.NumberStyles.HexNumber);

                //Reset Buffer
                Buffer = new byte[LoopIndex + 1];

                Log_This("Reading Chip...");

                //Run Loop
                Running = true;
                Mode = "Read";
                CurrentIndex = 0;
            }
        }

        private void button_Erase_Click(object sender, EventArgs e)
        {
            if (CheckCompatibility())
            {
                LoopIndex = GetChipAddressingLoop();
                StartValue = int.Parse(textBox_ChipStart.Text, System.Globalization.NumberStyles.HexNumber);

                Log_This("Erasing Chip...");

                //Run Loop
                Running = true;
                Mode = "Erase";
                CurrentIndex = 0;
            }
        }

        private void button_Blank_Click(object sender, EventArgs e)
        {
            if (CheckCompatibility())
            {
                //Get Chip Addressing (how many bytes to read)
                LoopIndex = GetChipAddressingLoop();
                StartValue = int.Parse(textBox_ChipStart.Text, System.Globalization.NumberStyles.HexNumber);

                //Reset Buffer
                Buffer = new byte[LoopIndex];

                Log_This("Blank Check Chip...");

                //Run Loop
                Running = true;
                Mode = "Blank";
                CurrentIndex = 0;
            }
        }

        private void button_Verify_Click(object sender, EventArgs e)
        {
            if (CheckCompatibility())
            {
                //Get Chip Addressing (how many bytes to read)
                LoopIndex = GetChipAddressingLoop();
                StartValue = int.Parse(textBox_ChipStart.Text, System.Globalization.NumberStyles.HexNumber);

                Log_This("Verify Chip...");

                //Run Loop
                Running = true;
                Mode = "Verify";
                CurrentIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Running = false;
            progressBar1.Value = 0;
            button1.Enabled = false;

            //Enable Control Button
            button_SaveBuffer.Enabled = true;
            button_LoadBuffer.Enabled = true;
            button_Blank.Enabled = true;
            //button_Edit.Enabled = true;
            button_Erase.Enabled = true;
            button_Read.Enabled = true;
            button_Verify.Enabled = true;
            button_Write.Enabled = true;

            //Set Textbox Enable
            listBox_Chips.Enabled = true;
            textBox_ChipStart.Enabled = true;
            textBox_ChipEnd.Enabled = true;
            textBox_BufferStart.Enabled = true;
            textBox_BufferEnd.Enabled = true;

            if (ReadOnly)
            {
                button_Write.Enabled = false;
                button_Erase.Enabled = false;
            }
        }

        private void DownloadPage_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.github.com/bouletmarc/");
        }
    }
}
