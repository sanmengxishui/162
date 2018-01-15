using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JPTCG.Sequencing
{
    public class FunctionObjects
    {
        private Func<int> OnExecute;
        private Func<int,int> OnError;
        public bool isInvokeReq = false;
        public string Name = "";

        public FunctionObjects(string myName,Func<int> onExeFunction)
        {
            Name = myName;
            OnExecute += onExeFunction;
        }

        public FunctionObjects(string myName,Func<int> onExeFunction, Func<int, int> onErrorFunction)
        {
            Name = myName;
            OnExecute += onExeFunction;
            OnError += onErrorFunction;
        }

        public FunctionObjects(string myName, Func<int> onExeFunction, Func<int, int> onErrorFunction, bool isInvoke)
        {
            Name = myName;
            OnExecute += onExeFunction;
            OnError += onErrorFunction;
            isInvokeReq = isInvoke; 
        }

        public bool OnErrorAssigned()
        {
            return (OnError != null);
        }

        public int Execute()
        {            
            if (OnExecute == null)
                return 0;            
            return OnExecute();
        }

        public int DoError(int Res)
        {
            if (OnError == null)
                return 0;
            return OnError(Res);
        }

    }
}
