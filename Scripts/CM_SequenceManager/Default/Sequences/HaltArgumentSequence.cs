using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(HaltArgumentSequence), SequenceType.Sequence)]
    public class HaltArgumentSequence : Sequence
    {
        private string _parameterTargetTable;
        private string _targetTable;
        //----------------------------------------------------------------------------------------------------------------
        public string ParameterTargetTable
        {
            get
            {
                return _parameterTargetTable;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _targetTable = null;
                }

                _parameterTargetTable = value;
            }
        }
        public string TargetTable
        {
            get
            {
                return _targetTable;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _parameterTargetTable = null;
                }

                _targetTable = value;
            }
        }

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (!string.IsNullOrEmpty(_parameterTargetTable) && SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_parameterTargetTable))
            {
                _targetTable = SequenceManager.CurrentSequenceManager.StringParameters[_parameterTargetTable];
            }

            if (string.IsNullOrEmpty(_targetTable) || !SequenceManager.CurrentSequenceManager.SequenceTables.ContainsKey(_targetTable))
            {
                Debug.LogWarning("SequenceManager : No table found");
            }
            else
            {
                SequenceManager.HaltCurrentArgument(_targetTable);
            }
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            HaltArgumentSequence haltArgumentSequence = new HaltArgumentSequence();

            if (elementData.Bools[0]) haltArgumentSequence._parameterTargetTable = elementData.Strings[0];
            else haltArgumentSequence._targetTable = elementData.Strings[0];

            return haltArgumentSequence;
        }
    }
}