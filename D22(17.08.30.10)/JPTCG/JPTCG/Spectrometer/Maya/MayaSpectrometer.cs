using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JPTCG.Spectrometer.Maya
{
    class MayaMod
    {
        public int devHandle = -1;
        public string serial = "";

        //Parameters
        //public avaspec.MeasConfigType Config = new avaspec.MeasConfigType();
    }

    public class MayaSpectrometer
    {
        public NewMayaLib SpecLib;

        private static bool firstInstance = false;
        private Object lockMaya;
        private List<MayaMod> dev = new List<MayaMod>();

        public MayaSpectrometer()
        {
            if (firstInstance)
            {
                MessageBox.Show("Error. Maya Object already Created.");
                return;
            }
            firstInstance = true;
            lockMaya = new object();
            SpecLib = new NewMayaLib();// SpecComLib.bqSpecLib();
            //InitMaya(); 
            
            if (InitMaya() != 0)
                MessageBox.Show("Error. Maya Library Loaded Failed.");

            OpenDevices();
        }
        ~MayaSpectrometer()
        {
            try
            {
                if (SpecLib != null)
                    SpecLib.Disconnect();
            }
            catch (Exception ex)
            { }
        }

        private int InitMaya()
        {
            int res=SpecLib.ConnectToLib();
            return res;
        }
        private int OpenDevices()
        {
            dev.Clear();
            int dvCnt = SpecLib.GetSpecCount();
            for (int i = 0; i < dvCnt; i++)
            {
                MayaMod dv = new MayaMod();
                dv.devHandle = i;
                dv.serial = GetSerialNumber(i);
                dev.Add(dv);
            }
            return dev.Count();
        }
        private String GetSerialNumber(int myIdx)
        {
            string S1 = "";
            lock (lockMaya)
            {
                S1 = SpecLib.GetSerialNo(myIdx);// SpecLib.GetSerialNo(myIdx, ref S1);
            }
            return S1;
        }
        public int GetIndex(string serial)
        {
            for (int i = 0; i < dev.Count; i++)
                if (dev[i].serial == serial)
                    return i;
            return -1;
        }
        public List<string> GetSpectrometerList()
        {
            List<string> myList = new List<string>();
            for (int i = 0; i < dev.Count; i++)
                myList.Add(dev[i].serial);
            return myList;
        }

        public int GetParamters(int Idx, ref int startPixel, ref int stopPixel, ref int IntegrateTime, ref int NOfAvg, ref int smoothingPixel)
        {
            lock (lockMaya)
            {
                SpecLib.SetSpecIndex(dev[Idx].devHandle);
                startPixel = 0;
                stopPixel = SpecLib.GetPixelCount(dev[Idx].devHandle);
                IntegrateTime = SpecLib.GetIntTime();// SpecLib.IntTime;
                NOfAvg = SpecLib.GetSTA();
                smoothingPixel = SpecLib.GetBCW();
            }
            return 0;
        }
        public int SetParamters(int Idx, int startPixel, int stopPixel, int IntegrateTime, int NOfAvg, int smoothingPixel)
        {
            //dev[Idx].Config.m_StartPixel = (ushort)startPixel;
            //dev[Idx].Config.m_StopPixel = (ushort)stopPixel;
            SpecLib.SetSpecIndex(Idx);//set the index otherwise the parameters may set to the wrong spectrometer
            SpecLib.SetIntTime(IntegrateTime);
            SpecLib.SetSTA(NOfAvg);
            SpecLib.SetBCW(smoothingPixel);            
            return 0;
        }
        
        public bool SetIO(int Idx, byte value)
        {
            int temp=0;
            if(value==1)
                temp=1;
            //lock (lockMaya)
            {
                SpecLib.SetShutterOpen(dev[Idx].devHandle, temp, 3);
                //SpecLib.SetSpecIndex(dev[Idx].devHandle);
                //SpecLib.SetStrobe1(value, 10, 5);
                //Thread.Sleep(SpecLib.STA * SpecLib.IntTime);
            }
            return true;
        }
        //20170622@ZJinP
        public void GetPeakWaveLength(int Idx,  ref double x, ref double y, double start, double stop)
        {
            SpecLib.GetPeak(Idx, start, stop, ref x, ref y);
        }

        //public void DoDelay(int Idx, int gain)
        //{
        //    SpecLib.DoDelay
        //}
        //20170622@ZJinP
        public List<float> GetWaveLength(int Idx)
        {
            List<float> myWL = new List<float>();
            double tmp;
            lock (lockMaya)
            {
                try
                {
                    int iCount = SpecLib.GetPixelCount(dev[Idx].devHandle);
                    double[] wavelengthArray = new double[iCount];// Array.CreateInstance(typeof(double), iCount);

                    SpecLib.SetSpecIndex(dev[Idx].devHandle);
                    SpecLib.GetWaveLength(Idx,ref wavelengthArray);

                    for (int i = 0; i < iCount; i++)
                    {
                        tmp = (double)wavelengthArray.GetValue(i);
                        myWL.Add((float)tmp);
                    }
                }
                catch (Exception ex)
                {
                   
                }
            }
            return myWL;             
        }

        public List<float> GetCount(int Idx,int IntTime, int bcw, int sta)
        {
            List<float> myCnt = new List<float>();
            double tmp;
            //lock (lockMaya)
            {
                try
                {
                    int iCount = SpecLib.GetPixelCount(dev[Idx].devHandle);
                    double[] spectrumArray = new double[iCount];// Array.CreateInstance(typeof(double), iCount);
                    //the follow
                    //SpecLib.SetSpecIndex(dev[Idx].devHandle);
                    //SpecLib.DoScan(); //测量**Scan for spectrum
                    //SpecLib.GetSpectrum(ref spectrumArray); //读取光谱数据**read the spectrum data
                    SpecLib.ScanAsyn(dev[Idx].devHandle, IntTime, bcw, sta);//index, inttime, bcw, sta
                    SpecLib.GetSpectrumAsyn(dev[Idx].devHandle, ref spectrumArray);//20161122
                    for (int i = 0; i < iCount; i++)  // Wei Se me yao -200????
                    {
                        tmp = (double)spectrumArray.GetValue(i);
                        myCnt.Add((float)tmp);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            return myCnt;
        }

        

        //public int PixelCount()
        //{
        //    return iPixelCount1;
        //}

        //设置光谱仪的IO函数
        private DateTime starttime = DateTime.Now;
        public void SetMayaIO(Byte value)
        {
            if (value == 1)
            {
                starttime = DateTime.Now;
            }
            else
            {
                //FSParamer.SpectrumLightUseTime = FSParamer.SpectrumLightUseTime + (DateTime.Now - starttime);
                //FileOperation.SaveDataToXMLRP(FSParamer.machineSettingsFilePath, "SpectrumLightUseTime", "Statistics", FSParamer.SpectrumLightUseTime.ToString());
            }
            SpecLib.SetStrobe1(value, 10, 5);
            Thread.Sleep(SpecLib.GetSTA() * SpecLib.GetIntTime());
        }
    }
}
