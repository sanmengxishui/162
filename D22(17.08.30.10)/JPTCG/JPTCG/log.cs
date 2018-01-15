using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPTCG
{
    class log
    {
        //StreamWriter sw = new StreamWriter(Para.MchConfigFileName + "ErrorLog.txt");
         public double macminiCost1=0;
         public double jptCost1;
         public double totalCost1;
         public double macminiCost2;
         public double jptCost2;
         public double totalCost2;
        public  void calculateTime(DateTime a,DateTime b,string testProcess)
        {                     
            //TimeSpan ts = a - b;
            //double i = ts.TotalMilliseconds;
            //string logLine = testProcess + "  cost: " + i + "ms";
            //sw.WriteLine(logLine);
            //if (testProcess == "finu1"  || testProcess == "rcst1"  || testProcess == "tsta1"  || testProcess == "dest1" )
            //{
            //    macminiCost1 += i; 
            //}
            //else if (testProcess == "finu2" || testProcess == "rcst2" || testProcess == "tsta2" || testProcess == "dest2")
            //{
            //    macminiCost2 += i;
            //}
            //else if (testProcess == "barCode" || testProcess == "Rotatry" || testProcess == "Cam1CCDCapture" || testProcess == "InspectMod1Unit" || testProcess == "CaptureCCDExposure1" ||  testProcess == "Dark Test1" || testProcess == "White Test1" || testProcess == "Spectrograph1" || testProcess == "SendImageAdvance1" || testProcess == "SendAdvanTestResult1")
            //{
            //    jptCost1 += i;
            //}
            //else if (testProcess == "barCode" || testProcess == "Rotatry" || testProcess == "Cam2CCDCapture" || testProcess == "InspectMod2Unit" || testProcess == "CaptureCCDExposure2" ||  testProcess == "Dark Test2" || testProcess == "White Test2" || testProcess == "Spectrograph2" || testProcess == "SendImageAdvance2" || testProcess == "SendAdvanTestResult2")
            //{
            //    jptCost2 += i;
            //}



            //if (testProcess == "dest1" ) 
            //{
            //    //macminiCost1 += i;
            //    sw.WriteLine("CGhost Total Cost 1=" + macminiCost1 + "ms");
            //    sw.WriteLine("JPT Total Cost 1=" + jptCost1 + "ms");
            //    sw.WriteLine("Total Cost 1=" +( macminiCost1 + jptCost1) + "ms");
            //    sw.WriteLine("################### Test1 End ####################");
            //    macminiCost1 = 0;
            //    jptCost1 = 0;

            //}
            //if (testProcess == "dest2")
            //{
            //    //macminiCost2 += i;
            //    sw.WriteLine("CGhost Total Cost 2=" + macminiCost2 + "ms");
            //    sw.WriteLine("JPT Total Cost 2=" + jptCost2 + "ms");
            //    sw.WriteLine("Total Cost 2=" + (macminiCost2+jptCost2) + "ms");
            //    sw.WriteLine("################### Test2 End ####################");
            //    macminiCost2 = 0;
            //    jptCost2 = 0;

              
            //}
            //sw.Flush();
           // sw.Close();
        }
        //public void saveLog(string b, string PassOrFail)
        //{
        //    //string strTemp = "";
        //    //strTemp += DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+":";
        //    //strTemp += b + ",";
        //    //strTemp += PassOrFail;
        //    //sw.WriteLine(strTemp);
        //    //sw.Flush();
        //}
        //public void saveCycleTime(string channel,string barCode,string startTime, string endTime)
        //{
        //    string strTemp = "";
        //    strTemp += channel+"_"+barCode+"_"+DateTime.Now.ToString("yyyy-MM-dd") + ":";
        //    strTemp += "StartTime" + ":" + startTime + ",";
        //    strTemp += "EndTime" + endTime;
        //    sw.WriteLine(strTemp);
        //    sw.Flush();
        //}
        public void saveerrorLog(string errormessage)
        {
            lock (Para.logobj)
            {
                string s_filename = "D:\\ErrorLog";
                if (!Directory.Exists(s_filename))
                {
                    Directory.CreateDirectory(s_filename);
                }
                string FileName1 = s_filename + "\\ErrorLog.txt";
                FileStream objFileStream;
                if (!File.Exists(FileName1))
                {
                    objFileStream = new FileStream(FileName1, FileMode.CreateNew, FileAccess.Write);
                }
                else
                {
                    objFileStream = new FileStream(FileName1, FileMode.Append, FileAccess.Write);
                }
                StreamWriter sw1 = new StreamWriter(objFileStream, System.Text.Encoding.GetEncoding(-0));
                string strTemp = "";
                strTemp += DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ":____";
                strTemp += errormessage;
                sw1.WriteLine(strTemp);
                sw1.Close();
                objFileStream.Close();
            }
        }
    }
}
