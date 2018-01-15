using Common;
using JPTCG.Common;
using JPTCG.Motion;
using JPTCG.Spectrometer.Avantes;
using JPTCG.Spectrometer.CAS140;
using JPTCG.Spectrometer.Maya;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Spectrometer
{
    public class SpectManager
    {                
        private static bool firstInstance = false;
        public  List<SpectModule> SpecList = new List<SpectModule>();
        private IntPtr avWinHdl;
        private AvantesSpect AvantesObj;
        private MayaSpectrometer MayaObj;
        private CAS140Spect CASObj;
        public string LoadFileName = "";

        public SpectManager(IntPtr WinHdl)
        {
            if (firstInstance)
            {
                MessageBox.Show("Error. Spectrometer Manager already Created.");
                return;
            }
            firstInstance = true;
            avWinHdl = WinHdl;
           
        }
        ~SpectManager()
        {
            
        }

        public int AddSpectrometer(string Name)
        {
            SpectModule newSpec = new SpectModule();
            newSpec.specType = SpectType.NoSpectrometer;
            newSpec.Name = Name;
            SpecList.Add(newSpec);
            return SpecList.Count-1;
        }
        private void InitSpectrometer()
        {
            bool isAvantes = false;
            bool isMaya = false;
            bool isCAS = false;

            if (SpecList.Count == 0)
                return;

            for (int i = 0; i < SpecList.Count; i++)            
            {
                switch (SpecList[i].specType)
                {
                    case SpectType.Avantes:
                        isAvantes = true;
                        break;
                   case SpectType.Maya:
                        isMaya = true;
                        break;
                   case SpectType.CAS140:
                        isCAS = true;
                        break;
                }                
            }

            if (isAvantes)
            {
                if (AvantesObj == null)
                    AvantesObj = new AvantesSpect(avWinHdl);
            }

            if (isMaya)
            {
                if (MayaObj == null)
                    MayaObj = new MayaSpectrometer();
            }

            if (isCAS)
            {
                if (CASObj == null)
                    CASObj = new CAS140Spect();
            }
        }

        public void SaveMachineSettings(string fileName)
        {
            string Header = "";
            for (int i = 0; i < SpecList.Count; i++)
            {
                Header = "Spectrometer" + (i + 1).ToString();
                FileOperation.SaveData(fileName, Header, "Type", SpecList[i].specType.ToString());
                FileOperation.SaveData(fileName, Header, "Serial", SpecList[i].serial);                
            }

            SaveSettings(fileName);
        }
        private void AssignSpecIndex()
        {
            for (int i = 0; i < SpecList.Count; i++)
            {
                switch (SpecList[i].specType)
                {
                    case SpectType.Avantes:
                        SpecList[i].Idx = AvantesObj.GetIndex(SpecList[i].serial);
                        break;
                    case SpectType.Maya:
                        SpecList[i].Idx = MayaObj.GetIndex(SpecList[i].serial);
                        break;
                    case SpectType.CAS140:
                        SpecList[i].Idx = CASObj.GetIndex(SpecList[i].serial);
                        break;
                }
            }
        }
        public void LoadMachineSettings(string fileName)
        {
            string Header = "";
            string strread = "";
            for (int i = 0; i < SpecList.Count; i++)
            {
                Header = "Spectrometer" + (i + 1).ToString();
                FileOperation.ReadData(fileName, Header, "Type", ref strread);
                if (strread != "0")
                {
                    SpectType spt = SpectType.NoSpectrometer;
                    Enum.TryParse(strread, out spt);

                    SpecList[i].specType = spt;//(SpectType)int.Parse(strread);
                }
                FileOperation.ReadData(fileName, Header, "Serial", ref strread);
                if (strread != "0")
                    SpecList[i].serial = strread;
            }

            InitSpectrometer();
            AssignSpecIndex();
            LoadFileName = fileName;
            LoadSettings(fileName);
        }
        public void SaveSettings(string fileName)
        {
            string Header = "";
            for (int i = 0; i < SpecList.Count; i++)
            {
                if (SpecList[i].specType == SpectType.NoSpectrometer)
                    continue;
                Header = "Spectrometer" + (i + 1).ToString();
                FileOperation.SaveData(fileName, Header, "Idx", SpecList[i].Idx.ToString());
                FileOperation.SaveData(fileName, Header, "StartPixel", SpecList[i].StartPixel.ToString());
                FileOperation.SaveData(fileName, Header, "EndPixel", SpecList[i].EndPixel.ToString());
                FileOperation.SaveData(fileName, Header, "IntegrationTime", SpecList[i].IntegrationTime.ToString());
                FileOperation.SaveData(fileName, Header, "NumOfAvg", SpecList[i].NumOfAvg.ToString());
                FileOperation.SaveData(fileName, Header, "SmoothingPixel", SpecList[i].SmoothingPixel.ToString());
                FileOperation.SaveData(fileName, Header, "UsedTime", SpecList[i].UsedTime.ToString());
                //SaveTestCriteria(fileName, i);
            }
        }
        public void LoadSettings(string fileName)
        {
            string Header = "";
            string strread = "";
            for (int i = 0; i < SpecList.Count; i++)
            {
                if (SpecList[i].specType == SpectType.NoSpectrometer)
                    continue;
                Header = "Spectrometer" + (i + 1).ToString();
                //FileOperation.ReadData(fileName, Header, "Idx", ref strread);
                //SpecList[i].Idx = int.Parse(strread);

                FileOperation.ReadData(fileName, Header, "StartPixel", ref strread);
                SpecList[i].StartPixel = int.Parse(strread);

                FileOperation.ReadData(fileName, Header, "EndPixel", ref strread);
                SpecList[i].EndPixel = int.Parse(strread);

                FileOperation.ReadData(fileName, Header, "IntegrationTime", ref strread);
                SpecList[i].IntegrationTime = int.Parse(strread);

                FileOperation.ReadData(fileName, Header, "NumOfAvg", ref strread);
                SpecList[i].NumOfAvg = int.Parse(strread);

                FileOperation.ReadData(fileName, Header, "SmoothingPixel", ref strread);
                SpecList[i].SmoothingPixel = int.Parse(strread);

                FileOperation.ReadData(fileName, Header, "UsedTime", ref strread);
                try
                {
                    SpecList[i].UsedTime = TimeSpan.Parse(strread);
                }
                catch { };

                if (Para.MachineOnline)
                    SetParamters(i, SpecList[i].StartPixel, SpecList[i].EndPixel, SpecList[i].IntegrationTime, SpecList[i].NumOfAvg, SpecList[i].SmoothingPixel);
                //LoadTestCriteria(fileName, i);
            }
        }
        public void SaveTestCriteria(string fileName)
        {
            for (int SpecIdx = 0; SpecIdx < SpecList.Count; SpecIdx++)
            {
                string Header = "Spectrometer" + (SpecIdx + 1).ToString();

                FileOperation.SaveData(fileName, Header, "CriteriaCount", SpecList[SpecIdx].Criteria.Count.ToString());
                for (int i = 0; i < SpecList[SpecIdx].Criteria.Count; i++)
                {
                    FileOperation.SaveData(fileName, Header, "CriteriaWL" + (i + 1).ToString(), SpecList[SpecIdx].Criteria[i].WaveLength.ToString());
                    FileOperation.SaveData(fileName, Header, "CriteriaMin" + (i + 1).ToString(), SpecList[SpecIdx].Criteria[i].Min.ToString("F2"));
                    FileOperation.SaveData(fileName, Header, "CriteriaMax" + (i + 1).ToString(), SpecList[SpecIdx].Criteria[i].Max.ToString("F2"));
                }
            }
        }
        public void LoadTestCriteria(string fileName)
        {
            for (int SpecIdx = 0; SpecIdx < SpecList.Count; SpecIdx++)
            {
                string Header = "Spectrometer" + (SpecIdx + 1).ToString();
                string strread = "";

                FileOperation.ReadData(fileName, Header, "CriteriaCount", ref strread);
                int cCnt = int.Parse(strread);
                SpecList[SpecIdx].Criteria.Clear();

                for (int i = 0; i < cCnt; i++)
                {
                    TestCriteria cri = new TestCriteria();
                    FileOperation.ReadData(fileName, Header, "CriteriaWL" + (i + 1).ToString(), ref strread);
                    cri.WaveLength = int.Parse(strread);
                    FileOperation.ReadData(fileName, Header, "CriteriaMin" + (i + 1).ToString(), ref strread);
                    cri.Min = double.Parse(strread);
                    FileOperation.ReadData(fileName, Header, "CriteriaMax" + (i + 1).ToString(), ref strread);
                    cri.Max = double.Parse(strread);
                    SpecList[SpecIdx].Criteria.Add(cri);
                }
            }
        }

        public List<string> GetSpectrometerList(SpectType myType)
        {
            List<string> myList = new List<string>();

            switch (myType)
            {
                case SpectType.NoSpectrometer:                    
                    break;
                case SpectType.Avantes:
                    if (AvantesObj == null)
                        AvantesObj = new AvantesSpect(avWinHdl);
                    myList = AvantesObj.GetSpectrometerList();
                    break;
                case SpectType.Maya:
                    if (MayaObj == null)
                        MayaObj = new MayaSpectrometer();
                    myList = MayaObj.GetSpectrometerList();
                    break;
                case SpectType.CAS140:
                    if (CASObj == null)
                        CASObj = new CAS140Spect();
                    myList = CASObj.GetSpectrometerList();
                    break;
            }
            return myList;
        }
        public void GetParamtersFromDevice(int idx)
        {
            switch (SpecList[idx].specType)
            {
                case SpectType.Avantes:
                    AvantesObj.GetParamters(SpecList[idx].Idx, ref SpecList[idx].StartPixel, ref SpecList[idx].EndPixel,
                                            ref SpecList[idx].IntegrationTime, ref SpecList[idx].NumOfAvg, ref SpecList[idx].SmoothingPixel);
                    break;
                case SpectType.Maya:
                    MayaObj.GetParamters(SpecList[idx].Idx, ref SpecList[idx].StartPixel, ref SpecList[idx].EndPixel,
                                            ref SpecList[idx].IntegrationTime, ref SpecList[idx].NumOfAvg, ref SpecList[idx].SmoothingPixel);
                    break;
                case SpectType.CAS140:
                    CASObj.GetParamters(SpecList[idx].Idx, ref SpecList[idx].StartPixel, ref SpecList[idx].EndPixel,
                                            ref SpecList[idx].IntegrationTime, ref SpecList[idx].NumOfAvg, ref SpecList[idx].SmoothingPixel);
                    break;
            }
        }
        public void SetParamters(int idx, int myStartPixel, int myEndPixel, int myIntegrateTime, int myNumOfAvg, int mySmoothing)
        {
            if (SpecList[idx].specType == SpectType.NoSpectrometer)
                return;

            SpecList[idx].StartPixel = myStartPixel;
            SpecList[idx].EndPixel = myEndPixel;
            SpecList[idx].IntegrationTime = myIntegrateTime;
            SpecList[idx].NumOfAvg = myNumOfAvg;
            SpecList[idx].SmoothingPixel = mySmoothing;
            switch (SpecList[idx].specType)
            {
                case SpectType.Avantes:
                    AvantesObj.SetParamters(SpecList[idx].Idx, SpecList[idx].StartPixel, SpecList[idx].EndPixel,
                                            SpecList[idx].IntegrationTime, SpecList[idx].NumOfAvg, SpecList[idx].SmoothingPixel);
                    break;
                case SpectType.Maya:
                    MayaObj.SetParamters(SpecList[idx].Idx, SpecList[idx].StartPixel, SpecList[idx].EndPixel,
                                            SpecList[idx].IntegrationTime, SpecList[idx].NumOfAvg, SpecList[idx].SmoothingPixel);
                    break;
                case SpectType.CAS140:
                    CASObj.SetParamters(SpecList[idx].Idx, SpecList[idx].StartPixel, SpecList[idx].EndPixel,
                                            SpecList[idx].IntegrationTime, SpecList[idx].NumOfAvg, SpecList[idx].SmoothingPixel);
                    break;
            }
        }

        public void SetAverage(int idx, int myNumOfAvg)
        {
            if (SpecList[idx].specType == SpectType.NoSpectrometer)
                return;

            SpecList[idx].NumOfAvg = myNumOfAvg;
            switch (SpecList[idx].specType)
            {
                case SpectType.Avantes:
                    AvantesObj.SetParamters(SpecList[idx].Idx, SpecList[idx].StartPixel, SpecList[idx].EndPixel,
                                            SpecList[idx].IntegrationTime, SpecList[idx].NumOfAvg, SpecList[idx].SmoothingPixel);
                    break;
                case SpectType.Maya:
                    MayaObj.SetParamters(SpecList[idx].Idx, SpecList[idx].StartPixel, SpecList[idx].EndPixel,
                                            SpecList[idx].IntegrationTime, SpecList[idx].NumOfAvg, SpecList[idx].SmoothingPixel);
                    break;
                case SpectType.CAS140:
                    CASObj.SetParamters(SpecList[idx].Idx, SpecList[idx].StartPixel, SpecList[idx].EndPixel,
                                            SpecList[idx].IntegrationTime, SpecList[idx].NumOfAvg, SpecList[idx].SmoothingPixel);
                    break;
            }
        }

        public void SetIO(int idx, bool OnOff)
        {            
            if (SpecList[idx].specType == SpectType.NoSpectrometer)
                return;
            
            switch (SpecList[idx].specType)
            {
                case SpectType.Avantes:
                    AvantesObj.SetIO(SpecList[idx].Idx, Convert.ToByte(OnOff));
                    break;
                case SpectType.Maya:
                    MayaObj.SetIO(SpecList[idx].Idx, Convert.ToByte(OnOff));
                    break;
                case SpectType.CAS140:
                    CASObj.SetIO(SpecList[idx].Idx, Convert.ToByte(OnOff));
                    break;
            }
            if (!Para.isOutShutter)
            {
                if (OnOff)
                {
                    SpecList[idx].starttime = DateTime.Now;
                    SpecList[idx].UsedTime = DateTime.Now - SpecList[idx].starttime;
                }
                else
                {
                    SpecList[idx].UsedTime = SpecList[idx].UsedTime + (DateTime.Now - SpecList[idx].starttime);
                }
            }
        }
        public List<float> GetWaveLength(int idx)
        {
            if (SpecList[idx].specType == SpectType.NoSpectrometer)
                return null;

            switch (SpecList[idx].specType)
            {
                case SpectType.Avantes:
                    return AvantesObj.GetWaveLength(SpecList[idx].Idx);                    
                case SpectType.Maya:
                    return MayaObj.GetWaveLength(SpecList[idx].Idx);
                case SpectType.CAS140:
                    return CASObj.GetWaveLength(SpecList[idx].Idx);
            }
            return null;
        }

        public List<float> GetCount(int idx, bool useSpecPara = false)
        {
            if (SpecList[idx].specType == SpectType.NoSpectrometer)
                return null;

            switch (SpecList[idx].specType)
            {
                case SpectType.Avantes:
                    return AvantesObj.GetCount(SpecList[idx].Idx);
                case SpectType.Maya:
                    if (useSpecPara)
                        return MayaObj.GetCount(SpecList[idx].Idx, 0, 0, 0);
                    else
                        return MayaObj.GetCount(SpecList[idx].Idx, SpecList[idx].IntegrationTime, SpecList[idx].SmoothingPixel, SpecList[idx].NumOfAvg);
                case SpectType.CAS140:
                    return CASObj.GetCount(SpecList[idx].Idx);
            }
            return null;
        }

        public SpectType GetType(int idx)
        {
            return SpecList[idx].specType; 
        }

        //public List<double> GetDarkRef(int idx)
        //{
        //    if (SpecList[idx].specType == SpectType.NoSpectrometer)
        //        return null;

        //    switch (SpecList[idx].specType)
        //    {
        //        case SpectType.Avantes:
        //            return AvantesObj.GetWaveLength(SpecList[idx].Idx);
        //        case SpectType.Maya:
        //            return MayaObj.GetWaveLength(SpecList[idx].Idx);
        //        case SpectType.CAS140:
        //            return CASObj.GetWaveLength(SpecList[idx].Idx);
        //    }
        //    return null;
        //}
        public void ShowSettings(DeltaMotionMgr myMotion)
        {
            WinSpectrometer myWin = new WinSpectrometer(this, myMotion);
            myWin.Show();
        }

        public List<float> MeasureDarkCurrent(int idx, bool closeShutter, bool openShutter)//20170220
        {
            if (SpecList[idx].specType == SpectType.NoSpectrometer)
                return null;

            switch (SpecList[idx].specType)
            {
                //case SpectType.Avantes:
                //    return AvantesObj.GetWaveLength(SpecList[idx].Idx);
                //case SpectType.Maya:
                //    return MayaObj.GetWaveLength(SpecList[idx].Idx);
                case SpectType.CAS140:
                    return CASObj.MeasureDarkCurrent(SpecList[idx].Idx, closeShutter, openShutter);
            }
            return null;
        }

        public void GetPeak(int idx, out double x, out double y, double start, double stop)//20170220
        {
            x = 0;
            y = 0;
            switch (SpecList[idx].specType)
            {
                case SpectType.CAS140:
                    CASObj.GetPeak(SpecList[idx].Idx, out x, out y, start, stop);
                    break;
            }
        }
    }
}
