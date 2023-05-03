using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CM.ApplicationManagement;
using CM.DataManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CM.SequenceManager
{
    [RequireComponent(typeof(SequenceManagerSaveData), typeof(SequenceManager))]
    public class SequenceManagerStartUp : MonoBehaviour
    {
        public void Awake()
        {
            if (!gameObject.TryGetComponent(typeof(SequenceManagerSaveData), out _))
            {
                gameObject.AddComponent<SequenceManagerSaveData>();
            }

            if (!gameObject.TryGetComponent(typeof(SequenceManager), out _))
            {
                gameObject.AddComponent<SequenceManager>();
            }

            SequenceManagerElementAttribute.Internalize();
            PrimaryApplicationStartUp.AddStartUp("CM.SequenceManager").AddListener(_startUp);
        }

        private void _startUp()
        {
            ApplicationManager.SetApplicationPackageState("CM.SequenceManager", false);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            
            if (Resources.Load<SequenceManagerSelector>("SequenceManagerSelector").
                Selector.ContainsKey(SceneManager.GetActiveScene().name))
            {
                Resources.Load<SequenceManagerSelector>("SequenceManagerSelector").
                    Selector[SceneManager.GetActiveScene().name].SetSequenceManager();
            }

            DataManagerEventHandler.LoadData(typeof(SequenceManagerSaveData));

            ApplicationManager.SetApplicationPackageState("CM.SequenceManager", true);

            SequenceManager.StartAllCurrentArguments();
        }
    }
}