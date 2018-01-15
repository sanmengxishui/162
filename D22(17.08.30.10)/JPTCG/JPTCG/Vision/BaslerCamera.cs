using HalconDotNet;
using PylonC.NET;
using PylonC.NETSupportLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Vision
{
    public class BaslerCamera
    {
        private ImageProvider m_imageProvider = new ImageProvider();
        object sny_Obj = new object();
        public bool bGrabDone = false;
        public Bitmap m_bitmap = null;
        public HObject mhalcon_image2;
        public HObject mhalcon_image1;
        public Byte[] imgPtr;
        public delegate void ImageReadyEventHandler();
        public event ImageReadyEventHandler ImageReadyEvent;
        public uint numDevices = 0;
        protected PYLON_DEVICE_INFO_HANDLE hDi;
        protected Thread m_grabThread;
        bool m_grabThreadRun = false;

        public static bool IsInitPylon = false;
        public BaslerCamera()
        {
            Init_Camera_Callback();

            if (!IsInitPylon)
            {
                Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "4000" /*ms*/);
                Pylon.Initialize();
                IsInitPylon = true;
            }
            numDevices = Pylon.EnumerateDevices();
            mhalcon_image2 = new HObject();
            mhalcon_image1 = new HObject();
        }

        public bool IsOpen
        {
            get { return m_imageProvider.IsOpen; }
        }

        private void Init_Camera_Callback()
        {
            try
            {
                /* Register for the events of the image provider needed for proper operation. */
                m_imageProvider.GrabErrorEvent += new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback);
                m_imageProvider.DeviceRemovedEvent += new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback);
                m_imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
                m_imageProvider.DeviceClosedEvent += new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback);
                m_imageProvider.GrabbingStartedEvent += new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback);
                m_imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
                m_imageProvider.GrabbingStoppedEvent += new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void Release_PR_Camera_Callback()
        {
            try
            {
                //lock (sny_Obj)
                {
                    /* Register for the events of the image provider needed for proper operation. */
                    m_imageProvider.GrabErrorEvent -= new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback);
                    m_imageProvider.DeviceRemovedEvent -= new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback);
                    m_imageProvider.DeviceOpenedEvent -= new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
                    m_imageProvider.DeviceClosedEvent -= new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback);
                    m_imageProvider.GrabbingStartedEvent -= new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback);
                    m_imageProvider.ImageReadyEvent -= new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
                    m_imageProvider.GrabbingStoppedEvent -= new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback);

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void OnGrabErrorEventCallback(Exception grabException, string additionalErrorMessage)
        {
            //try
            //{
            //    if (!this.Dispatcher.CheckAccess())
            //    {
            //        /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //        this.Dispatcher.BeginInvoke(new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback),
            //                                                                            grabException,
            //                                                                            additionalErrorMessage);
            //        return;
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
            //ShowException(grabException, additionalErrorMessage);
        }

        /* Handles the event related to a device being closed. */
        private void OnDeviceClosedEventCallback()
        {
            //if (!this.Dispatcher.CheckAccess())
            //{
            //    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //    this.Dispatcher.BeginInvoke(new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback));
            //    return;
            //}

            /* The image provider is closed. Disable all buttons. */
            //EnableButtons(false, false);
        }

        /* Handles the event related to the image provider executing grabbing. */
        private void OnGrabbingStartedEventCallback()
        {
            //if (!this.Dispatcher.CheckAccess())
            //{
            //    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //    this.Dispatcher.BeginInvoke(new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback));
            //    return;
            //}

            /* Do not update device list while grabbing to avoid jitter because the GUI-Thread is blocked for a short time when enumerating. */
            //updateDeviceListTimer.Stop();

            /* The image provider is grabbing. Disable the grab buttons. Enable the stop button. */
            //EnableButtons(false, true);
        }

        protected void OnImageReadyEvent()
        {
            if (ImageReadyEvent != null)
            {
                ImageReadyEvent();
            }
        }

        public int ImgHeight = 0;
        public int ImgWidth = 0;
        /* Handles the event related to an image having been taken and waiting for processing. */
        private void OnImageReadyEventCallback()
        {
            try
            {                
                lock (sny_Obj)
                {
                    //bGrabDone = false;
                    System.Threading.Thread.Sleep(3);
                    /* Acquire the image from the image provider. Only show the latest image. The camera may acquire images faster than images can be displayed*/
                    ImageProvider.Image image = m_imageProvider.GetLatestImage();

                    /* Check if the image has been removed in the meantime. */
                    if (image != null)
                    {
                        mhalcon_image2.GenEmptyObj();
                        mhalcon_image1.GenEmptyObj();

                        if (IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                        {

                            mhalcon_image2 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                            mhalcon_image1 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                            // mCGigecamera.GenertateGrayBitmap(mhalcon_image, out mm_bitmap);
                            /* To show the new image, request the display control to update itself. */
                            //pictureBox1.Refresh();

                        }
                        else
                        {
                            CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                            mhalcon_image2 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                            mhalcon_image1 = UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);

                        }
                        ImgHeight = image.Height;
                        ImgWidth = image.Width;
                        /* The processing of the image is done. Release the image buffer. */
                        m_imageProvider.ReleaseImage();
                        //BitmapData bmpData = m_bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        /* Get the pointer to the bitmap's buffer. */
                        //IntPtr ptrBmp = bmpData.Scan0;

                        OnImageReadyEvent(); 
                    }
                    while (!bGrabDone)
                        Thread.Sleep(5);

                    
                    Thread.Sleep(10);
                }
            }
            catch (Exception)
            {

                //ShowException(e, m_imageProvider.GetLastErrorMessage());
            }
        }
        public int MyImgStride = 0;
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
            MyImgStride = bmpData.Stride;
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
            imgPtr = new byte[buffer.Length];
            //Marshal.Copy(imgPtr, 0, buffer, bmpData.Stride * bitmap.Height);
            Buffer.BlockCopy(buffer, 0, imgPtr, 0, buffer.Length);
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
        /* Handles the event related to the image provider having stopped grabbing. */
        private void OnGrabbingStoppedEventCallback()
        {
            //try
            //{
            //    if (!this.Dispatcher.CheckAccess())
            //    {
            //        /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //        this.Dispatcher.BeginInvoke(new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback));
            //        return;
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }

            ///* Enable device list update again */
            //updateDeviceListTimer.Start();

            ///* The image provider stopped grabbing. Enable the grab buttons. Disable the stop button. */
            //EnableButtons(m_imageProvider.IsOpen, false);
        }

        /* Handles the event related to a device being open. */
        private void OnDeviceOpenedEventCallback()
        {
            //try
            //{
            //    if (!this.Dispatcher.CheckAccess())
            //    {
            //        /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //        this.Dispatcher.BeginInvoke(new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback));
            //        return;
            //    }
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }

            /* The image provider is ready to grab. Enable the grab buttons. */
            //EnableButtons(true, false);
        }

        private void OnDeviceRemovedEventCallback()
        {
            //try
            //{
            //    if (!this.Dispatcher.CheckAccess())
            //    {
            //        /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //        this.Dispatcher.BeginInvoke(new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback));
            //        return;
            //    }
            //    ///* Disable the buttons. */
            //    //EnableButtons(false, false);
            //    /* Stops the grabbing of images. */
            //    Stop();
            //    /* Close the image provider. */
            //    CloseTheImageProvider();
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }

            ///* Since one device is gone, the list needs to be updated. */
            //UpdateDeviceList();
        }

        public void CloseCamera()        
        {
            m_imageProvider.Stop();
            m_imageProvider.Close();
        }

        public bool OpenCameraByCameraID(string CamID)
        {
            bool res = false;
            
                int camIdx = GetCameraID(CamID);
                if (camIdx == -1)
                    return res;

                try
                {
                    if (!m_imageProvider.IsOpen)
                    {
                        m_imageProvider.Open((uint)camIdx);                       
                    }
                    res = true;
                    //m_imageProvider.ContinuousShot(); /* Start the grabbing of images until grabbing is stopped. */
                }
                catch (Exception)
                {
                    //ShowException(e, m_imageProvider.GetLastErrorMessage());
                    res = false;
                }
                //res = true;
            
            return res;
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
        public string GetDevicename(uint index) //获取相机名根据找到的设备的ID号
        {
            string devicename = "";

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


            return devicename;
        }

        //public void ContinuousShot()
        //{
        //    try
        //    {
        //        IsLive = true;
        //        m_imageProvider.ContinuousShot(); /* Start the grabbing of images until grabbing is stopped. */
        //    }
        //    catch (Exception)
        //    {
        //        //ShowException(e, m_imageProvider.GetLastErrorMessage());
        //    }
        //}
        public void ContinuousShot()
        {
            if (null == m_grabThread)
            {
                if (m_imageProvider.IsOpen)
                {
                    m_grabThreadRun = true;
                    m_grabThread = new Thread(new ThreadStart(GrabThread));
                    m_grabThread.IsBackground = true;
                    m_grabThread.Start();
                }
            }
            else
            {
                if (m_imageProvider.IsOpen && !m_grabThread.IsAlive)
                {
                    m_grabThreadRun = true;
                    m_grabThread = new Thread(new ThreadStart(GrabThread));
                    m_grabThread.IsBackground = true;
                    m_grabThread.Start();
                }
            }
            IsLive = true;
        }

        public void GrabThread()  //采集图像线程函数
        {            
            try
            {
                while (m_grabThreadRun)
                {                    
                    OneShot();
                    
                    //mhalcon_image2.Dispose();
                    Thread.Sleep(20);
                    //Application.DoEvents();
                }                
            }
            catch
            {
                m_grabThreadRun = false;                
            }            
        }

        public bool IsLive = false;

        public void Stop()
        {
            if (m_grabThread != null)
            {
                if (m_imageProvider.IsOpen) /* Only start when open and grabbing. */
                {
                    if (m_grabThread.IsAlive)
                    {
                        m_grabThreadRun = false; /* Causes the grab thread to stop. */                        
                        m_grabThread.Abort();
                        m_grabThread.Join(); /* Wait for it to stop. */
                    }
                    else
                    {
                        m_grabThread = null;
                    }
                }
            }           
            IsLive = false;            
        }

        public bool OneShot()
        {
            try
            {
                bGrabDone = false;
                if (m_imageProvider.IsOpen)
                    m_imageProvider.OneShot();

                DateTime st_time = DateTime.Now;
                TimeSpan time_span;
                while (!bGrabDone)
                {                    
                    Thread.Sleep(5);
                    Application.DoEvents();
                    time_span = DateTime.Now - st_time;
                    if (time_span.TotalMilliseconds > 800)
                    {                        
                        bGrabDone = true;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                //ShowException(e, m_imageProvider.GetLastErrorMessage());
            }
            return true;
        }

        public void SetCameraTriggerMode()
        {

            bool bval;
            NODE_HANDLE m_hNode = new NODE_HANDLE();
            m_hNode = m_imageProvider.GetNodeFromDevice("TriggerSelector");
            if (!m_hNode.IsValid)
                return;
            bval = GenApi.NodeIsWritable(m_hNode);
            if (!bval)
                return;
            GenApi.NodeFromString(m_hNode, "FrameStart");

            m_hNode = m_imageProvider.GetNodeFromDevice("TriggerMode");
            if (!m_hNode.IsValid)
                return;
            bval = GenApi.NodeIsWritable(m_hNode);
            if (!bval)
                return;
            GenApi.NodeFromString(m_hNode, "On");

            m_hNode = m_imageProvider.GetNodeFromDevice("TriggerSource");
            if (!m_hNode.IsValid)
                return;
            bval = GenApi.NodeIsWritable(m_hNode);
            if (!bval)
                return;
            GenApi.NodeFromString(m_hNode, "Software");
        }

        public void SetExposure(int value)
        {
            bool bval;
            //ExposureTimeRaw
            NODE_HANDLE m_hNode = new NODE_HANDLE();
            m_hNode = m_imageProvider.GetNodeFromDevice("ExposureTimeRaw");
            if (m_hNode.IsValid)
            {
                int inc = checked((int)GenApi.IntegerGetInc(m_hNode));
                int min = checked((int)GenApi.IntegerGetMin(m_hNode));
                int max = checked((int)GenApi.IntegerGetMax(m_hNode));
                int expVal = (value) - (value % inc);
                if (expVal < min)
                    expVal = min;
                if (expVal > max)
                    expVal = max;
                if (!m_hNode.IsValid)
                    return;
                bval = GenApi.NodeIsWritable(m_hNode);
                if (!bval)
                    return;
                GenApi.IntegerSetValue(m_hNode, expVal);
            }
        }
        //public long GetImageWidth()
        //{
        //    if (m_imageProvider.IsOpen)
        //    {
        //        return m_imageProvider.GetLatestImage().Width;
        //    }
        //    else
        //    {
 
        //    }
        //}
        //public long GetImageHeight()
        //{
        //    if (m_imageProvider.IsOpen)
        //    {
        //        return m_imageProvider.GetLatestImage().Height;
        //    }
        //    else
        //    {
 
        //    }
        //}
        
    }
}
