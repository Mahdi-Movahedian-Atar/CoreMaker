using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace CM.ApplicationManagement
{
    public class PrimaryApplicationStartUp : MonoBehaviour
    {
        private static bool _isInternalize = false;

        public static List<string> StartUpNames = new List<string>();
        public static List<UnityEvent> StartUpEvents = new List<UnityEvent>();

        void Awake()
        {
            _internalize();
        }

        void Start()
        {
            for (int i = 0; i < StartUpNames.Count; i++)
            {
                Debug.Log("ApplicationManager : " + StartUpNames[i] + " has been started");
                
                StartUpEvents[i].Invoke();
            }

            SecondaryApplicationStartUp.StartUp();
        }

        public static UnityEvent AddStartUp(string packageName)
        {
            if (!_isInternalize) _internalize();

            if (StartUpNames.Contains(packageName))
            {
                return StartUpEvents[StartUpNames.IndexOf(packageName)];
            }

            Debug.Log($"ApplicationManager : {packageName} does not exist");
            return null;
        }

        private static void _internalize()
        {
            ApplicationManager.CurrentApplicationManager.PublicSpeed = 1;
            ApplicationManager.CurrentApplicationManager.IsApplicationReady = false;
            for (int i = 0; i < ApplicationManager.CurrentApplicationManager.ApplicationPackageNames.Length; i++)
            {
                StartUpNames.Add(ApplicationManager.CurrentApplicationManager.ApplicationPackageNames[i]);
                StartUpEvents.Add(new UnityEvent());

                ApplicationManager.CurrentApplicationManager.ApplicationPackageStates[i] = false;
            }

            _isInternalize = true;
        }
    }
}