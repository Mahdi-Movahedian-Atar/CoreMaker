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
        public static List<string> StartUpNames = new List<string>();
        public static List<UnityEvent> StartUpEvents = new List<UnityEvent>();
        public static List<int> StartUpOrder = new List<int>();

        void Awake()
        {
            ApplicationManager.CurrentApplicationManager.PublicSpeed = 1;
            ApplicationManager.CurrentApplicationManager.IsApplicationReady = false;
            for (int i = 0; i < ApplicationManager.CurrentApplicationManager.ApplicationPartStates.Length; i++)
            {
                ApplicationManager.CurrentApplicationManager.ApplicationPartStates[i] = false;
            }
        }

        void Start()
        {
            int currentOrder = 0;

            for (int i = 0; i < StartUpOrder.Count; i++)
            {
                if (currentOrder == StartUpOrder[i])
                {
                    Debug.Log("ApplicationManager: " + StartUpNames[i] + " has been started");

                    currentOrder++;

                    StartUpEvents[i].Invoke();

                    i = -1;
                }
            }

            SecondaryApplicationStartUp.StartUp();
        }

        public static UnityEvent AddStartUp(string startUpName, int startUpOrder)
        {
            if (!startUpName.Contains(startUpName))
            {
                ApplicationManager.CurrentApplicationManager.ApplicationPartStates[startUpName.IndexOf(startUpName)] = false;
            }
            else
            {
                StartUpNames.Add(startUpName);

                foreach (int startOrder in StartUpOrder)
                {
                    if (startUpOrder == startOrder)
                    {
                        for (int i = 0; i < StartUpOrder.Count; i++)
                        {
                            if (startUpOrder >= StartUpOrder[i])
                            {
                                StartUpOrder[i]++;
                            }
                        }
                    }
                }

                StartUpOrder.Add(startUpOrder);

                StartUpEvents.Add(new UnityEvent());
            }

            return StartUpEvents[startUpName.IndexOf(startUpName)];
        }
    }
}