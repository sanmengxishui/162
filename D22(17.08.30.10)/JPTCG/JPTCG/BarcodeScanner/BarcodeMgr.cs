using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPTCG.BarcodeScanner
{
    public class BarcodeMgr
    {
        public List<KeyenceBarcode> barcodeList = new List<KeyenceBarcode>();

        public BarcodeMgr()
        {
            
        }
        ~BarcodeMgr()
        { }

        public void AddBarcode(string name)
        {
            KeyenceBarcode myBarcode = new KeyenceBarcode(name);
            barcodeList.Add(myBarcode);
        }

        public void LoadSettings(string FileName)
        {
            for (int i = 0; i < barcodeList.Count; i++)
                barcodeList[i].LoadSettings(FileName);
        }

        public void SaveSettings(string FileName)
        {
            for (int i = 0; i < barcodeList.Count; i++)
                barcodeList[i].SaveSettings(FileName);
        }

        public void ShowSettings()
        {
            WinBarcode myWin = new WinBarcode(this);
            myWin.ShowDialog();
        }

        public void Disable()
        {
            
        }
    }
}
