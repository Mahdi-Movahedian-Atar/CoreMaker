using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ResetSettingDataSequence), SequenceType.Sequence)]
    public class ResetSettingDataSequence : Sequence
    {
        private string _type;

        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.ResetSettingData(_type);
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new ResetSettingDataSequence() { _type = elementData.Strings[0] };
        }
    }
}
