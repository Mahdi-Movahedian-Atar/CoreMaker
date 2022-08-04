using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ChangeStringSequence), SequenceType.Sequence)]
    public class ChangeStringSequence : Sequence
    {
        private string _parameterName;

        private bool _isItAParameter;

        private string _changeValue;
        private string _changeValueParameter;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_parameterName))
            {
                if (_isItAParameter && SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_changeValueParameter))
                {
                    _changeValue = SequenceManager.CurrentSequenceManager.StringParameters[_changeValueParameter];
                }

                SequenceManager.CurrentSequenceManager.StringParameters[_parameterName] = _changeValue;
            }
            else
            {
                Debug.LogWarning("SequenceManager : No StringParameters found");
            }

            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            ChangeStringSequence changeStringSequence = new ChangeStringSequence()
            {
                _parameterName = elementData.Strings[0],
                _isItAParameter = elementData.Bools[0]
            };

            if (elementData.Bools[0])
            {
                changeStringSequence._changeValueParameter = elementData.Strings[1];
                return changeStringSequence;
            }

            changeStringSequence._changeValue = elementData.Strings[1];
            return changeStringSequence;
        }
    }
}