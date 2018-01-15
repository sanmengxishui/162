using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace JPTCG.AppleTestSW
{
    public class AppleTestSWCom
    {
        TcpClient tcpClient;
        byte[] Data = new byte[500000];

        public delegate void OnDataSendRecevie(string msg);
        public event OnDataSendRecevie OnSendAndRec;
        public string IP = "";
        public int Port = 61804;
        public string Name = "CGClient";
        public string returnCode = "";
        public int errorCode;                  //pass or fail 
        public string errorString = "";         //  error detail info
        public string bin_Code = "";//20170222

        public AppleTestSWCom(string myName)
        {
            tcpClient = new TcpClient();
            IP = "169.254.0.1";//Helper.LocalIPAddress();
            Name = myName;
        }
        ~AppleTestSWCom()
        {
            if (tcpClient.Connected)
                tcpClient.Close();
        }

        public bool IsConnected
        {
            get { return tcpClient.Connected; }
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
                    OnSendAndRec("Connected with sever at " + ip + ":" + PortNo);
                WaitMessageFromSever();
                res = true;
            }
            catch 
            {
                if (OnSendAndRec != null)
                    OnSendAndRec("Fail to Connect with sever at " + ip + ":" + PortNo);
            }
            return res;
        }

        public void Disconnect()
        {
            if (tcpClient.Connected)
            {
                if (OnSendAndRec != null)
                    OnSendAndRec("Disconnected with sever at" + IP);
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
               // Disconnect();
                Console.WriteLine("wait server error");
            }
        }

        object obj = new object();
        string RecievedMsg = "";

        public void ReceiveMessage(IAsyncResult ar)
        {
            //int bufferLength;
            try
            {
                lock (obj)
                {
                    int bufferLength;
                    bufferLength = tcpClient.GetStream().EndRead(ar);
                    if (bufferLength <= 1)
                        return;

                    string message = (System.Text.Encoding.ASCII.GetString(Data, 0, bufferLength)).ToString();
                    RecievedMsg = message;
                    Para.myMain.WriteOperationinformation("Receive "+Name+" : "+message);
                    if (OnSendAndRec != null)
                        OnSendAndRec(IP +" Recieve::"+message);
                    //if (bufferLength != 0)
                    //    RecievedMsg = DecodeRecieveMsg(bufferLength).ToString(); //ZJP
                    returnCode = message.Substring(6, 4);
                    errorCode =  (Data[17] << 8) | Data[16];
                    //Console.WriteLine("error code:" + errorCode);
                    errorString = message.Substring(18,28);
                    Console.WriteLine("length_________"+message.Length);
                    if (returnCode == "dest") 
                    {
                        bin_Code = message.Substring(46, 2);//20170222
                        Console.WriteLine("Bin_Code string:" + bin_Code);

                    }

                    WaitMessageFromSever();
                }

            }
            catch (Exception ex)
            {
              //Disconnect();
                Console.WriteLine("receive msg error");
            }

        }

        public bool WaitTestReply(string MsgCode)
        {
            DateTime sttTime = DateTime.Now;
            returnCode = "";

            while (returnCode != MsgCode)
            {
                TimeSpan timespStr = DateTime.Now - sttTime;
                if (timespStr.TotalMilliseconds < 30000)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                else
                {                    
                    return false; // Timeout
                }
            }

            return true; 
        }
        private bool WaitTestResult(string Code)
        {            
            //while (RecievedMsg == "")
            //{
            //    Thread.Sleep(1000);
            //    Application.DoEvents();
            //}
            DateTime sttTime = DateTime.Now;
            returnCode = "";
            while (RecievedMsg == "")
            {
                TimeSpan timespStr = DateTime.Now - sttTime;
                if (timespStr.TotalMilliseconds < 3000)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                else
                {
                    return false; // Timeout
                }
            }

            return true;
            //return RecievedMsg;
        }

        private byte _MTCP_calculateCheckSum(byte[] buff, int buffLeng)
        {
            byte rtn = 0;
	        for (int i=0;i<buffLeng;i++) 
                rtn += buff[i];
	        
            rtn = (byte)~rtn;
	        return rtn;
        }
        private short DecodeRecieveMsg(int bufferLength)
        {
            byte[] header = new byte[16];
            Array.Copy(Data, 0, header, 0, 16); 
            int pSize = DecodeHeader(header);
            byte[] payLoad = new byte[pSize]; //31 Size
            Array.Copy(Data, 16, payLoad, 0, pSize);
            string pl = System.Text.Encoding.UTF8.GetString(payLoad); //"0", "1" //ZJP "0Fail......."
            Console.WriteLine(pl);
            return BitConverter.ToInt16(payLoad, 0);//
            if (OnSendAndRec != null)
            OnSendAndRec(" Payload::" + pl);
            //byte[] payLoad = 
        }
        private int DecodeHeader(byte[] header)
        {
            string res = "";
            byte[] Err = new byte[2];
            Array.Copy(header, 4, Err, 0, 2);
            int Error = BitConverter.ToInt16(Err, 0);
            if (OnSendAndRec != null)
            OnSendAndRec(" ERR::" + Error.ToString());
            byte[] Ctr = new byte[4];
            Array.Copy(header, 6, Ctr, 0, 4);
            res = System.Text.Encoding.UTF8.GetString(Ctr);
            if (OnSendAndRec != null)
            OnSendAndRec(" CTRL::" + res);
            byte[] PLEN = new byte[4];
            Array.Copy(header, 10, PLEN, 0, 4);
            int Len = BitConverter.ToInt32(PLEN, 0);
            if (OnSendAndRec != null)
            OnSendAndRec(" PLEN::" + Len.ToString());
            return Len;
        }
        public bool SendStartTest(int myCellNum, string mySerial,string LSType, string SpecType)
        {
            bool res = false;
            if (!tcpClient.Connected)
            {
                return false;
            }

            byte[] payload = GetPayLoad(myCellNum, mySerial, LSType, SpecType);
            byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetStartTestHeader(payload.Length, P_CS);

            byte[] Cmd = new byte[payload.Length + header.Length + 1];

            Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            Cmd[Cmd.Length - 1] = P_CS;

            res = SendMessage(Cmd);
            return res;
        }
        public bool SendStartTest11(int myCellNum, string mySerial, string LSType, string SpecType,double Height)
        {
            bool res = false;
            if (!tcpClient.Connected)
            {
                return false;
            }

            byte[] payload = GetPayLoad11(myCellNum, mySerial, LSType, SpecType,Height);
            byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetStartTestHeader(payload.Length, P_CS);

            byte[] Cmd = new byte[payload.Length + header.Length + 1];

            Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            Cmd[Cmd.Length - 1] = P_CS;

            res = SendMessage(Cmd);
            return res;
        }
        public bool SendTestResult(List<double> RefVal, List<double> TestVal1, List<double> TestVal2, List<double> TestVal3, List<double> TestVal4, List<double> TestVal5)
        {
            bool res = false;
            if (!tcpClient.Connected)
            {
                return false;
            }
            //Assume that All List Value is already in 351 Size
            byte[] payload = GetTestPayLoad(RefVal, TestVal1, TestVal2, TestVal3, TestVal4, TestVal5);
            byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetTransTestHeader(payload.Length, P_CS);

            byte[] Cmd = new byte[payload.Length + header.Length + 1];

            Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next byte
            Cmd[Cmd.Length - 1] = P_CS;

            res = SendMessage(Cmd);
            return res;
        }
        public bool SendImage(int imgWidth, int imgHgt, byte[] imgPtr, DPoint XY, double Ang)
        {
            bool res = false;
            //if (!tcpClient.Connected)
            //{
            //    return false;
            //}
            byte[] payload = GetImagePayLoad(imgWidth, imgHgt, imgPtr, imgPtr.Length, 0, XY,Ang);
            byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetImageHeader(payload.Length, P_CS);

            byte[] Cmd = new byte[payload.Length + header.Length + 1];

            Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            Cmd[Cmd.Length - 1] = P_CS;

            res = SendMessage(Cmd);

            return true;

            //int size = 512000;
            //int cnt = (int)((imgWidth * imgHgt) / size);
            //int Remaining = (imgWidth * imgHgt) % size;
            
            //for (int i = 0; i < cnt; i++)
            //{
            //    byte[] newImgPtr = new byte[size];
            //    Buffer.BlockCopy(newImgPtr, 0, imgPtr, (i * size), newImgPtr.Length);
            //    byte[] payload = GetImagePayLoad(imgWidth, imgHgt, newImgPtr, size, i);
            //    byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            //    byte[] header = GetImageHeader(payload.Length, P_CS);

            //    byte[] Cmd = new byte[payload.Length + header.Length + 1];

            //    Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            //    Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            //    Cmd[Cmd.Length - 1] = P_CS;

            //    res = SendMessage(Cmd);
            //    Thread.Sleep(1000);
            //}

            //if (Remaining > 0)
            //{
            //    byte[] newImgPtr = new byte[Remaining];
            //    Buffer.BlockCopy(newImgPtr, 0, imgPtr, (cnt * size), newImgPtr.Length);
            //    byte[] payload = GetImagePayLoad(imgWidth, imgHgt, newImgPtr, Remaining, cnt);
            //    byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            //    byte[] header = GetImageHeader(payload.Length, P_CS);

            //    byte[] Cmd = new byte[payload.Length + header.Length + 1];

            //    Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            //    Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            //    Cmd[Cmd.Length - 1] = P_CS;

            //    res = SendMessage(Cmd);
            //}
            
            //return res;
        }
        public bool SendImageAdvance(int imgWidth, int imgHgt, byte[] imgPtr1, byte[] imgPtr2, byte[] imgPtr3, byte[] imgPtr4, int BGExpTime, int ALSExpTime, float centWL, float pixDensity, float beamSize)
        {
            bool res = false;

            byte[] payload = GetImagePayLoadAdvance(imgWidth, imgHgt, imgPtr1, imgPtr2, imgPtr3, imgPtr4, imgPtr1.Length * 4, 1, BGExpTime, ALSExpTime, centWL, pixDensity, beamSize);
            byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetImageHeader(payload.Length, P_CS);

            byte[] Cmd = new byte[payload.Length + header.Length + 1];

            Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            Cmd[Cmd.Length - 1] = P_CS;

            res = SendMessage(Cmd);

            return true; 
        }
        public bool SendAdvanTestResult(List<float> WLVal, List<float> darkVal, List<float> RefVal, List<float> TestVal1, List<float> TestVal2, List<float> TestVal3, List<float> TestVal4, List<float> TestVal5,
                                        List<DPoint> XY, double Ang)
        {
            bool res = false;
            if (!tcpClient.Connected)
            {
                return false;
            }

            byte[] payload = GetAdvanTestPayLoad(WLVal, darkVal, RefVal, TestVal1, TestVal2, TestVal3, TestVal4, TestVal5,XY, Ang);
            byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetAdvanTransTestHeader(payload.Length, P_CS);

            byte[] Cmd = new byte[payload.Length + header.Length + 1];

            Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            Cmd[Cmd.Length - 1] = P_CS;

            res = SendMessage(Cmd);
            return res;
        }

        public bool SendAdvanTestResultFor3Point(List<float> WLVal, List<float> darkVal, List<float> RefVal, List<float> TestVal1, List<float> TestVal2, List<float> TestVal3, List<DPoint> XY, double Ang)
        {
            bool res = false;
            if (!tcpClient.Connected)
            {
                return false;
            }

            byte[] payload = GetAdvanTestPayLoadFor3Point(WLVal, darkVal, RefVal, TestVal1, TestVal2, TestVal3, XY, Ang);
            byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetAdvanTransTestHeader(payload.Length, P_CS);

            byte[] Cmd = new byte[payload.Length + header.Length + 1];

            Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte
            Buffer.BlockCopy(payload, 0, Cmd, header.Length, payload.Length);// Copy Payload to Next 136 byte
            Cmd[Cmd.Length - 1] = P_CS;

            res = SendMessage(Cmd);
            return res;
        }

        //ZJP
        public int SendEndTest(string strmg)
        {
            int res = -1;
            if (!tcpClient.Connected)
            {
                return -1;
            }

            //byte[] payload = GetPayLoad(myCellNum, mySerial);
            //byte P_CS = _MTCP_calculateCheckSum(payload, payload.Length);
            byte[] header = GetEndTestHeader();

            //byte[] Cmd = new byte[header.Length];

            //Buffer.BlockCopy(header, 0, Cmd, 0, header.Length);// Copy Header to first 16 byte            
            //Cmd[Cmd.Length - 1] = P_CS;

            RecievedMsg = "";
            if (!SendMessage(header))
                return -1;

            //string testRes = WaitTestResult();
            
            //if (testRes == "0")
            //    res = true;
            //else
            //    res = false;

            //20160929

            if (!WaitTestReply("dest"))
            {
                Para.myMain.WriteOperationinformation("Recieve dest Reply Timeout ");
                return -1;
            }

            //if (!WaitTestResult("suibian"))
            //{
            //    Para.myMain.WriteOperationinformation("Recieved End Test Timeout");
            //    return -1;
            //}
            //string TEMP000 = testRes.Substring(4, 2);
            if (errorCode == 0)
                res = 0;          
            else
            {
                res = 1;
                Para.myMain.WriteOperationinformation(errorString);
            }
            //20160929

            return res;
        }

     

        private byte[] GetAdvanTestPayloadPoint(int tstPt, List<float> WLVal, List<float> darkVal,List<float> RefVal,List<float> MVal, DPoint XY, float Ang)
        {
            byte[] data_atst = new byte[17];
            data_atst[0] = (byte)tstPt;
            byte[] dataCnt = BitConverter.GetBytes(WLVal.Count);
            Buffer.BlockCopy(dataCnt, 0, data_atst, 1, dataCnt.Length);
            byte[] X_POS_OFFSET = BitConverter.GetBytes((float)XY.X);
            Buffer.BlockCopy(X_POS_OFFSET, 0, data_atst, 5, X_POS_OFFSET.Length);
            byte[] Y_POS_OFFSET = BitConverter.GetBytes((float)XY.Y);
            Buffer.BlockCopy(Y_POS_OFFSET, 0, data_atst, 9, Y_POS_OFFSET.Length);
            byte[] A_POS_OFFSET = BitConverter.GetBytes(Ang);
            Buffer.BlockCopy(A_POS_OFFSET, 0, data_atst, 13, A_POS_OFFSET.Length);

            byte[] point1 = new byte[16 * WLVal.Count];
            for (int i = 0; i < WLVal.Count; i++)
            {
                byte[] Val = BitConverter.GetBytes(WLVal[i]);
                Buffer.BlockCopy(Val, 0, point1, (i * 16), Val.Length); //WaveLength
                Val = BitConverter.GetBytes(darkVal[i]);
                Buffer.BlockCopy(Val, 0, point1, 4 + (i * 16), Val.Length);//Dark
                Val = BitConverter.GetBytes(RefVal[i]);
                Buffer.BlockCopy(Val, 0, point1, 8 + (i * 16), Val.Length);//White
                Val = BitConverter.GetBytes(MVal[i]);
                Buffer.BlockCopy(Val, 0, point1, 12 + (i * 16), Val.Length);//Measure
            }
            byte[] payload = new byte[data_atst.Length + point1.Length];
            Buffer.BlockCopy(data_atst, 0, payload, 0, data_atst.Length);
            Buffer.BlockCopy(point1, 0, payload, (data_atst.Length), point1.Length);
            return payload;
        }
        private byte[] GetImagePayLoad(int width, int height, byte[] imgPtr, int size, int order, DPoint XY, double Ang)
        {
            byte[] tMTCP_payload_UNIF_REQ = new byte[21];
            short sVal = (short)width;
            byte[] Val = BitConverter.GetBytes(sVal);
            Buffer.BlockCopy(Val, 0, tMTCP_payload_UNIF_REQ, 0, Val.Length);
            sVal = (short)height;
            Val = BitConverter.GetBytes(sVal);
            Buffer.BlockCopy(Val, 0, tMTCP_payload_UNIF_REQ, 2, Val.Length);
            tMTCP_payload_UNIF_REQ[4] = (byte)order;
            Val = BitConverter.GetBytes(size);
            Buffer.BlockCopy(Val, 0, tMTCP_payload_UNIF_REQ, 5, Val.Length);

            byte[] X_POS_OFFSET = BitConverter.GetBytes((float)XY.X);
            Buffer.BlockCopy(X_POS_OFFSET, 0, tMTCP_payload_UNIF_REQ, 9, X_POS_OFFSET.Length);
            byte[] Y_POS_OFFSET = BitConverter.GetBytes((float)XY.Y);
            Buffer.BlockCopy(Y_POS_OFFSET, 0, tMTCP_payload_UNIF_REQ, 13, Y_POS_OFFSET.Length);
            byte[] A_POS_OFFSET = BitConverter.GetBytes((float)Ang);
            Buffer.BlockCopy(A_POS_OFFSET, 0, tMTCP_payload_UNIF_REQ, 17, A_POS_OFFSET.Length);

            byte[] payload = new byte[(tMTCP_payload_UNIF_REQ.Length) + imgPtr.Length];
            Buffer.BlockCopy(tMTCP_payload_UNIF_REQ, 0, payload, 0, tMTCP_payload_UNIF_REQ.Length);// Copy Point 1
            Buffer.BlockCopy(imgPtr, 0, payload, tMTCP_payload_UNIF_REQ.Length, imgPtr.Length);// Copy Point 2
            return payload;
        }
        private byte[] GetImagePayLoadAdvance(int width, int height, byte[] imgPtr1, byte[] imgPtr2, byte[] imgPtr3, byte[] imgPtr4, int size, int order, int BGExp, int ALSExp, float centWL0, float pixDensity0, float beamSize0)
        {
            byte[] tMTCP_payload_UNIF_REQ = new byte[25];//From 13 to 25
            short sVal = (short)width;
            byte[] Val = BitConverter.GetBytes(sVal);
            Buffer.BlockCopy(Val, 0, tMTCP_payload_UNIF_REQ, 0, Val.Length);
            sVal = (short)height;
            Val = BitConverter.GetBytes(sVal);
            Buffer.BlockCopy(Val, 0, tMTCP_payload_UNIF_REQ, 2, Val.Length);
            tMTCP_payload_UNIF_REQ[4] = (byte)order;
            Val = BitConverter.GetBytes(size);
            Buffer.BlockCopy(Val, 0, tMTCP_payload_UNIF_REQ, 5, Val.Length);

            sVal = (short)BGExp;
            byte[] EXP_BACKGROUND = BitConverter.GetBytes(sVal);
            Buffer.BlockCopy(EXP_BACKGROUND, 0, tMTCP_payload_UNIF_REQ, 9, EXP_BACKGROUND.Length);
            sVal = (short)ALSExp;
            byte[] EXP_APERTURE = BitConverter.GetBytes(sVal);
            Buffer.BlockCopy(EXP_APERTURE, 0, tMTCP_payload_UNIF_REQ, 11, EXP_APERTURE.Length);
            //2016121713@Brando
            byte[] CENTROID_WL = BitConverter.GetBytes(centWL0);
            Buffer.BlockCopy(CENTROID_WL, 0, tMTCP_payload_UNIF_REQ, 13, CENTROID_WL.Length);

            byte[] PXL_DENSITY = BitConverter.GetBytes(pixDensity0);
            Buffer.BlockCopy(PXL_DENSITY, 0, tMTCP_payload_UNIF_REQ, 17, PXL_DENSITY.Length);

            byte[] BEAM_SIZE_UM = BitConverter.GetBytes(beamSize0);
            Buffer.BlockCopy(BEAM_SIZE_UM, 0, tMTCP_payload_UNIF_REQ, 21, BEAM_SIZE_UM.Length);
            //2016121713@Brando

            byte[] payload = new byte[(tMTCP_payload_UNIF_REQ.Length) + imgPtr1.Length + imgPtr2.Length + imgPtr3.Length + imgPtr4.Length];
            Buffer.BlockCopy(tMTCP_payload_UNIF_REQ, 0, payload, 0, tMTCP_payload_UNIF_REQ.Length);//
            Buffer.BlockCopy(imgPtr1, 0, payload, tMTCP_payload_UNIF_REQ.Length, imgPtr1.Length);// Image1
            Buffer.BlockCopy(imgPtr2, 0, payload, tMTCP_payload_UNIF_REQ.Length + imgPtr1.Length, imgPtr2.Length);// Image2
            Buffer.BlockCopy(imgPtr3, 0, payload, tMTCP_payload_UNIF_REQ.Length + imgPtr1.Length + imgPtr2.Length, imgPtr3.Length);// Image3
            Buffer.BlockCopy(imgPtr4, 0, payload, tMTCP_payload_UNIF_REQ.Length + imgPtr1.Length + imgPtr2.Length + imgPtr3.Length, imgPtr4.Length);// Image4
            return payload;
        }
        private byte[] GetAdvanTestPayLoad(List<float> WLVal, List<float> darkVal, List<float> RefVal, List<float> TestVal1, List<float> TestVal2, List<float> TestVal3, List<float> TestVal4, 
                                            List<float> TestVal5, List<DPoint> XY, double Ang)
        {
            byte[] pt1PL;
            byte[] pt2PL;
            byte[] pt3PL;
            byte[] pt4PL;
            byte[] pt5PL;

            if (Para.SampleShape == 0)
            {
                //Need to reverse the points. Rectangle
                pt1PL = GetAdvanTestPayloadPoint(1, WLVal, darkVal, RefVal, TestVal5, XY[4], (float)Ang);
                pt2PL = GetAdvanTestPayloadPoint(2, WLVal, darkVal, RefVal, TestVal4, XY[3], (float)Ang);
                pt3PL = GetAdvanTestPayloadPoint(3, WLVal, darkVal, RefVal, TestVal3, XY[2], (float)Ang);
                pt4PL = GetAdvanTestPayloadPoint(4, WLVal, darkVal, RefVal, TestVal2, XY[1], (float)Ang);
                pt5PL = GetAdvanTestPayloadPoint(5, WLVal, darkVal, RefVal, TestVal1, XY[0], (float)Ang);
            }
            else
            {
                //Need to reverse the points. Circle
                pt1PL = GetAdvanTestPayloadPoint(1, WLVal, darkVal, RefVal, TestVal1, XY[0], (float)Ang);
                pt2PL = GetAdvanTestPayloadPoint(2, WLVal, darkVal, RefVal, TestVal2, XY[1], (float)Ang);
                pt3PL = GetAdvanTestPayloadPoint(3, WLVal, darkVal, RefVal, TestVal3, XY[2], (float)Ang);
                pt4PL = GetAdvanTestPayloadPoint(4, WLVal, darkVal, RefVal, TestVal4, XY[3], (float)Ang);
                pt5PL = GetAdvanTestPayloadPoint(5, WLVal, darkVal, RefVal, TestVal5, XY[4], (float)Ang);
            }

            ////Need to reverse the points.
            //byte[] pt1PL = GetAdvanTestPayloadPoint(1, WLVal, darkVal, RefVal, TestVal5, XY[4], (float)Ang);
            //byte[] pt2PL = GetAdvanTestPayloadPoint(2, WLVal, darkVal, RefVal, TestVal4, XY[3], (float)Ang);
            //byte[] pt3PL = GetAdvanTestPayloadPoint(3, WLVal, darkVal, RefVal, TestVal3, XY[2], (float)Ang);
            //byte[] pt4PL = GetAdvanTestPayloadPoint(4, WLVal, darkVal, RefVal, TestVal2, XY[1], (float)Ang);
            //byte[] pt5PL = GetAdvanTestPayloadPoint(5, WLVal, darkVal, RefVal, TestVal1, XY[0], (float)Ang);

            //byte[] pt1PL = GetAdvanTestPayloadPoint(1, WLVal, darkVal, RefVal, TestVal1, XY[0], (float)Ang);
            //byte[] pt2PL = GetAdvanTestPayloadPoint(2, WLVal, darkVal, RefVal, TestVal2, XY[1], (float)Ang);
            //byte[] pt3PL = GetAdvanTestPayloadPoint(3, WLVal, darkVal, RefVal, TestVal3, XY[2], (float)Ang);
            //byte[] pt4PL = GetAdvanTestPayloadPoint(4, WLVal, darkVal, RefVal, TestVal4, XY[3], (float)Ang);
            //byte[] pt5PL = GetAdvanTestPayloadPoint(5, WLVal, darkVal, RefVal, TestVal5, XY[4], (float)Ang);

            byte[] payload = new byte[(pt1PL.Length * 5) + 1];
            payload[0] = (byte)5;
            Buffer.BlockCopy(pt1PL, 0, payload, 1 + (pt1PL.Length * 0), pt1PL.Length);// Copy Point 1
            Buffer.BlockCopy(pt2PL, 0, payload, 1 + (pt1PL.Length * 1), pt1PL.Length);// Copy Point 2
            Buffer.BlockCopy(pt3PL, 0, payload, 1 + (pt1PL.Length * 2), pt1PL.Length);// Copy Point 3
            Buffer.BlockCopy(pt4PL, 0, payload, 1 + (pt1PL.Length * 3), pt1PL.Length);// Copy Point 4
            Buffer.BlockCopy(pt5PL, 0, payload, 1 + (pt1PL.Length * 4), pt1PL.Length);// Copy Point 5

            return payload;

            ////Point 2
            //byte[] point2 = new byte[8 * WLVal.Count];
            //for (int i = 0; i < WLVal.Count; i++)
            //{
            //    short sVal = (short)WLVal[i];
            //    byte[] Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, (i * 2), Val.Length); //WaveLength
            //    sVal = (short)darkVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 2 + (i * 2), Val.Length);//Dark
            //    sVal = (short)RefVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 4 + (i * 2), Val.Length);//White
            //    sVal = (short)TestVal2[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 6 + (i * 2), Val.Length);//Measure
            //}

            ////Point 3
            //byte[] point3 = new byte[8 * WLVal.Count];
            //for (int i = 0; i < WLVal.Count; i++)
            //{
            //    short sVal = (short)WLVal[i];
            //    byte[] Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, (i * 2), Val.Length); //WaveLength
            //    sVal = (short)darkVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 2 + (i * 2), Val.Length);//Dark
            //    sVal = (short)RefVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 4 + (i * 2), Val.Length);//White
            //    sVal = (short)TestVal3[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 6 + (i * 2), Val.Length);//Measure
            //}

            ////Point 4
            //byte[] point4 = new byte[8 * WLVal.Count];
            //for (int i = 0; i < WLVal.Count; i++)
            //{
            //    short sVal = (short)WLVal[i];
            //    byte[] Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, (i * 2), Val.Length); //WaveLength
            //    sVal = (short)darkVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 2 + (i * 2), Val.Length);//Dark
            //    sVal = (short)RefVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 4 + (i * 2), Val.Length);//White
            //    sVal = (short)TestVal4[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 6 + (i * 2), Val.Length);//Measure
            //}

            ////Point 5
            //byte[] point5 = new byte[8 * WLVal.Count];
            //for (int i = 0; i < WLVal.Count; i++)
            //{
            //    short sVal = (short)WLVal[i];
            //    byte[] Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, (i * 2), Val.Length); //WaveLength
            //    sVal = (short)darkVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 2 + (i * 2), Val.Length);//Dark
            //    sVal = (short)RefVal[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 4 + (i * 2), Val.Length);//White
            //    sVal = (short)TestVal5[i];
            //    Val = BitConverter.GetBytes(sVal);
            //    Buffer.BlockCopy(Val, 0, point1, 6 + (i * 2), Val.Length);//Measure
            //}

            //byte[] payload = new byte[(point5.Length * 5) + 1];
            //payload[0] = (byte)5;
            //Buffer.BlockCopy(point1, 0, payload, 1 + (point1.Length * 0), point1.Length);// Copy Point 1
            //Buffer.BlockCopy(point2, 0, payload, 1 + (point1.Length * 1), point2.Length);// Copy Point 2
            //Buffer.BlockCopy(point3, 0, payload, 1 + (point1.Length * 2), point3.Length);// Copy Point 3
            //Buffer.BlockCopy(point4, 0, payload, 1 + (point1.Length * 3), point4.Length);// Copy Point 4
            //Buffer.BlockCopy(point5, 0, payload, 1 + (point1.Length * 4), point5.Length);// Copy Point 5

            //return payload;
        }

        private byte[] GetAdvanTestPayLoadFor3Point(List<float> WLVal, List<float> darkVal, List<float> RefVal, List<float> TestVal1, List<float> TestVal2, List<float> TestVal3,  List<DPoint> XY, double Ang)
        {
            byte[] pt1PL;
            byte[] pt2PL;
            byte[] pt3PL;

            if (Para.SampleShape == 0)
            {
                //Need to reverse the points. Rectangle
                pt1PL = GetAdvanTestPayloadPoint(1, WLVal, darkVal, RefVal, TestVal3, XY[3], (float)Ang);
                pt2PL = GetAdvanTestPayloadPoint(2, WLVal, darkVal, RefVal, TestVal2, XY[2], (float)Ang);
                pt3PL = GetAdvanTestPayloadPoint(3, WLVal, darkVal, RefVal, TestVal1, XY[1], (float)Ang);
            }
            else
            {
                //Need to reverse the points. Circle
                pt1PL = GetAdvanTestPayloadPoint(1, WLVal, darkVal, RefVal, TestVal1, XY[3], (float)Ang);
                pt2PL = GetAdvanTestPayloadPoint(2, WLVal, darkVal, RefVal, TestVal2, XY[2], (float)Ang);
                pt3PL = GetAdvanTestPayloadPoint(3, WLVal, darkVal, RefVal, TestVal3, XY[1], (float)Ang);
            }

            byte[] payload = new byte[(pt1PL.Length * 3) + 1];
            payload[0] = (byte)3;
            Buffer.BlockCopy(pt1PL, 0, payload, 1 + (pt1PL.Length * 0), pt1PL.Length);// Copy Point 1
            Buffer.BlockCopy(pt2PL, 0, payload, 1 + (pt1PL.Length * 1), pt1PL.Length);// Copy Point 2
            Buffer.BlockCopy(pt3PL, 0, payload, 1 + (pt1PL.Length * 2), pt1PL.Length);// Copy Point 3

            return payload;
        }

        private byte[] GetTestPayLoad(List<double> RefVal, List<double> TestVal1, List<double> TestVal2, List<double> TestVal3, List<double> TestVal4, List<double> TestVal5)
        {
            byte[] payload = new byte[4212];

            //Ref Val
            for (int i = 0; i < RefVal.Count; i++)
            {
                short sVal = (short)RefVal[i];
                byte[] Val = BitConverter.GetBytes(sVal);
                Buffer.BlockCopy(Val, 0, payload, (i * 2), Val.Length);
            }

            //Test Val 1
            for (int i = 0; i < TestVal1.Count; i++)
            {
                short sVal = (short)TestVal1[i];
                byte[] Val = BitConverter.GetBytes(sVal);
                Buffer.BlockCopy(Val, 0, payload, 702 + (i * 2), Val.Length);
            }

            //Test Val 2
            for (int i = 0; i < TestVal2.Count; i++)
            {
                short sVal = (short)TestVal2[i];
                byte[] Val = BitConverter.GetBytes(sVal);
                Buffer.BlockCopy(Val, 0, payload, 1404 + (i * 2), Val.Length);
            }

            //Test Val 3
            for (int i = 0; i < TestVal3.Count; i++)
            {
                short sVal = (short)TestVal3[i];
                byte[] Val = BitConverter.GetBytes(sVal);
                Buffer.BlockCopy(Val, 0, payload, 2106 + (i * 2), Val.Length);
            }

            //Test Val 4
            for (int i = 0; i < TestVal4.Count; i++)
            {
                short sVal = (short)TestVal4[i];
                byte[] Val = BitConverter.GetBytes(sVal);
                Buffer.BlockCopy(Val, 0, payload, 2808 + (i * 2), Val.Length);
            }

            //Test Val 5
            for (int i = 0; i < TestVal5.Count; i++)
            {
                short sVal = (short)TestVal5[i];
                byte[] Val = BitConverter.GetBytes(sVal);
                Buffer.BlockCopy(Val, 0, payload, 3510 + (i * 2), Val.Length);
            }

            return payload;
        }

        private byte[] GetPayLoad(int cellNum, string unitSerial, string LSType, string SpecType)
        {
            byte[] payload = new byte[232];

            string testerName = Para.MchName;
            byte[] TSTC_NAME = System.Text.Encoding.ASCII.GetBytes(testerName);
            byte TSTC_ID = (byte)cellNum;
            string swVersion = Para.SWVersion;
            byte[] SW_VER = System.Text.Encoding.ASCII.GetBytes(swVersion);
            string hwVersion = Para.HWVersion;
            byte[] HW_VER = System.Text.Encoding.ASCII.GetBytes(hwVersion);
            byte[] DUT_SN = System.Text.Encoding.ASCII.GetBytes(unitSerial);
            byte[] LIGHT_SOURCE_ID = System.Text.Encoding.ASCII.GetBytes(LSType);
            byte[] SPECTROMETER_ID = System.Text.Encoding.ASCII.GetBytes(SpecType);

            Buffer.BlockCopy(TSTC_NAME, 0, payload, 0, TSTC_NAME.Length);// Copy TSTC_NAME to first 32 byte
            payload[32] = TSTC_ID;// Copy TSTC_ID to Next byte
            Buffer.BlockCopy(SW_VER, 0, payload, 33, SW_VER.Length);// Copy SW_VER to Next 32 byte
            Buffer.BlockCopy(HW_VER, 0, payload, 65, HW_VER.Length);// Copy HW_VER to Next 32 byte
            Buffer.BlockCopy(DUT_SN, 0, payload, 97, DUT_SN.Length);// Copy DUT_SN to Next 64 byte
            Buffer.BlockCopy(LIGHT_SOURCE_ID, 0, payload, 161, LIGHT_SOURCE_ID.Length);// Copy LIGHT_SOURCE_ID to Next 32 byte
            Buffer.BlockCopy(SPECTROMETER_ID, 0, payload, 193, SPECTROMETER_ID.Length);// Copy SPECTROMETER_ID to Next 32 byte

            return payload;
        }

        private byte[] GetPayLoad11(int cellNum, string unitSerial, string LSType, string SpecType, double height)
        {
            byte[] payload = new byte[232 + 8];

            string testerName = Para.MchName;
            byte[] TSTC_NAME = System.Text.Encoding.ASCII.GetBytes(testerName);

            byte TSTC_ID = (byte)cellNum;

            string swVersion = Para.SWVersion;
            byte[] SW_VER = System.Text.Encoding.ASCII.GetBytes(swVersion);

            string hwVersion = Para.HWVersion;
            byte[] HW_VER = System.Text.Encoding.ASCII.GetBytes(hwVersion);

            byte[] DUT_SN = System.Text.Encoding.ASCII.GetBytes(unitSerial);

            byte[] LIGHT_SOURCE_ID = System.Text.Encoding.ASCII.GetBytes(LSType);

            byte[] SPECTROMETER_ID = System.Text.Encoding.ASCII.GetBytes(SpecType);

            byte[] HEIGHT_ID = BitConverter.GetBytes((float)height);
            byte[] CYCLETIME_ID = BitConverter.GetBytes((float)9.0);

            Buffer.BlockCopy(TSTC_NAME, 0, payload, 0, TSTC_NAME.Length);// Copy TSTC_NAME to first 32 byte

            payload[32] = TSTC_ID;// Copy TSTC_ID to Next byte
            Buffer.BlockCopy(SW_VER, 0, payload, 33, SW_VER.Length);// Copy SW_VER to Next 32 byte
            Buffer.BlockCopy(HW_VER, 0, payload, 65, HW_VER.Length);// Copy HW_VER to Next 32 byte
            Buffer.BlockCopy(DUT_SN, 0, payload, 97, DUT_SN.Length);// Copy DUT_SN to Next 64 byte
            Buffer.BlockCopy(LIGHT_SOURCE_ID, 0, payload, 161, LIGHT_SOURCE_ID.Length);// Copy LIGHT_SOURCE_ID to Next 32 byte
            Buffer.BlockCopy(SPECTROMETER_ID, 0, payload, 193, SPECTROMETER_ID.Length);// Copy SPECTROMETER_ID to Next 32 byte

            Buffer.BlockCopy(HEIGHT_ID, 0, payload, 232, HEIGHT_ID.Length);// Copy LIGHT_SOURCE_ID to Next 32 byte
            Buffer.BlockCopy(CYCLETIME_ID, 0, payload, 232 + 4, CYCLETIME_ID.Length);// Copy SPECTROMETER_ID to Next 32 byte     

            return payload;
        }
        private byte[] GetStartTestHeader(int payloadLen,byte P_CS)
        {
            string MTCP_PREAMBLE = "PCTM";// "MTCP";
            byte[] MTCP = System.Text.Encoding.ASCII.GetBytes(MTCP_PREAMBLE) ;//  MTCP_PREAMBLE //4byte
            byte[] ERRC = BitConverter.GetBytes(0);   //  valid only for backward packet, tMTCP_ERR code (defined below) 2 byte
            byte[] CTRL = System.Text.Encoding.ASCII.GetBytes("rcst");//System.Text.Encoding.ASCII.GetBytes("tscr");    //  packet control id (defined below)
            byte[] PLEN = BitConverter.GetBytes((uint)payloadLen+1);   //  System.Text.Encoding.ASCII.GetBytes("136");// +P_CS;//
	        byte    SEQN = (byte)0;   //  forward packet sequence number
            //byte H_CS = _MTCP_calculateCheckSum(;   //  header checksum, 1's complement of sum of Byte0~6

            byte[] header = new byte[16];

            Buffer.BlockCopy(MTCP, 0, header, 0, MTCP.Length);// Copy MTCP to first 4 byte
            Buffer.BlockCopy(ERRC, 0, header, 4, ERRC.Length);// Copy ERRC to Next 2 byte
            Buffer.BlockCopy(CTRL, 0, header, 6, CTRL.Length);// Copy CTRL to Next 4 byte
            Buffer.BlockCopy(PLEN, 0, header, 10, PLEN.Length);// Copy PLEN to Next 4 byte
            //header[13] = P_CS;
            header[14] = SEQN;
            header[15] = _MTCP_calculateCheckSum(header, header.Length);

            var str = System.Text.Encoding.Default.GetString(header);
            if (OnSendAndRec != null)
            OnSendAndRec(IP + " Header::" + str);

            return header;
        }
        private byte[] GetImageHeader(int payloadLen, byte P_CS)
        {
            string MTCP_PREAMBLE = "PCTM";// "MTCP";
            byte[] MTCP = System.Text.Encoding.ASCII.GetBytes(MTCP_PREAMBLE);//  MTCP_PREAMBLE //4byte
            byte[] ERRC = BitConverter.GetBytes(0);   //  valid only for backward packet, tMTCP_ERR code (defined below) 2 byte
            byte[] CTRL = System.Text.Encoding.ASCII.GetBytes("finu");//unif
            byte[] PLEN = BitConverter.GetBytes((uint)payloadLen + 1);   //  System.Text.Encoding.ASCII.GetBytes("136");// +P_CS;//
            byte SEQN = (byte)0;   //  forward packet sequence number
            //byte H_CS = _MTCP_calculateCheckSum(;   //  header checksum, 1's complement of sum of Byte0~6

            byte[] header = new byte[16];

            Buffer.BlockCopy(MTCP, 0, header, 0, MTCP.Length);// Copy MTCP to first 4 byte
            Buffer.BlockCopy(ERRC, 0, header, 4, ERRC.Length);// Copy ERRC to Next 2 byte
            Buffer.BlockCopy(CTRL, 0, header, 6, CTRL.Length);// Copy CTRL to Next 4 byte
            Buffer.BlockCopy(PLEN, 0, header, 10, PLEN.Length);// Copy PLEN to Next 4 byte
            //header[13] = P_CS;
            header[14] = SEQN;
            header[15] = _MTCP_calculateCheckSum(header, header.Length);

            var str = System.Text.Encoding.Default.GetString(header);
            //OnSendAndRec(IP + " Header::" + str);

            return header;
        }
        private byte[] GetAdvanTransTestHeader(int payloadLen, byte P_CS)
        {
            string MTCP_PREAMBLE = "PCTM";// "MTCP";
            byte[] MTCP = System.Text.Encoding.ASCII.GetBytes(MTCP_PREAMBLE);//  MTCP_PREAMBLE //4byte
            byte[] ERRC = BitConverter.GetBytes(0);   //  valid only for backward packet, tMTCP_ERR code (defined below) 2 byte
            byte[] CTRL = System.Text.Encoding.ASCII.GetBytes("tsta");
            byte[] PLEN = BitConverter.GetBytes((uint)payloadLen + 1);   //  System.Text.Encoding.ASCII.GetBytes("136");// +P_CS;//
            byte SEQN = (byte)0;   //  forward packet sequence number
            //byte H_CS = _MTCP_calculateCheckSum(;   //  header checksum, 1's complement of sum of Byte0~6

            byte[] header = new byte[16];

            Buffer.BlockCopy(MTCP, 0, header, 0, MTCP.Length);// Copy MTCP to first 4 byte
            Buffer.BlockCopy(ERRC, 0, header, 4, ERRC.Length);// Copy ERRC to Next 2 byte
            Buffer.BlockCopy(CTRL, 0, header, 6, CTRL.Length);// Copy CTRL to Next 4 byte
            Buffer.BlockCopy(PLEN, 0, header, 10, PLEN.Length);// Copy PLEN to Next 4 byte
            //header[13] = P_CS;
            header[14] = SEQN;
            header[15] = _MTCP_calculateCheckSum(header, header.Length);

            var str = System.Text.Encoding.Default.GetString(header);
            if (OnSendAndRec != null)
            OnSendAndRec(IP + " Header::" + str);

            return header;
        }
        private byte[] GetTransTestHeader(int payloadLen, byte P_CS)
        {
            string MTCP_PREAMBLE = "PCTM";// "MTCP";
            byte[] MTCP = System.Text.Encoding.ASCII.GetBytes(MTCP_PREAMBLE) ;//  MTCP_PREAMBLE //4byte
            byte[] ERRC = BitConverter.GetBytes(0);   //  valid only for backward packet, tMTCP_ERR code (defined below) 2 byte
            byte[] CTRL = System.Text.Encoding.ASCII.GetBytes("ttst");//System.Text.Encoding.ASCII.GetBytes("tscr");    //  packet control id (defined below)
            byte[] PLEN = BitConverter.GetBytes((uint)payloadLen+1);   //  System.Text.Encoding.ASCII.GetBytes("136");// +P_CS;//
	        byte    SEQN = (byte)0;   //  forward packet sequence number
            //byte H_CS = _MTCP_calculateCheckSum(;   //  header checksum, 1's complement of sum of Byte0~6

            byte[] header = new byte[16];

            Buffer.BlockCopy(MTCP, 0, header, 0, MTCP.Length);// Copy MTCP to first 4 byte
            Buffer.BlockCopy(ERRC, 0, header, 4, ERRC.Length);// Copy ERRC to Next 2 byte
            Buffer.BlockCopy(CTRL, 0, header, 6, CTRL.Length);// Copy CTRL to Next 4 byte
            Buffer.BlockCopy(PLEN, 0, header, 10, PLEN.Length);// Copy PLEN to Next 4 byte
            //header[13] = P_CS;
            header[14] = SEQN;
            header[15] = _MTCP_calculateCheckSum(header, header.Length);

            var str = System.Text.Encoding.Default.GetString(header);
            if (OnSendAndRec != null)
            OnSendAndRec(IP + " Header::" + str);

            return header;
        }
        private byte[] GetEndTestHeader()
        {
            string MTCP_PREAMBLE = "PCTM";
            byte[] MTCP = System.Text.Encoding.ASCII.GetBytes(MTCP_PREAMBLE);//  MTCP_PREAMBLE //4byte
            byte[] ERRC = BitConverter.GetBytes(0);   //  valid only for backward packet, tMTCP_ERR code (defined below) 2 byte
            byte[] CTRL = System.Text.Encoding.ASCII.GetBytes("dest");    //  packet control id (defined below)
            byte[] PLEN = BitConverter.GetBytes((uint)0);   //  payload length (if any), CTRL specific
            byte SEQN = (byte)0;   //  forward packet sequence number
            //byte H_CS = _MTCP_calculateCheckSum(;   //  header checksum, 1's complement of sum of Byte0~6

            byte[] header = new byte[16];

            Buffer.BlockCopy(MTCP, 0, header, 0, MTCP.Length);// Copy MTCP to first 4 byte
            Buffer.BlockCopy(ERRC, 0, header, 4, ERRC.Length);// Copy ERRC to Next 2 byte
            Buffer.BlockCopy(CTRL, 0, header, 6, CTRL.Length);// Copy ERRC to Next 4 byte
            Buffer.BlockCopy(PLEN, 0, header, 10, PLEN.Length);// Copy PLEN to Next 4 byte
            header[14] = SEQN;
            header[15] = _MTCP_calculateCheckSum(header, header.Length);
            Console.WriteLine("GetEndTestHeader:"+header.ToString());
            Console.WriteLine("byte Array:");
            for (int i = 0; i <= 15; i++)
            {
                Console.WriteLine(header[i].ToString());             
            }
            return header;
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

        public void SendMessage(string message)
        {            
            try
            {
                NetworkStream netStr = tcpClient.GetStream();
                byte[] Data = System.Text.Encoding.ASCII.GetBytes(message);
                netStr.Write(Data, 0, Data.Length);
                netStr.Flush();
                if (OnSendAndRec != null)
                    OnSendAndRec(IP + " Send::" + message);
                //AddToLog(message, true, DateTime.Now);
            }

            catch (Exception ex)
            {
                //AssignError(ex);
            }

        }

        public void ShowSettings()
        {
            WinAServer myWin = new WinAServer(this);
            myWin.Show();
        }
    }
}
