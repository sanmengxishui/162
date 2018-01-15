using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Sequencing
{
    class MainSeqController
    {            
        List<WorkerThread> workerList = new List<WorkerThread>();       
        private bool isMainBusy = false;
        private ListView myUILV;

        public MainSeqController()
        {            
        }
       
        public int Start()
        {
            if (workerList.Count == 0)
                return -1;

            if (isBusy())
                return -2;

            //Start all worker thread
            for (int i = 0; i < workerList.Count; i++)
            {
                workerList[i].Start();
            }     

            return 0;
        }
        public void AddWorkerThread(WorkerThread myThread)
        {
            workerList.Add(myThread);
            UpdateLV();
        }

        public bool isBusy()
        {           
            for (int i = 0; i < workerList.Count; i++)
                if (workerList[i].IsBusy)
                    return true;

            return false;
        }

        public void ResetAll()
        {
            if (isBusy())
            {
                return; 
            }
            for (int i = 0; i < workerList.Count; i++)
                workerList[i].Reset();
        }

        public void AssignUI(ListView myLV)
        {
            myUILV = myLV;
            UpdateLV();
        }

        private void UpdateLV()
        {
            if (myUILV == null)
                return;

            myUILV.Clear();            
            myUILV.Columns.Add("Idx", 40);
            myUILV.Columns.Add("WorkerName", 100);
            myUILV.Columns.Add("Fun Idx", 60);
            myUILV.Columns.Add("Function", 100);
            this.myUILV.View = System.Windows.Forms.View.Details;

            for (int i = 0; i < workerList.Count; i++)
            {
                ListViewItem item = myUILV.Items.Add(myUILV.Items.Count + "");
                item.SubItems.Add(workerList[i].Name);
                //item.SubItems.Add(myMgr.SpecList[ModIdx].Criteria[i].Min.ToString("F2"));
                //item.SubItems.Add(myMgr.SpecList[ModIdx].Criteria[i].Max.ToString("F2"));
                item.EnsureVisible();
            }
        }
    }
}
