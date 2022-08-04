using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CM.ApplicationManagement
{
    public class BucketSystem : MonoBehaviour
    {
        private static List<BucketMassage> _bucketMassages;
        //------------------------------------------------------------------------------------------------------------
        private static List<UnityEvent<BucketMassage>> _bucketMassageEvents;
        private static List<string> _bucketMassageNames;
        //------------------------------------------------------------------------------------------------------------
        private static List<string> _actorFilter;
        private static List<string> _actFilter;
        private static List<string> _actedFilter;
        private static List<int> _actCountFilter;
        //============================================================================================================
        void LateUpdate()
        {
            //Check if there is any massage---------------------------------------------------------------------------
            if (_bucketMassages != null && _bucketMassageNames!= null)
            {
                //Filter massage--------------------------------------------------------------------------------------
                foreach (BucketMassage bucketMassage in _bucketMassages)
                {
                    for (int i = 0; i < _bucketMassageEvents.Count; i++)
                    {
                        if (_actorFilter[i] == null || _actorFilter[i] == bucketMassage.Actor)
                        {
                            if (_actFilter[i] == null || _actFilter[i] == bucketMassage.Act)
                            {
                                if (_actedFilter[i] == null || _actedFilter[i] == bucketMassage.Acted)
                                {
                                    if (_actCountFilter[i] == 0 || _actCountFilter[i] == bucketMassage.ActCount)
                                    {
                                        Debug.Log("ApplicationManager : Bucket : " + bucketMassage);
                                        _bucketMassageEvents[i].Invoke(bucketMassage);
                                    }
                                }
                            }
                        }
                    }
                }
                //Empty the bucket------------------------------------------------------------------------------------
                _bucketMassages = null;
            }
        }
        //============================================================================================================
        public static void TrowToBucket(BucketMassage bucketMassage)
        {
            if (_bucketMassages == null)
            {
                _bucketMassages = new List<BucketMassage>();
            }

            _bucketMassages.Add(bucketMassage);
        }
        //------------------------------------------------------------------------------------------------------------
        public static UnityEvent<BucketMassage> SubscribeToBucket(string actorFilter, string actFilter, string actedFilter, int actCountFilter , string eventName)
        {
            if (_bucketMassageEvents == null)
            {
                _bucketMassageEvents = new List<UnityEvent<BucketMassage>>();
                _bucketMassageNames = new List<string>();
                _actorFilter= new List<string>();
                _actFilter =new List<string>();
                _actedFilter = new List<string>();
                _actCountFilter = new List<int>();
            }

            _bucketMassageEvents.Add(new UnityEvent<BucketMassage>());
            _bucketMassageNames.Add(eventName);

            _actorFilter.Add(actorFilter);
            _actFilter.Add(actFilter);
            _actedFilter.Add(actedFilter);
            _actCountFilter.Add(actCountFilter);

            return _bucketMassageEvents.Last();
        }

        public static void UnSubscribeToBucket(string eventName)
        {
            if (_bucketMassageEvents == null)
            {
                return;
            }

            if (_bucketMassageNames.Contains(eventName))
            {
                _bucketMassageEvents.RemoveAt(_bucketMassageNames.IndexOf(eventName));
                _bucketMassageNames.RemoveAt(_bucketMassageNames.IndexOf(eventName));
            }
        }
    }
}