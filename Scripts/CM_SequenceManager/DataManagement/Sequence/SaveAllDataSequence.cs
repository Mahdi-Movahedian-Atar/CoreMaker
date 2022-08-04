using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(SaveAllDataSequence), SequenceType.Sequence)]
    public class SaveAllDataSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.SaveAllData();
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new SaveAllDataSequence();
        }
    }
}
