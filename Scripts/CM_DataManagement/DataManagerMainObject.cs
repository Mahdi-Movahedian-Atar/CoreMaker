using System;
using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace CM.DataManagement
{
    public class DataManagerMainObject : MonoBehaviour
    {
        #region SaveEvent

        private static UnityEvent<string> _saveEvent;

        public static UnityEvent<string> SaveEvent
        {
            get
            {
                if (_saveEvent == null)
                {
                    _saveEvent = new UnityEvent<string>();
                }

                return _saveEvent;
            }
        }

        #endregion
        //--------------------------------------------------------------------------------------------------------
        #region LoadEvent

        private static UnityEvent<string> _loadEvent;

        public static UnityEvent<string> LoadEvent
        {
            get
            {
                if (_loadEvent == null)
                {
                    _loadEvent = new UnityEvent<string>();
                }

                return _loadEvent;
            }
        }

        #endregion
        //========================================================================================================
        void Awake()
        {
            SaveEvent.AddListener(type => {
                if (type == null)
                {
                    OnSave();
                    return;
                }

                if (type == GetType().ToString())
                {
                    OnSave();
                }
            });
            LoadEvent.AddListener(type => {
                if (type == null)
                {
                    OnSave();
                    return;
                }

                if (type == GetType().ToString())
                {
                    OnLoad();
                }
            });
        }
        //========================================================================================================
        protected virtual void OnSave()
        {
        }
        //--------------------------------------------------------------------------------------------------------
        protected virtual void OnLoad()
        {
        }

    }

}