using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CM.DataManagement
{
    public class DataManager : GeneralApplicationData
    {
        //PlayerProfiles==========================================================================================
        public static string[] PlayerProfiles;
        public static string CurrentPlayerProfile = "Default";
        //SaveSlots===============================================================================================
        public static string[] SaveSlots;
        public static string CurrentSaveSlot = "Default";
        //DefaultPath=============================================================================================
        public static string DefaultPath;
    }
}