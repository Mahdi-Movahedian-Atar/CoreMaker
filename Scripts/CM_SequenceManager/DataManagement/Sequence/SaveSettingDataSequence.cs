using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(SaveSettingDataSequence), SequenceType.Sequence)]
    public class SaveSettingDataSequence : Sequence
    {
        private string _type;

        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.SaveSettingData(_type);
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new SaveSettingDataSequence() { _type = elementData.Strings[0] };
        }
    }
}
