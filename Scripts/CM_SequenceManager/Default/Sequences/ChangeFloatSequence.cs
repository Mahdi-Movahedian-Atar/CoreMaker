using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ChangeFloatSequence), SequenceType.Sequence)]
    public class ChangeFloatSequence : Sequence
    {
        private string _parameterName;

        private bool _isItAParameter;

        private float _changeValue;
        private string _changeValueParameter;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (SequenceManager.CurrentSequenceManager.FloatParameters.ContainsKey(_parameterName))
            {
                if (_isItAParameter && SequenceManager.CurrentSequenceManager.FloatParameters.ContainsKey(_changeValueParameter))
                {
                    _changeValue = SequenceManager.CurrentSequenceManager.FloatParameters[_changeValueParameter];
                }
                SequenceManager.CurrentSequenceManager.FloatParameters[_parameterName] = _changeValue;
            }
            else
            {
                Debug.LogWarning("SequenceManager : No FloatParameters found");
            }
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            ChangeFloatSequence changeFloatSequence = new ChangeFloatSequence()
            {
                _parameterName = elementData.Strings[0],
                _isItAParameter = elementData.Bools[0]
            };

            if (elementData.Bools[0])
            {
                changeFloatSequence._changeValueParameter = elementData.Strings[1];
                return changeFloatSequence;
            }

            changeFloatSequence._changeValue = elementData.Floats[0];
            return changeFloatSequence;
        }
    }
}