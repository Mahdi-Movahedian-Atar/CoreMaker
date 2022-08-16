using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CM.Editor
{
    public class StartUpMenuItem : EditorWindow
    {
        [MenuItem("CoreMaker/StartUpSetting" , false,11)]
        private static void _startUpSetting()
        {
            GetWindow<StartUpMenuItem>("StartUpSetting");
        }
        
        void OnGUI()
        {
            if (PrimaryApplicationStartUp.StartUpNames.Count != 0)
            {
                EditorGUILayout.LabelField("Primary StartUp");
                EditorGUILayout.Separator();

                int currentOrder = 0;
                //------------------------------------------------------------------------------------------------------------
                for (int i = 0; i < PrimaryApplicationStartUp.StartUpNames.Count; i++)
                {
                    EditorGUILayout.LabelField(PrimaryApplicationStartUp.StartUpNames[i]);

                    if (GUILayout.Button("Start " + PrimaryApplicationStartUp.StartUpNames[i] + "'s event"))
                    {
                        PrimaryApplicationStartUp.StartUpEvents[i].Invoke();
                    }
                }
                //------------------------------------------------------------------------------------------------------------
                EditorGUILayout.LabelField(
                    "=================================================================================================================================================================================================");
                EditorGUILayout.LabelField("Secondary StartUp");
                EditorGUILayout.Separator();
                //------------------------------------------------------------------------------------------------------------
                if (SecondaryApplicationStartUp.StartUpNames != null)
                {
                    currentOrder = 0;

                    for (int i = 0; i < SecondaryApplicationStartUp.StartUpNames.Count; i++)
                    {
                        if (SecondaryApplicationStartUp.StartUpOrder[i] == currentOrder)
                        {
                            currentOrder++;

                            EditorGUILayout.LabelField(SecondaryApplicationStartUp.StartUpNames[i]);

                            if (GUILayout.Button("Start " + SecondaryApplicationStartUp.StartUpNames[i] + "'s event"))
                            {
                                SecondaryApplicationStartUp.StartUpEvents[i].Invoke();
                            }

                            i = -1;
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("Game is not active yet");
            }
        }
    }
}