using Common;
using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.BarcodeScanner
{
    public class KeyenceBarcode
    {
        public delegate void OnDataSendRecevie(string msg);
        public event OnDataSendRecevie OnSendAndRec;
        
        private TcpClient tcpClient;
        byte[] Data = new byte[500];

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //public int CommandPort = 9003; // 9003 for command
        public int Port = 9004; // 9004 for data
        public string IP = "192.168.100.100 ";
        //public bool IsConnected = false;
        public bool IsConnected
        {
            get { return tcpClient.Connected; }
        }
        public KeyenceBarcode(string myName)
        {
            name = myName;
            tcpClient = new TcpClient();
        }
        ~KeyenceBarcode()
        {
            if (tcpClient.Connected)
                tcpClient.Close();
        }
        public void SaveSettings(string fileName)
        {
            string Header = "";            
                
            Header = "Barcode" + Name;
            FileOperation.SaveData(fileName, Header, "IP", IP);
            //FileOperation.SaveData(fileName, Header, "CommandPort", CommandPort.ToString());
            FileOperation.SaveData(fileName, Header, "Port", Port.ToString());
        }
        public void LoadSettings(string fileName)
        {
            string Header = "";
            string strread = "";
           
           Header = "Barcode" + Name;

           FileOperation.ReadData(fileName, Header, "IP", ref strread);
           IP = strread;
           
           //FileOperation.ReadData(fileName, Header, "CommandPort", ref strread);
           //CommandPort = int.Parse(strread);

           FileOperation.ReadData(fileName, Header, "Port", ref strread);
           Port = int.Parse(strread);

           if (Para.MachineOnline)
           {
               if (!Connect(IP, Port))
                   MessageBox.Show(Name + " can't be connected.");
           }

        }

        public bool Connect(string ip, int PortNo)
        {
            bool res = false;
            try
            {
                Disconnect();
                IP = ip;
                Port = PortNo;
                tcpClient = new TcpClient();
                tcpClient.Connect(ip, PortNo);
                if (OnSendAndRec != null)
                    OnSendAndRec("Connected with Barcode at " + ip + ":" + PortNo);
                WaitMessageFromSever();
                res = true;        
            }
            catch
            {
                if (OnSendAndRec != null)
                    OnSendAndRec("Fail to Connect with Barcode at " + ip + ":" + PortNo);
            }
            return res;
        }
        public void Disconnect()
        {
            if (tcpClient.Connected)
            {
                if (OnSendAndRec != null)
                    OnSendAndRec("Disconnected with Barcode at" + IP);
                tcpClient.Close();
            }
        }

        void WaitMessageFromSever()
        {
            //ResetError();
            try
            {
                tcpClient.GetStream().BeginRead(Data, 0, Data.Length, ReceiveMessage, null);
            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }
        string barcode = "";
        object obj = new object();
        public void ReceiveMessage(IAsyncResult ar)
        {

            int bufferLength;

            try
            {
                lock (obj)
                {

                    bufferLength = tcpClient.GetStream().EndRead(ar);
                    barcode = (System.Text.Encoding.ASCII.GetString(Data, 0, bufferLength)).ToString();
                    if (OnSendAndRec != null)
                        OnSendAndRec(IP + " Recieve::" + barcode);                    
                    WaitMessageFromSever();
                }

            }
            catch (Exception ex)
            {
                Disconnect();
            }

        }
        public bool SendMessage(Byte[] msg)
        {
            try
            {
                NetworkStream netStr = tcpClient.GetStream();
                netStr.Write(msg, 0, msg.Length);
                netStr.Flush();
                if (OnSendAndRec != null)
                {
                    var str = System.Text.Encoding.Default.GetString(msg);
                    OnSendAndRec(IP + " Send::" + str);
                }
                return true;
                //AddToLog(message, true, DateTime.Now);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void LON()
        {
            string lon = "LON\r";   // CR is terminator
            Byte[] command = ASCIIEncoding.ASCII.GetBytes(lon);
            SendMessage(command);

        }
        public void LOFF()
        {
            string loff = "LOFF\r"; // CR is terminator
            Byte[] command = ASCIIEncoding.ASCII.GetBytes(loff);
            SendMessage(command);      
        }

        public string Read()
        {            
            barcode = "";
            LON();
            DateTime st_time = DateTime.Now;
            TimeSpan time_span;
            while (barcode == "")
            {
                Thread.Sleep(10);
                Application.DoEvents();
                time_span = DateTime.Now - st_time;
                if (time_span.TotalMilliseconds > 2000)
                {
                    break;
                }
            }
            LOFF();
            if (barcode != "")
                return barcode.Replace("\r","");//.Substring(0, barcode.IndexOf(':'));
            else
                return "";
        }
    }
}
