using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CM.ApplicationManagement
{
    public class SecondaryApplicationStartUp : MonoBehaviour
    {
        public static List<string> StartUpNames;
        public static List<UnityEvent> StartUpEvents;
        public static List<int> StartUpOrder;

        public static void StartUp()
        {
            int currentOrder = 0;

            if (StartUpNames == null)
            {
                return;
            }

            for (int i = 0; i < StartUpOrder.Count; i++)
            {
                if (currentOrder == StartUpOrder[i])
                {
                    currentOrder++;

                    StartUpEvents[i].Invoke();

                    Debug.Log(StartUpNames[i] + "ApplicationManager: " + " has been started");

                    i = 0;
                }
            }
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