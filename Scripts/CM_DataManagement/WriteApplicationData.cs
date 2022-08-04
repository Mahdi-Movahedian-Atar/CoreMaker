using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using CM.DataManagement;
using UnityEngine.PlayerLoop;


namespace CM.DataManagement
{
    public class WriteGameData : DataManager
    {
        public static bool CreateDefaultPath()
        {
            //Try to create a default path---------------------------------------------------------------------------------------------
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/" + CurrentGeneralApplicationData.ApplicationName);

                CreatePlayerProfile("Default");

                Debug.Log("DataManager : " + "A default data path has been created");

                return true;
            }
            //If it failed--------------------------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to Create a default data path" + Environment.NewLine + message);

                return false;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public static bool CreatePlayerProfile(string name)
        {
            //fix the name----------------------------------------------------------------------------------------------------------------
            if (name.Contains("/"))
            {
                Debug.LogError("DataManager : " + "Name can not contain this ( / ) character");
                return false;
            }
            //Try to get create a player profile------------------------------------------------------------------------------------------
            try
            {
                foreach (string playerProfile in PlayerProfiles)
                {
                    if (playerProfile == name)
                    {
                        Debug.LogError("DataManager : " + "A profile with the same name already exists");
                        return false;
                    }
                }

                Directory.CreateDirectory(DefaultPath + "/" + name);
                Directory.CreateDirectory(DefaultPath + "/" + name + "/Default");

                ReadGameData.ReadPlayerProfiles();

                Debug.Log("DataManager : " + "A player profile has been created");

                return true;
            }
            //If it failed--------------------------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to Create player profile" + Environment.NewLine + message);

                return false;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public static bool RemovePlayerProfile(string name)
        {
            //Try to remove a player profile----------------------------------------------------------------------------------------------
            try
            {
                foreach (string playerProfile in PlayerProfiles)
                {
                    if (playerProfile == name && name != "Default")
                    {
                        Directory.Delete(DefaultPath + "/" + name);

                        Debug.Log("DataManager : " + "A player profile has been remove");

                        ReadGameData.ReadPlayerProfiles();

                        return true;
                    }
                }
            }
            //If it failed----------------------------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to remove player profile" + Environment.NewLine + message);

                return false;
            }

            Debug.LogError("DataManager : " + "No player profile has been found");

            return false;
        }
        //--------------------------------------------------------------------------------------------------------
        public static bool CreateSaveSlot(string name)
        {
            //fix the name----------------------------------------------------------------------------------------------------------------
            if (name.Contains("/"))
            {
                Debug.LogError("DataManager : " + "Name can not contain this ( / ) character");
                return false;
            }

            string fileName = DateTime.Now.ToFileTime() + "-" + name + " " + DateTime.Now.ToLongDateString();

            //Try to get create a save slot------------------------------------------------------------------------------------------------
            try
            {
                foreach (string saveSlot in SaveSlots)
                {
                    if (saveSlot == name)
                    {
                        Debug.LogError("DataManager : " + "A slot with the same name already exists");

                        return false;
                    }
                }

                Directory.CreateDirectory(DefaultPath + "/" + CurrentPlayerProfile + "/" + fileName);

                ReadGameData.ReadSaveSlots();

                Debug.Log("DataManager : " + "A save slot has been created");

                return true;
            }
            //If it failed-------------------------------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to create a save slot" + Environment.NewLine + message);

                return false;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public static bool RemoveSaveSlot(string name)
        {
            //Try to remove a save slot---------------------------------------------------------------------------------------------------
            try
            {
                foreach (string saveSlot in SaveSlots)
                {
                    if (saveSlot == name && name != "Default")
                    {
                        Directory.Delete(DefaultPath + "/" + CurrentPlayerProfile + "/" + name);

                        Debug.Log("DataManager : " + "A save slot has been remove");

                        ReadGameData.ReadSaveSlots();

                        return true;
                    }
                }
            }
            //If it failed----------------------------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to remove save slot" + Environment.NewLine + message);

                return false;
            }

            Debug.LogError("DataManager : " + "No save slot has been found");

            return false;
        }
        //--------------------------------------------------------------------------------------------------------
        public static bool SaveObject(object saveObject, string saveName, BinaryFormatter binaryFormatter)
        {
            FileStream file = null;
            //Try to save data--------------------------------------------------------------------------------------------------------------
            try
            {
                if (DefaultPath == null || CurrentPlayerProfile == null || CurrentSaveSlot == null)
                {
                    Debug.LogError("DataManager : " + "Save path is missing");
                    return false;
                }

                file = File.Create(DefaultPath + "/" + CurrentPlayerProfile + "/" + CurrentSaveSlot + "/" + saveName + ".coresave");

                binaryFormatter.Serialize(file, saveObject);

                file.Close();

                Debug.Log("DataManager : " + "Data has been saved");

                return true;
            }
            //If it failed----------------------------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to save data" + Environment.NewLine + message);

                if (file != null)
                {
                    file.Close();
                }

                return false;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public static bool SaveSettingObject(object saveObject, string saveName, BinaryFormatter binaryFormatter)
        {
            FileStream file = null;
            //Try to save data--------------------------------------------------------------------------------------------------------------
            try
            {
                if (DefaultPath == null)
                {
                    Debug.LogError("DataManager : " + "Save path is missing");
                    return false;
                }
                
                file = File.Create(DefaultPath + "/" + saveName + ".coresave");

                binaryFormatter.Serialize(file, saveObject);

                if (file != null)
                {
                    file.Close();
                }

                Debug.Log("DataManager : " + "Data has been saved");

                return true;
            }
            //If it failed----------------------------------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogError("DataManager : " + "Failed to save data" + Environment.NewLine + message);

                if (file != null)
                {
                    file.Close();
                }

                return false;
            }
        }
    }
}