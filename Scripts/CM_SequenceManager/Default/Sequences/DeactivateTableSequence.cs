using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(DeactivateTableSequence), SequenceType.Sequence)]
    public class DeactivateTableSequence : Sequence
    {
        private string _parameterTargetTable;

        private string _targetTable;

        //----------------------------------------------------------------------------------------------------------------

        public string ParameterTargetTable
        {
            get => _parameterTargetTable;
            set
            {
                if (!string.IsNullOrEmpty(value)) _targetTable = null;

                _parameterTargetTable = value;
            }
        }

        public string TargetTable
        {
            get => _targetTable;
            set
            {
                if (!string.IsNullOrEmpty(value)) _parameterTargetTable = null;

                _targetTable = value;
            }
        }

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (!string.IsNullOrEmpty(_parameterTargetTable) &&
                SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_parameterTargetTable))
                _targetTable = SequenceManager.CurrentSequenceManager.StringParameters[_parameterTargetTable];

            if (string.IsNullOrEmpty(_targetTable) || !SequenceManager.CurrentSequenceManager.SequenceTables.ContainsKey(_targetTable))
                Debug.LogWarning("SequenceManager : No table found");
            else
                SequenceManager.CurrentSequenceManager.SequenceTables[_targetTable].IsTableActive = false;
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            DeactivateTableSequence deactivateTableSequence = new DeactivateTableSequence();

            if (elementData.Bools[0]) deactivateTableSequence._parameterTargetTable = elementData.Strings[0];
            else deactivateTableSequence._targetTable = elementData.Strings[0];

            return deactivateTableSequence;
        }
    }
}