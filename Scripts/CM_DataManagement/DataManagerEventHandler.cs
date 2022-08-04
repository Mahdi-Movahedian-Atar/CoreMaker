using System;
using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using UnityEngine;

namespace CM.DataManagement
{
    public class DataManagerEventHandler
    {
        //Call save events from here==============================================================================
        public static void SaveData(Type type)
        {
            DataManagerMainObject.SaveEvent.Invoke(type.ToString());
        }
        public static void SaveData(string type)
        {
            DataManagerMainObject.SaveEvent.Invoke(type);
        }
        //............................................................
        public static void SaveAllData()
        {
            DataManagerMainObject.SaveEvent.Invoke(null);
        }
        //--------------------------------------------------------------------------------------------------------
        public static void LoadData(Type type)
        {
            DataManagerMainObject.LoadEvent.Invoke(type.ToString());
        }
        public static void LoadData(string type)
        {
            DataManagerMainObject.LoadEvent.Invoke(type);
        }
        //............................................................
        public static void LoadAllData()
        {
            DataManagerMainObject.LoadEvent.Invoke(null);
        }
        //Call setting save Events from here======================================================================
        public static void SaveSettingData(Type type)
        {
            DataManagerSettingMainObject.SaveSettingEvent.Invoke(type.ToString());
        }
        public static void SaveSettingData(string type)
        {
            DataManagerSettingMainObject.SaveSettingEvent.Invoke(type);
        }
        //............................................................
        public static void SaveAllSettingData()
        {
            DataManagerSettingMainObject.SaveSettingEvent.Invoke(null);
        }
        //--------------------------------------------------------------------------------------------------------
        public static void LoadSettingData(Type type)
        {
            DataManagerSettingMainObject.LoadSettingEvent.Invoke(type.ToString());
        }
        public static void LoadSettingData(string type)
        {
            DataManagerSettingMainObject.LoadSettingEvent.Invoke(type);
        }
        //............................................................
        public static void LoadAllSettingData()
        {
            DataManagerSettingMainObject.LoadSettingEvent.Invoke(null);
        }
        //--------------------------------------------------------------------------------------------------------
        public static void ResetSettingData(Type type)
        {
            DataManagerSettingMainObject.ResetSettingEvent.Invoke(type.ToString());
        }
        public static void ResetSettingData(string type)
        {
            DataManagerSettingMainObject.ResetSettingEvent.Invoke(type);
        }
        //............................................................
        public static void ResetAllSettingData()
        {
            DataManagerSettingMainObject.ResetSettingEvent.Invoke(null);
        }
    }
}