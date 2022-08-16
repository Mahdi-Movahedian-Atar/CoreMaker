using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using CM.DataManagement;
using CM.ApplicationManagement;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CM.Editor
{
    public class GeneralApplicationDataMenuItem : EditorWindow
    {
        [MenuItem("CoreMaker/GeneralApplicationData", false, 1)]
        private static void _applicationGameData()
        {
            GetWindow<GeneralApplicationDataMenuItem>("GeneralApplicationData");
        }

        private void OnGUI()
        {
            GeneralApplicationData.CurrentGeneralApplicationData.ApplicationName =
                EditorGUILayout.TextField("applicationName",
                    GeneralApplicationData.CurrentGeneralApplicationData.ApplicationName);
            GeneralApplicationData.CurrentGeneralApplicationData.ApplicationVersion = EditorGUILayout.TextField(
                "applicationVersion", GeneralApplicationData.CurrentGeneralApplicationData.ApplicationVersion);

            EditorUtility.SetDirty(GeneralApplicationData.CurrentGeneralApplicationData);
        }
    }
}