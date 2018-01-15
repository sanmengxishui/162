using Common;
using HalconDotNet;
using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MVDLL;

namespace JPTCG.Vision
{
    public class HalconVision
    {
        public delegate void OnImageReady(HObject myImage);
        public event OnImageReady OnImageReadyFunction;

        //HalconInterface myInterface = null;
        public String Name = "";
        private BaslerCamera BaslerCam;
        public string cameraID = "NotAssigned";
        public HObject myImage = null;
        public DPoint CaliValue = new DPoint();
        public int threshold = 30;

        private HalconInspection HInspection = new HalconInspection();
        private bool bGrabDone = false;
        public bool bUIRefreshDone = false;
        public int ImageWidth = 0;
        public int ImageHeight = 0;
        public bool bSaveImage = false;
        List<string> BasCamList = new List<string>();

        public HalconVision(String name, Panel parentPnl,HWindowControl myHalconWin)
        {            
            Name = name;
            //myInterface = new HalconInterface(name, parentPnl, this, myHalconWin);
            BaslerCam = new BaslerCamera();//BaslerGigeCameraMgr.Inistance();
            //myImage = new HObject();
            myImage = new HImage();
            BaslerCam.ImageReadyEvent += new BaslerCamera.ImageReadyEventHandler(OnImageReadyEventCallback);
        }
        ~HalconVision()
        {
            StopCamera();
            BaslerCam.CloseCamera();
        }

        public void CloseCamera()
        {
            StopCamera();
            BaslerCam.CloseCamera();        
        }

        public void SaveSettings(string fileName)
        {
            string headerStr = "Cam_" + Name.Replace(" ", string.Empty);
            FileOperation.SaveData(fileName, headerStr, "CameraID", cameraID);
            //FileOperation.SaveData(fileName, headerStr, "CalibrationX", CaliValue.X.ToString("F4"));
            //FileOperation.SaveData(fileName, headerStr, "CalibrationY", CaliValue.Y.ToString("F4"));
        }
        public void LoadSettings(string fileName)
        {
            string strread = "";
            string headerStr = "Cam_"+Name.Replace(" ", string.Empty);
            FileOperation.ReadData(fileName, headerStr, "CameraID", ref strread);
            if (strread != "0")
                cameraID = strread;

            FileOperation.ReadData(fileName, headerStr, "CalibrationX", ref strread);
            if (strread != "0")
                CaliValue.X = double.Parse(strread);

            FileOperation.ReadData(fileName, headerStr, "CalibrationY", ref strread);
            if (strread != "0")
                CaliValue.Y = double.Parse(strread);

            FileOperation.ReadData(fileName, headerStr, "Threshold", ref strread);
            if (strread != "0")
                threshold = int.Parse(strread);

            if (cameraID != "NotAssigned")
                AssignBaslerCamera(cameraID);
            else
                MessageBox.Show(Name +" camera Not Assigned.");
        }
        public void AssignBaslerCamera(string camID)
        {
            if (camID == "")
                return;
            cameraID = camID;

            if (!ConnectBaslerCam())
            {
                MessageBox.Show(Name + "Camera ID:" + camID + " Not Found.");
            }
        }
        public bool ConnectBaslerCam()
        {
            bool res = false;
            if (cameraID == "")
                return res;

            for (int i = 0; i < 3; i++)
            {
                res = BaslerCam.OpenCameraByCameraID(cameraID);
                if (res)
                    break;
            }
            ImageWidth = (int)2592;
            ImageHeight = (int)1944;
            return res;
        }

        public void SetTrigMode()
        {
            BaslerCam.SetCameraTriggerMode();
        }

        public void SetExposure(int Val)
        {
            if (!IsCameraOpen())
                return;

            //StopCamera();
            //Application.DoEvents();
            //Thread.Sleep(100);

            BaslerCam.SetExposure(Val);
        }

        public JPTCG.Vision.HalconInspection.RectData Inspect(double caliValues)
        {
            //string DataFileName = @"D:\Images\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            //if (!Directory.Exists(DataFileName))
            //    Directory.CreateDirectory(DataFileName);
            //string temp1 = DataFileName + "Inspected" + DateTime.Now.ToString("HH_mm_ss");
            //HOperatorSet.WriteImage(myImage, "bmp", 0, temp1);

            if (Para.MachineOnline)
            {
                //if mhalcon_image2
                myImage = BaslerCam.mhalcon_image2;
            }
            JPTCG.Vision.HalconInspection.RectData myResult = new JPTCG.Vision.HalconInspection.RectData();
            if (myImage==null)
                return myResult;
            if (Para.SampleShape == 0)
                myResult = HInspection.FindRectNew(myImage, caliValues);//HInspection.FindRect(myImage, threshold);
            else
                myResult = HInspection.FindCircle(myImage);
            
            return myResult;
        }

        public JPTCG.Vision.HalconInspection.GreyValueData GetGreyValue()
        {
            return HInspection.MinMaxMeanGreyValue(myImage);
        }

        public List<String> GetBaslerCamSerialList()
        {
            BasCamList.Clear();
            uint numCam = BaslerCam.numDevices;         //相机初始化
            
            for (int i = 0; i< numCam;i++)
            {       
                BasCamList.Add(BaslerCam.GetDevicename((uint)i));                         
            }
            return BasCamList;
        }
        public void OnImageReadyEventCallback()
        {
            //myImage.Dispose();
            //myImage = new HImage(BaslerCam.mhalcon_image2);
            myImage = BaslerCam.mhalcon_image2;
             if (myImage != null)
             {
                 bUIRefreshDone = false;
                 if (OnImageReadyFunction != null)
                     OnImageReadyFunction(BaslerCam.mhalcon_image1);
                 
                 while (!bUIRefreshDone)
                 {
                     Thread.Sleep(10);
                     Application.DoEvents();
                 }
             }
             
             bGrabDone = true;
             BaslerCam.bGrabDone = true;
        }
        public bool IsCameraOpen()
        {
            return BaslerCam.IsOpen;
        }
        public bool IsCameraLive()
        {
            return BaslerCam.IsLive;
        }
        public void LiveCamera()
        {
            if (!BaslerCam.IsOpen)
                return;
            if (!BaslerCam.IsLive)
                BaslerCam.ContinuousShot();
        }
        public void StopCamera()
        {
            BaslerCam.Stop();
        }

        public bool Grab()
        {
            bGrabDone = false;
            BaslerCam.OneShot();
            DateTime st_time = DateTime.Now;
            TimeSpan time_span;
            while (!bGrabDone)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                time_span = DateTime.Now - st_time;
                if (time_span.TotalMilliseconds > 1000)
                {
                    return false;
                }
            }

            if (bSaveImage)
            {
                string DataFileName = @"D:\Images\" +Name+"_"+ DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                if (!Directory.Exists(DataFileName))
                    Directory.CreateDirectory(DataFileName);
                string temp1 = DataFileName + "RawImage" + DateTime.Now.ToString("HH_mm_ss");
                HOperatorSet.WriteImage(myImage, "bmp", 0, temp1);
            }

            return true;
        }

        public byte[] GetImgPtr()
        {
            byte[] myImgPtr = new byte[BaslerCam.imgPtr.Length];
            Buffer.BlockCopy(BaslerCam.imgPtr, 0, myImgPtr, 0, BaslerCam.imgPtr.Length);
            return myImgPtr;
        }

        public JPTCG.Vision.HalconInspection.RectData FindCircleCenter(double caliValues, HWindowControl hWin)
        {
            MachineVision Mc = new MachineVision();
            _result result = new _result();
            if (Para.MachineOnline)
            {
                //if mhalcon_image2
                myImage = BaslerCam.mhalcon_image2;
            }
            HTuple hv_High, hv_width;
            HOperatorSet.GetImageSize(myImage, out hv_width, out hv_High);
            HOperatorSet.SetPart(hWin.HalconWindow, 0, 0, hv_High, hv_width);
            myImage.DispObj(hWin.HalconWindow);
            result = Mc.Analys(myImage, hWin);
            JPTCG.Vision.HalconInspection.RectData myResult = new JPTCG.Vision.HalconInspection.RectData();
            if (myImage == null)
                return myResult;

            myResult.X = result.centerX;
            myResult.Y = result.centerY;
            if (myResult.X != 0.0 && myResult.Y != 0.00)
                myResult.Found = true;
            else
                myResult.Found = false;
            return myResult;
        }
    }
}
