using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(SaveAllSettingDataSequence), SequenceType.Sequence)]
    public class SaveAllSettingDataSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.SaveAllSettingData();
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new SaveAllSettingDataSequence();
        }
    }
}
