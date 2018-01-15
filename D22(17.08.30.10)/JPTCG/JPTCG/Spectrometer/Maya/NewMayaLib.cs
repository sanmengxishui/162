using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace JPTCG.Spectrometer.Maya
{
    public class NewMayaLib
    {
        [DllImport("NewSpecLib")]
        private static extern int BQ_Connect();

        [DllImport("NewSpecLib")]
        private static extern void BQ_Disconnect();

        [DllImport("NewSpecLib")]
        private static extern void BQ_Test(int index, int intTime, int sta, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)]ref double[] spec);

        [DllImport("NewSpecLib")]
        private static extern int BQ_GetPixelCount(int index);

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetSerialNo(int index, int iByte, ref Byte sByte);

        [DllImport("NewSpecLib")]
        private static extern int BQ_GetSpecCount();

        [DllImport("NewSpecLib")]
        private static extern void BQ_DoScan();

        [DllImport("NewSpecLib")]
        private static extern int BQ_Get_BCW();

        [DllImport("NewSpecLib")]
        private static extern int BQ_Get_IntTime();

        [DllImport("NewSpecLib")]
        private static extern int BQ_Get_STA();

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetSpectrum([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)]ref double[] spec);

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetSpectrumAsyn(int index, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)]ref double[] spec);

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetWaveLength(int index, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)]ref double[] wl);

        [DllImport("NewSpecLib")]
        private static extern void BQ_ScanAsyn(int index, int intTime, int bcw, int sta);

        [DllImport("NewSpecLib")]
        private static extern void BQ_Set_BCW(int Value);

        [DllImport("NewSpecLib")]
        private static extern void BQ_Set_IntTime(int Value);

        [DllImport("NewSpecLib")]
        private static extern void BQ_Set_STA(int Value);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetDarkCorrection(int Enabled);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetSpecIndex(int index);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetStrobe(int Enabled);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetStrobe1(int Enabled, int intTime, int sta);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetTriggerMode(int Mode);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetNonlinearityCorrection(int Enabled);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetFan(int Enabled);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetTEC(int Enabled);

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetBoardTemp(ref double Temp);

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetDetectorTemp(ref double Temp);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetDark(int index);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetRef(int index);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetStrayLightCorrection(int Enabled);

        [DllImport("NewSpecLib")]
        private static extern void BQ_SetShutterOpen(int index, int Openned, int delayGain);

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetAvg(int index, double minWl, double maxWl, ref double avg);

        [DllImport("NewSpecLib")]
        private static extern ulong BQ_GetTick();

        [DllImport("NewSpecLib")]
        private static extern void BQ_GetPeak(int Index, double wl_min, double wl_max, ref double x, ref double y);

        public int ConnectToLib()
        {
            try
            {
                return BQ_Connect();
            }
            catch
            {
                return -1;
            }
        }
        public void Disconnect()
        {
            BQ_Disconnect();
        }
        public int GetPixelCount(int index)
        {
            return BQ_GetPixelCount(index);
        }
        public string GetSerialNo(int index)
        {
            try
            {
                string s = "";
                byte B = 0;
                int i = 1;
                do
                {
                    BQ_GetSerialNo(index, i, ref B);
                    if (B != 0)
                        s += System.Convert.ToChar(B);
                    i++;
                }
                while (B != 0);
                return s;
            }
            catch
            {
                return "";
            }
        }
        public int GetSpecCount()
        {
            return BQ_GetSpecCount();
        }
        public void DoScan()
        {
            BQ_DoScan();
        }
        public int GetBCW()
        {
            return BQ_Get_BCW();
        }
        public int GetIntTime()
        {
            return BQ_Get_IntTime();
        }
        public int GetSTA()
        {
            return BQ_Get_STA();
        }
        public void GetSpectrum(ref double[] spec)
        {
            BQ_GetSpectrum(ref spec);
        }
        public void GetSpectrumAsyn(int index, ref double[] spec)
        {
            BQ_GetSpectrumAsyn(index, ref spec);
        }
        public void GetWaveLength(int index, ref double[] wl)
        {
            BQ_GetWaveLength(index, ref wl);
        }
        public void ScanAsyn(int index, int intTime, int bcw, int sta)
        {
            BQ_ScanAsyn(index, intTime, bcw, sta);
        }
        public void SetBCW(int Value)
        {
            BQ_Set_BCW(Value);
        }
        public void SetIntTime(int Value)
        {
            BQ_Set_IntTime(Value);
        }
        public void SetSTA(int Value)
        {
            BQ_Set_STA(Value);
        }

        public void SetDarkCorrection(int Enabled)
        {
            BQ_SetDarkCorrection(Enabled);
        }
        public void SetSpecIndex(int index)
        {
            BQ_SetSpecIndex(index);
        }
        public void SetStrobe(int Enabled)
        {
            BQ_SetStrobe(Enabled);
        }
        public void SetStrobe1(int Enabled, int intTime, int sta)
        {
            BQ_SetStrobe1(Enabled, intTime, sta);
        }
        public void SetTriggerMode(int Mode)
        {
            BQ_SetTriggerMode(Mode);
        }
        public void SetNonlinearityCorrection(int Enabled)
        {
            BQ_SetNonlinearityCorrection(Enabled);
        }
        public void SetFan(int Enabled)
        {
            BQ_SetFan(Enabled);
        }
        public void SetTEC(int Enabled)
        {
            BQ_SetTEC(Enabled);
        }
        public void GetBoardTemp(ref double Temp)
        {
            BQ_GetBoardTemp(ref Temp);
        }
        public void GetDetectorTemp(ref double Temp)
        {
            BQ_GetDetectorTemp(ref Temp);
        }
        public void SetDark(int index)
        {
            BQ_SetDark(index);
        }
        public void SetRef(int index)
        {
            BQ_SetRef(index);
        }
        public void SetStrayLightCorrection(int Enabled)
        {
            BQ_SetStrayLightCorrection(Enabled);
        }
        public void SetShutterOpen(int index, int Openned, int delayGain)
        {
            BQ_SetShutterOpen(index, Openned, delayGain);
        }
        public void GetAvg(int index, double minWl, double maxWl, ref double avg)
        {
            BQ_GetAvg(index, minWl, maxWl, ref avg);
        }
        public void Test(int index, int intTime, int sta, ref double[] spec)
        {
            BQ_Test(index, intTime, sta, ref spec);
        }
        public ulong GetTick()
        {
            return BQ_GetTick();
        }
        public void GetPeak(int index, double wlMin, double wlMax, ref double x, ref double y)
        {
            BQ_GetPeak(index, wlMin, wlMax, ref x, ref y);
        }
    }
}
