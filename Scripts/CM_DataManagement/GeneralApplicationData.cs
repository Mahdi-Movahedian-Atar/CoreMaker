using UnityEditor;
using UnityEngine;

namespace CM.DataManagement
{
    public class GeneralApplicationData : ScriptableObject
    {
        #region Singleton
        private static GeneralApplicationData _currentGeneralApplicationData;
        public static GeneralApplicationData CurrentGeneralApplicationData
        {
            get
            {
                if (_currentGeneralApplicationData == null)
                {
                    if (Resources.LoadAll<GeneralApplicationData>("GeneralApplicationData").Length == 0)
                    {
                        _currentGeneralApplicationData = CreateInstance<GeneralApplicationData>();

                        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                        {
                            AssetDatabase.CreateFolder("Assets", "Resources");
                        }
                        AssetDatabase.CreateAsset(_currentGeneralApplicationData, "Assets/Resources/GeneralApplicationData.asset");
                    }
                    _currentGeneralApplicationData = Resources.Load<GeneralApplicationData>("GeneralApplicationData");
                }

                return _currentGeneralApplicationData;
            }
        }
        #endregion

        public string ApplicationName;

        public string ApplicationVersion;
    }
}