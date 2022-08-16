using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using CM.ApplicationManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CM.Editor
{
    public class ApplicationManagerMenuItem : EditorWindow
    {
        private Label _isApplicationReady;
        private FloatField _publicSpeed;
        private ScrollView _packageList;

        private List<string> _names = new List<string>();

        [MenuItem("CoreMaker/ApplicationManagerSetting", false, 10)]
        private static void _applicationManagerSetting()
        {
            GetWindow<ApplicationManagerMenuItem>("ApplicationManagerSetting");
        }

        private void OnEnable()
        {
            Toolbar toolbar = new Toolbar() {style = { minHeight = 20}};

            ObjectField objectField = new ObjectField("Default Values")
            {
                objectType = typeof(ApplicationManagerDefaultAsset),
                style = { maxWidth = 360}
            };
            objectField[0].style.minWidth = 80;
            objectField.RegisterValueChangedCallback(target =>
            {
                if (!EditorApplication.isPlaying && objectField.value != null)
                {
                    _publicSpeed.value = ((ApplicationManagerDefaultAsset)objectField.value).Speed;

                    if (((ApplicationManagerDefaultAsset)objectField.value).Packages != null)
                    {
                        ApplicationManager.CurrentApplicationManager.ApplicationPackageNames =
                            ((ApplicationManagerDefaultAsset)objectField.value).Packages;

                        _packageList.Clear();
                        for (int i = 0; i < ApplicationManager.CurrentApplicationManager.ApplicationPackageNames.Length; i++)
                        { _addMember(ApplicationManager.CurrentApplicationManager.ApplicationPackageNames[i], i); }
                    }
                }
            });

            toolbar.Add(objectField);
            toolbar.Add(new Button(() =>
                {
                    if (objectField.value != null && ApplicationManager.CurrentApplicationManager.ApplicationPackageNames != null)
                    {
                        ((ApplicationManagerDefaultAsset)objectField.value).Speed =
                            ApplicationManager.CurrentApplicationManager.PublicSpeed;
                        
                        ((ApplicationManagerDefaultAsset)objectField.value).Packages =
                            ApplicationManager.CurrentApplicationManager.ApplicationPackageNames;
                        
                        EditorUtility.SetDirty(objectField.value);
                        AssetDatabase.Refresh();
                    }
                })
            { text = "Save" });

            _isApplicationReady = new Label();
            _isApplicationReady.style.height = 20;

            _publicSpeed = new FloatField("Public speed") { value = ApplicationManager.CurrentApplicationManager.PublicSpeed };

            _packageList = new ScrollView(ScrollViewMode.VerticalAndHorizontal);

            for (int i = 0; i < ApplicationManager.CurrentApplicationManager.ApplicationPackageNames.Length; i++)
            { _addMember(ApplicationManager.CurrentApplicationManager.ApplicationPackageNames[i], i); }

            rootVisualElement.Add(toolbar);
            rootVisualElement.Add(new Label("Is application ready"));
            rootVisualElement.Add(_isApplicationReady);
            rootVisualElement.Add(new Label(" "));
            rootVisualElement.Add(_publicSpeed);
            rootVisualElement.Add(new Label(" "));
            rootVisualElement.Add(_packageList);
            rootVisualElement.Add
                (new Button(() => _addMember("New package", _packageList.childCount)) { text = "Add package" });
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                if (ApplicationManager.CurrentApplicationManager.IsApplicationReady)
                {
                    _isApplicationReady.style.backgroundColor = Color.green;
                    _isApplicationReady.text = "Application is ready";
                }
                else
                {
                    _isApplicationReady.style.backgroundColor = Color.red;
                    _isApplicationReady.text = "Application is not ready";
                }

                for (int i = 0; i < _packageList.childCount; i++)
                {
                    if (ApplicationManager.CurrentApplicationManager.ApplicationPackageStates[i])
                        _packageList[i].style.backgroundColor = Color.green;
                    else
                        _packageList[i].style.backgroundColor = Color.red;
                }
            }
            else
            {
                _isApplicationReady.style.backgroundColor = Color.red;
                _isApplicationReady.text = "Application has not been started";

                if (_publicSpeed.value < 0) _publicSpeed.value = 0;

                ApplicationManager.CurrentApplicationManager.PublicSpeed = _publicSpeed.value;

                _names = new List<string>();
                for (int i = 0; i < _packageList.childCount; i++)
                {
                    _names.Add(((TextField)_packageList[i][0]).text);
                    _packageList[i].style.backgroundColor = Color.grey;
                }

                ApplicationManager.CurrentApplicationManager.ApplicationPackageStates = new bool[_packageList.childCount];
                ApplicationManager.CurrentApplicationManager.ApplicationPackageNames = _names.ToArray();

                EditorUtility.SetDirty(ApplicationManager.CurrentApplicationManager);
            }
        }

        private void _addMember(string text, int index)
        {
            if (!Application.isPlaying)
            {
                _names.Insert(index, text);

                _packageList.Insert(index, new ScrollView(ScrollViewMode.Horizontal));
                _packageList[index].Add(new TextField() { value = text, style = { width = 200 } });
                _packageList[index].Add(new Button(() => { _packageList.RemoveAt(index); _setIndex(); }) { text = "Remove" });

                _setIndex();
            }
        }

        private void _setIndex()
        {
            if (!Application.isPlaying)
            {
                int count = _packageList.childCount;

                for (int i = 0; i < count; i++)
                {
                    if (_packageList[i].childCount == 3) _packageList[i].RemoveAt(1);

                    _packageList[i].Insert(1, _dropdownField(i));
                }
            }
        }

        private DropdownField _dropdownField(int index)
        {
            if (!Application.isPlaying)
            {
                List<string> choices = new List<string>();
                for (int i = 0; i < _packageList.childCount; i++) choices.Add(i.ToString());

                DropdownField dropdownField = new DropdownField(choices, index, (choice =>
                {
                    int order = Convert.ToInt32(choice);
                    string name = ((TextField)_packageList[index][0]).value;

                    if (_packageList[index].childCount == 3)
                    {
                        _packageList.RemoveAt(index);
                        _names.RemoveAt(index);
                        _addMember(name, order);
                    }

                    return choice;
                }));

                return dropdownField;
            }

            return null;
        }
    }
}