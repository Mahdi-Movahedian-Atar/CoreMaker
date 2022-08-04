using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(WaitForSecondsArgument), SequenceType.Argument)]
    public class WaitForSecondsArgument : Argument
    {
        private string _parameterSeconds;
        private int _seconds;
        //----------------------------------------------------------------------------------------------------------------
        public string ParameterSeconds
        {
            get
            {
                return _parameterSeconds;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _seconds = 0;
                }

                _parameterSeconds = value;
            }
        }
        public int seconds
        {
            get
            {
                return _seconds;
            }
            set
            {
                if (value != 0)
                {
                    _parameterSeconds = null;
                }

                _seconds = value;
            }
        }

        //================================================================================================================

        public override void EnterArgument()
        {
            if (!string.IsNullOrEmpty(_parameterSeconds) && SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_parameterSeconds))
            {
                _seconds = SequenceManager.CurrentSequenceManager.IntParameters[_parameterSeconds];
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public override IEnumerator Invoke()
        {
            yield return new WaitForSeconds(_seconds);
            yield return null;

            SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
        }
        //----------------------------------------------------------------------------------------------------------------
        public override void ExitArgument()
        {

        }
        //----------------------------------------------------------------------------------------------------------------
        public override Argument MakeArgument(SequenceManagerElementData elementData)
        {
            WaitForSecondsArgument waitForSecondsArgument = new WaitForSecondsArgument();

            if (elementData.Strings != null && elementData.Strings.Length != 0)
                waitForSecondsArgument._parameterSeconds = elementData.Strings[0];
            else
                waitForSecondsArgument._seconds = elementData.Ints[0];

            return waitForSecondsArgument;
        }
    }
}