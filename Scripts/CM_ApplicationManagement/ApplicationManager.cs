using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CM.ApplicationManagement
{
    public class ApplicationManager : ScriptableObject
    {
        #region Singleton
        private static ApplicationManager _currentApplicationManager;
        public static ApplicationManager CurrentApplicationManager
        {
            get
            {
                if (_currentApplicationManager == null)
                {
                    if (Resources.LoadAll<ApplicationManager>("ApplicationManager").Length != 1)
                    {
                        _currentApplicationManager = CreateInstance<ApplicationManager>();

                        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                        {
                            AssetDatabase.CreateFolder("Assets", "Resources");
                        }
                        AssetDatabase.CreateAsset(_currentApplicationManager, "Assets/Resources/ApplicationManager.asset");
                    }
                    _currentApplicationManager = Resources.Load<ApplicationManager>("ApplicationManager");
                }

                return _currentApplicationManager;
            }
        }
        #endregion

        public float PublicSpeed = 1;
        public bool IsApplicationReady = false;

        public bool[] ApplicationPartStates;
        public string[] ApplicationPartNames;
        //------------------------------------------------------------------------------------------------------------------
        public static void SetApplicationPartState(string partName, bool state)
        {
            //Try to get applicationPart-------------------------------------------------------------------------------------------
            for (int i = 0; i < CurrentApplicationManager.ApplicationPartNames.Length; i++)
            {
                if (partName == CurrentApplicationManager.ApplicationPartNames[i])
                {
                    if (CurrentApplicationManager.ApplicationPartStates[i] != state)
                    {
                        CurrentApplicationManager.ApplicationPartStates[i] = state;

                        Debug.Log("ApplicationManager : " + partName + " state is " + state);

                        //Check if the application is ready----------------------------------------------------------------------------
                        foreach (bool applicationPart in CurrentApplicationManager.ApplicationPartStates)
                        {
                            if (!applicationPart)
                            {
                                return;
                            }
                        }

                        CurrentApplicationManager.IsApplicationReady = true;

                        Debug.Log("ApplicationManager : Application is ready");
                    }
                    return;
                }
            }
            //If it failed--------------------------------------------------------------------------------------------------
            Debug.LogError("ApplicationManager : Failed to found " + partName);
        }
    }
}