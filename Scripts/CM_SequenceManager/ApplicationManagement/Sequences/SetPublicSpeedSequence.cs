using System;
using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(SetPublicSpeedSequence), SequenceType.Sequence)]
    public class SetPublicSpeedSequence : Sequence
    {
        private bool _isItAParameter;

        private float _changeValue = 1;
        private string _changeValueParameter;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (_isItAParameter)
            {
                if (!String.IsNullOrEmpty(_changeValueParameter) &&
                    SequenceManager.CurrentSequenceManager.FloatParameters.ContainsKey(_changeValueParameter))
                {
                    _changeValue = SequenceManager.CurrentSequenceManager.FloatParameters[_changeValueParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : No FloatParameters found");
                }
            }

            ApplicationManager.CurrentApplicationManager.PublicSpeed = _changeValue;
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            SetPublicSpeedSequence setPublicSpeedSequence = new SetPublicSpeedSequence()
            {
                _changeValue = elementData.Floats[0],
                _changeValueParameter = elementData.Strings[0],
                _isItAParameter = elementData.Bools[0]
            };

            return setPublicSpeedSequence;
        }
    }
}
