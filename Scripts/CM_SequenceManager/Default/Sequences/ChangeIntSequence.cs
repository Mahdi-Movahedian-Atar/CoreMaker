using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ChangeIntSequence), SequenceType.Sequence)]
    public class ChangeIntSequence : Sequence
    {
        private string _parameterName;

        private bool _isItAParameter;

        private int _changeValue;
        private string _changeValueParameter;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_parameterName))
            {
                if (_isItAParameter && SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_changeValueParameter))
                {
                    _changeValue = SequenceManager.CurrentSequenceManager.IntParameters[_changeValueParameter];
                }
                SequenceManager.CurrentSequenceManager.IntParameters[_parameterName] = _changeValue;
            }
            else
            {
                Debug.LogWarning("SequenceManager : No IntParameters found");
            }
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            ChangeIntSequence changeIntSequence = new ChangeIntSequence()
            {
                _parameterName = elementData.Strings[0],
                _isItAParameter = elementData.Bools[0]
            };

            if (elementData.Bools[0])
            {
                changeIntSequence._changeValueParameter = elementData.Strings[1];
                return changeIntSequence;
            }

            changeIntSequence._changeValue = elementData.Ints[0];
            return changeIntSequence;
        }
    }
}
