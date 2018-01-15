using Common;
using JPTCG.Calibration;
using JPTCG.Common;
using JPTCG.Motion;
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

namespace JPTCG.Spectrometer
{
    public partial class WinSpectrometer : Form
    {
        SpectManager myMgr;
        DeltaMotionMgr mtMgr;
        List<float> whiteRef = new List<float>();
        List<float> darkRef = new List<float>();
        List<float> measVal = new List<float>();
        List<float> TransVal = new List<float>();

        public WinSpectrometer(SpectManager MgrObj, DeltaMotionMgr myMotion)
        {
            InitializeComponent();
            myMgr = MgrObj;
            mtMgr = myMotion;
        }

        private void WinSpectrometer_Load(object sender, EventArgs e)
        {
            InitChart(100);
            InitCaliLV();

            ModLB.Items.Clear();
            for (int i = 0; i < myMgr.SpecList.Count; i++)
                ModLB.Items.Add(myMgr.SpecList[i].Name);

            if (ModLB.Items.Count > 0)
            {
                ModLB.SelectedIndex = 0;
                UpdateUI(ModLB.SelectedIndex);
            }

            comboBox5.SelectedIndex = 0;
            LoadNIFilter();
        }        
        private void UpdateUI(int ModIdx)
        {
            SpecSettingsGB.Text = myMgr.SpecList[ModIdx].Name;
            TypeEB.Text = Helper.GetEnumDescription(myMgr.SpecList[ModIdx].specType);
            SerialEB.Text = myMgr.SpecList[ModIdx].serial;
            usedTimeEB.Text = myMgr.SpecList[ModIdx].UsedTime.ToString("h'h 'm'm 's's'");

            if (myMgr.SpecList[ModIdx].specType == SpectType.NoSpectrometer)
            {
                ParametersGB.Enabled = false;
                setIOCB.Enabled = false;
            }
            else
            {
                ParametersGB.Enabled = true;
                setIOCB.Enabled = true;
                StartPixelEB.Text = myMgr.SpecList[ModIdx].StartPixel.ToString();
                StopPixelEB.Text = myMgr.SpecList[ModIdx].EndPixel.ToString();
                IntegrateTimeEB.Text = myMgr.SpecList[ModIdx].IntegrationTime.ToString();
                SmoothPixEB.Text = myMgr.SpecList[ModIdx].SmoothingPixel.ToString();
                NumOfAvgEB.Text = myMgr.SpecList[ModIdx].NumOfAvg.ToString();
            }
            //UpdateTestLV(ModIdx);
        }
        private void UpdateTestLV(int ModIdx)
        {            
            InitTestDataLV();
            int cnt = myMgr.SpecList[ModIdx].Criteria.Count;
            for (int i = 0; i < cnt; i++)
            {
                ListViewItem item = NICaliLV.Items.Add(NICaliLV.Items.Count + "");
                item.SubItems.Add(Convert.ToString(myMgr.SpecList[ModIdx].Criteria[i].WaveLength));
                item.SubItems.Add(myMgr.SpecList[ModIdx].Criteria[i].Min.ToString("F2"));
                item.SubItems.Add(myMgr.SpecList[ModIdx].Criteria[i].Max.ToString("F2"));                
                item.EnsureVisible();
            }
        }
        private void ModLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI(ModLB.SelectedIndex);
        }

        private void InitChart(int MaxVal)
        {
            //Module 1 Chart
            SpecChart.Series[0].Color = Color.Red;//曲线颜色            
            SpecChart.Series[0].BorderWidth = 3;
            SpecChart.Legends[0].Position.X = 10; //标题位置
            SpecChart.Legends[0].Position.Y = 2;

            SpecChart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            SpecChart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            SpecChart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔
            // chart3.ChartAreas["ChartArea1"].AxisX.de
            SpecChart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度

            int ModIdx = ModLB.SelectedIndex;
            if (ModIdx < 0)
            {
                SpecChart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度
            }
            else
            {
                if (myMgr.SpecList[ModLB.SelectedIndex].specType == SpectType.CAS140)
                {
                    SpecChart.ChartAreas["ChartArea1"].AxisY.Maximum = scale;//2000;//最大刻度
                    SpecChart.ChartAreas["ChartArea1"].AxisY.Interval = scale/10;//刻度间隔
                }
                else
                {
                    SpecChart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度
                    SpecChart.ChartAreas["ChartArea1"].AxisY.Interval = 5000;//刻度间隔
                }
            }
            

            //设置坐标轴名称
            SpecChart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            SpecChart.ChartAreas["ChartArea1"].AxisY.Title = "Counts";// "数值";

            //设置网格的颜色
            SpecChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            SpecChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            SpecChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            SpecChart.Series["Series1"].Points.Clear();

        }

        private void InitTransChart()//初始化Chart函数
        {
            SpecChart.Series["Series1"].Points.Clear();
            SpecChart.Series[0].Color = Color.Green;//曲线颜色
            //chart3.Series[0].Name=
            SpecChart.Legends[0].Position.X = 10; //标题位置
            SpecChart.Legends[0].Position.Y = 2;
            //  
            SpecChart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            SpecChart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            SpecChart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔

            SpecChart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            SpecChart.ChartAreas["ChartArea1"].AxisY.Maximum = 100;//最大刻度
            SpecChart.ChartAreas["ChartArea1"].AxisY.Interval = 10;//刻度间隔

            //设置坐标轴名称
            SpecChart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            SpecChart.ChartAreas["ChartArea1"].AxisY.Title = "Trans";// "数值";

            //设置网格的颜色
            SpecChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            SpecChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            SpecChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;



        }
        private void InitTestDataLV()
        {
            NICaliLV.Clear();
            NICaliLV.Columns.Add("", 20);
            NICaliLV.Columns.Add("wavelength", 70);
            NICaliLV.Columns.Add("MinValue", 60);
            NICaliLV.Columns.Add("MaxValue", 60);
            this.NICaliLV.View = System.Windows.Forms.View.Details;
        }
        private void AssignBtn_Click(object sender, EventArgs e)
        {
            if (ModLB.Items.Count <= 0)
                return;
            WinAssignSpec myWin = new WinAssignSpec(myMgr, ModLB.SelectedIndex);
            myWin.ShowDialog();
            myMgr.GetParamtersFromDevice(ModLB.SelectedIndex);
            UpdateUI(ModLB.SelectedIndex);
            myMgr.SaveMachineSettings(Para.MchConfigFileName);
        }

        private void MoveDownBtn_Click(object sender, EventArgs e)
        {
            if (ModLB.SelectedIndex == -1)
                return;
            if (NICaliLV.SelectedIndices.Count == 0)
                return;

            int Idx = NICaliLV.SelectedIndices[0];
            if (Idx >= (myMgr.SpecList[ModLB.SelectedIndex].Criteria.Count - 1))
                return;

            TestCriteria myCri = myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx];
            TestCriteria myCri2 = myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx+1];

            myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx] = myCri2;
            myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx + 1] = myCri;

            UpdateTestLV(ModLB.SelectedIndex);

            //TestLV.SelectedIndices.Add(Idx + 1);
            NICaliLV.Items[Idx + 1].Focused = true;
            NICaliLV.Items[Idx + 1].Selected = true;
            //TestLV.Items[Idx + 1].BackColor = 
        }

        private void SetParameterBtn_Click(object sender, EventArgs e)
        {
            int myStartPixel = int.Parse(StartPixelEB.Text);
            int myEndPixel = int.Parse(StopPixelEB.Text);
            int myIntegrateTime = int.Parse(IntegrateTimeEB.Text);
            int mySmoothing = int.Parse(SmoothPixEB.Text);
            int myNumOfAvg = int.Parse(NumOfAvgEB.Text);

            myMgr.SetParamters(ModLB.SelectedIndex,myStartPixel,myEndPixel,myIntegrateTime,myNumOfAvg,mySmoothing);
            if (myMgr.LoadFileName != "")
                myMgr.SaveSettings(myMgr.LoadFileName);
        }

        private void setIOCB_CheckedChanged(object sender, EventArgs e)
        {
            myMgr.SetIO(ModLB.SelectedIndex, setIOCB.Checked);
            usedTimeEB.Text = myMgr.SpecList[ModLB.SelectedIndex].UsedTime.ToString("h'h 'm'm 's's'");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<float> wl = new List<float>();
            wl = myMgr.GetWaveLength(ModLB.SelectedIndex);

            List<float> cnt = new List<float>();
            cnt = myMgr.GetCount(ModLB.SelectedIndex);

            InitChart((int)cnt.Max());
            if ((wl == null) || (cnt == null))
            {
                MessageBox.Show("Error Getting Spectrum.");
                return;
            }
            UpdateChart(wl, cnt);
        }

        private void UpdateChart(List<float> myWL, List<float> myCnt)
        {
            for (int i = 0; i < myWL.Count; i++)
            {
                SpecChart.Series["Series1"].Points.AddXY(myWL[i], myCnt[i]);               
            }
            SpecChart.Series["Series1"].ChartType = SeriesChartType.FastLine;
        }

        private void InitCaliLV()
        {
            NICaliLV.Clear();
            NICaliLV.Columns.Add("", 20);
            NICaliLV.Columns.Add("wavelength", 70);
            NICaliLV.Columns.Add("Min %", 50);
            NICaliLV.Columns.Add("Max %", 50);
            NICaliLV.Columns.Add("Nominal", 50);
            NICaliLV.Columns.Add("Measure", 60);
            NICaliLV.Columns.Add("Result", 60);
            this.NICaliLV.View = System.Windows.Forms.View.Details;
        }

        private void AddCriBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            myMgr.SaveTestCriteria(Para.CurLoadConfigFileName);
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (ModLB.SelectedIndex == -1)
                return;

            if (NICaliLV.SelectedIndices.Count == 0)
                return;

            int Idx = NICaliLV.SelectedIndices[0];

            myMgr.SpecList[ModLB.SelectedIndex].Criteria.RemoveAt(Idx);
            UpdateTestLV(ModLB.SelectedIndex);
        }

        private void TestLV_SelectedIndexChanged(object sender, EventArgs e)
        {           
            //EditBtn.Enabled = true;
            
        }        

        private void EditBtn_Click(object sender, EventArgs e)
        {
            
           
        }

        private void DelAllBtn_Click(object sender, EventArgs e)
        {
            if (ModLB.SelectedIndex == -1)
                return;
            myMgr.SpecList[ModLB.SelectedIndex].Criteria.Clear();
            UpdateTestLV(ModLB.SelectedIndex);
        }

        private void MoveUpBtn_Click(object sender, EventArgs e)
        {
            if (ModLB.SelectedIndex == -1)
                return;
            if (NICaliLV.SelectedIndices.Count == 0)
                return;

            int Idx = NICaliLV.SelectedIndices[0];
            if (Idx == 0)
                return;

            TestCriteria myCri = myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx];
            TestCriteria myCri2 = myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx - 1];

            myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx] = myCri2;
            myMgr.SpecList[ModLB.SelectedIndex].Criteria[Idx - 1] = myCri;

            UpdateTestLV(ModLB.SelectedIndex);

            //TestLV.SelectedIndices.Add(Idx + 1);
            NICaliLV.Items[Idx - 1].Focused = true;
            NICaliLV.Items[Idx - 1].Selected = true;
        }

        private void DarkRefBtn_Click(object sender, EventArgs e)
        {
            if (Para.isOutShutter)
            {
                myMgr.SetIO(ModLB.SelectedIndex, false);
                mtMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
            }
            else
            {
                mtMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
                myMgr.SetIO(ModLB.SelectedIndex, true);
            }

            Thread.Sleep(1000);
            List<float> wl = new List<float>();
            wl = myMgr.GetWaveLength(ModLB.SelectedIndex);

            //List<double> cnt = new List<double>();
            darkRef = myMgr.GetCount(ModLB.SelectedIndex);

            InitChart((int)darkRef.Max());
            if ((wl == null) || (darkRef == null))
            {
                MessageBox.Show("Error Getting Spectrum.");
                return;
            }
            UpdateChart(wl, darkRef);
        }

        public float CalTranData(double dData, double wData, double counts)
        {
            double up = counts - dData;
            double down = wData - dData;

            if (down <= 0 || up <= 0)
                return 0;
            return (float)(up / down) * 100;
        }
        public List<float> CalculateTransRatio(List<float> darkRef, List<float> whiteRef, List<float> curCnt)
        {
            List<float> myTransRatio = new List<float>();

            for (int i = 0; i < curCnt.Count; i++)
                myTransRatio.Add(CalTranData(darkRef[i], whiteRef[i], curCnt[i]));
            return myTransRatio;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Para.isOutShutter)
            {
                myMgr.SetIO(ModLB.SelectedIndex, false);
                mtMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
                Thread.Sleep(1000);
            }
            else
            {
                mtMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
                myMgr.SetIO(ModLB.SelectedIndex, false);
                Thread.Sleep(1000);
            }
            
            List<float> wl = new List<float>();
            wl = myMgr.GetWaveLength(ModLB.SelectedIndex);

            //List<double> cnt = new List<double>();
            whiteRef = myMgr.GetCount(ModLB.SelectedIndex);

            InitChart((int)whiteRef.Max());
            if ((wl == null) || (whiteRef == null))
            {
                MessageBox.Show("Error Getting Spectrum.");
                return;
            }
            UpdateChart(wl, whiteRef);

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Para.isOutShutter)
            {
                myMgr.SetIO(ModLB.SelectedIndex, false);
                mtMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
                Thread.Sleep(500);
            }
            else
            {
                mtMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
                myMgr.SetIO(ModLB.SelectedIndex, false);
                Thread.Sleep(500);
            }
            List<float> wl = new List<float>();
            wl = myMgr.GetWaveLength(ModLB.SelectedIndex);

            //List<double> cnt = new List<double>();
            measVal = myMgr.GetCount(ModLB.SelectedIndex);

            InitChart((int)measVal.Max());
            if ((wl == null) || (measVal == null))
            {
                MessageBox.Show("Error Getting Spectrum.");
                return;
            }
            UpdateChart(wl, measVal);
            Thread.Sleep(500);

            InitTransChart();
            //Calculate Normalized Value
            TransVal = CalculateTransRatio(darkRef, whiteRef, measVal);
            UpdateChart(wl, TransVal);

            SaveRefDarkTransData("Manual", 400, 1100, wl, darkRef, whiteRef, measVal, TransVal);
            SaveRawData("Manual", 400, 1100, wl, darkRef, whiteRef, measVal);
        }

        public void SaveRawData(string barCode, int stWaveLength, int endWaveLength, List<float> WL,
                                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData)
        {
            string s_FileName = barCode + "_ManualRawData_" + DateTime.Now.ToString("yyyyMMdd");

            string path = "D:\\ManualData";
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
                    columnTitle = "SerialNumber" + "," + "TesterID" + ",";

                    for (int wave = stWaveLength; wave <= endWaveLength; wave++)
                    {
                        columnTitle += "dark_" + Convert.ToString(wave) + ",";
                    }
                    for (int wave = stWaveLength; wave <= endWaveLength; wave++)
                    {
                        columnTitle += "white_" + Convert.ToString(wave) + ",";
                    }
                    for (int wave = stWaveLength; wave <= endWaveLength; wave++)
                    {
                        columnTitle += "Measured_" + Convert.ToString(wave) + ",";
                    }
                    sw.WriteLine(columnTitle);
                }

                columnValue = barCode;//barCode.Replace(",", "") + string.Format("{0:D2}", index + 1) + ",";//二维码+num
                columnValue += Para.MchName + ",";

                int s = 0;

                //dark_
                for (i = stWaveLength; i <= endWaveLength; i++)
                {
                    t = NearestIndex(s, i, WL);
                    s = t;
                    tempCounts = darkRef[t];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
                    columnValue += tempCounts.ToString("F4") + ",";

                }

                //"White_"
                s = 0;
                for (i = stWaveLength; i <= endWaveLength; i++)
                {
                    t = NearestIndex(s, i, WL);
                    s = t;
                    tempCounts = WhiteRef[t];//MeasData[t] - darkRef[t];

                    columnValue += tempCounts.ToString("F4") + ",";
                }

                //"T%_"
                s = 0;
                for (i = stWaveLength; i <= endWaveLength; i++)
                {
                    t = NearestIndex(s, i, WL);
                    s = t;
                    columnValue += MeasData[t].ToString("F4") + ",";
                }
                sw.WriteLine(columnValue);
                sw.Close();
                objFileStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                sw.Close();
                objFileStream.Close();
            }

        }

        public void SaveRefDarkTransData(string barCode, int stWaveLength, int endWaveLength, List<float> WL,
                                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData, List<float> TransData)
        {
            string s_FileName = barCode + "_ManualTransData_" + DateTime.Now.ToString("yyyyMMdd");

            string path = "D:\\ManualData";
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
                    columnTitle = "SerialNumber" + "," + "TesterID" + ",";

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

                columnValue = barCode;//barCode.Replace(",", "") + string.Format("{0:D2}", index + 1) + ",";//二维码+num
                columnValue += Para.MchName + ",";

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
                MessageBox.Show(e.ToString());
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


        private void button4_Click(object sender, EventArgs e)
        {
            specTimer.Enabled = !specTimer.Enabled;
            if (specTimer.Enabled)
                button4.Text = "Stop Continuous Spectrum";
            else
                button4.Text = "Start Continuous Spectrum";
        }

        public int scale = 2000;
        private void specTimer_Tick(object sender, EventArgs e)
        {
            List<float> wl = new List<float>();
            wl = myMgr.GetWaveLength(ModLB.SelectedIndex);

            List<float> cnt = new List<float>();
            cnt = myMgr.GetCount(ModLB.SelectedIndex);

            InitChart((int)cnt.Max());            
            UpdateChart(wl, cnt);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            scale = scale / 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            scale = scale * 2;
        }

        int CaliWLCnt = 15;

        List<NDFileterInfo> NDInfo = new List<NDFileterInfo>();

        private void LoadNIFilter()
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
                //MessageBox.Show("ND Calibration File Not Found." + fileName, "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string strread = "";

            NDInfo.Clear();
            StreamReader sr = new StreamReader(fileName);            
            string line;
            string[] row;
            int Rowcnt = 0;
            while ((line = sr.ReadLine()) != null)
            {
                row = line.Split(',');
                if (row[0] == "")
                    continue;

                if (Rowcnt == 0)
                {
                    NDFileterInfo tmp = new NDFileterInfo();
                    tmp.Name = row[1];
                    NDInfo.Add(tmp);
                    NDFileterInfo tmp2 = new NDFileterInfo();
                    tmp2.Name = row[4];
                    NDInfo.Add(tmp2);
                    NDFileterInfo tmp3 = new NDFileterInfo();
                    tmp3.Name = row[7];
                    NDInfo.Add(tmp3);
                    NDFileterInfo tmp4 = new NDFileterInfo();
                    tmp4.Name = row[10];
                    NDInfo.Add(tmp4);
                    NDFileterInfo tmp5 = new NDFileterInfo();
                    tmp5.Name = row[13];
                    NDInfo.Add(tmp5);
                    NDFileterInfo tmp6 = new NDFileterInfo();
                    tmp6.Name = row[16];
                    NDInfo.Add(tmp6);
                    Rowcnt++;
                    continue;
                }

                int Idx = 1;
                CalibrationInfo info1 = new CalibrationInfo();
                info1.waveLength = int.Parse(row[0]);
                info1.Nominal = double.Parse(row[Idx]);
                info1.min = double.Parse(row[Idx+1]);
                info1.max = double.Parse(row[Idx+2]);
                NDInfo[0].data.Add(info1);

                Idx = 4;
                CalibrationInfo info2 = new CalibrationInfo();
                info2.waveLength = int.Parse(row[0]);
                info2.Nominal = double.Parse(row[Idx]);
                info2.min = double.Parse(row[Idx + 1]);
                info2.max = double.Parse(row[Idx + 2]);
                NDInfo[1].data.Add(info2);

                Idx = 7;
                CalibrationInfo info3 = new CalibrationInfo();
                info3.waveLength = int.Parse(row[0]);
                info3.Nominal = double.Parse(row[Idx]);
                info3.min = double.Parse(row[Idx + 1]);
                info3.max = double.Parse(row[Idx + 2]);
                NDInfo[2].data.Add(info3);

                Idx = 10;
                CalibrationInfo info4 = new CalibrationInfo();
                info4.waveLength = int.Parse(row[0]);
                info4.Nominal = double.Parse(row[Idx]);
                info4.min = double.Parse(row[Idx + 1]);
                info4.max = double.Parse(row[Idx + 2]);
                NDInfo[3].data.Add(info4);

                Idx = 13;
                CalibrationInfo info5 = new CalibrationInfo();
                info5.waveLength = int.Parse(row[0]);
                info5.Nominal = double.Parse(row[Idx]);
                info5.min = double.Parse(row[Idx + 1]);
                info5.max = double.Parse(row[Idx + 2]);
                NDInfo[4].data.Add(info5);

                Idx = 16;
                CalibrationInfo info6 = new CalibrationInfo();
                info6.waveLength = int.Parse(row[0]);
                info6.Nominal = double.Parse(row[Idx]);
                info6.min = double.Parse(row[Idx + 1]);
                info6.max = double.Parse(row[Idx + 2]);
                NDInfo[5].data.Add(info6);
            }

           // UpdateCaliUI();
            UpdateNDFilterLB();
        }

        private void UpdateNDFilterLB()
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < NDInfo.Count; i++)
                comboBox1.Items.Add(NDInfo[i].Name);
        }

        private void UpdateCaliUI(int Idx)
        {
            InitCaliLV();            //CaliInfo
            for (int i = 0; i < NDInfo[Idx].data.Count; i++)
            {
                ListViewItem item = NICaliLV.Items.Add(NICaliLV.Items.Count + "");
                item.SubItems.Add(NDInfo[Idx].data[i].waveLength.ToString());
                item.SubItems.Add(NDInfo[Idx].data[i].min.ToString("F2"));
                item.SubItems.Add(NDInfo[Idx].data[i].max.ToString("F2"));
                item.SubItems.Add(NDInfo[Idx].data[i].Nominal.ToString("F2"));
                item.EnsureVisible();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCaliUI(comboBox1.SelectedIndex);
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No ND Filter Selected.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int Idx = comboBox1.SelectedIndex;

            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("No Module Selected.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int ModIdx = comboBox2.SelectedIndex;
            if (myMgr.GetType(ModIdx) == SpectType.NoSpectrometer)
            {
                MessageBox.Show("Module Spectromter Not Assigned.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Remove All Unit from Test Module", "Calibration Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            ModLB.SelectedIndex = ModIdx;

            DarkRefBtn_Click(sender, e);
            button1_Click(sender, e);

            if (MessageBox.Show("Load ND Filter To Test Module", "Calibration Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                return;

            List<float> wl = new List<float>();
            wl = myMgr.GetWaveLength(ModIdx);

            //List<double> cnt = new List<double>();
            measVal = myMgr.GetCount(ModIdx);

            InitChart((int)measVal.Max());
            if ((wl == null) || (measVal == null))
            {
                MessageBox.Show("Error Getting Spectrum.");
                return;
            }
            UpdateChart(wl, measVal);
            Thread.Sleep(500);

            InitTransChart();
            //Calculate Normalized Value
            TransVal = CalculateTransRatio(darkRef, whiteRef, measVal);
            UpdateChart(wl, TransVal);

            int sIdx = 0;
            int idx = 0;
            InitCaliLV();
            List<float> NDTransData = new List<float>();

            for (int i = 0; i < NDInfo[Idx].data.Count; i++)
            {
                ListViewItem item = NICaliLV.Items.Add(NICaliLV.Items.Count + "");
                item.SubItems.Add(NDInfo[Idx].data[i].waveLength.ToString());
                item.SubItems.Add(NDInfo[Idx].data[i].min.ToString("F2"));
                item.SubItems.Add(NDInfo[Idx].data[i].max.ToString("F2"));
                item.SubItems.Add(NDInfo[Idx].data[i].Nominal.ToString("F2"));

                idx = NearestIndex(sIdx, NDInfo[Idx].data[i].waveLength, wl);
                sIdx = idx;

                item.SubItems.Add(TransVal[idx].ToString("F2"));
                item.UseItemStyleForSubItems = false;
                NDTransData.Add(TransVal[idx]);

                double min = NDInfo[Idx].data[i].Nominal * (NDInfo[Idx].data[i].min/100);
                double max = NDInfo[Idx].data[i].Nominal * (NDInfo[Idx].data[i].max / 100);
                double diff = TransVal[idx] - NDInfo[Idx].data[i].Nominal;

                if ((diff < min) || (diff > max))
                {
                    item.SubItems.Add("Fail");
                    item.SubItems[6].BackColor = Color.Red; 
                }
                else
                {
                    item.SubItems.Add("Pass");
                    item.SubItems[6].BackColor = Color.Lime;                    
                }
                item.EnsureVisible();
            }
            SaveNDFilterData(NDInfo[Idx], NDTransData, ModIdx + 1, NDInfo[Idx]);
            MessageBox.Show("ND Filter Calibration Completed.", "Calibration Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SaveNDFilterData(NDFileterInfo myNDFilterData, List<float> TransData, int ModIdx, NDFileterInfo NdData)
        {
            //lock (sny_Obj)
            {
                string s_FileName = "_NDFilterData_" + DateTime.Now.ToString("yyyyMMdd");

                string path = "D:\\DailyNDFilterData";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = "D:\\DailyNDFilterData\\Module" + (ModIdx).ToString();
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
                        columnTitle = "SerialNumber" + "," + "DateTime" + ",";

                        for (int wave = 0; wave < myNDFilterData.data.Count; wave++)
                        {
                            columnTitle += myNDFilterData.data[wave].waveLength.ToString("F1") + ",";
                        }                        
                        sw.WriteLine(columnTitle);
                    }

                    //Nominal
                    columnValue = myNDFilterData.Name + "_Nominal,";
                    columnValue += DateTime.Now.ToString("dd MM HH:mm:ss") + ",";
                    for (i = 0; i < NdData.data.Count; i++)
                    {
                        columnValue += NdData.data[i].Nominal.ToString("F3") + ",";
                    }
                    sw.WriteLine(columnValue);

                    //Min
                    columnValue = myNDFilterData.Name + "_Min,";
                    columnValue += DateTime.Now.ToString("dd MM HH:mm:ss") + ",";
                    for (i = 0; i < NdData.data.Count; i++)
                    {
                        columnValue += NdData.data[i].min.ToString("F3") + ",";
                    }
                    sw.WriteLine(columnValue);

                    //Max
                    columnValue = myNDFilterData.Name + "_Max,";
                    columnValue += DateTime.Now.ToString("dd MM HH:mm:ss") + ",";
                    for (i = 0; i < NdData.data.Count; i++)
                    {
                        columnValue += NdData.data[i].max.ToString("F3") + ",";
                    }
                    sw.WriteLine(columnValue);

                    columnValue = myNDFilterData.Name + "_T%,";
                    columnValue += DateTime.Now.ToString("dd MM HH:mm:ss") +",";                    
                    for (i = 0; i < TransData.Count; i++)
                    {
                        columnValue += TransData[i].ToString("F3") + ",";
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
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == 0)
            {
                NiCaliGB.Visible = true;
                GSCaliBtn.Visible = false;
            }
            else
            {
                NiCaliGB.Visible = false;
                GSCaliBtn.Visible = true;
            }
        }

        private void WinSpectrometer_FormClosing(object sender, FormClosingEventArgs e)
        {
            button4.Text = "Start Continuous Spectrum";
            specTimer.Enabled = false;
        }

        private void GSCaliBtn_Click(object sender, EventArgs e)
        {
            CaliForm myWin = new CaliForm( myMgr, mtMgr);
            myWin.ShowDialog();
        }
    }
}
