using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(SetArgumentSequence), SequenceType.Sequence)]
    public class SetArgumentSequence : Sequence
    {
        private string _targetTable;

        private string _targetArgument;

        public override IEnumerator Invoke()
        {
            SequenceManager.SetArgument(_targetTable, _targetArgument);

            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            SetArgumentSequence setArgumentSequence = new SetArgumentSequence()
            {
                _targetTable = elementData.Strings[0],
                _targetArgument = elementData.Strings[1]
            };
            return setArgumentSequence;
        }
    }
}
