using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ActivateTableSequence), SequenceType.Sequence)]
    public class ActivateTableSequence : Sequence
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
                SequenceManager.CurrentSequenceManager.SequenceTables[_targetTable].IsTableActive = true;
            }

            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            ActivateTableSequence activateTableSequence = new ActivateTableSequence();

            if (elementData.Bools[0]) activateTableSequence._parameterTargetTable = elementData.Strings[0];
            else activateTableSequence._targetTable = elementData.Strings[0];

            return activateTableSequence;
        }
    }
}
