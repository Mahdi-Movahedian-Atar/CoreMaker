using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(LoadDataSequence), SequenceType.Sequence)]
    public class LoadDataSequence : Sequence
    {
        private string _type;

        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.LoadData(_type);
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new LoadDataSequence() { _type = elementData.Strings[0] };
        }
    }
}
