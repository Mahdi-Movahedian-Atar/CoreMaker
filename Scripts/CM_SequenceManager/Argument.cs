using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.SequenceManager
{
    public abstract class Argument
    {
        public string ArgumentName;
        public string TableName;
        public string[] TargetSequenceName;

        public abstract void EnterArgument();
        public abstract IEnumerator Invoke();
        public abstract void ExitArgument();

        public abstract Argument MakeArgument(SequenceManagerElementData elementData);
    }
}