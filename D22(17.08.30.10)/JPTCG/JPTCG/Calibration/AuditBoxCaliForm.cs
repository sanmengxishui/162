using JPTCG.Common;
using JPTCG.Motion;
using JPTCG.Spectrometer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HalconDotNet;
using Common;

namespace JPTCG.Calibration
{
    public partial class AuditBoxForm : Form
    {
        
        SpectManager specMgr;
        DeltaMotionMgr motionMgr; //17
        string[] WLStr = new string[] { "400", "410", "420", "450", "500", "550", "600", "650", "700", "750", "800", "850", "900", "950", "1000", "1050", "1100" };

        System.Windows.Forms.Label[] mod1IndividualTestLabel;
        System.Windows.Forms.Label[] mod2IndividualTestLabel;
        System.Windows.Forms.Label[] mod1GroupTestLabel;
        System.Windows.Forms.Label[] mod2GroupTestLabel;
        string[] mod1NDFilterTestedID;
        string[] mod2NDFilterTestedID;
        bool[] mod1NDFilterTestedFail;
        bool[] mod2NDFilterTestedFail;

        bool[] mod1NDFilterTestedResult;
        bool[] mod2NDFilterTestedResult;

        bool[] LSResultFailed1 = new bool[3];
        bool[] LSResultFailed2 = new bool[3]; 
        


        public AuditBoxForm(SpectManager MySpecMgr, DeltaMotionMgr MyMotionMgr)
        {
            InitializeComponent();
            specMgr = MySpecMgr;
            motionMgr = MyMotionMgr;

        }
        /// <summary>
        /// ////////////Larry For AuditBox 20170117
        /// </summary>
        List<float> whiteRef1 = new List<float>();
        List<float> darkRef1 = new List<float>();
        List<float> measVal1 = new List<float>();
        List<float> TransVal1 = new List<float>();
        List<float> whiteRef2 = new List<float>();
        List<float> darkRef2 = new List<float>();
        List<float> measVal2 = new List<float>();
        List<float> TransVal2 = new List<float>();
        //List<float> wl = new List<float>();
        /// ////////


        private void CaliForm_Load(object sender, EventArgs e)
        {
            //barcodeform = new BarcodeInputForm();
            mod1IndividualTestLabel = new Label[] { label3, label5, label6, label9, label10, label11, label12, label13, label14 };
            mod2IndividualTestLabel = new Label[] { label15, label16, label17, label18, label19, label20, label21, label22, label23 };
            mod1GroupTestLabel = new Label[] { label31, label32, label33 };
            mod2GroupTestLabel = new Label[] { label34, label35, label36 };


            IndividualResultGB.Visible = false;
            GrouResultGB.Visible = false;
            mod1NDFilterTestedID = new string[mod1IndividualTestLabel.Length];
            mod2NDFilterTestedID = new string[mod2IndividualTestLabel.Length];
            mod1NDFilterTestedFail = new bool[mod1IndividualTestLabel.Length];
            mod2NDFilterTestedFail = new bool[mod2IndividualTestLabel.Length];
            mod1NDFilterTestedResult = new bool[mod1IndividualTestLabel.Length];
            mod2NDFilterTestedResult = new bool[mod2IndividualTestLabel.Length];

            InitIndividualTestResult();
            comboBox1.SelectedIndex = 0;

            radioButton2.Enabled = false;
            radioButton1.Checked = true;
            comboBox2.Visible = false;
            label24.Visible = false;
            label27.Visible = false;

            ClearComboBoxListview();
            LoadNDFilter();
            LoadGUSerial();
            InitGS1LV();
            InitGS2LV();
            InitIndividualTestResult();
            //InitZAxis();//20170303@Brando
        }

        private void LoadGUSerial()
        {
            GS1SerialCB.Items.Clear();
            GS2SerialCB.Items.Clear();
            //BarcodeInputForm.comboBox1.Items.Clear();
            for (int i = 0; i < myData.Count; i++)
            {
                GS1SerialCB.Items.Add(myData[i].Serial);
                GS2SerialCB.Items.Add(myData[i].Serial);
            }
        }
        private void InitGS2LV()
        {
            GS2LV.Clear();
            GS2LV.Items.Clear();
            GS2LV.Columns.Add("", 20);
            GS2LV.Columns.Add("wavelength", 70);
            GS2LV.Columns.Add("Point", 50);
            GS2LV.Columns.Add("Max", 50);
            GS2LV.Columns.Add("Min", 50);
            GS2LV.Columns.Add("Nominal", 50);
            GS2LV.Columns.Add("Meas", 50);
            GS2LV.Columns.Add("Pass/Fail", 80);
            this.GS2LV.View = System.Windows.Forms.View.Details;
        }
        private void LoadGS2UI()
        {
            int idx = GS2SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            InitGS2LV();
            for (int i = 0; i < myData[idx].TransRatio.Count; i++)
            {
                ListViewItem item = GS2LV.Items.Add(GS2LV.Items.Count + "");
                if (wlIdx >= WLStr.Length)
                {
                    wlIdx = 0;
                    ptCnt++;
                }
                item.SubItems.Add(WLStr[wlIdx]);
                wlIdx++;
                item.SubItems.Add(ptCnt.ToString());
                item.SubItems.Add(mySpec[i].UpperLimit.ToString("F2"));
                item.SubItems.Add(mySpec[i].LowerLimit.ToString("F2"));
                item.SubItems.Add(myData[idx].TransRatio[i].ToString("F3"));
                item.EnsureVisible();
            }
        }
        private void InitGS1LV()
        {
            GS1LV.Clear();
            GS1LV.Items.Clear();
            GS1LV.Columns.Add("", 20);
            GS1LV.Columns.Add("wavelength", 70);
            GS1LV.Columns.Add("Point", 50);
            GS1LV.Columns.Add("Max", 50);
            GS1LV.Columns.Add("Min", 50);
            GS1LV.Columns.Add("Nominal", 50);
            GS1LV.Columns.Add("Meas", 50);
            GS1LV.Columns.Add("Pass/Fail", 80);
            this.GS1LV.View = System.Windows.Forms.View.Details;
        }
        private void LoadGS1UI()
        {
            int idx = GS1SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            InitGS1LV();

            for (int i = 0; i < myData[idx].TransRatio.Count; i++)
            {
                ListViewItem item = GS1LV.Items.Add(GS1LV.Items.Count + "");
                if (wlIdx >= WLStr.Length)
                {
                    wlIdx = 0;
                    ptCnt++;
                }
                item.SubItems.Add(WLStr[wlIdx]);
                wlIdx++;
                item.SubItems.Add(ptCnt.ToString());
                item.SubItems.Add(mySpec[i].UpperLimit.ToString("F2"));
                item.SubItems.Add(mySpec[i].LowerLimit.ToString("F2"));
                item.SubItems.Add(myData[idx].TransRatio[i].ToString("F3"));
                //item.SubItems[1].BackColor = Color.Red;                                              
                item.EnsureVisible();

                // GS1LV.Items[GS1LV.Items.Count - 1].SubItems[1].BackColor = Color.Red;
            }
        }
        private void LoadGS1UIAndResult()
        {
            int idx = GS1SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            bool IsPass = true;
            InitGS1LV();
            for (int i = 0; i < myData[idx].TransRatio.Count; i++)
            {
                ListViewItem item = GS1LV.Items.Add(GS1LV.Items.Count + "");
                if (wlIdx >= WLStr.Length)
                {
                    wlIdx = 0;
                    ptCnt++;
                }
                item.SubItems.Add(WLStr[wlIdx]);
                wlIdx++;
                item.SubItems.Add(ptCnt.ToString());
                item.SubItems.Add(mySpec[i].UpperLimit.ToString("F2"));
                item.SubItems.Add(mySpec[i].LowerLimit.ToString("F2"));
                item.SubItems.Add(myData[idx].TransRatio[i].ToString("F3"));
                item.SubItems.Add(myData[idx].MeasTransRatio[i].ToString("F3"));
                item.UseItemStyleForSubItems = false;

                float diff = ((myData[idx].MeasTransRatio[i] - myData[idx].TransRatio[i]));
                if ((diff < -mySpec[i].LowerLimit) ||
                    (diff > mySpec[i].UpperLimit))
                {
                    item.SubItems.Add("Fail");
                    item.SubItems[7].BackColor = Color.Red;
                    IsPass = false;
                }
                else
                {
                    item.SubItems.Add("Pass");
                    item.SubItems[7].BackColor = Color.Blue;
                }
                item.EnsureVisible();
            }

            if (IsPass)
            {
                GS1ResLbl.Text = "Pass";
                GS1ResLbl.ForeColor = Color.Blue;
            }
            else
            {
                GS1ResLbl.Text = "Fail";
                GS1ResLbl.ForeColor = Color.Red;
            }
        }
        private void LoadGS2UIAndResult()
        {
            int idx = GS2SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            bool IsPass = true;

            InitGS2LV();
            for (int i = 0; i < myData[idx].TransRatio.Count; i++)
            {
                ListViewItem item = GS2LV.Items.Add(GS2LV.Items.Count + "");
                if (wlIdx >= WLStr.Length)
                {
                    wlIdx = 0;
                    ptCnt++;
                }
                item.SubItems.Add(WLStr[wlIdx]);
                wlIdx++;
                item.SubItems.Add(ptCnt.ToString());
                item.SubItems.Add(mySpec[i].UpperLimit.ToString("F2"));
                item.SubItems.Add(mySpec[i].LowerLimit.ToString("F2"));
                item.SubItems.Add(myData[idx].TransRatio[i].ToString("F3"));
                item.SubItems.Add(myData[idx].MeasTransRatio[i].ToString("F3"));
                item.UseItemStyleForSubItems = false;

                float diff = ((myData[idx].MeasTransRatio[i] - myData[idx].TransRatio[i]));
                if ((diff < -mySpec[i].LowerLimit) ||
                    (diff > mySpec[i].UpperLimit))

                //if (((myData[idx].MeasTransRatio[i] - myData[idx].TransRatio[i]) < mySpec[i].LowerLimit) ||
                //    ((myData[idx].MeasTransRatio[i] - myData[idx].TransRatio[i]) > mySpec[i].UpperLimit))
                {
                    item.SubItems.Add("Fail");
                    item.SubItems[7].BackColor = Color.Red;
                    IsPass = false;
                }
                else
                {
                    item.SubItems.Add("Pass");
                    item.SubItems[7].BackColor = Color.Blue;
                }
                item.EnsureVisible();
            }

            if (IsPass)
            {
                GS2ResLbl.Text = "Pass";
                GS2ResLbl.ForeColor = Color.Blue;
            }
            else
            {
                GS2ResLbl.Text = "Fail";
                GS2ResLbl.ForeColor = Color.Red;
            }
        }
        private void CalNDFResult()
        {
            int idx = GS1SerialCB.SelectedIndex;
            int s = 0;
            int t = 0;
            float TranRatio = 0;
            myData[idx].MeasTransRatio.Clear();
            for (int i = 0; i < 17; i++) //Point 1
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[0][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            idx = GS2SerialCB.SelectedIndex;
            s = 0;
            t = 0;
            myData[idx].MeasTransRatio.Clear();
            for (int i = 0; i < 17; i++) //Point 1
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[0][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
        }
        private void CalResult()
        {
            int idx = GS1SerialCB.SelectedIndex;

            int s = 0;
            int t = 0;
            float TranRatio = 0;
            myData[idx].MeasTransRatio.Clear();
            for (int i = 0; i < 17; i++) //Point 1
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[0][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++) //Point 2
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[1][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 3
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[2][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 4
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[3][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 5
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[4][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }

            idx = GS2SerialCB.SelectedIndex;
            s = 0;
            t = 0;
            myData[idx].MeasTransRatio.Clear();
            for (int i = 0; i < 17; i++) //Point 1
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[0][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++) //Point 2
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[1][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 3
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[2][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 4
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[3][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 5
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[4][t];
                myData[idx].MeasTransRatio.Add(TranRatio);
            }
        }
        private void DisplayResult()
        {
            InitGS1LV();
            InitGS2LV();
            CalResult();
            LoadGS1UIAndResult();
            LoadGS2UIAndResult();
        }
        private void DisplayNDFResult()
        {
            InitGS1LV();
            InitGS2LV();
            //CalNDFResult();
            LoadNDF1UIAndResult();
            LoadNDF2UIAndResult();
        }
        private void LoadNDF1UI()
        {
            int idx = GS1SerialCB.SelectedIndex;
            InitGS1LV();

            if (idx >= 0)
            {
                for (int j = 0; j < NDInfo1[idx].data.Count; j++)
                {
                    ListViewItem item = GS1LV.Items.Add(GS1LV.Items.Count + "");
                    item.SubItems.Add(NDInfo1[idx].data[j].waveLength.ToString());
                    item.SubItems.Add("");
                    item.SubItems.Add(NDInfo1[idx].data[j].max.ToString("F2"));
                    item.SubItems.Add(NDInfo1[idx].data[j].min.ToString("F2"));
                    item.SubItems.Add(NDInfo1[idx].data[j].Nominal.ToString("F3"));
                    item.EnsureVisible();
                }
            }
        }
        private void LoadNDF1UIAndResult()
        {
            int idx = GS1SerialCB.SelectedIndex;
            bool failed = false;
            InitGS1LV();
            if (idx >= 0)
            {
                for (int j = 0; j < NDInfo1[idx].data.Count; j++)
                {
                    ListViewItem item = GS1LV.Items.Add(GS1LV.Items.Count + "");
                    item.SubItems.Add(NDInfo1[idx].data[j].waveLength.ToString());
                    item.SubItems.Add("");
                    item.SubItems.Add(NDInfo1[idx].data[j].max.ToString("F2"));
                    item.SubItems.Add(NDInfo1[idx].data[j].min.ToString("F2"));
                    item.SubItems.Add(NDInfo1[idx].data[j].Nominal.ToString("F3"));
                    item.SubItems.Add(NDInfo1[idx].data[j].Measured.ToString("F3"));
                    item.SubItems.Add(NDInfo1[idx].data[j].result);
                    if (NDInfo1[idx].data[j].result == "Fail")
                        failed = true;
                    item.EnsureVisible();
                }
                mod1NDFilterTestedResult[currentTestSeq] = !failed;      
                mod1NDFilterTestedFail[currentTestSeq] = failed;
                //if (failed)
                //{
                //    GS1ResLbl.Text = "Fail";
                //    GS1ResLbl.ForeColor = Color.Red;
                //}
                //else
                //{
                //    GS1ResLbl.Text = "Pass";
                //    GS1ResLbl.ForeColor = Color.Lime;
                //}
            }
        }
        private void LoadNDF2UI()
        {
            int idx = GS2SerialCB.SelectedIndex;
            InitGS2LV();
            if (idx >= 0)
            {
                for (int j = 0; j < NDInfo2[idx].data.Count; j++)
                {
                    ListViewItem item = GS2LV.Items.Add(GS2LV.Items.Count + "");
                    item.SubItems.Add(NDInfo2[idx].data[j].waveLength.ToString());
                    item.SubItems.Add("");
                    item.SubItems.Add(NDInfo2[idx].data[j].max.ToString("F2"));
                    item.SubItems.Add(NDInfo2[idx].data[j].min.ToString("F2"));
                    item.SubItems.Add(NDInfo2[idx].data[j].Nominal.ToString("F3"));
                    item.EnsureVisible();
                }
            }
        }
        private void LoadNDF2UIAndResult()
        {
            int idx = GS2SerialCB.SelectedIndex;
            bool failed = false;
            InitGS2LV();
            if (idx >= 0)
            {
                for (int j = 0; j < NDInfo2[idx].data.Count; j++)
                {
                    ListViewItem item = GS2LV.Items.Add(GS2LV.Items.Count + "");
                    item.SubItems.Add(NDInfo2[idx].data[j].waveLength.ToString());
                    item.SubItems.Add("");
                    item.SubItems.Add(NDInfo2[idx].data[j].max.ToString("F2"));
                    item.SubItems.Add(NDInfo2[idx].data[j].min.ToString("F2"));
                    item.SubItems.Add(NDInfo2[idx].data[j].Nominal.ToString("F3"));
                    item.SubItems.Add(NDInfo2[idx].data[j].Measured.ToString("F3"));
                    item.SubItems.Add(NDInfo2[idx].data[j].result);
                    if (NDInfo2[idx].data[j].result == "Fail")
                        failed = true;
                    item.EnsureVisible();
                }
                mod2NDFilterTestedResult[currentTestSeq] = !failed;  
                mod2NDFilterTestedFail[currentTestSeq] = failed;
                //if (failed)
                //{
                //    GS2ResLbl.Text = "Fail";
                //    GS2ResLbl.ForeColor = Color.Red;
                //}
                //else
                //{
                //    GS2ResLbl.Text = "Pass";
                //    GS2ResLbl.ForeColor = Color.Lime;
                //}
            }
        }
        List<NDFileterInfo> NDInfo1 = new List<NDFileterInfo>();
        List<NDFileterInfo> NDInfo2 = new List<NDFileterInfo>();
        private void LoadNDFilter()
        {
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fileName = CalibrationFolder + "ND Filter T% reference data.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("ND Filter File Not Found." + fileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            myData.Clear();
            NDInfo1.Clear();
            NDInfo2.Clear();
            StreamReader sr = new StreamReader(fileName);
            string line;
            string[] row;
            int Rowcnt = 0;
            while ((line = sr.ReadLine()) != null)
            {
                row = line.Split(',');
                if (row[0] == "")
                    continue;
                int count = (row.Length - 1) / 3;

                if (Rowcnt == 0)
                {
                    myData.Clear();
                    NDFileterInfo[] filterInfo1 = new NDFileterInfo[count];
                    NDFileterInfo[] filterInfo2 = new NDFileterInfo[count];
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            filterInfo1[i] = new NDFileterInfo();
                            filterInfo2[i] = new NDFileterInfo();
                            filterInfo1[i].Name = row[1 + i * 3];
                            filterInfo2[i].Name = row[1 + i * 3];
                            NDInfo1.Add(filterInfo1[i]);
                            NDInfo2.Add(filterInfo2[i]);
                            CalibrationData tmpData = new CalibrationData();
                            tmpData.Serial = row[1 + i * 3];
                            myData.Add(tmpData);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Some data are wrong in " + fileName);
                    }
                    Rowcnt++;
                    continue;
                }

                try
                {
                    CalibrationInfo[] info = new CalibrationInfo[count];
                    int waveLength = int.Parse(row[0]);
                    for (int i = 0; i < count; i++)
                    {
                        info[i] = new CalibrationInfo();
                        info[i].waveLength = waveLength;
                        info[i].Nominal = double.Parse(row[1 + i * 3 + 0]);
                        info[i].min = double.Parse(row[1 + i * 3 + 1]);
                        info[i].max = double.Parse(row[1 + i * 3 + 2]);
                        info[i].Measured = 0;
                        info[i].result = "";
                        NDInfo1[i].data.Add(info[i]);
                        NDInfo2[i].data.Add(info[i]);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Some data are wrong in " + fileName);
                }
            }
        }
        List<CalibrationData> myData = new List<CalibrationData>();
        private void LoadGoldenSampleInformation()
        {
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fileName = CalibrationFolder + "Golden_Average.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            myData.Clear();
            StreamReader sr = new StreamReader(fileName);
            string line;
            string[] row;
            int Rowcnt = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                    continue;

                row = line.Split(',');
                if (row[0] == "")
                    continue;

                Rowcnt++;
                if (Rowcnt == 1)
                {
                    continue;
                }

                CalibrationData tmpData = new CalibrationData();
                tmpData.Serial = row[0];

                for (int i = 1; i <= 85; i++)
                    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 70; i <= 86; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 105; i <= 121; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 140; i <= 156; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 175; i <= 191; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));

                myData.Add(tmpData);
            }

        }
        List<CalibrationSpec> mySpec = new List<CalibrationSpec>();
        private void LoadGoldenSampleSpec()
        {
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fileName = CalibrationFolder + "Ref_Sample_Spec.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample Spec File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            mySpec.Clear();
            StreamReader sr = new StreamReader(fileName);
            string line;
            string[] row;
            int Rowcnt = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (Rowcnt < 2)
                {
                    Rowcnt++;
                    continue;
                }

                if (line == "")
                    continue;

                row = line.Split(',');
                if (row[0] == "")
                    continue;

                CalibrationSpec tmpData = new CalibrationSpec();
                tmpData.UpperLimit = float.Parse(row[1]);
                tmpData.LowerLimit = float.Parse(row[2]);


                mySpec.Add(tmpData);
            }

        }

        private void GS1SerialCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            GS1LV.Items.Clear();

            if (GoldenSampleCalibrateBtn.BackColor == Color.Lime)
                LoadGS1UIAndResult();
            else if (NDFilterCheckBtn.BackColor == Color.Lime)
                LoadNDF1UIAndResult();
            //LoadNDF1UIAndResult();
        }

        private void GS2SerialCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            GS2LV.Items.Clear();
            if (GoldenSampleCalibrateBtn.BackColor == Color.Lime)
                LoadGS2UIAndResult();
            else if (NDFilterCheckBtn.BackColor == Color.Lime)
                LoadNDF2UIAndResult();
            //LoadNDF2UIAndResult();
        }

        StationModule stationRes = new StationModule();

        private void button8_Click(object sender, EventArgs e)
        {
            GoldenSampleCalibrateBtn.BackColor = Color.Lime;
            NDFilterCheckBtn.BackColor = Color.Transparent;
            LightSourceCheckBtn.BackColor = Color.Transparent;
            this.tabControl1.SelectedTab = tabPage1;
            IndividualResultGB.Visible = false;
            GrouResultGB.Visible = false;
            //groupBox1.Visible = true;
            SkipBarcodeCB.Visible = true;

            LoadGoldenSampleInformation();
            LoadGoldenSampleSpec();
            LoadGUSerial();
            InitGS1LV();
            InitGS2LV();

            //if (GS1SerialCB.SelectedIndex == -1)
            //{
            //    MessageBox.Show("No Golden Unit 1 Selected.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //if (GS2SerialCB.SelectedIndex == -1)
            //{
            //    MessageBox.Show("No Golden Unit 2 Selected.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //if (GS1SerialCB.SelectedIndex == GS2SerialCB.SelectedIndex)
            //{
            //    MessageBox.Show("Both Golden Unit Selected Is the same.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            if (MessageBox.Show("Load Golden Units On the Tester ", "Golden Unit Calibration Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (!SkipBarcodeCB.Checked)
            {
                string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();

                if (Mod1Barcode == "")
                {
                    MessageBox.Show("Golden Sample Module 1 Barcode Read Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();

                if (Mod2Barcode == "")
                {
                    MessageBox.Show("Golden Sample Module 2 Barcode Read Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (GS1SerialCB.Items.Contains(Mod1Barcode.Substring(0, 17)))
                {
                    GS1SerialCB.SelectedIndex = GS1SerialCB.Items.IndexOf(Mod1Barcode.Substring(0, 17));
                    LoadGS1UI();
                    Application.DoEvents();
                }
                else
                {
                    MessageBox.Show("Module 1 Unit is Not Golden Sample.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (GS2SerialCB.Items.Contains(Mod2Barcode.Substring(0, 17)))
                {
                    GS2SerialCB.SelectedIndex = GS2SerialCB.Items.IndexOf(Mod2Barcode.Substring(0, 17));
                    LoadGS2UI();
                    Application.DoEvents();
                }
                else
                {
                    MessageBox.Show("Module 2 Unit is Not Golden Sample.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            stationRes.Unit1Barcode = GS1SerialCB.Items[GS1SerialCB.SelectedIndex].ToString();
            stationRes.Unit2Barcode = GS2SerialCB.Items[GS2SerialCB.SelectedIndex].ToString();

            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, 0);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, 0);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);

            int rtIdx = Para.CurrentRotaryIndex;
            switch (rtIdx)
            {
                case 0:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, true);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, true);
                    Thread.Sleep(100);
                    break;
                case 1:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, true);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, true);
                    Thread.Sleep(100);
                    break;
                case 2:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, true);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, true);
                    Thread.Sleep(100);
                    break;
                case 3:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, true);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, true);
                    Thread.Sleep(100);
                    break;
            }
            Para.myMain.WriteOperationinformation("AUDITBOX button8_Click T " + rtIdx.ToString());
            Thread.Sleep(500);
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Para.myMain.ClearInspectionResults();

            if (!InspectMod1())
            {
                MessageBox.Show("Module 1 Inspection Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!InspectMod2())
            {
                MessageBox.Show("Module 2 Inspection Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            GetDarkRef();
            Application.DoEvents();

            TestMod();

            motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);

            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            rtIdx = Para.CurrentRotaryIndex;
            switch (rtIdx)
            {
                case 0:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
                    Thread.Sleep(100);
                    break;
                case 1:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);
                    Thread.Sleep(100);
                    break;
                case 2:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);
                    Thread.Sleep(100);
                    break;
                case 3:
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    Thread.Sleep(100);
                    break;
            }
            Para.myMain.WriteOperationinformation("AUDITBOX button8_Click " + rtIdx.ToString());

            Thread.Sleep(500);
            DisplayResult();
            MessageBox.Show("Golden Unit Calibration Completed.", "Calibration Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool InspectMod1()
        {
            Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
            Thread.Sleep(1000);

            for (int c = 0; c < 3; c++)
            {
                bool GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Para.myMain.camera1.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }

                if (!GrabPass)
                {
                    Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                    Thread.Sleep(200);
                    return false;
                }

                stationRes.mod1VisResult = Para.myMain.camera1.Inspect(Para.CaliX1);

                if (stationRes.mod1VisResult.Found)
                {
                    break;
                }
            }

            Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
            //Thread.Sleep(200);

            if (!stationRes.mod1VisResult.Found)
                return false;

            Para.myMain.DisplayCam1Result(stationRes.mod1VisResult);

            return true;
        }
        private bool InspectMod2()
        {
            Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
            Thread.Sleep(1000);

            for (int c = 0; c < 3; c++)
            {
                bool GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Para.myMain.camera2.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }

                if (!GrabPass)
                {
                    Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                    Thread.Sleep(200);
                    return false;
                }

                stationRes.mod2VisResult = Para.myMain.camera2.Inspect(Para.CaliX2);

                if (stationRes.mod2VisResult.Found)
                {
                    break;
                }
            }

            Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
            //Thread.Sleep(200);

            if (!stationRes.mod2VisResult.Found)
                return false;

            Para.myMain.DisplayCam2Result(stationRes.mod2VisResult);

            return true;
        }

        private bool GetDarkRef()
        {
            Para.myMain.UpdateMod1TestStatus("Dark Test", Color.Lime);
            if (motionMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS))
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
                Thread.Sleep(1500);
            }
            //myMgr.SetIO(ModLB.SelectedIndex, true);
            Para.myMain.specMgr.SetIO(0, true);
            Para.myMain.specMgr.SetIO(1, true);

            stationRes.DarkRefMod1 = specMgr.GetCount(0);
            stationRes.WLMod1Dark = specMgr.GetWaveLength(0);
            Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.DarkRefMod1, false);

            Para.myMain.UpdateMod2TestStatus("Dark Test", Color.Lime);
            stationRes.DarkRefMod2 = specMgr.GetCount(1);
            stationRes.WLMod2Dark = specMgr.GetWaveLength(1);

            //////////////////////////////////////////////////////////////////////
            /// <summary>
            /// ////////////Larry For AuditBox 20170117
            /// </summary>
            //           wl = stationRes.WLMod2Dark;
            darkRef1 = stationRes.DarkRefMod1;
            darkRef2 = stationRes.DarkRefMod2;
            ///////////////////////////////////////////////////

            Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.DarkRefMod2, false);

            motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
            Para.myMain.specMgr.SetIO(0, false);
            Para.myMain.specMgr.SetIO(1, false);
            Thread.Sleep(1500);

            return true;
        }
        private bool TestMod()
        {
            //Move To White Reference Position
            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[1].X);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[1].Y);
            motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[1].X);
            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[1].Y);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);

            //Get White Ref
            Para.myMain.UpdateMod1TestStatus("White Test", Color.Lime);
            Para.myMain.UpdateMod2TestStatus("White Test", Color.Lime);
            stationRes.WhiteRefMod1 = specMgr.GetCount(0);
            stationRes.WhiteRefMod2 = specMgr.GetCount(1);
            Application.DoEvents();



            Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.WhiteRefMod1, false);
            Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.WhiteRefMod2, false);
            Application.DoEvents();


            //Unit Ctr Offset In MM
            double unitCtrOffX = (((Para.myMain.camera1.ImageWidth / 2) - stationRes.mod1VisResult.X) * Para.myMain.camera1.CaliValue.X) + Para.Module[0].CamToOriginOffset.X;
            double unitCtrOffY = (((Para.myMain.camera1.ImageHeight / 2) - stationRes.mod1VisResult.Y) * Para.myMain.camera1.CaliValue.Y) + Para.Module[0].CamToOriginOffset.Y;

            //Unit Ctr Offset In MM
            double unitCtrOffX2 = (((Para.myMain.camera2.ImageWidth / 2) - stationRes.mod2VisResult.X) * Para.myMain.camera2.CaliValue.X) + Para.Module[1].CamToOriginOffset.X;
            double unitCtrOffY2 = (((Para.myMain.camera2.ImageHeight / 2) - stationRes.mod2VisResult.Y) * Para.myMain.camera2.CaliValue.Y) + Para.Module[1].CamToOriginOffset.Y;


            System.Drawing.PointF CtrPt = new System.Drawing.PointF();
            CtrPt.X = (Para.myMain.camera1.ImageWidth / 2);
            CtrPt.Y = (Para.myMain.camera1.ImageHeight / 2);

            System.Drawing.PointF CtrPt2 = new System.Drawing.PointF();
            CtrPt2.X = (Para.myMain.camera2.ImageWidth / 2);
            CtrPt2.Y = (Para.myMain.camera2.ImageHeight / 2);

            for (int i = 0; i < Para.Module[0].TestPt.Count; i++)
            {
                Para.myMain.UpdateMod1TestStatus("Start Test Point " + (i + 1).ToString(), Color.Lime);
                Para.myMain.UpdateMod2TestStatus("Start Test Point " + (i + 1).ToString(), Color.Lime);
                Application.DoEvents();
                System.Drawing.PointF tpt1 = new System.Drawing.PointF(); // test pt in Pixel 
                tpt1.X = (float)(CtrPt.X + (Para.Module[0].TestPt[i].X / Para.myMain.camera1.CaliValue.X));
                tpt1.Y = (float)(CtrPt.Y + (Para.Module[0].TestPt[i].Y / Para.myMain.camera1.CaliValue.Y));

                System.Drawing.PointF tpt2 = new System.Drawing.PointF(); // test pt in Pixel 
                tpt2.X = (float)(CtrPt2.X + (Para.Module[1].TestPt[i].X / Para.myMain.camera2.CaliValue.X));
                tpt2.Y = (float)(CtrPt2.Y + (Para.Module[1].TestPt[i].Y / Para.myMain.camera2.CaliValue.Y));

                Helper.ApplyRotation(ref tpt1, -(float)stationRes.mod1VisResult.Angle, CtrPt);
                Helper.ApplyRotation(ref tpt2, -(float)stationRes.mod2VisResult.Angle, CtrPt2);

                DPoint tpt1Offset = new DPoint();
                tpt1Offset.X = (tpt1.X - CtrPt.X) * Para.myMain.camera1.CaliValue.X;
                tpt1Offset.Y = (tpt1.Y - CtrPt.Y) * Para.myMain.camera1.CaliValue.Y;

                DPoint tpt2Offset = new DPoint();
                tpt2Offset.X = (tpt2.X - CtrPt2.X) * Para.myMain.camera2.CaliValue.X;
                tpt2Offset.Y = (tpt2.Y - CtrPt2.Y) * Para.myMain.camera2.CaliValue.Y;

                motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[0].X + tpt1Offset.X + unitCtrOffX);
                motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY);
                motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[0].X + tpt2Offset.X + unitCtrOffX2);
                motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[0].Y + tpt2Offset.Y + unitCtrOffY2);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);


                stationRes.MeasDataMod1[i] = specMgr.GetCount(0);
                stationRes.MeasDataMod2[i] = specMgr.GetCount(1);


                Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.MeasDataMod1[i], false);
                Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.MeasDataMod2[i], false);
                Application.DoEvents();
                stationRes.transRatioMod1[i] = Para.myMain.SeqMgr.CalculateTransRatio(stationRes.DarkRefMod1, stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i]);
                stationRes.transRatioMod2[i] = Para.myMain.SeqMgr.CalculateTransRatio(stationRes.DarkRefMod2, stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i]);
                Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.transRatioMod1[i], true);
                Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.transRatioMod2[i], true);




                SaveRefDarkTransData("D:\\DailyGoldenData", stationRes.Unit1Barcode, 400, 1100, i + 1, stationRes.WLMod1Dark, stationRes.DarkRefMod1,
                                     stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i], stationRes.transRatioMod1[i], 1);

                SaveRawData("D:\\DailyGoldenData", stationRes.Unit1Barcode, 400, 1100, i + 1, stationRes.WLMod1Dark, stationRes.WLMod1Dark, stationRes.WLMod1Dark,
                    stationRes.DarkRefMod1, stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i], 1, stationRes.mod1VisResult);

                SaveRefDarkTransData("D:\\DailyGoldenData", stationRes.Unit2Barcode, 400, 1100, i + 1, stationRes.WLMod2Dark, stationRes.DarkRefMod2,
                                     stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i], stationRes.transRatioMod2[i], 2);

                SaveRawData("D:\\DailyGoldenData", stationRes.Unit2Barcode, 400, 1100, i + 1, stationRes.WLMod2Dark, stationRes.WLMod2Dark, stationRes.WLMod2Dark,
                    stationRes.DarkRefMod2, stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i], 2, stationRes.mod2VisResult);

            }

            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[0].X);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, 0);
            motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[0].X);
            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, 0);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
            return true;
        }
        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ////////////Larry For AuditBox 20170117 Copy From WinSpectrometer.cs
        /// </summary>
        //public void SaveRawData(string barCode, int stWaveLength, int endWaveLength, List<float> WL,
        //                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData)
        //{
        //    string s_FileName = barCode + "_RawData_" + DateTime.Now.ToString("yyyyMMdd");

        //    string path = "D:\\AuditBox";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string FileName = path + "\\" + s_FileName + ".csv";

        //    FileStream objFileStream;
        //    bool bCreatedNew = false;

        //    if (!File.Exists(FileName))
        //    {
        //        objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
        //        bCreatedNew = true;
        //    }
        //    else
        //    {
        //        objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
        //    }
        //    StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
        //    string columnTitle = "";
        //    int i;
        //    string columnValue = "";
        //    int t = 0;
        //    //int waveNum = pub.m_Lambda.Value.GetLength(0);//总的波长个数
        //    double tempCounts = 0;
        //    try
        //    {

        //        if (bCreatedNew)
        //        {
        //            columnTitle = "CG Test:" + "," + Para.SWVersion + ",";
        //            sw.WriteLine(columnTitle);
        //            columnTitle = "";

        //            //写入列标题
        //            columnTitle = "SerialNumber" + "," + "TesterID" + ",";

        //            for (int wave = stWaveLength; wave <= endWaveLength; wave++)
        //            {
        //                columnTitle += "dark_" + Convert.ToString(wave) + ",";
        //            }
        //            for (int wave = stWaveLength; wave <= endWaveLength; wave++)
        //            {
        //                columnTitle += "white_" + Convert.ToString(wave) + ",";
        //            }
        //            for (int wave = stWaveLength; wave <= endWaveLength; wave++)
        //            {
        //                columnTitle += "Measured_" + Convert.ToString(wave) + ",";
        //            }
        //            sw.WriteLine(columnTitle);
        //        }

        //        columnValue = barCode;//barCode.Replace(",", "") + string.Format("{0:D2}", index + 1) + ",";//二维码+num
        //        columnValue += Para.MchName + ",";

        //        int s = 0;

        //        //dark_
        //        for (i = stWaveLength; i <= endWaveLength; i++)
        //        {
        //            t = NearestIndex(s, i, WL);
        //            s = t;
        //            tempCounts = darkRef[t];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
        //            columnValue += tempCounts.ToString("F4") + ",";

        //        }

        //        //"White_"
        //        s = 0;
        //        for (i = stWaveLength; i <= endWaveLength; i++)
        //        {
        //            t = NearestIndex(s, i, WL);
        //            s = t;
        //            tempCounts = WhiteRef[t];//MeasData[t] - darkRef[t];

        //            columnValue += tempCounts.ToString("F4") + ",";
        //        }

        //        //"T%_"
        //        s = 0;
        //        for (i = stWaveLength; i <= endWaveLength; i++)
        //        {
        //            t = NearestIndex(s, i, WL);
        //            s = t;
        //            columnValue += MeasData[t].ToString("F4") + ",";
        //        }
        //        sw.WriteLine(columnValue);
        //        sw.Close();
        //        objFileStream.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.ToString());
        //    }
        //    finally
        //    {
        //        sw.Close();
        //        objFileStream.Close();
        //    }
        //}
        ////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// ////////////Larry For AuditBox 20170117 Copy From WinSpectrometer.cs
        ///// </summary>
        //public void SaveRefDarkTransData(string barCode, int stWaveLength, int endWaveLength, List<float> WL,
        //                                        List<float> darkRef, List<float> WhiteRef, List<float> MeasData, List<float> TransData)
        //{
        //    string s_FileName = barCode + "_TransData_" + DateTime.Now.ToString("yyyyMMdd");

        //    string path = "D:\\AuditBox";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string FileName = path + "\\" + s_FileName + ".csv";

        //    FileStream objFileStream;
        //    bool bCreatedNew = false;

        //    if (!File.Exists(FileName))
        //    {
        //        objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
        //        bCreatedNew = true;
        //    }
        //    else
        //    {
        //        objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
        //    }
        //    StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
        //    string columnTitle = "";
        //    int i;
        //    string columnValue = "";
        //    int t = 0;
        //    //int waveNum = pub.m_Lambda.Value.GetLength(0);//总的波长个数
        //    double tempCounts = 0;
        //    try
        //    {

        //        if (bCreatedNew)
        //        {
        //            columnTitle = "CG Test:" + "," + Para.SWVersion + ",";
        //            sw.WriteLine(columnTitle);
        //            columnTitle = "";

        //            //写入列标题
        //            columnTitle = "SerialNumber" + "," + "TesterID" + ",";

        //            for (int wave = stWaveLength; wave <= endWaveLength; wave++)
        //            {
        //                columnTitle += "ref-dark_" + Convert.ToString(wave) + ",";
        //            }
        //            for (int wave = stWaveLength; wave <= endWaveLength; wave++)
        //            {
        //                columnTitle += "trans-dark_" + Convert.ToString(wave) + ",";
        //            }
        //            for (int wave = stWaveLength; wave <= endWaveLength; wave++)
        //            {
        //                columnTitle += "T%_" + Convert.ToString(wave) + ",";
        //            }
        //            sw.WriteLine(columnTitle);
        //        }

        //        columnValue = barCode;//barCode.Replace(",", "") + string.Format("{0:D2}", index + 1) + ",";//二维码+num
        //        columnValue += Para.MchName + ",";

        //        int s = 0;

        //        //ref-dark_
        //        for (i = stWaveLength; i <= endWaveLength; i++)
        //        {
        //            t = NearestIndex(s, i, WL);
        //            s = t;
        //            tempCounts = WhiteRef[t] - darkRef[t];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
        //            columnValue += tempCounts.ToString("F4") + ",";

        //        }

        //        //"trans-dark_"
        //        s = 0;
        //        for (i = stWaveLength; i <= endWaveLength; i++)
        //        {
        //            t = NearestIndex(s, i, WL);
        //            s = t;
        //            tempCounts = MeasData[t] - darkRef[t];

        //            columnValue += tempCounts.ToString("F4") + ",";
        //        }

        //        //"T%_"
        //        s = 0;
        //        for (i = stWaveLength; i <= endWaveLength; i++)
        //        {
        //            t = NearestIndex(s, i, WL);
        //            s = t;
        //            columnValue += TransData[t].ToString("F4") + ",";
        //        }
        //        sw.WriteLine(columnValue);
        //        sw.Close();
        //        objFileStream.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.ToString());
        //    }
        //    finally
        //    {
        //        sw.Close();
        //        objFileStream.Close();
        //    }

        //}
        ////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ////////////Larry For AuditBox 20170117 Copy From WinSpectrometer.cs
        /// </summary>
        public float CalTranData(double dData, double wData, double counts)
        {
            double up = counts - dData;
            double down = wData - dData;

            if (down <= 0 || up <= 0)
                return 0;
            return (float)(up / down) * 100;
        }
        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ////////////Larry For AuditBox 20170117 Copy From WinSpectrometer.cs
        /// </summary>
        public List<float> CalculateTransRatio(List<float> darkRef, List<float> whiteRef, List<float> curCnt)
        {
            List<float> myTransRatio = new List<float>();
            if (darkRef.Count < curCnt.Count || whiteRef.Count < curCnt.Count)
                return myTransRatio;

            for (int i = 0; i < curCnt.Count; i++)
                myTransRatio.Add(CalTranData(darkRef[i], whiteRef[i], curCnt[i]));
            return myTransRatio;
        }
        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ////////////Larry For AuditBox 20170117 
        /// </summary>
        private bool TestNDFMod(out bool res1, out bool res2)
        {


            res1 = true;
            res2 = true;
            //Move To White Reference Position
            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[1].X);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[1].Y);
            motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[1].X);
            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[1].Y);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);

            //Get White Ref
            Para.myMain.UpdateMod1TestStatus("White Test", Color.Lime);
            Para.myMain.UpdateMod2TestStatus("White Test", Color.Lime);
            stationRes.WhiteRefMod1 = specMgr.GetCount(0);
            stationRes.WhiteRefMod2 = specMgr.GetCount(1);
            Application.DoEvents();

            //////////////////////////////////////////////////////////////////////
            /// <summary>
            /// ////////////Larry For AuditBox 20170117
            /// </summary>
            whiteRef1 = stationRes.WhiteRefMod1;
            whiteRef2 = stationRes.WhiteRefMod2;

            Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.WhiteRefMod1, false);
            Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.WhiteRefMod2, false);
            Application.DoEvents();


            //Unit Ctr Offset In MM
            double unitCtrOffX = (((Para.myMain.camera1.ImageWidth / 2) - stationRes.mod1VisResult.X) * Para.myMain.camera1.CaliValue.X) + Para.Module[0].CamToOriginOffset.X;
            double unitCtrOffY = (((Para.myMain.camera1.ImageHeight / 2) - stationRes.mod1VisResult.Y) * Para.myMain.camera1.CaliValue.Y) + Para.Module[0].CamToOriginOffset.Y;

            //Unit Ctr Offset In MM
            double unitCtrOffX2 = (((Para.myMain.camera2.ImageWidth / 2) - stationRes.mod2VisResult.X) * Para.myMain.camera2.CaliValue.X) + Para.Module[1].CamToOriginOffset.X;
            double unitCtrOffY2 = (((Para.myMain.camera2.ImageHeight / 2) - stationRes.mod2VisResult.Y) * Para.myMain.camera2.CaliValue.Y) + Para.Module[1].CamToOriginOffset.Y;


            System.Drawing.PointF CtrPt = new System.Drawing.PointF();
            CtrPt.X = (Para.myMain.camera1.ImageWidth / 2);
            CtrPt.Y = (Para.myMain.camera1.ImageHeight / 2);

            System.Drawing.PointF CtrPt2 = new System.Drawing.PointF();
            CtrPt2.X = (Para.myMain.camera2.ImageWidth / 2);
            CtrPt2.Y = (Para.myMain.camera2.ImageHeight / 2);
            int i = 0;
            //for (int i = 0; i < Para.Module[0].TestPt.Count; i++)
            //{
            //    Para.myMain.UpdateMod1TestStatus("Start Test Point " + (i + 1).ToString(), Color.Lime);
            //    Para.myMain.UpdateMod2TestStatus("Start Test Point " + (i + 1).ToString(), Color.Lime);
            //    Application.DoEvents();
            //    System.Drawing.PointF tpt1 = new System.Drawing.PointF(); // test pt in Pixel 
            //    tpt1.X = (float)(CtrPt.X + (Para.Module[0].TestPt[i].X / Para.myMain.camera1.CaliValue.X));
            //    tpt1.Y = (float)(CtrPt.Y + (Para.Module[0].TestPt[i].Y / Para.myMain.camera1.CaliValue.Y));

            //    System.Drawing.PointF tpt2 = new System.Drawing.PointF(); // test pt in Pixel 
            //    tpt2.X = (float)(CtrPt2.X + (Para.Module[1].TestPt[i].X / Para.myMain.camera2.CaliValue.X));
            //    tpt2.Y = (float)(CtrPt2.Y + (Para.Module[1].TestPt[i].Y / Para.myMain.camera2.CaliValue.Y));

            //    Helper.ApplyRotation(ref tpt1, -(float)stationRes.mod1VisResult.Angle, CtrPt);
            //    Helper.ApplyRotation(ref tpt2, -(float)stationRes.mod2VisResult.Angle, CtrPt2);

            DPoint tpt1Offset = new DPoint();
            tpt1Offset.X = 0; // (tpt1.X - CtrPt.X) * Para.myMain.camera1.CaliValue.X;
            tpt1Offset.Y = 0; // (tpt1.Y - CtrPt.Y) * Para.myMain.camera1.CaliValue.Y;

            DPoint tpt2Offset = new DPoint();
            tpt2Offset.X = 0; // (tpt2.X - CtrPt2.X) * Para.myMain.camera2.CaliValue.X;
            tpt2Offset.Y = 0; // (tpt2.Y - CtrPt2.Y) * Para.myMain.camera2.CaliValue.Y;

            ////unitCtrOffX = 0;
            //unitCtrOffY = 0;
            //unitCtrOffX2 = 0;
            //unitCtrOffY2 = 0;

            //motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[0].X + tpt1Offset.X + unitCtrOffX);
            //motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY);
            //motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[0].X + tpt2Offset.X + unitCtrOffX2);
            //motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[0].Y + tpt2Offset.Y + unitCtrOffY2);
            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[0].X + tpt1Offset.X + unitCtrOffX);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY);
            motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[0].X + tpt2Offset.X + unitCtrOffX2);
            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[0].Y + tpt2Offset.Y + unitCtrOffY2);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);


            stationRes.MeasDataMod1[i] = specMgr.GetCount(0);
            stationRes.MeasDataMod2[i] = specMgr.GetCount(1);
            //////////////////////////////////////////////////////////////////////
            /// <summary>
            /// ////////////Larry For AuditBox 20170117
            /// </summary>
            measVal1 = stationRes.MeasDataMod1[i];
            measVal2 = stationRes.MeasDataMod2[i];

            Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.MeasDataMod1[i], false);
            Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.MeasDataMod2[i], false);
            Application.DoEvents();
            stationRes.transRatioMod1[i] = Para.myMain.SeqMgr.CalculateTransRatio(stationRes.DarkRefMod1, stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i]);
            stationRes.transRatioMod2[i] = Para.myMain.SeqMgr.CalculateTransRatio(stationRes.DarkRefMod2, stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i]);
            Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.transRatioMod1[i], true);
            Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.transRatioMod2[i], true);
            Application.DoEvents();
            //SaveRefDarkTransData("D:\\AuditBoxNDFilterData", stationRes.Unit1Barcode, 400, 1100, i + 1, stationRes.WLMod1Dark, stationRes.DarkRefMod1,
            //                     stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i], stationRes.transRatioMod1[i], 1);

            //SaveRawData("D:\\AuditBoxNDFilterData", stationRes.Unit1Barcode, 400, 1100, i + 1, stationRes.WLMod1Dark, stationRes.WLMod1Dark, stationRes.WLMod1Dark,
            //    stationRes.DarkRefMod1, stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i], 1, stationRes.mod1VisResult);

            //SaveRefDarkTransData("D:\\AuditBoxNDFilterData", stationRes.Unit2Barcode, 400, 1100, i + 1, stationRes.WLMod2Dark, stationRes.DarkRefMod2,
            //                     stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i], stationRes.transRatioMod2[i], 2);

            //SaveRawData("D:\\AuditBoxNDFilterData", stationRes.Unit2Barcode, 400, 1100, i + 1, stationRes.WLMod2Dark, stationRes.WLMod2Dark, stationRes.WLMod2Dark,
            //    stationRes.DarkRefMod2, stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i], 2, stationRes.mod2VisResult);

            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, 0); //Para.Module[0].TeachPos[0].X);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, 0);
            motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, 0); //Para.Module[1].TeachPos[0].X);
            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, 0);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);

            //////////////////////////////////////////////////////////////////////
            /// <summary>
            /// ////////////Larry For AuditBox 20170117
            /// </summary>
            TransVal1 = CalculateTransRatio(darkRef1, whiteRef1, measVal1);
            TransVal2 = CalculateTransRatio(darkRef2, whiteRef2, measVal2);
            Application.DoEvents();

            //SaveRefDarkTransData(stationRes.Unit1Barcode, 400, 1100, wl, darkRef1, whiteRef1, measVal1, TransVal1);
            //SaveRawData(stationRes.Unit1Barcode, 400, 1100, wl, darkRef1, whiteRef1, measVal1);
            //SaveRefDarkTransData(stationRes.Unit2Barcode, 400, 1100, wl, darkRef2, whiteRef2, measVal2, TransVal2);
            //SaveRawData(stationRes.Unit2Barcode, 400, 1100, wl, darkRef2, whiteRef2, measVal2);
            int sIdx = 0;
            int idx = 0;
            //InitCaliLV();
            //InitGS1LV();
            //InitGS2LV();
            int Idx = 0;
            if (!RightModOnlyCB.Checked)
            {
                Idx = GS1SerialCB.Items.IndexOf(stationRes.Unit1Barcode);
                //List<float> NDTransData = new List<float>();

                for (int ii = 0; ii < NDInfo1[Idx].data.Count; ii++)
                {
                    idx = NearestIndex(sIdx, NDInfo1[Idx].data[ii].waveLength, stationRes.WLMod1Dark);
                    sIdx = idx;

                    //NDTransData.Add(TransVal1[idx]);
                    try
                    {
                        NDInfo1[Idx].data[ii].Measured = TransVal1[idx];
                    }
                    catch (Exception)
                    {
                        if (TransVal1.Count > 0)
                            NDInfo1[Idx].data[ii].Measured = TransVal1[TransVal1.Count - 1];
                    }

                    double min = NDInfo1[Idx].data[ii].Nominal * (NDInfo1[Idx].data[ii].min / 100);
                    double max = NDInfo1[Idx].data[ii].Nominal * (NDInfo1[Idx].data[ii].max / 100);
                    double diff = NDInfo1[Idx].data[ii].Measured - NDInfo1[Idx].data[ii].Nominal;
                    //double diff = TransVal1[idx] - NDInfo1[Idx].data[ii].Nominal;
                    NDInfo1[Idx].data[ii].Diff = diff / NDInfo1[Idx].data[ii].Nominal * 100;
                    if ((diff < min) || (diff > max))
                    {
                        res1 = false;
                        NDInfo1[Idx].data[ii].result = "Fail";
                    }
                    else
                    {
                        //res1 = true;
                        NDInfo1[Idx].data[ii].result = "Pass";
                    }
                }
                SaveNDFilterData(NDInfo1[Idx], 1);
            }


            if (!LeftModOnlyCB.Checked)
            {
                Idx = GS2SerialCB.Items.IndexOf(stationRes.Unit2Barcode);
                //List<float> NDTransData = new List<float>();
                //NDTransData.Clear();
                for (int ii = 0; ii < NDInfo2[Idx].data.Count; ii++)
                {
                    idx = NearestIndex(sIdx, NDInfo2[Idx].data[ii].waveLength, stationRes.WLMod2Dark);
                    sIdx = idx;

                    //NDTransData.Add(TransVal2[idx]);
                    try
                    {
                        NDInfo2[Idx].data[ii].Measured = TransVal2[idx];
                    }
                    catch (Exception)
                    {
                        if (TransVal2.Count > 0)
                            NDInfo2[Idx].data[ii].Measured = TransVal2[TransVal2.Count - 1];
                    }

                    double min = NDInfo2[Idx].data[ii].Nominal * (NDInfo2[Idx].data[ii].min / 100);
                    double max = NDInfo2[Idx].data[ii].Nominal * (NDInfo2[Idx].data[ii].max / 100);
                    double diff = NDInfo2[Idx].data[ii].Measured - NDInfo2[Idx].data[ii].Nominal;
                    //double diff = TransVal2[idx] - NDInfo2[Idx].data[ii].Nominal;
                    NDInfo2[Idx].data[ii].Diff = diff / NDInfo2[Idx].data[ii].Nominal * 100;
                    if ((diff < min) || (diff > max))
                    {
                        res2 = false;
                        NDInfo2[Idx].data[ii].result = "Fail";
                    }
                    else
                    {
                        //res2 = true;
                        NDInfo2[Idx].data[ii].result = "Pass";
                    }
                }
                SaveNDFilterData(NDInfo2[Idx], 2);
            }
            return true;
        }

        public void SaveRawData(string path, string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WLDark, List<float> WLWhite, List<float> WLMeas,
                                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData, int ModuleIndex, JPTCG.Vision.HalconInspection.RectData VisResult)
        {
            //lock (sny_Obj)
            {
                string s_FileName = barCode + "_RawTransData_" + DateTime.Now.ToString("yyyyMMdd");

                //string path = "D:\\DailyGoldenData";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //path = "D:\\DailyGoldenData\\Module" + (ModuleIndex).ToString();
                path += "\\Module" + (ModuleIndex).ToString();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string FileName = path + "\\" + s_FileName + ".csv";

                FileStream objFileStream;
                bool bCreatedNew = false;

                if (!File.Exists(FileName))
                {
                    objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                    bCreatedNew = true;
                }
                else
                {
                    objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                }
                StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
                string columnTitle = "";
                int i;
                string columnValue = "";
                int t = 0;
                //int waveNum = pub.m_Lambda.Value.GetLength(0);//总的波长个数
                double tempCounts = 0;
                try
                {
                    //bCreatedNew = true;

                    if (bCreatedNew)
                    {
                        columnTitle = "CG Test:" + "," + Para.SWVersion + ",";
                        sw.WriteLine(columnTitle);
                        columnTitle = "";

                        //写入列标题
                        columnTitle = "SerialNumber" + "," + "TesterID" + "," + "TestPoint" + ",";

                        for (int wave = 0; wave < WLDark.Count; wave++)
                        {
                            columnTitle += "D_" + WLDark[wave].ToString("F3") + ",";
                        }
                        for (int wave = 0; wave < WLWhite.Count; wave++)
                        {
                            columnTitle += "W_" + WLWhite[wave].ToString("F3") + ",";
                        }
                        for (int wave = 0; wave < WLMeas.Count; wave++)
                        {
                            columnTitle += "M_" + WLMeas[wave].ToString("F3") + ",";
                        }

                        columnTitle += "Vis_X,Vis_Y,Vis_Ang,";

                        sw.WriteLine(columnTitle);
                    }

                    columnValue = barCode + ",";//barCode.Replace(",", "") + string.Format("{0:D2}", index + 1) + ",";//二维码+num
                    columnValue += Para.MchName + "," + TestPoint.ToString() + ",";

                    int s = 0;

                    //dark_
                    for (i = 0; i < darkRef.Count; i++)
                    {
                        //t = NearestIndex(s, i, WL);
                        //s = t;
                        tempCounts = darkRef[i];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
                        columnValue += tempCounts.ToString("F4") + ",";

                    }

                    //"White_"
                    s = 0;
                    //for (i = stWaveLength; i <= endWaveLength; i++)
                    for (i = 0; i < WhiteRef.Count; i++)
                    {
                        //t = NearestIndex(s, i, WL);
                        //s = t;
                        tempCounts = WhiteRef[i];//MeasData[t] - darkRef[t];

                        columnValue += tempCounts.ToString("F4") + ",";
                    }

                    //"T%_"
                    //s = 0;
                    //for (i = stWaveLength; i <= endWaveLength; i++)
                    for (i = 0; i < MeasData.Count; i++)
                    {
                        //t = NearestIndex(s, i, WL);
                        //s = t;
                        columnValue += MeasData[i].ToString("F4") + ",";
                    }
                    columnValue += VisResult.X.ToString("F2") + ",";
                    columnValue += VisResult.Y.ToString("F2") + ",";
                    columnValue += VisResult.Angle.ToString("F2") + ",";

                    sw.WriteLine(columnValue);
                    sw.Close();
                    objFileStream.Close();
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                    objFileStream.Close();
                }
            }
        }

        public void SaveRefDarkTransData(string path, string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WL,
                                                 List<float> darkRef, List<float> WhiteRef, List<float> MeasData, List<float> TransData, int ModuleIndex)
        {
            string s_FileName = barCode + "_TransData_" + DateTime.Now.ToString("yyyyMMdd");

            //string path = "D:\\DailyGoldenData";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //path = "D:\\DailyGoldenData\\Module" + (ModuleIndex).ToString();
            path += "\\Module" + (ModuleIndex).ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FileName = path + "\\" + s_FileName + ".csv";

            FileStream objFileStream;
            bool bCreatedNew = false;

            if (!File.Exists(FileName))
            {
                objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                bCreatedNew = true;
            }
            else
            {
                objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
            }
            StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
            string columnTitle = "";
            int i;
            string columnValue = "";
            int t = 0;
            //int waveNum = pub.m_Lambda.Value.GetLength(0);//总的波长个数
            double tempCounts = 0;
            try
            {

                if (bCreatedNew)
                {
                    columnTitle = "CG Test:" + "," + Para.SWVersion + ",";
                    sw.WriteLine(columnTitle);
                    columnTitle = "";

                    //写入列标题
                    columnTitle = "SerialNumber" + "," + "TesterID" + "," + "TestPoint" + ",";

                    for (int wave = stWaveLength; wave <= endWaveLength; wave++)
                    {
                        columnTitle += "ref-dark_" + Convert.ToString(wave) + ",";
                    }
                    for (int wave = stWaveLength; wave <= endWaveLength; wave++)
                    {
                        columnTitle += "trans-dark_" + Convert.ToString(wave) + ",";
                    }
                    for (int wave = stWaveLength; wave <= endWaveLength; wave++)
                    {
                        columnTitle += "T%_" + Convert.ToString(wave) + ",";
                    }
                    sw.WriteLine(columnTitle);
                }

                columnValue = barCode + ",";//barCode.Replace(",", "") + string.Format("{0:D2}", index + 1) + ",";//二维码+num
                columnValue += Para.MchName + "," + TestPoint.ToString() + ",";

                int s = 0;

                //ref-dark_
                for (i = stWaveLength; i <= endWaveLength; i++)
                {
                    t = NearestIndex(s, i, WL);
                    s = t;
                    tempCounts = WhiteRef[t] - darkRef[t];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
                    columnValue += tempCounts.ToString("F4") + ",";

                }

                //"trans-dark_"
                s = 0;
                for (i = stWaveLength; i <= endWaveLength; i++)
                {
                    t = NearestIndex(s, i, WL);
                    s = t;
                    tempCounts = MeasData[t] - darkRef[t];

                    columnValue += tempCounts.ToString("F4") + ",";
                }

                //"T%_"
                s = 0;
                for (i = stWaveLength; i <= endWaveLength; i++)
                {
                    t = NearestIndex(s, i, WL);
                    s = t;
                    columnValue += TransData[t].ToString("F4") + ",";
                }
                sw.WriteLine(columnValue);
                sw.Close();
                objFileStream.Close();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
            finally
            {
                sw.Close();
                objFileStream.Close();
            }

        }
        public int NearestIndex(int st, int waveLength, List<float> wl)
        {
            int res = -1;
            int temp = 0;
            for (int i = st; i < wl.Count; i++)
            {
                temp = (int)wl[i];

                if (temp == waveLength)
                {
                    res = i;
                    break;
                }
            }
            if (res == -1)
            {
                res = 0;
            }
            return res;
        }

        int currentTestSeq = 0;
        private void InitIndividualTestResult()
        {
            for (int i = 0; i < mod1GroupTestLabel.Length; i++)
            {
                if (!RightModOnlyCB.Checked)
                    mod1GroupTestLabel[i].Text = "Grp1_" + Convert.ToString(i + 1);
                else
                    mod1GroupTestLabel[i].Text = "";
                if (!LeftModOnlyCB.Checked)
                    mod2GroupTestLabel[i].Text = "Grp2_" + Convert.ToString(i + 1);
                else
                    mod2GroupTestLabel[i].Text = "";

                mod1GroupTestLabel[i].ForeColor = Color.Black;
                mod2GroupTestLabel[i].ForeColor = Color.Black;
            }
            for (int i = 0; i < mod1IndividualTestLabel.Length; i++)
            {
                //mod1IndividualTestLabel[i].Text = "";
                //mod2IndividualTestLabel[i].Text = "";
                mod1IndividualTestLabel[i].ForeColor = Color.Black;
                if (!RightModOnlyCB.Checked)
                    mod1IndividualTestLabel[i].Text = "NDF1_" + Convert.ToString(i + 1);
                else
                    mod1IndividualTestLabel[i].Text = "";
                mod2IndividualTestLabel[i].ForeColor = Color.Black;
                if (!LeftModOnlyCB.Checked)
                    mod2IndividualTestLabel[i].Text = "NDF2_" + Convert.ToString(i + 1);
                else
                    mod2IndividualTestLabel[i].Text = "";
                mod1NDFilterTestedID[i] = "";
                mod2NDFilterTestedID[i] = "";
            }
            currentTestSeq = 0;
            GS1ResLbl.Text = "-";
            GS2ResLbl.Text = "-";

            GS1ResLbl.Visible = !RightModOnlyCB.Checked;
            GS2ResLbl.Visible = !LeftModOnlyCB.Checked;

            GS1SerialCB.Enabled = true;
            GS2SerialCB.Enabled = true;
            GS1LV.Enabled = true;
            GS2LV.Enabled = true;

            if (!RightModOnlyCB.Checked && LeftModOnlyCB.Checked)
            {
                GS2SerialCB.Enabled = false;
                GS2LV.Enabled = false;
            }

            if (!LeftModOnlyCB.Checked && RightModOnlyCB.Checked)
            {
                GS1SerialCB.Enabled = false;
                GS1LV.Enabled = false;
            }


        }

        private void InitZAxis()
        {
            motionMgr.SetServoOn((ushort)Axislist.Mod1ZAxis, 1);
            motionMgr.SetServoOn((ushort)Axislist.Mod2ZAxis, 1);
            Thread.Sleep(100);
            motionMgr.Homing((ushort)Axislist.Mod1ZAxis, 0);
            motionMgr.Homing((ushort)Axislist.Mod2ZAxis, 0);
            Thread.Sleep(500);
            if (motionMgr.WaitHomeDone((ushort)Axislist.Mod1ZAxis) != 0)
                Para.myMain.WriteOperationinformation("Module 1 Z Axis Home Error");
            if (motionMgr.WaitHomeDone((ushort)Axislist.Mod2ZAxis) != 0)
                Para.myMain.WriteOperationinformation("Module 2 Z Axis Home Error");
            motionMgr.SetPosition((ushort)Axislist.Mod1ZAxis, 0);
            motionMgr.SetPosition((ushort)Axislist.Mod2ZAxis, 0);
        }

        private void ClearComboBoxListview()
        {
            GS1LV.Clear();
            GS1LV.Items.Clear();
            GS2LV.Clear();
            GS2LV.Items.Clear();
            GS1SerialCB.Items.Clear();
            GS2SerialCB.Items.Clear();
        }

        private void EnableCommandButton()
        {
            NDFilterCheckBtn.Enabled = true;
            LightSourceCheckBtn.Enabled = true;
        }
        private void DisableCommandButton()
        {
            NDFilterCheckBtn.Enabled = false;
            LightSourceCheckBtn.Enabled = false;
        }
        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ////////////Larry For AuditBox 20170117 
        /// </summary>
        ///        string s_FileName;



        private void MyIndexRotaryMotionCCW()
        {
            if (!Para.myMain.RotMgr.IndexRotaryMotionCCW())
            {
                MessageBox.Show("Rotary Indexing CCW Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                {
                    EnableCommandButton();
                    return;
                }
            }
            if (Para.Enb45DegTest) //is45Degree)
                if (!Para.myMain.RotMgr.IndexRotaryMotionCCW())
                {
                    MessageBox.Show("Rotary Indexing CCW Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    {
                        EnableCommandButton();
                        return;
                    }
                }
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
        }

        private void NDFilterCheckBtn_Click(object sender, EventArgs e)
        {
            GoldenSampleCalibrateBtn.BackColor = Color.Transparent;
            NDFilterCheckBtn.BackColor = Color.Lime;
            LightSourceCheckBtn.BackColor = Color.Transparent;
            this.tabControl1.SelectedTab = tabPage1;
            //groupBox1.Visible = true;
            IndividualResultGB.Visible = true;
            GrouResultGB.Visible = true;
            SkipBarcodeCB.Checked = false;
            SkipBarcodeCB.Visible = false;
            LightSourceCheckBtn.Visible = true;

            ClearComboBoxListview();
            LoadNDFilter();
            LoadGUSerial();
            InitGS1LV();
            InitGS2LV();
            InitIndividualTestResult();
            //20170322
            motionMgr.MoveTo((ushort)Axislist.Mod1ZAxis, Para.Module[0].TeachPos[0].Z);//@Brando
            motionMgr.MoveTo((ushort)Axislist.Mod2ZAxis, Para.Module[1].TeachPos[0].Z);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1ZAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2ZAxis);
            //20170322


            if ((RightModOnlyCB.Checked && LeftModOnlyCB.Checked) || (!RightModOnlyCB.Checked && !LeftModOnlyCB.Checked))
            {
                MessageBox.Show("Please chose one module only!");
                return;
            }
            DisableCommandButton();

            directory = "D:\\AuditboxNDFilterData";
            datePath = DateTime.Now.ToString("yyyyMMdd");

            string aa = "";
            if (!RightModOnlyCB.Checked)
                //if (ModIdx == 1)
                    aa =specMgr.SpecList[0].specType+ specMgr.SpecList[0].serial;    ///Para.Spectrometer1SN;
            else
                    aa = specMgr.SpecList[1].specType + specMgr.SpecList[1].serial;  // Para.Spectrometer2SN;

            s_FileName = "NDFilterResult_" + Para.MchName + "_" + datePath + "_" + DateTime.Now.ToString("HHmmss") + "_" +aa + "_" + Para.LightSourceType;



            //AnalysisResult();
            //int flage = 0;
            int count = 0;
            double[] xCenter = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] yCenter = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            HOperatorSet.ClearWindow(hWindowControl1.HalconWindow);
            HOperatorSet.ClearWindow(hWindowControl2.HalconWindow);

            //for (int count = 0; count < mod1IndividualTestLabel.Length; count++)
            if (motionMgr.ReadIOIn((ushort)InputIOlist.SafetySensor))
            {
                MessageBox.Show("安全光栅被触发", "安全报警！");
                goto NDFilterEnd;
            }
            while (true)
            {
                CalibrationBtnPressed = true;

                System.Windows.Forms.DialogResult dr = MessageBox.Show("Load (" + (count + 1).ToString() + ") Fixtures On the Tester(Yes), Previous(No), Quit(Cancel)", "ND Filter Calibration", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                //MessageBox.Show("Load (" + (count + 1).ToString() + ") Fixtures On the Tester ", "ND Filter Calibration", MessageBoxButtons.YesCancel, MessageBoxIcon.Information);
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                {

                    if (count >= mod1IndividualTestLabel.Length)
                    {
                    //    FileStream fs = new
                    //FileStream("D:\\log.txt", FileMode.Create);
                    //    StreamWriter sw = new StreamWriter(fs);
                    //    for (int i = 0; i < mod1IndividualTestLabel.Length; i++)
                    //    {
                    //        sw.WriteLine(string.Join(Environment.NewLine, xCenter[i]) + "  " + yCenter[i]);

                    //    }
                       

                    //    sw.Flush();
                    //    sw.Close();
                    //    fs.Close();
                       
                        break;
                    }
                        



                    EnableCommandButton();
                    return;
                }



                if (dr == System.Windows.Forms.DialogResult.No)
                {
                    if (count > 0)
                        count--;
                }

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    if (count >= mod1IndividualTestLabel.Length)
                    {
                        break;
                    }
                    if (motionMgr.ReadIOIn((ushort)InputIOlist.SafetySensor))
                    {
                        MessageBox.Show("安全光栅被触发", "安全报警！");
                        break;
                    }

                }


                //Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, true);
                currentTestSeq = count;
                if (!SkipBarcodeCB.Checked)
                {
                    string Mod1Barcode;
                    string Mod2Barcode;
                    if (!RightModOnlyCB.Checked)
                    {
                    Barcode1Read:
                        if (checkBoxManualBarcode.Checked)
                            Mod1Barcode = GS1SerialCB.Items[count].ToString();

                        else
                            Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        if (Mod1Barcode == "")//|| Mod1Barcode.Length < 13 || Mod1Barcode.Length > 13)
                        {
                            //MessageBox.Show("Fixtrue 1 Barcode Read Fail.", "Fixtrue 1 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (MessageBox.Show("Fixtrue 1 Barcode Read Fail.Try again?", "No Barcode Read", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                            {
                                EnableCommandButton();
                                return;
                            }
                            goto Barcode1Read;
                        }
                        //if (GS1SerialCB.Items.Contains(Mod1Barcode.Substring(0, 13)))
                        if (GS1SerialCB.Items.Contains(Mod1Barcode))
                        {
                            //GS1SerialCB.SelectedIndex = GS1SerialCB.Items.IndexOf(Mod1Barcode.Substring(0, 13));

                            GS1SerialCB.SelectedIndex = GS1SerialCB.Items.IndexOf(Mod1Barcode);
                            LoadNDF1UI();
                            Application.DoEvents();
                        }
                        //else
                        //{
                        //    MessageBox.Show("Fixtrue 1 Unit is Not Fixtrue Sample.", "Fixture Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    {
                        //        EnableCommandButton();
                        //        return;
                        //    }
                        //}
                        mod1IndividualTestLabel[count].Text = Mod1Barcode;
                    }

                    if (!LeftModOnlyCB.Checked)
                    {
                    Barcode2Read:
                        if (checkBoxManualBarcode.Checked)
                            Mod2Barcode = GS2SerialCB.Items[count].ToString();

                        else
                            Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();

                        if (Mod2Barcode == "") // || Mod2Barcode.Length < 13 || Mod2Barcode.Length > 13)
                        {
                            //MessageBox.Show("Fixtrue 2 Barcode Read Fail.", "Fixtrue 2 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //continue;
                            if (MessageBox.Show("Fixtrue 2 Barcode Read Fail.Try again?", "No Barcode Read", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                            {
                                EnableCommandButton();
                                return;
                            }
                            goto Barcode2Read;
                        }
                        if (GS2SerialCB.Items.Contains(Mod2Barcode)) //.Substring(0, 13)))
                        {
                            //GS2SerialCB.SelectedIndex = GS2SerialCB.Items.IndexOf(Mod2Barcode.Substring(0, 13));
                            GS2SerialCB.SelectedIndex = GS2SerialCB.Items.IndexOf(Mod2Barcode);
                            LoadNDF2UI();
                            Application.DoEvents();
                        }
                        else
                        {
                            MessageBox.Show("Fixtrue 2 Unit is Not Fixture Sample.", "Fixture Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        mod2IndividualTestLabel[count].Text = Mod2Barcode;
                    }
                }
                if (!RightModOnlyCB.Checked)
                {
                    stationRes.Unit1Barcode = GS1SerialCB.Items[GS1SerialCB.SelectedIndex].ToString();
                    mod1NDFilterTestedID[count] = stationRes.Unit1Barcode;
                }
                if (!LeftModOnlyCB.Checked)
                {
                    //if (GS2SerialCB.SelectedIndex < 0)
                    //{
                    //    MessageBox.Show("Fixtrue 2 Unit is Not Fixture Sample.", "Fixture Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    continue;
                    //}
                    stationRes.Unit2Barcode = GS2SerialCB.Items[GS2SerialCB.SelectedIndex].ToString();
                    mod2NDFilterTestedID[count] = stationRes.Unit2Barcode;
                }

                motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, 0);
                motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, 0);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);

                int rtIdx = Para.CurrentRotaryIndex;
                switch (rtIdx)
                {
                    case 0:
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, true);
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, true);
                        Thread.Sleep(100);
                        break;
                    case 1:
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, true);
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, true);
                        Thread.Sleep(100);
                        break;
                    case 2:
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, true);
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, true);
                        Thread.Sleep(100);
                        break;
                    case 3:
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, true);
                        motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, true);
                        Thread.Sleep(100);
                        break;
                }
                Para.myMain.WriteOperationinformation("ND Filter button T" + rtIdx.ToString());

                Thread.Sleep(500);
                if (!Para.myMain.RotMgr.IndexRotaryMotion())
                {
                    MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    {
                        EnableCommandButton();
                        return;
                    }
                }
                if (Para.Enb45DegTest) //is45Degree)
                    if (!Para.myMain.RotMgr.IndexRotaryMotion())
                    {
                        MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        {
                            EnableCommandButton();
                            return;
                        }
                    }
                Para.myMain.ClearInspectionResults();

                Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
                Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
                Thread.Sleep(1000);

                //int count1 = 0;
                int[] exposuretime = new int[9];
                exposuretime[0] = 2500;
                exposuretime[1] = 15000;
                exposuretime[2] = 20000;
                exposuretime[3] = 25000;
                exposuretime[4] = 30000;
                exposuretime[5] = 40000;
                exposuretime[6] = 50000;
                exposuretime[7] = 12500;
                exposuretime[8] = 22500;


                if (!RightModOnlyCB.Checked)
                {
                    bool OK = false;
                    for (int i = 0; i < 7; i++)
                    {
                        Para.myMain.camera1.SetExposure(exposuretime[i]);
                        //AuditBoxInspectMod2();
                        if (AuditBoxInspectMod1())
                        {

                            stationRes.mod1VisResult = Para.myMain.camera1.FindCircleCenter(Para.CaliX1, hWindowControl1); ///hWindowControl1);
                            xCenter[count] = Math.Round(stationRes.mod1VisResult.X, 1);
                            yCenter[count] = Math.Round(stationRes.mod1VisResult.Y, 1);
                            textBox5.Text = exposuretime[i].ToString();
                            OK = true;
                            break;
                        }
                    }
                    if (!OK)
                    {
                        MyIndexRotaryMotionCCW();
                        MessageBox.Show("Module 1 Inspection Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //goto NDFilterEnd;
                        continue;
                    }
                }
                if (!LeftModOnlyCB.Checked)
                {
                    bool OK = false;
                    for (int i = 0; i < 7; i++)
                    {
                        
                        Para.myMain.camera2.SetExposure(exposuretime[i]);
                        //AuditBoxInspectMod1();
                        if (AuditBoxInspectMod2())
                        {
                            stationRes.mod2VisResult = Para.myMain.camera2.FindCircleCenter(Para.CaliX2, hWindowControl2); //hWindowControl2);
                            xCenter[count] = Math.Round(stationRes.mod2VisResult.X, 1);
                            yCenter[count] = Math.Round(stationRes.mod2VisResult.Y, 1);

                            textBox6.Text = exposuretime[i].ToString();
                            OK = true;
                            break;
                        }
                    }
                        if (!OK)
                        {
                            MyIndexRotaryMotionCCW();
                            MessageBox.Show("Module 2 Inspection Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //goto NDFilterEnd;
                            continue;
                        }
                    
                }

      
                Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);


                if (Para.Enb45DegTest)  //is45Degree)
                    if (!Para.myMain.RotMgr.IndexRotaryMotion())
                    {
                        MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto NDFilterEnd;
                    }
                if (!Para.myMain.RotMgr.IndexRotaryMotion())
                {
                    MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    {
                        EnableCommandButton();
                        return;
                    }
                }
                GetDarkRef();
                Application.DoEvents();

                bool result1, result2;
                TestNDFMod(out result1, out result2);
                if (!RightModOnlyCB.Checked)
                {
                    if (result1)
                        mod1IndividualTestLabel[count].ForeColor = Color.Blue;
                    else
                        mod1IndividualTestLabel[count].ForeColor = Color.Red;
                }
                if (!LeftModOnlyCB.Checked)
                {
                    if (result2)
                        mod2IndividualTestLabel[count].ForeColor = Color.Blue;
                    else
                        mod2IndividualTestLabel[count].ForeColor = Color.Red;
                }

                motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);

                if (Para.Enb45DegTest)  //is45Degree)
                    if (!Para.myMain.RotMgr.IndexRotaryMotion())
                    {
                        MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto NDFilterEnd;
                    }
                if (Para.Enb45DegTest)  //is45Degree)
                    if (!Para.myMain.RotMgr.IndexRotaryMotion())
                    {
                        MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto NDFilterEnd;
                    }
                if (!Para.myMain.RotMgr.IndexRotaryMotion())
                {
                    MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto NDFilterEnd;
                }
                if (!Para.myMain.RotMgr.IndexRotaryMotion())
                {
                    MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto NDFilterEnd;
                }

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
                //Thread.Sleep(100);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);
                //Thread.Sleep(100);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);
                //Thread.Sleep(100);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                Para.myMain.WriteOperationinformation("ND Filter button All");
                //Thread.Sleep(500);
                DisplayNDFResult();

                count++;
            }
            AnalysisResult();


            MessageBox.Show("NDFilter Calibration Completed.", "Calibration Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        NDFilterEnd:
            EnableCommandButton();

        }
        ///// <summary>
        ///// 保存MARK点List到txt文件
        ///// </summary>
        //private void saveList()
        //{
        //    FileStream fs = new
        //        FileStream(Module.Path + "\\log.txt", FileMode.Create);
        //    StreamWriter sw = new StreamWriter(fs);
        //    for (int i = 0; i < listBox1.Items.Count; i++)
        //    {
        //        sw.WriteLine(string.Join(Environment.NewLine, listBox1.Items[i]));
        //        //sw.WriteLine(listBox1.SelectedItems);
        //    }
        //    sw.Flush();
        //    sw.Close();
        //    fs.Close();
        //}

        private void AnalysisResult()
        {
            //for (int i = 0; i < mod1NDFilterTestedID.Length; i++)
            //{
            //    mod1NDFilterTestedID[i] = Convert.ToString(GS1SerialCB.Items[mod1NDFilterTestedID.Length - 1 - i]);
            //    mod2NDFilterTestedID[i] = Convert.ToString(GS2SerialCB.Items[mod1NDFilterTestedID.Length - 1 - i]);
            //}


            //IEnumerable<string> myquery = mod1NDFilterTestedID.OrderBy(x => x);

            //for (int i = 0; i < mod1NDFilterTestedID.Length; i++)
            //{
            //    //IEnumerable<string> query  = mod1NDFilterTestedID.OrderBy(x=>x);

            //    if (!RightModOnlyCB.Checked)
            //        for (int j = 1; j < mod1NDFilterTestedID.Length - i; j++)
            //        {
            //            bool b = mod1NDFilterTestedFail[j - 1];
            //            string str = mod1NDFilterTestedID[j - 1].Substring(8, 3);
            //            string stg = mod1NDFilterTestedID[j].Substring(8, 3);
            //            string sss = mod1NDFilterTestedID[j - 1];
            //            if (Convert.ToInt16(str) > Convert.ToInt16(stg))
            //            {
            //                mod1NDFilterTestedID[j - 1] = mod1NDFilterTestedID[j];
            //                mod1NDFilterTestedID[j] = sss;
            //                mod1NDFilterTestedFail[j - 1] = mod1NDFilterTestedFail[j];
            //                mod1NDFilterTestedFail[j] = b;
            //            }
            //        }
            //    if (!LeftModOnlyCB.Checked)
            //        for (int j = 1; j < mod2NDFilterTestedID.Length - i; j++)
            //        {
            //            bool b = mod2NDFilterTestedFail[j - 1];
            //            string str = mod2NDFilterTestedID[j - 1].Substring(8, 3);
            //            string stg = mod2NDFilterTestedID[j].Substring(8, 3);
            //            string sss = mod2NDFilterTestedID[j - 1];
            //            if (Convert.ToInt16(str) > Convert.ToInt16(stg))
            //            {
            //                mod2NDFilterTestedID[j - 1] = mod2NDFilterTestedID[j];
            //                mod2NDFilterTestedID[j] = sss;
            //                mod2NDFilterTestedFail[j - 1] = mod2NDFilterTestedFail[j];
            //                mod2NDFilterTestedFail[j] = b;
            //            }
            //        }
            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    if (!RightModOnlyCB.Checked)
            //        mod1GroupTestLabel[i].Text = mod1NDFilterTestedID[3 * i].Substring(8,3);
            //    if (!LeftModOnlyCB.Checked)
            //        mod2GroupTestLabel[i].Text = mod2NDFilterTestedID[3 * i].Substring(8,3);
            //}
            fail1 = new int[3];
            fail2 = new int[3];
            for (int i = 0; i < 3; i++)
            {
                int count = 0;
                if (!RightModOnlyCB.Checked)
                {
                    for (int j = 0; j < 3; j++)
                        if (mod1NDFilterTestedFail[3 * i + j])
                            count++;
                    if (count > 1)
                    {
                        fail1[i] = 1;
                        mod1GroupTestLabel[i].Text += " Fail";
                        mod1GroupTestLabel[i].ForeColor = Color.Red;
                    }
                    else
                    {
                        fail1[i] = 0;
                        mod1GroupTestLabel[i].Text += " Pass";
                        mod1GroupTestLabel[i].ForeColor = Color.Blue;
                    }
                }
                count = 0;
                if (!LeftModOnlyCB.Checked)
                {
                    for (int j = 0; j < 3; j++)
                        if (mod2NDFilterTestedFail[3 * i + j])
                            count++;

                    if (count > 1)
                    {
                        fail2[i] = 1;

                        mod2GroupTestLabel[i].Text += " Fail";
                        mod2GroupTestLabel[i].ForeColor = Color.Red;
                    }
                    else
                    {
                        fail2[i] = 0;
                        mod2GroupTestLabel[i].Text += " Pass";
                        mod2GroupTestLabel[i].ForeColor = Color.Blue;
                    }
                }
            }

            tfail1 = 0;
            tfail2 = 0;

            for (int i = 0; i < 3; i++)
            {
                tfail1 += fail1[i];
                tfail2 += fail2[i];
            }
            if (!RightModOnlyCB.Checked)
                if (tfail1 <= 1)
                {
                    GS1ResLbl.ForeColor = Color.Blue;
                    GS1ResLbl.Text = "Pass";
                }
                else
                {
                    GS1ResLbl.ForeColor = Color.Red;
                    GS1ResLbl.Text = "Fail";
                }
            if (!LeftModOnlyCB.Checked)
                if (tfail2 <= 1)
                {
                    GS2ResLbl.ForeColor = Color.Blue;
                    GS2ResLbl.Text = "Pass";
                }
                else
                {
                    GS2ResLbl.ForeColor = Color.Red;
                    GS2ResLbl.Text = "Fail";
                }

            SaveNDFilterResultSummary();
        }
        int[] fail1 = new int[3];
        int[] fail2 = new int[3];
        int tfail1 = 0;
        int tfail2 = 0;
        public void SaveNDFilterResultSummary()
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //string path = directory + "\\" + datePath;
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //modulenum = "\\Module" + (ModIdx).ToString();
            string path = directory + modulenum;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //s_FileName = "NDFilterResult_" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string FileName = path + "\\" + s_FileName + ".csv";

            //if (!Directory.Exists(directory))
            //{
            //    Directory.CreateDirectory(directory);
            //}

            //string path = directory + "\\" + datePath;
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            ////string path = directory + "\\Module" + (ModIdx).ToString();
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //string FileName = path + "\\" + s_FileName + ".csv";

            FileStream objFileStream;
            bool bCreatedNew = false;

            if (!File.Exists(FileName))
            {
                objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                bCreatedNew = true;
            }
            else
            {
                objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
            }
            StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));

            sw.WriteLine(DateTime.Now.ToShortTimeString());
            try
            {
                string str = "";

                for (int i = 0; i < mod1NDFilterTestedID.Length; i++)
                {
                    str = "";
                    if (!RightModOnlyCB.Checked)
                    {
                        str += (i + 1).ToString() + "," + mod1NDFilterTestedID[i] + ","; // +mod1GroupTestLabel[i].Text + ",";
                        if (!mod1NDFilterTestedFail[i])
                            str += "Pass,";
                        else
                            str += "Fail,";
                    }
                    if (!LeftModOnlyCB.Checked)
                    {
                        str += (i + 1).ToString() + "," + mod2NDFilterTestedID[i] + ","; // +mod2GroupTestLabel[i].Text + ",";
                        if (!mod2NDFilterTestedFail[i])
                            str += "Pass,";
                        else  //mod2GroupTestLabel[i].Text 
                            str += "Fail,";
                    }
                    sw.WriteLine(str);
                }
                sw.WriteLine("Summary");

                for (int i = 0; i < mod1GroupTestLabel.Length; i++)
                {
                    str = "";
                    if (!RightModOnlyCB.Checked)
                    {
                        str += (i + 1).ToString() + "," + mod1GroupTestLabel[i].Text + ",";
                        //str += (pass1[i] > 0) ? "pass" : "fail";
                    }
                    if (!LeftModOnlyCB.Checked)
                    {
                        str += (i + 1).ToString() + "," + mod2GroupTestLabel[i].Text + ","; // +mod2GroupTestLabel[i].Text;
                        //str += (pass2[i] > 0) ? "pass" : "fail";
                    }
                    sw.WriteLine(str);
                }
                sw.Write("Result,");
                str = "";
                if (!RightModOnlyCB.Checked)
                    str += GS1ResLbl.Text + ",";
                if (!LeftModOnlyCB.Checked)
                    str += GS2ResLbl.Text;
                sw.WriteLine(str);
                sw.WriteLine("End");
                sw.Close();
            }
            catch (Exception)
            { };

            string ss_FileNam = "NDFilter_";
            if (!RightModOnlyCB.Checked)
                ss_FileNam += "1" + GS1ResLbl.Text + "_";
            if (!LeftModOnlyCB.Checked)
                ss_FileNam += "2" + GS2ResLbl.Text + "_";


            string aa;
            if (!RightModOnlyCB.Checked)
                aa = specMgr.SpecList[0].serial;    ///Para.Spectrometer1SN;
            else
                aa = specMgr.SpecList[1].serial;  // Para.Spectrometer2SN;

            //s_FileName = "NDFilterResult_" + Para.MchName + "_" + datePath + "_" + DateTime.Now.ToString("HHmmss") + "_" + "CAS_" + aa + "_" + Para.LightSourceType;

            ss_FileNam += Para.MchName + "_" + datePath + "_" + DateTime.Now.ToString("HHmmss") + "_" + aa + "_" + Para.LightSourceType;

            string FileName2 = path + "\\" + ss_FileNam + ".csv";



            File.Move(FileName, FileName2);
            File.Delete(FileName);
        }
        string directory = "D:\\AuditboxNDFilterData";

        string datePath = "";
        string modulenum = "";
        string s_FileName = "";


        public void SaveNDFilterData(NDFileterInfo myNDFilterData, int ModIdx)
        {
            //lock (sny_Obj)
            {
                //string s_FileName = "NDFilterData_" + DateTime.Now.ToString("yyyyMMdd");

                //string path = "D:\\AuditboxNDFilterData";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                //string path = directory + "\\" + datePath;
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                modulenum = "\\Module" + (ModIdx).ToString();
                string path = directory + modulenum;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //s_FileName = "NDFilterResult_" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                string FileName = path + "\\" + s_FileName + ".csv";

                FileStream objFileStream;
                bool bCreatedNew = false;

                if (!File.Exists(FileName))
                {
                    objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                    bCreatedNew = true;
                }
                else
                {
                    objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                }

                StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));

                string columnTitle = "";
                int i;
                string columnValue = "";
                int t = 0;
                //int waveNum = pub.m_Lambda.Value.GetLength(0);//总的波长个数
                //double tempCounts = 0;
                string strdatetime = DateTime.Now.ToString("HH:mm:ss") + ",";
                try
                {
                    //bCreatedNew = true;

                    if (bCreatedNew)
                    {
                        string aa = "";
                        if (ModIdx == 1)
                            aa = specMgr.SpecList[ModIdx-1].serial;    ///Para.Spectrometer1SN;
                        else
                            aa = specMgr.SpecList[ModIdx-1].serial;  // Para.Spectrometer2SN;  
                        columnTitle = "CG Test:" + "," + Para.SWVersion + ",,Machine Name:" + Para.MchName + ",," + "_" + "CAS_" +","+ aa+ ",_"+ Para.LightSourceType ;
                        sw.WriteLine(columnTitle);
                        columnTitle = "";

                        //写入列标题
                        columnTitle = "SerialNumber" + "," + "Wavelength(nm)" + ",";

                        for (int wave = 0; wave < myNDFilterData.data.Count; wave++)
                        {
                            columnTitle += myNDFilterData.data[wave].waveLength.ToString("F1") + ",";
                        }
                        sw.WriteLine(columnTitle);
                    }
                    sw.WriteLine("Module" + (ModIdx).ToString() + ",Time," + myNDFilterData.Name);
                    //Nominal
                    columnValue = myNDFilterData.Name + "_NominalT%,";
                    columnValue += strdatetime; // DateTime.Now.ToString("HH:mm:ss") + ",";
                    for (i = 0; i < myNDFilterData.data.Count; i++)
                    {
                        columnValue += myNDFilterData.data[i].Nominal.ToString("F3") + ",";
                    }
                    sw.WriteLine(columnValue);

                    //Min
                    columnValue = myNDFilterData.Name + "_Min%,";
                    columnValue += strdatetime; // DateTime.Now.ToString("HH:mm:ss") + ",";
                    for (i = 0; i < myNDFilterData.data.Count; i++)
                    {
                        columnValue += myNDFilterData.data[i].min.ToString("F2") + ",";
                    }
                    sw.WriteLine(columnValue);

                    //Max
                    columnValue = myNDFilterData.Name + "_Max%,";
                    columnValue += strdatetime; // DateTime.Now.ToString("HH:mm:ss") + ",";
                    for (i = 0; i < myNDFilterData.data.Count; i++)
                    {
                        columnValue += myNDFilterData.data[i].max.ToString("F2") + ",";
                    }
                    sw.WriteLine(columnValue);

                    //Measured
                    columnValue = myNDFilterData.Name + "_T%,";
                    columnValue += strdatetime; // DateTime.Now.ToString("HH:mm:ss") + ",";
                    for (i = 0; i < myNDFilterData.data.Count; i++)
                    {
                        columnValue += myNDFilterData.data[i].Measured.ToString("F2") + ",";
                    }
                    sw.WriteLine(columnValue);

                    //Diff
                    columnValue = myNDFilterData.Name + "_Diff%,";
                    columnValue += strdatetime; // DateTime.Now.ToString("HH:mm:ss") + ",";
                    for (i = 0; i < myNDFilterData.data.Count; i++)
                    {
                        columnValue += myNDFilterData.data[i].Diff.ToString("F1") + ",";
                    }
                    sw.WriteLine(columnValue);

                    columnValue = myNDFilterData.Name + "_Result,";
                    columnValue += strdatetime; // DateTime.Now.ToString("HH:mm:ss") + ",";
                    int Idx;// = GS2SerialCB.Items.IndexOf(stationRes.Unit2Barcode);
                    int failCount = 0;

                    if (ModIdx == 1)
                    {
                        Idx = GS1SerialCB.Items.IndexOf(stationRes.Unit1Barcode);
                        for (int ii = 0; ii < NDInfo1[Idx].data.Count; ii++)
                        {
                            columnValue += NDInfo1[Idx].data[ii].result + ",";
                            if (NDInfo1[Idx].data[ii].result == "Fail")
                                failCount++;
                        }
                    }
                    else
                    {
                        Idx = GS2SerialCB.Items.IndexOf(stationRes.Unit2Barcode);
                        for (int ii = 0; ii < NDInfo2[Idx].data.Count; ii++)
                        {
                            columnValue += NDInfo2[Idx].data[ii].result + ",";
                            if (NDInfo2[Idx].data[ii].result == "Fail")
                                failCount++;
                        }
                    }
                    sw.WriteLine(columnValue);


                    columnValue = myNDFilterData.Name + "_Total,";
                    columnValue += strdatetime; // DateTime.Now.ToString("HH:mm:ss") + ",";
                    sw.Write(columnValue);

                    if (failCount > 0)
                        sw.WriteLine("Fail");
                    else
                        sw.WriteLine("Pass");

                    sw.Close();
                    objFileStream.Close();
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                    objFileStream.Close();
                }
            }
        }

        bool LSCalibrationBtnPressed = false;
        private void LightSourceCheckBtn_Click(object sender, EventArgs e)
        {
            LSCalibrationBtnPressed = true;
            InitZAxis();//20170303@Brando
            if (tabControl1.SelectedTab == tabPage1)
            {
                GoldenSampleCalibrateBtn.BackColor = Color.Transparent;
                NDFilterCheckBtn.BackColor = Color.Transparent;
                LightSourceCheckBtn.BackColor = Color.Lime;
                this.tabControl1.SelectedTab = tabPage2;
                //groupBox1.Visible = false;
                IndividualResultGB.Visible = false;
                GrouResultGB.Visible = false;
                SkipBarcodeCB.Visible = false;
                //button1.BackColor = Color.Lime;
                //LightSourceCheckBtn.Visible = false;
                return;
            }

            button1.Visible = false;
            if (comboBox1.SelectedIndex == -1)
                return;
            button1.Text = "Wait";

            LightSourceCheckBtn.Enabled = false;
            LightSourceCheckBtn.Text = "Wait";
            Application.DoEvents();

            double[,] data = new double[4, 3];
            data[0, 0] = 394; data[0, 1] = 414; data[0, 2] = 404.75;
            data[1, 0] = 425; data[1, 1] = 445; data[1, 2] = 435.833;
            data[2, 0] = 535; data[2, 1] = 555; data[2, 2] = 546.074;
            data[3, 0] = 753; data[3, 1] = 773; data[3, 2] = 763.511;
            RunLogDG.Rows.Clear();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                RunLogDG.Rows.Add();
                RunLogDG.Rows[i].Cells[0].Value = (i + 1).ToString();
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    RunLogDG.Rows[i].Cells[j + 1].Value = data[i, j].ToString("F3");

                }
            }
            if (checkBoxDarkMeas.Checked)
                specMgr.MeasureDarkCurrent(comboBox1.SelectedIndex, true, true);

            stationRes.WLMod1Dark = specMgr.GetWaveLength(comboBox1.SelectedIndex);
            stationRes.WhiteRefMod1 = specMgr.GetCount(comboBox1.SelectedIndex);
            double x, y;
            double start = 400;
            double stop = 410;
            bool failed = false;
            //LSResultFailed = false;
            double criteria = Convert.ToDouble(comboBox3.Text);
            button1.Text = "Check";

            for (int i = 0; i < data.GetLength(0); i++)
            {
                start = data[i, 0];
                stop = data[i, 1];
                specMgr.GetPeak(comboBox1.SelectedIndex, out x, out y, start, stop);
                RunLogDG.Rows[i].Cells[4].Value = x.ToString("F3");
                RunLogDG.Rows[i].Cells[5].Value = (x - data[i, 2]).ToString("F3");
                if (Math.Abs(x - data[i, 2]) <= criteria)
                {
                    //RunLogDG.Rows[i].Selected = false;
                    RunLogDG.Rows[i].Cells[6].Value = "Pass";
                    if (comboBox1.SelectedIndex == 0)
                        LSResultFailed1[comboBoxLightSourceNbr.SelectedIndex] = true;
                    if (comboBox1.SelectedIndex == 1)
                        LSResultFailed2[comboBoxLightSourceNbr.SelectedIndex] = true;
                }
                else
                {
                    //RunLogDG.Rows[i].Selected = true;
                    RunLogDG.Rows[i].Cells[6].Value = "Fail";
                    failed = true;
                    //if (comboBox1.SelectedIndex==0)
                    //    LSResultFailed1[comboBoxLightSourceNbr.SelectedIndex] = true;
                    //if (comboBox1.SelectedIndex == 1)
                    //    LSResultFailed2[comboBoxLightSourceNbr.SelectedIndex] = true;
                }
            }
            if (failed)
                if (comboBox1.SelectedIndex == 0)
                {
                    GS1ResLbl.Text = "Fail";
                    GS1ResLbl.ForeColor = Color.Red;
                }
                else
                {
                    GS2ResLbl.Text = "Fail";
                    GS2ResLbl.ForeColor = Color.Red;
                }

            else
                if (comboBox1.SelectedIndex == 0)
                {
                    GS1ResLbl.Text = "Pass";
                    GS1ResLbl.ForeColor = Color.Lime;
                }
                else
                {
                    GS2ResLbl.Text = "Pass";
                    GS2ResLbl.ForeColor = Color.Lime;
                }
            SaveLightSourceResult("D:\\AuditBoxLightSourceData", "LightSource", comboBox1.SelectedIndex + 1, failed, stationRes.WLMod1Dark, stationRes.WhiteRefMod1);
            float max = float.MinValue;
            for (int i = 0; i < stationRes.WLMod1Dark.Count; i++)
                if (stationRes.WhiteRefMod1[i] > max)
                    max = stationRes.WhiteRefMod1[i];

            if (max < 10)
                max += 1;
            else if (max < 100)
                max += 10;
            else
                max += 100;
            scale = (int)Math.Ceiling(max);
            SpectChart.ChartAreas["ChartArea1"].AxisY.Maximum = scale;
            SpectChart.ChartAreas["ChartArea1"].AxisY.Interval = scale / 5;
            UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.WhiteRefMod1, true);
            LightSourceCheckBtn.Enabled = true;
            LightSourceCheckBtn.Text = "Light Source Check";
        }

        public void UpdateMod1Chart(List<float> wl, List<float> cnt, bool isRatio)
        {
            Action ac = new Action(() =>
            {
                if (isRatio)
                    InitMod1TransChart();
                else
                    InitMod1TransChart();  // /InitMod1Chart(cnt.Max() + 1);

                UpdateMod1Chart(wl, cnt);
            });
            SpectChart.BeginInvoke(ac);
        }
        private void InitMod1TransChart()//初始化Chart函数
        {
            SpectChart.Series["Series1"].Points.Clear();
            SpectChart.Series[0].Color = Color.Green;//曲线颜色
            //chart3.Series[0].Name=
            SpectChart.Legends[0].Position.X = 10; //标题位置
            SpectChart.Legends[0].Position.Y = 2;
            //  
            SpectChart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            SpectChart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            SpectChart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔

            SpectChart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            SpectChart.ChartAreas["ChartArea1"].AxisY.Maximum = 100; // 62000;//最大刻度
            SpectChart.ChartAreas["ChartArea1"].AxisY.Interval = 10;  // 100;//刻度间隔

            //设置坐标轴名称
            SpectChart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            SpectChart.ChartAreas["ChartArea1"].AxisY.Title = "Count(%)";// "数值";

            //设置网格的颜色
            SpectChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            SpectChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            SpectChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
        }
        private void UpdateMod1Chart(List<float> myWL, List<float> myCnt)
        {
            float max = float.MinValue;
            for (int i = 0; i < myWL.Count; i++)
            {
                if (myCnt[i] > max)
                    max = myCnt[i];
            }


            for (int i = 0; i < myWL.Count; i++)
            {
                if (myCnt[i] < 0)
                    SpectChart.Series["Series1"].Points.AddXY(myWL[i], 0);
                else
                    //SpectChart.Series["Series1"].Points.AddXY(myWL[i], myCnt[i]);
                    SpectChart.Series["Series1"].Points.AddXY(myWL[i], myCnt[i] / max * 100);
            }
            SpectChart.Series["Series1"].ChartType = SeriesChartType.FastLine;
        }
        public int scale = 1000;
        private void button1_Click(object sender, EventArgs e)
        {
            ////if (comboBox1.SelectedIndex == -1)
            ////    return;
            ////button1.Text = "Wait";
            ////Application.DoEvents();

            ////double[,] data = new double[4, 3];
            ////data[0, 0] = 394; data[0, 1] = 414; data[0, 2] = 404.636;
            ////data[1, 0] = 425; data[1, 1] = 445; data[1, 2] = 435.833;
            ////data[2, 0] = 535; data[2, 1] = 555; data[2, 2] = 546.074;
            ////data[3, 0] = 753; data[3, 1] = 773; data[3, 2] = 763.511;

            //////data[0, 0] = 425; data[0, 1] = 445; data[0, 2] = 435.833;
            //////data[1, 0] = 535; data[1, 1] = 555; data[1, 2] = 546.074;
            //////data[2, 0] = 753; data[2, 1] = 773; data[2, 2] = 763.511;
            //////data[3, 0] = 801; data[3, 1] = 821; data[3, 2] = 811.531;

            //////data[0, 0] = 430; data[0, 1] = 440; data[0, 2] = 435.833;
            //////data[1, 0] = 541; data[1, 1] = 551; data[1, 2] = 546.074;
            //////data[2, 0] = 758; data[2, 1] = 768; data[2, 2] = 763.511;
            //////data[3, 0] = 806; data[3, 1] = 816; data[3, 2] = 811.531;


            ////RunLogDG.Rows.Clear();
            ////for (int i = 0; i < data.GetLength(0); i++)
            ////{
            ////    RunLogDG.Rows.Add();
            ////    RunLogDG.Rows[i].Cells[0].Value = (i + 1).ToString();
            ////    for (int j = 0; j < data.GetLength(1); j++)
            ////    {
            ////        RunLogDG.Rows[i].Cells[j + 1].Value = data[i, j].ToString("F3");

            ////    }
            ////}
            ////if(checkBoxDarkMeas.Checked)
            ////    specMgr.MeasureDarkCurrent(comboBox1.SelectedIndex, true,true);

            ////stationRes.WLMod1Dark = specMgr.GetWaveLength(comboBox1.SelectedIndex);
            ////stationRes.WhiteRefMod1 = specMgr.GetCount(comboBox1.SelectedIndex);
            ////double x, y;
            ////double start = 400;
            ////double stop = 410;
            ////bool failed = false;
            ////double criteria = Convert.ToDouble(comboBox3.Text);
            ////button1.Text = "Check";

            ////for (int i = 0; i < data.GetLength(0); i++)
            ////{
            ////    start = data[i, 0];
            ////    stop = data[i, 1];
            ////    specMgr.GetPeak(comboBox1.SelectedIndex, out x, out y, start, stop);
            ////    RunLogDG.Rows[i].Cells[4].Value = x.ToString("F3");
            ////    RunLogDG.Rows[i].Cells[5].Value = (x - data[i, 2]).ToString("F3");
            ////    if (Math.Abs(x - data[i, 2]) <= criteria)
            ////    {
            ////        RunLogDG.Rows[i].Selected = false;
            ////        RunLogDG.Rows[i].Cells[6].Value = "Pass";
            ////    }
            ////    else
            ////    {
            ////        RunLogDG.Rows[i].Selected = true;
            ////        RunLogDG.Rows[i].Cells[6].Value = "Fail";
            ////        failed = true;
            ////    }
            ////}
            ////if (failed)
            ////    if (comboBox1.SelectedIndex == 0)
            ////    {
            ////        GS1ResLbl.Text = "Fail";
            ////        GS1ResLbl.ForeColor = Color.Red;
            ////    }
            ////    else
            ////    {
            ////        GS2ResLbl.Text = "Fail";
            ////        GS2ResLbl.ForeColor = Color.Red;
            ////    }

            ////else
            ////    if (comboBox1.SelectedIndex == 0)
            ////    {
            ////        GS1ResLbl.Text = "Pass";
            ////        GS1ResLbl.ForeColor = Color.Lime;
            ////    }
            ////    else
            ////    {
            ////        GS2ResLbl.Text = "Pass";
            ////        GS2ResLbl.ForeColor = Color.Lime;
            ////    }
            ////SaveLightSourceResult("D:\\AuditBoxLightSourceData", "Light Source", comboBox1.SelectedIndex + 1, failed, stationRes.WLMod1Dark, stationRes.WhiteRefMod1);
            ////float max = float.MinValue;
            ////for (int i = 0; i < stationRes.WLMod1Dark.Count; i++)
            ////    if (stationRes.WhiteRefMod1[i] > max)
            ////        max = stationRes.WhiteRefMod1[i];

            ////if (max < 10)
            ////    max += 1;
            ////else if (max < 100)
            ////    max += 10;
            ////else
            ////    max += 100;
            ////scale = (int)Math.Ceiling(max );
            ////SpectChart.ChartAreas["ChartArea1"].AxisY.Maximum = scale;
            ////SpectChart.ChartAreas["ChartArea1"].AxisY.Interval = scale / 5;
            ////UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.WhiteRefMod1, true);
            ////Application.DoEvents();

            //SaveLightSourceRawData("D:\\AuditBoxLightSourceData", "Light Source", comboBox1.SelectedIndex + 1, stationRes.WLMod1Dark, stationRes.WhiteRefMod1);
        }
        public void SaveLightSourceResult(string path, string barCode, int ModuleIndex, bool fail, List<float> WLDark, List<float> MeasData)
        {
            //lock (sny_Obj)
            {
                string s_FileName = barCode + "_" + (ModuleIndex).ToString();
                if (fail)
                    s_FileName += "Fail";
                else
                    s_FileName += "Pass";
                s_FileName += "_" + Para.MchName + DateTime.Now.ToString("_yyyyMMdd_HHmmss");


                string bb = "";
                if (comboBox1.SelectedIndex == 0)
                //if (ModIdx == 1)
                {
                    bb = specMgr.SpecList[0].specType + specMgr.SpecList[0].serial;    ///Para.Spectrometer1SN;
                    s_FileName += "_" + bb + "_" + Para.LightSourceType;    ///Para.Spectrometer1SN;
                }                                                                   ///}
                else
                {
                    bb = specMgr.SpecList[1].specType + specMgr.SpecList[1].serial;    // Para.Spectrometer2SN;
                    s_FileName += "_" + bb + "_" + Para.LightSourceType;
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //path = "D:\\DailyGoldenData\\Module" + (ModuleIndex).ToString();
                path += "\\Module" + (ModuleIndex).ToString();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string FileName = path + "\\" + s_FileName + ".csv";

                FileStream objFileStream;
                bool bCreatedNew = false;

                if (!File.Exists(FileName))
                {
                    objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                    bCreatedNew = true;
                }
                else
                {
                    objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                }
                StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
                string columnTitle = "";
                int i;
                string columnValue = "";

                try
                {
                    if (bCreatedNew)
                    {
                        columnTitle = "CG Test:" + "," + Para.SWVersion + ",,Machine Name:" + Para.MchName + "," + bb + "," + Para.LightSourceType + ",";
                        sw.WriteLine(columnTitle);

                        columnTitle = "Criteria:" + "," + comboBox2.Text + ",(nm)" + ",,Light Source #:" + comboBoxLightSourceNbr.Text;
                        sw.WriteLine(columnTitle);

                        sw.WriteLine("Module" + (ModuleIndex).ToString());

                        columnTitle = "";

                        //写入列标题
                        for (i = 0; i < RunLogDG.ColumnCount; i++)
                            columnTitle += RunLogDG.Columns[i].HeaderText + ",";
                        sw.WriteLine(columnTitle);
                    }

                    for (i = 0; i < RunLogDG.Rows.Count; i++)
                    {
                        columnValue = "";
                        for (int j = 0; j < RunLogDG.Columns.Count; j++)
                            columnValue += RunLogDG.Rows[i].Cells[j].Value + ",";

                        sw.WriteLine(columnValue);

                    }
                    columnValue = "Result:,";
                    if (fail)
                        columnValue += " Fail";
                    else
                        columnValue += " Pass";
                    sw.WriteLine(columnValue);

                    sw.WriteLine("");
                    //写入列标题
                    columnTitle = "Seq" + "," + "WaveLength(nm)" + "," + "Count" + ",";
                    sw.WriteLine(columnTitle);
                    for (i = 0; i < WLDark.Count; i++)
                    {
                        columnValue = (i + 1).ToString() + "," + WLDark[i] + "," + MeasData[i]; //MeasData[t] - darkRef[t];

                        sw.WriteLine(columnValue);
                    }
                    sw.WriteLine("End");

                    sw.Close();
                    objFileStream.Close();
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                    objFileStream.Close();
                }
            }
        }
        public void SaveLightSourceRawData(string path, string barCode, int ModuleIndex, List<float> WLDark, List<float> MeasData)
        {
            //lock (sny_Obj)
            {
                string s_FileName = barCode + "RawData_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + (ModuleIndex).ToString();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //path = "D:\\DailyGoldenData\\Module" + (ModuleIndex).ToString();
                //path += "\\Module" + (ModuleIndex).ToString();
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                string FileName = path + "\\" + s_FileName + ".csv";

                FileStream objFileStream;
                bool bCreatedNew = false;

                if (!File.Exists(FileName))
                {
                    objFileStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                    bCreatedNew = true;
                }
                else
                {
                    objFileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                }
                StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
                string columnTitle = "";
                int i;
                string columnValue = "";


                try
                {
                    //bCreatedNew = true;
                    if (bCreatedNew)
                    {
                        columnTitle = "CG Test:" + "," + Para.SWVersion + ",,Machine Name:" + Para.MchName;
                        sw.WriteLine(columnTitle);
                        columnTitle = "";

                        //写入列标题
                        columnTitle = "Seq" + "," + "WaveLength(nm)" + "," + "Count" + ",";
                        sw.WriteLine(columnTitle);
                    }

                    //for (i = stWaveLength; i <= endWaveLength; i++)
                    for (i = 0; i < WLDark.Count; i++)
                    {
                        columnValue = i.ToString() + "," + WLDark[i] + "," + MeasData[i]; //MeasData[t] - darkRef[t];

                        sw.WriteLine(columnValue);
                    }

                    sw.Close();
                    objFileStream.Close();
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                    objFileStream.Close();
                }
            }
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void LeftModOnlyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (LeftModOnlyCB.Checked && RightModOnlyCB.Checked)
                return;
            InitIndividualTestResult();
        }

        private void RightModOnlyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (LeftModOnlyCB.Checked && RightModOnlyCB.Checked)
                return;
            InitIndividualTestResult();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            scale = scale / 2;
            SpectChart.ChartAreas["ChartArea1"].AxisY.Maximum = scale;
            SpectChart.ChartAreas["ChartArea1"].AxisY.Interval = scale / 5;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            scale = scale * 2;
            SpectChart.ChartAreas["ChartArea1"].AxisY.Maximum = scale;
            SpectChart.ChartAreas["ChartArea1"].AxisY.Interval = scale / 5;
        }

        bool CalibrationBtnPressed = false;
        private void AuditBoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LSCalibrationBtnPressed)
            {
                int temp1 = 0; 
                int temp2 = 0;
                if (LSResultFailed1[0])
                    temp1++;
                if (LSResultFailed1[1])
                    temp1++;
                if (LSResultFailed1[2])
                    temp1++;

                if (LSResultFailed2[0])
                    temp2++;
                if (LSResultFailed2[1])
                    temp2++;
                if (LSResultFailed2[2])
                    temp2++;
                if (temp1 >= 2 && temp2 >= 2)
                {
                    FileOperation.SaveData(Para.MchConfigFileName, "LSContinueRunTime", "Time", DateTime.Now.ToString());
                    Para.LSSystemRunTime = DateTime.Now;
                    MessageBox.Show("LS successed,please reset machine");
                    Para.myMain.OnlyHomeEnb();
                }
                else
                {
                    MessageBox.Show("LS not successed");
                    Para.myMain.AllDisabled();

                }
            }
            else
            {
                MessageBox.Show("LS did not excute,Please reset machine");
                Para.myMain.OnlyHomeEnb();
            }
            LSCalibrationBtnPressed = false;
            //FileOperation.SaveData(Para.MchConfigFileName, "ContinueRunTime", "Time", DateTime.Now.ToString());
            //MessageBox.Show("AuditBox Completed ! Please Click Home Button To Reset Machine");
            //motionMgr.MoveTo((ushort)Axislist.Mod1ZAxis, Para.Module[0].TeachPos[0].Z);//@Brando
            //motionMgr.MoveTo((ushort)Axislist.Mod2ZAxis, Para.Module[1].TeachPos[0].Z);
            //motionMgr.WaitAxisStop((ushort)Axislist.Mod1ZAxis);
            //motionMgr.WaitAxisStop((ushort)Axislist.Mod2ZAxis);
            if (CalibrationBtnPressed)
            {
                //if (mod1NDFilterTestedFail[0] && mod1NDFilterTestedFail[1] && mod1NDFilterTestedFail[2] && mod1NDFilterTestedFail[3] && mod1NDFilterTestedFail[4] && mod1NDFilterTestedFail[5] && mod1NDFilterTestedFail[6] && mod1NDFilterTestedFail[7] && mod1NDFilterTestedFail[8]
                //    && mod2NDFilterTestedFail[0] && mod2NDFilterTestedFail[1] && mod2NDFilterTestedFail[2] && mod2NDFilterTestedFail[3] && mod2NDFilterTestedFail[4] && mod2NDFilterTestedFail[5] && mod2NDFilterTestedFail[6] && mod2NDFilterTestedFail[7] && mod2NDFilterTestedFail[8])

                //{
                //    FileOperation.SaveData(Para.MchConfigFileName, "NDContinueRunTime", "Time", DateTime.Now.ToString());
                //    Para.NDSystemRunTime = DateTime.Now;
                //    MessageBox.Show("ND successed,please reset machine");
                //    Para.myMain.OnlyHomeEnb();
                //}
                //else
                //{
                //    MessageBox.Show("ND not successed");
                //    Para.myMain.AllDisabled();
                //}
                bool IsLeftPass1 = false, IsLeftPass2 = false, IsLeftPass3 = false, IsRightPass1 = false, IsRightPass2 = false,IsRightPass3 = false;
                //if (mod1NDFilterTestedResult[0] && mod1NDFilterTestedResult[1] && mod1NDFilterTestedResult[2] && mod1NDFilterTestedResult[3] && mod1NDFilterTestedResult[4] && mod1NDFilterTestedResult[5])
                //{
                //    IsLeftPass = true;
                //}
                //if (mod1NDFilterTestedResult[0] && mod1NDFilterTestedResult[1] && mod1NDFilterTestedResult[2] && mod1NDFilterTestedResult[6] && mod1NDFilterTestedResult[7] && mod1NDFilterTestedResult[8])
                //{
                //    IsLeftPass = true;
                //}
                //if (mod1NDFilterTestedResult[3] && mod1NDFilterTestedResult[4] && mod1NDFilterTestedResult[5] && mod1NDFilterTestedResult[6] && mod1NDFilterTestedResult[7] && mod1NDFilterTestedResult[8])
                //{
                //    IsLeftPass = true;
                //}
                if (mod1NDFilterTestedResult[0] && mod1NDFilterTestedResult[1])
                {
                    IsLeftPass1 = true;
                }
                if (mod1NDFilterTestedResult[0] && mod1NDFilterTestedResult[2])
                {
                    IsLeftPass1 = true;
                }
                if (mod1NDFilterTestedResult[1] && mod1NDFilterTestedResult[2])
                {
                    IsLeftPass1 = true;
                }


                if (mod1NDFilterTestedResult[3] && mod1NDFilterTestedResult[4])
                {
                    IsLeftPass2 = true;
                }
                if (mod1NDFilterTestedResult[3] && mod1NDFilterTestedResult[5])
                {
                    IsLeftPass2 = true;
                }
                if (mod1NDFilterTestedResult[4] && mod1NDFilterTestedResult[5])
                {
                    IsLeftPass2 = true;
                }

                if (mod1NDFilterTestedResult[6] && mod1NDFilterTestedResult[7])
                {
                    IsLeftPass3 = true;
                }
                if (mod1NDFilterTestedResult[6] && mod1NDFilterTestedResult[8])
                {
                    IsLeftPass3 = true;
                }
                if (mod1NDFilterTestedResult[7] && mod1NDFilterTestedResult[8])
                {
                    IsLeftPass3 = true;
                }          

                //if (mod2NDFilterTestedResult[0] && mod2NDFilterTestedResult[1] && mod2NDFilterTestedResult[2] && mod2NDFilterTestedResult[3] && mod2NDFilterTestedResult[4] && mod2NDFilterTestedResult[5])
                //{
                //    IsRightPass = true;
                //}
                //if (mod2NDFilterTestedResult[0] && mod2NDFilterTestedResult[1] && mod2NDFilterTestedResult[2] && mod2NDFilterTestedResult[6] && mod2NDFilterTestedResult[7] && mod2NDFilterTestedResult[8])
                //{
                //    IsRightPass = true;
                //}
                //if (mod2NDFilterTestedResult[3] && mod2NDFilterTestedResult[4] && mod2NDFilterTestedResult[5] && mod2NDFilterTestedResult[6] && mod2NDFilterTestedResult[7] && mod2NDFilterTestedResult[8])
                //{
                //    IsRightPass = true;
                //}

                if (mod2NDFilterTestedResult[0] && mod2NDFilterTestedResult[1])
                {
                    IsRightPass1 = true;
                }
                if (mod2NDFilterTestedResult[0] && mod2NDFilterTestedResult[2])
                {
                    IsRightPass1 = true;
                }
                if (mod2NDFilterTestedResult[1] && mod2NDFilterTestedResult[2])
                {
                    IsRightPass1 = true;
                }

                if (mod2NDFilterTestedResult[3] && mod2NDFilterTestedResult[4])
                {
                    IsRightPass2 = true;
                }
                if (mod2NDFilterTestedResult[3] && mod2NDFilterTestedResult[5])
                {
                    IsRightPass2 = true;
                }
                if (mod2NDFilterTestedResult[4] && mod2NDFilterTestedResult[5])
                {
                    IsRightPass2 = true;
                }

                if (mod2NDFilterTestedResult[6] && mod2NDFilterTestedResult[7])
                {
                    IsRightPass3 = true;
                }
                if (mod2NDFilterTestedResult[6] && mod2NDFilterTestedResult[8])
                {
                    IsRightPass3 = true;
                }
                if (mod2NDFilterTestedResult[7] && mod2NDFilterTestedResult[8])
                {
                    IsRightPass3 = true;
                }

                int templeft = 0;
                if (IsLeftPass1 == true)
                    templeft++;
                if (IsLeftPass2 == true)
                    templeft++;
                if (IsLeftPass3 == true)
                    templeft++;

                int tempright = 0;
                if (IsRightPass1 == true)
                    tempright++;
                if (IsRightPass2 == true)
                    tempright++;
                if (IsRightPass3 == true)
                    tempright++;

                if ((templeft >= 2) && (tempright>=2))
                {
                    FileOperation.SaveData(Para.MchConfigFileName, "NDContinueRunTime", "Time", DateTime.Now.ToString());
                    Para.NDSystemRunTime = DateTime.Now;
                    MessageBox.Show("ND successed,please reset machine");
                    Para.myMain.OnlyHomeEnb();
                }
                else
                {
                    MessageBox.Show("ND not successed");
                    Para.myMain.AllDisabled();
                }          
            }
            else
            {
                MessageBox.Show("ND did not excute,Please reset machine");
                Para.myMain.OnlyHomeEnb();
            }
            CalibrationBtnPressed = false;
        }

        private bool AuditBoxInspectMod1()
        {
            //Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
            //Thread.Sleep(1000);

            for (int c = 0; c < 3; c++)
            {
                bool GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Para.myMain.camera1.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }

                if (!GrabPass)
                {
                    //Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                    //Thread.Sleep(200);
                    return false;
                }

                stationRes.mod1VisResult = Para.myMain.camera1.FindCircleCenter(Para.CaliX1, hWindowControl1);

                textBox1.Text = stationRes.mod1VisResult.X.ToString("#0.000");
                textBox2.Text = stationRes.mod1VisResult.Y.ToString("#0.000");

                if (stationRes.mod1VisResult.Found)
                {
                    break;
                }
            }

            //Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
            //Thread.Sleep(200);

            if (!stationRes.mod1VisResult.Found)
                return false;

            Para.myMain.DisplayCam1Result(stationRes.mod1VisResult);

            return true;
        }
        private bool AuditBoxInspectMod2()
        {
            //Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
            //Thread.Sleep(1000);

            for (int c = 0; c < 3; c++)
            {
                bool GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Para.myMain.camera2.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }

                if (!GrabPass)
                {
                    //Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                    //Thread.Sleep(200);
                    return false;
                }

                stationRes.mod2VisResult = Para.myMain.camera2.FindCircleCenter(Para.CaliX2, hWindowControl2);

                textBox3.Text = stationRes.mod2VisResult.X.ToString("#0.000");
                textBox4.Text = stationRes.mod2VisResult.Y.ToString("#0.000");

                if (stationRes.mod2VisResult.Found)
                {
                    break;
                }
            }

            //Para.myMain.motionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
            //Thread.Sleep(200);

            if (!stationRes.mod2VisResult.Found)
                return false;

            Para.myMain.DisplayCam2Result(stationRes.mod2VisResult);

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AuditBoxInspectMod1();
            double unitCtrOffX = (((Para.myMain.camera1.ImageWidth / 2) - stationRes.mod1VisResult.X) * Para.myMain.camera1.CaliValue.X) + Para.Module[0].CamToOriginOffset.X;
            double unitCtrOffY = (((Para.myMain.camera1.ImageHeight / 2) - stationRes.mod1VisResult.Y) * Para.myMain.camera1.CaliValue.Y) + Para.Module[0].CamToOriginOffset.Y;
            double a = Para.myMain.camera1.ImageWidth;
            double b = stationRes.mod1VisResult.X;
            double c = Para.myMain.camera1.CaliValue.X;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            AuditBoxInspectMod2();
            //Unit Ctr Offset In MM
            double unitCtrOffX2 = (((Para.myMain.camera2.ImageWidth / 2) - stationRes.mod2VisResult.X) * Para.myMain.camera2.CaliValue.X) + Para.Module[1].CamToOriginOffset.X;
            double unitCtrOffY2 = (((Para.myMain.camera2.ImageHeight / 2) - stationRes.mod2VisResult.Y) * Para.myMain.camera2.CaliValue.Y) + Para.Module[1].CamToOriginOffset.Y;
            double a = Para.myMain.camera2.ImageWidth;
            double b = stationRes.mod2VisResult.X;
            double c = Para.myMain.camera2.CaliValue.X;
        }
    }
}
