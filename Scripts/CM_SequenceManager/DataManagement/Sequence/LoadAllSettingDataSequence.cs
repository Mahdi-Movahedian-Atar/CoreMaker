using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(LoadAllSettingDataSequence), SequenceType.Sequence)]
    public class LoadAllSettingDataSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.LoadAllSettingData();
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new LoadAllSettingDataSequence();
        }
    }
}
