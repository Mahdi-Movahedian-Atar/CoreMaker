using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CM.DataManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace CM.DataManagement
{
    public class DataManagerSettingMainObject : MonoBehaviour
    {
        #region SaveSettingEvent

        private static UnityEvent<string> _saveSettingEvent;

        public static UnityEvent<string> SaveSettingEvent
        {
            get
            {
                if (_saveSettingEvent == null)
                {
                    _saveSettingEvent = new UnityEvent<string>();
                }

                return _saveSettingEvent;
            }
        }

        #endregion
        //--------------------------------------------------------------------------------------------------------
        #region LoadSettingEvent

        private static UnityEvent<string> _loadSettingEvent;

        public static UnityEvent<string> LoadSettingEvent
        {
            get
            {
                if (_loadSettingEvent == null)
                {
                    _loadSettingEvent = new UnityEvent<string>();
                }

                return _loadSettingEvent;
            }
        }

        #endregion
        //--------------------------------------------------------------------------------------------------------
        #region ResetSettingEvent

        private static UnityEvent<string> _resetSettingEvent;

        public static UnityEvent<string> ResetSettingEvent
        {
            get
            {
                if (_resetSettingEvent == null)
                {
                    _resetSettingEvent = new UnityEvent<string>();
                }

                return _resetSettingEvent;
            }
        }

        #endregion
        //========================================================================================================
        void Awake()
        {
            SaveSettingEvent.AddListener(type => {
                if (type == null)
                {
                    OnSettingSave();
                    return;
                }

                if (type == GetType().ToString())
                {
                    OnSettingSave();
                }
            });
            LoadSettingEvent.AddListener(type => {
                if (type == null)
                {
                    OnSettingLoad();
                    return;
                }

                if (type == GetType().ToString())
                {
                    OnSettingLoad();
                }
            });
            ResetSettingEvent.AddListener(type => {
                if (type == null)
                {
                    OnSettingReset();
                    return;
                }

                if (type == GetType().ToString())
                {
                    OnSettingReset();
                }
            });
        }
        //========================================================================================================
        protected virtual void OnSettingSave()
        {
        }
        //--------------------------------------------------------------------------------------------------------
        protected virtual void OnSettingLoad()
        {
        }
        //--------------------------------------------------------------------------------------------------------
        protected virtual void OnSettingReset()
        {
        }
    }
}