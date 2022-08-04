using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CM.SequenceManager;
using UnityEngine;

namespace CM.InputManagement
{
    [SequenceManagerElement(typeof(ChangeInputTableSequence), SequenceType.Sequence)]
    public class ChangeInputTableSequence : Sequence
    {
        private string _parameterTable;
        private string _table;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (!string.IsNullOrEmpty(_parameterTable) && SequenceManager.SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_parameterTable))
            {
                _table = SequenceManager.SequenceManager.CurrentSequenceManager.StringParameters[_parameterTable];
            }

            if (string.IsNullOrEmpty(_table) || !InputManager.CurrentInputManager.InputTableNames.Contains(_table))
            {
                Debug.LogWarning("SequenceManager : No table found");
            }
            else
            {
                InputManager.CurrentInputManager.ChangeCurrentInputTable(_table);
            }
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            ChangeInputTableSequence changeInputTableSequence = new ChangeInputTableSequence();

            if (elementData.Bools[0])
                changeInputTableSequence._parameterTable = elementData.Strings[0];
            else
                changeInputTableSequence._table = elementData.Strings[0];

            return changeInputTableSequence;
        }
    }
}
