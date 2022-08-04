using System;
using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using UnityEngine;
using static CM.SequenceManager.SequenceManager;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(TrowToBucketSequence), SequenceType.Sequence)]
    public class TrowToBucketSequence : Sequence
    {
        private bool _isActAParameter;
        private string _actParameter;

        private bool _isActedAParameter;
        private string _actedParameter;
        
        private bool _isActorAParameter;
        private string _actorParameter;
        
        private bool _isMassageAParameter;
        private string _massageParameter;

        private bool _isActCountAParameter;
        private string _actCountParameter;

        private BucketMassage _bucketMassage;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (_isActAParameter)
            {
                if (!String.IsNullOrEmpty(_actParameter) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_actParameter))
                {
                    _bucketMassage.Act = CurrentSequenceManager.StringParameters[_actParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : TrowToBucketSequence : No StringParameters found");
                }
            }

            if (_isActedAParameter)
            {
                if (!String.IsNullOrEmpty(_actedParameter) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_actedParameter))
                {
                    _bucketMassage.Acted = CurrentSequenceManager.StringParameters[_actedParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : TrowToBucketSequence : No StringParameters found");
                }
            }

            if (_isActorAParameter)
            {
                if (!String.IsNullOrEmpty(_actorParameter) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_actorParameter))
                {
                    _bucketMassage.Actor = CurrentSequenceManager.StringParameters[_actorParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : TrowToBucketSequence : No StringParameters found");
                }
            }

            if (_isMassageAParameter)
            {
                if (!String.IsNullOrEmpty(_massageParameter) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_massageParameter))
                {
                    _bucketMassage.Massage = CurrentSequenceManager.StringParameters[_massageParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : TrowToBucketSequence : No StringParameters found");
                }
            }

            if (_isActCountAParameter)
            {
                if (!String.IsNullOrEmpty(_actCountParameter) &&
                    SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_actCountParameter))
                {
                    _bucketMassage.ActCount = CurrentSequenceManager.IntParameters[_actCountParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : TrowToBucketSequence : No IntParameters found");
                }
            }


            BucketSystem.TrowToBucket(_bucketMassage);
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            TrowToBucketSequence bucketSequence = new TrowToBucketSequence()
            {
                _isActAParameter = elementData.Bools[0],
                _isActedAParameter = elementData.Bools[1],
                _isActorAParameter = elementData.Bools[2],
                _isMassageAParameter = elementData.Bools[3],
                _isActCountAParameter = elementData.Bools[4]
            };

            BucketMassage bucketMassage = new BucketMassage();

            if (!elementData.Bools[0]) bucketMassage.Act = elementData.Strings[0];
            else bucketSequence._actParameter = elementData.Strings[0];

            if (!elementData.Bools[1]) bucketMassage.Acted = elementData.Strings[1];
            else bucketSequence._actedParameter = elementData.Strings[1];

            if (!elementData.Bools[2]) bucketMassage.Actor = elementData.Strings[2];
            else bucketSequence._actorParameter = elementData.Strings[2];

            if (!elementData.Bools[3]) bucketMassage.Massage = elementData.Strings[3];
            else bucketSequence._massageParameter = elementData.Strings[3];

            if (!elementData.Bools[4]) bucketMassage.ActCount = elementData.Ints[0];
            else bucketSequence._actCountParameter = elementData.Strings[4];

            bucketSequence._bucketMassage = bucketMassage;

            return bucketSequence;
        }
    }
}
