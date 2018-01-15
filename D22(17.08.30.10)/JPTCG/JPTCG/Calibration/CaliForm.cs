using Common;
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

namespace JPTCG.Calibration
{
    public partial class CaliForm : Form
    {
        string[] Title1 = new string[]{"T::TP1_T_PCT_400" ,"T::TP1_T_PCT_410" , "T::TP1_T_PCT_420" ,"T::TP1_T_PCT_450" ,"T::TP1_T_PCT_500" ,"T::TP1_T_PCT_550" , "T::TP1_T_PCT_600" ,
                                           "T::TP1_T_PCT_650" ,"T::TP1_T_PCT_700", "T::TP1_T_PCT_750" ,"T::TP1_T_PCT_800" ,"T::TP1_T_PCT_850" , "T::TP1_T_PCT_900" ,"T::TP1_T_PCT_950" ,
                                           "T::TP1_T_PCT_1000" ,"T::TP1_T_PCT_1050" ,"T::TP1_T_PCT_1100"};

        string[] Title2 = new string[] {"T::TP2_T_PCT_400" ,"T::TP2_T_PCT_410" , "T::TP2_T_PCT_420" ,"T::TP2_T_PCT_450" ,"T::TP2_T_PCT_500" ,"T::TP2_T_PCT_550" , "T::TP2_T_PCT_600" ,
                                           "T::TP2_T_PCT_650" ,"T::TP2_T_PCT_700", "T::TP2_T_PCT_750" ,"T::TP2_T_PCT_800" ,"T::TP2_T_PCT_850" , "T::TP2_T_PCT_900" ,"T::TP2_T_PCT_950" ,
                                           "T::TP2_T_PCT_1000" ,"T::TP2_T_PCT_1050" ,"T::TP2_T_PCT_1100"};

        string[] Title3 = new string[]{"T::TP3_T_PCT_400" ,"T::TP3_T_PCT_410" , "T::TP3_T_PCT_420" ,"T::TP3_T_PCT_450" ,"T::TP3_T_PCT_500" ,"T::TP3_T_PCT_550" , "T::TP3_T_PCT_600" ,
                                           "T::TP3_T_PCT_650" ,"T::TP3_T_PCT_700", "T::TP3_T_PCT_750" ,"T::TP3_T_PCT_800" ,"T::TP3_T_PCT_850" , "T::TP3_PCT_900" ,"T::TP3_T_PCT_950" ,
                                           "T::TP3_T_PCT_1000" ,"T::TP3_T_PCT_1050" ,"T::TP3_T_PCT_1100"};

        string[] Title4 = new string[]{"T::TP1_T_TLIT_400" ,"T::TP1_T_TLIT_410" , "T::TP1_T_TLIT_420" ,"T::TP1_T_TLIT_450" ,"T::TP1_T_TLIT_500" ,"T::TP1_T_TLIT_550" , "T::TP1_T_TLIT_600" ,
                                           "T::TP1_T_TLIT_650" ,"T::TP1_T_TLIT_700", "T::TP1_T_TLIT_750" ,"T::TP1_T_TLIT_800" ,"T::TP1_T_TLIT_850" , "T::TP1_T_TLIT_900" ,"T::TP1_T_TLIT_950" ,
                                           "T::TP1_T_TLIT_1000" ,"T::TP1_T_TLIT_1050" ,"T::TP1_T_TLIT_1100"};

        string[] Title5 = new string[] {"T::TP2_T_TLIT_400" ,"T::TP2_T_TLIT_410" , "T::TP2_T_TLIT_420" ,"T::TP2_T_TLIT_450" ,"T::TP2_T_TLIT_500" ,"T::TP2_T_TLIT_550" , "T::TP2_T_TLIT_600" ,
                                           "T::TP2_T_TLIT_650" ,"T::TP2_T_TLIT_700", "T::TP2_T_TLIT_750" ,"T::TP2_T_TLIT_800" ,"T::TP2_T_TLIT_850" , "T::TP2_T_TLIT_900" ,"T::TP2_T_TLIT_950" ,
                                           "T::TP2_T_TLIT_1000" ,"T::TP2_T_TLIT_1050" ,"T::TP2_T_TLIT_1100"};

        string[] Title6 = new string[]{"T::TP3_T_TLIT_400" ,"T::TP3_T_TLIT_410" , "T::TP3_T_TLIT_420" ,"T::TP3_T_TLIT_450" ,"T::TP3_T_TLIT_500" ,"T::TP3_T_TLIT_550" , "T::TP3_T_TLIT_600" ,
                                           "T::TP3_T_TLIT_650" ,"T::TP3_T_TLIT_700", "T::TP3_T_TLIT_750" ,"T::TP3_T_TLIT_800" ,"T::TP3_T_TLIT_850" , "T::TP3_TLIT_900" ,"T::TP3_T_TLIT_950" ,
                                           "T::TP3_T_TLIT_1000" ,"T::TP3_T_TLIT_1050" ,"T::TP3_T_TLIT_1100"};

        string[] Title20 = new string[]{"T::TLIT_AVERAGE_RATIO_400" ,"T::TLIT_AVERAGE_RATIO_410" , "T::TLIT_AVERAGE_RATIO_420" ,"T::TLIT_AVERAGE_RATIO_450" ,"T::TLIT_AVERAGE_RATIO_500" ,"T::TLIT_AVERAGE_RATIO_550" , "T::TLIT_AVERAGE_RATIO_600" ,
                                           "T::TLIT_AVERAGE_RATIO_650" ,"T::TLIT_AVERAGE_RATIO_700", "T::TLIT_AVERAGE_RATIO_750" ,"T::TLIT_AVERAGE_RATIO_800" ,"T::TLIT_AVERAGE_RATIO_850" , "T::TLIT_AVERAGE_RATIO_900" ,"T::TLIT_AVERAGE_RATIO_950" ,
                                           "T::TLIT_AVERAGE_RATIO_1000" ,"T::TLIT_AVERAGE_RATIO_1050" ,"T::TLIT_AVERAGE_RATIO_1100"};


        string[] Title21 = new string[]{"T::TP_AVERAGE_RATIO_PCT_400" ,"T::TP_AVERAGE_RATIO_PCT_410" , "T::TP_AVERAGE_RATIO_PCT_420" ,"T::TP_AVERAGE_RATIO_PCT_450" ,"T::TP_AVERAGE_RATIO_PCT_500" ,"T::TP_AVERAGE_RATIO_PCT_550" , "T::TP_AVERAGE_RATIO_PCT_600" ,
                                           "T::TP_AVERAGE_RATIO_PCT_650" ,"T::TP_AVERAGE_RATIO_PCT_700", "T::TP_AVERAGE_RATIO_PCT_750" ,"T::TP_AVERAGE_RATIO_PCT_800" ,"T::TP_AVERAGE_RATIO_PCT_850" , "T::TP_AVERAGE_RATIO_PCT_900" ,"T::TP_AVERAGE_RATIO_PCT_950" ,
                                           "T::TP_AVERAGE_RATIO_PCT_1000" ,"T::TP_AVERAGE_RATIO_PCT_1050" ,"T::TP_AVERAGE_RATIO_PCT_1100"};


        string[] Title30 = new string[]{"T::TLIT_AVERAGE_400" ,"T::_TLIT_AVERAGE_410" , "T::TLIT_AVERAGE_420" ,"T::TLIT_AVERAGE_450" ,"T::TLIT_AVERAGE_500" ,"T::TLIT_AVERAGE_550" , "T::TLIT_AVERAGE_600" ,
                                           "T::TLIT_AVERAGE_650" ,"T::TLIT_AVERAGE_700", "T::TLIT_AVERAGE_750" ,"T::TLIT_AVERAGE_800" ,"T::TLIT_AVERAGE_850" , "T::TLIT_AVERAGE_900" ,"T::TLIT_AVERAGE_950" ,
                                           "T::TLIT_AVERAGE_1000" ,"T::TLIT_AVERAGE_1050" ,"T::TLIT_AVERAGE_1100"};


        string[] Title31 = new string[]{"T::TP_AVERAGE_PCT_400" ,"T::TP_AVERAGE_PCT_410" , "T::TP_AVERAGE_PCT_420" ,"T::TP_AVERAGE_PCT_450" ,"T::TP_AVERAGE_PCT_500" ,"T::TP_AVERAGE_PCT_550" , "T::TP_AVERAGE_PCT_600" ,
                                           "T::TP_AVERAGE_PCT_650" ,"T::TP_AVERAGE_PCT_700", "T::TP_AVERAGE_PCT_750" ,"T::TP_AVERAGE_PCT_800" ,"T::TP_AVERAGE_PCT_850" , "T::TP_AVERAGE_PCT_900" ,"T::TP_AVERAGE_PCT_950" ,
                                           "T::TP_AVERAGE_PCT_1000" ,"T::TP_AVERAGE_PCT_1050" ,"T::TP_AVERAGE_PCT_1100"};


        string[] Title_T = new string[]{"T::TP1_T_PCT_400" ,"T::TP1_T_PCT_410" , "T::TP1_T_PCT_420" ,"T::TP1_T_PCT_450" ,"T::TP1_T_PCT_500" ,"T::TP1_T_PCT_550" , "T::TP1_T_PCT_600" ,
                                           "T::TP1_T_PCT_650" ,"T::TP1_T_PCT_700", "T::TP1_T_PCT_750" ,"T::TP1_T_PCT_800" ,"T::TP1_T_PCT_850" , "T::TP1_T_PCT_900" ,"T::TP1_T_PCT_950" ,
                                           "T::TP1_T_PCT_1000" ,"T::TP1_T_PCT_1050" ,"T::TP1_T_PCT_1100",

                                       "T::TP2_T_PCT_400" ,"T::TP2_T_PCT_410" , "T::TP2_T_PCT_420" ,"T::TP2_T_PCT_450" ,"T::TP2_T_PCT_500" ,"T::TP2_T_PCT_550" , "T::TP2_T_PCT_600" ,
                                           "T::TP2_T_PCT_650" ,"T::TP2_T_PCT_700", "T::TP2_T_PCT_750" ,"T::TP2_T_PCT_800" ,"T::TP2_T_PCT_850" , "T::TP2_T_PCT_900" ,"T::TP2_T_PCT_950" ,
                                           "T::TP2_T_PCT_1000" ,"T::TP2_T_PCT_1050" ,"T::TP2_T_PCT_1100",

                                       "T::TP3_T_PCT_400" ,"T::TP3_T_PCT_410" , "T::TP3_T_PCT_420" ,"T::TP3_T_PCT_450" ,"T::TP3_T_PCT_500" ,"T::TP3_T_PCT_550" , "T::TP3_T_PCT_600" ,
                                           "T::TP3_T_PCT_650" ,"T::TP3_T_PCT_700", "T::TP3_T_PCT_750" ,"T::TP3_T_PCT_800" ,"T::TP3_T_PCT_850" , "T::TP3_PCT_900" ,"T::TP3_T_PCT_950" ,
                                           "T::TP3_T_PCT_1000" ,"T::TP3_T_PCT_1050" ,"T::TP3_T_PCT_1100"};


        SpectManager specMgr;
        DeltaMotionMgr motionMgr; //17
        string[] WLStr = new string[] { "400", "410", "420", "450", "500", "550", "600", "650", "700", "750", "800", "850", "900", "950", "1000", "1050", "1100" };
        public string errorstring1 = null;
        public string errorstring2 = null;
        string starttime = "";


        public int StationIndex = 0;
        public int ColorSelection = 0;    //8  white     3 Dark



        public CaliForm(SpectManager MySpecMgr, DeltaMotionMgr MyMotionMgr)
        {
            InitializeComponent();
            specMgr = MySpecMgr;
            motionMgr = MyMotionMgr;
        }

        private void CaliForm_Load(object sender, EventArgs e)
        {
            starttime = DateTime.Now.ToString();
            LoadGoldenSampleInformation();
            LoadGoldenSampleSpec();
            LoadGUSerial();
            InitGS1LV();
            InitGS2LV();
            GS1ResLbl.Text = "waitting";
            GS1ResLbl.ForeColor = Color.Yellow;
            GS1ResLb2.Text = "waitting";
            GS1ResLb2.ForeColor = Color.Yellow;
            GS1ResLb3.Text = "waitting";
            GS1ResLb3.ForeColor = Color.Yellow;
            GS1ResLb4.Text = "waitting";
            GS1ResLb4.ForeColor = Color.Yellow;
            GS2ResLb1.Text = "waitting";
            GS2ResLb1.ForeColor = Color.Yellow;
            GS2ResLb2.Text = "waitting";
            GS2ResLb2.ForeColor = Color.Yellow;
            GS2ResLb3.Text = "waitting";
            GS2ResLb3.ForeColor = Color.Yellow;
            GS2ResLb4.Text = "waitting";
            GS2ResLb4.ForeColor = Color.Yellow;


            GS1ResLb5.Text = "waitting";
            GS1ResLb5.ForeColor = Color.Yellow;
            GS1ResLb6.Text = "waitting";
            GS1ResLb6.ForeColor = Color.Yellow;
            GS1ResLb7.Text = "waitting";
            GS1ResLb7.ForeColor = Color.Yellow;
            GS1ResLb8.Text = "waitting";
            GS1ResLb8.ForeColor = Color.Yellow;
            GS2ResLb5.Text = "waitting";
            GS2ResLb5.ForeColor = Color.Yellow;
            GS2ResLb6.Text = "waitting";
            GS2ResLb6.ForeColor = Color.Yellow;
            GS2ResLb7.Text = "waitting";
            GS2ResLb7.ForeColor = Color.Yellow;
            GS2ResLb8.Text = "waitting";
            GS2ResLb8.ForeColor = Color.Yellow;

            if (true)    //for d22
            {
                GS1ResLbl.Text = "Pass";
                GS1ResLbl.ForeColor = Color.Lime;
                GS1ResLb2.Text = "Pass";
                GS1ResLb2.ForeColor = Color.Lime;
                GS1ResLb3.Text = "Pass";
                GS1ResLb3.ForeColor = Color.Lime;
                GS1ResLb4.Text = "Pass";
                GS1ResLb4.ForeColor = Color.Lime;
                GS2ResLb1.Text = "Pass";
                GS2ResLb1.ForeColor = Color.Lime;
                GS2ResLb2.Text = "Pass";
                GS2ResLb2.ForeColor = Color.Lime;
                GS2ResLb3.Text = "Pass";
                GS2ResLb3.ForeColor = Color.Lime;
                GS2ResLb4.Text = "Pass";
                GS2ResLb4.ForeColor = Color.Lime;
                button8.Enabled = false;
            }
        }
        private void LoadGUSerial()
        {
            GS1SerialCB.Items.Clear();
            GS2SerialCB.Items.Clear();
            for (int i = 0; i < myData_T.Count; i++)
            {
                GS1SerialCB.Items.Add(myData_T[i].Serial);
                GS2SerialCB.Items.Add(myData_T[i].Serial);
            }
        }
        private void InitGS2LV()
        {
            GS2LV.Clear();
            GS2LV.Columns.Add("", 20);
            GS2LV.Columns.Add("WL", 40);
            GS2LV.Columns.Add("Point", 40);
            GS2LV.Columns.Add("Meas_T%", 70);
            GS2LV.Columns.Add("Meas_Tilt_Average_Ratio", 140);
            GS2LV.Columns.Add("Meas_T%_Average_Ratio", 140);
            GS2LV.Columns.Add("Pass/Fail", 110);
            this.GS2LV.View = System.Windows.Forms.View.Details;
        }
        private void LoadGS2UI()
        {
            int barcodeLength = stationRes.Unit2Barcode.Length;
            string blackorwhite = stationRes.Unit2Barcode.Substring(barcodeLength - 1, 1);
            int idx = GS2SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            InitGS2LV();

            for (int i = 0; i < myData_T[idx].TransRatio.Count; i++)
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
                //item.SubItems.Add(myData_C[idx].TransRatio[i].ToString("F3"));
                //item.SubItems.Add(myData_T[idx].TransRatio[i].ToString("F3"));
                //item.SubItems.Add(mySpec_Tilt[i].UpperLimit.ToString("F3"));
                //item.SubItems.Add(mySpec_Tilt[i].LowerLimit.ToString("F3"));
                //if (blackorwhite == "3")//Black
                //{
                //    item.SubItems.Add(mySpec_Black_TP[i].UpperLimit.ToString("F3"));
                //    item.SubItems.Add(mySpec_Black_TP[i].LowerLimit.ToString("F3"));
                //}
                //else
                //{
                //    item.SubItems.Add(mySpec_White_TP[i].UpperLimit.ToString("F3"));
                //    item.SubItems.Add(mySpec_White_TP[i].LowerLimit.ToString("F3"));
                //}
                //item.SubItems[1].BackColor = Color.Red;                                              
                item.EnsureVisible();

                // GS1LV.Items[GS1LV.Items.Count - 1].SubItems[1].BackColor = Color.Red;
            }
        }
        private void InitGS1LV()
        {
            GS1LV.Clear();
            GS1LV.Columns.Add("", 20);
            GS1LV.Columns.Add("WL", 40);
            GS1LV.Columns.Add("Point", 40);
            GS1LV.Columns.Add("Meas_T%", 70);
            GS1LV.Columns.Add("Meas_Tilt_Average_Ratio", 140);
            GS1LV.Columns.Add("Meas_T%_Average_Ratio", 140);
            GS1LV.Columns.Add("Pass/Fail", 110);
            this.GS1LV.View = System.Windows.Forms.View.Details;
        }
        //List<CalibrationSpec> mySpec_Tilt = new List<CalibrationSpec>();
        //List<CalibrationSpec> mySpec_White_TP = new List<CalibrationSpec>();
        //List<CalibrationSpec> mySpec_Black_TP = new List<CalibrationSpec>();
        private void LoadGS1UI()
        {
            int barcodeLength = stationRes.Unit1Barcode.Length;
            string blackorwhite = stationRes.Unit1Barcode.Substring(barcodeLength - 1, 1);
            int idx = GS1SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            InitGS1LV();

            for (int i = 0; i < myData_T[idx].TransRatio.Count; i++)
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
                //item.SubItems.Add(myData_C[idx].TransRatio[i].ToString("F3"));
                //item.SubItems.Add(myData_T[idx].TransRatio[i].ToString("F3"));
                //item.SubItems.Add(mySpec_Tilt[i].UpperLimit.ToString("F3"));
                //item.SubItems.Add(mySpec_Tilt[i].LowerLimit.ToString("F3"));
                //if (blackorwhite == "3")//Black
                //{
                //    item.SubItems.Add(mySpec_Black_TP[i].UpperLimit.ToString("F3"));
                //    item.SubItems.Add(mySpec_Black_TP[i].LowerLimit.ToString("F3"));

                //}
                //else
                //{
                //    item.SubItems.Add(mySpec_White_TP[i].UpperLimit.ToString("F3"));
                //    item.SubItems.Add(mySpec_White_TP[i].LowerLimit.ToString("F3"));
                //}
                //item.SubItems[1].BackColor = Color.Red;                                              
                item.EnsureVisible();

                // GS1LV.Items[GS1LV.Items.Count - 1].SubItems[1].BackColor = Color.Red;
            }
        }


        private void LoadGS1UIAndResult()
        {
            int barcodeLength = stationRes.Unit1Barcode.Length;
            string blackorwhite = stationRes.Unit1Barcode.Substring(barcodeLength - 1, 1);
            int idx = GS1SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            bool IsPass = true;
            InitGS1LV();
            //
            //
            stationRes.SelectCount1.Clear();

            float tilt = 0;

            for (int i = 0; i < 17; i++) //Point 1
            {
                tilt = (myData_T[idx].MeasTransRatio[i] / myData_T[idx].MeasTransRatio[5]) * 100;
                stationRes.SelectCount1.Add(tilt);
            }

            for (int i = 17; i < 34; i++) //Point 2
            {
                tilt = (myData_T[idx].MeasTransRatio[i] / myData_T[idx].MeasTransRatio[5 + 17]) * 100;
                stationRes.SelectCount1.Add(tilt);
            }

            for (int i = 34; i < 51; i++) //Point 3
            {
                tilt = (myData_T[idx].MeasTransRatio[i] / myData_T[idx].MeasTransRatio[5 + 34]) * 100;
                stationRes.SelectCount1.Add(tilt);
            }

            stationRes.SelectCount1_Avg.Clear();
            float temp = 0;
            for (int i = 0; i < 17; i++)
            {
                temp = (stationRes.SelectCount1[i] + stationRes.SelectCount1[i + 17] + stationRes.SelectCount1[i + 34]) / 3;
                stationRes.SelectCount1_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (stationRes.SelectCount1[i] + stationRes.SelectCount1[i + 17] + stationRes.SelectCount1[i + 34]) / 3;
                stationRes.SelectCount1_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (stationRes.SelectCount1[i] + stationRes.SelectCount1[i + 17] + stationRes.SelectCount1[i + 34]) / 3;
                stationRes.SelectCount1_Avg.Add(temp);
            }

            for (int i = 0; i < 17; i++)     //tilt avg ratio
            {
                myData_T[idx].MeasTiltPercent[i] = stationRes.SelectCount1_Avg[i] / ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTiltPercent[i + 17] = stationRes.SelectCount1_Avg[i] / ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTiltPercent[i + 34] = stationRes.SelectCount1_Avg[i] / ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3);
            }

            temp = 0;
            stationRes.MeasureRatio1_Avg.Clear();
            for (int i = 0; i < 17; i++)
            {
                temp = (myData_T[idx].MeasTransRatio[i] + myData_T[idx].MeasTransRatio[i + 17] + myData_T[idx].MeasTransRatio[i + 34]) / 3;
                stationRes.MeasureRatio1_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (myData_T[idx].MeasTransRatio[i] + myData_T[idx].MeasTransRatio[i + 17] + myData_T[idx].MeasTransRatio[i + 34]) / 3;
                stationRes.MeasureRatio1_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (myData_T[idx].MeasTransRatio[i] + myData_T[idx].MeasTransRatio[i + 17] + myData_T[idx].MeasTransRatio[i + 34]) / 3;
                stationRes.MeasureRatio1_Avg.Add(temp);
            }

            for (int i = 0; i < 17; i++)    //tilt avg ratio
            {
                myData_T[idx].MeasTPercent[i] = (stationRes.MeasureRatio1_Avg[i] - ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3)) / ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTPercent[i + 17] = (stationRes.MeasureRatio1_Avg[i] - ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3)) / ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTPercent[i + 34] = (stationRes.MeasureRatio1_Avg[i] - ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3)) / ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3);
            }

            errorstring1 = "";
            float tilt_ref_avg = 0;
            float tPercent_ref_avg = 0;
            for (int i = 0; i < myData_T[idx].TransRatio.Count; i++)
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
                item.SubItems.Add(myData_T[idx].MeasTransRatio[i].ToString("F3"));    //measure t%
                item.SubItems.Add(myData_T[idx].MeasTiltPercent[i].ToString("F3"));  // tilt average ratio
                item.SubItems.Add(myData_T[idx].MeasTPercent[i].ToString("F3"));  // t% average ratio
                item.UseItemStyleForSubItems = false;

                tilt_ref_avg = 0;
                tPercent_ref_avg = 0;
                if (i <= 16)
                {
                    tilt_ref_avg = (myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3;
                    tPercent_ref_avg = (myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3;
                }
                if (i >= 17 && i <= 33)
                {
                    tilt_ref_avg = (myData_C[idx].TransRatio[i - 17] + myData_C[idx].TransRatio[i + 17 - 17] + myData_C[idx].TransRatio[i + 34 - 17]) / 3;
                    tPercent_ref_avg = (myData_T[idx].TransRatio[i - 17] + myData_T[idx].TransRatio[i + 17 - 17] + myData_T[idx].TransRatio[i + 34 - 17]) / 3;
                }
                if (i >= 34 && i <= 50)
                {
                    tilt_ref_avg = (myData_C[idx].TransRatio[i - 34] + myData_C[idx].TransRatio[i + 17 - 34] + myData_C[idx].TransRatio[i + 34 - 34]) / 3;
                    tPercent_ref_avg = (myData_T[idx].TransRatio[i - 34] + myData_T[idx].TransRatio[i + 17 - 34] + myData_T[idx].TransRatio[i + 34 - 34]) / 3;
                }

                if (blackorwhite == "3")//Black
                {
                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit))
                        || (stationRes.SelectCount1_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit)) || (stationRes.SelectCount1_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                        || (stationRes.MeasureRatio1_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit)) || (stationRes.MeasureRatio1_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                        || (myData_T[idx].MeasTiltPercent[i] < mySpec_Tilt_Ratio[i].LowerLimit) || (myData_T[idx].MeasTiltPercent[i] > mySpec_Tilt_Ratio[i].UpperLimit)
                        || (myData_T[idx].MeasTPercent[i] < mySpec_Black_TP[i].LowerLimit) || (myData_T[idx].MeasTPercent[i] > mySpec_Black_TP[i].UpperLimit))
                    {
                        IsPass = false;
                        item.SubItems.Add("Fail");
                        item.SubItems[6].BackColor = Color.Red;
                    }
                    else
                    {
                        item.SubItems.Add("Pass");
                        item.SubItems[6].BackColor = Color.Lime;
                    }
                    item.EnsureVisible();

                    //T
                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit)))
                    {
                        errorstring1 += Title_T[i] + ";";
                    }



                    if (i == (myData_T[idx].TransRatio.Count - 1))
                    {
                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.SelectCount1_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit) || stationRes.SelectCount1_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                            {
                                errorstring1 += Title30[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.MeasureRatio1_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit) || stationRes.MeasureRatio1_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                            {
                                errorstring1 += Title31[j] + ";";
                            }
                        }

                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 1 || j == 2)
                                continue;
                            //TILT PERCENT
                            if ((myData_T[idx].MeasTiltPercent[j] < mySpec_Tilt_Ratio[j].LowerLimit) || (myData_T[idx].MeasTiltPercent[j] > mySpec_Tilt_Ratio[j].UpperLimit))
                            {
                                errorstring1 += Title20[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 13 || j == 14 || j == 15 || j == 16 || j == 1 || j == 2)
                                continue;
                            //T PERCENT av
                            if ((myData_T[idx].MeasTPercent[j] < mySpec_Black_TP[j].LowerLimit) || (myData_T[idx].MeasTPercent[j] > mySpec_Black_TP[j].UpperLimit))
                            {
                                errorstring1 += Title21[j] + ";";
                            }
                        }
                    }
                }
                else
                {

                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit))
                        || (stationRes.SelectCount1_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit)) || (stationRes.SelectCount1_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                        || (stationRes.MeasureRatio1_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit)) || (stationRes.MeasureRatio1_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                        || (myData_T[idx].MeasTiltPercent[i] < mySpec_Tilt_Ratio[i].LowerLimit) || (myData_T[idx].MeasTiltPercent[i] > mySpec_Tilt_Ratio[i].UpperLimit)
                        || (myData_T[idx].MeasTPercent[i] < mySpec_White_TP[i].LowerLimit) || (myData_T[idx].MeasTPercent[i] > mySpec_White_TP[i].UpperLimit))
                    {
                        IsPass = false;
                        item.SubItems.Add("Fail");
                        item.SubItems[6].BackColor = Color.Red;
                    }
                    else
                    {
                        item.SubItems.Add("Pass");
                        item.SubItems[6].BackColor = Color.Lime;
                    }
                    item.EnsureVisible();

                    //T
                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit)))
                    {
                        errorstring1 += Title_T[i] + ";";
                    }

                    if (i == (myData_T[idx].TransRatio.Count - 1))
                    {

                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.SelectCount1_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit) || stationRes.SelectCount1_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                            {
                                errorstring1 += Title30[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.MeasureRatio1_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit) || stationRes.MeasureRatio1_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                            {
                                errorstring1 += Title31[j] + ";";
                            }
                        }


                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 1 || j == 2)
                                continue;
                            //TILT PERCENT
                            if ((myData_T[idx].MeasTiltPercent[j] < mySpec_Tilt_Ratio[j].LowerLimit) || (myData_T[idx].MeasTiltPercent[j] > mySpec_Tilt_Ratio[j].UpperLimit))
                            {
                                errorstring1 += Title20[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 13 || j == 14 || j == 15 || j == 16)
                                continue;
                            //T PERCENT av
                            if ((myData_T[idx].MeasTPercent[j] < mySpec_White_TP[j].LowerLimit) || (myData_T[idx].MeasTPercent[j] > mySpec_White_TP[j].UpperLimit))
                            {
                                errorstring1 += Title21[j] + ";";
                            }
                        }
                    }
                }
            }
            if (StationIndex == 1)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS1ResLbl.Text = "Pass";
                        GS1ResLbl.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLbl.Text = "Fail";
                        GS1ResLbl.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS1ResLb5.Text = "Pass";
                        GS1ResLb5.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLb5.Text = "Fail";
                        GS1ResLb5.ForeColor = Color.Red;
                    }
                }

            }
            if (StationIndex == 2)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS1ResLb2.Text = "Pass";
                        GS1ResLb2.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLb2.Text = "Fail";
                        GS1ResLb2.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS1ResLb6.Text = "Pass";
                        GS1ResLb6.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLb6.Text = "Fail";
                        GS1ResLb6.ForeColor = Color.Red;
                    }
                }
            }
            if (StationIndex == 3)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS1ResLb3.Text = "Pass";
                        GS1ResLb3.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLb3.Text = "Fail";
                        GS1ResLb3.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS1ResLb7.Text = "Pass";
                        GS1ResLb7.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLb7.Text = "Fail";
                        GS1ResLb7.ForeColor = Color.Red;
                    }
                }
            }
            if (StationIndex == 4)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS1ResLb4.Text = "Pass";
                        GS1ResLb4.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLb4.Text = "Fail";
                        GS1ResLb4.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS1ResLb8.Text = "Pass";
                        GS1ResLb8.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS1ResLb8.Text = "Fail";
                        GS1ResLb8.ForeColor = Color.Red;
                    }
                }
            }
        }
        private void LoadGS2UIAndResult()
        {
            int barcodeLength = stationRes.Unit2Barcode.Length;
            string blackorwhite = stationRes.Unit2Barcode.Substring(barcodeLength - 1, 1);
            int idx = GS2SerialCB.SelectedIndex;
            int wlIdx = 0;
            int ptCnt = 1;
            bool IsPass = true;
            InitGS2LV();
            //
            //
            stationRes.SelectCount2.Clear();

            float tilt = 0;

            for (int i = 0; i < 17; i++) //Point 1
            {
                tilt = (myData_T[idx].MeasTransRatio[i] / myData_T[idx].MeasTransRatio[5]) * 100;
                stationRes.SelectCount2.Add(tilt);
            }

            for (int i = 17; i < 34; i++) //Point 2
            {
                tilt = (myData_T[idx].MeasTransRatio[i] / myData_T[idx].MeasTransRatio[5 + 17]) * 100;
                stationRes.SelectCount2.Add(tilt);
            }

            for (int i = 34; i < 51; i++) //Point 3
            {
                tilt = (myData_T[idx].MeasTransRatio[i] / myData_T[idx].MeasTransRatio[5 + 34]) * 100;
                stationRes.SelectCount2.Add(tilt);
            }

            stationRes.SelectCount2_Avg.Clear();
            float temp = 0;
            for (int i = 0; i < 17; i++)
            {
                temp = (stationRes.SelectCount2[i] + stationRes.SelectCount2[i + 17] + stationRes.SelectCount2[i + 34]) / 3;
                stationRes.SelectCount2_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (stationRes.SelectCount2[i] + stationRes.SelectCount2[i + 17] + stationRes.SelectCount2[i + 34]) / 3;
                stationRes.SelectCount2_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (stationRes.SelectCount2[i] + stationRes.SelectCount2[i + 17] + stationRes.SelectCount2[i + 34]) / 3;
                stationRes.SelectCount2_Avg.Add(temp);
            }

            for (int i = 0; i < 17; i++)     //tilt avg ratio
            {
                myData_T[idx].MeasTiltPercent[i] = stationRes.SelectCount2_Avg[i] / ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTiltPercent[i + 17] = stationRes.SelectCount2_Avg[i] / ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTiltPercent[i + 34] = stationRes.SelectCount2_Avg[i] / ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3);
            }

            temp = 0;
            stationRes.MeasureRatio2_Avg.Clear();
            for (int i = 0; i < 17; i++)
            {
                temp = (myData_T[idx].MeasTransRatio[i] + myData_T[idx].MeasTransRatio[i + 17] + myData_T[idx].MeasTransRatio[i + 34]) / 3;
                stationRes.MeasureRatio2_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (myData_T[idx].MeasTransRatio[i] + myData_T[idx].MeasTransRatio[i + 17] + myData_T[idx].MeasTransRatio[i + 34]) / 3;
                stationRes.MeasureRatio2_Avg.Add(temp);
            }
            for (int i = 0; i < 17; i++)
            {
                temp = (myData_T[idx].MeasTransRatio[i] + myData_T[idx].MeasTransRatio[i + 17] + myData_T[idx].MeasTransRatio[i + 34]) / 3;
                stationRes.MeasureRatio2_Avg.Add(temp);
            }

            for (int i = 0; i < 17; i++)    //tilt avg ratio
            {
                myData_T[idx].MeasTPercent[i] = (stationRes.MeasureRatio2_Avg[i] - ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3)) / ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTPercent[i + 17] = (stationRes.MeasureRatio2_Avg[i] - ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3)) / ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3);
                myData_T[idx].MeasTPercent[i + 34] = (stationRes.MeasureRatio2_Avg[i] - ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3)) / ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3);
            }

            errorstring2 = "";
            float tilt_ref_avg = 0;
            float tPercent_ref_avg = 0;
            for (int i = 0; i < myData_T[idx].TransRatio.Count; i++)
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
                item.SubItems.Add(myData_T[idx].MeasTransRatio[i].ToString("F3"));    //measure t%
                item.SubItems.Add(myData_T[idx].MeasTiltPercent[i].ToString("F3"));  // tilt average ratio
                item.SubItems.Add(myData_T[idx].MeasTPercent[i].ToString("F3"));  // t% average ratio

                item.UseItemStyleForSubItems = false;


                tilt_ref_avg = 0;
                tPercent_ref_avg = 0;
                if (i <= 16)
                {
                    tilt_ref_avg = (myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3;
                    tPercent_ref_avg = (myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3;
                }
                if (i >= 17 && i <= 33)
                {
                    tilt_ref_avg = (myData_C[idx].TransRatio[i - 17] + myData_C[idx].TransRatio[i + 17 - 17] + myData_C[idx].TransRatio[i + 34 - 17]) / 3;
                    tPercent_ref_avg = (myData_T[idx].TransRatio[i - 17] + myData_T[idx].TransRatio[i + 17 - 17] + myData_T[idx].TransRatio[i + 34 - 17]) / 3;
                }
                if (i >= 34 && i <= 50)
                {
                    tilt_ref_avg = (myData_C[idx].TransRatio[i - 34] + myData_C[idx].TransRatio[i + 17 - 34] + myData_C[idx].TransRatio[i + 34 - 34]) / 3;
                    tPercent_ref_avg = (myData_T[idx].TransRatio[i - 34] + myData_T[idx].TransRatio[i + 17 - 34] + myData_T[idx].TransRatio[i + 34 - 34]) / 3;
                }

                if (blackorwhite == "3")//Black
                {
                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit))
                        || (stationRes.SelectCount2_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit)) || (stationRes.SelectCount2_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                        || (stationRes.MeasureRatio2_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit)) || (stationRes.MeasureRatio2_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                        || (myData_T[idx].MeasTiltPercent[i] < mySpec_Tilt_Ratio[i].LowerLimit) || (myData_T[idx].MeasTiltPercent[i] > mySpec_Tilt_Ratio[i].UpperLimit)
                        || (myData_T[idx].MeasTPercent[i] < mySpec_Black_TP[i].LowerLimit) || (myData_T[idx].MeasTPercent[i] > mySpec_Black_TP[i].UpperLimit))
                    {
                        IsPass = false;
                        item.SubItems.Add("Fail");
                        item.SubItems[6].BackColor = Color.Red;
                    }
                    else
                    {
                        item.SubItems.Add("Pass");
                        item.SubItems[6].BackColor = Color.Lime;
                    }
                    item.EnsureVisible();

                    //T
                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit)))
                    {
                        errorstring2 += Title_T[i] + ";";
                    }

                    if (i == (myData_T[idx].TransRatio.Count - 1))
                    {
                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.SelectCount2_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit) || stationRes.SelectCount2_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                            {
                                errorstring2 += Title30[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.MeasureRatio2_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit) || stationRes.MeasureRatio2_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                            {
                                errorstring2 += Title31[j] + ";";
                            }
                        }

                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 1 || j == 2)
                                continue;
                            //TILT PERCENT
                            if ((myData_T[idx].MeasTiltPercent[j] < mySpec_Tilt_Ratio[j].LowerLimit) || (myData_T[idx].MeasTiltPercent[j] > mySpec_Tilt_Ratio[j].UpperLimit))
                            {
                                errorstring2 += Title20[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 13 || j == 14 || j == 15 || j == 16 || j == 1 || j == 2)
                                continue;
                            //T PERCENT av
                            if ((myData_T[idx].MeasTPercent[j] < mySpec_Black_TP[j].LowerLimit) || (myData_T[idx].MeasTPercent[j] > mySpec_Black_TP[j].UpperLimit))
                            {
                                errorstring2 += Title21[j] + ";";
                            }
                        }
                    }
                }
                else
                {
                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit))
                        || (stationRes.SelectCount2_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit)) || (stationRes.SelectCount2_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                        || (stationRes.MeasureRatio2_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit)) || (stationRes.MeasureRatio2_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                        || (myData_T[idx].MeasTiltPercent[i] < mySpec_Tilt_Ratio[i].LowerLimit) || (myData_T[idx].MeasTiltPercent[i] > mySpec_Tilt_Ratio[i].UpperLimit)
                        || (myData_T[idx].MeasTPercent[i] < mySpec_White_TP[i].LowerLimit) || (myData_T[idx].MeasTPercent[i] > mySpec_White_TP[i].UpperLimit))
                    {
                        IsPass = false;
                        item.SubItems.Add("Fail");
                        item.SubItems[6].BackColor = Color.Red;
                    }
                    else
                    {
                        item.SubItems.Add("Pass");
                        item.SubItems[6].BackColor = Color.Lime;
                    }
                    item.EnsureVisible();

                    //T
                    if ((myData_T[idx].MeasTransRatio[i] < (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit)) || (myData_T[idx].MeasTransRatio[i] > (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit)))
                    {
                        errorstring2 += Title_T[i] + ";";
                    }


                    if (i == (myData_T[idx].TransRatio.Count - 1))
                    {
                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.SelectCount2_Avg[i] < (tilt_ref_avg + mySpec_Tilt_Avg[i].LowerLimit) || stationRes.SelectCount2_Avg[i] > (tilt_ref_avg + mySpec_Tilt_Avg[i].UpperLimit))
                            {
                                errorstring2 += Title30[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (stationRes.MeasureRatio2_Avg[i] < (tPercent_ref_avg + mySpec_TPercent_Avg[i].LowerLimit) || stationRes.MeasureRatio2_Avg[i] > (tPercent_ref_avg + mySpec_TPercent_Avg[i].UpperLimit))
                            {
                                errorstring2 += Title31[j] + ";";
                            }
                        }

                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 1 || j == 2)
                                continue;
                            //TILT PERCENT
                            if ((myData_T[idx].MeasTiltPercent[j] < mySpec_Tilt_Ratio[j].LowerLimit) || (myData_T[idx].MeasTiltPercent[j] > mySpec_Tilt_Ratio[j].UpperLimit))
                            {
                                errorstring2 += Title20[j] + ";";
                            }
                        }
                        for (int j = 0; j < 17; j++)
                        {
                            if (j == 13 || j == 14 || j == 15 || j == 16)
                                continue;
                            //T PERCENT av
                            if ((myData_T[idx].MeasTPercent[j] < mySpec_White_TP[j].LowerLimit) || (myData_T[idx].MeasTPercent[j] > mySpec_White_TP[j].UpperLimit))
                            {
                                errorstring2 += Title21[j] + ";";
                            }
                        }
                    }
                }
            }

            if (StationIndex == 1)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS2ResLb1.Text = "Pass";
                        GS2ResLb1.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb1.Text = "Fail";
                        GS2ResLb1.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS2ResLb5.Text = "Pass";
                        GS2ResLb5.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb5.Text = "Fail";
                        GS2ResLb5.ForeColor = Color.Red;
                    }
                }
            }
            if (StationIndex == 2)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS2ResLb2.Text = "Pass";
                        GS2ResLb2.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb2.Text = "Fail";
                        GS2ResLb2.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS2ResLb6.Text = "Pass";
                        GS2ResLb6.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb6.Text = "Fail";
                        GS2ResLb6.ForeColor = Color.Red;
                    }
                }

            }
            if (StationIndex == 3)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS2ResLb3.Text = "Pass";
                        GS2ResLb3.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb3.Text = "Fail";
                        GS2ResLb3.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS2ResLb7.Text = "Pass";
                        GS2ResLb7.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb7.Text = "Fail";
                        GS2ResLb7.ForeColor = Color.Red;
                    }
                }
            }
            if (StationIndex == 4)
            {
                if (ColorSelection == 8)
                {
                    if (IsPass)
                    {
                        GS2ResLb4.Text = "Pass";
                        GS2ResLb4.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb4.Text = "Fail";
                        GS2ResLb4.ForeColor = Color.Red;
                    }
                }
                if (ColorSelection == 3)
                {
                    if (IsPass)
                    {
                        GS2ResLb8.Text = "Pass";
                        GS2ResLb8.ForeColor = Color.Lime;
                    }
                    else
                    {
                        GS2ResLb8.Text = "Fail";
                        GS2ResLb8.ForeColor = Color.Red;
                    }
                }
            }
        }
        private void CalResult()
        {
            int idx = GS1SerialCB.SelectedIndex;

            int s = 0;
            int t = 0;
            float TranRatio = 0;
            myData_T[idx].MeasTransRatio.Clear();    //CLEAR

            if (!Para.Enb3TestPtOnly) //Disable Point 1
            {
                for (int i = 0; i < 17; i++) //Point 1
                {
                    t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                    s = t;
                    TranRatio = stationRes.transRatioMod1[0][t];
                    myData_T[idx].MeasTransRatio.Add(TranRatio);
                }
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++) //Point 2
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[1][t];
                myData_T[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 3
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[2][t];
                myData_T[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 4
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                s = t;
                TranRatio = stationRes.transRatioMod1[3][t];
                myData_T[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            if (!Para.Enb3TestPtOnly) //Disable Point 1
            {
                for (int i = 0; i < 17; i++)//Point 5
                {
                    t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod1Dark);
                    s = t;
                    TranRatio = stationRes.transRatioMod1[4][t];
                    myData_T[idx].MeasTransRatio.Add(TranRatio);
                }
            }
            idx = GS2SerialCB.SelectedIndex;
            s = 0;
            t = 0;
            myData_T[idx].MeasTransRatio.Clear();
            if (!Para.Enb3TestPtOnly) //Disable Point 1
            {
                for (int i = 0; i < 17; i++) //Point 1
                {
                    t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                    s = t;
                    TranRatio = stationRes.transRatioMod2[0][t];
                    myData_T[idx].MeasTransRatio.Add(TranRatio);
                }
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++) //Point 2
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[1][t];
                myData_T[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 3
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[2][t];
                myData_T[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            for (int i = 0; i < 17; i++)//Point 4
            {
                t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                s = t;
                TranRatio = stationRes.transRatioMod2[3][t];
                myData_T[idx].MeasTransRatio.Add(TranRatio);
            }
            s = 0;
            t = 0;
            if (!Para.Enb3TestPtOnly) //Disable Point 1
            {
                for (int i = 0; i < 17; i++)//Point 5
                {
                    t = Para.myMain.SeqMgr.NearestIndex(s, int.Parse(WLStr[i]), stationRes.WLMod2Dark);
                    s = t;
                    TranRatio = stationRes.transRatioMod2[4][t];
                    myData_T[idx].MeasTransRatio.Add(TranRatio);
                }
            }
        }
        private void DisplayResult()
        {
            InitGS1LV();
            InitGS2LV();
            CalResult();
            LoadGS1UIAndResult();
            LoadGS2UIAndResult();
            SaveData();
            SaveTxtModule1();
            SaveTxtModule2();
        }

        public void SaveTxtModule1()
        {
            string columnTitle = "";
            if (!Directory.Exists("C:\\Dropbox"))
            {
                Directory.CreateDirectory("C:\\Dropbox");
            }
            string FileName1 = "C:\\Dropbox" + "\\" + stationRes.Unit1Barcode.Substring(0, 17) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            FileStream objFileStream5;    //white file
            if (!File.Exists(FileName1))
            {
                objFileStream5 = new FileStream(FileName1, FileMode.CreateNew, FileAccess.Write);
            }
            else
            {
                objFileStream5 = new FileStream(FileName1, FileMode.Append, FileAccess.Write);
            }

            StreamWriter sw5 = new StreamWriter(objFileStream5, System.Text.Encoding.GetEncoding(-0));
            try
            {
                columnTitle = "";
                int idx = GS1SerialCB.SelectedIndex;    //module1
                string okORfail = "Pass";
                myData_T[idx].MeasureResult = true;    //test result true
                if (errorstring1 != "")
                {
                    okORfail = "Fail";
                    myData_T[idx].MeasureResult = false;    //test result true
                }
                int stationID = 0;
                if (StationIndex == 1)
                    stationID = 1;
                if (StationIndex == 2)
                    stationID = 3;
                if (StationIndex == 3)
                    stationID = 5;
                if (StationIndex == 4)
                    stationID = 7;
                columnTitle = columnTitle + stationRes.Unit1Barcode.Substring(0, 17) + "," + okORfail + "," + "," + "," + starttime + "," + DateTime.Now.ToString("") + "," + Para.SWVersion.Substring(8, Para.SWVersion.Length - 8) + "," + "," + "StationID" + "," + "NA" + "," + "NA" + "," + "NA" + "," + Para.MchName.Substring(7, 3) + stationID.ToString() + ",";
                for (int i = 0; i < Title1.Length; i++)    //point1  t%    
                {
                    columnTitle = columnTitle + Title1[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                }
                for (int i = 0; i < Title1.Length; i++)    //point1  tilt    
                {
                    columnTitle = columnTitle + Title4[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                }

                for (int i = 17; i < (17 + Title1.Length); i++)     //point2  t% 
                {
                    columnTitle = columnTitle + Title2[i - 17] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                }

                for (int i = 17; i < (17 + Title1.Length); i++)    //point2  tilt
                {
                    columnTitle = columnTitle + Title5[i - 17] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                }

                for (int i = 34; i < (34 + Title1.Length); i++)    //point3   t%
                {
                    columnTitle = columnTitle + Title3[i - 34] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                }

                for (int i = 34; i < (34 + Title1.Length); i++)    //point3  tilt
                {
                    columnTitle += Title6[i - 34] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                }

                for (int i = 0; i < Title30.Length; i++)    //tilt average 
                {
                    columnTitle += Title30[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";

                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].LowerLimit).ToString("F4") + ",";
                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + stationRes.SelectCount1_Avg[i].ToString("F4") + ",";
                }

                for (int i = 0; i < Title31.Length; i++)    //t% average 
                {
                    columnTitle += Title31[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + stationRes.MeasureRatio1_Avg[i].ToString("F4") + ",";
                }

                for (int i = 0; i < Title20.Length; i++)    //tilt average ratio
                {
                    columnTitle += Title20[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + mySpec_Tilt_Ratio[i].LowerLimit.ToString("F4") + ",";
                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + mySpec_Tilt_Ratio[i].UpperLimit.ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTiltPercent[i].ToString("F4") + ",";
                }

                int barcodeLength = stationRes.Unit1Barcode.Length;
                string blackorwhite = stationRes.Unit1Barcode.Substring(barcodeLength - 1, 1);
                for (int i = 0; i < 17; i++)      //t% average ratio
                {
                    if (blackorwhite == "8")
                    {
                        columnTitle += Title21[i] + ",";
                        columnTitle = columnTitle + "NA" + ",";
                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].LowerLimit.ToString("F4") + ",";


                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].UpperLimit.ToString("F4") + ",";
                        if (i == 16)
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4");
                        else
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }
                    else
                    {
                        columnTitle += Title21[i] + ",";
                        columnTitle = columnTitle + "NA" + ",";
                        if (i == 1 || i == 2 || i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].LowerLimit.ToString("F4") + ",";


                        if (i == 1 || i == 2 || i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].UpperLimit.ToString("F4") + ",";
                        if (i == 16)
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4");
                        else
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }
                }
                sw5.WriteLine(columnTitle);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                sw5.Flush();
                sw5.Close();
                objFileStream5.Close();
            }

        }

        public void SaveTxtModule2()
        {
            string columnTitle = "";
            if (!Directory.Exists("C:\\Dropbox"))
            {
                Directory.CreateDirectory("C:\\Dropbox");
            }
            string FileName1 = "C:\\Dropbox" + "\\" + stationRes.Unit2Barcode.Substring(0, 17) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            FileStream objFileStream6;    //white file
            if (!File.Exists(FileName1))
            {
                objFileStream6 = new FileStream(FileName1, FileMode.CreateNew, FileAccess.Write);
            }
            else
            {
                objFileStream6 = new FileStream(FileName1, FileMode.Append, FileAccess.Write);
            }
            StreamWriter sw6 = new StreamWriter(objFileStream6, System.Text.Encoding.GetEncoding(-0));

            try
            {
                columnTitle = "";
                int idx = GS2SerialCB.SelectedIndex;    //module1
                string okORfail = "Pass";
                myData_T[idx].MeasureResult = true;    //test result true
                if (errorstring2 != "")
                {
                    okORfail = "Fail";
                    myData_T[idx].MeasureResult = false;    //test result true
                }
                int stationID = 0;
                if (StationIndex == 1)
                    stationID = 2;
                if (StationIndex == 2)
                    stationID = 4;
                if (StationIndex == 3)
                    stationID = 6;
                if (StationIndex == 4)
                    stationID = 8;
                columnTitle = columnTitle + stationRes.Unit2Barcode.Substring(0, 17) + "," + okORfail + "," + "," + "," + starttime + "," + DateTime.Now.ToString() + "," + Para.SWVersion.Substring(8, Para.SWVersion.Length - 8) + "," + "," + "StationID" + "," + "NA" + "," + "NA" + "," + "NA" + "," + Para.MchName.Substring(7, 3) + stationID.ToString() + ",";

                for (int i = 0; i < Title1.Length; i++)    //point1  t%    
                {
                    columnTitle = columnTitle + Title1[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                }
                for (int i = 0; i < Title1.Length; i++)    //point1  tilt    
                {
                    columnTitle = columnTitle + Title4[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                }

                for (int i = 17; i < (17 + Title1.Length); i++)     //point2  t% 
                {
                    columnTitle = columnTitle + Title2[i - 17] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                }

                for (int i = 17; i < (17 + Title1.Length); i++)    //point2  tilt
                {
                    columnTitle = columnTitle + Title5[i - 17] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                }

                for (int i = 34; i < (34 + Title1.Length); i++)    //point3   t%
                {
                    columnTitle = columnTitle + Title3[i - 34] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                }

                for (int i = 34; i < (34 + Title1.Length); i++)    //point3  tilt
                {
                    columnTitle += Title6[i - 34] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                }


                for (int i = 0; i < Title30.Length; i++)    //tilt average 
                {
                    columnTitle += Title30[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].LowerLimit).ToString("F4") + ",";
                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + stationRes.SelectCount2_Avg[i].ToString("F4") + ",";
                }

                for (int i = 0; i < Title31.Length; i++)    //t% average 
                {
                    columnTitle += Title31[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].LowerLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].UpperLimit).ToString("F4") + ",";
                    columnTitle = columnTitle + stationRes.MeasureRatio2_Avg[i].ToString("F4") + ",";
                }

                for (int i = 0; i < Title20.Length; i++)    //tilt average ratio
                {
                    columnTitle += Title20[i] + ",";
                    columnTitle = columnTitle + "NA" + ",";
                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + mySpec_Tilt_Ratio[i].LowerLimit.ToString("F4") + ",";
                    if (i == 1 || i == 2)
                        columnTitle = columnTitle + "NA" + ",";
                    else
                        columnTitle = columnTitle + mySpec_Tilt_Ratio[i].UpperLimit.ToString("F4") + ",";
                    columnTitle = columnTitle + myData_T[idx].MeasTiltPercent[i].ToString("F4") + ",";
                }



                int barcodeLength = stationRes.Unit2Barcode.Length;
                string blackorwhite = stationRes.Unit2Barcode.Substring(barcodeLength - 1, 1);
                for (int i = 0; i < 17; i++)      //t% average ratio
                {
                    if (blackorwhite == "8")
                    {
                        columnTitle += Title21[i] + ",";
                        columnTitle = columnTitle + "NA" + ",";
                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].LowerLimit.ToString("F4") + ",";


                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].UpperLimit.ToString("F4") + ",";
                        if (i == 16)
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4");
                        else
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }
                    else
                    {
                        columnTitle += Title21[i] + ",";
                        columnTitle = columnTitle + "NA" + ",";
                        if (i == 1 || i == 2 || i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].LowerLimit.ToString("F4") + ",";

                        if (i == 1 || i == 2 || i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].UpperLimit.ToString("F4") + ",";
                        if (i == 16)
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4");
                        else
                            columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }
                }
                sw6.WriteLine(columnTitle);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                sw6.Flush();
                sw6.Close();
                objFileStream6.Close();
            }
        }

        bool Module1WhiteNew = false, Module1DarkNew = false, Module2WhiteNew = false, Module2DarkNew = false;
        bool Module1WhiteNew1 = false, Module1DarkNew1 = false, Module2WhiteNew1 = false, Module2DarkNew1 = false;

        public void SaveData()
        {
            /////////////////////////////////////////  module1 file create  ///////////////////////////////////////////////////////////////////
            int stationID = 0;
            if (StationIndex == 1)
                stationID = 1;
            if (StationIndex == 2)
                stationID = 3;
            if (StationIndex == 3)
                stationID = 5;
            if (StationIndex == 4)
                stationID = 7;

            int barcodeLength = -1;
            string blackorwhite = "";
            int idx = -1;
            string columnTitle = "";
            string standdardref = "";
            string txtstring1 = "", txtstring2 = "", txtstring3 = "", txtstring4 = "", txtstring5 = "", txtstring6 = "";
            string FileName1 = "";

            string s_filename = "D:\\DailtAuditData";
            string datatime = DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(s_filename))
            {
                Directory.CreateDirectory(s_filename);
            }
            string ss_filename = s_filename + "\\" + datatime;
            if (!Directory.Exists(ss_filename))
            {
                Directory.CreateDirectory(ss_filename);
            }
            string path1 = ss_filename + "\\Module1";
            string path2 = ss_filename + "\\Module2";

            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }

            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }

            ///////////////////////////// if file is new write title ///////////////////////////////////////////////////////////////////////////////

            try
            {
                txtstring1 = ""; txtstring2 = ""; txtstring3 = ""; txtstring4 = ""; txtstring5 = ""; txtstring6 = "";
                barcodeLength = stationRes.Unit1Barcode.Length;
                blackorwhite = stationRes.Unit1Barcode.Substring(barcodeLength - 1, 1);
                if (blackorwhite == "8")
                {
                    FileStream objFileStream1;    //white file
                    FileName1 = path1 + "\\" + Para.MchName + "_White_" + stationRes.Unit1Barcode + ".csv";
                    if (!File.Exists(FileName1))
                    {
                        objFileStream1 = new FileStream(FileName1, FileMode.CreateNew, FileAccess.Write);
                        Module1WhiteNew = true;
                        Module1WhiteNew1 = true;

                    }
                    else
                    {
                        objFileStream1 = new FileStream(FileName1, FileMode.Append, FileAccess.Write);
                    }
                    StreamWriter sw1 = new StreamWriter(objFileStream1, System.Text.Encoding.GetEncoding(-0));    //sw1 white   



                    idx = GS1SerialCB.SelectedIndex;
                    ////////////////Title//////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "SerialNumber:" + "," + "DataType " + "," + "Color " + "," + "TestID" + "," + "LightSource" + "," + "CAS_SN" + "," + "SW_Version" + "," + "AuditResult" + "," + "StartTime" + "," + "EndTime" + "," + "FailedItem" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle += Title1[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title4[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)   //point2
                    {
                        columnTitle += Title2[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title5[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)    //point3
                    {
                        columnTitle += Title3[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title6[i] + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE  
                    {
                        columnTitle += Title30[i] + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle += Title31[i] + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE   TATIO
                    {
                        columnTitle += Title20[i] + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        columnTitle += Title21[i] + ",";
                    }

                    txtstring1 = columnTitle;
                    ///////////Uplimited//////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_USL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1    t
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title1.Length; i++)    //tilt
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";  //t
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].UpperLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].UpperLimit).ToString("F4") + ",";
                    }


                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE   RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].UpperLimit.ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].UpperLimit.ToString("F4") + ",";
                    }

                    txtstring2 = columnTitle;

                    //////////lowlimited////////////////////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_LSL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE   RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].LowerLimit.ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].LowerLimit.ToString("F4") + ",";
                    }

                    txtstring3 = columnTitle;
                    if (Module1WhiteNew == true)
                    {
                        Module1WhiteNew = false;
                        sw1.WriteLine(txtstring1);
                        sw1.WriteLine(txtstring2);
                        sw1.WriteLine(txtstring3);
                    }

                    //////////////////////
                    txtstring1 = ""; txtstring2 = ""; txtstring3 = ""; txtstring4 = ""; txtstring5 = ""; txtstring6 = "";
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "GolddenData " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";        //T                      
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";  //Tilt
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)     //AVG REF
                    {
                        columnTitle = columnTitle + ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }

                    for (int i = 0; i < 34; i++)    //AVG RATIO REF
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    txtstring1 = columnTitle;
                    if (Module1WhiteNew1 == true)
                    {
                        Module1WhiteNew1 = false;
                        sw1.WriteLine(columnTitle);
                    }


                    string okORfail = "Pass";
                    myData_T[idx].MeasureResult = true;    //test result true
                    if (errorstring1 != "")
                    {
                        okORfail = "Fail";
                        myData_T[idx].MeasureResult = false;    //test result true
                    }
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "AuditTest" + "," + "White " + "," + (Para.MchName + "_" + stationID.ToString()) + "," + Para.LightSourceType + "," + specMgr.SpecList[0].specType + "_" + specMgr.SpecList[0].serial + "," + Para.SWVersion + "," + okORfail + "," + starttime + "," + DateTime.Now.ToString() + "," + errorstring1 + ",";


                    for (int i = 0; i < Title1.Length; i++)    //point1   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)    //point1  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                    }


                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //tilt average 
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1_Avg[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average 
                    {
                        columnTitle = columnTitle + stationRes.MeasureRatio1_Avg[i].ToString("F4") + ",";
                    }


                    for (int i = 0; i < 17; i++)  //tilt average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTiltPercent[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }

                    txtstring2 = columnTitle;
                    sw1.WriteLine(columnTitle);

                    sw1.Close();
                    objFileStream1.Close();
                }
                else
                {
                    FileStream objFileStream2;    //dark file
                    FileName1 = path1 + "\\" + Para.MchName + "_Black_" + stationRes.Unit1Barcode + ".csv";
                    if (!File.Exists(FileName1))
                    {
                        objFileStream2 = new FileStream(FileName1, FileMode.CreateNew, FileAccess.Write);
                        Module1DarkNew = true;
                        Module1DarkNew1 = true;
                    }
                    else
                    {
                        objFileStream2 = new FileStream(FileName1, FileMode.Append, FileAccess.Write);
                    }
                    StreamWriter sw2 = new StreamWriter(objFileStream2, System.Text.Encoding.GetEncoding(-0));    //sw2 adrk   

                    idx = GS1SerialCB.SelectedIndex;
                    ////////title///////////////////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "SerialNumber:" + "," + "DataType " + "," + "Color " + "," + "TestID" + "," + "LightSource" + "," + "CAS_SN" + "," + "SW_Version" + "," + "AuditResult" + "," + "StartTime" + "," + "EndTime" + "," + "FailedItem" + ",";
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title1[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title4[i] + ",";
                    }

                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title2[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title5[i] + ",";
                    }

                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title3[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title6[i] + ",";
                    }
                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        columnTitle += Title30[i] + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle += Title31[i] + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE    RATIO
                    {
                        columnTitle += Title20[i] + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        columnTitle += Title21[i] + ",";
                    }

                    txtstring4 = columnTitle;

                    /////////////////////uplimited///////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_USL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].UpperLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].UpperLimit).ToString("F4") + ",";
                    }


                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE  RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].UpperLimit.ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16 || i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].UpperLimit.ToString("F4") + ",";
                    }
                    txtstring5 = columnTitle;

                    ///////////////////lowlimited/////////////////////////////////       
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_LSL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE  RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].LowerLimit.ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16 || i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].LowerLimit.ToString("F4") + ",";
                    }

                    txtstring6 = columnTitle;

                    if (Module1DarkNew == true)
                    {
                        Module1DarkNew = false;
                        sw2.WriteLine(txtstring4);
                        sw2.WriteLine(txtstring5);
                        sw2.WriteLine(txtstring6);
                    }
                    ///////////////////////////
                    txtstring1 = ""; txtstring2 = ""; txtstring3 = ""; txtstring4 = ""; txtstring5 = ""; txtstring6 = "";
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "GolddenData " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";        //T                      
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";  //Tilt
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)     //AVG REF
                    {
                        columnTitle = columnTitle + ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }

                    for (int i = 0; i < 34; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    txtstring3 = columnTitle;
                    if (Module1DarkNew1 == true)
                    {
                        Module1DarkNew1 = false;
                        sw2.WriteLine(columnTitle);
                    }

                    string okORfail = "Pass";
                    myData_T[idx].MeasureResult = true;    //test result true
                    if (errorstring1 != "")
                    {
                        okORfail = "Fail";
                        myData_T[idx].MeasureResult = false;    //test result true
                    }
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "AuditTest" + "," + "Black " + "," + (Para.MchName + "_" + (stationID).ToString()) + "," + Para.LightSourceType + "," + specMgr.SpecList[0].specType + "_" + specMgr.SpecList[0].serial + "," + Para.SWVersion + "," + okORfail + "," + starttime + "," + DateTime.Now.ToString() + "," + errorstring1 + ",";

                    for (int i = 0; i < Title1.Length; i++)    //point1   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)    //point1  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                    }


                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //tilt average 
                    {
                        columnTitle = columnTitle + stationRes.SelectCount1_Avg[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average 
                    {
                        columnTitle = columnTitle + stationRes.MeasureRatio1_Avg[i].ToString("F4") + ",";
                    }



                    for (int i = 0; i < 17; i++)  //tilt average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTiltPercent[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }


                    txtstring4 = columnTitle;
                    sw2.WriteLine(columnTitle);

                    sw2.Close();
                    objFileStream2.Close();
                }
                ////////////////////////////////////////////////////////////////////////////
                txtstring1 = ""; txtstring2 = ""; txtstring3 = ""; txtstring4 = ""; txtstring5 = ""; txtstring6 = "";
                barcodeLength = stationRes.Unit2Barcode.Length;
                blackorwhite = stationRes.Unit2Barcode.Substring(barcodeLength - 1, 1);
                if (blackorwhite == "8")
                {
                    FileName1 = path2 + "\\" + Para.MchName + "_White_" + stationRes.Unit2Barcode + ".csv";
                    FileStream objFileStream3;    //white file
                    if (!File.Exists(FileName1))
                    {
                        objFileStream3 = new FileStream(FileName1, FileMode.CreateNew, FileAccess.Write);
                        Module2WhiteNew = true;
                        Module2WhiteNew1 = true;
                    }
                    else
                    {
                        objFileStream3 = new FileStream(FileName1, FileMode.Append, FileAccess.Write);
                    }
                    StreamWriter sw3 = new StreamWriter(objFileStream3, System.Text.Encoding.GetEncoding(-0));   //sw3 white   
                    idx = GS2SerialCB.SelectedIndex;
                    /////////////title///////////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "SerialNumber:" + "," + " DataType" + "," + "Color " + "," + "TestID" + "," + "LightSource" + "," + "CAS_SN" + "," + "SW_Version" + "," + "AuditResult" + "," + "StartTime" + "," + "EndTime" + "," + "FailedItem" + ",";
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title1[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title4[i] + ",";
                    }

                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title2[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title5[i] + ",";
                    }

                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title3[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title6[i] + ",";
                    }
                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   TATIO
                    {
                        columnTitle += Title30[i] + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle += Title31[i] + ",";
                    }


                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE  RATIO
                    {
                        columnTitle += Title20[i] + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        columnTitle += Title21[i] + ",";
                    }

                    txtstring1 = columnTitle;

                    //////////////////////uplimited/////////////////////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_USL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].UpperLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].UpperLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE  RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].UpperLimit.ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].UpperLimit.ToString("F4") + ",";
                    }

                    txtstring2 = columnTitle;

                    ////////////lowlimited////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_LSL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE   RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].LowerLimit.ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_White_TP[i].LowerLimit.ToString("F4") + ",";
                    }

                    txtstring3 = columnTitle;

                    if (Module2WhiteNew == true)
                    {
                        Module2WhiteNew = false;
                        sw3.WriteLine(txtstring1);
                        sw3.WriteLine(txtstring2);
                        sw3.WriteLine(txtstring3);
                    }

                    /////////////////////
                    txtstring1 = ""; txtstring2 = ""; txtstring3 = ""; txtstring4 = ""; txtstring5 = ""; txtstring6 = "";
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "GolddenData " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";        //T                      
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";  //Tilt
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)     //AVG REF
                    {
                        columnTitle = columnTitle + ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }

                    for (int i = 0; i < 34; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    txtstring1 = columnTitle;
                    if (Module2WhiteNew1 == true)
                    {
                        Module2WhiteNew1 = false;
                        sw3.WriteLine(columnTitle);
                    }

                    string okORfail = "Pass";
                    myData_T[idx].MeasureResult = true;    //test result true
                    if (errorstring2 != "")
                    {
                        okORfail = "Fail";
                        myData_T[idx].MeasureResult = false;    //test result true
                    }
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "AuditTest" + "," + "White " + "," + (Para.MchName + "_" + (stationID + 1).ToString()) + "," + Para.LightSourceType + "," + specMgr.SpecList[0].specType + "_" + specMgr.SpecList[0].serial + "," + Para.SWVersion + "," + okORfail + "," + starttime + "," + DateTime.Now.ToString() + "," + errorstring2 + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)    //point1  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                    }


                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //tilt average 
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2_Avg[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average 
                    {
                        columnTitle = columnTitle + stationRes.MeasureRatio2_Avg[i].ToString("F4") + ",";
                    }


                    for (int i = 0; i < 17; i++)  //tilt average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTiltPercent[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }


                    txtstring2 = columnTitle;
                    sw3.WriteLine(columnTitle);


                    sw3.Close();
                    objFileStream3.Close();
                }
                else
                {
                    FileStream objFileStream4;    //dark file
                    FileName1 = path2 + "\\" + Para.MchName + "_Black_" + stationRes.Unit2Barcode + ".csv";
                    if (!File.Exists(FileName1))
                    {
                        objFileStream4 = new FileStream(FileName1, FileMode.CreateNew, FileAccess.Write);
                        Module2DarkNew = true;
                        Module2DarkNew1 = true;
                    }
                    else
                    {
                        objFileStream4 = new FileStream(FileName1, FileMode.Append, FileAccess.Write);
                    }
                    StreamWriter sw4 = new StreamWriter(objFileStream4, System.Text.Encoding.GetEncoding(-0));   //sw4 dark   
                    idx = GS2SerialCB.SelectedIndex;

                    //////////////////////title//////////////////////////////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "SerialNumber:" + "," + "DataType " + "," + "Color " + "," + "TestID" + "," + "LightSource" + "," + "CAS_SN" + "," + "SW_Version" + "," + "AuditResult" + "," + "StartTime" + "," + "EndTime" + "," + "FailedItem" + ",";
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title1[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title4[i] + ",";
                    }

                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title2[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title5[i] + ",";
                    }

                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title3[i] + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle += Title6[i] + ",";
                    }
                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   TATIO
                    {
                        columnTitle += Title30[i] + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle += Title31[i] + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE    RATIO
                    {
                        columnTitle += Title20[i] + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        columnTitle += Title21[i] + ",";
                    }

                    txtstring4 = columnTitle;

                    /////////////////////uplimited//////////////////////////////////////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_USL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].UpperLimit).ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].UpperLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].UpperLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].UpperLimit.ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16 || i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].UpperLimit.ToString("F4") + ",";
                    }

                    txtstring5 = columnTitle;

                    ////////////////////////lowlimited//////////////////////////////////////
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = "Golden_LSL" + "," + " " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 17; i < (17 + Title1.Length); i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }


                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3
                    {
                        columnTitle = columnTitle + (myData_T[idx].TransRatio[i] + mySpec_T_TP[i].LowerLimit).ToString("F4") + ",";
                    }
                    for (int i = 34; i < (34 + Title1.Length); i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)    //AVERAGE   
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + (((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3) + mySpec_Tilt_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + (((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3) + mySpec_TPercent_Avg[i].LowerLimit).ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title20.Length; i++)    //AVERAGE  RATIO
                    {
                        if (i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Tilt_Ratio[i].LowerLimit.ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title21.Length; i++)
                    {
                        if (i == 13 || i == 14 || i == 15 || i == 16 || i == 1 || i == 2)
                            columnTitle = columnTitle + "NA" + ",";
                        else
                            columnTitle = columnTitle + mySpec_Black_TP[i].LowerLimit.ToString("F4") + ",";
                    }

                    txtstring6 = columnTitle;
                    if (Module2DarkNew == true)
                    {
                        Module2DarkNew = false;
                        sw4.WriteLine(txtstring4);
                        sw4.WriteLine(txtstring5);
                        sw4.WriteLine(txtstring6);
                    }

                    //////////////////////////
                    txtstring1 = ""; txtstring2 = ""; txtstring3 = ""; txtstring4 = ""; txtstring5 = ""; txtstring6 = "";
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "GolddenData " + "," + "NA " + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + "," + "NA" + ",";
                    for (int i = 0; i < Title1.Length; i++)    //point1
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";        //T                      
                    }
                    for (int i = 0; i < Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";  //Tilt
                    }


                    for (int i = 17; i < 17 + Title1.Length; i++)    //point2
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < 17 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < 34 + Title1.Length; i++)    //point3
                    {
                        columnTitle = columnTitle + myData_T[idx].TransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < 34 + Title1.Length; i++)
                    {
                        columnTitle = columnTitle + myData_C[idx].TransRatio[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < Title30.Length; i++)     //AVG REF
                    {
                        columnTitle = columnTitle + ((myData_C[idx].TransRatio[i] + myData_C[idx].TransRatio[i + 17] + myData_C[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title31.Length; i++)
                    {
                        columnTitle = columnTitle + ((myData_T[idx].TransRatio[i] + myData_T[idx].TransRatio[i + 17] + myData_T[idx].TransRatio[i + 34]) / 3).ToString("F4") + ",";
                    }

                    for (int i = 0; i < 34; i++)
                    {
                        columnTitle = columnTitle + "NA" + ",";
                    }

                    txtstring3 = columnTitle;
                    if (Module2DarkNew1 == true)
                    {
                        Module2DarkNew1 = false;
                        sw4.WriteLine(columnTitle);
                    }

                    string okORfail = "Pass";
                    myData_T[idx].MeasureResult = true;    //test result true
                    if (errorstring2 != "")
                    {
                        okORfail = "Fail";
                        myData_T[idx].MeasureResult = false;    //test result true
                    }
                    columnTitle = "";
                    standdardref = "";
                    columnTitle = stationRes.Unit1Barcode + "," + "AuditTest" + "," + "Black " + "," + (Para.MchName + "_" + (stationID + 1).ToString()) + "," + Para.LightSourceType + "," + specMgr.SpecList[0].specType + "_" + specMgr.SpecList[0].serial + "," + Para.SWVersion + "," + okORfail + "," + starttime + "," + DateTime.Now.ToString() + "," + errorstring2 + ",";


                    for (int i = 0; i < Title1.Length; i++)    //point1   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 0; i < Title1.Length; i++)    //point1  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                    }


                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 17; i < (17 + Title1.Length); i++)    //point2  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                    }


                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3   t
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTransRatio[i].ToString("F4") + ",";
                    }
                    for (int i = 34; i < (34 + Title1.Length); i++)    //point3  tilt
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //tilt average 
                    {
                        columnTitle = columnTitle + stationRes.SelectCount2_Avg[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average 
                    {
                        columnTitle = columnTitle + stationRes.MeasureRatio2_Avg[i].ToString("F4") + ",";
                    }


                    for (int i = 0; i < 17; i++)  //tilt average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTiltPercent[i].ToString("F4") + ",";
                    }

                    for (int i = 0; i < 17; i++)  //t% average ratio
                    {
                        columnTitle = columnTitle + myData_T[idx].MeasTPercent[i].ToString("F4") + ",";
                    }
                    txtstring4 = columnTitle;
                    sw4.WriteLine(columnTitle);

                    sw4.Close();
                    objFileStream4.Close();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        List<CalibrationData> myData_T = new List<CalibrationData>();
        List<CalibrationData> myData_C = new List<CalibrationData>();
        private void LoadGoldenSampleInformation()
        {
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fileName = CalibrationFolder + "Golden_Average_T.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                for (int i = 1; i <= 51; i++)
                {
                    tmpData.TransRatio.Add(float.Parse(row[i]));
                    tmpData.MeasTiltPercent.Add(float.Parse(row[i]));
                    tmpData.MeasTPercent.Add(float.Parse(row[i]));
                }
                //for (int i = 70; i <= 86; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 105; i <= 121; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 140; i <= 156; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 175; i <= 191; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                myData_T.Add(tmpData);
            }

            //
            //
            CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fileName = CalibrationFolder + "Golden_Average_Tilt.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sr = new StreamReader(fileName);
            Rowcnt = 0;
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

                for (int i = 1; i <= 51; i++)
                    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 70; i <= 86; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 105; i <= 121; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 140; i <= 156; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                //for (int i = 175; i <= 191; i++)
                //    tmpData.TransRatio.Add(float.Parse(row[i]));
                myData_C.Add(tmpData);
            }

        }
        List<CalibrationSpec> mySpec_Tilt_Ratio = new List<CalibrationSpec>();
        List<CalibrationSpec> mySpec_White_TP = new List<CalibrationSpec>();
        List<CalibrationSpec> mySpec_Black_TP = new List<CalibrationSpec>();
        List<CalibrationSpec> mySpec_T_TP = new List<CalibrationSpec>();
        List<CalibrationSpec> mySpec_Tilt_Avg = new List<CalibrationSpec>();
        List<CalibrationSpec> mySpec_TPercent_Avg = new List<CalibrationSpec>();


        private void LoadGoldenSampleSpec()
        {
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fileName = CalibrationFolder + "Tilt_Ratio_Ref_Sample_Spec.csv";
            string line;
            string[] row;
            int Rowcnt = 0;

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample Spec File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StreamReader sr = new StreamReader(fileName);
            Rowcnt = 0;
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

                mySpec_Tilt_Ratio.Add(tmpData);
            }


            //
            //

            fileName = CalibrationFolder + "White_Ref_Sample_Spec.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample Spec File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sr = new StreamReader(fileName);
            Rowcnt = 0;
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

                mySpec_White_TP.Add(tmpData);
            }

            //
            //


            fileName = CalibrationFolder + "Black_Ref_Sample_Spec.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample Spec File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sr = new StreamReader(fileName);
            Rowcnt = 0;
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

                mySpec_Black_TP.Add(tmpData);
            }

            //
            //
            fileName = CalibrationFolder + "T_Ref_Sample_Spec.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample Spec File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sr = new StreamReader(fileName);
            Rowcnt = 0;
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

                mySpec_T_TP.Add(tmpData);
            }


            //
            //
            fileName = CalibrationFolder + "T%_Avg_Ref_Sample_Spec.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample Spec File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sr = new StreamReader(fileName);
            Rowcnt = 0;
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

                mySpec_TPercent_Avg.Add(tmpData);
            }

            //
            //
            fileName = CalibrationFolder + "Tilt_Avg_Ref_Sample_Spec.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Golden Sample Spec File Not Found.", "Golden Unit Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sr = new StreamReader(fileName);
            Rowcnt = 0;
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
                mySpec_Tilt_Avg.Add(tmpData);
            }
        }

        private void GS1SerialCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGS1UI();
        }

        private void GS2SerialCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGS2UI();
        }

        StationModule stationRes = new StationModule();

        private void MyFunction()
        {
            errorstring1 = null;
            errorstring2 = null;

            if (StationIndex == 2)
            {
                if (ColorSelection == 8)
                {
                    if (GS1ResLb2.Text == "waitting" || GS2ResLb2.Text == "waitting")
                    {
                        string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        while (Mod1Barcode != "" && Mod2Barcode != "")
                        {
                            MessageBox.Show("请取走点检样品.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                            Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        }
                        if (!Para.myMain.RotMgr.IndexRotaryMotion())
                        {
                            MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                    if (GS1ResLb2.Text == "Fail" || GS2ResLb2.Text == "Fail")
                    {
                    }
                }
                if (ColorSelection == 3)
                {
                    if (GS1ResLb6.Text == "waitting" || GS2ResLb6.Text == "waitting")
                    {
                        string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        while (Mod1Barcode != "" && Mod2Barcode != "")
                        {
                            MessageBox.Show("请取走点检样品.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                            Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        }
                        if (!Para.myMain.RotMgr.IndexRotaryMotion())
                        {
                            MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                    if (GS1ResLb6.Text == "Fail" || GS2ResLb6.Text == "Fail")
                    {
                    }
                }
            }


            if (StationIndex == 3)
            {
                if (ColorSelection == 8)
                {
                    if (GS1ResLb3.Text == "waitting" || GS2ResLb3.Text == "waitting")
                    {
                        string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        while (Mod1Barcode != "" && Mod2Barcode != "")
                        {
                            MessageBox.Show("请取走点检样品.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                            Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        }
                        if (!Para.myMain.RotMgr.IndexRotaryMotion())
                        {
                            MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                    if (GS1ResLb3.Text == "Fail" || GS2ResLb3.Text == "Fail")
                    {
                    }
                }
                if (ColorSelection == 3)
                {

                    if (GS1ResLb7.Text == "waitting" || GS2ResLb7.Text == "waitting")
                    {
                        string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        while (Mod1Barcode != "" && Mod2Barcode != "")
                        {
                            MessageBox.Show("请取走点检样品.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                            Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        }
                        if (!Para.myMain.RotMgr.IndexRotaryMotion())
                        {
                            MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                    if (GS1ResLb7.Text == "Fail" || GS2ResLb7.Text == "Fail")
                    {
                    }
                }

            }
            if (StationIndex == 4)
            {
                if (ColorSelection == 8)
                {
                    if (GS1ResLb4.Text == "waitting" || GS2ResLb4.Text == "waitting")
                    {
                        string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        while (Mod1Barcode != "" && Mod2Barcode != "")
                        {
                            MessageBox.Show("请取走点检样品.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                            Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        }
                        if (!Para.myMain.RotMgr.IndexRotaryMotion())
                        {
                            MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                    if (GS1ResLb4.Text == "Fail" || GS2ResLb4.Text == "Fail")
                    {
                    }
                }
                if (ColorSelection == 3)
                {
                    if (GS1ResLb8.Text == "waitting" || GS2ResLb8.Text == "waitting")
                    {
                        string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        while (Mod1Barcode != "" && Mod2Barcode != "")
                        {
                            MessageBox.Show("请取走点检样品.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                            Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        }
                        if (!Para.myMain.RotMgr.IndexRotaryMotion())
                        {
                            MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                    if (GS1ResLb8.Text == "Fail" || GS2ResLb8.Text == "Fail")
                    {
                    }
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

            MessageBox.Show("请放上点检片 ", "Golden Unit Calibration Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //CalibrationBtnPressed = true;
            if (true)
            {
                string Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();

                if (Mod1Barcode == "")
                {
                    MessageBox.Show("没扫上二维码，再试一次", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                    if (Mod1Barcode == "")
                    {
                        MessageBox.Show("没扫上二维码，再试一次", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Mod1Barcode = Para.myMain.BarCMgr.barcodeList[0].Read();
                        if (Mod1Barcode == "")
                        {
                            MessageBox.Show("没扫上二维码，再试一次", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                }
                string Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();

                if (Mod2Barcode == "")
                {
                    MessageBox.Show("没扫上二维码，再试一次", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                    if (Mod2Barcode == "")
                    {
                        MessageBox.Show("没扫上二维码，再试一次", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Mod2Barcode = Para.myMain.BarCMgr.barcodeList[1].Read();
                        if (Mod2Barcode == "")
                        {
                            MessageBox.Show("没扫上二维码，再试一次", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                }

                if (GS1SerialCB.Items.Contains(Mod1Barcode.Substring(0, 44)))
                {
                    if (StationIndex == 2 || StationIndex == 3 || StationIndex == 4)
                    {
                        if (stationRes.Unit1Barcode != Mod1Barcode.Substring(0, 44))
                        {
                            MessageBox.Show("点检片不一致.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }
                    GS1SerialCB.SelectedIndex = GS1SerialCB.Items.IndexOf(Mod1Barcode.Substring(0, 44));
                    stationRes.Unit1Barcode = GS1SerialCB.Items[GS1SerialCB.SelectedIndex].ToString();
                    LoadGS1UI();
                    Application.DoEvents();
                }
                else
                {
                    MessageBox.Show("Module 1 Unit is Not Golden Sample.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StationIndex = 0;
                    return;
                }

                if (GS2SerialCB.Items.Contains(Mod2Barcode.Substring(0, 44)))
                {
                    if (StationIndex == 2 || StationIndex == 3 || StationIndex == 4)
                    {
                        if (stationRes.Unit2Barcode != Mod2Barcode.Substring(0, 44))
                        {
                            MessageBox.Show("点检片不一致.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            StationIndex = 0;
                            return;
                        }
                    }

                    GS2SerialCB.SelectedIndex = GS2SerialCB.Items.IndexOf(Mod2Barcode.Substring(0, 44));
                    stationRes.Unit2Barcode = GS2SerialCB.Items[GS2SerialCB.SelectedIndex].ToString();
                    LoadGS2UI();
                    Application.DoEvents();
                }
                else
                {
                    MessageBox.Show("Module 2 Unit is Not Golden Sample.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StationIndex = 0;
                    return;
                }
            }

            if (ColorSelection == 8)    //white
            {
                int barcodeLength = stationRes.Unit1Barcode.Length;
                string blackorwhite = stationRes.Unit1Barcode.Substring(barcodeLength - 1, 1);
                int barcodeLength1 = stationRes.Unit2Barcode.Length;
                string blackorwhite1 = stationRes.Unit2Barcode.Substring(barcodeLength - 1, 1);
                if ((blackorwhite != "8") || (blackorwhite1 != "8"))
                {
                    MessageBox.Show("不是白色点检片", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StationIndex = 0;
                    return;
                }
            }

            if (ColorSelection == 3)    //dark
            {
                int barcodeLength = stationRes.Unit1Barcode.Length;
                string blackorwhite = stationRes.Unit1Barcode.Substring(barcodeLength - 1, 1);
                int barcodeLength1 = stationRes.Unit2Barcode.Length;
                string blackorwhite1 = stationRes.Unit2Barcode.Substring(barcodeLength - 1, 1);
                if ((blackorwhite != "3") || (blackorwhite1 != "3"))
                {
                    MessageBox.Show("不是黑色点检片", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StationIndex = 0;
                    return;
                }
            }


            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, 0);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, 0);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
            CalibrationBtnPressed = true;
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

            Thread.Sleep(500);
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StationIndex = 0;
                return;
            }
            Para.myMain.ClearInspectionResults();

            if (!InspectMod1())
            {
                MessageBox.Show("Module 1 Inspection Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StationIndex = 0;
                return;
            }

            if (!InspectMod2())
            {
                MessageBox.Show("Module 2 Inspection Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StationIndex = 0;
                return;
            }

            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StationIndex = 0;
                return;
            }

            GetDarkRef();
            Application.DoEvents();

            TestMod();
            if (Para.isOutShutter)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
            }
            else
            {
                specMgr.SetIO(0, true);
                specMgr.SetIO(1, true);
            }

            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StationIndex = 0;
                return;
            }
            if (!Para.myMain.RotMgr.IndexRotaryMotion())
            {
                MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StationIndex = 0;
                return;
            }
            if (Para.Enb45DegTest)
            {
                if (!Para.myMain.RotMgr.IndexRotaryMotion())
                {
                    MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StationIndex = 0;
                    return;
                }
                if (!Para.myMain.RotMgr.IndexRotaryMotion())
                {
                    MessageBox.Show("Rotary Indexing Fail.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StationIndex = 0;
                    return;
                }
            }//20170207

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

            Thread.Sleep(500);

            DisplayResult();

            MessageBox.Show("请取出点检片，按下OK按钮继续检测", "Calibration Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool InspectMod1()
        {
            string strread1 = "";
            FileOperation.ReadData(Para.CurLoadConfigFileName, "ExposureTime", "disableAutoExpTime1", ref strread1);
            if (strread1 == "False")
            {
                int barcodeLength = stationRes.Unit1Barcode.Length;
                string blackorwhite = stationRes.Unit1Barcode.Substring(barcodeLength - 1, 1);
                if (blackorwhite == "3")//Black
                {
                    Para.myMain.camera1.SetExposure(Para.Cam1ExposureTimeB);
                }
                else
                {
                    Para.myMain.camera1.SetExposure(Para.Cam1ExposureTimeW);

                }
            }
            else
            {
                if (Para.selected1BorW == 0)
                {
                    Para.myMain.camera1.SetExposure(Para.Cam1ExposureTimeB);
                }
                else
                {
                    Para.myMain.camera1.SetExposure(Para.Cam1ExposureTimeW);
                }
            }
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
            string strread1 = "";
            FileOperation.ReadData(Para.CurLoadConfigFileName, "ExposureTime", "disableAutoExpTime2", ref strread1);
            if (strread1 == "False")
            {
                int barcodeLength = stationRes.Unit2Barcode.Length;
                string blackorwhite = stationRes.Unit2Barcode.Substring(barcodeLength - 1, 1);
                if (blackorwhite == "3")//Black
                {
                    Para.myMain.camera2.SetExposure(Para.Cam2ExposureTimeB);
                }
                else
                {
                    Para.myMain.camera2.SetExposure(Para.Cam2ExposureTimeW);
                }
            }
            else
            {
                if (Para.selected2BorW == 0)
                {

                    Para.myMain.camera2.SetExposure(Para.Cam2ExposureTimeB);
                }

                else
                {
                    Para.myMain.camera2.SetExposure(Para.Cam2ExposureTimeW);
                }
            }
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
        private bool TestDarkRef(List<float> darkvalue)
        {
            bool res = true;    //
            double temp = 0.0;
            float avg = (darkvalue[248] + darkvalue[249] + darkvalue[250] + darkvalue[251]) / 4;
            for (int i = 0; i < darkvalue.Count; i++)
            {
                temp = darkvalue[i] / avg;
                if (temp < 0.95 || temp > 1.05)
                    res = false;
            }

            return res;
        }

        public void SaveDarkError1()
        {
            string columnTitle = "";
            string s_filename = "D:\\ErrorDark";
            if (!Directory.Exists(s_filename))
            {
                Directory.CreateDirectory(s_filename);
            }
            FileStream objFileStream1;
            string FileName = s_filename + "\\" + Para.MchName + "ErrorDark_Module1" + ".csv";
            if (!File.Exists(FileName))
            {
                objFileStream1 = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                CreateNew1_ErrorDark = true;

            }
            else
            {
                objFileStream1 = new FileStream(FileName, FileMode.Append, FileAccess.Write);
            }
            StreamWriter sw1 = new StreamWriter(objFileStream1, System.Text.Encoding.GetEncoding(-0));
            try
            {
                columnTitle = "";
                if (CreateNew1_ErrorDark)
                {
                    CreateNew1_ErrorDark = false;
                    columnTitle = "WL" + ",";
                    for (int i = 0; i < stationRes.WLMod1Dark.Count; i++)
                    {
                        columnTitle += stationRes.WLMod1Dark[i].ToString("F4") + ",";   //WL
                    }
                    sw1.WriteLine(columnTitle);
                }
                columnTitle = "";
                columnTitle = "COUNT" + ",";
                for (int i = 0; i < stationRes.WLMod1Dark.Count; i++)
                {
                    columnTitle += stationRes.DarkRefMod1[i].ToString("F4") + ",";  //Count
                }
                sw1.WriteLine(columnTitle);
                sw1.Close();
                objFileStream1.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                sw1.Close();
                objFileStream1.Close();
            }
        }

        public void SaveDarkError2()
        {
            string columnTitle = "";
            string s_filename = "D:\\ErrorDark";
            if (!Directory.Exists(s_filename))
            {
                Directory.CreateDirectory(s_filename);
            }
            FileStream objFileStream2;
            string FileName = s_filename + "\\" + Para.MchName + "ErrorDark_Module2" + ".csv";
            if (!File.Exists(FileName))
            {
                objFileStream2 = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                CreateNew2_ErrorDark = true;

            }
            else
            {
                objFileStream2 = new FileStream(FileName, FileMode.Append, FileAccess.Write);
            }
            StreamWriter sw2 = new StreamWriter(objFileStream2, System.Text.Encoding.GetEncoding(-0));
            try
            {
                columnTitle = "";
                if (CreateNew2_ErrorDark)
                {
                    CreateNew2_ErrorDark = false;
                    columnTitle = "WL" + ",";
                    for (int i = 0; i < stationRes.WLMod2Dark.Count; i++)
                    {
                        columnTitle += stationRes.WLMod2Dark[i].ToString("F4") + ",";   //WL
                    }
                    sw2.WriteLine(columnTitle);
                }
                columnTitle = "";
                columnTitle = "COUNT" + ",";
                for (int i = 0; i < stationRes.WLMod2Dark.Count; i++)
                {
                    columnTitle += stationRes.DarkRefMod2[i].ToString("F4") + ",";  //Count
                }
                sw2.WriteLine(columnTitle);
                sw2.Close();
                objFileStream2.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                sw2.Close();
                objFileStream2.Close();
            }
        }

        bool CreateNew1_ErrorDark = false, CreateNew2_ErrorDark = false;

        private bool GetDarkRef()
        {
            if (Para.isOutShutter)
            {
                if (motionMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS))
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
                }
            }
            else
            {
                specMgr.SetIO(0, true);
                specMgr.SetIO(1, true);
            }
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(1500);
                Para.myMain.UpdateMod1TestStatus("Dark Test", Color.Lime);
                stationRes.DarkRefMod1 = specMgr.GetCount(0);
                stationRes.WLMod1Dark = specMgr.GetWaveLength(0);
                Para.myMain.UpdateMod1Chart(stationRes.WLMod1Dark, stationRes.DarkRefMod1, false);
                if (!TestDarkRef(stationRes.DarkRefMod1))
                {
                    SaveDarkError1();
                    MessageBox.Show("Dark Error");
                }
                break;
            }

            for (int i = 0; i < 3; i++)
            {
                Para.myMain.UpdateMod2TestStatus("Dark Test", Color.Lime);
                stationRes.DarkRefMod2 = specMgr.GetCount(1);
                stationRes.WLMod2Dark = specMgr.GetWaveLength(1);
                Para.myMain.UpdateMod2Chart(stationRes.WLMod2Dark, stationRes.DarkRefMod2, false);
                if (!TestDarkRef(stationRes.DarkRefMod2))
                {
                    SaveDarkError2();
                    MessageBox.Show("Dark Error");
                }
                break;
            }

            if (Para.isOutShutter)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
            }
            else
            {
                specMgr.SetIO(0, false);
                specMgr.SetIO(1, false);
            }
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
                if (Para.Enb3TestPtOnly)
                {
                    if ((i == 0) || (i == 4))
                    {
                        //XY.Add(new DPoint(0, 0));
                        continue;
                    }
                    //}//20170211
                }

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
                Application.DoEvents();
                //SaveRefDarkTransData(stationRes.Unit1Barcode, 400, 1100, i + 1, stationRes.WLMod1Dark, stationRes.DarkRefMod1,
                //                     stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i], stationRes.transRatioMod1[i],1);

                //SaveRawData(stationRes.Unit1Barcode, 400, 1100, i + 1, stationRes.WLMod1Dark, stationRes.WLMod1Dark, stationRes.WLMod1Dark,
                //    stationRes.DarkRefMod1, stationRes.WhiteRefMod1, stationRes.MeasDataMod1[i], 1, stationRes.mod1VisResult);

                //SaveRefDarkTransData(stationRes.Unit2Barcode, 400, 1100, i + 1, stationRes.WLMod2Dark, stationRes.DarkRefMod2,
                //                     stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i], stationRes.transRatioMod2[i], 2);               

                //SaveRawData(stationRes.Unit2Barcode, 400, 1100, i + 1, stationRes.WLMod2Dark, stationRes.WLMod2Dark, stationRes.WLMod2Dark,
                //    stationRes.DarkRefMod2, stationRes.WhiteRefMod2, stationRes.MeasDataMod2[i], 2, stationRes.mod2VisResult);             
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

        public void SaveRawData(string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WLDark, List<float> WLWhite, List<float> WLMeas,
                                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData, int ModuleIndex, JPTCG.Vision.HalconInspection.RectData VisResult)
        {
            //lock (sny_Obj)
            {
                string s_FileName = barCode + "_RawTransData_" + DateTime.Now.ToString("yyyyMMdd");

                string path = "D:\\DailyGoldenData";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = "D:\\DailyGoldenData\\Module" + (ModuleIndex).ToString();
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

        public void SaveRefDarkTransData(string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WL,
                                                 List<float> darkRef, List<float> WhiteRef, List<float> MeasData, List<float> TransData, int ModuleIndex)
        {
            string s_FileName = barCode + "_TransData_" + DateTime.Now.ToString("yyyyMMdd");

            string path = "D:\\DailyGoldenData";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = "D:\\DailyGoldenData\\Module" + (ModuleIndex).ToString();
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


        // Light Source
        List<NDFileterInfo> LSDataInfo = new List<NDFileterInfo>();

        public void CreateLightSourceCalibrationFile(List<float> Mod1LSWL, List<float> Mod1LSCount, List<float> Mod2LSWL, List<float> Mod2LSCount, float LowerLimit, float UpperLimit)
        {
            string exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                //MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
                Directory.CreateDirectory(CalibrationFolder);
            }

            string fileName = CalibrationFolder + "LS_Ref_Data.csv";


            if (File.Exists(fileName))
            {
                //if (MessageBox.Show(fileName+" Light Source Calibration File Exist. Overwrite File?","Save Light Source Calibration File", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                //    return;
                File.Delete(fileName);
            }

            FileStream objFileStream;
            objFileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
            string columnTitle = "";
            try
            {
                columnTitle = "WaveLength" + ",Module1,Min%, Max%, Modue2,Min%, Max%";
                sw.WriteLine(columnTitle);
                columnTitle = "";

                int stWaveLength = 400;
                int endWaveLength = 1100;
                int i = 0;
                int Mod1Idx = 0;
                int Mod1S = 0;
                int Mod2Idx = 0;
                int Mod2S = 0;
                for (i = stWaveLength; i <= endWaveLength; i++)
                {
                    columnTitle = i.ToString() + ",";
                    Mod1Idx = NearestIndex(Mod1S, i, Mod1LSWL);
                    Mod1S = Mod1Idx;
                    columnTitle += Mod1LSCount[Mod1Idx].ToString("F4") + ",";
                    columnTitle += LowerLimit.ToString("F2") + ",";
                    columnTitle += UpperLimit.ToString("F2") + ",";
                    Mod2Idx = NearestIndex(Mod2S, i, Mod2LSWL);
                    Mod2S = Mod2Idx;
                    columnTitle += Mod2LSCount[Mod2Idx].ToString("F4") + ",";
                    columnTitle += LowerLimit.ToString("F2") + ",";
                    columnTitle += UpperLimit.ToString("F2") + ",";
                    sw.WriteLine(columnTitle);//2016010711
                }

            }
            catch (Exception e)
            {
            }
            finally
            {
                sw.Close();
                objFileStream.Close();
            }

            MessageBox.Show("Light Source Calibration File Created.", "Light Source Calibration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void CreateFileBtn_Click(object sender, EventArgs e)
        {
            motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
            Thread.Sleep(500);
            specMgr.SetIO(0, false);
            specMgr.SetIO(1, false);
            List<float> Mod1WL = new List<float>();
            List<float> Mod1Cnt = new List<float>();
            List<float> Mod2WL = new List<float>();
            List<float> Mod2Cnt = new List<float>();

            Mod1WL = specMgr.GetWaveLength(0);
            Mod1Cnt = specMgr.GetCount(0);
            Mod2WL = specMgr.GetWaveLength(1);
            Mod2Cnt = specMgr.GetCount(1);

            specMgr.SetIO(0, true);
            specMgr.SetIO(1, true);

            CreateLightSourceCalibrationFile(Mod1WL, Mod1Cnt, Mod2WL, Mod2Cnt, float.Parse(LSLowerTB.Text), float.Parse(LSUpperTB.Text));

        }
        private void LoadLSCalibrationFile()
        {

            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fileName = CalibrationFolder + "LS_Ref_Data.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("ND Calibration File Not Found." + fileName, "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string strread = "";

            LSDataInfo.Clear();
            //LSDataInfo[0].data = new List<CalibrationInfo>();
            //LSDataInfo[1].data = new List<CalibrationInfo>();
            LSDataInfo.Add(new NDFileterInfo());
            LSDataInfo.Add(new NDFileterInfo());

            StreamReader sr = new StreamReader(fileName);
            string line;
            string[] row;
            int Rowcnt = 0;
            bool isFirst = true;
            while ((line = sr.ReadLine()) != null)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                row = line.Split(',');
                if (row[0] == "")
                    continue;

                {
                    int Idx = 1;
                    CalibrationInfo Mod1Cal = new CalibrationInfo();
                    Mod1Cal.waveLength = int.Parse(row[0]);
                    Mod1Cal.Nominal = double.Parse(row[Idx]);
                    Mod1Cal.min = double.Parse(row[Idx + 1]);
                    Mod1Cal.max = double.Parse(row[Idx + 2]);
                    LSDataInfo[0].data.Add(Mod1Cal);

                    Idx = 4;
                    CalibrationInfo Mod2Cal = new CalibrationInfo();
                    Mod2Cal.waveLength = int.Parse(row[0]);
                    Mod2Cal.Nominal = double.Parse(row[Idx]);
                    Mod2Cal.min = double.Parse(row[Idx + 1]);
                    Mod2Cal.max = double.Parse(row[Idx + 2]);
                    LSDataInfo[1].data.Add(Mod2Cal);
                }
            }

            // UpdateCaliUI();
            UpdateLSCaliLV();
        }
        private void InitLSCaliLV()
        {
            LSMod1LV.Clear();
            LSMod1LV.Columns.Add("", 20);
            LSMod1LV.Columns.Add("wavelength", 70);
            LSMod1LV.Columns.Add("Min %", 50);
            LSMod1LV.Columns.Add("Max %", 50);
            LSMod1LV.Columns.Add("Nominal", 50);
            LSMod1LV.Columns.Add("Measure", 60);
            LSMod1LV.Columns.Add("Result", 60);
            this.LSMod1LV.View = System.Windows.Forms.View.Details;

            LSMod2LV.Clear();
            LSMod2LV.Columns.Add("", 20);
            LSMod2LV.Columns.Add("wavelength", 70);
            LSMod2LV.Columns.Add("Min %", 50);
            LSMod2LV.Columns.Add("Max %", 50);
            LSMod2LV.Columns.Add("Nominal", 50);
            LSMod2LV.Columns.Add("Measure", 60);
            LSMod2LV.Columns.Add("Result", 60);
            this.LSMod2LV.View = System.Windows.Forms.View.Details;
        }
        private void UpdateLSCaliLV()
        {
            InitLSCaliLV();
            int Idx = 0;
            //CaliInfo
            for (int i = 0; i < LSDataInfo[Idx].data.Count; i++)
            {
                ListViewItem item = LSMod1LV.Items.Add(LSMod1LV.Items.Count + "");
                item.SubItems.Add(LSDataInfo[Idx].data[i].waveLength.ToString());
                item.SubItems.Add(LSDataInfo[Idx].data[i].min.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].max.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].Nominal.ToString("F2"));
                item.EnsureVisible();
            }
            Idx = 1;
            //CaliInfo
            for (int i = 0; i < LSDataInfo[Idx].data.Count; i++)
            {
                ListViewItem item = LSMod2LV.Items.Add(LSMod2LV.Items.Count + "");
                item.SubItems.Add(LSDataInfo[Idx].data[i].waveLength.ToString());
                item.SubItems.Add(LSDataInfo[Idx].data[i].min.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].max.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].Nominal.ToString("F2"));
                item.EnsureVisible();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((specMgr.GetType(0) == SpectType.NoSpectrometer) || (specMgr.GetType(1) == SpectType.NoSpectrometer))
            {
                MessageBox.Show("Module Spectromter Not Assigned.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Remove All Unit from Test Module", "Calibration Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            motionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
            Thread.Sleep(500);
            specMgr.SetIO(0, false);
            specMgr.SetIO(1, false);
            List<float> Mod1WL = new List<float>();
            List<float> Mod1Cnt = new List<float>();
            List<float> Mod2WL = new List<float>();
            List<float> Mod2Cnt = new List<float>();
            Mod1WL = specMgr.GetWaveLength(0);
            Mod1Cnt = specMgr.GetCount(0);
            Mod2WL = specMgr.GetWaveLength(1);
            Mod2Cnt = specMgr.GetCount(1);

            int sIdx = 0;
            int idx = 0;
            LoadLSCalibrationFile();
            InitLSCaliLV();
            List<float> NDTransData = new List<float>();

            int Idx = 0;
            bool Mod1Fail = false;
            for (int i = 0; i < LSDataInfo[Idx].data.Count; i++)
            {
                ListViewItem item = LSMod1LV.Items.Add(LSMod1LV.Items.Count + "");
                item.SubItems.Add(LSDataInfo[Idx].data[i].waveLength.ToString());
                item.SubItems.Add(LSDataInfo[Idx].data[i].min.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].max.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].Nominal.ToString("F2"));

                idx = NearestIndex(sIdx, LSDataInfo[Idx].data[i].waveLength, Mod1WL);
                sIdx = idx;

                item.SubItems.Add(Mod1Cnt[idx].ToString("F2"));
                item.UseItemStyleForSubItems = false;

                double min = LSDataInfo[Idx].data[i].Nominal * (LSDataInfo[Idx].data[i].min / 100);
                double max = LSDataInfo[Idx].data[i].Nominal * (LSDataInfo[Idx].data[i].max / 100);
                double diff = Mod1Cnt[idx] - LSDataInfo[Idx].data[i].Nominal;

                if ((diff < min) || (diff > max))
                {
                    item.SubItems.Add("Fail");
                    item.SubItems[6].BackColor = Color.Red;
                    Mod1Fail = true;
                }
                else
                {
                    item.SubItems.Add("Pass");
                    item.SubItems[6].BackColor = Color.Lime;
                }
                item.EnsureVisible();
            }

            Idx = 1;
            bool Mod2Fail = false;
            for (int i = 0; i < LSDataInfo[Idx].data.Count; i++)
            {
                ListViewItem item = LSMod2LV.Items.Add(LSMod2LV.Items.Count + "");
                item.SubItems.Add(LSDataInfo[Idx].data[i].waveLength.ToString());
                item.SubItems.Add(LSDataInfo[Idx].data[i].min.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].max.ToString("F2"));
                item.SubItems.Add(LSDataInfo[Idx].data[i].Nominal.ToString("F2"));

                idx = NearestIndex(sIdx, LSDataInfo[Idx].data[i].waveLength, Mod2WL);
                sIdx = idx;

                item.SubItems.Add(Mod2Cnt[idx].ToString("F2"));
                item.UseItemStyleForSubItems = false;

                double min = LSDataInfo[Idx].data[i].Nominal * (LSDataInfo[Idx].data[i].min / 100);
                double max = LSDataInfo[Idx].data[i].Nominal * (LSDataInfo[Idx].data[i].max / 100);
                double diff = Mod2Cnt[idx] - LSDataInfo[Idx].data[i].Nominal;

                if ((diff < min) || (diff > max))
                {
                    item.SubItems.Add("Fail");
                    item.SubItems[6].BackColor = Color.Red;
                    Mod2Fail = true;
                }
                else
                {
                    item.SubItems.Add("Pass");
                    item.SubItems[6].BackColor = Color.Lime;
                }
                item.EnsureVisible();
            }

            if (Mod1Fail)
            {
                LSMod1Lbl.Text = "Fail";
                LSMod1Lbl.ForeColor = Color.Red;
            }
            else
            {
                LSMod1Lbl.Text = "Pass";
                LSMod1Lbl.ForeColor = Color.Lime;
            }

            if (Mod2Fail)
            {
                LSMod2Lbl.Text = "Fail";
                LSMod2Lbl.ForeColor = Color.Red;
            }
            else
            {
                LSMod2Lbl.Text = "Pass";
                LSMod2Lbl.ForeColor = Color.Lime;
            }
            MessageBox.Show("Light Source Calibration Completed.", "Calibration Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //private void Enter(object sender, EventArgs e)
        //{
        //    LoadLSCalibrationFile();
        //}

        private void Enter_Tab(object sender, EventArgs e)
        {
            //LoadLSCalibrationFile();
        }

        bool CalibrationBtnPressed = false;
        private void CaliForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CalibrationBtnPressed)
            {
                if (GS1ResLbl.Text.Contains("Pass") && GS1ResLb2.Text.Contains("Pass") && GS1ResLb3.Text.Contains("Pass") && GS1ResLb4.Text.Contains("Pass")
                    && GS1ResLb5.Text.Contains("Pass") && GS1ResLb6.Text.Contains("Pass") && GS1ResLb7.Text.Contains("Pass") && GS1ResLb8.Text.Contains("Pass")
                    && GS2ResLb1.Text.Contains("Pass") && GS2ResLb2.Text.Contains("Pass") && GS2ResLb3.Text.Contains("Pass") && GS2ResLb4.Text.Contains("Pass")
                    && GS2ResLb5.Text.Contains("Pass") && GS2ResLb6.Text.Contains("Pass") && GS2ResLb7.Text.Contains("Pass") && GS2ResLb8.Text.Contains("Pass"))
                {
                    FileOperation.SaveData(Para.MchConfigFileName, "ContinueRunTime", "Time", DateTime.Now.ToString());
                    Para.SystemRunTime = DateTime.Now;
                    MessageBox.Show("Dailycheck successed,please reset machine");
                    Para.myMain.OnlyHomeEnb();
                }
                else
                {
                    MessageBox.Show("Dailycheck not successed");
                    Para.myMain.AllDisabled();
                }
            }
            else
            {
                MessageBox.Show("Dailycheck did not excute,Please reset machine");
                Para.myMain.OnlyHomeEnb();
            }
            CalibrationBtnPressed = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            StationIndex = 1;
            if (StationIndex == 1)
            {
                if (MessageBox.Show("是否开始点检 ?", "Golden Unit Calibration Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else
                {
                    GS1ResLbl.Text = "waitting";
                    GS1ResLbl.ForeColor = Color.Yellow;
                    GS1ResLb2.Text = "waitting";
                    GS1ResLb2.ForeColor = Color.Yellow;
                    GS1ResLb3.Text = "waitting";
                    GS1ResLb3.ForeColor = Color.Yellow;
                    GS1ResLb4.Text = "waitting";
                    GS1ResLb4.ForeColor = Color.Yellow;

                    GS2ResLb1.Text = "waitting";
                    GS2ResLb1.ForeColor = Color.Yellow;
                    GS2ResLb2.Text = "waitting";
                    GS2ResLb2.ForeColor = Color.Yellow;
                    GS2ResLb3.Text = "waitting";
                    GS2ResLb3.ForeColor = Color.Yellow;
                    GS2ResLb4.Text = "waitting";
                    GS2ResLb4.ForeColor = Color.Yellow;
                    Para.myMain.SeqMgr.StartHoming();
                    Application.DoEvents();
                    ColorSelection = 8;
                }
            }
            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }

            //GS1ResLbl.Text = "Pass"; GS2ResLb1.Text = "Pass";

            if (GS1ResLbl.Text.Contains("Fail") || GS2ResLb1.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLbl.Text.Contains("Fail") || GS2ResLb1.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLbl.Text.Contains("Fail") || GS2ResLb1.Text.Contains("Fail"))
            {
                return;
            }



            StationIndex = 2;
            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }
            //GS1ResLb2.Text = "Pass"; GS2ResLb2.Text = "Pass";
            if (GS1ResLb2.Text.Contains("Fail") || GS2ResLb2.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb2.Text.Contains("Fail") || GS2ResLb2.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb2.Text.Contains("Fail") || GS2ResLb2.Text.Contains("Fail"))
            {
                return;
            }



            StationIndex = 3;
            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }
            // GS1ResLb3.Text = "Pass"; GS2ResLb3.Text = "Pass";
            if (GS1ResLb3.Text.Contains("Fail") || GS2ResLb3.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb3.Text.Contains("Fail") || GS2ResLb3.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb3.Text.Contains("Fail") || GS2ResLb3.Text.Contains("Fail"))
            {
                return;
            }


            StationIndex = 4;
            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }
            //GS1ResLb4.Text = "Pass"; GS2ResLb4.Text = "Pass";
            if (GS1ResLb4.Text.Contains("Fail") || GS2ResLb4.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb4.Text.Contains("Fail") || GS2ResLb4.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb4.Text.Contains("Fail") || GS2ResLb4.Text.Contains("Fail"))
            {
                return;
            }
            StationIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StationIndex = 1;
            if (StationIndex == 1)
            {
                if (MessageBox.Show("是否开始点检 ?", "Golden Unit Calibration Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else
                {
                    GS1ResLb5.Text = "waitting";
                    GS1ResLb5.ForeColor = Color.Yellow;
                    GS1ResLb6.Text = "waitting";
                    GS1ResLb6.ForeColor = Color.Yellow;
                    GS1ResLb7.Text = "waitting";
                    GS1ResLb7.ForeColor = Color.Yellow;
                    GS1ResLb8.Text = "waitting";
                    GS1ResLb8.ForeColor = Color.Yellow;


                    GS2ResLb5.Text = "waitting";
                    GS2ResLb5.ForeColor = Color.Yellow;
                    GS2ResLb6.Text = "waitting";
                    GS2ResLb6.ForeColor = Color.Yellow;
                    GS2ResLb7.Text = "waitting";
                    GS2ResLb7.ForeColor = Color.Yellow;
                    GS2ResLb8.Text = "waitting";
                    GS2ResLb8.ForeColor = Color.Yellow;
                    Para.myMain.SeqMgr.StartHoming();
                    Application.DoEvents();
                    ColorSelection = 3;    //color selection
                }
            }


            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }
            //GS1ResLb5.Text = "Pass"; GS2ResLb5.Text = "Pass";
            if (GS1ResLb5.Text.Contains("Fail") || GS2ResLb5.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb5.Text.Contains("Fail") || GS2ResLb5.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb5.Text.Contains("Fail") || GS2ResLb5.Text.Contains("Fail"))
            {
                return;
            }



            StationIndex = 2;
            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }
            // GS1ResLb6.Text = "Pass"; GS2ResLb6.Text = "Pass";
            if (GS1ResLb6.Text.Contains("Fail") || GS2ResLb6.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb6.Text.Contains("Fail") || GS2ResLb6.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb6.Text.Contains("Fail") || GS2ResLb6.Text.Contains("Fail"))
            {
                return;
            }


            StationIndex = 3;
            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }

            //GS1ResLb7.Text = "Pass"; GS2ResLb7.Text = "Pass";
            if (GS1ResLb7.Text.Contains("Fail") || GS2ResLb7.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb7.Text.Contains("Fail") || GS2ResLb7.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb7.Text.Contains("Fail") || GS2ResLb7.Text.Contains("Fail"))
            {
                return;
            }



            StationIndex = 4;
            MyFunction();
            if (StationIndex == 0)
            {
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                return;
            }
            //GS1ResLb8.Text = "Pass"; GS2ResLb8.Text = "Pass";
            if (GS1ResLb8.Text.Contains("Fail") || GS2ResLb8.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb8.Text.Contains("Fail") || GS2ResLb8.Text.Contains("Fail"))
            {
                MessageBox.Show("测试失败,再试一次.", "Golden Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MyFunction();
                if (StationIndex == 0)
                {
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);

                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    motionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    return;
                }
            }
            if (GS1ResLb8.Text.Contains("Fail") || GS2ResLb8.Text.Contains("Fail"))
            {
                return;
            }
            StationIndex = 0;
        }

    }
}
