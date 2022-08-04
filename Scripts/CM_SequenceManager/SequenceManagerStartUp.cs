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
            PrimaryApplicationStartUp.AddStartUp("SequenceManager", 2).AddListener(_startUp);
        }

        private void _startUp()
        {
            ApplicationManager.SetApplicationPartState("SequenceManager",false);

            if (!AssetDatabase.IsValidFolder("Assets/Resources/SequenceManager"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                AssetDatabase.CreateFolder("Assets/Resources", "SequenceManager");
            }

            Resources.Load<SequenceManagerData>("SequenceManager/" + SceneManager.GetActiveScene().name).SetSequenceManager();

            DataManagerEventHandler.LoadData(typeof(SequenceManagerSaveData));
            
            ApplicationManager.SetApplicationPartState("SequenceManager", true);
            
            SequenceManager.StartAllCurrentArguments();
        }
    }
}
