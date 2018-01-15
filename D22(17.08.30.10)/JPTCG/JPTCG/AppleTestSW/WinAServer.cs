using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.AppleTestSW
{
    public partial class WinAServer : Form
    {
        AppleTestSWCom AMgr;
        public WinAServer(AppleTestSWCom myMgr)
        {
            InitializeComponent();
            AMgr = myMgr;
            AMgr.OnSendAndRec += AddMsg;

            Text = "CGClient Version 2.0 <<" +myMgr.Name +">>";
        }

        private void AddMsg(string msg)
        {
            if (MsgLB.InvokeRequired)
            {
                MsgLB.Invoke(
                    new Action(() =>
                    {
                        MsgLB.Items.Add("<<" + DateTime.Now.ToString("HH:mm:ss.fff") + ">>" + msg);
                    })

                    );
            }
            else
            {
                MsgLB.Items.Add("<<" + DateTime.Now.ToString("HH:mm:ss.fff") + ">>" + msg);
            }
        }

        private void WinAServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            AMgr.OnSendAndRec -= AddMsg;
        }

        private void WinAServer_Load(object sender, EventArgs e)
        {
            IPTB.Text = AMgr.IP;
            portTB.Text = AMgr.Port.ToString();
            MsgLB.Items.Clear();

            if (AMgr.IsConnected)
            {
                ConnectBtn.Enabled = false;
                DisconnectBtn.Enabled = true;
            }
            else
            {
                ConnectBtn.Enabled = true;
                DisconnectBtn.Enabled = false;
            }
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            string myIp = IPTB.Text;
            int myPort = int.Parse(portTB.Text);
            if (AMgr.Connect(myIp,myPort))
            {
                ConnectBtn.Enabled = false;
                DisconnectBtn.Enabled = true;
            }
            else
            {
                ConnectBtn.Enabled = true;
                DisconnectBtn.Enabled = false;
            }
        }

        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            AMgr.Disconnect();
            ConnectBtn.Enabled = true;
            DisconnectBtn.Enabled = false;
        }

        private void StartTestBtn_Click(object sender, EventArgs e)
        {
            AMgr.SendStartTest(1, "SerialTest01","LS","CAS140");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AMgr.SendEndTest("Button");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //AMgr.TestBC();
            List<double> refVal = new List<double>();
            List<double> Val1 = new List<double>();
            List<double> Val2 = new List<double>();
            List<double> Val3 = new List<double>();
            List<double> Val4 = new List<double>();
            List<double> Val5 = new List<double>();
            for (int i = 0; i < 350; i++)
            {
                refVal.Add(i/10);
                Val1.Add(i/10);
                Val2.Add(i/10);
                Val3.Add(i/10);
                Val4.Add(i/10);
                Val5.Add(i/10);
            }
            //AMgr.SendTestResult(refVal, Val1, Val2, Val3, Val4, Val5);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //typedef struct tMTCP_data_ATST{
            //uint8_t				POINT_IDX;		// 0 for reference light
            //uint32_t			DATA_CNT;		// Data count for Spectrometer reading
            //float				X_POS_OFFSET;	// adjustment in X axis, in mm
            //float				Y_POS_OFFSET;   // adjustment in Y axis, in mm
            //float				A_POS_OFFSET;   // adjustment in Z angle, in degree
            //   } tMTCP_data_ATST;  
            //AMgr.TestBC();
            List<float> darkVal = new List<float>();
            List<float> refVal = new List<float>();
            List<float> Val1 = new List<float>();
            List<float> Val2 = new List<float>();
            List<float> Val3 = new List<float>();
            List<float> Val4 = new List<float>();
            List<float> Val5 = new List<float>();
            for (int i = 0; i < 700; i++)
            {
                darkVal.Add(i);
                refVal.Add(i*20);
                Val1.Add(i * 10);
                Val2.Add(i * 10);
                Val3.Add(i * 10);
                Val4.Add(i * 10);
                Val5.Add(i * 10);
            }
            List<float> wlVal = new List<float>();
            for (int i = 400; i < 1100; i++)
            {
                wlVal.Add(i);
            }
            //AMgr.SendAdvanTestResult(wlVal,darkVal, refVal, Val1, Val2, Val3, Val4, Val5);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open Image file";
            openFileDialog1.ShowHelp = true;
            openFileDialog1.Filter = "(*.gif)|*.gif|(*.jpg)|*.jpg|(*.JPEG)|*.JPEG|(*.bmp)|*.bmp|(*.png)|*.png|All files (*.*)|*.*";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string strHeadImagePath = openFileDialog1.FileName;
                    Bitmap myImg = new Bitmap(strHeadImagePath);
                    byte[] imgPtr = JPTCG.AppleTestSW.WinAServer.ReadImage(strHeadImagePath);
                    AMgr.SendImage(myImg.Width, myImg.Height, imgPtr, new Common.DPoint(0,0),0);
                }
                catch
                {
                    MessageBox.Show("format not correct");
                }
            }
        }

        private static byte[] ReadImage(string imgPath)
        {
            byte[] imageData = null;
            FileInfo fileIn = new FileInfo(imgPath);
            long imgLen = fileIn.Length;
            FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imgLen);
            return imageData;

        }        
    }
}
