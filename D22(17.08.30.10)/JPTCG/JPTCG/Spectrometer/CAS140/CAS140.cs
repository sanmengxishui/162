using InstrumentSystems.CAS4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Spectrometer.CAS140
{
    class CAS140Mod
    {
        public int devHandle = -1;
        public string serial = "";

        //Parameters
        //public avaspec.MeasConfigType Config = new avaspec.MeasConfigType();
    }

    public class CAS140Spect
    {
        private static bool firstInstance = false;
        private Object lockCAS;
        private List<CAS140Mod> dev = new List<CAS140Mod>();
        private StringBuilder sb = new StringBuilder(256);
        private string ConfigPathFolder = "";

        public CAS140Spect()
        {
            if (firstInstance)
            {
                MessageBox.Show("Error. CAS140 Object already Created.");
                return;
            }
            firstInstance = true;
            lockCAS = new object();                                   

            String exePath = System.AppDomain.CurrentDomain.BaseDirectory;            
            ConfigPathFolder = exePath + "CAS140Config\\";
            if (!Directory.Exists(ConfigPathFolder))
            {
                Directory.CreateDirectory(ConfigPathFolder);
            }

            OpenDevices();
        }
        ~CAS140Spect()
        {
            for (int i=0; i< dev.Count;i++)
                CAS4DLL.casDoneDevice(dev[i].devHandle);            
        }
        
        private int OpenDevices()
        {
            dev.Clear();

            int iface = CAS4DLL.InterfaceUSB;
            int dvCnt = CAS4DLL.casGetDeviceTypeOptions(iface);
            int option;
            int res = -1;

            for (int i = 0; i < dvCnt; i++)
            {
                CAS140Mod dv = new CAS140Mod();
				
                option = CAS4DLL.casGetDeviceTypeOption(iface, i);
				CAS4DLL.casGetDeviceTypeOptionName(iface, i, sb, sb.Capacity);
                dv.serial = sb.ToString();

                dv.devHandle = CAS4DLL.casCreateDeviceEx(iface, option);

                if (dv.devHandle < 0)
				    continue;

                if (!File.Exists(ConfigPathFolder + dv.serial + ".ini"))
                {
                    MessageBox.Show("Error. CAS140 " + dv.serial + " config file Not Found. " + ConfigPathFolder + dv.serial + ".ini");
                    continue;
                }

                if (!File.Exists(ConfigPathFolder + dv.serial + ".isc"))
                {
                    MessageBox.Show("Error. CAS140 " + dv.serial + " calibrate file Not Found. " + ConfigPathFolder + dv.serial + ".isc");
                    continue;
                }

                //setup config, calib
                res = CAS4DLL.casSetDeviceParameterString(dv.devHandle, CAS4DLL.dpidConfigFileName, ConfigPathFolder + dv.serial + ".ini");//"8560142E1"
                if (res < 0)
                {
                    MessageBox.Show("Error. CAS140 " + dv.serial + " config file Not Found. " + ConfigPathFolder + dv.serial + ".ini");
                    continue;
                }
                res = CAS4DLL.casSetDeviceParameterString(dv.devHandle, CAS4DLL.dpidCalibFileName, ConfigPathFolder + dv.serial + ".isc");
                if (res < 0)
                {
                    MessageBox.Show("Error. CAS140 " + dv.serial + " calibrate file Not Found. " + ConfigPathFolder + dv.serial + ".isc");
                    continue;
                }
                res = CAS4DLL.casInitialize(dv.devHandle, CAS4DLL.InitForced);
                if (res < 0)
                    continue;  
  
                dev.Add(dv);	
			}
            return dev.Count();
        }
        public List<string> GetSpectrometerList()
        {
            List<string> myList = new List<string>();
            for (int i = 0; i < dev.Count; i++)
                myList.Add(dev[i].serial);
            return myList;
        }
        public int GetIndex(string serial)
        {
            for (int i = 0; i < dev.Count; i++)
                if (dev[i].serial == serial)
                    return i;
            return -1;
        }
        public int GetParamters(int Idx, ref int startPixel, ref int stopPixel, ref int IntegrateTime, ref int NOfAvg, ref int smoothingPixel)
        {
            lock (lockCAS)
            {
                IntegrateTime = (int)CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.mpidIntegrationTime);
                startPixel = (int)CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidDeadPixels);
                stopPixel = (int)CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidVisiblePixels);
                NOfAvg = (int)CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.mpidAverages);  
                smoothingPixel = (int)CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.mpidPulseWidth);
            }
            return 0;
        }
        public int SetParamters(int Idx, int startPixel, int stopPixel, int IntegrateTime, int NOfAvg, int smoothingPixel)
        {
            string path = "D:\\SaveISDFiles";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = "D:\\SaveISDFiles\\Module" + Idx.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FileName = path + "\\" + "ISDFILE" + ".isd";
            CAS4DLL.casSaveCalibration(dev[Idx].devHandle, FileName);
            if (dev.Count == 0)
                return 0;
                         
            lock (lockCAS)
            {
                CAS4DLL.casSetMeasurementParameter(dev[Idx].devHandle, CAS4DLL.mpidIntegrationTime, (double)IntegrateTime);
                CAS4DLL.casSetMeasurementParameter(dev[Idx].devHandle, CAS4DLL.mpidAverages, (double)NOfAvg);
                CAS4DLL.casSetMeasurementParameter(dev[Idx].devHandle, CAS4DLL.mpidPulseWidth, (double)smoothingPixel);                
            }
            return 0;
        }

        public bool SetIO(int Idx, byte value)
        {
            lock (lockCAS)
            {
                CAS4DLL.casSetShutter(dev[Idx].devHandle, value);
            }
            return true;
        }

        public List<float> GetWaveLength(int Idx)
        {
            List<float> myWL = new List<float>();
            
            lock (lockCAS)
            {
                int pixCnt = (int)Math.Round(CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidVisiblePixels));

                int Dpix = (int)Math.Round(CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidDeadPixels));

                for (int i = 0; i < pixCnt; i++)
                {                    
                    //get the wavelengths; don't forget about skipping dead pixels
                    myWL.Add((float)CAS4DLL.casGetXArray(dev[Idx].devHandle, i + Dpix));
                }
            }
            return myWL;
        }

        public List<float> GetCount(int Idx)
        {
            List<float> myCnt = new List<float>();

            //lock (lockCAS)
            {
                CAS4DLL.casMeasure(dev[Idx].devHandle);

                int pixCnt = (int)Math.Round(CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidVisiblePixels));
                int Dpix = (int)Math.Round(CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidDeadPixels));

                for (int i = 0; i < pixCnt; i++)
                {
                    //Spectrum
                    //float spectrum = (float)CAS4DLL.casGetCalibrationFactors(dev[Idx].devHandle, CAS4DLL.gcfRawData, i + Dpix, 0);
                    //myCnt.Add(CAS4DLL.casGetData(dev[Idx].devHandle, i + Dpix));
                    myCnt.Add((float)CAS4DLL.casGetCalibrationFactors(dev[Idx].devHandle, CAS4DLL.gcfRawData, i + Dpix, 0));
                    //get the wavelengths; don't forget about skipping dead pixels
                    //myWL.Add(CAS4DLL.casGetXArray(dev[Idx].devHandle, i + Dpix));
                }
            }
            return myCnt;
        }

        public List<float> MeasureDarkCurrent(int Idx, bool closeShutter, bool openShutter)//20170220
        {
            List<float> myWL = new List<float>();
            if (Idx < 0)
                return myWL;

            //lock (lockCAS)
            //{
            //    int pixCnt = (int)Math.Round(CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidVisiblePixels));

            //    int Dpix = (int)Math.Round(CAS4DLL.casGetDeviceParameter(dev[Idx].devHandle, CAS4DLL.dpidDeadPixels));

            //    for (int i = 0; i < pixCnt; i++)
            //    {
            //        //get the wavelengths; don't forget about skipping dead pixels
            //        myWL.Add((float)CAS4DLL.casGetXArray(dev[Idx].devHandle, i + Dpix));
            //    }
            //}
            lock (lockCAS)
            {
                if (closeShutter)
                    CAS4DLL.casSetShutter(dev[Idx].devHandle, 1);
                CAS4DLL.casMeasureDarkCurrent(dev[Idx].devHandle);
                if (openShutter)
                    CAS4DLL.casSetShutter(dev[Idx].devHandle, 0);
            }


            return myWL;
        }

        public void GetPeak(int Idx, out double x, out double y, double start, double stop)//20170220
        {
            CAS4DLL.casSetColormetricStart(dev[Idx].devHandle, start);
            CAS4DLL.casSetColormetricStop(dev[Idx].devHandle, stop);
            int a = CAS4DLL.casColorMetric(dev[Idx].devHandle);
            CAS4DLL.casGetPeak(dev[Idx].devHandle, out x, out y);
        }
    }
}
