using Common;
using HalconDotNet;
using JPTCG.AppleTestSW;
using JPTCG.BarcodeScanner;
using JPTCG.Calibration;
using JPTCG.Common;
using JPTCG.Motion;
using JPTCG.Spectrometer;
using JPTCG.Vision;
using Keyences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ViewROI;

namespace JPTCG
{
    public partial class mainWin : Form
    {
        public DeltaMotionMgr motionMgr;
        public HalconVision camera1;
        public HalconVision camera2;
        public IOInterface myIO;
        public JogPad JP1 , JP2, JP3;
        public SpectManager specMgr;
        public MachineSeq SeqMgr;
        public BarcodeMgr BarCMgr;
        public AppleTestSWCom AMgrMod1, AMgrMod2;
        public RotaryMotion RotMgr;
        public UserLogin uLogin = new UserLogin();

        //Alvin 15112016
        public KeyenceDLRS1A DLRS1, DLRS2;

        string AppTitle = "JPT CG Manual Tester";
        string mchSetFileName = "JPT_CG_Settings.xml";
        string LSSetFileName = "LS_Setting.xml";
        string defaultSettingsName = "JPT_CG_default.xml";
        string settingsFullFilePath = "";
        string configFolderPath = "";
        string mchSettingsFilePath = "";
        string LSSettingFilePath = "";

        int Mod1SpecIdx = -1;
        int Mod2SpecIdx = -1;

        private HWndCtrl hWndCtrl1;
        private HWndCtrl hWndCtrl2;
        private HWndCtrl hWndCtrlRes1;
        private HWndCtrl hWndCtrlRes2;
        private Object m_lockShowpicture1;
        private Object m_lockShowpicture2;
        SplashScreen ss = new SplashScreen();
        public mainWin()
        {
            InitializeComponent();
            ss.Show();
            Application.DoEvents();
            this.mainTC.Region = new Region(new RectangleF(this.HomeTP.Left, this.HomeTP.Top, this.HomeTP.Width, this.HomeTP.Height));//Hide Tabcontrol

            motionMgr = DeltaMotionMgr.CreateMotion();
            camera1 = new HalconVision("Module 1", Cam1Pnl, HalconWin1);
            camera2 = new HalconVision("Module 2", Cam2Pnl, HalconWin2);
            myIO = new IOInterface(motionMgr, IOPnl);
            JP1 = new JogPad(motionMgr,Jp1Pnl);
            JP2 = new JogPad(motionMgr,Jp2Pnl);
            JP3 = new JogPad(motionMgr, Jp3Pnl);
            JP1.OnApplyClick += OnJP1ApplyClick;
            JP2.OnApplyClick += OnJP2ApplyClick;
            JP3.OnApplyClick += OnJP3ApplyClick;
            specMgr = new SpectManager(this.Handle);
            Mod1SpecIdx = specMgr.AddSpectrometer("Module 1 Spectrometer");
            Mod2SpecIdx = specMgr.AddSpectrometer("Module 2 Spectrometer");
            BarCMgr = new BarcodeMgr();
            BarCMgr.AddBarcode("Barcode1");
            BarCMgr.AddBarcode("Barcode2");
            RotMgr = new RotaryMotion(motionMgr);
           
            AMgrMod1 = new AppleTestSWCom("Module1");
            AMgrMod2 = new AppleTestSWCom("Module2");

            //Alvin 15112016
            DLRS1 = new KeyenceDLRS1A("Module1");
            DLRS2 = new KeyenceDLRS1A("Module2");

            SeqMgr = new MachineSeq(this, motionMgr, RotMgr, specMgr, BarCMgr, camera1, camera2, AMgrMod1, AMgrMod2);

            Para.myMain = this;
        }
        private void VisionUIInit()
        {
            hWndCtrl1 = new HWndCtrl(HalconWin1);
            hWndCtrl1.setViewState(HWndCtrl.MODE_VIEW_NONE);
            hWndCtrl1.repaint();

            hWndCtrlRes1 = new HWndCtrl(hWinCntrlResCam1);
            hWndCtrlRes1.setViewState(HWndCtrl.MODE_VIEW_NONE);
            hWndCtrlRes1.repaint();
            

            camera1.OnImageReadyFunction += OnImgReadyCam1;

            CaliXLbl.Text = camera1.CaliValue.X.ToString("F7");
            CaliYLbl.Text = camera1.CaliValue.Y.ToString("F7");         
            m_lockShowpicture1 = new object();

            hWndCtrl2 = new HWndCtrl(HalconWin2);
            hWndCtrl2.setViewState(HWndCtrl.MODE_VIEW_NONE);
            hWndCtrl2.repaint();
            camera2.OnImageReadyFunction += OnImgReadyCam2;

            hWndCtrlRes2 = new HWndCtrl(hWinCntrlResCam2);
            hWndCtrlRes2.setViewState(HWndCtrl.MODE_VIEW_NONE);
            hWndCtrlRes2.repaint();

            CaliXLbl2.Text = camera2.CaliValue.X.ToString("F7");
            CaliYLbl2.Text = camera2.CaliValue.Y.ToString("F7");
            m_lockShowpicture2 = new object();

            camIDLbl.Text = camera1.cameraID;
            camIDLbl2.Text = camera2.cameraID;

            string strHeadImagePath;
            HImage image, image2;
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string ImgFolderPath = exePath + "Resources\\";

            strHeadImagePath = ImgFolderPath+"DefaultImg.bmp";
            image = new HImage(strHeadImagePath);
            camera1.myImage = image;
            hWndCtrl1.ClearResult();
            hWndCtrl1.addIconicVar(image);
            image2 = new HImage(strHeadImagePath);
            camera2.myImage = image2;
            hWndCtrl2.ClearResult();
            hWndCtrl2.addIconicVar(image2);                        

        }
        private void OnImgReadyCam1(HObject myImage)
        {            
                Action ac = new Action(() =>
                {
                    hWndCtrl1.addIconicVar(myImage);
                    //hWndCtrl1.repaint();
                    camera1.bUIRefreshDone = true;
                    Application.DoEvents();
                });

                BeginInvoke(ac);
          

        }
        private void OnImgReadyCam2(HObject myImage)
        {
            lock (m_lockShowpicture2)
            {
                Action ac = new Action(() =>
                {
                    hWndCtrl2.addIconicVar(myImage);
                    //hWndCtrl2.repaint();
                    camera2.bUIRefreshDone = true;
                });

                BeginInvoke(ac);

            }
        }
        private void mainWin_Load(object sender, EventArgs e)
        {
            Para.SWRunTime = DateTime.Now;
            VersionLbl.Text = Para.SWVersion;
            CommentLbl.Text = Para.SWComment;
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            mchSettingsFilePath = exePath + mchSetFileName;
            configFolderPath = exePath + "Config\\";
            LSSettingFilePath = exePath + LSSetFileName;

            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }

            settingsFullFilePath = configFolderPath + defaultSettingsName;
            this.Text = AppTitle + " < " + settingsFullFilePath + " > ";
            Para.CurLoadConfigFileName = settingsFullFilePath;
            Para.MchConfigFileName = mchSettingsFilePath;
            
            LoadMchSettings();
            specMgr.LoadMachineSettings(mchSettingsFilePath);
            camera1.LoadSettings(mchSettingsFilePath);
            camera2.LoadSettings(mchSettingsFilePath);
            BarCMgr.LoadSettings(mchSettingsFilePath);
            //Alvin 15112016
            DLRS1.LoadSettings(mchSettingsFilePath);
            DLRS2.LoadSettings(mchSettingsFilePath);

            LoadSettings(Para.CurLoadConfigFileName);            
            UpdateUI();
            UpdateConfigUI();

            VisionUIInit();
            InitCharts();

            debugBtn.Visible = Para.DebugMode;
            CBDryRun.Visible = Para.DebugMode;

            //if (!AMgrMod1.Connect(AMgrMod1.IP, AMgrMod1.Port))
            //{
            //    MessageBox.Show("CGClient Module 1 Fail To Connect To CCGHost");
            //}

            //if (!AMgrMod2.Connect(AMgrMod2.IP, AMgrMod2.Port))
            //{
            //    MessageBox.Show("CGClient Module 2 Fail To Connect To CCGHost");
            //}
            ss.Close();
            userCB.Items.Clear();
            for (int i = 0; i < uLogin.user.Count(); i++)
                userCB.Items.Add(uLogin.user[i]);

            userCB.SelectedIndex = 0;
            SetUIAccess(0);

            if (!motionMgr.ReadIOIn((ushort)InputIOlist.BtnEMO))
            {
                MessageBox.Show("Stop Button Pressed!", "EMO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Para.MachineOnline)
                safetyTimer.Enabled = true;
            motionMgr.SetServoOn((ushort)Axislist.Mod1XAxis, 1);
            motionMgr.SetServoOn((ushort)Axislist.Mod2XAxis, 1);
            motionMgr.SetServoOn((ushort)Axislist.Mod1YAxis, 1);
            motionMgr.SetServoOn((ushort)Axislist.Mod2YAxis, 1);
            SeqMgr.StartHoming();
            motionMgr.InitRM32NT();
            UpdateStatusLbl(Color.Orange);
            if (Para.EnableZaxis)
            {
                MessageBox.Show("Z Axis has opened");
            }

            TimeSpan time_span11 = DateTime.Now - Para.SystemRunTime;
            if (time_span11.TotalHours >= 24)
            {
                //FileOperation.SaveData(Para.MchConfigFileName, "ContinueRunTime", "Time", DateTime.Now.ToString());
                MessageBox.Show("Run over 24h,Please,Dailycheck");
            }

            time_span11 = DateTime.Now - Para.NDSystemRunTime;
            if (time_span11.TotalHours >= 24*8)
            {
                //FileOperation.SaveData(Para.MchConfigFileName, "ContinueRunTime", "Time", DateTime.Now.ToString());
                MessageBox.Show(" Run over 8 days,Please,ND");
            }

            time_span11 = DateTime.Now - Para.LSSystemRunTime;
            if (time_span11.TotalHours >= 24 * 8)
            {
                //FileOperation.SaveData(Para.MchConfigFileName, "ContinueRunTime", "Time", DateTime.Now.ToString());
                MessageBox.Show(" Run over 8 days,Please,LS");
            }

        }
        public void SetUIAccess(int accesslvl)
        {
            configCB.Enabled = true;
            checkBox2.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;
            CBEngMode.Enabled = true;
            mchNameEB.Enabled = true;
            CreateNewBtn.Enabled = true;
            SaveBtn.Enabled = true;
            deltaPnl.Enabled = true;
            RotaryGB.Enabled = true;
            label20.Enabled = true;
            label15.Enabled = true;
            label16.Enabled = true;
            BarcodeMenuLbl.Enabled = true;
            Cam1Pnl.Enabled = true;
            Cam2Pnl.Enabled = true;
            CBEngMode.Enabled = true;
            label15.Enabled = true;
            label42.Enabled = true;       


            switch (accesslvl)
            {
                case 0: //Operator
                    //button17.Enabled = false;
                    //
                    checkBox6.Visible = false;
                    checkBox7.Visible = false;
                    checkBox2.Visible = false;
                    checkBox4.Visible = false;
                    CBEngMode.Visible = false;
                    checkBox5.Visible = false;

                    //
                    configCB.Enabled = false;
                    checkBox7.Enabled = false;
                    checkBox6.Enabled = false;
                    //buttonAuditBox.Enabled = false;
                    checkBox2.Enabled = false;
                    checkBox4.Enabled = false;
                    checkBox5.Enabled = false;
                    CBEngMode.Enabled = false;
                    mchNameEB.Enabled = false;
                    CreateNewBtn.Enabled = false;
                    SaveBtn.Enabled = false;
                    deltaPnl.Enabled = false;
                    RotaryGB.Enabled = false;
                    label20.Enabled = false;
                    label15.Enabled = false;
                    label16.Enabled = false;
                    BarcodeMenuLbl.Enabled = false;
                    Cam1Pnl.Enabled = false;
                    Cam2Pnl.Enabled = false;
                    label42.Enabled = false;
                    label21.Text = uLogin.user[0];
                    break;
                case 1: // Engineer
                    CBEngMode.Enabled = false;
                    label15.Enabled = false;
                    label21.Text = uLogin.user[1];
                    break;
                case 2: //full Access
                    //
                    checkBox6.Visible = true;
                    checkBox7.Visible = true;
                    checkBox2.Visible = true;
                    checkBox4.Visible = true;
                    CBEngMode.Visible = true;
                    checkBox5.Visible = true;
                    //
                    button17.Enabled = true;
                    checkBox7.Enabled = true;
                    checkBox6.Enabled = true;
                    //buttonAuditBox.Enabled = true;
                    label21.Text = uLogin.user[2];
                    break;

            }
        }

        private void LoadMchSettings()
        {           
            string strread = "";

            FileOperation.ReadData(mchSettingsFilePath, "Machine", "Name", ref strread);
            if (strread != "0")
                Para.MchName = strread;

            //FileOperation.ReadData(mchSettingsFilePath, "Machine", "LastLoadFile", ref strread);
            //if (strread != "0")
            //    Para.CurLoadConfigFileName = strread;

            //if (!File.Exists(Para.CurLoadConfigFileName))
            //{
            //    MessageBox.Show(Para.CurLoadConfigFileName+"Settings File Not Found. Default Setting is Loaded.", AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    Para.CurLoadConfigFileName = settingsFullFilePath;
            //}

            FileOperation.ReadData(mchSettingsFilePath, "Machine", "ModCount", ref strread);
            if (strread != "0")
                Para.ModCount = int.Parse(strread);

            String IP = "";
            int Port = 61804;

            FileOperation.ReadData(mchSettingsFilePath, "CGHost", "Mod1IP", ref strread);
            if (strread != "0")
                IP = strread;

            FileOperation.ReadData(mchSettingsFilePath, "CGHost", "Mod1Port", ref strread);
            if (strread != "0")
                Port = int.Parse(strread);

            AMgrMod1.IP = IP;
            AMgrMod1.Port = Port;

            FileOperation.ReadData(mchSettingsFilePath, "CGHost", "Mod2IP", ref strread);
            if (strread != "0")
                IP = strread;

            FileOperation.ReadData(mchSettingsFilePath, "CGHost", "Mod2Port", ref strread);
            if (strread != "0")
                Port = int.Parse(strread);
            AMgrMod2.IP = IP;
            AMgrMod2.Port = Port;


            FileOperation.ReadData(mchSettingsFilePath, "UnitCount", "TotalCount", ref strread);
            if (strread != "0")
                Para.TotalTestUnit = int.Parse(strread);
            
            FileOperation.ReadData(mchSettingsFilePath, "UnitCount", "Mod1FailCnt", ref strread);
            if (strread != "0")
                Para.Mod1FailUnitCnt = int.Parse(strread);

            FileOperation.ReadData(mchSettingsFilePath, "UnitCount", "Mod1FailCnt", ref strread);
            if (strread != "0")
                Para.Mod2FailUnitCnt = int.Parse(strread);

            UpdateTotalCountUI();

            FileOperation.ReadData(mchSettingsFilePath, "Machine", "EnbMod4Unloading",ref strread);
            if (strread != "0")
                Para.EnbStation4Unloading = bool.Parse(strread);

            FileOperation.ReadData(mchSettingsFilePath, "Machine", "EnableZaxis", ref strread);
            if (strread != "0")
                Para.EnableZaxis = bool.Parse(strread);

            FileOperation.ReadData(mchSettingsFilePath, "Machine", "isOutShutter", ref strread);
            if (strread != "0")
                Para.isOutShutter = bool.Parse(strread);
            //20170211
            //FileOperation.ReadData(mchSettingsFilePath, "LightSource", "LSType", ref strread);
            //Para.LightSourceType = strread;
            FileOperation.ReadData(mchSettingsFilePath, "LightSource", "LSType", ref strread);
            Para.LightSourceType = strread;
            if (strread != "0")
                Para.LightSourceType = strread;
            else
            {
                Para.LightSourceType = "EQ";
                FileOperation.SaveData(Para.MchConfigFileName, "LightSource", "LSType", Para.LightSourceType);
            }


            FileOperation.ReadData(mchSettingsFilePath, "ContinueRunTime", "Time", ref strread);
            if (strread != "0")
                Para.SystemRunTime = DateTime.Parse(strread);
            else
            {
                Para.SystemRunTime = DateTime.Now;
                FileOperation.SaveData(Para.MchConfigFileName, "ContinueRunTime", "Time", DateTime.Now.ToString());
            }

            FileOperation.ReadData(mchSettingsFilePath, "NDContinueRunTime", "Time", ref strread);
            if (strread != "0")
                Para.NDSystemRunTime = DateTime.Parse(strread);
            else
            {
                Para.NDSystemRunTime = DateTime.Now;
                FileOperation.SaveData(Para.MchConfigFileName, "NDContinueRunTime", "Time", DateTime.Now.ToString());
            }

            FileOperation.ReadData(mchSettingsFilePath, "LSContinueRunTime", "Time", ref strread);
            if (strread != "0")
                Para.LSSystemRunTime = DateTime.Parse(strread);
            else
            {
                Para.LSSystemRunTime = DateTime.Now;
                FileOperation.SaveData(Para.MchConfigFileName, "LSContinueRunTime", "Time", DateTime.Now.ToString());
            }

            FileOperation.ReadData(mchSettingsFilePath, "LSUsedTime", "Time", ref strread);
            if (strread != "0")
                Para.LSUsedTime = double.Parse(strread);
            else
            {
                FileOperation.SaveData(Para.MchConfigFileName, "LSUsedTime", "Time", Para.LSUsedTime.ToString());
            }

            FileOperation.ReadData(mchSettingsFilePath, "NDTime", "Time", ref strread);
            if (strread != "0")
                Para.NDTime = int.Parse(strread);
            else
            {
                Para.NDTime = 8;
                FileOperation.SaveData(Para.MchConfigFileName, "NDTime", "Time", Convert.ToString(8));
            }

            FileOperation.ReadData(mchSettingsFilePath, "Spectrometer1", "Serial", ref strread);
            if (strread.Length > 31)
                Para.Spectrometer1SN = "Serial1toLong";
            else
                Para.Spectrometer1SN = strread;

            FileOperation.ReadData(mchSettingsFilePath, "Spectrometer2", "Serial", ref strread);
            if (strread.Length > 31)
                Para.Spectrometer2SN = "Serial2toLong";
            else
                Para.Spectrometer2SN = strread;
                //20170211

                //FileOperation.ReadData(mchSettingsFilePath, "P2Test", "Enabled", ref strread);
                //if (strread != "0")
                //    Para.EnableP2Test = bool.Parse(strread);


                Cam1Exp1GB.Enabled = Para.EnableP2Test;
                Cam1Exp3GB.Enabled = Para.EnableP2Test;
                Cam2Exp1GB.Enabled = Para.EnableP2Test;
                Cam2Exp3GB.Enabled = Para.EnableP2Test;
            

        }
        private void SaveMchSettings()
        {            
            FileOperation.SaveData(mchSettingsFilePath, "Machine", "Name", Para.MchName);
            FileOperation.SaveData(mchSettingsFilePath, "Machine", "LastLoadFile", Para.CurLoadConfigFileName);
            FileOperation.SaveData(mchSettingsFilePath, "Machine", "ModCount", Para.ModCount.ToString());

            FileOperation.SaveData(mchSettingsFilePath, "CGHost", "Mod1IP", AMgrMod1.IP);
            FileOperation.SaveData(mchSettingsFilePath, "CGHost", "Mod1Port", AMgrMod1.Port.ToString());
            FileOperation.SaveData(mchSettingsFilePath, "CGHost", "Mod2IP", AMgrMod2.IP);
            FileOperation.SaveData(mchSettingsFilePath, "CGHost", "Mod2Port", AMgrMod2.Port.ToString());

            FileOperation.SaveData(mchSettingsFilePath, "UnitCount", "TotalCount", Para.TotalTestUnit.ToString());
            FileOperation.SaveData(mchSettingsFilePath, "UnitCount", "Mod1FailCnt", Para.Mod1FailUnitCnt.ToString());
            FileOperation.SaveData(mchSettingsFilePath, "UnitCount", "Mod1FailCnt", Para.Mod2FailUnitCnt.ToString());

            FileOperation.SaveData(mchSettingsFilePath, "Machine", "EnbMod4Unloading", Para.EnbStation4Unloading.ToString());
            FileOperation.SaveData(mchSettingsFilePath, "Machine", "EnableZaxis", Para.EnableZaxis.ToString());
            FileOperation.SaveData(mchSettingsFilePath, "Machine", "isOutShutter", Para.isOutShutter.ToString());
            FileOperation.SaveData(mchSettingsFilePath, "LightSource", "LSType", Para.LightSourceType);//20170211
            
            //FileOperation.SaveData(mchSettingsFilePath, "P2Test", "Enabled", Para.EnableP2Test.ToString());

        }

        private void CreateDefaultModuleParameters()
        {
            for (int m = 0; m < Para.ModCount; m++)
            {
                Para.Module.Add(new ModuleSettings());
                Para.Module[m].TeachPos.Clear();
                for (int i = 0; i < Para.TeachPosCount; i++)
                    Para.Module[m].TeachPos.Add(new PosPoint());
                Para.Module[m].TestPt.Clear();
                for (int i = 0; i < Para._Max_Test_Point; i++)
                {
                    Para.Module[m].TestPt.Add(new DPoint());
                    Para.Module[m].TestPtEnb.Add(true);
                }
                //Para.Module[m].TestPt[0].X = -3.0;
                //Para.Module[m].TestPt[0].Y = 0;
                //Para.Module[m].TestPt[1].X = -1.5;
                //Para.Module[m].TestPt[1].Y = 0;
                //Para.Module[m].TestPt[2].X = 0.0;
                //Para.Module[m].TestPt[2].Y = 0;
                //Para.Module[m].TestPt[3].X = 1.5;
                //Para.Module[m].TestPt[3].Y = 0;
                //Para.Module[m].TestPt[4].X = 3.0;
                //Para.Module[m].TestPt[4].Y = 0;
                Para.Module[m].TestPt[0].X = -1.91;
                Para.Module[m].TestPt[0].Y = 0;
                Para.Module[m].TestPt[1].X = -0.41;
                Para.Module[m].TestPt[1].Y = 0;
                Para.Module[m].TestPt[2].X = 1.09;
                Para.Module[m].TestPt[2].Y = 0;

                Para.Module[m].TestPt[3].X = 2.59;
                Para.Module[m].TestPt[3].Y = 0;
                Para.Module[m].TestPt[4].X = 3.60;
                Para.Module[m].TestPt[4].Y = 0;
            }
        }
        private void LoadSettings(String fileName)
        {
            
            if (!File.Exists(fileName))
            {
                CreateDefaultModuleParameters();
                SaveSettings(fileName);
                return;
            }

            string strread = "";

            Para.Module.Clear();

            //FileOperation.ReadData(fileName, "TestPoint", "Count", ref strread);
            //if (strread != "0")
            //    Para.TestPtCnt = int.Parse(strread);

            FileOperation.ReadData(fileName, "SampleShape", "Shape", ref strread);
            Para.SampleShape = int.Parse(strread);
            SampleShapeCB.SelectedIndex = Para.SampleShape;
            if (Para.SampleShape == 0) 
            {
                hWinCntrlResCam1.Width = 150;
                hWinCntrlResCam1.Height = 30;
                hWinCntrlResCam2.Width = 150;
                hWinCntrlResCam2.Height = 30;
            }else
            {
                hWinCntrlResCam1.Width = 150;
                hWinCntrlResCam1.Height = 150;
                hWinCntrlResCam2.Width = 150;
                hWinCntrlResCam2.Height = 150;
            }
            //20161018
            string strread1 = "";
            //Camera1
            FileOperation.ReadData(fileName, "ExposureTime", "disableAutoExpTime1", ref strread1);
            if (strread1 == "True")
            {
                Cam1ckB.Checked = true;
            }
            else
            {
                Cam1ckB.Checked = false;
            }
                                      
            FileOperation.ReadData(fileName, "ExposureTime", "Camera1B", ref strread);
            if (strread != "0")
                Para.Cam1ExposureTimeB = int.Parse(strread);
            
            FileOperation.ReadData(fileName, "ExposureTime", "Camera1W", ref strread);
            if (strread != "0")
               Para.Cam1ExposureTimeW = int.Parse(strread);

           //Clovis
            FileOperation.ReadData(fileName, "ExposureTime", "Selected1BorW", ref strread1);

            if (strread1 == "0")
            {
                Cam1cbBox.SelectedIndex = 0;
                Cam1ExpTimeEB.Text = Para.Cam1ExposureTimeB.ToString();
                camera1.SetExposure(Para.Cam1ExposureTimeB);
            }
            else
            {
                Cam1cbBox.SelectedIndex = 1;
                Cam1ExpTimeEB.Text = Para.Cam1ExposureTimeW.ToString();
                camera1.SetExposure(Para.Cam1ExposureTimeW);
            }


            //Clovis
            FileOperation.ReadData(fileName, "ExposureTime", "Selected2BorW", ref strread1);

            if (strread1 == "0")
            {
                Cam2cbBox.SelectedIndex = 0;
                Cam2ExpTimeEB.Text = Para.Cam2ExposureTimeB.ToString();
                camera2.SetExposure(Para.Cam2ExposureTimeB);
            }
            else
            {
                Cam2cbBox.SelectedIndex = 1;
                Cam2ExpTimeEB.Text = Para.Cam2ExposureTimeW.ToString();
                camera2.SetExposure(Para.Cam2ExposureTimeW);
            }



            FileOperation.ReadData(fileName, "ExposureTime", "Camera1Exp1", ref strread);
            if (strread != "0")
                Para.Cam1ExposureTime1 = int.Parse(strread);
            Cam1ExpTime1EB.Text = Para.Cam1ExposureTime1.ToString();

            FileOperation.ReadData(fileName, "ExposureTime", "Camera1Exp3", ref strread);
            if (strread != "0")
                Para.Cam1ExposureTime3 = int.Parse(strread);
            Cam1ExpTime3EB.Text = Para.Cam1ExposureTime3.ToString();

            //Camera2
            FileOperation.ReadData(fileName, "ExposureTime", "disableAutoExpTime2", ref strread1);
            if (strread1 == "True")
            {
                Cam2ckB.Checked = true;
            }
            else
            {
                Cam2ckB.Checked = false;
            }
            
                            
            FileOperation.ReadData(fileName, "ExposureTime", "Camera2B", ref strread);
            if (strread != "0")
                    Para.Cam2ExposureTimeB = int.Parse(strread);
            
            FileOperation.ReadData(fileName, "ExposureTime", "Camera2W", ref strread);           
            if (strread != "0")
                Para.Cam2ExposureTimeW = int.Parse(strread);

            FileOperation.ReadData(fileName, "ExposureTime", "selected2BorW", ref strread1);
            if (strread1 == "0")
            {
                Cam2cbBox.SelectedIndex = 0;
                Cam2ExpTimeEB.Text = Para.Cam2ExposureTimeB.ToString();
                camera2.SetExposure(Para.Cam2ExposureTimeB);
            }else
            {
                Cam2cbBox.SelectedIndex = 1;
                Cam2ExpTimeEB.Text = Para.Cam2ExposureTimeW.ToString();
                camera2.SetExposure(Para.Cam2ExposureTimeW);
            }

            FileOperation.ReadData(fileName, "ExposureTime", "Camera2Exp1", ref strread);
            if (strread != "0")
                Para.Cam2ExposureTime1 = int.Parse(strread);
            Cam2ExpTime1EB.Text = Para.Cam2ExposureTime1.ToString();

            FileOperation.ReadData(fileName, "ExposureTime", "Camera2Exp3", ref strread);
            if (strread != "0")
                Para.Cam2ExposureTime3 = int.Parse(strread);
            Cam2ExpTime3EB.Text = Para.Cam2ExposureTime3.ToString();
            //20161129
            FileOperation.ReadData(fileName, "PicturePath", "PicPath", ref strread);
            PicPath.Text = strread;
            Para.PicPath = strread;

            //20161220
            FileOperation.ReadData(fileName, "Add3ParameterForImage", "CentroidWL1", ref strread);
            CenWL1textBox.Text = strread;
            Para.CenWL1 = float.Parse(strread);
            FileOperation.ReadData(fileName, "Add3ParameterForImage", "PixelDensity1", ref strread);
            PixDen1textBox.Text = strread;
            Para.PixDen1 = float.Parse(strread);
            FileOperation.ReadData(fileName, "Add3ParameterForImage", "BeamSize1", ref strread);
            BeamSize1textBox.Text = strread;
            Para.BeamSize1 = float.Parse(strread);

            FileOperation.ReadData(fileName, "Add3ParameterForImage", "CentroidWL2", ref strread);
            CenWL2textBox.Text = strread;
            Para.CenWL2 = float.Parse(strread);
            FileOperation.ReadData(fileName, "Add3ParameterForImage", "PixelDensity2", ref strread);
            PixDen2textBox.Text = strread;
            Para.PixDen2 = float.Parse(strread);
            FileOperation.ReadData(fileName, "Add3ParameterForImage", "BeamSize2", ref strread);
            BeamSize2textBox.Text = strread;
            Para.BeamSize2 = float.Parse(strread);
            //20161220
            //TestPtCB.SelectedIndex = TestPtCB.Items.IndexOf(Para.TestPtCnt.ToString());

            for (int m = 0; m < Para.ModCount; m++)
            {
                Para.Module.Add(new ModuleSettings());
                Para.Module[m].TeachPos.Clear();
                for (int i = 0; i < Para.TeachPosCount; i++)
                {
                    PosPoint myPos = new PosPoint();
                    FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "TeachPosX" + (i + 1).ToString(), ref strread);
                    if (strread != "0")
                        myPos.X = double.Parse(strread);
                    FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "TeachPosY" + (i + 1).ToString(), ref strread);
                    if (strread != "0")
                        myPos.Y = double.Parse(strread);
                    FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "TeachPosZ" + (i + 1).ToString(), ref strread);
                    if (strread != "0")
                        myPos.Z = double.Parse(strread);
                    Para.Module[m].TeachPos.Add(myPos);
                }

                Para.Module[m].TestPt.Clear();

                for (int i = 0; i < Para.TestPtCnt; i++)
                {
                    DPoint myPt = new DPoint();
                    FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "TestPtX" + (i + 1).ToString(), ref strread);
                    if (strread != "0")
                        myPt.X = double.Parse(strread);
                    FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "TestPtY" + (i + 1).ToString(), ref strread);
                    if (strread != "0")
                        myPt.Y = double.Parse(strread);
                    Para.Module[m].TestPt.Add(myPt);

                    FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "EnbTestPt" + (i + 1).ToString(), ref strread);
                    if (strread != "0")
                        Para.Module[m].TestPtEnb.Add(bool.Parse(strread));
                    else
                        Para.Module[m].TestPtEnb.Add(true);
                }

                //Test Position
                
                    if (Para.SampleShape == 0)
                    {
                        //Para.Module[m].TestPt[0].X = -3.0;
                        //Para.Module[m].TestPt[0].Y = 0;
                        //Para.Module[m].TestPt[1].X = -1.5;
                        //Para.Module[m].TestPt[1].Y = 0;
                        //Para.Module[m].TestPt[2].X = 0.0;
                        //Para.Module[m].TestPt[2].Y = 0;

                        //Para.Module[m].TestPt[3].X = 1.5;
                        //Para.Module[m].TestPt[3].Y = 0;
                        //Para.Module[m].TestPt[4].X = 3.0;
                        //Para.Module[m].TestPt[4].Y = 0;
                        Para.Module[m].TestPt[0].X = -1.91;
                        Para.Module[m].TestPt[0].Y = 0;
                        Para.Module[m].TestPt[1].X = -0.41;
                        Para.Module[m].TestPt[1].Y = 0;
                        Para.Module[m].TestPt[2].X = 1.09;
                        Para.Module[m].TestPt[2].Y = 0;

                        Para.Module[m].TestPt[3].X = 2.59;
                        Para.Module[m].TestPt[3].Y = 0;
                        Para.Module[m].TestPt[4].X = 3.60;
                        Para.Module[m].TestPt[4].Y = 0;
                    }
                    else
                    {
                        Para.Module[m].TestPt[0].X = 0;
                        Para.Module[m].TestPt[0].Y = 0;
                        Para.Module[m].TestPt[1].X = 0;
                        Para.Module[m].TestPt[1].Y = -0.444;
                        Para.Module[m].TestPt[2].X = 0.384;
                        Para.Module[m].TestPt[2].Y = 0.222;

                        Para.Module[m].TestPt[3].X = -0.384;
                        Para.Module[m].TestPt[3].Y = 0.222;
                        Para.Module[m].TestPt[4].X = 0;
                        Para.Module[m].TestPt[4].Y = 0;
                    }
                
                FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "CamToOriginOffsetX", ref strread);
                if (strread != "0")
                    Para.Module[m].CamToOriginOffset.X = double.Parse(strread);
                FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "CamToOriginOffsetY", ref strread);
                if (strread != "0")
                    Para.Module[m].CamToOriginOffset.Y = double.Parse(strread);
                FileOperation.ReadData(fileName, "Module" + (m + 1).ToString(), "AngleOffset", ref strread);
                if (strread != "0")
                    Para.Module[m].AngleOffset = float.Parse(strread);
            }
            Para.CurLoadConfigFileName = fileName;
            this.Text = AppTitle + " < " + fileName + " > ";

            
            specMgr.LoadTestCriteria(Para.CurLoadConfigFileName);
        }
        private void SaveSettings(string fileName)
        {
            FileOperation.SaveData(fileName, "TestPoint", "Count", Para.TestPtCnt.ToString());

            FileOperation.SaveData(fileName, "SampleShape", "Shape", Para.SampleShape.ToString());

            FileOperation.SaveData(fileName, "ExposureTime", "Camera1B", Para.Cam1ExposureTimeB.ToString());
            FileOperation.SaveData(fileName, "ExposureTime", "Camera1W", Para.Cam1ExposureTimeW.ToString());
            FileOperation.SaveData(fileName, "ExposureTime", "Camera2B", Para.Cam2ExposureTimeB.ToString());
            FileOperation.SaveData(fileName, "ExposureTime", "Camera2W", Para.Cam2ExposureTimeW.ToString());

            FileOperation.SaveData(fileName, "ExposureTime", "Camera1Exp1", Para.Cam1ExposureTime1.ToString());
            FileOperation.SaveData(fileName, "ExposureTime", "Camera1Exp3", Para.Cam1ExposureTime3.ToString());
            FileOperation.SaveData(fileName, "ExposureTime", "Camera2Exp1", Para.Cam2ExposureTime1.ToString());
            FileOperation.SaveData(fileName, "ExposureTime", "Camera2Exp3", Para.Cam2ExposureTime3.ToString());

            for (int m = 0; m < Para.ModCount; m++)
            {                
                for (int i = 0; i < Para.TeachPosCount; i++)
                {                    
                    FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "TeachPosX" + (i + 1).ToString(), Para.Module[m].TeachPos[i].X.ToString("F3"));
                    FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "TeachPosY" + (i + 1).ToString(), Para.Module[m].TeachPos[i].Y.ToString("F3"));
                    FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "TeachPosZ" + (i + 1).ToString(), Para.Module[m].TeachPos[i].Z.ToString("F3")); 
                }
                
                for (int i = 0; i < Para.TestPtCnt; i++)
                {
                    FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "TestPtX" + (i + 1).ToString(), Para.Module[m].TestPt[i].X.ToString("F3"));
                    FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "TestPtY" + (i + 1).ToString(), Para.Module[m].TestPt[i].Y.ToString("F3"));
                    FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "EnbTestPt" + (i + 1).ToString(), Para.Module[m].TestPtEnb[i].ToString());
                }

                FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "CamToOriginOffsetX", Para.Module[m].CamToOriginOffset.X.ToString("F3"));
                FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "CamToOriginOffsetY", Para.Module[m].CamToOriginOffset.Y.ToString("F3"));
                FileOperation.SaveData(fileName, "Module" + (m + 1).ToString(), "AngleOffset", Para.Module[m].AngleOffset.ToString("F3"));                

            }
            Para.CurLoadConfigFileName = fileName;
            this.Text = AppTitle + " < " + fileName + " > ";

            specMgr.SaveTestCriteria(Para.CurLoadConfigFileName);
        }
        private void InitMod1TransChart()//初始化Chart函数
        {
            Mod1Chart.Series["Series1"].Points.Clear();
            Mod1Chart.Series[0].Color = Color.Green;//曲线颜色
            //chart3.Series[0].Name=
            Mod1Chart.Legends[0].Position.X = 10; //标题位置
            Mod1Chart.Legends[0].Position.Y = 2;
            //  
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔

            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 100;//最大刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Interval = 10;//刻度间隔

            //设置坐标轴名称
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Title = "Trans";// "数值";

            //设置网格的颜色
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

        }
        private void InitMod1Chart(double MaxVal)
        {
            //Module 1 Chart
            Mod1Chart.Series[0].Color = Color.Red;//曲线颜色            
            Mod1Chart.Series[0].BorderWidth = 3;
            Mod1Chart.Legends[0].Position.X = 10; //标题位置
            Mod1Chart.Legends[0].Position.Y = 2;

            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔
            // chart3.ChartAreas["ChartArea1"].AxisX.de

            if (specMgr.GetType(0) == SpectType.CAS140)
            {
                Mod1Chart.ChartAreas["ChartArea1"].AxisY.Maximum = MaxVal;//2000;//最大刻度
                Mod1Chart.ChartAreas["ChartArea1"].AxisY.Interval = MaxVal/10;//刻度间隔
            }
            else
            {
                Mod1Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度   
                Mod1Chart.ChartAreas["ChartArea1"].AxisY.Interval = 5000;//刻度间隔
            }
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            //Mod1Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度
            

            //设置坐标轴名称
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Title = "Counts";// "数值";

            //设置网格的颜色
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            Mod1Chart.Series["Series1"].Points.Clear();
        }
        private void InitMod2TransChart()//初始化Chart函数
        {
            Mod2Chart.Series["Series1"].Points.Clear();
            Mod2Chart.Series[0].Color = Color.Green;//曲线颜色
            //chart3.Series[0].Name=
            Mod2Chart.Legends[0].Position.X = 10; //标题位置
            Mod2Chart.Legends[0].Position.Y = 2;
            //  
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔

            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 100;//最大刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Interval = 10;//刻度间隔

            //设置坐标轴名称
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Title = "Trans";// "数值";

            //设置网格的颜色
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

        }
        private void InitMod2Chart(double MaxVal)
        {
            //Module 1 Chart
            Mod2Chart.Series[0].Color = Color.Red;//曲线颜色            
            Mod2Chart.Series[0].BorderWidth = 3;
            Mod2Chart.Legends[0].Position.X = 10; //标题位置
            Mod2Chart.Legends[0].Position.Y = 2;

            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔
            // chart3.ChartAreas["ChartArea1"].AxisX.de
            if (specMgr.GetType(1) == SpectType.CAS140)
            {
                Mod2Chart.ChartAreas["ChartArea1"].AxisY.Maximum = MaxVal;//2000;//最大刻度
                Mod2Chart.ChartAreas["ChartArea1"].AxisY.Interval = MaxVal/10;//刻度间隔
            }
            else
            {
                Mod2Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度 
                Mod2Chart.ChartAreas["ChartArea1"].AxisY.Interval = 5000;//刻度间隔
            }

            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            //Mod2Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度
            

            //设置坐标轴名称
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Title = "Counts";// "数值";

            //设置网格的颜色
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            Mod2Chart.Series["Series1"].Points.Clear();
        }

        private void InitCharts()
        {
            //Module 1 Chart
            Mod1Chart.Series[0].Color = Color.Red;//曲线颜色            
            Mod1Chart.Series[0].BorderWidth = 3;
            Mod1Chart.Legends[0].Position.X = 10; //标题位置
            Mod1Chart.Legends[0].Position.Y = 2;

            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔
            // chart3.ChartAreas["ChartArea1"].AxisX.de
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Interval = 5000;//刻度间隔

            //设置坐标轴名称
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.Title = "Counts";// "数值";

            //设置网格的颜色
            Mod1Chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            Mod1Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            Mod1Chart.Series["Series1"].Points.Clear();

            //Module 2 Chart
            Mod2Chart.Series[0].Color = Color.Red;//曲线颜色            
            Mod2Chart.Series[0].BorderWidth = 3;            

            Mod2Chart.Legends[0].Position.X = 10; //标题位置
            Mod2Chart.Legends[0].Position.Y = 2;            

            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Minimum = 400;//最小刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Maximum = 1100;//最大刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Interval = 50;//刻度间隔
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;//最小刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 62000;//最大刻度
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Interval = 5000;//刻度间隔

            //设置坐标轴名称
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.Title = "WaveLength(nm)";// "随机数";
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.Title = "Counts";// "数值";

            //设置网格的颜色
            Mod2Chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            Mod2Chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            Mod2Chart.Series["Series1"].Points.Clear();

        }

        private void labelCamera_Click(object sender, EventArgs e)
        {
            this.mainTC.SelectedTab = this.CamTP;
        }

        private void labelSet_Click(object sender, EventArgs e)
        {
            this.mainTC.SelectedTab = this.SetTP;
        }

        private void labelHome_Click(object sender, EventArgs e)
        {
            this.mainTC.SelectedTab = this.HomeTP;
        }

        private void mainWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            TimeSpan tp = DateTime.Now - Para.SWRunTime;
            FileOperation.SaveData(Para.MchConfigFileName, "LSUsedTime", "Time", (Para.LSUsedTime + tp.TotalHours).ToString("F1"));

            camera1.StopCamera();
            camera2.StopCamera();
            Application.DoEvents();
            Thread.Sleep(500);

            RotIOTimer.Enabled = false;
            JP1.DisableTimer();
            JP2.DisableTimer();
            JP3.DisableTimer();
            myIO.SetIOTimer(false);
            SeqMgr.StopAuto();

            Thread.Sleep(200);

            
            camera1.CloseCamera();
            camera2.CloseCamera();
            if (Para.MachineOnline == false)
                return;
            SaveMchSettings();
            //camera1.SaveSettings(mchSettingsFilePath);
            //camera2.SaveSettings(mchSettingsFilePath);
                        
            motionMgr.ResetIO();
            motionMgr.resetRM32NT();
        }

        private void UpdateUI()
        {
            //button17.Enabled = false;
            checkBox7.Enabled = false;
            checkBox6.Enabled = false;
            //checkBox6.Checked = true;
            //checkBox6.Visible = false;
            //buttonAuditBox.Enabled = false;
            PicPath.Enabled = false;
            mchNameEB.Text = Para.MchName;
            PosLB.Items.Clear();

            for (int i = 0; i < Para.TeachPosCount; i++)
                PosLB.Items.Add(Para.posName[i]);

            ModSelCB.Items.Clear();
            for (int i = 0; i < Para.ModCount; i++)
                ModSelCB.Items.Add((i + 1).ToString());
            if (Para.Module.Count != 0)
            {
                ModSelCB.SelectedIndex = 0;
                UpdateModuleUI(ModSelCB.SelectedIndex);
            }
            if (Para.EnbStation4Unloading)
            {
                label53.Text = "Station 4 Status";
                label52.Text = "双人操作模式";
            }
            else
            {
                label52.Text = "单人操作模式";
                label53.Text = "Station 1 Status";
            }
        }
        private void UpdateModuleUI(int ModIdx)
        {            
            //TestPtX1EB.Text = Para.Module[ModIdx].TestPt[0].X.ToString("F3");
            //TestPtY1EB.Text = Para.Module[ModIdx].TestPt[0].Y.ToString("F3");
            //TestPtX2EB.Text = Para.Module[ModIdx].TestPt[1].X.ToString("F3");
            //TestPtY2EB.Text = Para.Module[ModIdx].TestPt[1].Y.ToString("F3");
            //TestPtX3EB.Text = Para.Module[ModIdx].TestPt[2].X.ToString("F3");
            //TestPtY3EB.Text = Para.Module[ModIdx].TestPt[2].Y.ToString("F3");
            EnbTestPt1CB.Checked = Para.Module[ModIdx].TestPtEnb[0];
            EnbTestPt2CB.Checked = Para.Module[ModIdx].TestPtEnb[1];
            EnbTestPt3CB.Checked = Para.Module[ModIdx].TestPtEnb[2];
            EnbTestPt4CB.Checked = Para.Module[ModIdx].TestPtEnb[3];
            EnbTestPt5CB.Checked = Para.Module[ModIdx].TestPtEnb[4];
            
            testPt1XEB.Text = Para.Module[ModIdx].TestPt[0].X.ToString("F3");
            testPt1YEB.Text = Para.Module[ModIdx].TestPt[0].Y.ToString("F3");
            testPt2XEB.Text = Para.Module[ModIdx].TestPt[1].X.ToString("F3");
            testPt2YEB.Text = Para.Module[ModIdx].TestPt[1].Y.ToString("F3");
            testPt3XEB.Text = Para.Module[ModIdx].TestPt[2].X.ToString("F3");
            testPt3YEB.Text = Para.Module[ModIdx].TestPt[2].Y.ToString("F3");
            testPt4XEB.Text = Para.Module[ModIdx].TestPt[3].X.ToString("F3");
            testPt4YEB.Text = Para.Module[ModIdx].TestPt[3].Y.ToString("F3");
            testPt5XEB.Text = Para.Module[ModIdx].TestPt[4].X.ToString("F3");
            testPt5YEB.Text = Para.Module[ModIdx].TestPt[4].Y.ToString("F3");

            CamToOriginOffsetXEB.Text = Para.Module[ModIdx].CamToOriginOffset.X.ToString("F3");
            CamToOriginOffsetYEB.Text = Para.Module[ModIdx].CamToOriginOffset.Y.ToString("F3");
            CamToOriginOffsetAngEB.Text = Para.Module[ModIdx].AngleOffset.ToString("F3");

            PosLB.SelectedIndex = 0;
            UpdateTeachUI(ModSelCB.SelectedIndex, PosLB.SelectedIndex);
        }
        private void UpdateTeachUI(int ModIdx, int TeachIdx)
        {
            Axislist JP1Axis, JP2Axis, JP3Axis;
            switch (ModIdx)
			{
				case 0: //Module 1
                    JP1Axis = Axislist.Mod1XAxis;
                    JP2Axis = Axislist.Mod1YAxis;
                    if (Para.EnableZaxis)
                    {
                        JP3Axis = Axislist.Mod1ZAxis;
                        JP3.Assign(JP3Axis, Para.Module[ModIdx].TeachPos[TeachIdx].Z, JogDir.UpDown, false);
                    }
					break;
                case 1: //Module 2
                    JP1Axis = Axislist.Mod2XAxis;
                    JP2Axis = Axislist.Mod2YAxis;
                    if (Para.EnableZaxis)
                    {
                        JP3Axis = Axislist.Mod2ZAxis;
                        JP3.Assign(JP3Axis, Para.Module[ModIdx].TeachPos[TeachIdx].Z, JogDir.UpDown, false);
                    }
                    break;
				default:
					JP1Axis = Axislist.Mod1XAxis;
                    JP2Axis = Axislist.Mod1YAxis;
                    if (Para.EnableZaxis)
                    {
                        JP3Axis = Axislist.Mod1ZAxis;
                        JP3.Assign(JP3Axis, Para.Module[ModIdx].TeachPos[TeachIdx].Z, JogDir.UpDown, false);
                    }
					break;
			}

            JP1.Assign(JP1Axis, Para.Module[ModIdx].TeachPos[TeachIdx].X, JogDir.LeftRight, true);
            JP2.Assign(JP2Axis, Para.Module[ModIdx].TeachPos[TeachIdx].Y, JogDir.UpDown, false);

            if (TeachIdx == 0)
                JP3.Visible = true;
            else
                JP3.Visible = false;           
        }
        private void UpdateConfigUI()
        {
            if (configCB.Items.Count != FileOperation.GetfileNames(configFolderPath).Count())
            {
                configCB.Items.Clear();
                List<string> myName = FileOperation.GetfileNames(configFolderPath);
                for (int i = 0; i < myName.Count; i++)
                    configCB.Items.Add(myName[i]);
                configCB.SelectedIndex = configCB.Items.IndexOf(FileOperation.ExtractFileName(Para.CurLoadConfigFileName));
            }
        }        

        private void mchNameEB_Leave(object sender, EventArgs e)
        {
            Para.MchName = mchNameEB.Text;
        }

        private void ModSelCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModuleUI(ModSelCB.SelectedIndex);
        }

        private void PosLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTeachUI(ModSelCB.SelectedIndex, PosLB.SelectedIndex);
        }

        private void OnJP1ApplyClick(double myPos)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            int TeachIdx = PosLB.SelectedIndex;
            Para.Module[ModIdx].TeachPos[TeachIdx].X = myPos;
        }
        private void OnJP2ApplyClick(double myPos)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            int TeachIdx = PosLB.SelectedIndex;
            Para.Module[ModIdx].TeachPos[TeachIdx].Y = myPos;
        }
        private void OnJP3ApplyClick(double myPos)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            int TeachIdx = PosLB.SelectedIndex;
            Para.Module[ModIdx].TeachPos[TeachIdx].Z = myPos;
        }
        private void CamToOriginOffsetXEB_Leave(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].CamToOriginOffset.X = double.Parse(CamToOriginOffsetXEB.Text);
        }

        private void CamToOriginOffsetYEB_Leave(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].CamToOriginOffset.Y = double.Parse(CamToOriginOffsetYEB.Text);
        }

        private void CreateNewBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Create New Setting File?", AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
            {
                String input = Microsoft.VisualBasic.Interaction.InputBox("New Setting Name", "Name", "JPTCGNew");
                if (input == "")
                    return;                
                SaveSettings(configFolderPath+input+".xml");
                UpdateConfigUI();
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveSettings(Para.CurLoadConfigFileName);     
        }

        private void configCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (configCB.SelectedIndex == -1)
                return;
            string input = configCB.Items[configCB.SelectedIndex].ToString();
            if (FileOperation.ExtractFileName(Para.CurLoadConfigFileName) == input)
                return;

            if (MessageBox.Show("Load Setting File?", AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
            {
                LoadSettings(configFolderPath+input+".xml");
                UpdateConfigUI();
                UpdateUI();
            }
            else
            {
                configCB.SelectedIndex = configCB.Items.IndexOf(FileOperation.ExtractFileName(Para.CurLoadConfigFileName));
            }
        }

        private void TestPtX1EB_Leave(object sender, EventArgs e)
        {
            //int ModIdx = ModSelCB.SelectedIndex;
            //int TestIdx = 0;
            //Para.Module[ModIdx].TestPt[TestIdx].X = double.Parse(TestPtX1EB.Text);
        }

        private void TestPtY1EB_Leave(object sender, EventArgs e)
        {
            //int ModIdx = ModSelCB.SelectedIndex;
            //int TestIdx = 0;
            //Para.Module[ModIdx].TestPt[TestIdx].Y = double.Parse(TestPtY1EB.Text);
        }

        private void TestPtX2EB_Leave(object sender, EventArgs e)
        {
            //int ModIdx = ModSelCB.SelectedIndex;
            //int TestIdx = 1;
            //Para.Module[ModIdx].TestPt[TestIdx].X = double.Parse(TestPtX2EB.Text);
        }

        private void TestPtX3EB_Leave(object sender, EventArgs e)
        {
            //int ModIdx = ModSelCB.SelectedIndex;
            //int TestIdx = 2;
            //Para.Module[ModIdx].TestPt[TestIdx].X = double.Parse(TestPtX3EB.Text);
        }

        private void TestPtY2EB_Leave(object sender, EventArgs e)
        {
            //int ModIdx = ModSelCB.SelectedIndex;
            //int TestIdx = 1;
            //Para.Module[ModIdx].TestPt[TestIdx].Y = double.Parse(TestPtY2EB.Text);
        }

        private void TestPtY3EB_Leave(object sender, EventArgs e)
        {
            //int ModIdx = ModSelCB.SelectedIndex;
            //int TestIdx = 2;
            //Para.Module[ModIdx].TestPt[TestIdx].Y = double.Parse(TestPtY3EB.Text);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            motionMgr.ShowSettings();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            specMgr.ShowSettings(motionMgr);
        }
        public void DisplayBarcode(int ModIndex, string BarCode)
        {
            Action ac = new Action(() =>
            {
                switch(ModIndex)
                {
                    case 1:
                        if (BarCode.Length > 17)
                            Mod1BarLbl.Text = BarCode;
                        else
                            Mod1BarLbl.Text = BarCode;
                        break;
                    case 2:
                        if (BarCode.Length > 17)
                            Mod2BarLbl.Text = BarCode;
                        else
                            Mod2BarLbl.Text = BarCode;
                        break;
                }
            });
            Mod1BarLbl.BeginInvoke(ac);
        }
        public List<float> GetMod1Spec()
        {
            List<float> cnt = new List<float>();
            Action ac = new Action(() =>
            {
                cnt = specMgr.GetCount(0);
            });
            this.BeginInvoke(ac);

            return cnt;
        }

        public List<float> GetMod1WL()
        {
            List<float> cnt = new List<float>();
            Action ac = new Action(() =>
            {
                cnt = specMgr.GetWaveLength(0);
            });
            this.BeginInvoke(ac);

            return cnt;
        }
        public void UpdateMod1Chart(List<float> wl, List<float> cnt, bool isRatio)
        {
            Action ac = new Action(() =>
            {
                if (isRatio)
                    InitMod1TransChart();
                else
                    InitMod1Chart(cnt.Max()+1);
                    
                UpdateMod1Chart(wl, cnt);
            });
            Mod1Chart.BeginInvoke(ac); 
        }
        public void UpdateMod1TestStatus(string status, Color myColor)
        {
            Action ac = new Action(() =>
            {
                Mod1TestStatusLbl.Text = status;
                Mod1TestStatusLbl.BackColor = myColor;
                if (Para.DebugMode)
                    listBox1.Items.Add(status);
            });
            Mod1TestStatusLbl.BeginInvoke(ac);
        }
        public void UpdateMod2TestStatus(string status, Color myColor)
        {
            Action ac = new Action(() =>
            {
                Mod2TestStatusLbl.Text = status;
                Mod2TestStatusLbl.BackColor = myColor;
                if (Para.DebugMode)
                    listBox2.Items.Add(status);
            });
            Mod2TestStatusLbl.BeginInvoke(ac);
        }
        public void UpdateMod1TestResult(int Mod1Res ,int indexEandC)
        {
            Action ac = new Action(() =>
            {
                if (Mod1Res==0)
                {
                    Mod1TestResLbl.ForeColor = Color.Lime;
                    Mod1TestResLbl.Text = "PASS";
                    errcode1.Text = Para.errorCode1Array[indexEandC];  //display errcode
                    errstr1.Text = Para.errorStr1Array[indexEandC];    //display errorString
                    binCode1.Text = Para.bin_Code1Array[indexEandC];   //bin_code
                    motionMgr.writeLightIO(2, true);
                    motionMgr.writeLightIO(3, true);
                    Thread.Sleep(100);
                    motionMgr.writeLightIO(11, true);
                    motionMgr.writeLightIO(12, true);
                    Thread.Sleep(500);
                    motionMgr.writeLightIO(2, false);
                    motionMgr.writeLightIO(3, true);
                    Thread.Sleep(100);
                    motionMgr.writeLightIO(11, false);
                    motionMgr.writeLightIO(12, true);
                }
                else if (Mod1Res == -1)
                    {
                        //Mod1TestResLbl.ForeColor = Color.Black;
                        //Mod1TestResLbl.Text = "N.A";
                        //if (!Para.res1FromMini)
                        //{
                        errcode1.Text = "";  //display errcode
                        errstr1.Text = "";    //display errorString
                        binCode1.Text = Para.bin_Code1Array[indexEandC];   //bin_Code
                            Mod1TestResLbl.ForeColor = Color.Blue;
                            Mod1TestResLbl.Text = "*Please Retest*";
                        //}
                        motionMgr.writeLightIO(2, false);
                        motionMgr.writeLightIO(3, false);
                        Thread.Sleep(100);
                        motionMgr.writeLightIO(11, false);
                        motionMgr.writeLightIO(12, false);
                    }
                    else
                    {
                        Mod1TestResLbl.ForeColor = Color.Red;
                        Mod1TestResLbl.Text = "FAIL";
                        errcode1.Text = Para.errorCode1Array[indexEandC];  //display errcode
                        errstr1.Text = Para.errorStr1Array[indexEandC];    //display errorString
                        binCode1.Text = Para.bin_Code1Array[indexEandC];   //bin_Code
                        motionMgr.writeLightIO(2, true);
                        motionMgr.writeLightIO(3, true);
                        Thread.Sleep(100);
                        motionMgr.writeLightIO(11, true);
                        motionMgr.writeLightIO(12, true);
                        Thread.Sleep(500);
                        motionMgr.writeLightIO(2, true);
                        motionMgr.writeLightIO(3, false);
                        Thread.Sleep(100);
                        motionMgr.writeLightIO(11, true);
                        motionMgr.writeLightIO(12, false);
                    }
                if (SeqMgr.stationStatus[Para.CurrentRotaryIndex].mod1VisResult.InspectedImage.IsInitialized())
                {
                    HOperatorSet.WriteImage(SeqMgr.stationStatus[Para.CurrentRotaryIndex].mod1VisResult.InspectedImage, "bmp", 0, "D:\\1.bmp");
                    HImage image = new HImage("D:\\1.bmp");
                    hWndCtrlRes1.addIconicVar(image);
                }//20161117
                //hWndCtrlRes1.addIconicVar(SeqMgr.stationStatus[Para.CurrentRotaryIndex].mod1VisResult.InspectedImage);
            });
            Mod1TestResLbl.BeginInvoke(ac);
        }
        public void UpdateMod2TestResult(int Mod2Res, int indexEandC)
        {
            Action ac = new Action(() =>
            {                
                if (Mod2Res==0)
                {
                    Mod2TestResLbl.ForeColor = Color.Lime;
                    Mod2TestResLbl.Text = "PASS";
                    errcode2.Text = Para.errorCode2Array[indexEandC];  //display errcode
                    errstr2.Text = Para.errorStr2Array[indexEandC];    //display errorString
                    binCode2.Text = Para.bin_Code2Array[indexEandC];   //bin_Code
                    motionMgr.writeLightIO(4, true);
                    motionMgr.writeLightIO(5, true);
                    Thread.Sleep(100);
                    motionMgr.writeLightIO(13, true);
                    motionMgr.writeLightIO(14, true);
                    Thread.Sleep(500);
                    motionMgr.writeLightIO(4, false);
                    motionMgr.writeLightIO(5, true);
                    Thread.Sleep(100);
                    motionMgr.writeLightIO(13, false);
                    motionMgr.writeLightIO(14, true);
                }
                else if (Mod2Res == -1)
                    {
                        //Mod2TestResLbl.ForeColor = Color.Black;
                        //Mod2TestResLbl.Text = "N.A";
                        //if (!Para.res2FromMini)
                        //{
                        errcode2.Text = "";  //display errcode
                        errstr2.Text = "";   //display errorString
                        binCode2.Text = "";  //bin_code
                            Mod2TestResLbl.ForeColor = Color.Blue;
                            Mod2TestResLbl.Text = "*Please Retest*";
                        //}
                        motionMgr.writeLightIO(4, false);
                        motionMgr.writeLightIO(5, false);
                        Thread.Sleep(100);
                        motionMgr.writeLightIO(13, false);
                        motionMgr.writeLightIO(14, false);
                    }
                else
                    {
                        Mod2TestResLbl.ForeColor = Color.Red;
                        Mod2TestResLbl.Text = "FAIL";
                        errcode2.Text = Para.errorCode2Array[indexEandC];  //display errcode
                        errstr2.Text = Para.errorStr2Array[indexEandC];    //display errorString
                        binCode2.Text = Para.bin_Code2Array[indexEandC];   //bin_Code
                        motionMgr.writeLightIO(4, true);
                        motionMgr.writeLightIO(5, true);
                        Thread.Sleep(100);
                        motionMgr.writeLightIO(13, true);
                        motionMgr.writeLightIO(14, true);
                        Thread.Sleep(500);
                        motionMgr.writeLightIO(4, true);
                        motionMgr.writeLightIO(5, false);
                        Thread.Sleep(100);
                        motionMgr.writeLightIO(13, true);
                        motionMgr.writeLightIO(14, false);
                    }
                if (SeqMgr.stationStatus[Para.CurrentRotaryIndex].mod2VisResult.InspectedImage.IsInitialized())
                {
                    HOperatorSet.WriteImage(SeqMgr.stationStatus[Para.CurrentRotaryIndex].mod2VisResult.InspectedImage, "bmp", 0, "D:\\2.bmp");
                    HImage image = new HImage("D:\\2.bmp");
                    hWndCtrlRes2.addIconicVar(image);
                }
                //Mod2TestResLbl.Text = " ";
                //hWndCtrlRes2.addIconicVar(SeqMgr.stationStatus[Para.CurrentRotaryIndex].mod2VisResult.InspectedImage);
            });
            Mod2TestResLbl.BeginInvoke(ac);
        }
        public void UpdateMod2Chart(List<float> wl, List<float> cnt, bool isRatio)
        {
            Action ac = new Action(() =>
            {
                if (isRatio)
                    InitMod2TransChart();
                else
                    InitMod2Chart(cnt.Max()+1);

                UpdateMod2Chart(wl, cnt);
            });
            Mod2Chart.BeginInvoke(ac);
        }
        private void UpdateMod1Chart(List<float> myWL, List<float> myCnt)
        {
            for (int i = 0; i < myWL.Count; i++)
            {
                if (myCnt[i] < 0)
                    Mod1Chart.Series["Series1"].Points.AddXY(myWL[i], 0);
                else
                    Mod1Chart.Series["Series1"].Points.AddXY(myWL[i], myCnt[i]);
            }
            Mod1Chart.Series["Series1"].ChartType = SeriesChartType.FastLine;
        }
        private void UpdateMod2Chart(List<float> myWL, List<float> myCnt)
        {
            for (int i = 0; i < myWL.Count; i++)
            {
                Mod2Chart.Series["Series1"].Points.AddXY(myWL[i], myCnt[i]);
            }
            Mod2Chart.Series["Series1"].ChartType = SeriesChartType.FastLine;
        }
        public void ClearInspectionResults()
        {
            Action ac = new Action(() =>
            {
                hWndCtrl1.ClearResult();
                hWndCtrl2.ClearResult();
            });
            HalconWin1.BeginInvoke(ac);             
        }
        public void DisplayCam1Result(JPTCG.Vision.HalconInspection.RectData myRes)
        {
            Action ac = new Action(() =>
            {
                hWndCtrl1.DrawRect(myRes,camera1.myImage);
                int RotaryIdxAtCam = SeqMgr.GetIndexOfCam();
                //hWndCtrl1.InspectedImage(ref SeqMgr.stationStatus[RotaryIdxAtCam].Mod1InspectedImage);
            });
            HalconWin1.BeginInvoke(ac);            
        }
        public void DisplayCam2Result(JPTCG.Vision.HalconInspection.RectData myRes)
        {
            Action ac = new Action(() =>
            {
                hWndCtrl2.DrawRect(myRes, camera2.myImage);
                int RotaryIdxAtCam = SeqMgr.GetIndexOfCam();
                //hWndCtrl2.InspectedImage(ref SeqMgr.stationStatus[RotaryIdxAtCam].Mod2InspectedImage);
            });
            HalconWin2.BeginInvoke(ac);
        }
        public void ShowErrorMessageBox(string error, string ErrTitle)
        {
            Action ac = new Action(() =>
            {
                //MessageBox.Show(error);
                MessageBox.Show(error, ErrTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
            this.BeginInvoke(ac);
        }

        public void WriteOperationinformation(string LogInfor)
        {
            if (RunLogDG.InvokeRequired)
            {
                RunLogDG.Invoke(
                    new Action(() =>
                    {
                        RunLogDG.RowCount = RunLogDG.RowCount + 1;
                        int rwCnt = RunLogDG.RowCount;
                        RunLogDG.ColumnCount = 2;
                        RunLogDG.Rows[rwCnt - 1].Cells[0].Value = DateTime.Now.ToString("dd MM HH:mm:ss");
                        RunLogDG.Rows[rwCnt - 1].Cells[1].Value = LogInfor;
                        RunLogDG.FirstDisplayedScrollingRowIndex = RunLogDG.Rows.Count - 1;
                    })

                    );
            }
            else
            {
                RunLogDG.RowCount = RunLogDG.RowCount + 1;
                int rwCnt = RunLogDG.RowCount;
                RunLogDG.ColumnCount = 2;
                RunLogDG.Rows[rwCnt - 1].Cells[0].Value = DateTime.Now.ToString("dd MM HH:mm:ss");
                RunLogDG.Rows[rwCnt - 1].Cells[1].Value = LogInfor;
                RunLogDG.FirstDisplayedScrollingRowIndex = RunLogDG.Rows.Count - 1;
            }

        }

        private void btnALLaxisgoHome_Click(object sender, EventArgs e)
        {
            
        }

        private void debugBtn_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Int16.MaxValue.ToString());
            this.mainTC.SelectedTab = this.debugTP;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SeqMgr.AssignUI(MainSeqLV, CamDebugLV,TestStationLV, checkBox1.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SeqMgr.StartAuto();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SeqMgr.StopAuto();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SeqMgr.PauseAuto();
        }

        private void labelStart_Click(object sender, EventArgs e)
        {
            string strTemp = "";
            FileOperation.ReadData(mchSettingsFilePath, "LightSource", "LSType", ref strTemp);//20170303@Brando
            //strTemp = strTemp.Substring(0, 2);
            //if (strTemp != "EQ")//20170303@Brando
            //{
            //    MessageBox.Show("The Light Source not Match,Please change it!");
            //}
            if (!strTemp.Contains( "EQ"))
            {
                MessageBox.Show("The Light Source not Match,Please change it!");
            }
            else
            {
                motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, 0);
                motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, 0);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);

                if (camera1.IsCameraOpen())
                {
                    camera1.StopCamera();
                    //EnbToolBtn(true);
                    camStatusLbl.Text = "Idle...";
                    camStatusLbl.BackColor = Color.Orange;
                }

                if (camera2.IsCameraOpen())
                {
                    camera2.StopCamera();
                    //EnbToolBtn(true);
                    camStatusLbl2.Text = "Idle...";
                    camStatusLbl2.BackColor = Color.Orange;
                }

                if (Para.EnableCGHost)
                {
                    if (!AMgrMod1.Connect(AMgrMod1.IP, AMgrMod1.Port))
                    {
                        MessageBox.Show("CGClient Module 1 Not To Connect To CGHost.");
                        return;
                    }
                    //AMgrMod1.Disconnect();
                    if (!AMgrMod2.IsConnected)
                        if (!AMgrMod2.Connect(AMgrMod2.IP, AMgrMod2.Port))
                        {
                            MessageBox.Show("CGClient Module 2 Not To Connect To CGHost.");
                            return;
                        }
                }

                if (DLRS1.Enable)
                {
                    DLRS1.Disconnect();
                    Thread.Sleep(100);
                    if (!DLRS1.Connect(DLRS1.MyPortName))
                    {
                        MessageBox.Show(" Module 1 Height Senser not Connected");
                        return;
                    }
                }
                if (DLRS2.Enable)
                {
                    DLRS2.Disconnect();
                    Thread.Sleep(100);
                    if (!DLRS2.Connect(DLRS2.MyPortName))
                    {
                        MessageBox.Show("Module 2 Height Senser not Connected");
                        return;
                    }
                }

                labelHome_Click(sender, e);
                Thread.Sleep(100);
                HomeBtn.Enabled = false;
                SeqMgr.StartAuto();
                UIStartClicked();
            }//20170303@Brando
        }

        public void UIStartClicked()
        {
            HomeBtn.Enabled = false;
            labelStart.Enabled = false;
            labelStop.Enabled = true;
            labelPause.Enabled = true;
            

            RunningLight();

            if (!Para.MachineOnline)
            {
                StartLightLbl.BackColor = Color.Lime;
                IdleLightLbl.BackColor = Color.Black;
                ErrorLightLbl.BackColor = Color.Black;
            }

            labelSet.Enabled = false;
            labelCamera.Enabled = false;
            WriteOperationinformation("Auto Run Started");
        }
        private void labelStop_Click(object sender, EventArgs e)
        {
            SeqMgr.StopAuto();
            UIStopClicked();
        }

        public void UIStopClicked()
        {
            HomeBtn.Enabled = true;
            labelStart.Enabled = true;
            labelStop.Enabled = false;
            labelPause.Enabled = false;

            labelSet.Enabled = true;
            labelCamera.Enabled = true;            

            WriteOperationinformation("Auto Run Stop");
            StoppingLight();
            if (!Para.MachineOnline)
            {
                StartLightLbl.BackColor = Color.Black;
                IdleLightLbl.BackColor = Color.Orange;
                ErrorLightLbl.BackColor = Color.Black;
            }

            if (Para.isRotaryError)
            {
                labelStart.Enabled = false;
                labelPause.Enabled = false;
                labelStop.Enabled = false;
            }
        }
        private void labelPasue_Click(object sender, EventArgs e)
        {
            SeqMgr.PauseAuto();
            UIPauseClicked(false);
        }
        private void RunningLight()
        {
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampGreen, true);
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampAmber, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampRed, false);
            UpdateStatusLbl(Color.Lime);
        }

        private void StoppingLight()
        {
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampGreen, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampAmber, true);
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampRed, false);
            UpdateStatusLbl(Color.Orange);
        }

        private void ErrorLight()
        {
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampGreen, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampAmber, false);
            motionMgr.WriteIOOut((ushort)OutputIOlist.LampRed, true);
            UpdateStatusLbl(Color.Red);
        }
        public void OnlyHomeEnb()
        {
            Action ac = new Action(() =>
            {
                labelStart.Enabled = false;
                labelStop.Enabled = false;
                labelPause.Enabled = false;
                labelSet.Enabled = false;
                labelCamera.Enabled = false;
                HomeBtn.Enabled = true;
            });
            labelStart.BeginInvoke(ac);                        
        }

        public void AllDisabled()
        {
            Action ac = new Action(() =>
            {
                labelStart.Enabled = false;
                labelStop.Enabled = false;
                labelPause.Enabled = false;
                labelSet.Enabled = false;
                labelCamera.Enabled = false;
                HomeBtn.Enabled = false;
            });
            labelStart.BeginInvoke(ac);
        }

        public void UIPauseClicked(bool isError)
        {
            HomeBtn.Enabled = true;
               
            labelStart.Enabled = true;
            labelStop.Enabled = true;
            labelPause.Enabled = false;
            

            labelSet.Enabled = true;
            labelCamera.Enabled = true;

            if (isError)
            {
                ErrorLight();
                WriteOperationinformation("Auto Run Pause On Error");
            }
            else
            {
                StoppingLight();                
                WriteOperationinformation("Auto Run Pause");
            }

            if (Para.isRotaryError)
            {
                labelStart.Enabled = false;
                labelPause.Enabled = false;
                labelStop.Enabled = false;
            }
            if (!Para.MachineOnline)
            {
                StartLightLbl.BackColor = Color.Black;
                IdleLightLbl.BackColor = Color.Orange;
                if (isError)                
                    ErrorLightLbl.BackColor = Color.Red;
            }
        }

        private void labelLogin_Click(object sender, EventArgs e)
        {
            this.mainTC.SelectedTab = this.userTP;
        }

        private void BarcodeMenuLbl_Click(object sender, EventArgs e)
        {
            BarCMgr.ShowSettings();
        }

        private void label20_Click(object sender, EventArgs e)
        {
            AMgrMod1.ShowSettings();
            AMgrMod2.ShowSettings(); 
        }

        private void RotIOTimer_Tick(object sender, EventArgs e)
        {
            RotIndexTB.Text = Para.CurrentRotaryIndex.ToString();

            //Output
            if (motionMgr.ReadIOOut((ushort)OutputIOlist.RotaryEnabled))
            {
                if (RotEnbLbl.BackColor != Color.Lime)
                    RotEnbLbl.BackColor = Color.Lime;
            }
            else
            {
                if (RotEnbLbl.BackColor != Color.Black)
                    RotEnbLbl.BackColor = Color.Black;
            }

            //input
            if (motionMgr.ReadIOIn((ushort)InputIOlist.RotaryOrigin))
            {
                if (RotOriginLbl.BackColor != Color.Lime)
                    RotOriginLbl.BackColor = Color.Lime;
            }
            else
            {
                if (RotOriginLbl.BackColor != Color.Black)
                    RotOriginLbl.BackColor = Color.Black;
            }
            if (motionMgr.ReadIOIn((ushort)InputIOlist.RotaryMotionDone))
            {
                if (RotMotionDoneLbl.BackColor != Color.Lime)
                    RotMotionDoneLbl.BackColor = Color.Lime;
            }
            else
            {
                if (RotMotionDoneLbl.BackColor != Color.Black)
                    RotMotionDoneLbl.BackColor = Color.Black;
            }
            if (!motionMgr.ReadIOIn((ushort)InputIOlist.RotaryError))
            {
                if (RotErrorLbl.BackColor != Color.Lime)
                    RotErrorLbl.BackColor = Color.Lime;
            }
            else
            {
                if (RotErrorLbl.BackColor != Color.Black)
                    RotErrorLbl.BackColor = Color.Black;
            }
        }

        private void SetTP_Enter(object sender, EventArgs e)
        {
            RotIOTimer.Enabled = true;
            myIO.SetIOTimer(true);
        }

        private void SetTP_Leave(object sender, EventArgs e)
        {
            RotIOTimer.Enabled = false;
            myIO.SetIOTimer(false);
        }

        private void RotEnbBtn_Click(object sender, EventArgs e)
        {
            bool val = motionMgr.ReadIOOut((ushort)OutputIOlist.RotaryEnabled);
            motionMgr.WriteIOOut((ushort)OutputIOlist.RotaryEnabled, !val);
        }

        private void RotaryIndexBtn_Click(object sender, EventArgs e)
        {
            if ((!motionMgr.ReadIOIn((ushort)InputIOlist.SafetySensor)) && (!motionMgr.ReadIOIn((ushort)InputIOlist.BtnStop)))
            {
                if (Para.DisableSafeDoor)
                    RotMgr.IndexRotaryMotion();
                else
                {
                    if (motionMgr.ReadIOIn((ushort)InputIOlist.DoorSensor))
                        RotMgr.IndexRotaryMotion();
                    else
                    {
                        MessageBox.Show("安全门或安全光栅被触发 ");
                    }
                }
            }
            else
            {
                MessageBox.Show("安全门或安全光栅被触发 ");
            }
        }

        private void TestPtCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Para.TestPtCnt = int.Parse(TestPtCB.SelectedItem.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            motionMgr.Homing((ushort)Axislist.Mod1YAxis, 1);
        }

        private void CBDryRun_CheckedChanged(object sender, EventArgs e)
        {
            Para.DryRunMode = CBDryRun.Checked;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //SeqMgr.TestSafety();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //if ((!motionMgr.ReadIOIn((ushort)InputIOlist.SafetySensor)) && (!motionMgr.ReadIOIn((ushort)InputIOlist.BtnStop)))
            //{
            //    RotMgr.IndexRotaryMotionCCW();
            //}
            //else
            //{
            //    MessageBox.Show("SafetySensor has been Touched or Door opened ");
            //}
            //if(Para.DisableSafeDoor)
            //    RotMgr.IndexRotaryMotionCCW();
            if ((!motionMgr.ReadIOIn((ushort)InputIOlist.SafetySensor)) && (!motionMgr.ReadIOIn((ushort)InputIOlist.BtnStop)))
            {
                if (Para.DisableSafeDoor)
                    RotMgr.IndexRotaryMotionCCW();
                else
                {
                    if (motionMgr.ReadIOIn((ushort)InputIOlist.DoorSensor))
                        RotMgr.IndexRotaryMotionCCW();
                    else
                    {
                        MessageBox.Show("安全门或安全光栅被触发 ");
                    }
                }
            }
            else
            {
                MessageBox.Show("安全门或安全光栅被触发 ");
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            RotMgr.GoHome();
        }

        private void AssignCamBtn_Click(object sender, EventArgs e)
        {
            CameraSelectionWin camSelWin = new CameraSelectionWin(camera1);
            camSelWin.ShowDialog();
            camIDLbl.Text = camera1.cameraID;
            camera1.SaveSettings(mchSettingsFilePath);
        }

        private void LoadImgBtn_Click(object sender, EventArgs e)
        {
            string strHeadImagePath;
            HImage image;

            OpenImageDialog.Title = "Open Image file";
            OpenImageDialog.ShowHelp = true;
            OpenImageDialog.Filter = "(*.gif)|*.gif|(*.jpg)|*.jpg|(*.JPEG)|*.JPEG|(*.bmp)|*.bmp|(*.png)|*.png|All files (*.*)|*.*";
            DialogResult result = OpenImageDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    strHeadImagePath = OpenImageDialog.FileName;
                    image = new HImage(strHeadImagePath);
                    camera1.myImage = image;
                    hWndCtrl1.ClearResult();
                    hWndCtrl1.addIconicVar(image);
                    //hWndCtrl1.repaint();
                }
                catch
                {
                    MessageBox.Show("format not correct");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CameraSelectionWin camSelWin = new CameraSelectionWin(camera2);
            camSelWin.ShowDialog();
            camIDLbl2.Text = camera2.cameraID;
            camera2.SaveSettings(mchSettingsFilePath);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string strHeadImagePath;
            HImage image;

            OpenImageDialog.Title = "Open Image file";
            OpenImageDialog.ShowHelp = true;
            OpenImageDialog.Filter = "(*.gif)|*.gif|(*.jpg)|*.jpg|(*.JPEG)|*.JPEG|(*.bmp)|*.bmp|(*.png)|*.png|All files (*.*)|*.*";
            DialogResult result = OpenImageDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    strHeadImagePath = OpenImageDialog.FileName;
                    image = new HImage(strHeadImagePath);
                    camera2.myImage = image;
                    hWndCtrl2.ClearResult();
                    hWndCtrl2.addIconicVar(image);
                    //hWndCtrl2.repaint();
                }
                catch
                {
                    MessageBox.Show("format not correct");
                }
            }
        }

        private void TBFitToScreen_Click(object sender, EventArgs e)
        {
            try
            {
                lock (m_lockShowpicture1)
                {
                    hWndCtrl1.resetWindow();
                    hWndCtrl1.repaint();
                }
            }
            catch
            {
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                lock (m_lockShowpicture2)
                {
                    hWndCtrl2.resetWindow();
                    hWndCtrl2.repaint();
                }
            }
            catch
            {
            }
        }

        private void TBZoomIn_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture1)
            {
                hWndCtrl1.zoomImage(0.9);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture2)
            {
                hWndCtrl2.zoomImage(0.9);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture2)
            {
                hWndCtrl2.zoomImage(1.1);
            }
        }

        private void TBZoomOut_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture1)
            {
                hWndCtrl1.zoomImage(1.1);
            }
        }

        private void TBMove_Click(object sender, EventArgs e)
        {
            if (hWndCtrl1.GetViewState() == HWndCtrl.MODE_VIEW_MOVE)
                hWndCtrl1.setViewState(HWndCtrl.MODE_VIEW_NONE);
            else
                hWndCtrl1.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (hWndCtrl2.GetViewState() == HWndCtrl.MODE_VIEW_MOVE)
                hWndCtrl2.setViewState(HWndCtrl.MODE_VIEW_NONE);
            else
                hWndCtrl2.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        private void TBCrossHair_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture1)
            {
                hWndCtrl1.bShowCrossHair = !hWndCtrl1.bShowCrossHair;
                hWndCtrl1.repaint();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            lock (m_lockShowpicture2)
            {
                hWndCtrl2.bShowCrossHair = !hWndCtrl2.bShowCrossHair;
                hWndCtrl2.repaint();
            }
        }

        private void TBLiveCam_Click(object sender, EventArgs e)
        {
            if (camera1.IsCameraOpen())
            {
                hWndCtrl1.ClearResult();
                hWndCtrl1.setViewState(HWndCtrl.MODE_VIEW_NONE);
                hWndCtrl1.resetWindow();
                hWndCtrl1.repaint();
                camera1.LiveCamera();
                //EnbToolBtn(false);
                camStatusLbl.Text = "Live...";
                camStatusLbl.BackColor = Color.Lime;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (camera2.IsCameraOpen())
            {
                hWndCtrl2.ClearResult();
                hWndCtrl2.setViewState(HWndCtrl.MODE_VIEW_NONE);
                hWndCtrl2.resetWindow();
                hWndCtrl2.repaint();
                camera2.LiveCamera();
                //EnbToolBtn(false);
                camStatusLbl2.Text = "Live...";
                camStatusLbl2.BackColor = Color.Lime;
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (camera2.IsCameraOpen())
            {
                camera2.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl2.Text = "Idle...";
                camStatusLbl2.BackColor = Color.Orange;
            }
        }

        private void TBStopCam_Click(object sender, EventArgs e)
        {
            if (camera1.IsCameraOpen())
            {
                camera1.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl.Text = "Idle...";
                camStatusLbl.BackColor = Color.Orange;
            }
        }

        private void HalconWin1_HMouseMove(object sender, HMouseEventArgs e)
        {
            HTuple Window = new HTuple();
            Window = HalconWin1.HalconWindow;
            HTuple Row1, Col1, Button;
            try
            {
                HOperatorSet.GetMposition(Window, out Row1, out Col1, out Button);
                StatusLblX.Text = Col1.ToString();
                StatusLblY.Text = Row1.ToString();
            }
            catch
            {
            }
        }

        private void HalconWin2_HMouseMove(object sender, HMouseEventArgs e)
        {
            HTuple Window = new HTuple();
            Window = HalconWin2.HalconWindow;
            HTuple Row1, Col1, Button;
            try
            {
                HOperatorSet.GetMposition(Window, out Row1, out Col1, out Button);
                StatusLblX2.Text = Col1.ToString();
                StatusLblY2.Text = Row1.ToString();
            }
            catch
            {
            }
        }

        private void InspectBtn_Click(object sender, EventArgs e)
        {
            if (camera1.IsCameraLive())
                camera1.StopCamera();
            Thread.Sleep(200);            
            hWndCtrl1.ClearResult();
            //camera1.Grab();
            //Thread.Sleep(200);
            JPTCG.Vision.HalconInspection.RectData myRes = camera1.Inspect(Para.CaliX1);            
            hWndCtrl1.DrawRect(myRes,camera1.myImage);

            //HOperatorSet.WriteImage(myRes.InspectedImage, "bmp", 0, "D:\\1.bmp");
            //HImage image = new HImage("D:\\1.bmp");
            //hWndCtrlRes1.addIconicVar(image);
            //UpdateMod1TestResult(0);
            //hWndCtrlRes1.addIconicVar(myRes.InspectedImage);
            //hWndCtrlRes1.resetWindow();
            //hWndCtrlRes1.repaint();
        }

        private void HomeTP_Enter(object sender, EventArgs e)
        {
            HalconWin1.Parent = imgPnl1;
            statusStrip1.Parent = groupBox7;
            HalconWin2.Parent = imgPnl2;
            statusStrip2.Parent = groupBox6;
        }

        private void CamTP_Enter(object sender, EventArgs e)
        {
            HalconWin1.Parent = Cam1Pnl;
            statusStrip1.Parent = Cam1Pnl;
            HalconWin2.Parent = Cam2Pnl;
            statusStrip2.Parent = Cam2Pnl;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (camera1.IsCameraOpen())
            {
                camera1.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl.Text = "Idle...";
                camStatusLbl.BackColor = Color.Orange;
            }

            camera1.Grab();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            if (camera2.IsCameraLive())
                camera2.StopCamera();
            Thread.Sleep(200);
            //camera1.Grab();
            hWndCtrl2.ClearResult();
            JPTCG.Vision.HalconInspection.RectData myRes = camera2.Inspect(Para.CaliX2);
            hWndCtrl2.DrawRect(myRes,camera2.myImage);
        }

        private void CBEndLot_CheckedChanged(object sender, EventArgs e)
        {
            //Para.EndLot = CBEndLot.Checked;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int cnt = int.Parse(textBox1.Text);
            List<DPoint> myRL = new List<DPoint>();

            for (int i = 0; i <= cnt; i++)
            {
                hWndCtrl1.ClearResult();
                camera1.Grab();

                JPTCG.Vision.HalconInspection.RectData myRes = camera1.Inspect(Para.CaliX1);
                hWndCtrl1.DrawRect(myRes,camera1.myImage);
                Application.DoEvents();
                myRL.Add(new DPoint(myRes.X, myRes.Y));
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Execl files (*.csv)|*.csv";
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;
            dlg.CreatePrompt = true;
            dlg.Title = "保存为cvs文件";

            dlg.ShowDialog();
            

            //string path = "D:\\生产数据\\DataInfo";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            string FileName = dlg.FileName;//path + "\\" + s_FileName + ".csv";

            FileStream objFileStream;
            StreamWriter objStreamWriter;
            string strLine = "";
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
            string columnTitle;
            try
            {

                if (bCreatedNew)
                {
                    columnTitle = "Count,PixelX,PixelY";
                    sw.WriteLine(columnTitle);                    
                }

                for (int i = 0; i <= cnt; i++)
                {
                    columnTitle = i.ToString() + "," + myRL[i].X.ToString("F3") + "," + myRL[i].Y.ToString("F3") + ",";                   
                    sw.WriteLine(columnTitle);
                }
                sw.Close();
                objFileStream.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(e.ToString());
            }
            finally
            {
                sw.Close();
                objFileStream.Close();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Para.ContTestRunData = checkBox2.Checked;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (camera2.IsCameraOpen())
            {
                camera2.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl2.Text = "Idle...";
                camStatusLbl2.BackColor = Color.Orange;
            }
            camera2.Grab();
        }

        private void Cam1SaveImgCB_CheckedChanged(object sender, EventArgs e)
        {
            camera1.bSaveImage = Cam1SaveImgCB.Checked;
        }

        private void Cam2SaveImgCB_CheckedChanged(object sender, EventArgs e)
        {
            camera2.bSaveImage = Cam2SaveImgCB.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Para.EndLot = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Para.EnableCGHost = !checkBox4.Checked;
        }

        private void CamToOriginOffsetAngEB_Leave(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].AngleOffset = float.Parse(CamToOriginOffsetAngEB.Text);
        }

        private void SampleShapeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Para.SampleShape = SampleShapeCB.SelectedIndex;
            if (Para.Module.Count == 0)
                return;
            for (int m = 0; m < Para.ModCount; m++)
            {
                if (Para.SampleShape == 0)
                {
                    Para.Module[m].TestPt[0].X = -3.0;
                    Para.Module[m].TestPt[0].Y = 0;
                    Para.Module[m].TestPt[1].X = -1.5;
                    Para.Module[m].TestPt[1].Y = 0;
                    Para.Module[m].TestPt[2].X = 0.0;
                    Para.Module[m].TestPt[2].Y = 0;

                    Para.Module[m].TestPt[3].X = 1.5;
                    Para.Module[m].TestPt[3].Y = 0;
                    Para.Module[m].TestPt[4].X = 3.0;
                    Para.Module[m].TestPt[4].Y = 0;

                    hWinCntrlResCam1.Width = 150;
                    hWinCntrlResCam1.Height = 30;
                    hWinCntrlResCam2.Width = 150;
                    hWinCntrlResCam2.Height = 30;

                }
                else
                {
                    Para.Module[m].TestPt[0].X = 0;
                    Para.Module[m].TestPt[0].Y = 0;
                    Para.Module[m].TestPt[1].X = 0;
                    Para.Module[m].TestPt[1].Y = -0.444;
                    Para.Module[m].TestPt[2].X = 0.384;
                    Para.Module[m].TestPt[2].Y = 0.222;

                    Para.Module[m].TestPt[3].X = -0.384;
                    Para.Module[m].TestPt[3].Y = 0.222;
                    Para.Module[m].TestPt[4].X = 0;
                    Para.Module[m].TestPt[4].Y = 0;

                    hWinCntrlResCam1.Width = 150;
                    hWinCntrlResCam1.Height = 150;
                    hWinCntrlResCam2.Width = 150;
                    hWinCntrlResCam2.Height = 150;
                }
            }
            UpdateModuleUI(ModSelCB.SelectedIndex);
        }

        private void BtnTimer_Tick(object sender, EventArgs e)
        {

        }

        private void EnbTestPt1CB_CheckedChanged(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].TestPtEnb[0] = EnbTestPt1CB.Checked;
        }

        private void EnbTestPt2CB_CheckedChanged(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].TestPtEnb[1] = EnbTestPt2CB.Checked;
        }

        private void EnbTestPt3CB_CheckedChanged(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].TestPtEnb[2] = EnbTestPt3CB.Checked;
        }

        private void EnbTestPt4CB_CheckedChanged(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].TestPtEnb[3] = EnbTestPt4CB.Checked;
        }

        private void EnbTestPt5CB_CheckedChanged(object sender, EventArgs e)
        {
            int ModIdx = ModSelCB.SelectedIndex;
            Para.Module[ModIdx].TestPtEnb[4] = EnbTestPt5CB.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Para.DisableBarcode = checkBox5.Checked;
        }

        private void btnALLaxisgoHome_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Please Clear All Samples.", "Homing", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnALLaxisgoHome.Enabled = false;
            SeqMgr.StopAuto();
            UIStopClicked();

            motionMgr.ResetIO();
            Thread.Sleep(200);
            SeqMgr.StartHoming();
            SeqMgr.ResetAll();
            btnALLaxisgoHome.Enabled = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Para.TotalTestUnit = 0;
            Para.Mod1FailUnitCnt = 0;
            Para.Mod2FailUnitCnt = 0;

            UpdateTotalCountUI();
        }

        public void UpdateTotalCountUI()
        {
            Action ac = new Action(() =>
            {
                TotalCntEB.Text = Para.TotalTestUnit.ToString();
                Mod1FailCntEB.Text = Para.Mod1FailUnitCnt.ToString();
                Mod2FailCntEB.Text = Para.Mod2FailUnitCnt.ToString();
                TotalFailCntEB.Text = (Para.Mod1FailUnitCnt + Para.Mod2FailUnitCnt).ToString();
            });
            TotalCntEB.BeginInvoke(ac);
        }

        public void UpdateStatusLbl(Color myColor)
        {
            Action ac = new Action(() =>
            {
                StatusLbl.BackColor = myColor;
            });
            StatusLbl.BeginInvoke(ac);            
        }

        private void CBEngMode_CheckedChanged(object sender, EventArgs e)
        {
            Para.EngineerMode = CBEngMode.Checked;
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please Clear All Samples.", "Homing", MessageBoxButtons.OK, MessageBoxIcon.Information);

            HomeBtn.Enabled = false;
            labelStart.Enabled = false;
            labelStop.Enabled = false;
            labelPause.Enabled = false;

            SeqMgr.StopAuto();
            //UIStopClicked();

            motionMgr.ResetIO();
            Thread.Sleep(200);
            SeqMgr.StartHoming();
            Para.isRotaryError = false;
            Para.EndLot = false;
            SeqMgr.ResetAll();
            HomeBtn.Enabled = true;            
            UIStopClicked();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Para.selected1BorW = Cam1cbBox.SelectedIndex;
            int TempInt = 0;
            int Div35Int = 0;
            if (int.TryParse(Cam1ExpTimeEB.Text, out TempInt))
            {
                Div35Int = TempInt / 35;
                if (Div35Int == 0)
                    Div35Int = 1;
                TempInt = Div35Int * 35;
                Cam1ExpTimeEB.Text = TempInt.ToString();
                if (Para.selected1BorW == 0)//20161018
                {
                    Para.Cam1ExposureTimeB = TempInt;
                    camera1.SetExposure(Para.Cam1ExposureTimeB);
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera1B", Para.Cam1ExposureTimeB.ToString());
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Selected1BorW", "0");
                }
                else if (Para.selected1BorW == 1)
                {
                    Para.Cam1ExposureTimeW = TempInt;
                    camera1.SetExposure(Para.Cam1ExposureTimeW);
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera1W", Para.Cam1ExposureTimeW.ToString());
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Selected1BorW", "1");
                }
            }
            else
            {
                MessageBox.Show("Input Value is wrong, Please input again !");
            }
            //SaveSettings(Para.CurLoadConfigFileName);//20161018
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Para.selected2BorW = Cam2cbBox.SelectedIndex;
            int TempInt = 0;
            int Div35Int = 0;
            if (int.TryParse(Cam2ExpTimeEB.Text, out TempInt))
            {
                Div35Int = TempInt / 35;
                if (Div35Int == 0)
                    Div35Int = 1;
                TempInt = Div35Int * 35;
                Cam2ExpTimeEB.Text = TempInt.ToString();
                if (Para.selected2BorW == 0)//20161018
                {
                    Para.Cam2ExposureTimeB = TempInt;
                    camera2.SetExposure(Para.Cam2ExposureTimeB);
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera2B", Para.Cam2ExposureTimeB.ToString());
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Selected2BorW", "0");
                }
                else if (Para.selected2BorW == 1)
                {
                    Para.Cam2ExposureTimeW = TempInt;
                    camera2.SetExposure(Para.Cam2ExposureTimeW);
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera2W", Para.Cam2ExposureTimeW.ToString());
                    FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Selected2BorW", "1");
                }
            }
            else
            {
                MessageBox.Show("Input Value is wrong, Please input again !");
            }
            //SaveSettings(Para.CurLoadConfigFileName);//20161018
        }


        private void CamTP_Leave(object sender, EventArgs e)
        {
            TBStopCam_Click(sender, e);
            toolStripButton7_Click(sender, e);
        }

        private void safetyTimer_Tick(object sender, EventArgs e)
        {
            //if (!motionMgr.ReadIOIn((ushort)InputIOlist.BtnEMO))
            //{
            //    SeqMgr.StopAuto();
            //    UIStopClicked();
            //    MessageBox.Show("EMO Button Pressed!", "Safety Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Para.isRotaryError = true;
            //    Application.DoEvents();
            //    motionMgr.WriteIOOut((ushort)OutputIOlist.RotaryEnabled, false);
            //    OnlyHomeEnb();
            //}
            //if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.DoorSensor))
            //    res = -6;
            checkBox3.Checked = Para.EndLot;//20161101
            if (motionMgr.ReadIOIn((ushort)InputIOlist.BtnStop))
            {
                SeqMgr.StopAuto();
                UIStopClicked();
                //MessageBox.Show("Stop Button Pressed!", "Safety Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }

            //
            TimeSpan tp = DateTime.Now - Para.SystemRunTime;
            if (tp.TotalHours >= 24)
            {
                DailyCheckTime.Text = "0 h";
                DailyCheckTime.BackColor = Color.Red;
            }
            if ((tp.TotalHours < 24) && (tp.TotalMinutes >= 20))
            {
                DailyCheckTime.Text = (24 - tp.TotalHours).ToString("F1") + " h";
                DailyCheckTime.BackColor = Color.Yellow;
            }
            if ((tp.TotalMinutes < 20) && (tp.TotalMinutes >= 0))
            {
                DailyCheckTime.Text = (24 - tp.TotalHours).ToString("F1") + " h";
                DailyCheckTime.BackColor = Color.LightGreen;
            }
            //
            //
             tp = DateTime.Now - Para.NDSystemRunTime;
            if (tp.TotalHours >= Para.NDTime*24)
            {
                NDRemain.Text = "0 h";
                NDRemain.BackColor = Color.Red;
            }
            if ((tp.TotalHours < Para.NDTime * 24) && (tp.TotalHours >= (Para.NDTime-1) * 24))
            {
                
                //NDRemain.Text = (24 * Para.NDTime - tp.TotalHours).ToString("F1") + " h";
                NDRemain.Text = Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24).ToString() + " day " + ((24 * Para.NDTime - tp.TotalHours) - Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24) * 24).ToString("F1")+" h ";
                NDRemain.BackColor = Color.Yellow;
            }
            if ((tp.TotalHours < (Para.NDTime-1) * 24) && (tp.TotalHours >= 0))
            {
                NDRemain.Text = Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24).ToString() + " day " + ((24 * Para.NDTime - tp.TotalHours) - Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24) * 24).ToString("F1") + " h ";
                NDRemain.BackColor = Color.LightGreen;
            }
            //

            tp = DateTime.Now - Para.LSSystemRunTime;
            if (tp.TotalHours >= Para.NDTime * 24)
            {
                LSRemain.Text = "0 h";
                LSRemain.BackColor = Color.Red;
            }
            if ((tp.TotalHours < Para.NDTime * 24) && (tp.TotalHours >= (Para.NDTime-1) * 24))
            {
                LSRemain.Text = Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24).ToString() + " day " + ((24 * Para.NDTime - tp.TotalHours) - Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24) * 24).ToString("F1") + " h ";
                LSRemain.BackColor = Color.Yellow;
            }
            if ((tp.TotalHours < (Para.NDTime-1) * 24) && (tp.TotalHours >= 0))
            {
                LSRemain.Text = Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24).ToString() + " day " + ((24 * Para.NDTime - tp.TotalHours) - Math.Floor((24 * Para.NDTime - tp.TotalHours) / 24) * 24).ToString("F1") + " h ";
                LSRemain.BackColor = Color.LightGreen;
            }

            //
            tp = DateTime.Now - Para.SWRunTime;
            LSAge.Text = (Para.LSUsedTime + tp.TotalHours).ToString("F1");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            CaliForm myWin = new CaliForm(specMgr, motionMgr);
            myWin.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int access = uLogin.GetAccess(userCB.SelectedItem.ToString(), textBoxPassword.Text);
            SetUIAccess(access);
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                int access = uLogin.GetAccess(userCB.SelectedItem.ToString(), textBoxPassword.Text);
                SetUIAccess(access);
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            SetUIAccess(0);
        }

        private void Cam1cbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //20161018            
            Para.selected1BorW = Cam1cbBox.SelectedIndex;
            if (Para.selected1BorW == 0)
            {

                Cam1ExpTimeEB.Text = Para.Cam1ExposureTimeB.ToString();
            }
            else if (Para.selected1BorW == 1)
            {
                Cam1ExpTimeEB.Text = Para.Cam1ExposureTimeW.ToString();
            }
        }

        private void Cam2cbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //20161018
            
            Para.selected2BorW = Cam2cbBox.SelectedIndex;
            if (Para.selected2BorW == 0)
            {
                
                Cam2ExpTimeEB.Text = Para.Cam2ExposureTimeB.ToString();
            }
            else if (Para.selected2BorW == 1)
            {

                Cam2ExpTimeEB.Text = Para.Cam2ExposureTimeW.ToString();
            }
        }

        private void Cam1ckB_CheckedChanged(object sender, EventArgs e)
        {
            //
            Para.disableAutoExpTime1 = Cam1ckB.Checked;
            FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "disableAutoExpTime1", (Para.disableAutoExpTime1).ToString());
        }

        private void Cam2ckB_CheckedChanged(object sender, EventArgs e)
        {
            //
            Para.disableAutoExpTime2 = Cam2ckB.Checked;
            FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "disableAutoExpTime2", (Para.disableAutoExpTime2).ToString());
        }

        private void button18_Click(object sender, EventArgs e)
        {
            int TempInt = 0;
            int Div35Int = 0;
            if (int.TryParse(Cam1ExpTime1EB.Text, out TempInt))
            {
                Div35Int = TempInt / 35;
                if (Div35Int == 0)
                    Div35Int = 1;
                TempInt = Div35Int * 35;
                Cam1ExpTime1EB.Text = TempInt.ToString();
                Para.Cam1ExposureTime1 = TempInt;
                camera1.SetExposure(Para.Cam1ExposureTime1);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera1Exp1", Para.Cam1ExposureTime1.ToString());//20161028
            }
            else
            {
                MessageBox.Show("Input Value is wrong, Please input again !");
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int TempInt = 0;
            int Div35Int = 0;
            if (int.TryParse(Cam1ExpTime3EB.Text, out TempInt))
            {
                Div35Int = TempInt / 35;
                if (Div35Int == 0)
                    Div35Int = 1;
                TempInt = Div35Int * 35;
                Cam1ExpTime3EB.Text = TempInt.ToString();
                Para.Cam1ExposureTime3 = TempInt;
                camera1.SetExposure(Para.Cam1ExposureTime3);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera1Exp3", Para.Cam1ExposureTime3.ToString());//20161028
            }
            else
            {
                MessageBox.Show("Input Value is wrong, Please input again !");
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            int TempInt = 0;
            int Div35Int = 0;
            if (int.TryParse(Cam2ExpTime1EB.Text, out TempInt))
            {
                Div35Int = TempInt / 35;
                if (Div35Int == 0)
                    Div35Int = 1;
                TempInt = Div35Int * 35;
                Cam2ExpTime1EB.Text = TempInt.ToString();
                Para.Cam2ExposureTime1 = TempInt;
                camera2.SetExposure(Para.Cam2ExposureTime1);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera2Exp1", Para.Cam2ExposureTime1.ToString());//20161028 
            }
            else
            {
                MessageBox.Show("Input Value is wrong, Please input again !");
            }                    
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int TempInt = 0;
            int Div35Int = 0;
            if (int.TryParse(Cam2ExpTime3EB.Text, out TempInt))
            {
                Div35Int = TempInt / 35;
                if (Div35Int == 0)
                    Div35Int = 1;
                TempInt = Div35Int * 35;
                Cam2ExpTime3EB.Text = TempInt.ToString();
                Para.Cam2ExposureTime3 = TempInt;
                camera2.SetExposure(Para.Cam2ExposureTime3);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "ExposureTime", "Camera2Exp3", Para.Cam2ExposureTime3.ToString());//20161028
            }
            else
            {
                MessageBox.Show("Input Value is wrong, Please input again !");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (camera1.IsCameraOpen())
            {
                camera1.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl.Text = "Idle...";
                camStatusLbl.BackColor = Color.Orange;
            }
            Thread.Sleep(200);
            JPTCG.Vision.HalconInspection.GreyValueData myData = camera1.GetGreyValue();
            Cam1GVLB.Items.Clear();
            Cam1GVLB.Items.Add("Min : " + myData.min.ToString());
            Cam1GVLB.Items.Add("Max : " + myData.max.ToString());
            Cam1GVLB.Items.Add("Mean : " + myData.mean.ToString("F1"));
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (camera2.IsCameraOpen())
            {
                camera2.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl2.Text = "Idle...";
                camStatusLbl2.BackColor = Color.Orange;
            }
            Thread.Sleep(200);
            JPTCG.Vision.HalconInspection.GreyValueData myData = camera2.GetGreyValue();
            Cam2GVLB.Items.Clear();
            Cam2GVLB.Items.Add("Min : " + myData.min.ToString());
            Cam2GVLB.Items.Add("Max : " + myData.max.ToString());
            Cam2GVLB.Items.Add("Mean : " + myData.mean.ToString("F1"));
        }

        private void SetThreshold_Click(object sender, EventArgs e)
        {
            Para.Threshold = int.Parse(Threshold.Text);
        }

        private void label42_Click(object sender, EventArgs e)
        {
            KeyenceDLRS_Settings myWin = new KeyenceDLRS_Settings();
            myWin.ShowDialog();
        }

        private unsafe void button24_Click(object sender, EventArgs e)
        {           
            HTuple pointer, type, width, height; 
            HOperatorSet.GetImagePointer1(camera1.myImage, out pointer, out type, out width, out height);
            byte* p = (byte*)pointer[0].L;
           
            int ImgWidth = width.I;
            int ImgHgt = height.I;
            int size = ImgWidth * ImgHgt;
            Byte[] Exp1Dark = new Byte[size];
            

            Marshal.Copy((IntPtr)p, Exp1Dark, 0, size);           

        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            //Image returnImage;
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                return Image.FromStream(ms, true);//Exception occurs here
            }
            catch { }
            return null;
        }

        private void PicPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tempIndex = PicPath.SelectedIndex;
            switch (tempIndex)
            {
                case 0:
                    PicPath.Text = "C";
                    Para.PicPath = "C";
                    break;
                case 1:
                    PicPath.Text = "D";
                    Para.PicPath = "D";
                    break;
                case 2:
                    PicPath.Text = "E";
                    Para.PicPath = "E";
                    break;
                default:
                    PicPath.Text = "F";
                    Para.PicPath = "F";
                    break;
            }
            FileOperation.SaveData(Para.CurLoadConfigFileName, "PicturePath", "PicPath", Para.PicPath);
        }

        private void FindMod1Center_Click(object sender, EventArgs e)
        {
            //
            double XLeft = 0;
            double XRight = 0;
            double XCenter = 0;
            double YUp = 0;
            double YDown = 0;
            double YCenter = 0;
            List<float> RefValue = new List<float>();
            List<float> TempRefValue = new List<float>();
            List<float> WLValue = new List<float>();
            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[2].X);
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[2].Y);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
            Thread.Sleep(500);
            RefValue = specMgr.GetCount(0);
            WLValue = specMgr.GetWaveLength(0);
            UpdateMod1Chart(WLValue, RefValue);
            //Move X
            for (int i = 0; i < 40; i++)
            {
                motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[2].X + (i + 1) * 0.025);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
                TempRefValue = specMgr.GetCount(0);
                UpdateMod1Chart(WLValue, TempRefValue);
                if (TempRefValue[100] / RefValue[100] < 0.9)
                {
                    XLeft = Para.Module[0].TeachPos[2].X + (i + 1) * 0.025;
                    break;
                }             
            }
            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[2].X);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);

            for (int i = 0; i < 40; i++)
            {
                motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[2].X - (i + 1) * 0.025);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
                TempRefValue = specMgr.GetCount(0);
                UpdateMod1Chart(WLValue, TempRefValue);
                if (TempRefValue[100] / RefValue[100] < 0.9)
                {
                    XRight = Para.Module[0].TeachPos[2].X - (i + 1) * 0.025;
                    break;
                }
            }
            XCenter = (XLeft + XRight) / 2;
            MessageBox.Show("XCenter is :" + XCenter.ToString());

            motionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[2].X);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);

            //Move Y
            for (int i = 0; i < 40; i++)
            {
                motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[2].Y + (i + 1) * 0.025);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
                TempRefValue = specMgr.GetCount(0);
                UpdateMod1Chart(WLValue, TempRefValue);
                if (TempRefValue[100] / RefValue[100] < 0.9)
                {
                    YUp = Para.Module[0].TeachPos[2].Y + (i + 1) * 0.025;
                    break;
                }
            }
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[2].Y);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);

            for (int i = 0; i < 40; i++)
            {
                motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[2].Y - (i + 1) * 0.025);
                motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
                TempRefValue = specMgr.GetCount(0);
                UpdateMod1Chart(WLValue, TempRefValue);
                if (TempRefValue[100] / RefValue[100] < 0.9)
                {
                    YDown = Para.Module[0].TeachPos[2].Y - (i + 1) * 0.025;
                    break;
                }
            }
            YCenter = (YUp + YDown) / 2;
            MessageBox.Show("YCenter is :" + YCenter.ToString());
            motionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[2].Y);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
        }

        private void FindMod2Center_Click(object sender, EventArgs e)
        {
            //
            motionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[2].X);
            motionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[2].Y);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            motionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
        }

        private void Save3ParaBtn1_Click(object sender, EventArgs e)
        {
            try
            {
                //
                FileOperation.SaveData(Para.CurLoadConfigFileName, "Add3ParameterForImage", "CentroidWL1", CenWL1textBox.Text);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "Add3ParameterForImage", "PixelDensity1", PixDen1textBox.Text);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "Add3ParameterForImage", "BeamSize1", BeamSize1textBox.Text);
                Para.CenWL1 = float.Parse(CenWL1textBox.Text);
                Para.PixDen1 = float.Parse(PixDen1textBox.Text);
                Para.BeamSize1 = float.Parse(BeamSize1textBox.Text);
            }
            catch
            {
                MessageBox.Show("输入数据有误，请重新输入！");
            }
        }

        private void Save3ParaBtn2_Click(object sender, EventArgs e)
        {
            try
            {
                //
                FileOperation.SaveData(Para.CurLoadConfigFileName, "Add3ParameterForImage", "CentroidWL2", CenWL2textBox.Text);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "Add3ParameterForImage", "PixelDensity2", PixDen2textBox.Text);
                FileOperation.SaveData(Para.CurLoadConfigFileName, "Add3ParameterForImage", "BeamSize2", BeamSize2textBox.Text);
                Para.CenWL2 = float.Parse(CenWL2textBox.Text);
                Para.PixDen2 = float.Parse(PixDen2textBox.Text);
                Para.BeamSize2 = float.Parse(BeamSize2textBox.Text);
            }
            catch
            {
                MessageBox.Show("输入数据有误，请重新输入！");
            }
        }

        private void buttonAuditBox_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Please,wait a moment untill Auditbox open completely", "Auditbox", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                AuditBoxForm myWin = new AuditBoxForm(specMgr, motionMgr);
                myWin.ShowDialog();
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            PicPath.Enabled = true;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            PicPath.Enabled = false;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Para.DisableSafeDoor = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Para.DisableUpTriggerMode = checkBox7.Checked;
            Para.BothBtnAreLow = false;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (camera1.IsCameraOpen())
            {
                hWndCtrl1.ClearResult();
                hWndCtrl1.setViewState(HWndCtrl.MODE_VIEW_NONE);
                hWndCtrl1.resetWindow();
                hWndCtrl1.repaint();
                camera1.LiveCamera();
                //EnbToolBtn(false);
                camStatusLbl.Text = "Live...";
                camStatusLbl.BackColor = Color.Lime;
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (camera1.IsCameraOpen())
            {
                camera1.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl.Text = "Idle...";
                camStatusLbl.BackColor = Color.Orange;
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (camera2.IsCameraOpen())
            {
                hWndCtrl2.ClearResult();
                hWndCtrl2.setViewState(HWndCtrl.MODE_VIEW_NONE);
                hWndCtrl2.resetWindow();
                hWndCtrl2.repaint();
                camera2.LiveCamera();
                //EnbToolBtn(false);
                camStatusLbl2.Text = "Live...";
                camStatusLbl2.BackColor = Color.Lime;
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (camera2.IsCameraOpen())
            {
                camera2.StopCamera();
                //EnbToolBtn(true);
                camStatusLbl2.Text = "Idle...";
                camStatusLbl2.BackColor = Color.Orange;
            }
        }

        public void UpdateMod1Height(string height)
        {
            Action ac = new Action(() =>
            {
                mod1_Height_Control.Text = height;
            });
            mod1_Height_Control.BeginInvoke(ac);
        }

        public void UpdateMod2Height(string height)
        {
            Action ac = new Action(() =>
            {
                mod2_Height_Control.Text = height;
            });
            mod2_Height_Control.BeginInvoke(ac);
        }
    }
}
