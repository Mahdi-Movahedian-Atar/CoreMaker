using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CM.SequenceManager;
using UnityEngine;

namespace CM.InputManagement
{
    [SequenceManagerElement(typeof(WaitForKeyArgument), SequenceType.Argument)]
    public class WaitForKeyArgument : Argument
    {
        private string _parameterTable;
        private string _table;

        private string _parameterKeyCodeName;
        private string _keyCodeName;

        public override void EnterArgument()
        {
            if (!string.IsNullOrEmpty(_parameterTable) && SequenceManager.SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_parameterTable))
            {
                _table = SequenceManager.SequenceManager.CurrentSequenceManager.StringParameters[_parameterTable];
            }

            if (string.IsNullOrEmpty(_table))
            {
                Debug.LogWarning("SequenceManager : WaitForKeyArgument : given table is null");
            }
            else
            {
                if (!string.IsNullOrEmpty(_parameterKeyCodeName) && SequenceManager.SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_parameterKeyCodeName))
                {
                    _table = SequenceManager.SequenceManager.CurrentSequenceManager.StringParameters[_parameterKeyCodeName];
                }

                if (string.IsNullOrEmpty(_table))
                {
                    Debug.LogWarning("SequenceManager : WaitForKeyArgument : given parameterKeyCodeName is null");
                }
                else
                {
                    InputManager.CurrentInputManager.GetEvent(_table, _keyCodeName).AddListener(_waitForKey);
                }
            }
        }

        public override IEnumerator Invoke()
        {
            yield return null;
        }

        public override void ExitArgument()
        {
            if (string.IsNullOrEmpty(_table))
            {
                Debug.LogWarning("SequenceManager : WaitForKeyArgument : given table is null");
            }
            else
            {
                if (string.IsNullOrEmpty(_table))
                {
                    Debug.LogWarning("SequenceManager : WaitForKeyArgument : given parameterKeyCodeName is null");
                }
                else
                {
                    InputManager.CurrentInputManager.GetEvent(_table, _keyCodeName).RemoveListener(_waitForKey);
                }
            }
        }

        public override Argument MakeArgument(SequenceManagerElementData elementData)
        {
            WaitForKeyArgument waitForKeyArgument = new WaitForKeyArgument();

            if (elementData.Bools[0])
                waitForKeyArgument._parameterTable = elementData.Strings[0];
            else
                waitForKeyArgument._table = elementData.Strings[0];

            if (elementData.Bools[1])
                waitForKeyArgument._parameterKeyCodeName = elementData.Strings[1];
            else
                waitForKeyArgument._keyCodeName = elementData.Strings[1];

            return waitForKeyArgument;
        }

        private void _waitForKey()
        {
            SequenceManager.SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
        }
    }
}
