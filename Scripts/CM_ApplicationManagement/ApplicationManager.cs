using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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

        public bool[] ApplicationPackageStates;
        public string[] ApplicationPackageNames;
        //------------------------------------------------------------------------------------------------------------------
        public static void SetApplicationPackageState(string packageName, bool state)
        {
            //Try to get Package-------------------------------------------------------------------------------------------
            for (int i = 0; i < CurrentApplicationManager.ApplicationPackageNames.Length; i++)
            {
                if (packageName == CurrentApplicationManager.ApplicationPackageNames[i])
                {
                    if (CurrentApplicationManager.ApplicationPackageStates[i] != state)
                    {
                        CurrentApplicationManager.ApplicationPackageStates[i] = state;

                        Debug.Log("ApplicationManager : " + packageName + " state is " + state);

                        //Check if the application is ready----------------------------------------------------------------------------
                        foreach (bool applicationPart in CurrentApplicationManager.ApplicationPackageStates)
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
            Debug.LogError("ApplicationManager : Failed to found " + packageName);
        }
    }
}