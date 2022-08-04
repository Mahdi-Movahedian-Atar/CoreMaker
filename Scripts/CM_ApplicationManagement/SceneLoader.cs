using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CM.ApplicationManagement
{
    public class SceneLoader : MonoBehaviour
    {
        #region SingletonSceneIndex
        private static int _sceneIndex;
        public static int SceneIndex
        {
            get { return _sceneIndex; }
        }
        #endregion

        #region SingletonLoadProgress
        private static float _loadProgress;
        public static float LoadProgress
        {
            get { return _loadProgress; }
        }
        #endregion

        //============================================================================================================

        void Start()
        {
            StartCoroutine(_loadScene());
        }

        //============================================================================================================

        public static void ChangeScene(int index)
        {
            SceneManager.LoadScene(0);

            _sceneIndex = index;
        }
        //------------------------------------------------------------------------------------------------------------
        private static IEnumerator _loadScene()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneIndex);

            do
            {
                _loadProgress = asyncOperation.progress;
                Debug.Log(_loadProgress);
                yield return null;
            }
            while (asyncOperation.isDone);
        }
    }
}