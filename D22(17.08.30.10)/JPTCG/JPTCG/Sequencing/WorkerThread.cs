using JPTCG.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPTCG.Sequencing
{
    class WorkerThread
    {
        public delegate void OnFunctionComplete(int Idx, bool IsError);
        public event OnFunctionComplete OnFuncComplete;

        public delegate void OnFunctionStart(int Idx);
        public event OnFunctionStart OnFuncStart;

        public delegate void OnThreadStart();
        public event OnThreadStart OnThreadStarted;

        public delegate void OnThreadStop();
        public event OnThreadStop OnThreadStoppedOnError;

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
        }

        private bool isExit = true;
        private int runIdx = 0;        
        private Thread wkThread = null;
        public bool isError = false;        
        public bool StopOnError = true;
        bool pauseSeq = false;

        public string Name = "";
        private Form mainWd;
        private int NextJumpIndex = -1;

        List<FunctionObjects> functionList = new List<FunctionObjects>();

        public WorkerThread(string myName, Form myWin)
        {            
            //wkThread.Start();
            mainWd = myWin;            
            runIdx = 0;
            Name = myName;
        }
        ~WorkerThread()  // destructor
        {
            isExit = true;
            if (wkThread != null)
                wkThread.Abort();
        }

        public void AssignSeqList(List<FunctionObjects> seqList)
        {
            functionList = seqList;
        }

        public void AddFunction(string FunctionName, Func<int> onExeFunction)
        {
            FunctionObjects myFun = new FunctionObjects(FunctionName, onExeFunction);
            functionList.Add(myFun);
        }

        public void AddFunction(string FunctionName, Func<int> onExeFunction, Func<int, int> onErrorFunction)
        {
            FunctionObjects myFun = new FunctionObjects(FunctionName,onExeFunction, onErrorFunction, false);
            functionList.Add(myFun);
        }
        public void AddFunction(string FunctionName, Func<int> onExeFunction, Func<int, int> onErrorFunction, bool ReqInvoke)
        {
            FunctionObjects myFun = new FunctionObjects(FunctionName,onExeFunction, onErrorFunction, ReqInvoke);
            functionList.Add(myFun);
        }

        private void WaitPauseOnError()
        {
            while (pauseSeq)
            {
                isBusy = false;
                if (isExit)
                    return;
                Application.DoEvents();
                Thread.Sleep(10);
            }
            isBusy = true;
        }

        //res 99 = jumpIndex
        private void StartWorkerThread()
        {
            int res = 0;
            while (true)
            {
                isBusy = true;
                isError = false;

                if (OnFuncStart != null)
                    OnFuncStart(runIdx);

                if (functionList[runIdx].isInvokeReq)
                {
                    Action ac = new Action(() =>
                    {
                        res = functionList[runIdx].Execute();
                        Thread.Sleep(2);
                    });

                    mainWd.BeginInvoke(ac);
                }
                else
                {
                    res = functionList[runIdx].Execute();
                }

                if (res < 0)
                {
                    if (functionList[runIdx].OnErrorAssigned())
                    {
                        Action ac = new Action(() =>
                        {
                            res = functionList[runIdx].DoError(res); //Alvin 2/3/17
                            Thread.Sleep(2);
                        });

                        mainWd.BeginInvoke(ac);
                        isError = true;
                    }
                }

                if (OnFuncComplete != null)
                    OnFuncComplete(runIdx, isError);

                if (StopOnError)
                {
                    if (isError)
                    {
                        if (OnThreadStoppedOnError != null)
                        {
                            Action ac = new Action(() =>
                            {
                                OnThreadStoppedOnError();
                                Thread.Sleep(2);
                            });

                            mainWd.BeginInvoke(ac);                            
                        }
                        pauseSeq = true;                        
                    }
                }

                WaitPauseOnError();
                if (isExit)
                    break;

                if (res == 99) //JumpIndex
                {
                    runIdx = NextJumpIndex;
                    continue;
                }

                if (!isError)
                {                                            
                    runIdx++;                    
                    if (runIdx >= functionList.Count) //Auto ResetIndex
                        runIdx = 0;
                }
                if (isExit)
                    break;
                Thread.Sleep(5);
            }

            isBusy = false;
        }
        
        public int Start()
        {
            if (functionList.Count == 0)
                return -1;

            if (isBusy)
            {                
                return -2;
            }

            if (isExit)
            {
                isExit = false;
                wkThread = new Thread(new ThreadStart(StartWorkerThread));
                wkThread.IsBackground = true;
                wkThread.Start();
            }

            if (pauseSeq) //resume
            {
                pauseSeq = false;                
            }

            if (OnThreadStarted != null)
                OnThreadStarted();

            return 0;
        }
        public int Stop()
        {
            if (functionList.Count == 0)
                return -1;

            isExit = true;
            while (isBusy)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }

            return 0;
        }
        public int Pause()
        {
            pauseSeq = true;
            while (isBusy)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
            return 0;
        }

        public bool IsPause
        {
            get { return pauseSeq; }
        }
        public int Reset()
        {
            if (functionList.Count == 0)
                return -1;

            if (isBusy)
            {
                Stop();                
                //return -2;
            }
            runIdx = 0;
            return 0;
        }
        public int CurrentIndex()
        {
            return runIdx;
        }
        public int JumpIndex(int Idx)        
        {
            if (isBusy)
                NextJumpIndex = Idx;
            else
                runIdx = Idx;
            return 0;
        }
    }
}
