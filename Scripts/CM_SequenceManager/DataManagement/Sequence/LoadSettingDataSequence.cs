using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(LoadSettingDataSequence), SequenceType.Sequence)]
    public class LoadSettingDataSequence : Sequence
    {
        private string _type;

        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.LoadSettingData(_type);
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new LoadSettingDataSequence() { _type = elementData.Strings[0] };
        }
    }
}
