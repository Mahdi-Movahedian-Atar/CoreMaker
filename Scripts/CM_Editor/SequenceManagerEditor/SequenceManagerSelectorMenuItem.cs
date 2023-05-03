using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CM.SequenceManager;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    public class SequenceManagerSelectorMenuItem : EditorWindow
    {
        private ScrollView _selectorContainer;
        private SequenceManagerSelector _selector;

        private List<int> _effectedIndexes = new List<int>();
        private string[] _scenes;
        private SequenceManagerData[] _sequenceManagerDatas;

        [MenuItem("CoreMaker/SequenceManagerSelector", false, 21)]
        private static void _sequenceManagerSelector()
        {
            GetWindow<SequenceManagerSelectorMenuItem>("SequenceManagerSelectorMenuItem");
        }

        private void OnEnable()
        {
            rootVisualElement.style.backgroundColor = new Color(0.2745098f, 0.3529412f, 0.509804f, 1);
            _selectorContainer = SM_ScrollView(ScrollViewMode.Vertical);
            //=====
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            _scenes = new string[sceneCount - 1];
            _sequenceManagerDatas = new SequenceManagerData[sceneCount - 1];
            for (int i = 0; i < sceneCount - 1; i++)
            {
                _scenes[i] =
                    System.IO.Path.GetFileNameWithoutExtension(
                        UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i + 1));
            }
            //======

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            _selector = AssetDatabase.LoadAssetAtPath<SequenceManagerSelector>(
                "Assets/Resources/SequenceManagerSelector.asset");

            //======
            rootVisualElement.Add(SM_Button("Save", () => _Save()));
            rootVisualElement[0].style.maxWidth = 30000;
            rootVisualElement.Add(_selectorContainer);

            for (int i = 0; i < _scenes.Length; i++)
            {
                if (_selector != null && _selector.Selector.ContainsKey(_scenes[i]))
                {
                    _sequenceManagerDatas[i] = _selector.Selector[_scenes[i]];
                    ObjectField element = _createListElement(_scenes[i], i);

                    element.label = "(" + _sequenceManagerDatas[i].Name + ")" + _scenes[i];
                    element.style.backgroundColor = new Color(0.06038627f, 0.5566038f, 0.06572194f);

                    _selectorContainer.Add(element);
                }
                else
                {
                    ObjectField element = _createListElement(_scenes[i], i);

                    element.label = "(Empty)" + _scenes[i];
                    element.style.backgroundColor = new Color(0.8113208f, 0.1787202f, 0.1569064f);

                    _selectorContainer.Add(element);
                }
            }
        }

        private ObjectField _createListElement(string scene, int index)
        {
            ObjectField objectContainer = SM_ObjectField(scene, null, typeof(SequenceManagerAsset));
            objectContainer.name = scene;
            objectContainer[0].style.width = 200;
            objectContainer[1].style.maxWidth = 30000;

            objectContainer.RegisterValueChangedCallback(evt =>
            {
                if (!_effectedIndexes.Contains(index))
                    _effectedIndexes.Add(index);

                if (objectContainer.value == null)
                {
                    objectContainer.style.backgroundColor = new Color(0.8113208f, 0.1787202f, 0.1569064f);
                    objectContainer.label = "(Empty)" + scene;
                    _sequenceManagerDatas[index] = null;
                }
                else
                {
                    objectContainer.style.backgroundColor = new Color(0.06038627f, 0.5566038f, 0.06572194f);
                    objectContainer.label = "(" + ((SequenceManagerAsset)objectContainer.value).Name + ")" + scene;
                    _sequenceManagerDatas[index] = ((SequenceManagerAsset)objectContainer.value).Data;
                }
            });

            return objectContainer;
        }

        private void _Save()
        {
            List<string> scenes = new List<string>();
            List<SequenceManagerData> sequenceManagerDatas = new List<SequenceManagerData>();

            for (int i = 0; i < _scenes.Length; i++)
            {
                if (_sequenceManagerDatas[i] != null)
                {
                    scenes.Add(_scenes[i]);
                    sequenceManagerDatas.Add(_sequenceManagerDatas[i]);
                }
            }

            _selector = CreateInstance<SequenceManagerSelector>();
            _selector.Setup(scenes.ToArray(), sequenceManagerDatas.ToArray());

            AssetDatabase.CreateAsset(_selector, "Assets/Resources/SequenceManagerSelector.asset");
            
            EditorUtility.SetDirty(_selector);
            AssetDatabase.Refresh();
        }
    }
}