using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.BarcodeScanner
{
    public partial class WinBarcode : Form
    {
        BarcodeMgr myMgr;
        public WinBarcode(BarcodeMgr mgr)
        {
            InitializeComponent();
            myMgr = mgr;
        }

        private void WinBarcode_Load(object sender, EventArgs e)
        {
            ModLB.Items.Clear();
            for (int i = 0; i < myMgr.barcodeList.Count; i++)
                ModLB.Items.Add(myMgr.barcodeList[i].Name);
            if (ModLB.Items.Count != 0)
            {
                ModLB.SelectedIndex = 0;
                UpdateUI(0);
            }
        }

        private void UpdateUI(int BarIdx)
        {
            ip1TB.Text = myMgr.barcodeList[BarIdx].IP.ToString();
            
            cmdTB.Text = myMgr.barcodeList[BarIdx].Port.ToString();

            //myMgr.barcodeList[BarIdx].OnSendAndRec += AddMsg;
        }

        private void AddMsg(string msg)
        {
            if (msgLB.InvokeRequired)
            {
                msgLB.Invoke(
                    new Action(() =>
                    {
                        msgLB.Items.Add(msg);
                    })

                    );
            }
            else
            {
                msgLB.Items.Add(msg);
            }
        }
        private void WinBarcode_FormClosing(object sender, FormClosingEventArgs e)
        {
            myMgr.SaveSettings(Para.MchConfigFileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ModLB.Items.Count == 0)
                return;

            int BarIdx = ModLB.SelectedIndex;

            myMgr.barcodeList[BarIdx].IP = ip1TB.Text;
            myMgr.barcodeList[BarIdx].Port = int.Parse(cmdTB.Text);

            //myMgr.barcodeList[BarIdx].Connect(myMgr.barcodeList[BarIdx].IP, myMgr.barcodeList[BarIdx].Port);
            if (!myMgr.barcodeList[BarIdx].Connect(myMgr.barcodeList[BarIdx].IP, myMgr.barcodeList[BarIdx].Port))
                msgLB.Items.Add(myMgr.barcodeList[BarIdx].Name + " Failed to connect.");
            else
                msgLB.Items.Add(myMgr.barcodeList[BarIdx].Name + " Connected.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ModLB.Items.Count == 0)
                return;

            int BarIdx = ModLB.SelectedIndex;

            myMgr.barcodeList[BarIdx].Disconnect();
            msgLB.Items.Add(myMgr.barcodeList[BarIdx].Name + " Disconected.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ModLB.Items.Count == 0)
                return;

            int BarIdx = ModLB.SelectedIndex;

            if (!myMgr.barcodeList[BarIdx].IsConnected)
                msgLB.Items.Add(myMgr.barcodeList[BarIdx].Name + " Is Not Connected.");

            //myMgr.barcodeList[BarIdx].Read();

            string barcode = myMgr.barcodeList[BarIdx].Read();
            if (barcode == "")
                msgLB.Items.Add(myMgr.barcodeList[BarIdx].Name + " No Barcode Readed.");
            else
                msgLB.Items.Add(myMgr.barcodeList[BarIdx].Name + ">>" + barcode);

        }

        private void ModLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI(ModLB.SelectedIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ModLB.Items.Count == 0)
                return;

            int BarIdx = ModLB.SelectedIndex;

            if (!myMgr.barcodeList[BarIdx].IsConnected)
                msgLB.Items.Add(myMgr.barcodeList[BarIdx].Name + " Is Not Connected.");

            myMgr.barcodeList[BarIdx].LOFF();
        }

        private void msgLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(msgLB.Text);
        }
    }
}
