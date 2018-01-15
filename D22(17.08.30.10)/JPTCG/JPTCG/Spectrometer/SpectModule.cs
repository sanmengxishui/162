using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPTCG.Spectrometer
{
    public class TestCriteria
    {
        public int WaveLength = 0;
        public double Min = 0.0;
        public double Max = 0.0;
    }

    public class SpectModule
    {
        public SpectType specType = SpectType.NoSpectrometer;
        public String serial = "";
        public string Name = "";
        public int Idx = -1;  
        public TimeSpan UsedTime = new TimeSpan(0);

        public int StartPixel = -1;
        public int EndPixel = -1;
        public int IntegrationTime = -1;
        public int NumOfAvg = 0;
        public int SmoothingPixel = -1;

        public DateTime starttime;

        public List<TestCriteria> Criteria = new List<TestCriteria>();
    }


}
