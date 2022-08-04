using System.Collections;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(PrintSequence),SequenceType.Sequence)]
    public class PrintSequence : Sequence
    {
        private string _parameterMessage;
        private string _message;
        //----------------------------------------------------------------------------------------------------------------
        public string ParameterMessage
        {
            get
            {
                return _parameterMessage;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _message = null;
                }

                _parameterMessage = value;
            }
        }
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _parameterMessage = null;
                }

                _message = value;
            }
        }

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (!string.IsNullOrEmpty(_parameterMessage) && SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_parameterMessage))
            {
                _message = SequenceManager.CurrentSequenceManager.StringParameters[_parameterMessage];
            }

            if (string.IsNullOrEmpty(_message))
            {
                Debug.LogWarning("SequenceManager : No message found");
            }

            else
            {
                Debug.Log(_message);
            }
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            PrintSequence printSequence = new PrintSequence();

            if (elementData.Bools[0])
                printSequence._parameterMessage = elementData.Strings[0];
            else
                printSequence._message = elementData.Strings[0];

            return printSequence;
        }
    }
}