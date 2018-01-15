using Common;
using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keyences
{
    public class KeyenceDLRS1A
    {

        public String RecivedMsg = "";
        private SerialPort port = null;
        public string MyPortName = "COM1";
        public string MyName = "DLRS1A";
        public double OriginValue = 0;
        public double Uplimited = 0.05;
        public double Lowlimited = 0.05;
        public bool Enable = false;

        public KeyenceDLRS1A(string myName)
        {
            MyName = myName; 
        }
        ~KeyenceDLRS1A()
        {
            Disconnect();
        }
        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //String str = port.ReadExisting();
                String str = port.ReadLine();

                RecivedMsg = str;
                //port.
            }
            catch (Exception)
            {

            }
        }

        public bool Connect(string PortName)
        {
            try
            {
                MyPortName = PortName;
                port = new SerialPort();
                port.PortName = PortName;//"COM" + PortNum.ToString();
                port.BaudRate = 9600;
                port.StopBits = System.IO.Ports.StopBits.One;
                port.DataBits = 8;
                port.Parity = System.IO.Ports.Parity.None;
                port.Handshake = Handshake.None;
                port.DataReceived += port_DataReceived;

                if (port.IsOpen)
                    port.Close();
                Thread.Sleep(5);
                port.Open();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void  Disconnect()
        {
            try
            {
                if (port != null &&
                    port.IsOpen)
                {
                    port.Close();
                    port = null;
                }
            }
            catch (Exception ex)
            {
                return;
            }           
        }

        private string GetReply(int timeout_seconds)
        {
            RecivedMsg = "";

            bool bTimeout = false;
            DateTime st = DateTime.Now;
            TimeSpan ts = DateTime.Now - st;
            while (!bTimeout)
            {
                if (RecivedMsg != "")
                    break;
                ts = DateTime.Now - st;
                bTimeout = ts.Seconds >= timeout_seconds;
                Thread.Sleep(5);
            }
            return RecivedMsg;
        }

        private byte[] GetByteArray(string hex)
        {

            string[] ssArray = hex.Split(' ');
            List<byte> bytList = new List<byte>();
            foreach (var s in ssArray)
            {
                //将十六进制的字符串转换成数值
                bytList.Add(Convert.ToByte(s, 16));
            }
            //返回字节数组
            return bytList.ToArray();
        }

        public bool IsConnected()
        {
            if (port == null)
                return false;

            return port.IsOpen;
        }

        public double Read()
        {
            if (!port.IsOpen)
                return 0;
            //Read All data
            string data = "4D 30 0D 0A";
            byte[] hex = GetByteArray(data);
            //port.DiscardInBuffer();
            //port.DiscardOutBuffer();

           
            double tempDou = 0;

            for (int i =0 ;i <3;i++)
            {
                port.Write(hex, 0, hex.Length);
                Thread.Sleep(5*i);

                string tempStr = GetReply(1);
                string tempStr1 = "";               
                try
                {
                    tempStr1 = tempStr.Substring(3);
                    tempDou = double.Parse(tempStr1);
                }
                catch (Exception e)
                {
                    tempDou = 0;
                }

                if (tempDou != 0)
                    break;
                
            }

            return tempDou;
        }

        public void SaveSettings(string fileName)
        {
            string Header = "";

            Header = "DLRS_" + MyName;

            FileOperation.SaveData(fileName, Header, "ComPort", MyPortName);

            FileOperation.SaveData(fileName, Header, "OriginValue", OriginValue.ToString());

            FileOperation.SaveData(fileName, Header, "Uplimted", Uplimited.ToString());

            FileOperation.SaveData(fileName, Header, "Lowlimted", Lowlimited.ToString());

            FileOperation.SaveData(fileName, Header, "Enable", Enable.ToString());
            
        }
        public void LoadSettings(string fileName)
        {
            string Header = "";
            string strread = "";
            Header = "DLRS_" + MyName;

            FileOperation.ReadData(fileName, Header, "ComPort", ref strread);
            if (strread != "0")
                MyPortName = strread;
             else
            {
                FileOperation.SaveData(fileName, Header, "ComPort", MyPortName);
            }

            //if (Para.MachineOnline)
            //{
            //    if (!Connect(MyPortName))
            //    {
            //        MessageBox.Show("DLRS " + MyName + " can't be connected.");//20170222
            //    }
            //}

            FileOperation.ReadData(fileName, Header, "OriginValue", ref strread);
            if (strread != "0")
                OriginValue = double.Parse(strread);
            else
            {
                FileOperation.SaveData(fileName, Header, "OriginValue", OriginValue.ToString());
            }

            FileOperation.ReadData(fileName, Header, "Uplimted", ref strread);
            if (strread != "0")
                Uplimited = double.Parse(strread);
            else
            {
                FileOperation.SaveData(fileName, Header, "Uplimted", Uplimited.ToString());
            }

            FileOperation.ReadData(fileName, Header, "Lowlimted", ref strread);
            if (strread != "0")
                Lowlimited = double.Parse(strread);
            else
            {
                FileOperation.SaveData(fileName, Header, "Lowlimted", Lowlimited.ToString());
            }

            FileOperation.ReadData(fileName, Header, "Enable", ref strread);
            if (strread != "0")
                Enable = bool.Parse(strread);
            else
            {
                FileOperation.SaveData(fileName, Header, "Enable", Enable.ToString());
            }

            if (Para.MachineOnline)
            {
                if (Enable)
                {
                    if (!Connect(MyPortName))
                    {
                        MessageBox.Show("DLRS " + MyName + " can't be connected.");//20170222
                    }
                }
            }
        }
    }
}
