using System;
using System.Collections;
using CM.ApplicationManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ChangeSceneSequence), SequenceType.Sequence)]
    public class ChangeSceneSequence : Sequence
    {
        private bool _isItAParameter;

        private int _sceneIndex;
        private string _sceneIndexParameter;

        //================================================================================================================

        public override IEnumerator Invoke()
        {
            if (_isItAParameter)
            {
                if (!String.IsNullOrEmpty(_sceneIndexParameter) &&
                    SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_sceneIndexParameter))
                {
                    _sceneIndex = SequenceManager.CurrentSequenceManager.IntParameters[_sceneIndexParameter];
                }
                else
                {
                    Debug.Log("SequenceManager : No FloatParameters found");
                }
            }

            if (_sceneIndex == 0 || SceneManager.sceneCount < _sceneIndex)
            {
                Debug.Log("SequenceManager : Wrong index : " + _sceneIndex);
                yield break;
            }

            SceneLoader.ChangeScene(_sceneIndex);
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            ChangeSceneSequence changeSceneSequence = new ChangeSceneSequence()
            {
                _sceneIndex = elementData.Ints[0],
                _sceneIndexParameter = elementData.Strings[0],
                _isItAParameter = elementData.Bools[0]
            };

            return changeSceneSequence;
        }
    }
}