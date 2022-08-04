using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CM.DataManagement
{
    public class ReadGameData : DataManager
    {
        public static bool ReadDefaultPath()
        {
            //Check for default path-------------------------------------------------------------------------------
            if (Directory.Exists(Application.persistentDataPath + "/" + CurrentGeneralApplicationData.ApplicationName))
            {
                DefaultPath = Application.persistentDataPath + "/" + CurrentGeneralApplicationData.ApplicationName;

                Debug.Log("DataManager : " + "Default save path has been found");

                return true;
            }
            //If it failed-------------------------------------------------------------------------------------------
            Debug.LogWarning("DataManager : " + "Failed to found the default save path");

            if (WriteGameData.CreateDefaultPath())
            {
                return ReadDefaultPath();
            }

            return false;
        }
        //-----------------------------------------------------------------------------------------------------------
        public static bool ReadPlayerProfiles()
        {
            //Try to get player profiles-----------------------------------------------------------------------------
            try
            {
                PlayerProfiles = Directory.GetDirectories(DefaultPath);

                if (!Directory.Exists(DefaultPath + "/Default"))
                {
                    WriteGameData.CreatePlayerProfile("Default");
                }

                for (int i = 0; i < PlayerProfiles.Length; i++)
                {
                    PlayerProfiles[i] = PlayerProfiles[i].Remove(0, DefaultPath.Length + 1);
                }

                Array.Sort(PlayerProfiles);

                Debug.Log("DataManager : " + "Player profiles has been found");

                return true;
            }
            //If it failed---------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to found player profiles" + Environment.NewLine + message);

                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ChangePlayerProfile(string name)
        {
            //Try to select a player profile---------------------------------------------------------------------------
            foreach (string playerProfile in PlayerProfiles)
            {
                if (playerProfile == name && name != "Default")
                {
                    CurrentPlayerProfile = name;

                    Debug.Log("DataManager : " + "A player profile has been selected");

                    return true;
                }
            }
            //If it failed----------------------------------------------------------------------------------------------
            Debug.LogError("DataManager : " + "No player profile has been found");

            return false;
        }
        //--------------------------------------------------------------------------------------------------------------
        public static bool ReadSaveSlots()
        {
            //Try to get save slots-------------------------------------------------------------------------------------
            try
            {
                string currentPlayerProfilesPath = DefaultPath + "/" + CurrentPlayerProfile;

                SaveSlots = Directory.GetDirectories(currentPlayerProfilesPath);

                if (!Directory.Exists(DefaultPath + "/" + CurrentPlayerProfile + "/Default"))
                {
                    WriteGameData.CreateSaveSlot("Default");
                }

                for (int i = 0; i < SaveSlots.Length; i++)
                {
                    SaveSlots[i] = SaveSlots[i].Remove(0, currentPlayerProfilesPath.Length + 1);
                }

                Array.Sort(SaveSlots);

                Debug.Log("DataManager : " + "Save slots has been found");

                return true;
            }
            //If it failed---------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to found save slots" + Environment.NewLine + message);

                return false;
            }
        }

        //------------------------------------------------------------------------------------------------------------
        public static bool ChangeSaveSlot(string name)
        {
            //Try to select a player profile--------------------------------------------------------------------------
            foreach (string saveSlot in SaveSlots)
            {
                if (saveSlot == name && name != "Default")
                {
                    CurrentSaveSlot = name;

                    Debug.Log("DataManager : " + "A save slot has been selected");

                    return true;
                }
            }
            //If it failed---------------------------------------------------------------------------------------------
            Debug.LogError("DataManager : " + "No save slot has been found");

            return false;
        }
        //------------------------------------------------------------------------------------------------------------
        public static bool LoadObject(ref object saveObject, string saveName, BinaryFormatter binaryFormatter)
        {
            FileStream file = null;
            //Try to load data----------------------------------------------------------------------------------------
            try
            {
                if (DefaultPath == null || CurrentPlayerProfile == null || CurrentSaveSlot == null)
                {
                    Debug.LogWarning("DataManager : " + "Save path is missing");
                    return false;
                }

                file = File.Open(DefaultPath + "/" + CurrentPlayerProfile + "/" + CurrentSaveSlot + "/" + saveName + ".coresave", FileMode.Open);

                saveObject = binaryFormatter.Deserialize(file);

                file.Close();

                Debug.Log("DataManager : " + "Data has been loaded");

                return true;
            }
            //If it failed-------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogWarning("DataManager : " + "Failed to load data" + Environment.NewLine + message);

                if (file != null)
                {
                    file.Close();
                }

                return false;
            }
        }
        //----------------------------------------------------------------------------------------------------------
        public static bool LoadSettingObject(ref object saveObject, string saveName, BinaryFormatter binaryFormatter)
        {
            FileStream file = null;
            //Try to load data--------------------------------------------------------------------------------------
            try
            {
                if (DefaultPath == null)
                {
                    Debug.LogWarning("DataManager : " + "Save path is missing");
                    return false;
                }

                file = File.Open(DefaultPath + "/" + saveName + ".coresave", FileMode.Open);

                saveObject = binaryFormatter.Deserialize(file);

                file.Close();

                Debug.Log("DataManager : " + "Data has been loaded");

                return true;
            }
            //If it failed------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogWarning("DataManager : " + "Failed to load data" + Environment.NewLine + message);

                if (file != null)
                {
                    file.Close();
                }

                return false;
            }
        }
    }
}