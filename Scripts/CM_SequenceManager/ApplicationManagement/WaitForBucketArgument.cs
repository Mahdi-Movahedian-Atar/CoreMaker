using System;
using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using UnityEngine;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(WaitForBucketArgument),SequenceType.Argument)]
    public class WaitForBucketArgument : Argument
    {

        private bool _isActAParameter;
        private string _act;

        private bool _isActedAParameter;
        private string _acted;

        private bool _isActorAParameter;
        private string _actor;

        private bool _isMassageAParameter;
        private string _massage;

        private bool _isActCountAParameter;
        private string _actCountParameter;
        private int _actCount;

        public override void EnterArgument()
        {
            if (_isActAParameter)
            {
                if (!String.IsNullOrEmpty(_act) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_act))
                {
                    _act = SequenceManager.CurrentSequenceManager.StringParameters[_act];
                }
                else
                {
                    Debug.Log("SequenceManager : WaitForBucketArgument : No StringParameters found");
                }
            }

            if (_isActedAParameter)
            {
                if (!String.IsNullOrEmpty(_acted) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_acted))
                {
                    _acted = SequenceManager.CurrentSequenceManager.StringParameters[_acted];
                }
                else
                {
                    Debug.Log("SequenceManager : WaitForBucketArgument : No StringParameters found");
                }
            }

            if (_isActorAParameter)
            {
                if (!String.IsNullOrEmpty(_actor) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_actor))
                {
                    _actor = SequenceManager.CurrentSequenceManager.StringParameters[_actor];
                }
                else
                {
                    Debug.Log("SequenceManager : WaitForBucketArgument : No StringParameters found");
                }
            }

            if (_isMassageAParameter)
            {
                if (!String.IsNullOrEmpty(_massage) &&
                    SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_massage))
                {
                    _massage = SequenceManager.CurrentSequenceManager.StringParameters[_massage];
                }
                else
                {
                    Debug.Log("SequenceManager : WaitForBucketArgument : No StringParameters found");
                }
            }

            if (_isActCountAParameter)
            {
                if (!String.IsNullOrEmpty(_actCountParameter) &&
                    SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_actCountParameter))
                {
                    _actCount = SequenceManager.CurrentSequenceManager.IntParameters[_actCountParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : WaitForBucketArgument : No IntParameters found");
                }
            }

            BucketSystem.SubscribeToBucket(_actor, _act, _acted, _actCount, "SequenceManager_" +TableName+ ArgumentName).AddListener(_next);
        }

        public override IEnumerator Invoke()
        {
            yield break;
        }

        public override void ExitArgument()
        {
            BucketSystem.UnSubscribeToBucket("SequenceManager_" + TableName + ArgumentName);
        }

        public override Argument MakeArgument(SequenceManagerElementData elementData)
        {
            WaitForBucketArgument waitForBucketArgument = new WaitForBucketArgument()
            {
                _isActAParameter = elementData.Bools[0],
                _isActedAParameter = elementData.Bools[1],
                _isActorAParameter = elementData.Bools[2],
                _isMassageAParameter = elementData.Bools[3],
                _isActCountAParameter = elementData.Bools[4],

                _act =  elementData.Strings[0],
                _acted =  elementData.Strings[1],
                _actor =  elementData.Strings[2],
                _massage =  elementData.Strings[3],
                _actCountParameter=  elementData.Strings[4],

                _actCount = elementData.Ints[0]
            };

            return waitForBucketArgument;
        }

        private void _next(BucketMassage bucketMassage)
        {
            if (bucketMassage.Massage == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(_massage))
            {
                SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
            }
            if (bucketMassage.Massage is string massage)
            {
                if (massage == _massage) SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
            }
            if (bucketMassage.Massage.ToString() == _massage)
            {
                SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
            }
        }
    }
}
