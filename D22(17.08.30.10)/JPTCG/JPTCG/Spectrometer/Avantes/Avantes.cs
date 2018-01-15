using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AVS_SPACEC_Sharp;
using System.Runtime.InteropServices;
using System.Threading;

namespace JPTCG.Spectrometer.Avantes
{       
    class AvMod
    {
        public long devHandle =-1;
        public string serial = "";

        //Parameters
        public avaspec.MeasConfigType Config = new avaspec.MeasConfigType();                
    }
    public class AvantesSpect
    {
        private static bool firstInstance = false;
        private Object lockAvantes;
        private IntPtr a_hWnd;
        private List<AvMod> dev = new List<AvMod>();

        public AvantesSpect(IntPtr winHdl)
        {
            if (firstInstance)
                MessageBox.Show("Error. Avantes Object already Created.");
            firstInstance = true;
            lockAvantes = new object();
            OpenDevices();
            a_hWnd = winHdl;
        }
        ~AvantesSpect()
        {
            avaspec.AVS_Done();   
        }

        public int OpenDevices()
        {
            int dvCnt = -1;
            int l_Port = avaspec.AVS_Init(0);
            if (l_Port <= 0)
            {
                avaspec.AVS_Done();
                return -1;
            }

            dvCnt = avaspec.AVS_GetNrOfDevices();

            if (dvCnt > 0)
            {
                avaspec.AvsIdentityType[] l_Id = new avaspec.AvsIdentityType[dvCnt];
                uint l_RequiredSize = ((uint)dvCnt) * (uint)Marshal.SizeOf(typeof(avaspec.AvsIdentityType));
                for (int i = 0; i < dvCnt; i++)
                {
                    switch (l_Id[i].m_Status)
                    {                        
                        case avaspec.DEVICE_STATUS.USB_AVAILABLE:
                            AvMod dv = new AvMod();
                            dv.serial = l_Id[i].m_SerialNumber;
                            dv.devHandle = (long)avaspec.AVS_Activate(ref l_Id[i]);
                            avaspec.AVS_UseHighResAdc((IntPtr)dv.devHandle, true);
                            uint l_Size;
                            l_Size = (uint)Marshal.SizeOf(typeof(avaspec.DeviceConfigType));
                            avaspec.DeviceConfigType l_pDeviceData = new avaspec.DeviceConfigType();
                            int l_Res = (int)avaspec.AVS_GetParameter((IntPtr)dv.devHandle, l_Size, ref l_Size, ref l_pDeviceData);
                            if (avaspec.ERR_SUCCESS == l_Res)
                            {                                
                                dv.Config = l_pDeviceData.m_StandAlone.m_Meas;
                                dev.Add(dv);   
                            }                            
                            break;                        
                        default:                            
                            break;
                    }
                }
            }
            return dev.Count();
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
        public int GetParamters(int Idx, ref int startPixel,ref int stopPixel, ref int IntegrateTime, ref int NOfAvg, ref int smoothingPixel)
        {
            startPixel = dev[Idx].Config.m_StartPixel;
            stopPixel = dev[Idx].Config.m_StopPixel;
            IntegrateTime = (int)dev[Idx].Config.m_IntegrationTime;
            NOfAvg = (int)dev[Idx].Config.m_NrAverages;
            smoothingPixel = dev[Idx].Config.m_Smoothing.m_SmoothPix;

            return 0;
        }
        public int SetParamters(int Idx, int startPixel, int stopPixel, int IntegrateTime, int NOfAvg, int smoothingPixel)
        {
            dev[Idx].Config.m_StartPixel = (ushort)startPixel;
            dev[Idx].Config.m_StopPixel = (ushort)stopPixel;
            dev[Idx].Config.m_IntegrationTime = IntegrateTime;
            dev[Idx].Config.m_NrAverages = (uint)NOfAvg;
            dev[Idx].Config.m_Smoothing.m_SmoothPix = (ushort)smoothingPixel;
            SetMeasurePara(Idx);
            return 0;
        }

        public bool SetIO(int Idx, byte value)  
        {
            int l_Res = avaspec.AVS_SetDigOut((IntPtr)dev[Idx].devHandle, 3, value);//port=3,a_Value=1则打开光源IO，a_Value=0则关闭光源;
            if (l_Res != avaspec.ERR_SUCCESS)
            {                
                return false;
            }
            else
            {               
                return true;
            }
        }
        public bool SetMeasurePara(int Idx)
        {
            lock (lockAvantes)
            {
                int l_Res = (int)avaspec.AVS_PrepareMeasure((IntPtr)dev[Idx].devHandle, ref dev[Idx].Config);
                if (avaspec.ERR_SUCCESS == l_Res)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<float> GetWaveLength(int Idx)
        {
            List<float> myWL = new List<float>();
            avaspec.PixelArrayType m_Lambda = new avaspec.PixelArrayType();
            if (avaspec.ERR_SUCCESS == (int)avaspec.AVS_GetLambda((IntPtr)dev[Idx].devHandle, ref m_Lambda))
            {
                for (int i = 0; i < m_Lambda.Value.Length; i++)
                    myWL.Add((float)m_Lambda.Value[i]);
                return myWL;
            }
            else
                return null;// fail
        }
        public List<float> GetCount(int Idx)
        {
            StartMeasure(Idx);

            avaspec.PixelArrayType l_pSpectrum = new avaspec.PixelArrayType();
            Getdata(Idx, ref l_pSpectrum);

            List<float> myCnt = new List<float>();

            for (int i = 0; i < l_pSpectrum.Value.Length; i++)
                myCnt.Add((float)l_pSpectrum.Value[i]);

            return myCnt;            
        }

        private string StartMeasure(int Idx)
        {
            //设采样次数为1
            //SetMeasurePara(Idx);
            lock (lockAvantes)
            {
                int l_Res = (int)avaspec.AVS_Measure((IntPtr)dev[Idx].devHandle, a_hWnd, 1);
                if (avaspec.ERR_SUCCESS != l_Res)
                {
                    switch (l_Res)
                    {
                        case avaspec.ERR_INVALID_PARAMETER:
                            return "Meas.Status: invalid parameter";
                        default:
                            return "Meas.Status: start failed, code: ";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        private bool Getdata(int Idx, ref avaspec.PixelArrayType l_pSpectrum)
        {
            uint l_Time = 0;
            int res = -1;
            DateTime st_time = DateTime.Now;
            TimeSpan time_span = DateTime.Now - st_time;
            double timeOutMillSec = dev[Idx].Config.m_IntegrationTime * dev[Idx].Config.m_NrAverages + 100;
            do
            {
                Thread.Sleep(100);
                res = (int)avaspec.AVS_PollScan((IntPtr)dev[Idx].devHandle);
                time_span = DateTime.Now - st_time;
                if (time_span.TotalMilliseconds > timeOutMillSec)
                {
                    return false;
                }
            } while (res <= 0);

            lock (lockAvantes)
            {
                avaspec.AVS_GetScopeData((IntPtr)dev[Idx].devHandle, ref l_Time, ref l_pSpectrum);
            }
            return true;
        }
    }
}
