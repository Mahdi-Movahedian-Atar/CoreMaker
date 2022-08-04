using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(SaveDataSequence),SequenceType.Sequence)]
    public class SaveDataSequence : Sequence
    {
        private string _type;

        public override IEnumerator Invoke()
        {
            DataManagerEventHandler.SaveData(_type);
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new SaveDataSequence(){_type = elementData.Strings[0]};
        }
    }
}
