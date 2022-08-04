using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.SequenceManager
{
    public abstract class Sequence
    {
        public string SequenceName;
        public string TableName;
        public string TargetArgumentName;

        public abstract IEnumerator Invoke();

        public abstract Sequence MakeSequence(SequenceManagerElementData elementData);
    }
}