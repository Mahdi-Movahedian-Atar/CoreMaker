using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ChangeBoolSequence), SequenceType.Sequence)]
    public class ChangeBoolSequence : Sequence
    {
        private string _parameterName;

        private bool _isItAParameter;

        private bool _changeValue;
        private string _changeValueParameter;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (SequenceManager.CurrentSequenceManager.BoolParameters.ContainsKey(_parameterName))
            {
                if (_isItAParameter && SequenceManager.CurrentSequenceManager.BoolParameters.ContainsKey(_changeValueParameter))
                {
                    _changeValue = SequenceManager.CurrentSequenceManager.BoolParameters[_changeValueParameter];
                }
                SequenceManager.CurrentSequenceManager.BoolParameters[_parameterName] = _changeValue; 
            }
            else
            {
                Debug.LogWarning("SequenceManager : No BoolParameter found");
            }
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            ChangeBoolSequence changeBoolSequence = new ChangeBoolSequence()
            {
                _parameterName = elementData.Strings[0],
                _isItAParameter = elementData.Bools[0]
            };

            if (elementData.Bools[0])
            {
                changeBoolSequence._changeValueParameter = elementData.Strings[1];
                return changeBoolSequence;
            }

            changeBoolSequence._changeValue = elementData.Bools[1];
            return changeBoolSequence;
        }
    }
}