using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ResetSettingAllDataSequence), SequenceType.Sequence)]
    public class ResetSettingAllDataSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.ResetAllSettingData();
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new ResetSettingAllDataSequence();
        }
    }
}
