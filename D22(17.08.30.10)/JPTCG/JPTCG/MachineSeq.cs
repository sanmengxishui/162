using JPTCG.AppleTestSW;
using JPTCG.BarcodeScanner;
using JPTCG.Common;
using JPTCG.Motion;
using JPTCG.Sequencing;
using JPTCG.Spectrometer;
using JPTCG.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using HalconDotNet;
using System.Runtime.InteropServices;
using System.Collections;

namespace JPTCG
{
    public class StationModule    
    {
        public bool isUnitLoad1 = false;
        public bool isUnitLoad2 = false;

        public string Unit1Barcode = "";
        public string Unit2Barcode = "";

        public HObject CCD1LightFalse = new HObject();
        public HObject CCD1LightTrue = new HObject();
        public HObject inspCCD1LightFalse = new HObject();
        public HObject inspCCD1LightTrue = new HObject();

        public HObject CCD1LightFalseRotate = new HObject();
        public HObject CCD1LightTrueRotate = new HObject();
        public HObject inspCCD1LightFalseRotate = new HObject();
        public HObject inspCCD1LightTrueRotate = new HObject();

        public HObject CCD2LightFalse = new HObject();
        public HObject CCD2LightTrue = new HObject();
        public HObject inspCCD2LightFalse = new HObject();
        public HObject inspCCD2LightTrue = new HObject();

        public HObject CCD2LightFalseRotate = new HObject();
        public HObject CCD2LightTrueRotate = new HObject();
        public HObject inspCCD2LightFalseRotate = new HObject();
        public HObject inspCCD2LightTrueRotate = new HObject();

        public HObject cutRectangle = new HObject();
        public HObject temtRectangle = new HObject();

        public JPTCG.Vision.HalconInspection.RectData mod1VisResult = new HalconInspection.RectData();
        public JPTCG.Vision.HalconInspection.RectData mod2VisResult = new HalconInspection.RectData();

        public List<float> DarkRefMod1 = new List<float>();
        public List<float> WhiteRefMod1 = new List<float>();
        public List<float> WLMod1Dark = new List<float>();
        //public List<float> WLMod1White = new List<float>();

        //public List<float>[] MeasWLMod1 = new List<float>[Para._Max_Test_Point];
        public List<float> SelectCount1 = new List<float>(); //Per point 32.
        public List<float> SelectCount1_Avg = new List<float>(); //Per point 32.
        public List<float> MeasureRatio1_Avg = new List<float>(); //Per point 32.
        public List<float>[] MeasDataMod1 = new List<float>[Para._Max_Test_Point];
        public List<float>[] transRatioMod1 = new List<float>[Para._Max_Test_Point];

        public List<float> DarkRefMod2 = new List<float>();
        public List<float> WhiteRefMod2 = new List<float>();
        public List<float> WLMod2Dark = new List<float>();
       // public List<float> WLMod2White = new List<float>();
        //public List<float>[] MeasWLMod2 = new List<float>[Para._Max_Test_Point];
        public List<float> SelectCount2 = new List<float>(); //Per point 32.
        public List<float> SelectCount2_Avg = new List<float>(); //Per point 32.
        public List<float> MeasureRatio2_Avg = new List<float>(); //Per point 32.



        public List<float>[] MeasDataMod2 = new List<float>[Para._Max_Test_Point];
        public List<float>[] transRatioMod2 = new List<float>[Para._Max_Test_Point];

        //Exposure Time 2
        public Byte[] Mod1DarkImage;
        public Byte[] Mod1Image;
        public Byte[] Mod2Image;
        public Byte[] Mod2DarkImage;        

        public bool IsMod1UnitPassed = false;
        public bool IsMod2UnitPassed = false;
        //public List<double> transRatio = new List<double>();
        //public 

        public double Unit1CGHeight = 0;
        public double Unit2CGHeight = 0;

        //P2Test 45 deg
        public Byte[] Mod1Exp1DarkImage;
        public Byte[] Mod1Exp1Image;
        public Byte[] Mod2Exp1DarkImage;
        public Byte[] Mod2Exp1Image;
    }

    public class MachineSeq
    {
        private List<FunctionObjects> camSeqList = new List<FunctionObjects>();
        private List<FunctionObjects> testSeqList = new List<FunctionObjects>();
        private List<FunctionObjects> testMod1List = new List<FunctionObjects>();
        private List<FunctionObjects> testMod2List = new List<FunctionObjects>();
        private List<FunctionObjects> mainSeqList = new List<FunctionObjects>();
        private List<FunctionObjects> safetySeqList = new List<FunctionObjects>();

        DeltaMotionMgr myMotionMgr;
        HalconVision Cam1, Cam2;
        SpectManager specMgr;
        BarcodeMgr barcodeMgr;
        RotaryMotion rotMgr;
        AppleTestSWCom AComMod1, AComMod2;

        Thread HomingThread;       
        mainWin mainWnd;
        ListView mainLV, camLV, testLV;

        WorkerThread mainSeq , camSeq, testSeq , testMod1Seq, testMod2Seq;
        WorkerThread cam1InspSeq, cam2InspSeq, cam1CCDSeq, cam2CCDSeq;
        WorkerThread safetySeq, UnloadingSeq,TestCameraSeq, TestCamera2Seq;

        public bool isHomeDone = false;
        bool isInspectionReady, isTestStationReady, isCamStationRotaryIndexDone, isTestStationRotaryIndexDone;
        bool isCam1CCDIndexDone, isCam1CCDReady, isCam2CCDIndexDone, isCam2CCDReady;
        bool isCam1InspIndexDone, isCam1InspReady, isCam2InspIndexDone, isCam2InspReady;

        bool isUnloadStationRotaryIndexDone, UnloadStationUnloadDone;
        bool isTestCameraDone, isTestCamera2Done;

        bool mod1StartTest, mod2StartTest, mod1TestEnd, mod2TestEnd;

        EngModeCCDWin Cam1Win = new EngModeCCDWin("Camera 1");
        EngModeCCDWin Cam2Win = new EngModeCCDWin("Camera 2");

        const int _Num_Of_Station = 5; //index 0 not Used
        public StationModule[] stationStatus = new StationModule[_Num_Of_Station];
        log logg = new JPTCG.log();

        //int CurRotIdx = 1;
        public MachineSeq(mainWin myMain, DeltaMotionMgr myMotion,RotaryMotion myRotMotion, SpectManager mySpecMgr ,BarcodeMgr myBarcode, HalconVision myCam1, HalconVision myCam2,
                        AppleTestSWCom myAComMod1, AppleTestSWCom myAComMod2)
        {
            for (int i = 0; i < _Num_Of_Station; i++)
            {
                stationStatus[i] = new StationModule();
                for (int p = 0; p < Para.TestPtCnt; p++)
                {
                    stationStatus[i].transRatioMod1[p] = new List<float>();
                    stationStatus[i].transRatioMod2[p] = new List<float>();
                }
            }
            myMotionMgr = myMotion;
            specMgr = mySpecMgr;
            Cam1 = myCam1;
            Cam2 = myCam2;
            barcodeMgr = myBarcode;
            rotMgr = myRotMotion;
            mainWnd = myMain;
            AComMod1 = myAComMod1;
            AComMod2 = myAComMod2;
            //
         
            safetySeq = new WorkerThread("SafetySeqThread", mainWnd);
            safetySeq.OnThreadStoppedOnError += OnThreadStoppedError;

            mainSeq = new WorkerThread("MainSeqThread", mainWnd);
            mainSeq.OnFuncStart += OnFunctionStartMain;
            mainSeq.OnFuncComplete += OnFunctionCompleteMain;
            //mainSeq.OnThreadStarted += OnThreadStarted;
            mainSeq.OnThreadStoppedOnError += OnThreadStoppedError;
            camSeq = new WorkerThread("CameraSeqThread", mainWnd);
            camSeq.OnFuncStart += OnFunctionStartCam;
            camSeq.OnFuncComplete += OnFunctionCompleteCam;
           // camSeq.OnThreadStarted += OnThreadStarted;
            camSeq.OnThreadStoppedOnError += OnThreadStoppedError;
            testSeq = new WorkerThread("TestSeqThread", mainWnd);   
            testSeq.OnFuncStart += OnFunctionStartTest;
            testSeq.OnFuncComplete += OnFunctionCompleteTest;
            //testSeq.OnThreadStarted += OnThreadStarted;
            testSeq.OnThreadStoppedOnError += OnThreadStoppedError;

            testMod1Seq = new WorkerThread("TestMod1SeqThread", mainWnd);
            //testMod1Seq.OnThreadStarted += OnThreadStarted;
            testMod1Seq.OnThreadStoppedOnError += OnThreadStoppedError;

            testMod2Seq = new WorkerThread("TestMod2SeqThread", mainWnd);
            //testMod2Seq.OnThreadStarted += OnThreadStarted;
            testMod2Seq.OnThreadStoppedOnError += OnThreadStoppedError;
            //Clovis
            cam1InspSeq = new WorkerThread("Cam1InspSeqThread", mainWnd);
            cam1InspSeq.OnThreadStoppedOnError += OnThreadStoppedError;
            cam2InspSeq = new WorkerThread("Cam2InspSeqThread", mainWnd);
            cam2InspSeq.OnThreadStoppedOnError += OnThreadStoppedError;
            cam1CCDSeq = new WorkerThread("Cam1CCDSeqThread", mainWnd);
            cam1CCDSeq.OnThreadStoppedOnError += OnThreadStoppedError;
            cam2CCDSeq = new WorkerThread("Cam2CCDSeqThread", mainWnd);
            cam2CCDSeq.OnThreadStoppedOnError += OnThreadStoppedError;

            UnloadingSeq = new WorkerThread("Mod4UnloadingSeqThread", mainWnd);
            UnloadingSeq.OnThreadStoppedOnError += OnThreadStoppedError;

            //TestCamera
            TestCameraSeq = new WorkerThread("TestCameraSeqThread", mainWnd);
            TestCameraSeq.OnThreadStoppedOnError += OnThreadStoppedError;

            //TestCamera2
            TestCamera2Seq = new WorkerThread("TestCamera2SeqThread", mainWnd);
            TestCamera2Seq.OnThreadStoppedOnError += OnThreadStoppedError;

            InitSeqFunction();
        }
        ~MachineSeq()
        {
            
        }
        #region Sequence Debug UI
        private void OnFunctionStartMain(int Idx)
        {
            if (mainLV == null)
                return;
            mainLV.Invoke(
                        new Action(() =>
                        {
                            UpdateMainLV(Idx, 1);
                        })
                        );  
        }
        private void OnFunctionCompleteMain(int Idx, bool isError)
        {
            if (mainLV == null)
                return;
            mainLV.Invoke(
                        new Action(() =>
                        {
                            if (isError)
                                UpdateMainLV(Idx, -1);
                            else
                                UpdateMainLV(Idx, 0);
                        })
                        );
        }
        private void OnFunctionStartCam(int Idx)
        {
            if (mainLV == null)
                return;
            mainLV.Invoke(
                        new Action(() =>
                        {
                            UpdateCamLV(Idx, 1);
                        })
                        );
        }
        private void OnFunctionCompleteCam(int Idx, bool isError)
        {
            if (mainLV == null)
                return;
            mainLV.Invoke(
                        new Action(() =>
                        {
                            if (isError)
                                UpdateCamLV(Idx, -1);
                            else
                                UpdateCamLV(Idx, 0);
                        })
                        );
        }
        private void OnFunctionStartTest(int Idx)
        {
            if (mainLV == null)
                return;
            mainLV.Invoke(
                        new Action(() =>
                        {
                            UpdateTestLV(Idx, 1);
                        })
                        );
        }
        private void OnFunctionCompleteTest(int Idx, bool isError)
        {
            if (mainLV == null)
                return;
            mainLV.Invoke(
                        new Action(() =>
                        {
                            if (isError)
                                UpdateTestLV(Idx, -1);
                            else
                                UpdateTestLV(Idx, 0);
                        })
                        );
        }
        public void AssignUI(ListView myMainLV, ListView myCamLV, ListView myTestLV, bool toAssign)
        {
            if (toAssign)
            {
                if (mainLV == null)
                {
                    mainLV = myMainLV;
                }

                mainLV.Clear();
                mainLV.Columns.Add("Idx", 30);
                mainLV.Columns.Add("Name", 150);
                mainLV.Columns.Add("Status", 60);
                this.mainLV.View = System.Windows.Forms.View.Details;
                for (int i = 0; i < mainSeqList.Count; i++)
                {
                    ListViewItem item = mainLV.Items.Add(mainLV.Items.Count + "");
                    item.SubItems.Add(mainSeqList[i].Name);
                    item.SubItems.Add("Idle");
                    item.EnsureVisible();
                }

                if (camLV == null)
                {
                    camLV = myCamLV;
                }
                camLV.Clear();
                camLV.Columns.Add("Idx", 30);
                camLV.Columns.Add("Name", 150);
                camLV.Columns.Add("Status", 60);
                this.camLV.View = System.Windows.Forms.View.Details;
                for (int i = 0; i < camSeqList.Count; i++)
                {
                    ListViewItem item = camLV.Items.Add(camLV.Items.Count + "");
                    item.SubItems.Add(camSeqList[i].Name);
                    item.SubItems.Add("Idle");
                    item.EnsureVisible();
                }

                if (testLV == null)
                {
                    testLV = myTestLV;
                }

                testLV.Clear();
                testLV.Columns.Add("Idx", 30);
                testLV.Columns.Add("Name", 150);
                testLV.Columns.Add("Status", 60);
                this.testLV.View = System.Windows.Forms.View.Details;
                for (int i = 0; i < testSeqList.Count; i++)
                {
                    ListViewItem item = testLV.Items.Add(testLV.Items.Count + "");
                    item.SubItems.Add(testSeqList[i].Name);
                    item.SubItems.Add("Idle");
                    item.EnsureVisible();
                }
            }
            else
            {
                mainLV.Clear();
                mainLV = null;
                camLV.Clear();
                camLV = null;
                testLV.Clear();
                testLV = null;
            }

            
        }
        private void UpdateMainLV(int Idx, int Status)
        {
            if (mainLV == null)
                return;
            //mainLB.SelectedIndex = Idx;
            switch (Status)
            {
                case 0:// OK
                    mainLV.Items[Idx].BackColor = Color.Lime;
                    mainLV.Items[Idx].SubItems[2].Text = "OK";
                    break;
                case 1:// runing
                    mainLV.Items[Idx].BackColor = Color.Orange;
                    mainLV.Items[Idx].SubItems[2].Text = "Running";
                        break;
                case -1 : //error
                        mainLV.Items[Idx].BackColor = Color.Red;
                        mainLV.Items[Idx].SubItems[2].Text = "Error";
                    break;
            }

            //if (Status == 1)
            //{
            //    int preIdx = Idx - 1;
            //    if (preIdx < 0)
            //    {
            //        preIdx = mainLV.Items.Count - 1;
            //    }
            //    mainLV.Items[preIdx].BackColor = Color.White;
            //    mainLV.Items[preIdx].SubItems[2].Text = "Idle";
            //}
        }
        private void UpdateCamLV(int Idx, int Status)
        {
            if (camLV == null)
                return;
            //mainLB.SelectedIndex = Idx;
            switch (Status)
            {
                case 0:// OK
                    camLV.Items[Idx].BackColor = Color.Lime;
                    camLV.Items[Idx].SubItems[2].Text = "OK";
                    break;
                case 1:// runing
                    camLV.Items[Idx].BackColor = Color.Orange;
                    camLV.Items[Idx].SubItems[2].Text = "Running";
                    break;
                case -1: //error
                    camLV.Items[Idx].BackColor = Color.Red;
                    camLV.Items[Idx].SubItems[2].Text = "Error";
                    break;
            }
        }
        private void UpdateTestLV(int Idx, int Status)
        {
            if (testLV == null)
                return;
            //mainLB.SelectedIndex = Idx;
            switch (Status)
            {
                case 0:// OK
                    testLV.Items[Idx].BackColor = Color.Lime;
                    testLV.Items[Idx].SubItems[2].Text = "OK";
                    break;
                case 1:// runing
                    testLV.Items[Idx].BackColor = Color.Orange;
                    testLV.Items[Idx].SubItems[2].Text = "Running";
                    break;
                case -1: //error
                    testLV.Items[Idx].BackColor = Color.Red;
                    testLV.Items[Idx].SubItems[2].Text = "Error";
                    break;
            }

            //if (Status == 1)
            //{
            //    int preIdx = Idx - 1;
            //    if (preIdx < 0)
            //    {
            //        preIdx = testLV.Items.Count - 1;
            //    }
            //    testLV.Items[preIdx].BackColor = Color.White;
            //    testLV.Items[preIdx].SubItems[2].Text = "Idle";
            //}
        }
        #endregion

        #region Testing Functions
        private int TestFunction()
        {
            Thread.Sleep(1000);
            return 0;
        }
        private int TestErrorFunction()
        {
            Thread.Sleep(1000);
            return -1;
        }
        private int TestError(int res)
        {
            MessageBox.Show("Test Error");
            mainSeq.JumpIndex(mainSeq.CurrentIndex()+1);//Jump To Next
            return 0;
        }
        #endregion

        private void InitSeqFunction()
        {            
            //Main Seq
            mainSeqList.Add(new FunctionObjects("UnloadUnit", UnloadUnit)); //Idx 0
            mainSeqList.Add(new FunctionObjects("WaitUnitLoaded", WaitUnitLoaded)); //Idx 1
            mainSeqList.Add(new FunctionObjects("WaitUnloadStationUnitUnLoaded", WaitUnloadStationUnitUnloaded)); //Idx 2
            mainSeqList.Add(new FunctionObjects("CheckUnitPresentAfterLoad", CheckUnitPresentAfterLoaded, CheckUnitPresentError)); //Idx 3
            mainSeqList.Add(new FunctionObjects("ScanBarCode", ScanBarCode, ScanBarCodeError)); // Idx 4
            mainSeqList.Add(new FunctionObjects("WaitInspectAndTestReady", WaitInspectAndTestReady)); //Idx 5
            mainSeqList.Add(new FunctionObjects("IndexRotaryMotorAndIdx", RotaryIndexing, RotaryIndexingError)); //Idx 6
            mainSeq.AssignSeqList(mainSeqList);

            //CCD Seq Clovis
            cam1CCDSeq.AddFunction("WaitUnitReady", Cam1CCDWaitUnitReady, InspectUnitError); //Idx 0
            cam1CCDSeq.AddFunction("Cam1CCDCapture", Cam1CCDCapture, InspectUnitError);
            cam2CCDSeq.AddFunction("WaitUnitReady", Cam2CCDWaitUnitReady, InspectUnitError); //Idx 0
            cam2CCDSeq.AddFunction("Cam1CCDCapture", Cam2CCDCapture, InspectUnitError);

            //Camera Station
            camSeqList.Add(new FunctionObjects("WaitUnitReady", WaitUnitReady)); //Idx 0
            camSeqList.Add(new FunctionObjects("WaitInspectionDone", WaitInspectionDone));            
            camSeqList.Add(new FunctionObjects("SetInspectionDone", SetInspectionDone)); //Idx 3
            camSeq.AssignSeqList(camSeqList);
            //Additional Clovis
            cam1InspSeq.AddFunction("WaitUnitReady", Cam1InspWaitUnitReady, InspectUnitError); //Idx 0
            cam1InspSeq.AddFunction("InspectMod1Unit", InspectMod1Unit, InspectUnitError);
            cam2InspSeq.AddFunction("WaitUnitReady", Cam2InspWaitUnitReady, InspectUnitError); //Idx 0
            cam2InspSeq.AddFunction("InspectMod2Unit", InspectMod2Unit, InspectUnitError);

            //Test Station
            testSeqList.Add(new FunctionObjects("WaitUnitReady", TestStationWaitUnitReady)); //Idx 0
            testSeqList.Add(new FunctionObjects("SetStartOfTest", TestStationSetStartTest)); //Idx 1
            testSeqList.Add(new FunctionObjects("WaitModTestEnd", TestStationWaitModTestEnd)); //Idx 2            
            testSeqList.Add(new FunctionObjects("SetTestStationDone", SetTestStationDone)); //Idx 3
            testSeq.AssignSeqList(testSeqList);

            testMod1List.Add(new FunctionObjects("WaitStartOfTest", Mod1WaitStartOfTest)); //Idx 0
            testMod1List.Add(new FunctionObjects("TestMod1", TestMod1, TestModuleError)); //Idx 1
            testMod1List.Add(new FunctionObjects("SetMod1TestDone", SetMod1TestDone)); //Idx 2
            testMod1Seq.AssignSeqList(testMod1List);

            testMod2List.Add(new FunctionObjects("WaitStartOfTest", Mod2WaitStartOfTest)); //Idx 0
            testMod2List.Add(new FunctionObjects("TestMod2", TestMod2, TestModuleError)); //Idx 1
            testMod2List.Add(new FunctionObjects("SetMod2TestDone", SetMod2TestDone)); //Idx 2
            testMod2Seq.AssignSeqList(testMod2List);


            //Module 4 Unloading
            UnloadingSeq.AddFunction("UnloadStationWaitUnitReady", UnloadStationWaitUnitReady); //Idx 0
            UnloadingSeq.AddFunction("UnloadStationUnloadUnit", UnloadStationUnloadUnit); //Idx 0
            UnloadingSeq.AddFunction("UnloadStationWaitUNloadReady", UnloadStationWaitUnloadReady); //Idx 0

            //Alvin 2/3/17

            //Test Camera
            TestCameraSeq.AddFunction("TestCameraWaitUnitReady", TestCameraWaitUnitReady);//Idx 0
            TestCameraSeq.AddFunction("TestCameraStartTest", TestCameraStartTest, InspectUnitError); //Idx 1
            TestCameraSeq.AddFunction("TestCameraStartTest", SetTestCameraDone);//Idx 2

            //Test Camera2
            TestCamera2Seq.AddFunction("TestCamera2WaitUnitReady", TestCamera2WaitUnitReady);//Idx 0
            TestCamera2Seq.AddFunction("TestCamera2StartTest", TestCamera2StartTest, InspectUnitError); //Idx 1
            TestCamera2Seq.AddFunction("TestCamera2StartTest", SetTestCamera2Done);//Idx 2

            ResetAll();

            for (int i = 0; i < _Num_Of_Station; i++)
            {
                HImage image, image2;
                String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
                string ImgFolderPath = exePath + "Resources\\";

                string strHeadImagePath = ImgFolderPath + "DefaultImg.bmp";
                image = new HImage(strHeadImagePath);
                image2 = new HImage(strHeadImagePath);
                stationStatus[i].mod1VisResult.InspectedImage = image;
                stationStatus[i].mod2VisResult.InspectedImage = image2;
            }
        }

        private void OnThreadStoppedError()
        {            
            PauseAuto();
            //Buzzer
            if (Para.EnableBuzzer)
            {
                int BuzzerCnt = 2;
                int BuzzerTime = 100;
                for (int i = 0; i < BuzzerCnt; i++)
                {
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.
                        Buzzer, true);
                    Thread.Sleep(BuzzerTime);
                    Application.DoEvents();
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.Buzzer, false);
                }
            }
            mainWnd.UIPauseClicked(true);

            if (Para.isRotaryError)
            {
                mainWnd.OnlyHomeEnb();
            }
        }

        public void StartAuto()
        {
            //safetySeq.Start();
            mainSeq.Start();
            camSeq.Start();
            testSeq.Start();
            testMod1Seq.Start();
            testMod2Seq.Start(); 
            cam1InspSeq.Start();
            cam2InspSeq.Start();
            cam1CCDSeq.Start();
            cam2CCDSeq.Start();
            UnloadingSeq.Start();
            TestCameraSeq.Start();
            TestCamera2Seq.Start();
        }

        public void StopAuto()
        {
            //safetySeq.Stop();
            mainSeq.Stop();
            camSeq.Stop();
            testSeq.Stop();
            testMod1Seq.Stop();
            testMod2Seq.Stop();
            cam1InspSeq.Stop();
            cam2InspSeq.Stop();
            cam1CCDSeq.Stop();
            cam2CCDSeq.Stop();
            UnloadingSeq.Stop();
            TestCameraSeq.Stop();
            TestCamera2Seq.Stop();
            ResetAll();
        }

        public void PauseAuto()
        {
            //safetySeq.Pause();
            mainSeq.Pause();
            camSeq.Pause();
            testSeq.Pause();
            testMod1Seq.Pause();
            testMod2Seq.Pause();
            cam1InspSeq.Pause();
            cam2InspSeq.Pause();
            cam1CCDSeq.Pause();
            cam2CCDSeq.Pause();
            UnloadingSeq.Pause();
            TestCameraSeq.Pause();
            TestCamera2Seq.Stop();
        }
        //public void TestSafety()
        //{
        //    safetySeq.Reset();
        //    safetySeq.Start();
        //}
        public void ResetAll()
        {
            //safetySeq.Reset();
            mainSeq.Reset();
            camSeq.Reset();
            testSeq.Reset();
            testMod1Seq.Reset();
            testMod2Seq.Reset();
            cam1InspSeq.Reset();
            cam2InspSeq.Reset();
            cam1CCDSeq.Reset();
            cam2CCDSeq.Reset();
            UnloadingSeq.Reset();
            TestCameraSeq.Reset();
            TestCamera2Seq.Reset();
            isInspectionReady = true;
            isTestStationReady = true;
            isCamStationRotaryIndexDone = false;
            isTestStationRotaryIndexDone = false;            
            isCam1CCDIndexDone= false; isCam1CCDReady= false; isCam2CCDIndexDone= false; isCam2CCDReady= false;
            isCam1InspIndexDone = false; isCam1InspReady = false; isCam2InspIndexDone = false; isCam2InspReady = false;
            mod1StartTest = false;
            mod2StartTest = false;
            mod1TestEnd = false;
            mod2TestEnd = false;
            isUnloadStationRotaryIndexDone = false;
            UnloadStationUnloadDone = true;

            isTestCameraDone = false;
            isTestCamera2Done = false;

            for (int i = 0; i < _Num_Of_Station; i++)
            {
                stationStatus[i].isUnitLoad1 = false;
                stationStatus[i].isUnitLoad2 = false;
                stationStatus[i].Unit1Barcode = "";
                stationStatus[i].Unit2Barcode = "";
            }
        }

        private int SafetyCheck()
        {
            int res = 0;
            //if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.AirPressure))
            //    res = -7;

            //if (myMotionMgr.ReadIOIn((ushort)InputIOlist.SafetySensor))
            //    res = -5;

            if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnEMO))
                res = -4;

            //if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.DoorSensor))
            //    res = -6;

            if (myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnStop))
                res = -2;

            return res;
        }

        private int SafetyCheckError(int result)
        {
            switch (result)
            {
                case -2:
                    MessageBox.Show("Stop Button Pressed!", "Safety Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -4:
                    //mainWnd.WriteOperationinformation("Emergency Button Done !!!");
                    Para.isRotaryError = true;
                    Application.DoEvents();
                    mainWnd.OnlyHomeEnb();
                    MessageBox.Show("EMO Button Pressed!", "Safety Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -5:
                    Para.isRotaryError = true;
                    Application.DoEvents();
                    mainWnd.OnlyHomeEnb();
                    MessageBox.Show("Safety Sensor Activated.", "Safety Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -6:
                    MessageBox.Show("Machine Door Opened!", "Safety Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -7:
                    MessageBox.Show("Air Pressure Low!", "Safety Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            return 0;
        }

        #region main Sequencing Functions
        private int UnloadUnit()
        {
            int rtIdx = Para.CurrentRotaryIndex;

            if (Para.ContTestRunData)
            {
                Thread.Sleep(1000);
                mainSeq.JumpIndex(5);

                if (!Para.EnbStation4Unloading)
                {                   
                    if (stationStatus[rtIdx].isUnitLoad1)
                    {                       
                        if (stationStatus[rtIdx].IsMod1UnitPassed)
                        {
                            mainWnd.UpdateMod1TestResult(0, rtIdx);

                        }
                        else
                        {
                            mainWnd.UpdateMod1TestResult(1, rtIdx);

                        }
                        if (stationStatus[rtIdx].IsMod2UnitPassed)
                        {
                            mainWnd.UpdateMod2TestResult(0, rtIdx);

                        }
                        else
                        {
                            mainWnd.UpdateMod2TestResult(1, rtIdx);

                        }

                        mainWnd.DisplayBarcode(1, stationStatus[rtIdx].Unit1Barcode);
                        mainWnd.DisplayBarcode(2, stationStatus[rtIdx].Unit2Barcode);

                        mainWnd.UpdateTotalCountUI();
                         //To Jump Index
                    }
                }
                return 99;
            }
            else
            {
                switch (rtIdx)
                {
                    case 0:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
                        break;
                    case 1:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);                        
                        break;
                    case 2:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);                        
                        break;
                    case 3:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);                        
                        break;
                }

                if (!Para.EnbStation4Unloading)
                {
                    if (stationStatus[rtIdx].isUnitLoad1)
                    {
                        if (stationStatus[rtIdx].IsMod1UnitPassed)
                            mainWnd.UpdateMod1TestResult(0, rtIdx);
                        else
                            mainWnd.UpdateMod1TestResult(1, rtIdx);

                        mainWnd.DisplayBarcode(1, stationStatus[rtIdx].Unit1Barcode);

                        if (!stationStatus[rtIdx].IsMod1UnitPassed)
                        {
                            Para.Mod1FailUnitCnt = Para.Mod1FailUnitCnt + 1;
                        }
                    }
                    else
                    {
                        mainWnd.UpdateMod1TestResult(-1, rtIdx);
                    }

                    if (stationStatus[rtIdx].isUnitLoad2)
                    {
                        if (stationStatus[rtIdx].IsMod2UnitPassed)
                            mainWnd.UpdateMod2TestResult(0, rtIdx);
                        else
                            mainWnd.UpdateMod2TestResult(1, rtIdx);

                        mainWnd.DisplayBarcode(2, stationStatus[rtIdx].Unit2Barcode);

                        if (!stationStatus[rtIdx].IsMod2UnitPassed)
                        {

                            Para.Mod2FailUnitCnt = Para.Mod2FailUnitCnt + 1;

                        }
                    }
                    else
                    {
                        mainWnd.UpdateMod2TestResult(-1, rtIdx);
                    }

                    if (stationStatus[rtIdx].isUnitLoad1)
                    {

                        Para.TotalTestUnit = Para.TotalTestUnit + 2;
                    }

                    mainWnd.UpdateTotalCountUI();
                    stationStatus[rtIdx].isUnitLoad1 = false;
                    stationStatus[rtIdx].isUnitLoad2 = false;
                }
            }
                
            
                if (Para.EndLot)
                {
                    if ((stationStatus[0].isUnitLoad1 == false) && (stationStatus[0].isUnitLoad2 == false) &&
                        (stationStatus[1].isUnitLoad1 == false) && (stationStatus[1].isUnitLoad2 == false) &&
                        (stationStatus[2].isUnitLoad1 == false) && (stationStatus[2].isUnitLoad2 == false) &&
                        (stationStatus[3].isUnitLoad1 == false) && (stationStatus[3].isUnitLoad2 == false))
                    {
                        Para.EndLot = false;
                        //StopAuto();
                    }
                }
            return 0;
        }
        private int CheckUnitPresent()
        {
            int rtIdx = Para.CurrentRotaryIndex;
            int myRes = 0;
            switch (rtIdx)
            {
                case 1:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, true);
                    Thread.Sleep(100);                   
                    break;
                case 2:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, true);
                    Thread.Sleep(100);                   
                    break;
                case 3:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, true);
                    Thread.Sleep(100);                   
                    break;
                case 4:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, true);
                    Thread.Sleep(100);                    
                    break;

            }

            return myRes;
        }
        private int CheckUnitPresentError(int myRes)
        {
            switch (myRes)
            {
                case -1:
                    //MessageBox.Show("Unit Present at Loading. Rotary Index 1", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("工位 1 当前有料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -2:
                    MessageBox.Show("工位 2 当前有料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -3:
                    MessageBox.Show("工位 3 当前有料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -4:
                    MessageBox.Show("工位 4 当前有料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            return 0;
        }
        private int WaitUnitLoaded()
        {
            if (!Para.MachineOnline)
            {
                Thread.Sleep(2000);
                return 0;
            }
            if (!Para.DisableUpTriggerMode)
            {
                if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnLeft) && !myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnRight))
                {
                    Para.BothBtnAreLow = true;
                }

                if (Para.BothBtnAreLow)
                {
                    if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnLeft) || !myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnRight))
                    {
                        Thread.Sleep(50);
                        Application.DoEvents();

                        mainSeq.JumpIndex(mainSeq.CurrentIndex());
                        return 99; //To Jump Index
                    }
                    string timeStartStr = DateTime.Now.ToString("hh-mm-ss-fff");
                    Para.startTime = timeStartStr;
                    mainWnd.WriteOperationinformation("Test Start:" + timeStartStr);//20161208
                    Para.BothBtnAreLow = false;
                    return 0;
                }
                Thread.Sleep(50);
                Application.DoEvents();
                mainSeq.JumpIndex(mainSeq.CurrentIndex());
                return 99; //To Jump Index
            }
            else
            {
                if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnLeft) || !myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnRight))
                {
                    Thread.Sleep(50);
                    Application.DoEvents();

                    mainSeq.JumpIndex(mainSeq.CurrentIndex());
                    return 99; //To Jump Index
                }
                string timeStartStr = DateTime.Now.ToString("hh-mm-ss-fff");
                Para.startTime = timeStartStr;
                mainWnd.WriteOperationinformation("Test Start:" + timeStartStr);//20161208
                Para.BothBtnAreLow = false;
                return 0;
            }
            //if (!Para.MachineOnline)
            //{
            //    Thread.Sleep(2000);
            //    return 0;
            //}
            //if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnLeft) || !myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnRight))
            //{
            //    Thread.Sleep(50);
            //    Application.DoEvents();

            //    mainSeq.JumpIndex(mainSeq.CurrentIndex());
            //    return 99; //To Jump Index
            //}
            //string timeStartStr = DateTime.Now.ToString("hh-mm-ss-fff");
            //Para.startTime = timeStartStr;
            //mainWnd.WriteOperationinformation("Test Start:" + timeStartStr);//20161208
            
            //return 0;
        }
        private int WaitUnloadStationUnitUnloaded()
        {
            if (Para.EnbStation4Unloading)
            {
                if (!UnloadStationUnloadDone)
                {
                    
                    Thread.Sleep(50);
                    Application.DoEvents();

                    mainSeq.JumpIndex(mainSeq.CurrentIndex());
                    return 99; //To Jump Index
                }

            }
            return 0;
        }
        
        private int CheckUnitPresentAfterLoaded()
        {
            int rtIdx = Para.CurrentRotaryIndex;
            int myRes = 0;
            if (!Para.MachineOnline)            
            {
                Thread.Sleep(500);
                return 0;
            }

            //No Sensor
            switch (rtIdx)
            {
                case 0:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, true);
                    Thread.Sleep(100);                    
                    break;
                case 1:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, true);
                    Thread.Sleep(100);                    
                    break;
                case 2:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, true);
                    Thread.Sleep(100);                   
                    break;
                case 3:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, true);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, true);
                    Thread.Sleep(100);                    
                    break;

            }            
            //stationStatus[rtIdx].isUnitLoad1 = true;
            //stationStatus[rtIdx].isUnitLoad2 = true;   
            return myRes;
        }
        private int CheckUnitNotPresentError(int myRes)
        {
            switch (myRes)
            {
                case -1:
                    //MessageBox.Show("Unit Not Present at Loading. Rotary Index 1", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("工位 1 当前无料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -2:
                    MessageBox.Show("工位 2 当前无料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -3:
                    MessageBox.Show("工位 3 当前无料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -4:
                    MessageBox.Show("工位 4 当前无料！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            return 0;
        }
        private int ScanBarCode()
        {
            string strread = "";
            FileOperation.ReadData(Para.MchConfigFileName, "ContinueRunTime", "Time", ref strread);
            Para.SystemRunTime = DateTime.Parse(strread);

            TimeSpan time_span11 = DateTime.Now - Para.SystemRunTime;
            if (time_span11.TotalHours >= 24)
            {
                //Para.SystemRunTime = DateTime.Now;
                return -11;
            }

            FileOperation.ReadData(Para.MchConfigFileName, "NDContinueRunTime", "Time", ref strread);
            Para.NDSystemRunTime = DateTime.Parse(strread);

            time_span11 = DateTime.Now - Para.NDSystemRunTime;
            if (time_span11.TotalHours >= 24 * Para.NDTime)
            {
                //Para.SystemRunTime = DateTime.Now;
                return -12;
            }

            FileOperation.ReadData(Para.MchConfigFileName, "LSContinueRunTime", "Time", ref strread);
            Para.LSSystemRunTime = DateTime.Parse(strread);

            time_span11 = DateTime.Now - Para.LSSystemRunTime;
            if (time_span11.TotalHours >= 24 * Para.NDTime)
            {
                //Para.SystemRunTime = DateTime.Now;
                return -13;
            }
            int rtIdx = Para.CurrentRotaryIndex;
            int res = 0;

            if ((!Para.MachineOnline) || (Para.DryRunMode))
            {             
                Thread.Sleep(500);
                return 0;
            }
            bool barCodeEnter = false;

            if (Para.EngineerMode)
            {
                Action ac = new Action(() =>
                {
                    String input = Microsoft.VisualBasic.Interaction.InputBox("Module 1 Barcode", "Barcode", "Barcode");
                    stationStatus[rtIdx].Unit1Barcode = input;
                    stationStatus[rtIdx].isUnitLoad1 = true;
                    mainWnd.DisplayBarcode(1, stationStatus[rtIdx].Unit1Barcode);
                    input = Microsoft.VisualBasic.Interaction.InputBox("Module 2 Barcode", "Barcode", "Barcode");
                    stationStatus[rtIdx].Unit2Barcode = input;
                    stationStatus[rtIdx].isUnitLoad2 = true;
                    mainWnd.DisplayBarcode(2, stationStatus[rtIdx].Unit2Barcode);
                    barCodeEnter = true;
                });
                mainWnd.BeginInvoke(ac);
                //return 0;
            }

            if (Para.EngineerMode)
            {
                while (!barCodeEnter)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                return 0;
            }
            if (Para.DisableBarcode) 
            {
                stationStatus[rtIdx].Unit1Barcode = "DisableBarcodeA";
                stationStatus[rtIdx].isUnitLoad1 = true;
                mainWnd.DisplayBarcode(1, stationStatus[rtIdx].Unit1Barcode);
                stationStatus[rtIdx].Unit2Barcode = "DisableBarcodeB";
                stationStatus[rtIdx].isUnitLoad2 = true;
                mainWnd.DisplayBarcode(2, stationStatus[rtIdx].Unit2Barcode);
                return 0;
            }

            if (Para.EndLot)
            {
                //int rtIdx = Para.CurrentRotaryIndex;
                switch (rtIdx)
                {
                    case 0:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
                        break;
                    case 1:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);
                        break;
                    case 2:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);
                        break;
                    case 3:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                        break;
                }
                return 0;
            }
            DateTime timeString1 = DateTime.Now;
           // mainWnd.WriteOperationinformation("Start read barcode:" + timeString1);
            if (barcodeMgr.barcodeList[0].IsConnected)
                stationStatus[rtIdx].Unit1Barcode = barcodeMgr.barcodeList[0].Read();

            if (stationStatus[rtIdx].Unit1Barcode == "")
            {
                stationStatus[rtIdx].isUnitLoad1 = false;
                res = - 1;
            }
            else
            {
                stationStatus[rtIdx].isUnitLoad1 = true;
                stationStatus[rtIdx].Unit1Barcode = stationStatus[rtIdx].Unit1Barcode.Substring(0, 44);
                mainWnd.WriteOperationinformation("二维码1：" + stationStatus[rtIdx].Unit1Barcode);
                //mainWnd.DisplayBarcode(1, stationStatus[rtIdx].Unit1Barcode);
            }

            if (barcodeMgr.barcodeList[1].IsConnected)
                stationStatus[rtIdx].Unit2Barcode = barcodeMgr.barcodeList[1].Read();

            if (stationStatus[rtIdx].Unit2Barcode == "")
            {
                stationStatus[rtIdx].isUnitLoad2 = false;
                res = - 2;
            }
            else
            {
                stationStatus[rtIdx].isUnitLoad2 = true;
                stationStatus[rtIdx].Unit2Barcode = stationStatus[rtIdx].Unit2Barcode.Substring(0, 44);
                mainWnd.WriteOperationinformation("二维码2：" + stationStatus[rtIdx].Unit2Barcode);
            }
            DateTime timeString2 = DateTime.Now;
                logg.calculateTime(timeString2,timeString1,"barCode");
          //  mainWnd.WriteOperationinformation("End read barcode:" + timeString2);

            if ((stationStatus[rtIdx].isUnitLoad1 == false) && (stationStatus[rtIdx].isUnitLoad2 == false))
            {
                if (Para.EndLot)
                {
                    //int rtIdx = Para.CurrentRotaryIndex;
                    switch (rtIdx)
                    {
                        case 0:
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
                            break;
                        case 1:
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);
                            break;
                        case 2:
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);
                            break;
                        case 3:
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                            break;
                    }
                    return 0;
                }
                return -9; //No Unit loaded
            }

            if (!Para.EnableSingleModule)
            {
                if ((stationStatus[rtIdx].isUnitLoad1 == false) || (stationStatus[rtIdx].isUnitLoad2 == false))
                {
                    return res;
                }
            }

            return 0;
        }
        private int ScanBarCodeError(int result)
        {
            switch (result)
            {
                case -1:
                    //MessageBox.Show("Unit 1 Barcode Can't Read.", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("工位 1 未读到二维码！.", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    break;
                case -2:
                    MessageBox.Show("工位 2 未读到二维码！.", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -3:
                    MessageBox.Show("工位 3 未读到二维码！.", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -4:
                    MessageBox.Show("工位 4 未读到二维码！.", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -9:
                    //MessageBox.Show("Unit 1 and Unit 2 Barcode Can't Read.", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (MessageBox.Show("Both Unit Barcode Can't Read. End Lot?", "Main Sequence", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Para.EndLot = true;
                    }
                    break;
                case -11:
                    MessageBox.Show("运行时间超过24小时，请Dailycheck", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -12:
                    MessageBox.Show("运行时间超时，请ND", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -13:
                    MessageBox.Show("运行时间超时，请LS", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            int rtIdx = Para.CurrentRotaryIndex;
            switch (rtIdx)
            {
                case 0:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
                    break;
                case 1:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);
                    break;
                case 2:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);
                    break;
                case 3:
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                    break;
            }
            mainSeq.JumpIndex(0);
            return 99; //To Jump Index
        }
        private int WaitInspectAndTestReady()
        {
            if (!isInspectionReady || !isTestStationReady)
            {
                Thread.Sleep(50);
                Application.DoEvents();

                mainSeq.JumpIndex(mainSeq.CurrentIndex());
                return 99; //To Jump Index
            }
            return 0;
        }
        private int RotaryIndexing()
        {
            //Trigger Stations Index Done
            if (!Para.MachineOnline)
            {
                Thread.Sleep(2000);
            }
            //if ((myMotionMgr.ReadIOIn((ushort)InputIOlist.SafetySensor)) || (myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnStop)))
            //    if (Para.DisableSafeDoor)
            //        return -5;
            //    else
            //    {
            //        if((!myMotionMgr.ReadIOIn((ushort)InputIOlist.DoorSensor)))
            //            return -5;
            //    }
            //string timeStart45_1_Str = DateTime.Now.ToString("hh-mm-ss-fff");
            //mainWnd.WriteOperationinformation("Test Start 45_1:" + timeStart45_1_Str);//20161208
            if (!rotMgr.IndexRotaryMotion())
                return -5;
            UnloadStationUnloadDone = false;     //xiesuming
            rotMgr.InTestCameraDone = false;//20161214@Brando
            Thread.Sleep(50);//from 500 to 50
            //string timeEnd45_1_Str = DateTime.Now.ToString("hh-mm-ss-fff");
            //mainWnd.WriteOperationinformation("Test End 45_1:" + timeEnd45_1_Str);//20161208
            if (Para.Enb45DegTest)
            {
                if (Para.isRotaryAt45)
                {
                CaptureCCDExposure1();
                ReadCGHeight();
                if (!rotMgr.IndexRotaryMotion())
                    return -1;
                }
            }
            //string timeEnd45_2_Str = DateTime.Now.ToString("hh-mm-ss-fff");
            //mainWnd.WriteOperationinformation("Test End 45_2:" + timeEnd45_2_Str);//20161208

            isCamStationRotaryIndexDone = true;
            isTestStationRotaryIndexDone = true;
            isUnloadStationRotaryIndexDone = true;
            //IncLoadRotIdx();//CurRotIdx++;
            return 0;
        }
        

        //P2 Test 45 deg
        private int CaptureCCDExposure1()
        {

            isCam1CCDIndexDone = true;
            isCam2CCDIndexDone = true;
            Application.DoEvents();
            Thread.Sleep(200);
            

            DateTime st_time = DateTime.Now;
            TimeSpan time_span;
            while (!isCam1CCDReady || !isCam2CCDReady)
            {
                Thread.Sleep(10);
                Application.DoEvents();
                time_span = DateTime.Now - st_time;                
                if (time_span.TotalMilliseconds > 20000)
                {
                    return -1;
                }
            }
            return 0;                        
        }
        private int ReadCGHeight()
        {
            int RotaryIdxAtCam45 = GetIndexOfCam();
            //if (stationStatus[RotaryIdxAtCam45].isUnitLoad1)
            //{
            //    double tmp = Para.myMain.DLRS1.Read();
                
            //    if (Math.Abs(tmp) > 2)
            //        tmp = Para.myMain.DLRS1.Read();

            //    stationStatus[RotaryIdxAtCam45].Unit1CGHeight = tmp;
            //}
            //if (stationStatus[RotaryIdxAtCam45].isUnitLoad2)
            //{
            //    double tmp = Para.myMain.DLRS2.Read();

            //    if (Math.Abs(tmp) > 2)
            //        tmp = Para.myMain.DLRS2.Read();
            //    stationStatus[RotaryIdxAtCam45].Unit2CGHeight = tmp;
            //}
            return 0;
        }
        private int RotaryIndexingError(int result)
        {
            switch (result)
            {
                case -1:
                    Para.isRotaryError = true;
                    Thread.Sleep(500);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RotaryEnabled, false);
                    Application.DoEvents();
                    mainWnd.OnlyHomeEnb();
                    //MessageBox.Show("Y Axis Not At Home Position. Not Safe to Move.", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("Y 轴不在Home位置，请确保安全后再移动！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Para.isRotaryError = true;
                    break;
                case -5:
                    Para.isRotaryError = true;
                    Thread.Sleep(500);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RotaryEnabled, false);
                    Application.DoEvents();
                    mainWnd.OnlyHomeEnb();
                    //MessageBox.Show("Safety Sensor Activate During Rotary Moving. Please Home All Again", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("安全门或安全光栅被触发，请先复位！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Para.isRotaryError = true;
                    break;                
            }
            return 0;
        }
        #endregion

        #region Camera Station Sequencing function

        private int Cam1InspWaitUnitReady()
        {
            if (!isCam1InspIndexDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                cam1InspSeq.JumpIndex(0);
                return 99; //To Jump Index
            }
            isCam1InspReady = false;
            isCam1InspIndexDone = false;                                             
            return 0;
        }
        private int Cam2InspWaitUnitReady()
        {
            if (!isCam2InspIndexDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                cam2InspSeq.JumpIndex(0);
                return 99; //To Jump Index
            }
            isCam2InspReady = false;
            isCam2InspIndexDone = false;
            return 0;
        }
        private int Cam1CCDCapture()
        {
            DateTime timeCam1CCDCapture1 = DateTime.Now;
             int rtIdx = Para.CurrentRotaryIndex;
            int RotaryIdxAtCam45 = Para.CurrentRotaryIndex;

            if (stationStatus[RotaryIdxAtCam45].isUnitLoad1)
            {
                //Dark
                Cam1.SetExposure(Para.Cam1ExposureTime1);
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                Thread.Sleep(100);
                bool GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Cam1.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }
                if (!GrabPass)
                {
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                    Thread.Sleep(100);
                    return -1;
                }
                stationStatus[RotaryIdxAtCam45].Mod1Exp1DarkImage = Cam1.GetImgPtr();
                //20161028
                //string timeStr0 = DateTime.Now.ToString("yyMMdd");
                //string path = "D:\\Picture\\"+timeStr0;
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //string timeStr = DateTime.Now.ToString("yyMMddHHmmss");

                //string FileName = path + "\\" + stationStatus[rtIdx].Unit1Barcode +"_"+ timeStr + "_" + Para.Cam1ExposureTime1 +"_1" + ".bmp";

                //HOperatorSet.WriteImage(Cam1.myImage, "bmp", 0, FileName);
                
               //stationStatus[RotaryIdxAtCam45].CCD1LightFalse = Cam1.myImage;
               //HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam45].CCD1LightFalse, 1, 1);
               //string DataFileName = @"D:\Picture\";
               //string temp1 = DataFileName + "1114_A";
               //HOperatorSet.WriteImage(stationStatus[RotaryIdxAtCam45].CCD1LightFalse, "bmp", 0, temp1);
                HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam45].CCD1LightFalse, 1, 1);
                //20161028
                //White
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
                Thread.Sleep(200);
                GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Cam1.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }
                if (!GrabPass)
                {
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                    Thread.Sleep(200);
                    return -1;
                }
                stationStatus[RotaryIdxAtCam45].Mod1Exp1Image = Cam1.GetImgPtr();
                //20161028
                //timeStr0 = DateTime.Now.ToString("yyMMdd");
                //path = "D:\\Picture\\" + timeStr0;
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //timeStr = DateTime.Now.ToString("yyMMddHHmmss");

                //FileName = path + "\\" + stationStatus[rtIdx].Unit1Barcode + "_" + timeStr + "_" + Para.Cam1ExposureTime1 + "_2" + ".bmp";

                //HOperatorSet.WriteImage(Cam1.myImage, "bmp", 0, FileName);
                //stationStatus[RotaryIdxAtCam45].CCD1LightTrue = Cam1.myImage;
                HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam45].CCD1LightTrue, 1, 1);
                //20161028
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                DateTime timeCam1CCDCapture2 = DateTime.Now;
                logg.calculateTime(timeCam1CCDCapture2, timeCam1CCDCapture1, "Cam1CCDCapture");
                Thread.Sleep(100);
            }
            isCam1CCDReady = true;
            return 0;
        }
        private int Cam2CCDCapture()
        {
            DateTime timeCam2CCDCapture1 = DateTime.Now;
            int rtIdx = Para.CurrentRotaryIndex;
            int RotaryIdxAtCam45 = Para.CurrentRotaryIndex;

            if (stationStatus[RotaryIdxAtCam45].isUnitLoad2)
            {
                //Dark
                Cam2.SetExposure(Para.Cam2ExposureTime1);
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                Thread.Sleep(200);
                bool GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Cam2.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }
                if (!GrabPass)
                {
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                    Thread.Sleep(200);
                    return -3;
                }
                stationStatus[RotaryIdxAtCam45].Mod2Exp1DarkImage = Cam2.GetImgPtr();
                //20161028
                //string timeStr0 = DateTime.Now.ToString("yyMMdd");
                //string path = "D:\\Picture\\" + timeStr0;
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //string timeStr = DateTime.Now.ToString("yyMMddHHmmss");

                //string FileName = path + "\\" + stationStatus[rtIdx].Unit2Barcode + "_" + timeStr + "_" + Para.Cam2ExposureTime1 + "_1" + ".bmp";

                //HOperatorSet.WriteImage(Cam2.myImage, "bmp", 0, FileName);
                //stationStatus[RotaryIdxAtCam45].CCD2LightFalse = Cam2.myImage;
                HOperatorSet.CopyObj(Cam2.myImage, out stationStatus[RotaryIdxAtCam45].CCD2LightFalse, 1, 1);
                //20161028

                //White
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
                Thread.Sleep(200);
                GrabPass = false;
                for (int i = 0; i < 3; i++)
                {
                    if (Cam2.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }
                if (!GrabPass)
                {
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                    Thread.Sleep(200);
                    return -3;
                }
                stationStatus[RotaryIdxAtCam45].Mod2Exp1Image = Cam2.GetImgPtr();
                //20161028
                //timeStr0 = DateTime.Now.ToString("yyMMdd");
                //path = "D:\\Picture\\" + timeStr0;
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}

                //timeStr = DateTime.Now.ToString("yyMMddHHmmss");

                //FileName = path + "\\" + stationStatus[rtIdx].Unit2Barcode + "_" + timeStr + "_" + Para.Cam2ExposureTime1 + "_2" + ".bmp";

                ////FileName = path + "\\" + "2"+timeStr + stationStatus[rtIdx].Unit2Barcode + ".bmp";
                //HOperatorSet.WriteImage(Cam2.myImage, "bmp", 0, FileName);
                stationStatus[RotaryIdxAtCam45].CCD2LightTrue = Cam2.myImage;
                HOperatorSet.CopyObj(Cam2.myImage, out stationStatus[RotaryIdxAtCam45].CCD2LightTrue, 1, 1);
                //20161028
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                DateTime timeCam2CCDCapture2 = DateTime.Now;
                logg.calculateTime(timeCam2CCDCapture2, timeCam2CCDCapture1, "Cam2CCDCapture");
                Thread.Sleep(200);
            }
            isCam2CCDReady = true;
            return 0;
        }
        private int Cam1CCDWaitUnitReady()
        {
            
            if (!isCam1CCDIndexDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                cam1CCDSeq.JumpIndex(0);
                return 99; //To Jump Index
            }
            isCam1CCDReady = false;
            isCam1CCDIndexDone = false;
                                 
            mainWnd.ClearInspectionResults();
            return 0;
        }
        private int Cam2CCDWaitUnitReady()
        {

            if (!isCam2CCDIndexDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                cam2CCDSeq.JumpIndex(0);
                return 99; //To Jump Index
            }
            isCam2CCDReady = false;
            isCam2CCDIndexDone = false;                                 
            mainWnd.ClearInspectionResults();
            return 0;
        }
        public int GetIndexOfCam()
        {
            int res = Para.CurrentRotaryIndex -1;
            if (res < 0)
                res = 3;

            return res;
        }

        private int WaitInspectionDone()
        {
            if (!isCam1InspReady)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                camSeq.JumpIndex(1);
                return 99; //To Jump Index
            }
            if (!isCam2InspReady)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                camSeq.JumpIndex(1);
                return 99; //To Jump Index
            }
            return 0;
        }
        private int WaitUnitReady()
        {
            if (!isCamStationRotaryIndexDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                camSeq.JumpIndex(0);
                return 99; //To Jump Index
            }

            //Alvin 2/3/17
            if ((isTestCameraDone) || (isTestCamera2Done))
            {
                Thread.Sleep(50);
                Application.DoEvents();
                camSeq.JumpIndex(0);
                return 99; //To Jump Index
            }

            isCamStationRotaryIndexDone = false;
            //isInspectionReady = false;

            int RotaryIdxAtCam = GetIndexOfCam();//CurRotIdx -1;
            
            if ((!stationStatus[RotaryIdxAtCam].isUnitLoad1) && (!stationStatus[RotaryIdxAtCam].isUnitLoad2))
            {
                camSeq.JumpIndex(0);
                return 99; //To Jump Index
            }

            isInspectionReady = false; 
            mainWnd.ClearInspectionResults();
            isCam1InspIndexDone = true;
            isCam2InspIndexDone = true;

            Thread.Sleep(200);
            return 0;
        }
        private int InspectMod1Unit()
        {
            DateTime timeInspectMod1Unit1 = DateTime.Now;
            int RotaryIdxAtCam = GetIndexOfCam();

            if (!Para.MachineOnline)               
            {
                Thread.Sleep(1000);
                return 0;
                //return -1;
            }

            if (stationStatus[RotaryIdxAtCam].isUnitLoad1)
            {
                //20161018
                string strread1 = "";

                //20161028
                string exposureTimeRecord = "";
                //20161028

                FileOperation.ReadData(Para.CurLoadConfigFileName, "ExposureTime", "disableAutoExpTime1", ref strread1);
                if (strread1 == "False")
                {
                    int barcodeLength = stationStatus[RotaryIdxAtCam].Unit1Barcode.Length;
                    string blackorwhite = stationStatus[RotaryIdxAtCam].Unit1Barcode.Substring(barcodeLength-1, 1);
                    if (blackorwhite == "3")//Black
                    {
                        mainWnd.camera1.SetExposure(Para.Cam1ExposureTimeB);

                        //20161028
                        exposureTimeRecord = Para.Cam1ExposureTimeB.ToString();
                        //20161028
                    }
                    else
                    {
                        mainWnd.camera1.SetExposure(Para.Cam1ExposureTimeW);

                        //20161028
                        exposureTimeRecord = Para.Cam1ExposureTimeW.ToString();
                        //20161028
                    }
                }
                else
                {
                    if (Para.selected1BorW == 0)
                    {

                        mainWnd.camera1.SetExposure(Para.Cam1ExposureTimeB);

                        //20161028
                        exposureTimeRecord = Para.Cam1ExposureTimeB.ToString();
                        //20161028
                    }

                    else
                    {
                        mainWnd.camera1.SetExposure(Para.Cam1ExposureTimeW);

                        //20161028
                        exposureTimeRecord = Para.Cam1ExposureTimeW.ToString();
                        //20161028
                    }

                }


                if (Para.Enb45DegTest)
                {
                    if (Para.EnableP2Test)
                    {
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                        Thread.Sleep(100);
                        bool GrabPassD = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (Cam1.Grab())
                            {
                                GrabPassD = true;
                                break;
                            }
                            Application.DoEvents();
                        }
                        if (!GrabPassD)
                        {
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                            Thread.Sleep(100);
                            return -1;
                        }
                        stationStatus[RotaryIdxAtCam].Mod1DarkImage = Cam1.GetImgPtr();
                        HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam].inspCCD1LightFalse, 1, 1);

                        //20161028
                    }
                }
                //20161018ok
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
                //logg.saveerrorLog("Mod1_LED open normaly");
                Thread.Sleep(1000);

                for (int c = 0; c < 3; c++)
                {
                    bool GrabPass = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (Cam1.Grab())
                        {
                            GrabPass = true;
                            break;
                        }
                        Application.DoEvents();
                    }

                    if (!GrabPass)
                    {
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                        Thread.Sleep(100);
                        return -1;

                    }

                    if (Para.EngineerMode)
                    {
                        bool CCDEnter = false;
                        Action ac = new Action(() =>
                        {
                            stationStatus[RotaryIdxAtCam].mod1VisResult = new HalconInspection.RectData();
                            stationStatus[RotaryIdxAtCam].mod1VisResult.Found = true;

                            Cam1Win = new EngModeCCDWin("Camera 1");
                            Cam1Win.Show();

                            while (Cam1Win.Visible)
                            {
                                Thread.Sleep(50);
                                Application.DoEvents();
                            }

                            //String input = Microsoft.VisualBasic.Interaction.InputBox("Module 1 CCD X", "CCD X Pixel", (Cam1.ImageWidth/2).ToString("F3"));
                            stationStatus[RotaryIdxAtCam].mod1VisResult.X = Cam1Win.X;
                            // Microsoft.VisualBasic.Interaction.InputBox("Module 1 CCD Y", "CCD Y Pixel", (Cam1.ImageHeight / 2).ToString("F3"));
                            stationStatus[RotaryIdxAtCam].mod1VisResult.Y = Cam1Win.Y; ;
                            //Microsoft.VisualBasic.Interaction.InputBox("Module 1 CCD Angle", "CCD Angle Degree", (0).ToString("F3"));
                            stationStatus[RotaryIdxAtCam].mod1VisResult.Angle = (double)Helper.GetRadianFromDegree((float)Cam1Win.Ang);
                            CCDEnter = true;
                        });
                        mainWnd.BeginInvoke(ac);

                        while (!CCDEnter)
                        {
                            Thread.Sleep(50);
                            Application.DoEvents();
                        }

                        break;
                    }
                    else
                    {
                        //Thread.Sleep(100);
                        stationStatus[RotaryIdxAtCam].mod1VisResult = Cam1.Inspect(Para.CaliX1);
                    }
                    if (stationStatus[RotaryIdxAtCam].mod1VisResult.Found)
                    {
                        break;
                    }
                }
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                //logg.saveerrorLog("Mod1_LED close normaly");
                Thread.Sleep(1500);

                if (myMotionMgr.ReadIOOut((ushort)OutputIOlist.Cam1Light))
                {
                    Thread.Sleep(200);
                    //return -8;
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                    Thread.Sleep(1500);
                }

                if (myMotionMgr.ReadIOOut((ushort)OutputIOlist.Cam1Light))
                {
                    return -8;
                }

                if (!stationStatus[RotaryIdxAtCam].mod1VisResult.Found)
                    return -2;

                stationStatus[RotaryIdxAtCam].Mod1Image = Cam1.GetImgPtr();
                //20161028  
                //20161121
                string timeStr = DateTime.Now.ToString("yyMMdd");
                string path = Para.PicPath + ":\\M1Picture\\" + timeStr;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //20161121
                HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam].inspCCD1LightTrue, 1, 1);
                //20170216
                HTuple HomMat2D, HomMat2DRotate;
                HOperatorSet.HomMat2dIdentity(out HomMat2D);
                HOperatorSet.HomMat2dRotate(HomMat2D, -stationStatus[RotaryIdxAtCam].mod1VisResult.Angle, stationStatus[RotaryIdxAtCam].mod1VisResult.Y,
                                            stationStatus[RotaryIdxAtCam].mod1VisResult.X, out HomMat2DRotate);
                //20170216
                string tStrinspCCD1LightTrue = DateTime.Now.ToString("yyMMddHHmmss");
                string FNinspCCD1LightTrue = path + "\\" + stationStatus[RotaryIdxAtCam].Unit1Barcode + "_" + tStrinspCCD1LightTrue + "_" + exposureTimeRecord + "_4_A" + ".bmp";
                HOperatorSet.WriteImage(Cam1.myImage, "bmp", 0, FNinspCCD1LightTrue);  //zhe ge ky?
                //20161028
                if (Para.isRotaryingGrab)
                {
                    //InspecDark 20161214@Brando
                    bool GrabPassDD = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (Cam1.Grab())
                        {
                            GrabPassDD = true;
                            break;
                        }
                        Application.DoEvents();
                    }
                    if (!GrabPassDD)
                    {
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                        Thread.Sleep(100);
                        return -1;
                    }
                    stationStatus[RotaryIdxAtCam].Mod1DarkImage = Cam1.GetImgPtr();
                    HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam].inspCCD1LightFalse, 1, 1);

                    string tStrinspCCD1LightFalse = DateTime.Now.ToString("yyMMddHHmmss");
                    string FNinspCCD1LightFalse = path + "\\" + stationStatus[RotaryIdxAtCam].Unit1Barcode + "_" + tStrinspCCD1LightFalse + "_" + exposureTimeRecord + "_3_A" + ".bmp";
                    HOperatorSet.WriteImage(Cam1.myImage, "bmp", 0, FNinspCCD1LightFalse);  //zhe ge ky?
                    //Dark
                    Cam1.SetExposure(Para.Cam1ExposureTime1);
                    bool GrabPassDark = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (Cam1.Grab())
                        {
                            GrabPassDark = true;
                            break;
                        }
                        Application.DoEvents();
                    }
                    if (!GrabPassDark)
                    {
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                        Thread.Sleep(100);
                        return -1;
                    }
                    stationStatus[RotaryIdxAtCam].Mod1Exp1DarkImage = Cam1.GetImgPtr();
                    HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam].CCD1LightFalse, 1, 1);

                    string tStrCCD1LightFalse = DateTime.Now.ToString("yyMMddHHmmss");
                    string FNCCD1LightFalse = path + "\\" + stationStatus[RotaryIdxAtCam].Unit1Barcode + "_" + tStrCCD1LightFalse + "_" + Para.Cam1ExposureTime1.ToString() + "_2_A" + ".bmp";
                    HOperatorSet.WriteImage(Cam1.myImage, "bmp", 0, FNCCD1LightFalse);  //zhe ge ky?
                    //20161214@Brando
                }
                //20170216
                if (Para.SampleShape == 0)
                {
                    HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].inspCCD1LightTrue, out stationStatus[RotaryIdxAtCam].inspCCD1LightTrueRotate,
                                                  HomMat2DRotate, "constant", "true");
                    HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].CCD1LightFalse, out stationStatus[RotaryIdxAtCam].CCD1LightFalseRotate,
                                                  HomMat2DRotate, "constant", "true");
                    HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].CCD1LightTrue, out stationStatus[RotaryIdxAtCam].CCD1LightTrueRotate,
                                                  HomMat2DRotate, "constant", "true");
                    HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].inspCCD1LightFalse, out stationStatus[RotaryIdxAtCam].inspCCD1LightFalseRotate,
                                                  HomMat2DRotate, "constant", "true");
                }
                else
                {
                    HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].inspCCD1LightTrue, out stationStatus[RotaryIdxAtCam].inspCCD1LightTrueRotate, 1, 1);
                    HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].CCD1LightFalse, out stationStatus[RotaryIdxAtCam].CCD1LightFalseRotate, 1, 1);
                    HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].CCD1LightTrue, out stationStatus[RotaryIdxAtCam].CCD1LightTrueRotate, 1, 1);
                    HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].inspCCD1LightFalse, out stationStatus[RotaryIdxAtCam].inspCCD1LightFalseRotate, 1, 1);
                }
                //20170216
                HObject hV_reducedImg, ho_saveImg;
                HObject ho_Rectangle;
                HOperatorSet.GenEmptyObj(out hV_reducedImg);

                mainWnd.DisplayCam1Result(stationStatus[RotaryIdxAtCam].mod1VisResult);
                if (Para.SampleShape == 0)
                {
                    HOperatorSet.GenRectangle2(out ho_Rectangle, stationStatus[RotaryIdxAtCam].mod1VisResult.Y, stationStatus[RotaryIdxAtCam].mod1VisResult.X,
                                           0, stationStatus[RotaryIdxAtCam].mod1VisResult.Width / 2 , stationStatus[RotaryIdxAtCam].mod1VisResult.Height / 2);
                }
                else
                {
                    HOperatorSet.GenCircle(out ho_Rectangle, stationStatus[RotaryIdxAtCam].mod1VisResult.Y, stationStatus[RotaryIdxAtCam].mod1VisResult.X,
                                          stationStatus[RotaryIdxAtCam].mod1VisResult.Radius);
                }


                HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].inspCCD1LightTrueRotate, ho_Rectangle, out hV_reducedImg);
                HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                string timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                string FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit1Barcode + "_" + timeStr01 + "_" + exposureTimeRecord + "_4_A" + ".bmp";
                //HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);  //zhe ge ky?
                HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].inspCCD1LightTrue, 1, 1);

                //if (Para.Enb45DegTest)
                if(true)
                {
                    HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].CCD1LightFalseRotate, ho_Rectangle, out hV_reducedImg);
                    HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                    timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                    FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit1Barcode + "_" + timeStr01 + "_" + Para.Cam1ExposureTime1.ToString() + "_1_A" + ".bmp";
                    //HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);
                    HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].CCD1LightFalse, 1, 1);

                    ////HOperatorSet.ReduceDomain(Para.inspCCD1LightFalse, Para.cutRectangle, out Para.temtRectangle);
                    HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].CCD1LightTrueRotate, ho_Rectangle, out hV_reducedImg);
                    HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                    timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                    FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit1Barcode + "_" + timeStr01 + "_" + Para.Cam1ExposureTime1.ToString() + "_2_A" + ".bmp";
                    //HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);
                    HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].CCD1LightTrue, 1, 1);

                    ////HOperatorSet.ReduceDomain(Cam1.myImage, Para.cutRectangle, out Para.temtRectangle);
                    HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].inspCCD1LightFalseRotate, ho_Rectangle, out hV_reducedImg);
                    HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                    timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                    FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit1Barcode + "_" + timeStr01 + "_" + exposureTimeRecord + "_3_A" + ".bmp";
                    //HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);
                    HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].inspCCD1LightFalse, 1, 1);
                }


            }
            isCam1InspReady = true;
            DateTime timeInspectMod1Unit2 = DateTime.Now;
            logg.calculateTime(timeInspectMod1Unit2, timeInspectMod1Unit1,"InspectMod1Unit");
            return 0;
        }
        private void HobjectToHimage(HObject hobject, ref HImage image)
        {
            HTuple pointer, type, width, height;

            HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
        }
        private int InspectMod2Unit()
        {
            DateTime timeInspectMod2Unit1 = DateTime.Now;
            int RotaryIdxAtCam = GetIndexOfCam();

            if (!Para.MachineOnline)
            {
                Thread.Sleep(1000);
                return 0;
                //return -1;
            }

            if (stationStatus[RotaryIdxAtCam].isUnitLoad2)
            {                
                //20161018
                string strread1 = "";

                //20161028
                string exposureTimeRecord = "";
                //20161028

                FileOperation.ReadData(Para.CurLoadConfigFileName, "ExposureTime", "disableAutoExpTime2", ref strread1);
                if (strread1 == "False")
                {
                    int barcodeLength = stationStatus[RotaryIdxAtCam].Unit2Barcode.Length;
                    string blackorwhite = stationStatus[RotaryIdxAtCam].Unit2Barcode.Substring(barcodeLength-1, 1);
                    if (blackorwhite == "3")//Black
                    {
                        mainWnd.camera2.SetExposure(Para.Cam2ExposureTimeB);
                        
                        //20161028
                        exposureTimeRecord = Para.Cam2ExposureTimeB.ToString();
                        //20161028
                    }
                    else
                    {
                        mainWnd.camera2.SetExposure(Para.Cam2ExposureTimeW);

                        //20161028
                        exposureTimeRecord = Para.Cam2ExposureTimeW.ToString();
                        //20161028
                    }
                }
                else
                {
                    
                    if (Para.selected2BorW == 0)
                    {

                        mainWnd.camera2.SetExposure(Para.Cam2ExposureTimeB);

                        //20161028
                        exposureTimeRecord = Para.Cam2ExposureTimeB.ToString();
                        //20161028
                    }

                    else
                    {
                        mainWnd.camera2.SetExposure(Para.Cam2ExposureTimeW);

                        //20161028
                        exposureTimeRecord = Para.Cam2ExposureTimeW.ToString();
                        //20161028
                    }
                }

                if (Para.Enb45DegTest)
                {
                    if (Para.EnableP2Test)
                    {
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                        Thread.Sleep(100);
                        bool GrabPassD = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (Cam2.Grab())
                            {
                                GrabPassD = true;
                                break;
                            }
                            Application.DoEvents();
                        }
                        if (!GrabPassD)
                        {
                            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                            Thread.Sleep(100);
                            return -1;
                        }
                        stationStatus[RotaryIdxAtCam].Mod2DarkImage = Cam2.GetImgPtr();                        
                        HOperatorSet.CopyObj(Cam2.myImage, out stationStatus[RotaryIdxAtCam].inspCCD2LightFalse, 1, 1);
                        //20161028
                    }
                }
                //20161018
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
                //logg.saveerrorLog("Mod2_LED open normaly");
                Thread.Sleep(1000);

                for (int c = 0; c < 3; c++)
                {
                    bool GrabPass = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (Cam2.Grab())
                        {
                            GrabPass = true;
                            break;
                        }
                    }

                    if (!GrabPass)
                    {
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                        Thread.Sleep(100);
                        return -3;
                    }
                               
                    //return -3;
                    if (Para.EngineerMode)
                    {
                        bool CCDEnter = false;
                        Action ac = new Action(() =>
                        {
                            stationStatus[RotaryIdxAtCam].mod2VisResult = new HalconInspection.RectData();
                            stationStatus[RotaryIdxAtCam].mod2VisResult.Found = true;

                            Cam2Win = new EngModeCCDWin("Camera 2");
                            Cam2Win.Show();

                            while (Cam2Win.Visible)
                            {
                                Thread.Sleep(50);
                                Application.DoEvents();
                            }

                            //String input = Microsoft.VisualBasic.Interaction.InputBox("Module 2 CCD X", "CCD X Pixel", (Cam2.ImageWidth / 2).ToString("F3"));
                            stationStatus[RotaryIdxAtCam].mod2VisResult.X = Cam2Win.X;
                            //Microsoft.VisualBasic.Interaction.InputBox("Module 2 CCD Y", "CCD Y Pixel", (Cam2.ImageHeight / 2).ToString("F3"));
                            stationStatus[RotaryIdxAtCam].mod2VisResult.Y = Cam2Win.Y;
                            //Microsoft.VisualBasic.Interaction.InputBox("Module 2 CCD Angle", "CCD Angle Degree", (0).ToString("F3"));
                            stationStatus[RotaryIdxAtCam].mod2VisResult.Angle = (double)Helper.GetRadianFromDegree((float)Cam2Win.Ang);
                            CCDEnter = true;
                        });
                        mainWnd.BeginInvoke(ac);

                        while (!CCDEnter)
                        {
                            Thread.Sleep(50);
                            Application.DoEvents();
                        }   

                        break;
                    }
                    else
                    {
                        stationStatus[RotaryIdxAtCam].mod2VisResult = Cam2.Inspect(Para.CaliX2);                        
                        if (stationStatus[RotaryIdxAtCam].mod2VisResult.Found)
                        {
                            break;
                        }
                    }
                }

                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                //logg.saveerrorLog("Mod2_LED close normally");
                Thread.Sleep(1500);

                if (myMotionMgr.ReadIOOut((ushort)OutputIOlist.Cam2Light))
                {
                    Thread.Sleep(200);
                    //return -11;
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                    Thread.Sleep(1500);
                }
                if (myMotionMgr.ReadIOOut((ushort)OutputIOlist.Cam2Light))
                {
                    return -11;
                }

                //20161121
                string timeStr = DateTime.Now.ToString("yyMMdd");
                string path = Para.PicPath + ":\\M2Picture\\" + timeStr;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //20161121
                if (!stationStatus[RotaryIdxAtCam].mod2VisResult.Found)
                    return -4;
                stationStatus[RotaryIdxAtCam].Mod2Image = Cam2.GetImgPtr();
                 stationStatus[RotaryIdxAtCam].inspCCD2LightTrue = Cam2.myImage;
                 HOperatorSet.CopyObj(Cam2.myImage, out stationStatus[RotaryIdxAtCam].inspCCD2LightTrue, 1, 1);
                //20170216
                 HTuple HomMat2D, HomMat2DRotate;
                 HOperatorSet.HomMat2dIdentity(out HomMat2D);
                 HOperatorSet.HomMat2dRotate(HomMat2D, -stationStatus[RotaryIdxAtCam].mod2VisResult.Angle, stationStatus[RotaryIdxAtCam].mod2VisResult.Y,
                                             stationStatus[RotaryIdxAtCam].mod2VisResult.X, out HomMat2DRotate);
                //20170216
                 string tSinspCCD2LightTrue = DateTime.Now.ToString("yyMMddHHmmss");
                 string FNinspCCD2LightTrue = path + "\\" + stationStatus[RotaryIdxAtCam].Unit2Barcode + "_" + tSinspCCD2LightTrue + "_" + exposureTimeRecord + "_4_B" + ".bmp";
                 HOperatorSet.WriteImage(Cam2.myImage, "bmp", 0, FNinspCCD2LightTrue);  //zhe ge ky?
                //20161028
                 if (Para.isRotaryingGrab)
                 {
                     //InspecDark 20161214@Brando
                     bool GrabPassDark2 = false;
                     for (int i = 0; i < 3; i++)
                     {
                         if (Cam2.Grab())
                         {
                             GrabPassDark2 = true;
                             break;
                         }
                         Application.DoEvents();
                     }
                     if (!GrabPassDark2)
                     {
                         myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                         Thread.Sleep(100);
                         return -3;
                     }
                     stationStatus[RotaryIdxAtCam].Mod2DarkImage = Cam2.GetImgPtr();
                     HOperatorSet.CopyObj(Cam2.myImage, out stationStatus[RotaryIdxAtCam].inspCCD2LightFalse, 1, 1);
                     string tSinspCCD2LightFalse = DateTime.Now.ToString("yyMMddHHmmss");
                     string FNinspCCD2LightFalse = path + "\\" + stationStatus[RotaryIdxAtCam].Unit2Barcode + "_" + tSinspCCD2LightFalse + "_" + exposureTimeRecord + "_3_B" + ".bmp";
                     HOperatorSet.WriteImage(Cam2.myImage, "bmp", 0, FNinspCCD2LightFalse);  //zhe ge ky?
                     //Dark
                     Cam2.SetExposure(Para.Cam2ExposureTime1);
                     GrabPassDark2 = false;
                     for (int i = 0; i < 3; i++)
                     {
                         if (Cam2.Grab())
                         {
                             GrabPassDark2 = true;
                             break;
                         }
                         Application.DoEvents();
                     }
                     if (!GrabPassDark2)
                     {
                         myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                         Thread.Sleep(200);
                         return -3;
                     }
                     stationStatus[RotaryIdxAtCam].Mod2Exp1DarkImage = Cam2.GetImgPtr();
                     HOperatorSet.CopyObj(Cam2.myImage, out stationStatus[RotaryIdxAtCam].CCD2LightFalse, 1, 1);
                     string tSCCD2LightFalse = DateTime.Now.ToString("yyMMddHHmmss");
                     string FNCCD2LightFalse = path + "\\" + stationStatus[RotaryIdxAtCam].Unit2Barcode + "_" + tSCCD2LightFalse + "_" + Para.Cam2ExposureTime1.ToString() + "_2_B" + ".bmp";
                     HOperatorSet.WriteImage(Cam2.myImage, "bmp", 0, FNCCD2LightFalse);  //zhe ge ky?
                     //20161214@Brando
                 }
                //20170216
                 if (Para.SampleShape == 0)
                 {
                     HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].inspCCD2LightTrue, out stationStatus[RotaryIdxAtCam].inspCCD2LightTrueRotate,
                                                   HomMat2DRotate, "constant", "true");
                     HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].CCD2LightFalse, out stationStatus[RotaryIdxAtCam].CCD2LightFalseRotate,
                                                   HomMat2DRotate, "constant", "true");
                     HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].CCD2LightTrue, out stationStatus[RotaryIdxAtCam].CCD2LightTrueRotate,
                                                   HomMat2DRotate, "constant", "true");
                     HOperatorSet.AffineTransImage(stationStatus[RotaryIdxAtCam].inspCCD2LightFalse, out stationStatus[RotaryIdxAtCam].inspCCD2LightFalseRotate,
                                                   HomMat2DRotate, "constant", "true");
                 }
                 else
                 {
                     HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].inspCCD2LightTrue, out stationStatus[RotaryIdxAtCam].inspCCD2LightTrueRotate, 1, 1);
                     HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].CCD2LightFalse, out stationStatus[RotaryIdxAtCam].CCD2LightFalseRotate, 1, 1);
                     HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].CCD2LightTrue, out stationStatus[RotaryIdxAtCam].CCD2LightTrueRotate, 1, 1);
                     HOperatorSet.CopyObj(stationStatus[RotaryIdxAtCam].inspCCD2LightFalse, out stationStatus[RotaryIdxAtCam].inspCCD2LightFalseRotate, 1, 1);
                 }
                //20170216
                 HObject hV_reducedImg, ho_saveImg;
                 HObject ho_Rectangle;
                 HOperatorSet.GenEmptyObj(out hV_reducedImg);

                mainWnd.DisplayCam2Result(stationStatus[RotaryIdxAtCam].mod2VisResult);

                if (Para.SampleShape == 0)
                {
                    HOperatorSet.GenRectangle2(out ho_Rectangle, stationStatus[RotaryIdxAtCam].mod2VisResult.Y, stationStatus[RotaryIdxAtCam].mod2VisResult.X,
                                            0, stationStatus[RotaryIdxAtCam].mod2VisResult.Width / 2, stationStatus[RotaryIdxAtCam].mod2VisResult.Height / 2);
                }
                else
                {
                    HOperatorSet.GenCircle(out ho_Rectangle, stationStatus[RotaryIdxAtCam].mod2VisResult.Y, stationStatus[RotaryIdxAtCam].mod2VisResult.X,
                                            stationStatus[RotaryIdxAtCam].mod2VisResult.Radius);
                }

                HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].inspCCD2LightTrueRotate, ho_Rectangle, out hV_reducedImg);
                HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                string timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                string FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit2Barcode + "_" + timeStr01 + "_" + exposureTimeRecord + "_4_B" + ".bmp";
               // HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);  //zhe ge ky?
                HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].inspCCD2LightTrue, 1, 1);

                //if (Para.Enb45DegTest)
                if(true)
                {
                    HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].CCD2LightFalseRotate, ho_Rectangle, out hV_reducedImg);
                    HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                    timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                    FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit2Barcode + "_" + timeStr01 + "_" + Para.Cam2ExposureTime1.ToString() + "_1_B" + ".bmp";
                    //HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);
                    HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].CCD2LightFalse, 1, 1);

                    HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].CCD2LightTrueRotate, ho_Rectangle, out hV_reducedImg);
                    HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                    timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                    FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit2Barcode + "_" + timeStr01 + "_" + Para.Cam2ExposureTime1.ToString() + "_2_B" + ".bmp";
                    //HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);
                    HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].CCD2LightTrue, 1, 1);

                    HOperatorSet.ReduceDomain(stationStatus[RotaryIdxAtCam].inspCCD2LightFalseRotate, ho_Rectangle, out hV_reducedImg);
                    HOperatorSet.CropDomain(hV_reducedImg, out ho_saveImg);
                    timeStr01 = DateTime.Now.ToString("yyMMddHHmmss");
                    FileName0 = path + "\\" + stationStatus[RotaryIdxAtCam].Unit2Barcode + "_" + timeStr01 + "_" + exposureTimeRecord + "_3_B" + ".bmp";
                    //HOperatorSet.WriteImage(ho_saveImg, "bmp", 0, FileName0);
                    HOperatorSet.CopyObj(ho_saveImg, out stationStatus[RotaryIdxAtCam].inspCCD2LightFalse, 1, 1);
                }
            }
            isCam2InspReady = true;
            DateTime timeInspectMod2Unit2 = DateTime.Now;
            logg.calculateTime(timeInspectMod2Unit2,timeInspectMod2Unit1,"InspectMod2Unit");
            return 0;
        }
        private int InspectUnitError(int res)
        {
            switch (res)
            {
                case -1:
                    //MessageBox.Show("Camera 1 Grab Image Fail.", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("相机 1 抓图失败！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -2:
                    //MessageBox.Show("Vision Inspection Fail At Module 1.", "Main Sequence Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    //if (MessageBox.Show("Vision Inspection Fail At Module 1. Click Yes To Retry, Click No to Skip.", "Main Sequence Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                    if (MessageBox.Show("模块 1 视觉检测失败，点击Yes重试，点击No跳过！", "主线程错误！", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                    {
                        int RotaryIdxAtCam = GetIndexOfCam();
                        stationStatus[RotaryIdxAtCam].isUnitLoad1 = false;
                    }
                    break;
                case -3:
                    MessageBox.Show("相机 2 抓图失败！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -4:
                    //MessageBox.Show("Vision Inspection Fail At Module 2.", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (MessageBox.Show("模块 2 视觉检测失败，点击Yes重试，点击No跳过！", "主线程错误！", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                    {
                        int RotaryIdxAtCam = GetIndexOfCam();
                        stationStatus[RotaryIdxAtCam].isUnitLoad2 = false;
                    }
                    break;
                case -5://Alvin 2/3/17
                    //MessageBox.Show("Camera 1 Grab Image Fail.", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("相机 1 旋转抓图失败！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TestCameraSeq.JumpIndex(2);
                    return 99;
                    break;
                case -6://Alvin 2/3/17
                    //MessageBox.Show("Camera 1 Grab Image Fail.", "Main Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("相机 2 旋转抓图失败！", "主线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TestCamera2Seq.JumpIndex(2);
                    return 99;
                    break;
                case -8:
                    logg.saveerrorLog("Mod1_LED_error");
                    MessageBox.Show("Mod1LED输出与输入不一致");
                    break;
                case -11:
                    logg.saveerrorLog("Mod2_LED_error");
                    MessageBox.Show("Mod2LED输出与输入不一致");
                    break;
            }
            return 0;
        }
        private int SetInspectionDone()
        {
            isInspectionReady = true;
            return 0;
        }
        #endregion

        #region Test Station Sequencing Functions

        private int GetIndexOfTestStation()
        {
            int res = Para.CurrentRotaryIndex -2;
            if (res < 0)
                res = (res+4);

            return res;
        }
        private int TestStationWaitUnitReady()
        {
            if (!isTestStationRotaryIndexDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                testSeq.JumpIndex(0);
                return 99; //To Jump Index
            }
            isTestStationRotaryIndexDone = false;            

            int RotaryIdxAtTest = GetIndexOfTestStation();//CurRotIdx -1;            

            if((!stationStatus[RotaryIdxAtTest].isUnitLoad1) && (!stationStatus[RotaryIdxAtTest].isUnitLoad2))
            {
                camSeq.JumpIndex(0);
                return 99; //To Jump Index
            }
            if (Para.DisableSpecTest)
            {
                camSeq.JumpIndex(0);
                return 99; //To Jump Index
            }

            isTestStationReady = false;
            return 0;
        }
        public int TestStationSetStartTest()
        {
            //Dark Reference
            //specMgr.SetIO(0, false);
            //Thread.Sleep(1500);
            int myIdx = GetIndexOfTestStation();
            //if (Para.isOutShutter)
            //{
            //    specMgr.SetIO(0, false);
            //    specMgr.SetIO(1, false);
            //    if (myMotionMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS))
            //    {
            //        myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
            //        Thread.Sleep(500);
            //    }
            //}
            //else
            //{
            //    if (!myMotionMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS))
            //    {
            //        myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
            //        Thread.Sleep(500);
            //    }
            //    specMgr.SetIO(0, true);
            //    specMgr.SetIO(1, true);
            //    Thread.Sleep(500);
            //}

            //if (stationStatus[myIdx].isUnitLoad1)
            //{
            //    mainWnd.UpdateMod1TestStatus("Dark Test", Color.Lime);
            //    stationStatus[myIdx].DarkRefMod1 = specMgr.GetCount(0);
            //    stationStatus[myIdx].WLMod1Dark = specMgr.GetWaveLength(0);
            //    mainWnd.UpdateMod1Chart(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].DarkRefMod1, false);

            //}
            ////specMgr.SetIO(1, false);
            //if (stationStatus[myIdx].isUnitLoad2)
            //{
            //    mainWnd.UpdateMod2TestStatus("Dark Test", Color.Lime);
            //    stationStatus[myIdx].DarkRefMod2 = specMgr.GetCount(1);
            //    stationStatus[myIdx].WLMod2Dark = specMgr.GetWaveLength(1);
            //    mainWnd.UpdateMod2Chart(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].DarkRefMod2, false);
            //    //Para.TotalTestUnit = Para.TotalTestUnit + 1;             
            //}
            //if (Para.isOutShutter)
            //{
            //    myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
            //    Thread.Sleep(1500);
            //}
            //else
            //{
            //    specMgr.SetIO(0, false);
            //    specMgr.SetIO(1, false);
            //}

            mod1StartTest = true;
            mod2StartTest = true;
            mod1TestEnd = false;
            mod2TestEnd = false;

            if (!stationStatus[myIdx].isUnitLoad1)
                mod1TestEnd = true;

            if (!stationStatus[myIdx].isUnitLoad2)
                mod2TestEnd = true;

            return 0;
        }
        public int TestStationWaitModTestEnd()
        {
            if ((!mod1TestEnd) || (!mod2TestEnd))
            {
                Thread.Sleep(50);
                Application.DoEvents();
                testSeq.JumpIndex(testSeq.CurrentIndex());
                return 99; //To Jump Index
            }
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
            return 0;
        }

        public float CalTranData(float dData, float wData, float counts)
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
                myTransRatio.Add(CalTranData(darkRef[i],whiteRef[i],curCnt[i]));
            return myTransRatio;
        }
        private int Mod1WaitStartOfTest()
        {
            if ((!mod1StartTest))
            {
                Thread.Sleep(50);
                Application.DoEvents();
                testMod1Seq.JumpIndex(testMod1Seq.CurrentIndex());
                return 99; //To Jump Index
            }
            return 0;
        }
        private int SetMod1TestDone()        
        {
            mod1TestEnd = true;
            mod1StartTest = false;
            return 0;
        }
        private int Mod2WaitStartOfTest()
        {
            if ((!mod2StartTest))
            {
                Thread.Sleep(50);
                Application.DoEvents();
                testMod2Seq.JumpIndex(testMod2Seq.CurrentIndex());
                return 99; //To Jump Index
            }
            return 0;
        }
        private int SetMod2TestDone()
        {
            mod2TestEnd = true;
            mod2StartTest = false;
            return 0;
        }

        private int TestModuleError(int result)
        {
            switch (result)
            {
                case -1:
                    //MessageBox.Show("Module 1 Fail To Connect To Mac Mini.", "Test Module Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("模块 1 未能连接到Mac Mini !", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -2:
                    MessageBox.Show("模块 2 未能连接到Mac Mini !", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -3:
                    //MessageBox.Show("Module 1 Fail To Get White Reference Spectrum.", "Test Module Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("模块 1 未能获得白参考值或白参考饱和, 请打开光源，且降低积分时间！", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -4:
                    MessageBox.Show("模块 2 未能获得白参考值或白参考饱和, 请打开光源，且降低积分时间！", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -5:
                    //MessageBox.Show("Module 1 Y Axis Not Save To Move.", "Test Module Sequence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("模块 1 Y轴不在安全位！", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -6:
                    MessageBox.Show("模块 2 Y轴不在安全位！", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -7:
                    MessageBox.Show("Module 1 CGHost Recieved Reply Timeout", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -8:
                    MessageBox.Show("Module 2 CGHost Recieved Reply Timeout", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -9:
                    MessageBox.Show("Module 1 CGHost Send All Datas Not OK.", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -10:
                    MessageBox.Show("Module 2 CGHost Send All Datas Not OK.", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -11:
                    MessageBox.Show("运行时间超过24小时，请Dailycheck", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -12:
                    MessageBox.Show("运行时间超过8天，请ND", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -13:
                    MessageBox.Show("运行时间超过8天，请LS", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -14:
                    MessageBox.Show("Module 1 检测高度超出控制范围", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -15:
                    MessageBox.Show("Module 2 检测高度超出控制范围", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -16:
                    MessageBox.Show("Module 1 connect failed", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case -17:
                    MessageBox.Show("Module 1 connect failed", "测试模块线程错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            return 0;
        }

        private bool TestWhiteData(List<float> darkRef, List<float> whiteRef)
        {
            //Clovis
            //return true;
            float max = whiteRef.Max();
            ArrayList aa = new ArrayList(whiteRef);
            int firstindex = aa.IndexOf(max);
            int lastindex = aa.LastIndexOf(max);
            if (firstindex != lastindex)
                return false;

            for (int i = 0; i < darkRef.Count; i++)
            {
                if ((i < 50) || (i > 900))
                    continue;
                if (((whiteRef[i] - darkRef[i]) < 0) || (whiteRef[i]>60000))
                    return false;
            }
            return true;
        }

        private int LoadAndCheckLS(int Idx, int startWL, int endWL, List<float> whiteRef, List<float> WLength) //Idx:1 means LS file's first colum,4 means LS file's fourth colum
        {
            //
            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            string LSfilePath = exePath + "LS_Setting.xml";

            if (!Directory.Exists(CalibrationFolder))
            {
                MessageBox.Show("Calibration Folder Not Found.", "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            string fileName = CalibrationFolder + "LS_Ref_Data.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("ND Calibration File Not Found." + fileName, "Calibration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
            string strread = "";
            StreamReader sr = new StreamReader(fileName);
            string line;
            string strLS = "";
            string[] row;
            int Rowcnt = 0;
            int t = 0;
            int s = 0;
            int tempWL = 0;
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

                    CalibrationInfo Mod1Cal = new CalibrationInfo();
                    Mod1Cal.waveLength = int.Parse(row[0]);
                    tempWL = (Mod1Cal.waveLength - 400) % 50;
                    if (tempWL == 0)
                    {
                        for (int i = startWL; i <= endWL; i = i + 50)
                        {
                            if ((Mod1Cal.waveLength == i))
                            {
                                t = NearestIndex(0, i, WLength);
                                s = t;
                                double tempPer = (whiteRef[s] - double.Parse(row[Idx])) / (double.Parse(row[Idx]));
                                FileOperation.ReadData(LSfilePath, "UpLimit", "LS" + i.ToString(), ref strLS);
                                double tempUpValue = double.Parse(strLS);
                                FileOperation.ReadData(LSfilePath, "LowLimit", "LS" + i.ToString(), ref strLS);
                                double tempLowValue = double.Parse(strLS);
                                if (tempPer > tempUpValue/100 || tempPer < tempLowValue/100)
                                {
                                    return 7;
                                }
                            }
                        }
                    }                         
            }
            return 0;
            //s = 0;
            //for (i = stWaveLength; i <= endWaveLength; i++)
            //{
            //    t = NearestIndex(s, i, WL);
            //    s = t;
            //    columnValue += TransData[t].ToString("F4") + ",";
            //}
            //sw.WriteLine(columnValue);
        }

        private int TestMod1()
        {            
            if (!Para.MachineOnline)
            {
                Thread.Sleep(3000);
                return 0;
            }

            int myIdx = GetIndexOfTestStation();

            if (!stationStatus[myIdx].mod1VisResult.Found)
            {
                return 0;
            }

            if (!stationStatus[myIdx].isUnitLoad1)
            {
                return 0;
            }

            //xsm
            //
            double height = 0.0;
            if (Para.myMain.DLRS1.Enable)
            {
                for (int i = 0; i < 3; i++)
                {
                    height = Para.myMain.DLRS1.Read();
                }
                mainWnd.UpdateMod1Height((height - Para.myMain.DLRS1.OriginValue).ToString("F3"));
                if ((height < (Para.myMain.DLRS1.OriginValue - Para.myMain.DLRS1.Lowlimited)) || (height > (Para.myMain.DLRS1.OriginValue + Para.myMain.DLRS1.Uplimited)))
                    return -14;
            }


            if (Para.EnableCGHost)
            { 
                 DateTime timeConnect1 = DateTime.Now;
                 DateTime sttTime = DateTime.Now;
                 if (Para.myMain.DLRS1.Enable)
                 {
                     AComMod1.Connect(AComMod1.IP, AComMod1.Port);
                     AComMod1.SendStartTest11(1 + (myIdx * 2), stationStatus[myIdx].Unit1Barcode, Para.LightSourceType, "CAS_" + Para.Spectrometer1SN, height - Para.myMain.DLRS1.OriginValue);
                 }
                 else
                 {
                     AComMod1.Connect(AComMod1.IP, AComMod1.Port);
                     AComMod1.SendStartTest(1 + (myIdx * 2), stationStatus[myIdx].Unit1Barcode, Para.LightSourceType, "CAS_" + Para.Spectrometer1SN);
                 }

                DateTime timeConnect2 = DateTime.Now;
                logg.calculateTime(timeConnect2,timeConnect1,"rcst1");               
               
                //20161108

            }
            if (Para.isOutShutter)
            {
                if (myMotionMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS))
                {
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
                    Thread.Sleep(100);
                }
            }
            else
            {
                specMgr.SetIO(0, true);
                Thread.Sleep(100);
            }
            mainWnd.UpdateMod1TestStatus("Dark Test", Color.Lime);
            specMgr.SetAverage(0, 3);
            stationStatus[myIdx].DarkRefMod1 = specMgr.GetCount(0);
            stationStatus[myIdx].WLMod1Dark = specMgr.GetWaveLength(0);
            mainWnd.UpdateMod1Chart(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].DarkRefMod1, false);
            Thread.Sleep(100);

            //Move To White Reference Position
            int motionRes = 0;
            //myMotionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[1].X);
            DateTime timeYtoWhiteMod = DateTime.Now;
            motionRes = myMotionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[1].Y);
            if (motionRes != 0)
                return -5;

            //myMotionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            myMotionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
            logg.calculateTime(DateTime.Now, timeYtoWhiteMod, "YtoWhite"); 

            //White Reference
            //specMgr.SetIO(0, true);
            DateTime stTime = DateTime.Now;
            mainWnd.UpdateMod1TestStatus("White Test", Color.Lime);
            if (Para.isOutShutter)
            {
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
                Thread.Sleep(100);
            }
            else
            {
                specMgr.SetIO(0, false);
                Thread.Sleep(100);
            }
            Thread.Sleep(100);
            bool whiteDataOK = false;
            for (int i = 0; i < 5; i++)//from 3 to 2
            {
                specMgr.SetAverage(0, 10);
                stationStatus[myIdx].WhiteRefMod1 = specMgr.GetCount(0);
                //stationStatus[myIdx].WLMod1White = specMgr.GetWaveLength(0);
                if (TestWhiteData(stationStatus[myIdx].DarkRefMod1,stationStatus[myIdx].WhiteRefMod1))
                {
                    whiteDataOK = true;
                    break;
                }
            }
            logg.calculateTime(DateTime.Now, stTime, "White Test1");
            TimeSpan time_span = DateTime.Now - stTime;           

            //if (!whiteDataOK)
            //    return -3;

            mainWnd.UpdateMod1Chart(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].WhiteRefMod1, false);

            if (!whiteDataOK)
                return -3;
            //2016010711
            string tempTimestring = DateTime.Now.ToString("hh");
            if (Para.timeStip1 != tempTimestring)
            {
                SaveLSValue(1, 400, 1100, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].WLMod1Dark);
                Para.timeStip1 = tempTimestring;
            }
           int restVal =  LoadAndCheckLS(1, 400, 1100, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].WLMod1Dark);
           if (restVal != 0)
           {
               mainWnd.UpdateMod1TestStatus("光源波动太大，联系工程师", Color.Red);
               return 0;
           }
            //2016010711

            //string timeEndWhiteStr = DateTime.Now.ToString("hh-mm-ss-fff");
            //mainWnd.WriteOperationinformation("End White test:" + timeEndWhiteStr);//20161208

            //Unit Ctr Offset In MM
            double unitCtrOffX = (((Cam1.ImageWidth / 2) - stationStatus[myIdx].mod1VisResult.X) * Cam1.CaliValue.X) + Para.Module[0].CamToOriginOffset.X;
            double unitCtrOffY = (((Cam1.ImageHeight / 2) - stationStatus[myIdx].mod1VisResult.Y) * Cam1.CaliValue.Y) + Para.Module[0].CamToOriginOffset.Y;

            System.Drawing.PointF CtrPt = new System.Drawing.PointF();
            CtrPt.X = (Cam1.ImageWidth / 2);
            CtrPt.Y = (Cam1.ImageHeight / 2);

            //System.Drawing.PointF RefPoint = new System.Drawing.PointF();
            //RefPoint.X = (float)((Cam1.ImageWidth / 2) - (stationStatus[myIdx].mod1VisResult.Length / 2));
            //RefPoint.Y = (float)((Cam1.ImageHeight / 2) - (stationStatus[myIdx].mod1VisResult.Width / 2));            
            List<DPoint> XY = new List<DPoint>();
            List<DPoint>OrgPos = new List<DPoint>();
            List<DPoint>OffPos = new List<DPoint>();
               DateTime timeSPECTRO = DateTime.Now;
            for (int i = 0; i < Para.Module[0].TestPt.Count; i++)
            {
                if (Para.Enb3TestPtOnly)
                {
                    //if (Para.SampleShape == 0)
                    //{
                    //    if ((i % 2) != 0)
                    //    {
                    //        XY.Add(new DPoint(0, 0));
                    //        continue;
                    //    }
                    //}
                    //else
                    //{
                        if ((i == 0) || (i == 4))
                        {
                            XY.Add(new DPoint(0, 0));
                            continue;
                        }
                    //}//20170211
                }
                DateTime ptStTime = DateTime.Now;

                mainWnd.UpdateMod1TestStatus("Start Test Point " + (i + 1).ToString(), Color.Lime);

                System.Drawing.PointF tpt1 = new System.Drawing.PointF(); // test pt in Pixel 
                tpt1.X = (float)(CtrPt.X + (Para.Module[0].TestPt[i].X / Cam1.CaliValue.X));
                tpt1.Y = (float)(CtrPt.Y + (Para.Module[0].TestPt[i].Y / Cam1.CaliValue.Y));

                Helper.ApplyRotation(ref tpt1, -(float)stationStatus[myIdx].mod1VisResult.Angle, CtrPt);

                DPoint tpt1Offset = new DPoint();
                
                tpt1Offset.X = (tpt1.X - CtrPt.X) * Cam1.CaliValue.X;
                tpt1Offset.Y = (tpt1.Y - CtrPt.Y) * Cam1.CaliValue.Y;

                OrgPos.Add(new DPoint(Para.Module[0].TeachPos[0].X + Para.Module[0].TestPt[i].X, Para.Module[0].TeachPos[0].Y + Para.Module[0].TestPt[i].Y));
                OffPos.Add(new DPoint(Para.Module[0].TeachPos[0].X + tpt1Offset.X + unitCtrOffX, Para.Module[0].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY));

                //mainWnd.UpdateMod1TestStatus("Move To Test Point " + (i + 1).ToString(), Color.Lime);
                stTime = DateTime.Now;
                XY.Add(new DPoint(stationStatus[myIdx].mod1VisResult.X, stationStatus[myIdx].mod1VisResult.Y));

                if (i == 4)
                {
                    mainWnd.WriteOperationinformation("Test1" + (i + 1).ToString() + "PosX:" + (Para.Module[0].TeachPos[0].X + tpt1Offset.X + unitCtrOffX).ToString() + " PosY:" + (Para.Module[0].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY).ToString());
                }

                myMotionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[0].X + tpt1Offset.X + unitCtrOffX);
                motionRes = myMotionMgr.MoveTo((ushort)Axislist.Mod1YAxis, Para.Module[0].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY);
                if (motionRes != 0)
                    return -5;
                myMotionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
                myMotionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);
                logg.calculateTime(DateTime.Now, stTime, "Mod1 Point"+i.ToString()+" Motion");


                time_span = DateTime.Now - stTime;
               // mainWnd.UpdateMod1TestStatus("Move To Test Point " + (i + 1).ToString() + " Done " + time_span.TotalMilliseconds.ToString() + "ms", Color.Lime);

                //mainWnd.UpdateMod1TestStatus("Start Collect Spectrum Test Point " + (i + 1).ToString(), Color.Lime);
                stTime = DateTime.Now;
                stationStatus[myIdx].MeasDataMod1[i] = specMgr.GetCount(0);
                //stationStatus[myIdx].MeasWLMod1[i] = specMgr.GetWaveLength(0);
                logg.calculateTime(DateTime.Now, stTime, "Mod1 Point" + i.ToString() + " GetCount");
                mainWnd.UpdateMod1Chart(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].MeasDataMod1[i], false);
                time_span = DateTime.Now - stTime;
                //mainWnd.UpdateMod1TestStatus("End Collect Spectrum Test Point " + (i + 1).ToString() +" "+ time_span.TotalMilliseconds.ToString() + "ms", Color.Lime);

                //mainWnd.UpdateMod1TestStatus("Start Calculating And Saving Test Point " + (i + 1).ToString(), Color.Lime);
                stTime = DateTime.Now;
                stationStatus[myIdx].transRatioMod1[i] = CalculateTransRatio(stationStatus[myIdx].DarkRefMod1, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].MeasDataMod1[i]);

                mainWnd.UpdateMod1Chart(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].transRatioMod1[i], true);

                SaveRefDarkTransData(stationStatus[myIdx].Unit1Barcode, 400, 1100, i + 1, stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].DarkRefMod1,
                                     stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].MeasDataMod1[i], stationStatus[myIdx].transRatioMod1[i]);

                SaveRawData(stationStatus[myIdx].Unit1Barcode, 400, 1100, i + 1, stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].WLMod1Dark,
                    stationStatus[myIdx].DarkRefMod1, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].MeasDataMod1[i], 1, stationStatus[myIdx].mod1VisResult);
                logg.calculateTime(DateTime.Now, stTime, "Mod1 Point" + i.ToString() + " CalTrans And SaveData");

                time_span = DateTime.Now - ptStTime;
                mainWnd.UpdateMod1TestStatus("Test Point " + (i + 1).ToString() + " Done ", Color.Lime);
                string timeEndPointStr = DateTime.Now.ToString("hh-mm-ss-fff");
                mainWnd.WriteOperationinformation("Test1 Point " + (i + 1).ToString() + "End:" + timeEndPointStr);
            }
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
            //logg.calculateTime(DateTime.Now, timeSPECTRO, "Spectrograph1");
            //specMgr.SetIO(0, true);
            //DPoint VisRes = new DPoint(stationStatus[myIdx].mod1VisResult.X,stationStatus[myIdx].mod1VisResult.Y);
            //SaveOffsetData(stationStatus[myIdx].Unit1Barcode, new DPoint(CtrPt.X, CtrPt.Y), VisRes, stationStatus[myIdx].mod1VisResult.Angle, OrgPos[0], OrgPos[1], OrgPos[2], OrgPos[3], OrgPos[4], OffPos[0], OffPos[1],
            //                OffPos[2], OffPos[3], OffPos[4], 1);
            
            mainWnd.UpdateMod1TestStatus("Sending Test Result", Color.Orange);
            string tSSendImageAndTrans = DateTime.Now.ToString("hh-mm-ss-fff");
            mainWnd.WriteOperationinformation("Sending Result Start1:" + tSSendImageAndTrans);
            DateTime ToOrigin = DateTime.Now;
            myMotionMgr.MoveTo((ushort)Axislist.Mod1XAxis, Para.Module[0].TeachPos[0].X);
            myMotionMgr.MoveTo((ushort)Axislist.Mod1YAxis, 0);
            if (Para.EnableCGHost)
            {
                int ALSExpTime = 0;

                if (Para.DisableBarcode)
                {
                    int barcodeLength = stationStatus[myIdx].Unit1Barcode.Length;
                    string blackorwhite = stationStatus[myIdx].Unit1Barcode.Substring(barcodeLength-1, 1);
                    if (blackorwhite != "3")//Black
                    {
                        ALSExpTime = Para.Cam1ExposureTimeW;
                    }
                    else
                    {
                        ALSExpTime = Para.Cam1ExposureTimeB;
                    }
                }
                else
                {
                    ALSExpTime = Para.Cam1ExposureTimeB;
                }
                //Thread.Sleep(300);
                //AComMod1.SendImageAdvance(Cam1.ImageWidth, Cam1.ImageHeight, stationStatus[myIdx].Mod1Exp1DarkImage, stationStatus[myIdx].Mod1Exp1Image,
                //                                                            stationStatus[myIdx].Mod1DarkImage, stationStatus[myIdx].Mod1Image, Para.Cam1ExposureTime1, ALSExpTime);

                //if (Para.Enb45DegTest)
                //string timeStartSendImageAndTrans = DateTime.Now.ToString("hh-mm-ss-fff");
                //mainWnd.WriteOperationinformation("Start Send Image And Data:" + timeStartSendImageAndTrans);
                if (true)//20161202
                {
                    int ImgWidth = GetImageWidth(stationStatus[myIdx].CCD1LightFalse);
                    int ImgHgt = GetImageHeight(stationStatus[myIdx].CCD1LightFalse);
                    Byte[] Exp1Dark = GetImageByte(stationStatus[myIdx].CCD1LightFalse);
                    Byte[] Exp1White = GetImageByte(stationStatus[myIdx].CCD1LightTrue);
                    Byte[] ALSDark = GetImageByte(stationStatus[myIdx].inspCCD1LightFalse);
                    Byte[] ALSWhite = GetImageByte(stationStatus[myIdx].inspCCD1LightTrue);
                    //int ImgWidth = GetImageWidth(stationStatus[myIdx].inspCCD1LightTrue);
                    //int ImgHgt = GetImageHeight(stationStatus[myIdx].inspCCD1LightTrue);
                    //Byte[] Exp1Dark = GetImageByte(stationStatus[myIdx].inspCCD1LightTrue);
                    //Byte[] Exp1White = GetImageByte(stationStatus[myIdx].inspCCD1LightTrue);
                    //Byte[] ALSDark = GetImageByte(stationStatus[myIdx].inspCCD1LightTrue);
                    //Byte[] ALSWhite = GetImageByte(stationStatus[myIdx].inspCCD1LightTrue);
                    DateTime timeSend1 = DateTime.Now;
                    //AComMod1.SendImageAdvance(ImgWidth, ImgHgt, Exp1Dark, Exp1White, ALSDark, ALSWhite, Para.Cam1ExposureTime1, ALSExpTime);
                    AComMod1.SendImageAdvance(ImgWidth, ImgHgt, Exp1Dark, Exp1White, ALSDark, ALSWhite, Para.Cam1ExposureTime1, ALSExpTime, Para.CenWL1, Para.PixDen1, Para.BeamSize1);//20161220@Brando
                    logg.calculateTime(DateTime.Now, timeSend1, "SendImageAdvance1");
                    DateTime timefinu1 = DateTime.Now;
                    if (!AComMod1.WaitTestReply("finu"))
                    {
                        Para.myMain.WriteOperationinformation("Module 1 Recieve finu Reply Timeout ");
                        return -7;
                    }
                    logg.calculateTime(DateTime.Now, timefinu1, "finu1");
                    string timeStartSendImageAndTrans = DateTime.Now.ToString("hh-mm-ss-fff");
                    mainWnd.WriteOperationinformation("Sending Image Finished1:" + timeStartSendImageAndTrans);
                }
                else
                {
                    DateTime timeSend1 = DateTime.Now;
                    int ImgWidth = GetImageWidth(stationStatus[myIdx].inspCCD1LightTrue);
                    int ImgHgt = GetImageHeight(stationStatus[myIdx].inspCCD1LightTrue);
                    Byte[] ALSWhite = GetImageByte(stationStatus[myIdx].inspCCD1LightTrue);
                    AComMod1.SendImage(ImgWidth, ImgHgt,ALSWhite, new DPoint(stationStatus[myIdx].mod1VisResult.X, stationStatus[myIdx].mod1VisResult.Y), stationStatus[myIdx].mod1VisResult.Angle);
                    logg.calculateTime(DateTime.Now, timeSend1, "SendImage1");
                    Thread.Sleep(1000);
                    //DateTime timefinu1 = DateTime.Now;
                    //if (!AComMod1.WaitTestReply("finu"))
                    //{
                    //    Para.myMain.WriteOperationinformation("Module 1 Recieve finu Reply Timeout ");
                    //    return -7;
                    //}
                    //logg.calculateTime(DateTime.Now, timefinu1, "finu1");
                }                                

            //}

            DateTime timeSend2 = DateTime.Now;
            if (Para.Enb3TestPtOnly)
            {
                //AComMod1.SendAdvanTestResult(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].DarkRefMod1, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].MeasDataMod1[0], stationStatus[myIdx].MeasDataMod1[0],
                //                        stationStatus[myIdx].MeasDataMod1[2], stationStatus[myIdx].MeasDataMod1[2], stationStatus[myIdx].MeasDataMod1[4], XY, stationStatus[myIdx].mod1VisResult.Angle);
                
                //if (Para.SampleShape == 0)
                //{
                //    AComMod1.SendAdvanTestResultFor3Point(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].DarkRefMod1, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].MeasDataMod1[0],
                //                            stationStatus[myIdx].MeasDataMod1[2], stationStatus[myIdx].MeasDataMod1[4], XY, stationStatus[myIdx].mod1VisResult.Angle);
                //}
                //else
                //{
                    AComMod1.SendAdvanTestResultFor3Point(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].DarkRefMod1, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].MeasDataMod1[1],
                                            stationStatus[myIdx].MeasDataMod1[2], stationStatus[myIdx].MeasDataMod1[3], XY, stationStatus[myIdx].mod1VisResult.Angle);
                //}
            }
            else
            {
                AComMod1.SendAdvanTestResult(stationStatus[myIdx].WLMod1Dark, stationStatus[myIdx].DarkRefMod1, stationStatus[myIdx].WhiteRefMod1, stationStatus[myIdx].MeasDataMod1[0], stationStatus[myIdx].MeasDataMod1[1],
                                        stationStatus[myIdx].MeasDataMod1[2], stationStatus[myIdx].MeasDataMod1[3], stationStatus[myIdx].MeasDataMod1[4], XY, stationStatus[myIdx].mod1VisResult.Angle);
            }
            string timeStartSendData = DateTime.Now.ToString("hh-mm-ss-fff");
            mainWnd.WriteOperationinformation("Sending Data Finished1:" + timeStartSendData);
            //logg.calculateTime(DateTime.Now, timeSend2, "SendAdvanTestResult1");
            //DateTime timetsta1 = DateTime.Now;
            //if (!AComMod1.WaitTestReply("tsta"))
            //{
            //    Para.myMain.WriteOperationinformation("Module 1 Recieve tsta Reply Timeout ");
            //    return -7;
            //}
            //logg.calculateTime(DateTime.Now, timetsta1, "tsta1");
            }

            myMotionMgr.WaitAxisStop((ushort)Axislist.Mod1XAxis);
            myMotionMgr.WaitAxisStop((ushort)Axislist.Mod1YAxis);//20161207
            

            if (Para.EnableCGHost)
            {
                Thread.Sleep(100);
                DateTime timeendtest = DateTime.Now;
                string timeStartSendTestEnd = DateTime.Now.ToString("hh-mm-ss-fff");
                mainWnd.WriteOperationinformation("Send Testend:" + timeStartSendTestEnd);
                int res = AComMod1.SendEndTest("Trans");
                timeStartSendTestEnd = DateTime.Now.ToString("hh-mm-ss-fff");
                mainWnd.WriteOperationinformation("Recive Testend:" + timeStartSendTestEnd);
              
                if (res ==-1)
                {
                    Para.myMain.WriteOperationinformation("Module 1 Recieve End Test Reply Timeout ");
                    return -7; //Timeout
                }

                if (res ==0)
                    stationStatus[myIdx].IsMod1UnitPassed = true;
                else
                    stationStatus[myIdx].IsMod1UnitPassed = false;

                Para.errorCode1Array[myIdx] = AComMod1.errorCode.ToString();
                Para.errorStr1Array[myIdx] = AComMod1.errorString;
                if (Para.EnBin_Code)
                    Para.bin_Code1Array[myIdx] = AComMod1.bin_Code;//20170222
                else
                    Para.bin_Code1Array[myIdx] = "";
                if (AComMod1.returnCode != "dest")
                {
                    Para.myMain.WriteOperationinformation("Module 1 send all datas NOT OK. ");
                    return -9;
                }
                //logg.calculateTime(DateTime.Now, timeendtest,"dest1");
                //if (stationStatus[myIdx].IsMod1UnitPassed)
                //    //logg.saveLog(stationStatus[myIdx].Unit1Barcode, "Pass");
                //else
                //    //logg.saveLog(stationStatus[myIdx].Unit1Barcode, "Fail");
               
                AComMod1.Disconnect();
            }
            string timeTestEnd = DateTime.Now.ToString("hh-mm-ss-fff");
            //logg.saveCycleTime("Channel1", stationStatus[myIdx].Unit1Barcode,Para.startTime, timeTestEnd);
            //mainWnd.WriteOperationinformation("Test End:" + timeTestEnd);
            mainWnd.UpdateMod1TestStatus("Test End", Color.Orange);           
            Para.firstFinish = true;
            return 0;
        }

        private int GetImageWidth(HObject myImage)
        {
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(myImage, out pointer, out type, out width, out height);

            return width.I;
        }
        private int GetImageHeight(HObject myImage)
        {
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(myImage, out pointer, out type, out width, out height);

            return height.I;
        }
        private unsafe byte[] GetImageByte(HObject myImage)
        {
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(myImage, out pointer, out type, out width, out height);
            byte* p = (byte*)pointer[0].L;

            int ImgWidth = width.I;
            int ImgHgt = height.I;
            int size = ImgWidth * ImgHgt;
            Byte[] Exp1Dark = new Byte[size];
            Marshal.Copy((IntPtr)p, Exp1Dark, 0, size);

            return Exp1Dark;
        }
        private void SaveLSValue(int Idx, int startWL, int endWL, List<float> whiteRef, List<float> WLength)
        {
            string exePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string CalibrationFolder = exePath + "CalibrationFiles\\";

            if (!Directory.Exists(CalibrationFolder))
            {
                Directory.CreateDirectory(CalibrationFolder);
            }
            string fileName = CalibrationFolder + "Module" + Idx.ToString() + "_LS_Every_Hour_Data.csv";
            FileStream objFileStream;
            bool bCreatedNew = false;

            if (!File.Exists(fileName))
            {
                objFileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                bCreatedNew = true;
            }
            else
            {
                objFileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            }
            StreamWriter sw = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
            string columnTitle = "";
            string columnValue = "";
            int t = 0;
            double tempCounts = 0;
            try
            {
                int s = 0;
                //columnTitle = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ",";
                //sw.WriteLine(columnTitle);
                //for (int i = startWL; i <= endWL; i = i + 50)
                //{
                //    t = NearestIndex(s, i, WLength);
                //    s = t;
                //    tempCounts = whiteRef[t];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
                //    columnValue = i.ToString() + "," + tempCounts.ToString("F4") + ",";
                //    sw.WriteLine(columnValue);
                //}
                columnValue = ",";
                if (bCreatedNew)
                {
                    for (int i = startWL; i <= endWL; i = i + 50)
                    {
                        //t = NearestIndex(s, i, WLength);
                        //s = t;
                        //tempCounts = whiteRef[t];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
                        columnValue += i.ToString() + ",";
                    }
                    sw.WriteLine(columnValue);
                }
                columnValue = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ",";
                //sw.WriteLine(columnTitle);
                for (int i = startWL; i <= endWL; i = i + 50)
                {
                    t = NearestIndex(s, i, WLength);
                    s = t;
                    tempCounts = whiteRef[t];// pub.m_transData[index, t];// pub.l_pSpectrum[index].Value[t];
                    columnValue += tempCounts.ToString("F4") + ",";
                }
                sw.WriteLine(columnValue);
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
        private int TestMod2()
        {
            if (!Para.MachineOnline)
            {
                Thread.Sleep(3000);
                return 0;
            }
            int myIdx = GetIndexOfTestStation();

            if (!stationStatus[myIdx].mod2VisResult.Found)
            {
                return 0;
            }

            if (!stationStatus[myIdx].isUnitLoad2)
            {
                return 0;
            }

            //xsm
            //
            double height = 0.0;
            if (Para.myMain.DLRS2.Enable)
            {
                for (int i = 0; i < 3; i++)
                {
                    height = Para.myMain.DLRS2.Read();
                }
                    mainWnd.UpdateMod2Height((height - Para.myMain.DLRS2.OriginValue).ToString("F3"));

                    if ((height < (Para.myMain.DLRS2.OriginValue - Para.myMain.DLRS2.Lowlimited)) || (height > (Para.myMain.DLRS2.OriginValue + Para.myMain.DLRS2.Uplimited)))
                        return -15;
            }

            if (Para.EnableCGHost)
            {

                DateTime c2 = DateTime.Now;

                if (Para.myMain.DLRS2.Enable)
                {
                    AComMod2.Connect(AComMod2.IP, AComMod2.Port);
                    AComMod2.SendStartTest11(2 + (myIdx * 2), stationStatus[myIdx].Unit2Barcode, Para.LightSourceType, "CAS_" + Para.Spectrometer2SN, height - Para.myMain.DLRS2.OriginValue);
                }
                else
                {
                    AComMod2.Connect(AComMod2.IP, AComMod2.Port);
                    AComMod2.SendStartTest(2 + (myIdx * 2), stationStatus[myIdx].Unit2Barcode, Para.LightSourceType, "CAS_" + Para.Spectrometer2SN);
                }

                logg.calculateTime(DateTime.Now,c2,"rcst2");                

            }
            if (Para.isOutShutter)
            {
                if (myMotionMgr.ReadIOOut((ushort)OutputIOlist.SpectrumLS))
                {
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
                    Thread.Sleep(100);
                }
            }
            else
            {
                specMgr.SetIO(1, true);
                Thread.Sleep(100);
            }
            mainWnd.UpdateMod2TestStatus("Dark Test", Color.Lime);
            specMgr.SetAverage(1, 3);
            stationStatus[myIdx].DarkRefMod2 = specMgr.GetCount(1);
            stationStatus[myIdx].WLMod2Dark = specMgr.GetWaveLength(1);
            mainWnd.UpdateMod2Chart(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].DarkRefMod2, false);
            Thread.Sleep(100);

            //Move To White Reference Position
            int motionRes = 0;
            //myMotionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[1].X);
            motionRes = myMotionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[1].Y);
            if (motionRes != 0)
                return -6;
            //myMotionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            myMotionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);

            //White Reference
            //specMgr.SetIO(1, true);
            mainWnd.UpdateMod2TestStatus("White Test", Color.Lime);

            DateTime stTime = DateTime.Now;
            if (Para.isOutShutter)
            {
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, true);
                Thread.Sleep(100);
            }
            else
            {
                specMgr.SetIO(1, false);
                Thread.Sleep(100);
            }
            Thread.Sleep(100);
            bool whiteDataOK = false;
            for (int i = 0; i < 3; i++)//from 3 to 2
            {
                specMgr.SetAverage(1, 10);
                stationStatus[myIdx].WhiteRefMod2 = specMgr.GetCount(1);
                //stationStatus[myIdx].WLMod2White = specMgr.GetWaveLength(1); 
                if (TestWhiteData(stationStatus[myIdx].DarkRefMod2, stationStatus[myIdx].WhiteRefMod2))
                {
                    whiteDataOK = true;
                    break;
                }
            }
            logg.calculateTime(DateTime.Now, stTime, "White Test2");
            TimeSpan time_span = DateTime.Now - stTime;
            //mainWnd.UpdateMod2TestStatus("White Test Done " + time_span.TotalMilliseconds.ToString() + "ms", Color.Lime);

            //if (!whiteDataOK)
            //    return -4;

            mainWnd.UpdateMod2Chart(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].WhiteRefMod2, false);

            if (!whiteDataOK)
                return -4;
            //2016010711
            string tempTimestring = DateTime.Now.ToString("hh");
            if (Para.timeStip2 != tempTimestring)
            {
                SaveLSValue(2, 400, 1100, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].WLMod2Dark);
                Para.timeStip2 = tempTimestring;
            }
            int restVal = LoadAndCheckLS(4, 400, 1100, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].WLMod2Dark);
            if (restVal != 0)
            {
                mainWnd.UpdateMod2TestStatus("光源波动太大，联系工程师", Color.Red);
                return 0;
            }
            //2016010711

            //Unit Ctr Offset In MM
            double unitCtrOffX = (((Cam2.ImageWidth / 2) - stationStatus[myIdx].mod2VisResult.X) * Cam1.CaliValue.X) + Para.Module[1].CamToOriginOffset.X;
            double unitCtrOffY = (((Cam2.ImageHeight / 2) - stationStatus[myIdx].mod2VisResult.Y) * Cam1.CaliValue.Y) + Para.Module[1].CamToOriginOffset.Y;

            System.Drawing.PointF CtrPt = new System.Drawing.PointF();
            CtrPt.X = (Cam2.ImageWidth / 2);
            CtrPt.Y = (Cam2.ImageHeight / 2);

            //System.Drawing.PointF RefPoint = new System.Drawing.PointF();
            //RefPoint.X = (float)((Cam2.ImageWidth / 2) - (stationStatus[myIdx].mod2VisResult.Height / 2));
            //RefPoint.Y = (float)((Cam2.ImageHeight / 2) - (stationStatus[myIdx].mod2VisResult.Width / 2));

            List<DPoint> XY = new List<DPoint>();
            //List<DPoint> OrgPos = new List<DPoint>();
            //List<DPoint> OffPos = new List<DPoint>();
            DateTime ptStTime = DateTime.Now;
            for (int i = 0; i < Para.Module[1].TestPt.Count; i++)
            {
                if (Para.Enb3TestPtOnly)
                {
                    //if (Para.SampleShape == 0)
                    //{
                    //    if ((i % 2) != 0)
                    //    {
                    //        XY.Add(new DPoint(0, 0));
                    //        continue;
                    //    }
                    //}
                    //else
                    //{
                        if ((i == 0) || (i == 4))
                        {
                            XY.Add(new DPoint(0, 0));
                            continue;
                        }
                    //}
                }

                mainWnd.UpdateMod2TestStatus("Start Test Point " + (i + 1).ToString(), Color.Lime);

                System.Drawing.PointF tpt1 = new System.Drawing.PointF(); // test pt in Pixel 
                tpt1.X = (float)(CtrPt.X + (Para.Module[1].TestPt[i].X / Cam1.CaliValue.X));
                tpt1.Y = (float)(CtrPt.Y + (Para.Module[1].TestPt[i].Y / Cam1.CaliValue.Y));

                Helper.ApplyRotation(ref tpt1, -(float)stationStatus[myIdx].mod2VisResult.Angle, CtrPt);

                DPoint tpt1Offset = new DPoint();
                tpt1Offset.X = (tpt1.X - CtrPt.X) * Cam2.CaliValue.X;
                tpt1Offset.Y = (tpt1.Y - CtrPt.Y) * Cam2.CaliValue.Y;
                //tpt1Offset.X = (tpt1.X - RefPoint.X) * Cam2.CaliValue.X;
                //tpt1Offset.Y = (tpt1.Y - RefPoint.Y) * Cam2.CaliValue.Y;

                //OrgPos.Add(new DPoint(Para.Module[1].TeachPos[0].X + Para.Module[1].TestPt[i].X, Para.Module[1].TeachPos[0].Y + Para.Module[1].TestPt[i].Y));
                //OffPos.Add(new DPoint(Para.Module[1].TeachPos[0].X + tpt1Offset.X + unitCtrOffX, Para.Module[1].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY));

                //mainWnd.UpdateMod2TestStatus("Move To Test Point " + (i + 1).ToString(), Color.Lime);
                stTime = DateTime.Now;

                //XY.Add(new DPoint(tpt1Offset.X - unitCtrOffX, tpt1Offset.Y + unitCtrOffY));

                

                XY.Add(new DPoint(stationStatus[myIdx].mod2VisResult.X, stationStatus[myIdx].mod2VisResult.Y));
                myMotionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[0].X + tpt1Offset.X + unitCtrOffX);
                motionRes = myMotionMgr.MoveTo((ushort)Axislist.Mod2YAxis, Para.Module[1].TeachPos[0].Y + tpt1Offset.Y + unitCtrOffY);
                if (motionRes != 0)
                    return -6;
                myMotionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
                myMotionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);

                time_span = DateTime.Now - stTime;
                //mainWnd.UpdateMod2TestStatus("Move To Test Point " + (i + 1).ToString() + " Done " + time_span.TotalMilliseconds.ToString() + "ms", Color.Lime);

                //mainWnd.UpdateMod2TestStatus("Start Collect Spectrum Test Point " + (i + 1).ToString(), Color.Lime);
                stTime = DateTime.Now;

                //mainWnd.UpdateMod2TestStatus("Test Point " + (i + 1).ToString(), Color.Lime);
                stationStatus[myIdx].MeasDataMod2[i] = specMgr.GetCount(1);
                //stationStatus[myIdx].MeasWLMod2[i] = specMgr.GetWaveLength(1);
                mainWnd.UpdateMod2Chart(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].MeasDataMod2[i], false);
                //time_span = DateTime.Now - stTime;
                //mainWnd.UpdateMod2TestStatus("End Collect Spectrum Test Point " + (i + 1).ToString() + " " + time_span.TotalMilliseconds.ToString() + "ms", Color.Lime);

                //mainWnd.UpdateMod2TestStatus("Start Calculating And Saving Test Point " + (i + 1).ToString(), Color.Lime);
                stTime = DateTime.Now;

                stationStatus[myIdx].transRatioMod2[i] = CalculateTransRatio(stationStatus[myIdx].DarkRefMod2, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[i]);
                mainWnd.UpdateMod2Chart(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].transRatioMod2[i], true);

                SaveRefDarkTransData2(stationStatus[myIdx].Unit2Barcode, 400, 1100, i + 1, stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].DarkRefMod2,
                                     stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[i], stationStatus[myIdx].transRatioMod2[i]);

                //SaveRawData(stationStatus[myIdx].Unit2Barcode, 400, 1100, i + 1, stationStatus[myIdx].WLMod2, stationStatus[myIdx].DarkRefMod2,
                //                     stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[i]);

                SaveRawData2(stationStatus[myIdx].Unit2Barcode, 400, 1100, i + 1, stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].WLMod2Dark,
                    stationStatus[myIdx].DarkRefMod2, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[i], 2, stationStatus[myIdx].mod2VisResult);

                //string timeEndPointStr = DateTime.Now.ToString("hh-mm-ss-fff");
                //mainWnd.WriteOperationinformation("Test2 Point " + (i + 1).ToString() + "End:" + timeEndPointStr);
            }
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
            //logg.calculateTime(DateTime.Now, ptStTime, "Spectrograph2");
            //specMgr.SetIO(1, true); 

            //DPoint VisRes = new DPoint(stationStatus[myIdx].mod2VisResult.X, stationStatus[myIdx].mod2VisResult.Y);
            //SaveOffsetData(stationStatus[myIdx].Unit2Barcode, new DPoint(CtrPt.X, CtrPt.Y), VisRes, stationStatus[myIdx].mod2VisResult.Angle, OrgPos[0], OrgPos[1], OrgPos[2], OrgPos[3], OrgPos[4], OffPos[0], OffPos[1],
            //                OffPos[2], OffPos[3], OffPos[4], 2);

            mainWnd.UpdateMod2TestStatus("Sending Test Result", Color.Orange);
            myMotionMgr.MoveTo((ushort)Axislist.Mod2XAxis, Para.Module[1].TeachPos[0].X);
            myMotionMgr.MoveTo((ushort)Axislist.Mod2YAxis, 0);
            if (Para.EnableCGHost)
            {
                int ALSExpTime = 0;

                if (Para.DisableBarcode)
                {
                    int barcodeLength = stationStatus[myIdx].Unit2Barcode.Length;
                    string blackorwhite = stationStatus[myIdx].Unit2Barcode.Substring(barcodeLength-1, 1);
                    if (blackorwhite != "B")//Black
                    {
                        ALSExpTime = Para.Cam2ExposureTimeW;
                    }
                    else
                    {
                        ALSExpTime = Para.Cam2ExposureTimeB;
                    }
                }
                else
                {
                    ALSExpTime = Para.Cam2ExposureTimeB;
                }
                //AComMod2.SendImageAdvance(Cam2.ImageWidth, Cam2.ImageHeight, stationStatus[myIdx].Mod2Exp1DarkImage, stationStatus[myIdx].Mod2Exp1Image,
                //                                                            stationStatus[myIdx].Mod2DarkImage, stationStatus[myIdx].Mod2Image, Para.Cam2ExposureTime1, ALSExpTime);

                //if (Para.Enb45DegTest)
                //string timeStartSendImageAndTrans = DateTime.Now.ToString("hh-mm-ss-fff");
                //mainWnd.WriteOperationinformation("Start Send Image And Data 2:" + timeStartSendImageAndTrans);
                if (true)//20161202
                {
                    int ImgWidth2 = GetImageWidth(stationStatus[myIdx].CCD2LightFalse);
                    int ImgHgt2 = GetImageHeight(stationStatus[myIdx].CCD2LightFalse);
                    Byte[] Exp2Dark = GetImageByte(stationStatus[myIdx].CCD2LightFalse);
                    Byte[] Exp2White = GetImageByte(stationStatus[myIdx].CCD2LightTrue);
                    Byte[] ALS2Dark = GetImageByte(stationStatus[myIdx].inspCCD2LightFalse);
                    Byte[] ALS2White = GetImageByte(stationStatus[myIdx].inspCCD2LightTrue);
                    //int ImgWidth2 = GetImageWidth(stationStatus[myIdx].inspCCD2LightTrue);
                    //int ImgHgt2 = GetImageHeight(stationStatus[myIdx].inspCCD2LightTrue);
                    //Byte[] Exp2Dark = GetImageByte(stationStatus[myIdx].inspCCD2LightTrue);
                    //Byte[] Exp2White = GetImageByte(stationStatus[myIdx].inspCCD2LightTrue);
                    //Byte[] ALS2Dark = GetImageByte(stationStatus[myIdx].inspCCD2LightTrue);
                    //Byte[] ALS2White = GetImageByte(stationStatus[myIdx].inspCCD2LightTrue);
                    DateTime finu2 = DateTime.Now;
                    AComMod2.SendImageAdvance(ImgWidth2, ImgHgt2, Exp2Dark, Exp2White, ALS2Dark, ALS2White, Para.Cam2ExposureTime1, ALSExpTime, Para.CenWL2, Para.PixDen2, Para.BeamSize2);
                    if (!AComMod2.WaitTestReply("finu"))
                    {
                        Para.myMain.WriteOperationinformation("Module 2 Recieve finu Reply Timeout ");
                        return -8;
                    }
                    logg.calculateTime(DateTime.Now, finu2, "finu2");
                }
                else
                {
                    DateTime timeSend1 = DateTime.Now;
                    int ImgWidth = GetImageWidth(stationStatus[myIdx].inspCCD2LightTrue);
                    int ImgHgt = GetImageHeight(stationStatus[myIdx].inspCCD2LightTrue);
                    Byte[] ALSWhite = GetImageByte(stationStatus[myIdx].inspCCD2LightTrue);
                    //AComMod2.SendImage(ImgWidth, ImgHgt, ALSWhite, new DPoint(stationStatus[myIdx].mod2VisResult.X, stationStatus[myIdx].mod2VisResult.Y), stationStatus[myIdx].mod2VisResult.Angle);
                    logg.calculateTime(DateTime.Now, timeSend1, "SendImage2");
                    Thread.Sleep(1000);
                    //DateTime timefinu1 = DateTime.Now;
                    //if (!AComMod2.WaitTestReply("finu"))
                    //{
                    //    Para.myMain.WriteOperationinformation("Module 2 Recieve finu Reply Timeout ");
                    //    return -7;
                    //}
                    //logg.calculateTime(DateTime.Now, timefinu1, "finu2");
                }

                DateTime tsta2 = DateTime.Now;
                if (Para.Enb3TestPtOnly)
                {
                    //AComMod2.SendAdvanTestResult(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].DarkRefMod2, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[0], stationStatus[myIdx].MeasDataMod2[0],
                    //                    stationStatus[myIdx].MeasDataMod2[2], stationStatus[myIdx].MeasDataMod2[2], stationStatus[myIdx].MeasDataMod2[4], XY, stationStatus[myIdx].mod2VisResult.Angle);
                    
                    //if (Para.SampleShape == 0)
                    //{
                    //    AComMod2.SendAdvanTestResultFor3Point(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].DarkRefMod2, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[0],
                    //                        stationStatus[myIdx].MeasDataMod2[2], stationStatus[myIdx].MeasDataMod2[4], XY, stationStatus[myIdx].mod2VisResult.Angle);
                    //}
                    //else
                    //{
                        AComMod2.SendAdvanTestResultFor3Point(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].DarkRefMod2, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[1],
                                            stationStatus[myIdx].MeasDataMod2[2], stationStatus[myIdx].MeasDataMod2[3], XY, stationStatus[myIdx].mod2VisResult.Angle);
                    //}
                }
                else
                    AComMod2.SendAdvanTestResult(stationStatus[myIdx].WLMod2Dark, stationStatus[myIdx].DarkRefMod2, stationStatus[myIdx].WhiteRefMod2, stationStatus[myIdx].MeasDataMod2[0], stationStatus[myIdx].MeasDataMod2[1],
                                        stationStatus[myIdx].MeasDataMod2[2], stationStatus[myIdx].MeasDataMod2[3], stationStatus[myIdx].MeasDataMod2[4], XY, stationStatus[myIdx].mod2VisResult.Angle);
                
                if (!AComMod2.WaitTestReply("tsta"))
                {
                    Para.myMain.WriteOperationinformation("Module 2 Recieve tsta Reply Timeout ");
                    return -8;
                }
                logg.calculateTime(DateTime.Now,tsta2,"tsta2");               
            }
            myMotionMgr.WaitAxisStop((ushort)Axislist.Mod2XAxis);
            myMotionMgr.WaitAxisStop((ushort)Axislist.Mod2YAxis);
            DateTime end2 = DateTime.Now;
            if (Para.EnableCGHost)
            {
                Thread.Sleep(100);
                int res = AComMod2.SendEndTest("Image2");
                if (res == -1)
                {
                    Para.myMain.WriteOperationinformation("Module 2 Recieve End Test Reply Timeout ");
                    return -8; //Timeout
                }
                if (res == 0)
                    stationStatus[myIdx].IsMod2UnitPassed = true;
                else
                    stationStatus[myIdx].IsMod2UnitPassed = false;

                Para.errorCode2Array[myIdx] = AComMod2.errorCode.ToString();
                Para.errorStr2Array[myIdx] = AComMod2.errorString;
                if (Para.EnBin_Code)
                    Para.bin_Code2Array[myIdx] = AComMod2.bin_Code;
                else
                    Para.bin_Code2Array[myIdx] = "";
                if (AComMod2.returnCode != "dest")
                {
                    Para.myMain.WriteOperationinformation("Module 2 send all datas NOT OK. ");
                    return -10;
                }
                //logg.calculateTime(DateTime.Now,end2,"dest2"); 
                //if (stationStatus[myIdx].IsMod2UnitPassed)
                //    logg.saveLog(stationStatus[myIdx].Unit2Barcode, "Pass");
                //else
                //    logg.saveLog(stationStatus[myIdx].Unit2Barcode, "Fail");
                AComMod2.Disconnect();
            }
            //string timeTestEnd = DateTime.Now.ToString("hh-mm-ss-fff");
            //mainWnd.WriteOperationinformation("Test End 2:" + timeTestEnd);
            string timeTestEnd = DateTime.Now.ToString("hh-mm-ss-fff");
            //logg.saveCycleTime("Channel2", stationStatus[myIdx].Unit2Barcode, Para.startTime, timeTestEnd);
            mainWnd.UpdateMod2TestStatus("Test End", Color.Orange);            
            Para.firstFinish = false;
            return 0;
        }
        object sny_Obj = new object();
        public void SaveOffsetData(string barCode, DPoint ImgCtr, DPoint VisRes, double Ang, DPoint OrgPt1, DPoint OrgPt2, DPoint OrgPt3, DPoint OrgPt4, DPoint OrgPt5,
                                    DPoint OffPt1, DPoint OffPt2, DPoint OffPt3, DPoint OffPt4, DPoint OffPt5, int ModIndx)
        {
            lock (sny_Obj)
            {
                string s_FileName = barCode + "_OffsetData_" + DateTime.Now.ToString("yyyyMMdd");

                string path = "D:\\OffsetData";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = "D:\\OffsetData\\Module" + (ModIndx).ToString();
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
                        columnTitle = "ImageCtrX" + ",";
                        columnTitle += "ImageCtrY" + ",";
                        columnTitle += "FoundCtrX" + ",";
                        columnTitle += "FoundCtrY" + ",";
                        columnTitle += "FoundAng" + ",";
                        columnTitle += "Org_Pt1_X" + ",";
                        columnTitle += "Org_Pt1_Y" + ",";
                        columnTitle += "Org_Pt2_X" + ",";
                        columnTitle += "Org_Pt2_Y" + ",";
                        columnTitle += "Org_Pt3_X" + ",";
                        columnTitle += "Org_Pt3_Y" + ",";
                        columnTitle += "Org_Pt4_X" + ",";
                        columnTitle += "Org_Pt4_Y" + ",";
                        columnTitle += "Org_Pt5_X" + ",";
                        columnTitle += "Org_Pt5_Y" + ",";

                        columnTitle += "OFF_Pt1_X" + ",";
                        columnTitle += "OFF_Pt1_Y" + ",";
                        columnTitle += "OFF_Pt2_X" + ",";
                        columnTitle += "OFF_Pt2_Y" + ",";
                        columnTitle += "OFF_Pt3_X" + ",";
                        columnTitle += "OFF_Pt3_Y" + ",";
                        columnTitle += "OFF_Pt4_X" + ",";
                        columnTitle += "OFF_Pt4_Y" + ",";
                        columnTitle += "OFF_Pt5_X" + ",";
                        columnTitle += "OFF_Pt5_Y" + ",";

                        sw.WriteLine(columnTitle);
                    }

                    columnValue = ImgCtr.X.ToString("F3") + ",";
                    columnValue += ImgCtr.Y.ToString("F3") + ",";
                    columnValue += VisRes.X.ToString("F3") + ",";
                    columnValue += VisRes.Y.ToString("F3") + ",";
                    columnValue += Ang.ToString("F3") + ",";
                    
                    columnValue += OrgPt1.X.ToString("F3") + ",";
                    columnValue += OrgPt1.Y.ToString("F3") + ",";
                    columnValue += OrgPt2.X.ToString("F3") + ",";
                    columnValue += OrgPt2.Y.ToString("F3") + ",";
                    columnValue += OrgPt3.X.ToString("F3") + ",";
                    columnValue += OrgPt3.Y.ToString("F3") + ",";
                    columnValue += OrgPt4.X.ToString("F3") + ",";
                    columnValue += OrgPt4.Y.ToString("F3") + ",";
                    columnValue += OrgPt5.X.ToString("F3") + ",";
                    columnValue += OrgPt5.Y.ToString("F3") + ",";

                    columnValue += OffPt1.X.ToString("F3") + ",";
                    columnValue += OffPt1.Y.ToString("F3") + ",";
                    columnValue += OffPt2.X.ToString("F3") + ",";
                    columnValue += OffPt2.Y.ToString("F3") + ",";
                    columnValue += OffPt3.X.ToString("F3") + ",";
                    columnValue += OffPt3.Y.ToString("F3") + ",";
                    columnValue += OffPt4.X.ToString("F3") + ",";
                    columnValue += OffPt4.Y.ToString("F3") + ",";
                    columnValue += OffPt5.X.ToString("F3") + ",";
                    columnValue += OffPt5.Y.ToString("F3") + ",";

                    
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
        public void SaveRawData2(string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WLDark, List<float> WLWhite, List<float> WLMeas,
                                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData, int ModuleIndex, JPTCG.Vision.HalconInspection.RectData VisResult)
        {
            lock (sny_Obj)
            {
                string s_FileName = barCode + "_RawTransData_" + DateTime.Now.ToString("yyyyMMdd");

                string path = "D:\\RawData";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = "D:\\RawData\\Module" + (ModuleIndex).ToString();
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
        public void SaveRawData(string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WLDark, List<float> WLWhite, List<float> WLMeas,
                                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData, int ModuleIndex, JPTCG.Vision.HalconInspection.RectData VisResult)
        {
            lock (sny_Obj)
            {
                string s_FileName = barCode + "_RawTransData_" + DateTime.Now.ToString("yyyyMMdd");

                string path = "D:\\RawData";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = "D:\\RawData\\Module"+(ModuleIndex).ToString();
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
        public void SaveRefDarkTransData2(string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WL,
                                               List<float> darkRef, List<float> WhiteRef, List<float> MeasData, List<float> TransData)
        {
            string s_FileName = barCode + "_RefDarkTransData_" + DateTime.Now.ToString("yyyyMMdd");

            string path = "D:\\DataInfo";
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

        //ZJP
        public void SaveRefDarkTransData(string barCode, int stWaveLength, int endWaveLength, int TestPoint, List<float> WL,
                                                List<float> darkRef, List<float> WhiteRef, List<float> MeasData, List<float> TransData)
        {
            string s_FileName = barCode+"_RefDarkTransData_" + DateTime.Now.ToString("yyyyMMdd");
            
            string path = "D:\\DataInfo";
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

                columnValue = barCode+",";//barCode.Replace(",", "") + string.Format("{0:D2}", index + 1) + ",";//二维码+num
                columnValue += Para.MchName + "," + TestPoint.ToString() +",";             

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

        private int SetTestStationDone()
        {
            isTestStationReady = true;
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.SpectrumLS, false);
            //Thread.Sleep(1500);     //20161206       
            return 0;
        }
        #endregion


        #region Module 4 Unloading Sequencing Functions
        private int GetIndexOfUnloadStation()
        {
            int res = Para.CurrentRotaryIndex - 3;
            if (res < 0)
                res = (res + 4);

            return res;
        }

        private int UnloadStationWaitUnitReady()
        {
            if (!Para.EnbStation4Unloading)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                UnloadingSeq.JumpIndex(0);
                return 99; //To Jump Index
            }

            if (!isUnloadStationRotaryIndexDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                UnloadingSeq.JumpIndex(0);
                return 99; //To Jump Index
            }

            isUnloadStationRotaryIndexDone = false;            

            int RotaryIdxAtUnload = GetIndexOfUnloadStation();//CurRotIdx -1;            

            if ((!stationStatus[RotaryIdxAtUnload].isUnitLoad1) && (!stationStatus[RotaryIdxAtUnload].isUnitLoad2))
            {
                UnloadingSeq.JumpIndex(0);
                UnloadStationUnloadDone = true;
                return 99; //To Jump Index
            }
            UnloadStationUnloadDone = false;
            //isUnloadStationReady = false;
            return 0;
        }
        private int UnloadStationUnloadUnit()
        {
            int RotaryIdxAtUnload = GetIndexOfUnloadStation();

            if (Para.ContTestRunData)
            {
                if (stationStatus[RotaryIdxAtUnload].isUnitLoad1)
                {
                    if (stationStatus[RotaryIdxAtUnload].IsMod1UnitPassed)
                    {
                        mainWnd.UpdateMod1TestResult(0, RotaryIdxAtUnload);

                    }
                    else
                    {
                        mainWnd.UpdateMod1TestResult(1, RotaryIdxAtUnload);

                    }
                    if (stationStatus[RotaryIdxAtUnload].IsMod2UnitPassed)
                    {
                        mainWnd.UpdateMod2TestResult(0, RotaryIdxAtUnload);

                    }
                    else
                    {
                        mainWnd.UpdateMod2TestResult(1, RotaryIdxAtUnload);

                    }

                    mainWnd.DisplayBarcode(1, stationStatus[RotaryIdxAtUnload].Unit1Barcode);
                    mainWnd.DisplayBarcode(2, stationStatus[RotaryIdxAtUnload].Unit2Barcode);

                    mainWnd.UpdateTotalCountUI();
                    //To Jump Index
                }
            }else
            {
                switch (RotaryIdxAtUnload)
                {
                    case 0:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI1Vac2, false);
                        break;
                    case 1:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI2Vac2, false);
                        break;
                    case 2:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI3Vac2, false);
                        break;
                    case 3:
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac1, false);
                        myMotionMgr.WriteIOOut((ushort)OutputIOlist.RI4Vac2, false);
                        break;
                }

                if (stationStatus[RotaryIdxAtUnload].isUnitLoad1)
                {
                    if (stationStatus[RotaryIdxAtUnload].IsMod1UnitPassed)
                        mainWnd.UpdateMod1TestResult(0, RotaryIdxAtUnload);
                    else
                        mainWnd.UpdateMod1TestResult(1, RotaryIdxAtUnload);

                    mainWnd.DisplayBarcode(1, stationStatus[RotaryIdxAtUnload].Unit1Barcode);

                    if (!stationStatus[RotaryIdxAtUnload].IsMod1UnitPassed)
                    {
                        Para.Mod1FailUnitCnt = Para.Mod1FailUnitCnt + 1;
                    }
                }
                else
                {
                    mainWnd.UpdateMod1TestResult(-1, RotaryIdxAtUnload);
                    mainWnd.DisplayBarcode(1, stationStatus[RotaryIdxAtUnload].Unit1Barcode);
                }

                if (stationStatus[RotaryIdxAtUnload].isUnitLoad2)
                {
                    if (stationStatus[RotaryIdxAtUnload].IsMod2UnitPassed)
                        mainWnd.UpdateMod2TestResult(0, RotaryIdxAtUnload);
                    else
                        mainWnd.UpdateMod2TestResult(1, RotaryIdxAtUnload);

                    mainWnd.DisplayBarcode(2, stationStatus[RotaryIdxAtUnload].Unit2Barcode);

                    if (!stationStatus[RotaryIdxAtUnload].IsMod2UnitPassed)
                    {
                        Para.Mod2FailUnitCnt = Para.Mod2FailUnitCnt + 1;
                    }
                }
                else
                {
                    mainWnd.UpdateMod2TestResult(-1, RotaryIdxAtUnload);
                    mainWnd.DisplayBarcode(2, stationStatus[RotaryIdxAtUnload].Unit2Barcode);
                }

                if (stationStatus[RotaryIdxAtUnload].isUnitLoad1)
                {

                    Para.TotalTestUnit = Para.TotalTestUnit + 2;
                }

                mainWnd.UpdateTotalCountUI();
                stationStatus[RotaryIdxAtUnload].isUnitLoad1 = false;
                stationStatus[RotaryIdxAtUnload].isUnitLoad2 = false;
            }


            if (Para.EndLot)
            {
                if ((stationStatus[0].isUnitLoad1 == false) && (stationStatus[0].isUnitLoad2 == false) &&
                    (stationStatus[1].isUnitLoad1 == false) && (stationStatus[1].isUnitLoad2 == false) &&
                    (stationStatus[2].isUnitLoad1 == false) && (stationStatus[2].isUnitLoad2 == false) &&
                    (stationStatus[3].isUnitLoad1 == false) && (stationStatus[3].isUnitLoad2 == false))
                {
                    Para.EndLot = false; 
                }
            }

            return 0;
        }

        private int UnloadStationWaitUnloadReady()
        {        
            // New Sensor start Btn

            if (!myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnLeftFor4) || !myMotionMgr.ReadIOIn((ushort)InputIOlist.BtnRightFor4))
            {
                Thread.Sleep(50);
                Application.DoEvents();

                UnloadingSeq.JumpIndex(UnloadingSeq.CurrentIndex());
                return 99; //To Jump Index
            }
            UnloadStationUnloadDone = true;
            return 0;        
        }
        #endregion
        #region TestCamera Sequencing Functions
        private int TestCameraWaitUnitReady()
        {
            if (!rotMgr.InTestCameraDone)
                isTestCameraDone = false;
            else
                isTestCameraDone = true;
            if ((!isTestCameraDone))
            {
                Thread.Sleep(50);
                Application.DoEvents();
                TestCameraSeq.JumpIndex(TestCameraSeq.CurrentIndex());
                return 99; //To Jump Index
            }
            return 0;
        }
        private int TestCameraStartTest()
        {
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
            //logg.saveerrorLog("Mod1_LED open normally");
            int rtIdx = Para.CurrentRotaryIndex;
            int RotaryIdxAtCam45 = Para.CurrentRotaryIndex;
            Cam1.SetExposure(Para.Cam1ExposureTime1);
        
            Thread.Sleep(120);//120
            bool GrabPass = false;
            for (int i = 0; i < 3; i++)
            {
                //Thread.Sleep(10);
                if (Cam1.Grab())
                {
                    GrabPass = true;
                    break;
                }
                Application.DoEvents();
            }
            if (GrabPass == false || Cam1.myImage == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    //Thread.Sleep(10);
                    if (Cam1.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }
            }
            if (!GrabPass)
            {
                stationStatus[RotaryIdxAtCam45].isUnitLoad1 = false;//Alvin 2/3/17
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, false);
                Thread.Sleep(200);
                return -5;//Alvin 2/3/17
            }

            if (Cam1.myImage == null)//Alvin 2/3/17
            {
                stationStatus[RotaryIdxAtCam45].isUnitLoad1 = false;
                return -5;
            }
            stationStatus[RotaryIdxAtCam45].Mod1Exp1Image = Cam1.GetImgPtr();
            HOperatorSet.CopyObj(Cam1.myImage, out stationStatus[RotaryIdxAtCam45].CCD1LightTrue, 1, 1);
            //2017020319  
            string timeStr = DateTime.Now.ToString("yyMMdd");
            string path = Para.PicPath + ":\\M1Picture\\" + timeStr;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string tStrCCD1LightTrue = DateTime.Now.ToString("yyMMddHHmmss");
            string FNCCD1LightTrue = path + "\\" + stationStatus[RotaryIdxAtCam45].Unit1Barcode + "_" + tStrCCD1LightTrue + "_" + Para.Cam1ExposureTime1.ToString() + "_1_A" + ".bmp";
            HOperatorSet.WriteImage(Cam1.myImage, "bmp", 0, FNCCD1LightTrue);  //zhe ge ky?
            //2017020319
            return 0;
        }
        private int SetTestCameraDone()
        {
            isTestCameraDone = false;
            rotMgr.InTestCameraDone = false;
            return 0;
        }
        #endregion

        #region TestCamera2 Sequencing Functions
        private int TestCamera2WaitUnitReady()
        {
            if (!rotMgr.InTestCameraDone)
                isTestCamera2Done = false;
            else
                isTestCamera2Done = true;
            if ((!isTestCamera2Done))
            {
                Thread.Sleep(50);
                Application.DoEvents();
                TestCamera2Seq.JumpIndex(TestCamera2Seq.CurrentIndex());
                return 99; //To Jump Index
            }
            return 0;
        }
        private int TestCamera2StartTest()
        {
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
            //logg.saveerrorLog("Mod2_LED open normally ");
            int rtIdx = Para.CurrentRotaryIndex;
            int RotaryIdxAtCam45 = Para.CurrentRotaryIndex;
            Cam2.SetExposure(Para.Cam2ExposureTime1);
           
            Thread.Sleep(20);//20
            bool GrabPass = false;
            for (int i = 0; i < 3; i++)
            {
                //Thread.Sleep(10);
                if (Cam2.Grab())
                {
                    GrabPass = true;
                    break;
                }
                Application.DoEvents();
            }
            if (GrabPass == false || Cam2.myImage == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    //Thread.Sleep(10);
                    if (Cam2.Grab())
                    {
                        GrabPass = true;
                        break;
                    }
                    Application.DoEvents();
                }
            }
            if (!GrabPass)
            {
                stationStatus[RotaryIdxAtCam45].isUnitLoad2 = false;//Alvin 2/3/17
                myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, false);
                Thread.Sleep(200);
                return -6;//Alvin 2/3/17
            }

            if (Cam2.myImage == null)//Alvin 2/3/17
            {
                stationStatus[RotaryIdxAtCam45].isUnitLoad2 = false;
                return -6;
            }
            stationStatus[RotaryIdxAtCam45].Mod2Exp1Image = Cam2.GetImgPtr();
            stationStatus[RotaryIdxAtCam45].CCD2LightTrue = Cam2.myImage;
            HOperatorSet.CopyObj(Cam2.myImage, out stationStatus[RotaryIdxAtCam45].CCD2LightTrue, 1, 1);
            //2017020319  
            string timeStr = DateTime.Now.ToString("yyMMdd");
            string path = Para.PicPath + ":\\M2Picture\\" + timeStr;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string tStrCCD2LightTrue = DateTime.Now.ToString("yyMMddHHmmss");
            string FNCCD2LightTrue = path + "\\" + stationStatus[RotaryIdxAtCam45].Unit2Barcode + "_" + tStrCCD2LightTrue + "_" + Para.Cam2ExposureTime1.ToString() + "_1_A" + ".bmp";
            HOperatorSet.WriteImage(Cam2.myImage, "bmp", 0, FNCCD2LightTrue);  //zhe ge ky?
            //2017020319
            return 0;
        }
        private int SetTestCamera2Done()
        {
            isTestCamera2Done = false;
            rotMgr.InTestCameraDone = false;
            return 0;
        }
        #endregion
        #region Machine Homing
        public void StartHoming()
        {
            if (mainSeq.IsBusy)
                return;
            isHomeDone = false;
            SplashScreen ss = new SplashScreen();
            ss.Show();

            HomingThread = new Thread(new ThreadStart(StartHomeThread));
            HomingThread.IsBackground = true;
            HomingThread.Start();            

            while (!isHomeDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();                
            }
            ss.Close();
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam1Light, true);
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.Cam2Light, true);
        }
        private void RunningLight()
        {
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampGreen, true);
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampAmber, false);
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampRed, false);
        }

        private void StoppingLight()
        {
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampGreen, false);
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampAmber, true);
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampRed, false);
        }

        private void ErrorLight()
        {
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampGreen, false);
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampAmber, false);
            myMotionMgr.WriteIOOut((ushort)OutputIOlist.LampRed, true);
        }
        private void StartHomeThread()
        {
            RunningLight();
            if (Para.MachineOnline)
            {
                myMotionMgr.SetServoOn((ushort)Axislist.Mod1XAxis, 1);
                myMotionMgr.SetServoOn((ushort)Axislist.Mod2XAxis, 1);
                myMotionMgr.SetServoOn((ushort)Axislist.Mod1YAxis, 1);
                myMotionMgr.SetServoOn((ushort)Axislist.Mod2YAxis, 1);
                if (true)
                {
                    myMotionMgr.SetServoOn((ushort)Axislist.Mod1ZAxis, 1);
                    myMotionMgr.SetServoOn((ushort)Axislist.Mod2ZAxis, 1);
                }

                myMotionMgr.Homing((ushort)Axislist.Mod1XAxis, 1);
                myMotionMgr.Homing((ushort)Axislist.Mod1YAxis, 1);
                myMotionMgr.Homing((ushort)Axislist.Mod2XAxis, 1);
                myMotionMgr.Homing((ushort)Axislist.Mod2YAxis, 1);
                if (true)
                {
                    myMotionMgr.Homing((ushort)Axislist.Mod1ZAxis, 0);
                    myMotionMgr.Homing((ushort)Axislist.Mod2ZAxis, 0);
                }

                if (myMotionMgr.WaitHomeDone((ushort)Axislist.Mod1XAxis) != 0)
                    mainWnd.WriteOperationinformation("Module 1 X Axis Home Error");
                if (myMotionMgr.WaitHomeDone((ushort)Axislist.Mod1YAxis) != 0)
                    mainWnd.WriteOperationinformation("Module 1 Y Axis Home Error");
                if (myMotionMgr.WaitHomeDone((ushort)Axislist.Mod2XAxis) != 0)
                    mainWnd.WriteOperationinformation("Module 2 X Axis Home Error");
                if (myMotionMgr.WaitHomeDone((ushort)Axislist.Mod2YAxis) != 0)
                    mainWnd.WriteOperationinformation("Module 2 Y Axis Home Error");
                if (true)
                {
                    if (myMotionMgr.WaitHomeDone((ushort)Axislist.Mod1ZAxis) != 0)
                        mainWnd.WriteOperationinformation("Module 1 Z Axis Home Error");
                    if (myMotionMgr.WaitHomeDone((ushort)Axislist.Mod2ZAxis) != 0)
                        mainWnd.WriteOperationinformation("Module 2 Z Axis Home Error");
                }
                Thread.Sleep(200);

                if (myMotionMgr.NEL((ushort)Axislist.Mod1YAxis))
                {
                    myMotionMgr.Homing((ushort)Axislist.Mod1YAxis, 1);
                    myMotionMgr.WaitHomeDone((ushort)Axislist.Mod1YAxis);
                }
                if (myMotionMgr.NEL((ushort)Axislist.Mod2YAxis))
                {
                    myMotionMgr.Homing((ushort)Axislist.Mod2YAxis, 1);
                    myMotionMgr.WaitHomeDone((ushort)Axislist.Mod2YAxis);
                }
                myMotionMgr.SetPosition((ushort)Axislist.Mod1XAxis, 0);
                myMotionMgr.SetPosition((ushort)Axislist.Mod1YAxis,0);
                myMotionMgr.SetPosition((ushort)Axislist.Mod2XAxis, 0);
                myMotionMgr.SetPosition((ushort)Axislist.Mod2YAxis, 0);
                if (true)
                {
                    myMotionMgr.SetPosition((ushort)Axislist.Mod1ZAxis, 0);
                    myMotionMgr.SetPosition((ushort)Axislist.Mod2ZAxis, 0);
                }

                if (!rotMgr.GoHome())
                {
                    Thread.Sleep(300);
                    myMotionMgr.WriteIOOut((ushort)OutputIOlist.RotaryEnabled, false);
                    mainWnd.WriteOperationinformation("Rotary Motor Home Error");
                    Para.isRotaryError = true;
                    Application.DoEvents();
                    mainWnd.OnlyHomeEnb();
                    mainWnd.ShowErrorMessageBox("安全门或安全光栅被触发，请先复位！", "安全报警！"); 
                }
                if (true)
                {
                    myMotionMgr.MoveTo((ushort)Axislist.Mod1ZAxis, Para.Module[0].TeachPos[0].Z);
                    myMotionMgr.MoveTo((ushort)Axislist.Mod2ZAxis, Para.Module[1].TeachPos[0].Z);
                    myMotionMgr.WaitAxisStop((ushort)Axislist.Mod1ZAxis);
                    myMotionMgr.WaitAxisStop((ushort)Axislist.Mod2ZAxis);
                }
            }
            else
            {
                Thread.Sleep(2000);
            }
            StoppingLight();
            mainWnd.WriteOperationinformation("All Axis Home Done");
            isHomeDone = true;
        }
        #endregion
    }
}
