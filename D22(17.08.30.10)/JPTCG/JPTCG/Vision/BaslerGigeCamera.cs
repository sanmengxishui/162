using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using PylonC.NET;
using System.Runtime.InteropServices;
using HalconDotNet;


namespace JPTCG.Vision
{
    class BaslerGigeCamera
    {
       
        public class Image
        {
            public Image(int newWidth, int newHeight, Byte[] newBuffer, bool color)
            {
                Width = newWidth;
                Height = newHeight;
                Buffer = newBuffer;
                Color = color;
            }

            public readonly int Width; /* The width of the image. */
            public readonly int Height; /* The height of the image. */
            public readonly Byte[] Buffer; /* The raw image data. */
            public readonly bool Color; /* If false the buffer contains a Mono8 image. Otherwise, RGBA8packed is provided. */
        }

        protected class GrabResult
        {
            public Image ImageData; /* Holds the taken image. */
            public PYLON_STREAMBUFFER_HANDLE Handle; /* Holds the handle of the image registered at the stream grabber. It is used to queue the buffer associated with itself for the next grab. */
        }
        public static double Kx = 1;  //相机与轴标定的X轴的参数
        public static double Ky = 1;  //相机与轴标定的y轴的参数
        public static double Bx = 0.0;
        public static double By = 0.0;
        // public HObject halcon_image = null;
        //public Bitmap m_bitmap = null;
        const uint NUM_DEVICES = 10;
        protected PYLON_DEVICE_HANDLE hDev;
        protected PYLON_DEVICE_INFO_HANDLE hDi;
        protected PYLON_WAITOBJECTS_HANDLE wos;
        protected PYLON_STREAMGRABBER_HANDLE hGrabber = null;
        protected PYLON_WAITOBJECT_HANDLE hWaitGrabFinished;
        protected PYLON_WAITOBJECT_HANDLE hWaitGrabStop;
        protected Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> buffers;
        protected List<GrabResult> m_grabbedBuffers;
        protected PYLON_IMAGE_FORMAT_CONVERTER_HANDLE m_hConverter;
        protected Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> m_convertedBuffers;
        //protected PylonGrabResult_t grabResult; 
        protected bool m_converterOutputFormatIsColor = false;

        protected uint numberOfBuffersUsed = 10;
        public string[] Devicename = new string[NUM_DEVICES];
        public uint numDevices;
        public uint indexd;
        public long oldvalGain;
        protected uint payloadSize;
        protected uint nStreams;
        protected int i;

        protected bool bAcquisitionStartAvailable = false;
        protected bool bFrameStartAvailable = false;
        protected bool m_open = false;
        protected bool isAvail;
        public bool m_grabOnce = false;  //为true时单帧拍照，否则为连续采集
        protected bool m_grabTriggerSoftware = false;
        protected bool m_grabTriggerExternal = false;
        protected string m_lastError = "";
        protected string m_TriggerSelector = "FrameStart";
        protected bool m_grabThreadRun = false;
        public Bitmap m_bitmap = null;
        public HObject mhalcon_image2;
        public bool bGrabDone = false;

        protected Thread m_grabThread;
        protected Object m_lockObject;
        public delegate void ImageReadyEventHandler();
        public event ImageReadyEventHandler ImageReadyEvent;

        public delegate void NotifySaveWarning(string strwarm);//用于显示报警信息
        public event NotifySaveWarning OnNotifySaveWarning;

        public bool IsOpen
        {
            get { return m_open; }
        }
        public static BaslerGigeCamera mCGigecamera;

        public static BaslerGigeCamera Inistance()
        {
            if (mCGigecamera == null) mCGigecamera = new BaslerGigeCamera();
            return mCGigecamera;
        }


        public BaslerGigeCamera()
        {
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
            // PylonC.NET.Pylon.Initialize();
            
            m_lockObject = new Object();
            buffers = new Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>>();
            hDev = new PYLON_DEVICE_HANDLE();
            hDi = new PYLON_DEVICE_INFO_HANDLE();
            wos = new PYLON_WAITOBJECTS_HANDLE();
            m_grabbedBuffers = new List<GrabResult>();
            m_hConverter = new PYLON_IMAGE_FORMAT_CONVERTER_HANDLE();
            m_grabThread = null;
            mhalcon_image2 = new HObject();
        }

        private void SendMsgToDisplayWarning(string strwarning)
        {
            if (OnNotifySaveWarning != null)
                OnNotifySaveWarning(strwarning);
        }

        public uint FindDeviceInitialize()//发现相机并返回相机个数
        {
            try
            {
                Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
                PylonC.NET.Pylon.Initialize();
                numDevices = PylonC.NET.Pylon.EnumerateDevices();
                if (numDevices == 0)
                {
                    PylonC.NET.Pylon.Terminate();
                    throw new Exception("No devices found!");
                    return 0;
                }


            }
            catch (Exception e)
            {
                SendMsgToDisplayWarning("Did not find the CCD!");
                return 0;
            }

            return numDevices;
        }
        private int GetCameraID(string CamID)
        {
            for (int i = 0; i < numDevices; i++)
            {
                if (GetDevicename((uint)i) == CamID)
                    return i;
            }
            return -1;
        }
        public bool OpenCameraByCameraID(string CamID)
        {
            bool res = false;
            if (numDevices > 0)
            {
                int camIdx = GetCameraID(CamID);
                if (camIdx == -1)
                    return res;

                OpenCamera((uint)camIdx);
                res =  true;
            }
            return res;
        }

        public void OpenCamera(uint index)  //根据找到的相机的ID号打开相机
        {
            if (numDevices > 0)
            {

                indexd = index;
                try
                {

                    hDev = PylonC.NET.Pylon.CreateDeviceByIndex(index);
                    PylonC.NET.Pylon.DeviceOpen(hDev, PylonC.NET.Pylon.cPylonAccessModeControl | PylonC.NET.Pylon.cPylonAccessModeStream);
                    m_open = true;
                }
                catch (Exception e)
                {

                    string msg = GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail();
                    if (msg != "\n")
                    {
                        // throw new Exception("Exception caught:" + e.Message + msg);
                      //  MessageBox.Show("Exception caught:" + e.Message + msg);
                        SendMsgToDisplayWarning("Exception caught:" + e.Message + msg);
                    }
                    try
                    {
                        if (hDev.IsValid)
                        {

                            if (PylonC.NET.Pylon.DeviceIsOpen(hDev))
                            {
                                PylonC.NET.Pylon.DeviceClose(hDev);
                                m_open = false;
                            }
                            PylonC.NET.Pylon.DestroyDevice(hDev);
                        }
                    }
                    catch (Exception)
                    {
                        SendMsgToDisplayWarning("Open the CCD failed!" );
                    }

                    PylonC.NET.Pylon.Terminate();

                }

            }

        }

        public string GetDevicename(uint index) //获取相机名根据找到的设备的ID号
        {
            string devicename = "";
            if (IsOpen)
            {
               
                if (numDevices > 0)
                {
                    try
                    {
                        hDi = PylonC.NET.Pylon.GetDeviceInfoHandle(index);
                        devicename = PylonC.NET.Pylon.DeviceInfoGetPropertyValueByName(hDi, PylonC.NET.Pylon.cPylonDeviceInfoFriendlyNameKey);
                    }
                    catch
                    {
                        devicename = "Not found the CCD";
                    }

                }
                else
                {
                    devicename = "Not found the CCD";

                }
               
            }
            else
            {
                devicename = "Not found the CCD";
            }
            return devicename;
        }

        public void SetImageWidth(long lWidth) //设置图像宽度函数
        {
            try
            {
                string featureName = "Width";
                long val, min, max, incr;
                if (IsOpen)
                {
                    min = PylonC.NET.Pylon.DeviceGetIntegerFeatureMin(hDev, featureName);
                    max = PylonC.NET.Pylon.DeviceGetIntegerFeatureMax(hDev, featureName);
                    incr = PylonC.NET.Pylon.DeviceGetIntegerFeatureInc(hDev, featureName);

                    if (lWidth < min)
                    {
                        val = min;
                    }
                    else if (lWidth > max)
                    {
                        val = max;
                    }

                    if (incr == 1)
                    {
                        val = lWidth;
                    }
                    else
                    {
                        val = min + ((lWidth - min) / incr) * incr;
                    }

                    if (PylonC.NET.Pylon.DeviceFeatureIsWritable(hDev, featureName))
                    {
                        PylonC.NET.Pylon.DeviceSetIntegerFeature(hDev, featureName, val);
                    }


                    if (PylonC.NET.Pylon.DeviceFeatureIsWritable(hDev, "CenterX"))
                    {
                        PylonC.NET.Pylon.DeviceSetBooleanFeature(hDev, "CenterX", true);
                    }
                }
               
            }
            catch
            {

                UpdateLastError();

                try
                {
                    Close();
                }
                catch
                {
                    /* Another exception cannot be handled. */
                }
                //throw;
            }
        }

        public void SetImageHeight(long lHeight)  //设置图像高度函数
        {
            try
            {
                string featureName = "Height";  /* Name of the feature used in this sample: AOI Width. */
                long val, min, max, incr;      /* Properties of the feature. */
                if (IsOpen)
                {
                    min = PylonC.NET.Pylon.DeviceGetIntegerFeatureMin(hDev, featureName);  /* Get the minimum value. */
                    max = PylonC.NET.Pylon.DeviceGetIntegerFeatureMax(hDev, featureName);  /* Get the maximum value. */
                    incr = PylonC.NET.Pylon.DeviceGetIntegerFeatureInc(hDev, featureName);  /* Get the increment value. */

                    if (lHeight < min)
                    {
                        val = min;
                    }
                    else if (lHeight > max)
                    {
                        val = max;
                    }

                    if (incr == 1)
                    {
                        val = lHeight;
                    }
                    else
                    {
                        val = min + ((lHeight - min) / incr) * incr;
                    }

                    /* Set the Width to its maximum allowed value. */
                    if (PylonC.NET.Pylon.DeviceFeatureIsWritable(hDev, featureName))
                    {
                        PylonC.NET.Pylon.DeviceSetIntegerFeature(hDev, featureName, val);
                    }


                    if (PylonC.NET.Pylon.DeviceFeatureIsWritable(hDev, "CenterY"))
                    {
                        PylonC.NET.Pylon.DeviceSetBooleanFeature(hDev, "CenterY", true);
                    }
                }
               
            }
            catch
            {
                /* Get the last error message here, because it could be overwritten by cleaning up. */
                UpdateLastError();

                try
                {
                    Close(); /* Try to close any open handles. */
                }
                catch
                {
                    /* Another exception cannot be handled. */
                }
                // throw;
            }
        }

        public void SetImageFormat() //设置图像格式函数
        {
            try
            {
                if (IsOpen)
                {
                    isAvail = PylonC.NET.Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_PixelFormat_Mono8");

                    if (!isAvail)
                    {

                        throw new Exception("Device doesn't support the Mono8 pixel format.");
                    }
                    if (PylonC.NET.Pylon.DeviceFeatureIsWritable(hDev, "PixelFormat"))
                    {
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "PixelFormat", "Mono8");
                    }

                }
               

            }
            catch
            {

                UpdateLastError();

                try
                {
                    Close();
                }
                catch
                {

                }
                //  throw;
            }


        }

        public void SetExposureTimeAbs(double value)  //设置曝光时间
        {
            try
            {
                string featureName = "ExposureTimeAbs";
                bool isAvailable;
                bool isWritable;
                double min, max, oldvalue, newvalue;
                if (IsOpen)
                {
                    newvalue = value;

                    isAvailable = PylonC.NET.Pylon.DeviceFeatureIsAvailable(hDev, featureName);

                    if (isAvailable)
                    {
                        min = PylonC.NET.Pylon.DeviceGetFloatFeatureMin(hDev, featureName);
                        max = PylonC.NET.Pylon.DeviceGetFloatFeatureMax(hDev, featureName);
                        oldvalue = PylonC.NET.Pylon.DeviceGetFloatFeature(hDev, featureName);

                        if (value > max)
                        {
                            newvalue = max;
                        }
                        else if (value < min)
                        {
                            newvalue = min;
                        }

                        isWritable = PylonC.NET.Pylon.DeviceFeatureIsWritable(hDev, featureName);
                        if (isWritable)
                        {
                            PylonC.NET.Pylon.DeviceSetFloatFeature(hDev, featureName, newvalue);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The ExposureTimeAbs feature isn't available.");
                    }

                }
               
            }
            catch
            {

                UpdateLastError();

                try
                {
                    Close();
                }
                catch
                {

                }
                // throw;
            }
        }


        public void SetGain(long value)  //设置相机的增益
        {
            try
            {
                string featureName = "GainRaw";
                long newval, min, max, incr;
                if (IsOpen)
                {
                    min = PylonC.NET.Pylon.DeviceGetIntegerFeatureMin(hDev, featureName);
                    max = PylonC.NET.Pylon.DeviceGetIntegerFeatureMax(hDev, featureName);
                    incr = PylonC.NET.Pylon.DeviceGetIntegerFeatureInc(hDev, featureName);
                    oldvalGain = PylonC.NET.Pylon.DeviceGetIntegerFeature(hDev, featureName);

                    if (value < min)
                    {
                        newval = min;
                    }
                    else if (value > max)
                    {
                        newval = max;
                    }
                    if (incr == 1)
                    {
                        newval = value;
                    }
                    else
                    {
                        newval = min + (((value - min) / incr) * incr);
                    }

                    PylonC.NET.Pylon.DeviceSetIntegerFeature(hDev, featureName, newval);
                }
                

            }
            catch
            {

                UpdateLastError();

                try
                {
                    Close(); /* Try to close any open handles. */
                }
                catch
                {
                    /* Another exception cannot be handled. */
                }
                throw;
            }
        }
        public double GetCamerExposureTimeAbs() //获得相机曝光值
        {
            string featureName = "ExposureTimeAbs";
            double oldExposureTime = 0;
            if (IsOpen)
            {
                try
                {
                    oldExposureTime = PylonC.NET.Pylon.DeviceGetFloatFeature(hDev, featureName); 
                }
                catch
                { 

                }
               
            }

            return oldExposureTime;
        }

        public long GetCamerGain() //获得相机增益
        {
            string featureName = "GainRaw";
            long oldGain=0;
            if (IsOpen)
            {
                try
                {
                    oldGain = PylonC.NET.Pylon.DeviceGetIntegerFeature(hDev, featureName);
                }
                catch
                { 
                }
               
            }
             
             return oldGain;
        }

        public long GetImageWidth()
        {
            string featureName = "Width";
            return GetCameraWidthorHeight(featureName);
        }
        public long GetImageHeight()
        {
            string featureName = "Height";
            return GetCameraWidthorHeight(featureName);
        }
        public long GetCameraWidthorHeight(string Name)  // 获取当前图像的"Width"和"Height";
        {
            string featureName = Name;
            long  incr=0;
            if (IsOpen)
            {
                try
                {
                    incr = PylonC.NET.Pylon.DeviceGetIntegerFeature(hDev, featureName);
                }
                catch
                { 
                }
                
            }
 
            return incr;
        }
        public void GrabPrepare()   //准备采集，做采集的准备工作
        {
            try
            {
                isAvail = PylonC.NET.Pylon.DeviceFeatureIsWritable(hDev, "GevSCPSPacketSize");
                if (isAvail)
                {
                    PylonC.NET.Pylon.DeviceSetIntegerFeature(hDev, "GevSCPSPacketSize", 1500);
                }
                payloadSize = checked((uint)PylonC.NET.Pylon.DeviceGetIntegerFeature(hDev, "PayloadSize"));

                nStreams = PylonC.NET.Pylon.DeviceGetNumStreamGrabberChannels(hDev);

                if (nStreams < 1)
                {
                    throw new Exception("The transport layer doesn't support image streams.");
                }

                hGrabber = PylonC.NET.Pylon.DeviceGetStreamGrabber(hDev, 0);
                PylonC.NET.Pylon.StreamGrabberOpen(hGrabber);

                hWaitGrabFinished = PylonC.NET.Pylon.StreamGrabberGetWaitObject(hGrabber);

                PylonC.NET.Pylon.StreamGrabberSetMaxNumBuffer(hGrabber, numberOfBuffersUsed);
                PylonC.NET.Pylon.StreamGrabberSetMaxBufferSize(hGrabber, payloadSize);
                PylonC.NET.Pylon.StreamGrabberPrepareGrab(hGrabber);
                buffers = new Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>>();
                for (i = 0; i < numberOfBuffersUsed; ++i)
                {
                    PylonBuffer<Byte> buffer = new PylonBuffer<byte>(payloadSize, true);
                    PYLON_STREAMBUFFER_HANDLE handle = PylonC.NET.Pylon.StreamGrabberRegisterBuffer(hGrabber, ref buffer);
                    buffers.Add(handle, buffer);
                }
                i = 0;
                foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in buffers)
                {
                    PylonC.NET.Pylon.StreamGrabberQueueBuffer(hGrabber, pair.Key, i++);
                }
                wos = PylonC.NET.Pylon.WaitObjectsCreate();
                hWaitGrabStop = PylonC.NET.Pylon.WaitObjectCreate();
                PylonC.NET.Pylon.WaitObjectReset(hWaitGrabStop);
                PylonC.NET.Pylon.WaitObjectsAdd(wos, hWaitGrabStop);
                PylonC.NET.Pylon.WaitObjectsAdd(wos, hWaitGrabFinished);
                if (m_grabOnce)
                {

                    PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "AcquisitionMode", "SingleFrame");
                }
                else
                {
                    PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "AcquisitionMode", "Continuous");
                }

                bAcquisitionStartAvailable = PylonC.NET.Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_AcquisitionStart");
                bFrameStartAvailable = PylonC.NET.Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_FrameStart");

                /* Set the software FrameStartTrigger mode */
                if (m_grabTriggerSoftware)
                {

                    if (bAcquisitionStartAvailable && bFrameStartAvailable)
                    {
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "AcquisitionStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");


                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "FrameStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "On");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSource", "Software");
                        m_TriggerSelector = "FrameStart";
                    }
                    else if (bAcquisitionStartAvailable && !bFrameStartAvailable)
                    {
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "AcquisitionStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "On");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSource", "Software");
                        m_TriggerSelector = "AcquisitionStart";
                    }
                }
                else if (m_grabTriggerExternal)/* Set the external FrameStartTrigger mode */
                {

                    if (bAcquisitionStartAvailable && bFrameStartAvailable)
                    {
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "AcquisitionStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");


                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "FrameStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "On");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSource", "Line1");
                        m_TriggerSelector = "FrameStart";
                    }
                    else if (bAcquisitionStartAvailable && !bFrameStartAvailable)
                    {
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "AcquisitionStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "On");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSource", "Line1");
                        m_TriggerSelector = "AcquisitionStart";
                    }
                }
                else
                {

                    if (bAcquisitionStartAvailable)
                    {
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "AcquisitionStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");

                    }

                    if (bFrameStartAvailable)
                    {
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "FrameStart");
                        PylonC.NET.Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");

                    }
                }

                m_hConverter.SetInvalid();
            }
            catch
            {
                Close();

            }



        }

        public void GrabCleanup()  //采集完成后的清理工作
        {
            try
            {

                if (m_hConverter.IsValid)
                {
                    PylonC.NET.Pylon.ImageFormatConverterDestroy(m_hConverter);

                    m_hConverter.SetInvalid();

                    foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in m_convertedBuffers)
                    {
                        pair.Value.Dispose();
                    }
                    m_convertedBuffers = null;
                }

                PylonC.NET.Pylon.StreamGrabberCancelGrab(hGrabber);

                {
                    bool isReady;
                    do
                    {
                        PylonGrabResult_t grabResult;
                        isReady = PylonC.NET.Pylon.StreamGrabberRetrieveResult(hGrabber, out grabResult);

                    } while (isReady);
                }

                foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in buffers)
                {
                    PylonC.NET.Pylon.StreamGrabberDeregisterBuffer(hGrabber, pair.Key);
                }

                foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in buffers)
                {
                    pair.Value.Dispose();
                }
                buffers.Clear();


                PylonC.NET.Pylon.StreamGrabberFinishGrab(hGrabber);
                PylonC.NET.Pylon.StreamGrabberClose(hGrabber);
                hGrabber = null;

                PylonC.NET.Pylon.WaitObjectsRemoveAll(wos);
                PylonC.NET.Pylon.WaitObjectDestroy(hWaitGrabStop);
                PylonC.NET.Pylon.WaitObjectsDestroy(wos);
            }
            catch
            {
                Close();

            }
        }

        public void GrabStart()  //开始采集
        {
            //  halcon_image.Dispose();
            try
            {

                PylonC.NET.Pylon.DeviceExecuteCommandFeature(hDev, "AcquisitionStart");
            }
            catch
            {

                UpdateLastError();

                try
                {
                    Close();
                }
                catch
                {

                }
                // throw;
            }


        }

        public void GrabStop()  //停止采集
        {
            try
            {

                PylonC.NET.Pylon.DeviceExecuteCommandFeature(hDev, "AcquisitionStop");
            }
            catch
            {
                UpdateLastError();

                try
                {
                    Close();
                }
                catch
                {

                }
                throw;
            }


        }

        //public void GrabOnce()
        //{
        //    GrabPrepare();
        //    GrabStart();
        //    try
        //    {
        //        PylonGrabResult_t grabResult;
        //        if (!PylonC.NET.Pylon.StreamGrabberRetrieveResult(hGrabber, out grabResult))
        //        {
        //            throw new Exception("Failed to retrieve a grab result.");
        //        }
        //        int bufferIndex;
        //        bufferIndex = (int)grabResult.Context;
        //        if (grabResult.Status == EPylonGrabStatus.Grabbed)
        //        {
        //            PylonBuffer<Byte> buffer;
        //            if (!buffers.TryGetValue(grabResult.hBuffer, out buffer))
        //            {
        //                throw new Exception("Failed to find the buffer associated with the handle returned in grab result.");
        //            }
        //            //////////////////////////////////////////////////////////////////
        //            // grabResult.hBuffer


        //            //////////////////////////////////////////////////////////////////


        //            EnqueueTakenImage(grabResult);


        //            Image image = GetLatestImage();
        //            if (image != null)
        //            {
        //                mhalcon_image2.GenEmptyObj();
        //                if (BaslerGigeCamera.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
        //                {

        //                    mhalcon_image2 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
        //                    // mCGigecamera.GenertateGrayBitmap(mhalcon_image, out mm_bitmap);
        //                    /* To show the new image, request the display control to update itself. */
        //                    //pictureBox1.Refresh();

        //                }
        //                else
        //                {
        //                    CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
        //                    mhalcon_image2 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);

        //                    //Bitmap bitmap = pictureBox1.Image as Bitmap;
        //                    /////* Provide the display control with the new bitmap. This action automatically updates the display. */
        //                    //pictureBox1.Image = m_bitmap;
        //                    //if (bitmap != null)
        //                    //{
        //                    //    /* Dispose the bitmap. */
        //                    //    bitmap.Dispose();
        //                    //}
        //                    //    pictureBox1.Refresh();
        //                    //}
        //                }
        //                m_bitmap = null;
        //                ReleaseImage();
        //            }




        //            OnImageReadyEvent();
        //            // OnImageReadyshow();

        //            /* Perform processing. */
        //            //buffer.Array is you image data
        //            //getMinMax(buffer.Array, grabResult.SizeX, grabResult.SizeY, out min, out max);
        //            //Console.WriteLine("Grabbed frame {0} into buffer {1}. Min. gray value = {2}, Max. gray value = {3}",
        //            //    nGrabs, bufferIndex, min, max);
        //            // ReleaseImage();

        //            if (m_grabOnce)
        //            {
        //                m_grabThreadRun = false;
        //                break;
        //            }

        //        }
        //        else if (grabResult.Status == EPylonGrabStatus.Failed)
        //        {
        //            throw new Exception(string.Format("A grab failure occurred. See the method ImageProvider::Grab for more information. The error code is {0:X08}.", grabResult.ErrorCode));
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        public void GrabThread()  //采集图像线程函数
        {
            GrabPrepare();
            GrabStart();

            uint woIndex;
            try
            {
                while (m_grabThreadRun)
                {
                    //if (!PylonC.NET.Pylon.WaitObjectWait(wos, 15000))
                    //{
                    //    lock (m_lockObject)
                    //    {
                    //        if (m_grabbedBuffers.Count != numberOfBuffersUsed)
                    //        {
                    //            /* A timeout occurred. This can happen if an external trigger is used or
                    //               if the programmed exposure time is longer than the grab timeout. */
                    //            throw new Exception("A grab timeout occurred.");
                    //        }
                    //        continue;
                    //    }
                    //}
                    bGrabDone = false;
                    bool Bwaitobject = false;
                    Bwaitobject = PylonC.NET.Pylon.WaitObjectsWaitForAny(wos, 5000, out woIndex);
                    if (!Bwaitobject)
                    {
                        lock (m_lockObject)
                        {
                            if (woIndex != numberOfBuffersUsed)
                            {
                                MessageBox.Show("A grab timeout occurred.");
                            }
                            continue;
                        }
                    }
                    //if (woIndex == 0)
                    //{
                    //m_grabThreadRun = false;
                    //       // break;
                    //}
                    //else 
                    //{
                    //   if (woIndex == 1)
                    //   {
                    //     if (m_grabOnce)
                    //    {
                    //        m_grabThreadRun = false;
                    //       // break;
                    //    }
                    //   }




                    PylonGrabResult_t grabResult;
                    if (!PylonC.NET.Pylon.StreamGrabberRetrieveResult(hGrabber, out grabResult))
                    {
                        throw new Exception("Failed to retrieve a grab result.");
                    }
                    int bufferIndex;
                    bufferIndex = (int)grabResult.Context;
                    if (grabResult.Status == EPylonGrabStatus.Grabbed)
                    {
                        PylonBuffer<Byte> buffer;
                        if (!buffers.TryGetValue(grabResult.hBuffer, out buffer))
                        {
                            throw new Exception("Failed to find the buffer associated with the handle returned in grab result.");
                        }
                        //////////////////////////////////////////////////////////////////
                        // grabResult.hBuffer


                        //////////////////////////////////////////////////////////////////


                        EnqueueTakenImage(grabResult);


                        Image image = GetLatestImage();
                        if (image != null)
                        {
                            mhalcon_image2.GenEmptyObj();
                            if (BaslerGigeCamera.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                            {

                                mhalcon_image2 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                                // mCGigecamera.GenertateGrayBitmap(mhalcon_image, out mm_bitmap);
                                /* To show the new image, request the display control to update itself. */
                                //pictureBox1.Refresh();

                            }
                            else
                            {
                                CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                                mhalcon_image2 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);

                                //Bitmap bitmap = pictureBox1.Image as Bitmap;
                                /////* Provide the display control with the new bitmap. This action automatically updates the display. */
                                //pictureBox1.Image = m_bitmap;
                                //if (bitmap != null)
                                //{
                                //    /* Dispose the bitmap. */
                                //    bitmap.Dispose();
                                //}
                                //    pictureBox1.Refresh();
                                //}
                            }
                            m_bitmap = null;
                            ReleaseImage();
                        }




                        OnImageReadyEvent();
                        // OnImageReadyshow();

                        /* Perform processing. */
                        //buffer.Array is you image data
                        //getMinMax(buffer.Array, grabResult.SizeX, grabResult.SizeY, out min, out max);
                        //Console.WriteLine("Grabbed frame {0} into buffer {1}. Min. gray value = {2}, Max. gray value = {3}",
                        //    nGrabs, bufferIndex, min, max);
                        // ReleaseImage();

                        if (m_grabOnce)
                        {
                            m_grabThreadRun = false;
                            break;
                        }

                    }
                    else if (grabResult.Status == EPylonGrabStatus.Failed)
                    {
                        throw new Exception(string.Format("A grab failure occurred. See the method ImageProvider::Grab for more information. The error code is {0:X08}.", grabResult.ErrorCode));
                    }
                    //PylonC.NET.Pylon.StreamGrabberQueueBuffer(hGrabber, grabResult.hBuffer, bufferIndex);
                    while (!bGrabDone)
                        Thread.Sleep(5);
                }
                //}
              
            }
            catch
            {
                m_grabThreadRun = false;
                string lastErrorMessage = GetLastErrorText();
                try
                {
                    GrabStop();

                    GrabCleanup();
                }
                catch
                {

                }
            }

            try
            {
                GrabStop();

                GrabCleanup();


            }
            catch
            {

            }

        }


        public void OneShot() //开始采集图像
        {
            try
            {
                if (null == m_grabThread)
                {
                    if (m_open)
                    {
                        numberOfBuffersUsed = 1;
                        m_grabOnce = true;
                        m_grabTriggerSoftware = false;
                        m_grabTriggerExternal = false;
                        m_grabThreadRun = true;
                        m_grabThread = new Thread(GrabThread);
                        m_grabThread.Start();
                    }
                }
                else
                {
                    if (m_open && !m_grabThread.IsAlive)
                    {
                        numberOfBuffersUsed = 1;
                        m_grabOnce = true;
                        m_grabTriggerSoftware = false;
                        m_grabTriggerExternal = false;
                        m_grabThreadRun = true;
                        m_grabThread = new Thread(new ThreadStart(GrabThread));
                        m_grabThread.IsBackground = true;
                        m_grabThread.Start();
                    }
                }
                
            }
            catch
            { }

        }

        public void ContinuousShot()  //实时采集图像函数
        {
            //if (m_grabThread != null)
            //{
            //    if (!m_grabThread.IsAlive)
            //    {
            //        m_grabThread = null;
            //    }
            //}
            if (null == m_grabThread)
            {
                if (m_open )
                {
                    numberOfBuffersUsed = 10;
                    m_grabOnce = false;
                    m_grabTriggerSoftware = false;
                    m_grabTriggerExternal = false;
                    m_grabThreadRun = true;
                    m_grabThread = new Thread(new ThreadStart(GrabThread));
                    m_grabThread.IsBackground = true;
                    m_grabThread.Start();
                }
            }
            else
            {
                if (m_open && !m_grabThread.IsAlive)
                {
                    numberOfBuffersUsed = 10;
                    m_grabOnce = false;
                    m_grabTriggerSoftware = false;
                    m_grabTriggerExternal = false;
                    m_grabThreadRun = true;
                    m_grabThread = new Thread(new ThreadStart(GrabThread));
                    m_grabThread.IsBackground = true;
                    m_grabThread.Start();
                }
            }
            
            
            
        }

        public void SoftwareTriggerShot(bool SingleFrame)  //软件触发单帧图像采集函数，当SingleFrame为true时单帧采集，否则为连续采集
        {
           
            if (null == m_grabThread)
            {
                if (m_open)
                {
                    if (SingleFrame)
                    {
                        numberOfBuffersUsed = 1;
                        m_grabOnce = true;
                    }
                    else
                    {
                        numberOfBuffersUsed = 10;
                        m_grabOnce = false;
                    }
                    m_grabTriggerSoftware = true;
                    m_grabTriggerExternal = false;
                    m_grabThreadRun = true;
                    m_grabThread = new Thread(new ThreadStart(GrabThread));
                    m_grabThread.IsBackground = true;
                    m_grabThread.Start();
                }
            }
           
        }

        public void ExternalTriggerShot()  //外部硬件触发函数
        {
            if (m_open /*&& !m_grabThread.IsAlive*/)
            {
                numberOfBuffersUsed = 10;
                m_grabOnce = false;
                m_grabTriggerSoftware = false;
                m_grabTriggerExternal = true;
                m_grabThreadRun = true;
                m_grabThread = new Thread(GrabThread);
                m_grabThread.IsBackground = true;
                m_grabThread.Start();
            }
        }

        public bool IsLive()
        {
            return m_grabThreadRun;
        }

        public void StopThread()//停止采集图像
        {
            if (m_grabThread != null)
            {
                //m_grabThread.Abort();
                //if (!m_grabThread.IsAlive)
                //{
                //    m_grabThread = null;
                //}
                if (m_open) /* Only start when open and grabbing. */
                {
                    if (m_grabThread.IsAlive)
                    {
                        m_grabThreadRun = false; /* Causes the grab thread to stop. */
                        //Thread.Sleep(10);
                        m_grabThread.Abort();
                        m_grabThread.Join(); /* Wait for it to stop. */
                    }
                    else
                    {
                        m_grabThread = null;
                    }


                }
            }
           
        }

        private void EnqueueTakenImage(PylonGrabResult_t grabResult)   //将采集到的数据存储到另一缓存中
        {
            PylonBuffer<Byte> buffer;  /* Reference to the buffer attached to the grab result. */

            /* Get the buffer from the dictionary. */
            if (!buffers.TryGetValue(grabResult.hBuffer, out buffer))
            {
                /* Oops. No buffer available? We should never have reached this point. Since all buffers are
                   in the dictionary. */
                throw new Exception("Failed to find the buffer associated with the handle returned in grab result.");
            }
            GrabResult newGrabResultInternal = new GrabResult();
            newGrabResultInternal.Handle = grabResult.hBuffer;
            if (grabResult.PixelType == EPylonPixelType.PixelType_Mono8 || grabResult.PixelType == EPylonPixelType.PixelType_RGBA8packed)
            {
                newGrabResultInternal.ImageData = new Image(grabResult.SizeX, grabResult.SizeY, buffer.Array, grabResult.PixelType == EPylonPixelType.PixelType_RGBA8packed);
            }
            else
            {
                if (!m_hConverter.IsValid)
                {
                    m_convertedBuffers = new Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<byte>>(); /* Create a new dictionary for the converted buffers. */
                    m_hConverter = PylonC.NET.Pylon.ImageFormatConverterCreate(); /* Create the converter. */
                    m_converterOutputFormatIsColor = !PylonC.NET.Pylon.IsMono(grabResult.PixelType) || PylonC.NET.Pylon.IsBayer(grabResult.PixelType);
                }
                PylonBuffer<Byte> convertedBuffer = null;
                bool bufferListed = m_convertedBuffers.TryGetValue(grabResult.hBuffer, out convertedBuffer);
                PylonC.NET.Pylon.ImageFormatConverterSetOutputPixelFormat(m_hConverter, m_converterOutputFormatIsColor ? EPylonPixelType.PixelType_BGRA8packed : EPylonPixelType.PixelType_Mono8);
                PylonC.NET.Pylon.ImageFormatConverterConvert(m_hConverter, ref convertedBuffer, buffer, grabResult.PixelType, (uint)grabResult.SizeX, (uint)grabResult.SizeY, (uint)grabResult.PaddingX, EPylonImageOrientation.ImageOrientation_TopDown);
                if (!bufferListed)
                {
                    m_convertedBuffers.Add(grabResult.hBuffer, convertedBuffer);
                }
                newGrabResultInternal.ImageData = new Image(grabResult.SizeX, grabResult.SizeY, convertedBuffer.Array, m_converterOutputFormatIsColor);
            }
            lock (m_lockObject)
            {
                m_grabbedBuffers.Add(newGrabResultInternal);
            }
        }



        public Image GetLatestImage() //获取存储序列中最后一副图像函数
        {
            lock (m_lockObject) /* Lock the grab result queue to avoid that two threads modify the same data. */
            {
                /* Release all images but the latest. */
                while (m_grabbedBuffers.Count > 1)
                {
                    ReleaseImage();
                }
                if (m_grabbedBuffers.Count > 0) /* If images available. */
                {
                    return m_grabbedBuffers[0].ImageData;
                }
            }
            return null; /* No image available. */
        }

        public bool ReleaseImage()  //释放缓存中图像
        {
            lock (m_lockObject) /* Lock the grab result queue to avoid that two threads modify the same data. */
            {
                if (m_grabbedBuffers.Count > 0) /* If images are available and grabbing is in progress.*/
                {
                    if (m_grabThreadRun)
                    {
                        /* Requeue the buffer. */
                        PylonC.NET.Pylon.StreamGrabberQueueBuffer(hGrabber, m_grabbedBuffers[0].Handle, 0);
                    }
                    /* Remove it from the grab result queue. */
                    m_grabbedBuffers.RemoveAt(0);
                    return true;
                }
            }
            return false;
        }


        public static bool IsCompatible(Bitmap bitmap, int width, int height, bool color) //比较图像属性函数
        {
            if (bitmap == null
                || bitmap.Height != height
                || bitmap.Width != width
                || bitmap.PixelFormat != GetFormat(color)
             )
            {
                return false;
            }
            return true;
        }

        private static PixelFormat GetFormat(bool color)
        {
            return color ? PixelFormat.Format32bppRgb : PixelFormat.Format8bppIndexed;
        }

        private static int GetStride(int width, bool color)
        {
            return color ? width * 4 : width;
        }

        public HObject UpdateBitmap(Bitmap bitmap, byte[] buffer, int width, int height, bool color) //强Byte的数组转换为Bitmap类型，然后再转换为HObject类型图像并输出
        {
            HObject halcon_image = new HObject();
            halcon_image.GenEmptyObj();
            /* Check if the bitmap can be updated with the image data. */
            if (!IsCompatible(bitmap, width, height, color))
            {
                throw new Exception("Cannot update incompatible bitmap.");
            }

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptrBmp = bmpData.Scan0;

            int imageStride = GetStride(width, color);
            if (imageStride == bmpData.Stride)
            {
                System.Runtime.InteropServices.Marshal.Copy(buffer, 0, ptrBmp, bmpData.Stride * bitmap.Height);
            }
            else
            {
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    Marshal.Copy(buffer, i * imageStride, new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), width);
                }
            }
            //////////////////////////////////////////////////////////////////////////////////Bitmap转化为halcon 图像格式
            //HOperatorSet.GenEmptyObj(out halcon_image);
            // HObject halcon_image=new HObject();
            HOperatorSet.GenImage1(out halcon_image, "byte", bitmap.Width, bitmap.Height, bmpData.Scan0);
           // halcon_image.GenImage1("byte", bitmap.Width, bitmap.Height, bmpData.Scan0);
            //halcon_image.GenImage1Extern("byte", bitmap.Width, bitmap.Height, bmpData.Scan0, 0);
           // HOperatorSet.GenImage1Extern(out halcon_image, "byte", (HTuple)(bitmap.Width * 4), (HTuple)bitmap.Height, (HTuple)ptrBmp, 0);


            bitmap.UnlockBits(bmpData);
            return halcon_image;
        }
        #region
        //Bitmap 转halcon函数
        //public HObject HImageConvertFromBitmap8(Bitmap bmp)
        //{
        //    HObject ho_Image;
        //    HOperatorSet.GenEmptyObj(out ho_Image);
        //    unsafe
        //    {
        //        BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //        unsafe
        //        {
        //           // HOperatorSet.GenImageInterleaved(out ho_Image, bmpData.Scan0, "bgrx", bmp.Width, bmp.Height, -1, "byte", bmp.Width, bmp.Height, 0, 0, -1, 0);
        //           HOperatorSet.GenImage1(out ho_Image, "byte", bmp.Width, bmp.Height, bmpData.Scan0);

        //        }
        //        bmp.UnlockBits(bmpData);
        //        return ho_Image;
        //    }

        //}
        #endregion


        /* Creates a new bitmap object with the supplied properties. */
        public void CreateBitmap(out Bitmap bitmap, int width, int height, bool color)
        {
            bitmap = new Bitmap(width, height, GetFormat(color));

            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = colorPalette;
            }
        }

        protected void OnImageReadyEvent()
        {
            if (ImageReadyEvent != null)
            {
                ImageReadyEvent();
            }
        }

        //private void ConvertToHalcon()   //将halcon的图像显示到picture.box中
        //{
        //    try
        //    {
        //      Image image = GetLatestImage();

        //        if (image != null)
        //        {
        //            if (CGigecamera.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
        //            {
        //                UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
        //                //  pictureBox.Refresh();
        //            }
        //            else
        //            {
        //                CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
        //               UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);

        //            }

        //            ReleaseImage();

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e + GetLastErrorText());
        //    }
        //}


        #region
        private void UpdateLastError()
        {
            m_lastError = GetLastErrorText();
        }
        public string GetLastErrorText()
        {
            string lastErrorMessage = GenApi.GetLastErrorMessage();
            string lastErrorDetail = GenApi.GetLastErrorDetail();

            string lastErrorText = lastErrorMessage;
            if (lastErrorDetail.Length > 0)
            {
                lastErrorText += "\n\nDetails:\n";
            }
            lastErrorText += lastErrorDetail;
            return lastErrorText;
        }
        public void Close()
        {
            Exception lastException = null;
            if (hGrabber != null && hGrabber.IsValid)
            {
                try
                {

                    PylonC.NET.Pylon.StreamGrabberClose(hGrabber);
                    hGrabber.SetInvalid();
                    hGrabber = null;
                }
                catch (Exception e)
                {
                    //lastException = e; UpdateLastError();
                }
            }

            if (hDev != null && hDev.IsValid)
            {
                /* Try to close the device. */
                try
                {
                    /* ... Close and release the pylon device. */
                    if (PylonC.NET.Pylon.DeviceIsOpen(hDev))
                    {
                        PylonC.NET.Pylon.DeviceClose(hDev);
                    }
                }
                catch (Exception e)
                {
                    // lastException = e;
                    // UpdateLastError();
                }

                /* Try to destroy the device. */
                try
                {
                    PylonC.NET.Pylon.DestroyDevice(hDev);
                    hDev.SetInvalid();
                    hDev = null;
                }
                catch (Exception e)
                {
                    //lastException = e; UpdateLastError();
                }
            }

            PylonC.NET.Pylon.Terminate();

            /* If an exception occurred throw it. */
            if (lastException != null)
            {
                // throw lastException;
                MessageBox.Show(lastException.Message);
            }
        }
        #endregion
    }
}
