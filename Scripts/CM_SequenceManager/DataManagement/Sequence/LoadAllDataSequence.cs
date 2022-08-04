using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(LoadAllDataSequence), SequenceType.Sequence)]
    public class LoadAllDataSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.LoadAllData();
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new LoadAllDataSequence();
        }
    }
}
