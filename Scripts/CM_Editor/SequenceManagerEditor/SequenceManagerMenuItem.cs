using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CM.ApplicationManagement;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using CM.SequenceManager;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;
using Button = UnityEngine.UIElements.Button;
using Toggle = UnityEngine.UIElements.Toggle;

namespace CM.Editor.SequenceManagerEditor
{
    public class SequenceManagerMenuItem : EditorWindow
    {
        public SequenceManagerAsset CurrentAsset
        {
            get => (SequenceManagerAsset)((ObjectField)_toolContainer.ElementAt(3)).value;
            set => ((ObjectField)_toolContainer.ElementAt(3)).value = value;
        }

        private List<SequenceManagerGraphView> _graphViews = new List<SequenceManagerGraphView>();
        private List<bool> _isChanged = new List<bool>();

        private int _currentGraphViewIndex = -1;

        private VisualElement _graphViewContainer;
        private VisualElement _toolContainer;

        private ScrollView _sequenceTablesContainer;
        private ScrollView _stringParametersContainer;
        private ScrollView _intParametersContainer;
        private ScrollView _floatParametersContainer;
        private ScrollView _boolParametersContainer;

        [MenuItem("CoreMaker/SequenceManager", false, 20)]
        private static void _sequenceManager()
        {
            GetWindow<SequenceManagerMenuItem>("SequenceManagerMenuItem");
        }

        private void OnEnable()
        {
            NodeTypeGroups.Internalize();

            _sequenceTablesContainer = SM_ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _sequenceTablesContainer.style.borderLeftWidth = 0;
            _sequenceTablesContainer.style.borderRightWidth = 0;
            _sequenceTablesContainer.style.borderBottomWidth = 0;
            _sequenceTablesContainer.style.borderTopWidth = 0;
            _stringParametersContainer = SM_ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _stringParametersContainer.style.borderLeftWidth = 0;
            _stringParametersContainer.style.borderRightWidth = 0;
            _stringParametersContainer.style.borderBottomWidth = 0;
            _stringParametersContainer.style.borderTopWidth = 0;
            _intParametersContainer = SM_ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _intParametersContainer.style.borderLeftWidth = 0;
            _intParametersContainer.style.borderRightWidth = 0;
            _intParametersContainer.style.borderBottomWidth = 0;
            _intParametersContainer.style.borderTopWidth = 0;
            _floatParametersContainer = SM_ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _floatParametersContainer.style.borderLeftWidth = 0;
            _floatParametersContainer.style.borderRightWidth = 0;
            _floatParametersContainer.style.borderBottomWidth = 0;
            _floatParametersContainer.style.borderTopWidth = 0;
            _boolParametersContainer = SM_ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _boolParametersContainer.style.borderLeftWidth = 0;
            _boolParametersContainer.style.borderRightWidth = 0;
            _boolParametersContainer.style.borderBottomWidth = 0;
            _boolParametersContainer.style.borderTopWidth = 0;

            ObjectField objectField = SM_ObjectField(null, null, typeof(SequenceManagerAsset));
            objectField.RegisterValueChangedCallback(target =>
            {
                _sequenceTablesContainer.Clear();
                _stringParametersContainer.Clear();
                _intParametersContainer.Clear();
                _floatParametersContainer.Clear();
                _boolParametersContainer.Clear();

                _currentGraphViewIndex = -1;
                _loadAsset();
            });

            _toolContainer = SM_ScrollView(ScrollViewMode.Vertical);

            _toolContainer.Add(SM_Button("Save", _saveAsset));
            _toolContainer.Add(SM_Button("Load", _loadAsset));
            _toolContainer.Add(SM_Button("Set", _setSequenceManager));
            _toolContainer.Add(objectField);

            _toolContainer.Add(SM_Foldout("Sequence Tables"));
            _toolContainer.ElementAt(_toolContainer.childCount - 1).Add(_sequenceTablesContainer);

            _toolContainer.Add(SM_Foldout("String Parameters"));
            _toolContainer.ElementAt(_toolContainer.childCount - 1).Add(_stringParametersContainer);

            _toolContainer.Add(SM_Foldout("Int Parameters"));
            _toolContainer.ElementAt(_toolContainer.childCount - 1).Add(_intParametersContainer);

            _toolContainer.Add(SM_Foldout("Float Parameters"));
            _toolContainer.ElementAt(_toolContainer.childCount - 1).Add(_floatParametersContainer);

            _toolContainer.Add(SM_Foldout("Bool Parameters"));
            _toolContainer.ElementAt(_toolContainer.childCount - 1).Add(_boolParametersContainer);

            _graphViewContainer = new VisualElement();
            _graphViewContainer.style.backgroundColor = new Color(0, 0.09019608f, 0.33333334f);

            TwoPaneSplitView twoPaneSplitView = new TwoPaneSplitView(0, 225, TwoPaneSplitViewOrientation.Horizontal);

            twoPaneSplitView.Add(_toolContainer);
            twoPaneSplitView.Add(_graphViewContainer);
            twoPaneSplitView.style.backgroundColor = Color.blue + Color.blue;

            rootVisualElement.Add(twoPaneSplitView);
        }

        private void _setGraph(int index)
        {
            _graphViewContainer.Clear();

            if (_currentGraphViewIndex != -1)
            {
                _sequenceTablesContainer.ElementAt(_currentGraphViewIndex).style.backgroundColor =
                    new Color(0.2745098f, 0.3529412f, 0.509804f);
            }

            if (index != -1)
            {
                _sequenceTablesContainer.ElementAt(index).style.backgroundColor =
                    new Color(0, 0.09019608f, 0.33333334f);
                _isChanged[index] = true;
                _graphViewContainer.Add(_graphViews[index]);
            }

            _currentGraphViewIndex = index;
        }

        private void _createTableContainer()
        {
            _sequenceTablesContainer.Clear();
            _graphViews = new List<SequenceManagerGraphView>();
            _isChanged = new List<bool>();

            if (CurrentAsset.GraphViewDataList != null)
            {
                SequenceManagerGraphViewData[] graphViewData = CurrentAsset.GraphViewDataList.ToArray();

                for (int i = 0; i < graphViewData.Length; i++)
                {
                    ScrollView row = SM_ScrollView(ScrollViewMode.Horizontal);
                    row.style.borderLeftWidth = 0;
                    row.style.borderRightWidth = 0;
                    row.style.borderBottomWidth = 0;
                    row.style.borderTopWidth = 0;

                    int index = i;

                    row.Add(SM_Button(null, () => _setGraph(index)));

                    Button button = SM_Button(null, () => _removeTable(index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField(graphViewData[i].GraphName));

                    row.Add(SM_Toggle(CurrentAsset.GraphViewStateList[i]));

                    _sequenceTablesContainer.Add(row);

                    SequenceManagerGraphView graphView = new SequenceManagerGraphView(graphViewData[i]);
                    SequenceManagerSearchWindow searchWindow = CreateInstance<SequenceManagerSearchWindow>();
                    searchWindow.Initialize(graphView, this);
                    graphView.nodeCreationRequest = context =>
                        SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
                    graphView.StretchToParentSize();

                    _graphViews.Add(graphView);
                    _isChanged.Add(false);
                }
            }

            _sequenceTablesContainer.Add(SM_Button("New Table", _addTable));
        }

        private void _addTable()
        {
            ScrollView row = SM_ScrollView(ScrollViewMode.Horizontal);
            row.style.borderLeftWidth = 0;
            row.style.borderRightWidth = 0;
            row.style.borderBottomWidth = 0;
            row.style.borderTopWidth = 0;

            int index = _sequenceTablesContainer.childCount - 1;

            row.Add(SM_Button(null, () => _setGraph(index)));

            Button button = SM_Button(null, () => _removeTable(index));
            button.style.backgroundColor = Color.red;
            row.Add(button);

            row.Add(SM_TextField("NewElement"));

            row.Add(SM_Toggle(true));

            _sequenceTablesContainer.Insert(_sequenceTablesContainer.childCount - 1, row);

            SequenceManagerGraphView graphView = new SequenceManagerGraphView(new SequenceManagerGraphViewData());
            SequenceManagerSearchWindow searchWindow = CreateInstance<SequenceManagerSearchWindow>();
            searchWindow.Initialize(graphView, this);
            graphView.nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            graphView.StretchToParentSize();

            _graphViews.Add(graphView);
            _isChanged.Add(true);
        }

        private void _removeTable(int index)
        {
            _sequenceTablesContainer.RemoveAt(index);
            _graphViews.RemoveAt(index);
            _isChanged.RemoveAt(index);

            for (int i = index; i < _sequenceTablesContainer.childCount - 1; i++)
            {
                int newIndex = i;

                Button selectButton = (SM_Button(null, () => _setGraph(newIndex)));

                Button removeButton = SM_Button(null, () => _removeTable(newIndex));
                removeButton.style.backgroundColor = Color.red;

                _sequenceTablesContainer.ElementAt(i).RemoveAt(0);
                _sequenceTablesContainer.ElementAt(i).Insert(0, selectButton);

                _sequenceTablesContainer.ElementAt(i).RemoveAt(1);
                _sequenceTablesContainer.ElementAt(i).Insert(1, removeButton);
            }
        }

        private void _createParametersContainer()
        {
            _stringParametersContainer.Clear();
            if (CurrentAsset.StringParameterNames != null)
            {
                for (int i = 0; i < CurrentAsset.StringParameterNames.Count; i++)
                {
                    ScrollView row = SM_ScrollView(ScrollViewMode.Horizontal);
                    row.style.borderLeftWidth = 0;
                    row.style.borderRightWidth = 0;
                    row.style.borderBottomWidth = 0;
                    row.style.borderTopWidth = 0;

                    int index = i;
                    Button button = SM_Button(null, () => _removeParameter(0, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField(CurrentAsset.StringParameterNames[i]));
                    row.Add(SM_TextField(CurrentAsset.StringParameterValues[i]));

                    _stringParametersContainer.Add(row);
                }
            }

            _stringParametersContainer.Add(SM_Button("New String", () => _addParameter(0)));

            _intParametersContainer.Clear();
            if (CurrentAsset.IntParameterNames != null)
            {
                for (int i = 0; i < CurrentAsset.IntParameterNames.Count; i++)
                {
                    ScrollView row = SM_ScrollView(ScrollViewMode.Horizontal);
                    row.style.borderLeftWidth = 0;
                    row.style.borderRightWidth = 0;
                    row.style.borderBottomWidth = 0;
                    row.style.borderTopWidth = 0;

                    int index = i;
                    Button button = SM_Button(null, () => _removeParameter(1, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField(CurrentAsset.IntParameterNames[i]));
                    row.Add(SM_IntField(CurrentAsset.IntParameterValues[i]));

                    _intParametersContainer.Add(row);
                }
            }

            _intParametersContainer.Add(SM_Button("New Int", () => _addParameter(1)));

            _floatParametersContainer.Clear();
            if (CurrentAsset.FloatParameterNames != null)
            {
                for (int i = 0; i < CurrentAsset.FloatParameterNames.Count; i++)
                {
                    ScrollView row = SM_ScrollView(ScrollViewMode.Horizontal);
                    row.style.borderLeftWidth = 0;
                    row.style.borderRightWidth = 0;
                    row.style.borderBottomWidth = 0;
                    row.style.borderTopWidth = 0;

                    int index = i;
                    Button button = SM_Button(null, () => _removeParameter(2, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField(CurrentAsset.FloatParameterNames[i]));
                    row.Add(SM_FloatField(CurrentAsset.FloatParameterValues[i]));

                    _floatParametersContainer.Add(row);
                }
            }

            _floatParametersContainer.Add(SM_Button("New Float", () => _addParameter(2)));

            _boolParametersContainer.Clear();
            if (CurrentAsset.BoolParameterNames != null)
            {
                for (int i = 0; i < CurrentAsset.BoolParameterNames.Count; i++)
                {
                    ScrollView row = SM_ScrollView(ScrollViewMode.Horizontal);
                    row.style.borderLeftWidth = 0;
                    row.style.borderRightWidth = 0;
                    row.style.borderBottomWidth = 0;
                    row.style.borderTopWidth = 0;

                    int index = i;
                    Button button = SM_Button(null, () => _removeParameter(3, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField(CurrentAsset.BoolParameterNames[i]));
                    row.Add(SM_Toggle(CurrentAsset.BoolParameterValues[i]));

                    _boolParametersContainer.Add(row);
                }
            }

            _boolParametersContainer.Add(SM_Button("New Bool", () => _addParameter(3)));
        }

        private void _addParameter(int parameterIndex)
        {
            ScrollView row = SM_ScrollView(ScrollViewMode.Horizontal);
            row.style.borderLeftWidth = 0;
            row.style.borderRightWidth = 0;
            row.style.borderBottomWidth = 0;
            row.style.borderTopWidth = 0;

            int index;
            Button button;

            switch (parameterIndex)
            {
                case 0:
                    index = _stringParametersContainer.childCount - 1;
                    button = SM_Button(null, () => _removeParameter(0, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField("New String"));
                    row.Add(SM_TextField("New String"));

                    _stringParametersContainer.Insert(_stringParametersContainer.childCount - 1, row);

                    break;

                case 1:
                    index = _intParametersContainer.childCount - 1;
                    button = SM_Button(null, () => _removeParameter(1, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField("New Int"));
                    row.Add(SM_IntField(0));

                    _intParametersContainer.Insert(_intParametersContainer.childCount - 1, row);

                    break;

                case 2:
                    index = _floatParametersContainer.childCount - 1;
                    button = SM_Button(null, () => _removeParameter(2, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField("New Float"));
                    row.Add(SM_FloatField(0));

                    _floatParametersContainer.Insert(_floatParametersContainer.childCount - 1, row);

                    break;

                case 3:
                    index = _boolParametersContainer.childCount - 1;
                    button = SM_Button(null, () => _removeParameter(3, index));
                    button.style.backgroundColor = Color.red;
                    row.Add(button);

                    row.Add(SM_TextField("New Bool"));
                    row.Add(SM_Toggle(false));

                    _boolParametersContainer.Insert(_boolParametersContainer.childCount - 1, row);

                    break;
            }
        }

        private void _removeParameter(int parameterIndex, int index)
        {
            switch (parameterIndex)
            {
                case 0:
                    _stringParametersContainer.RemoveAt(index);

                    for (int i = index; i < _stringParametersContainer.childCount - 1; i++)
                    {
                        int newIndex = i;
                        Button button = SM_Button(null, () => _removeParameter(0, newIndex));
                        button.style.backgroundColor = Color.red;

                        _stringParametersContainer.ElementAt(i).RemoveAt(0);
                        _stringParametersContainer.ElementAt(i).Insert(0, button);
                    }

                    break;

                case 1:
                    _intParametersContainer.RemoveAt(index);

                    for (int i = index; i < _intParametersContainer.childCount - 1; i++)
                    {
                        int newIndex = i;
                        Button button = SM_Button(null, () => _removeParameter(1, newIndex));
                        button.style.backgroundColor = Color.red;

                        _intParametersContainer.ElementAt(i).RemoveAt(0);
                        _intParametersContainer.ElementAt(i).Insert(0, button);
                    }

                    break;

                case 2:
                    _floatParametersContainer.RemoveAt(index);

                    for (int i = index; i < _floatParametersContainer.childCount - 1; i++)
                    {
                        int newIndex = i;
                        Button button = SM_Button(null, () => _removeParameter(2, newIndex));
                        button.style.backgroundColor = Color.red;

                        _floatParametersContainer.ElementAt(i).RemoveAt(0);
                        _floatParametersContainer.ElementAt(i).Insert(0, button);
                    }

                    break;

                case 3:
                    _boolParametersContainer.RemoveAt(index);

                    for (int i = index; i < _boolParametersContainer.childCount - 1; i++)
                    {
                        int newIndex = i;
                        Button button = SM_Button(null, () => _removeParameter(3, newIndex));
                        button.style.backgroundColor = Color.red;

                        _boolParametersContainer.ElementAt(i).RemoveAt(0);
                        _boolParametersContainer.ElementAt(i).Insert(0, button);
                    }

                    break;
            }
        }

        private void _loadAsset()
        {
            if (CurrentAsset != null)
            {
                _createTableContainer();
                _createParametersContainer();
            }

            _setGraph(_currentGraphViewIndex);
        }

        private void _saveAsset()
        {
            List<SequenceManagerGraphViewData> data = CurrentAsset.GraphViewDataList;
            CurrentAsset.GraphViewDataList = new List<SequenceManagerGraphViewData>();

            List<bool> state = CurrentAsset.GraphViewStateList;
            CurrentAsset.GraphViewStateList = new List<bool>();

            List<string> names = new List<string>();
            for (int i = 0; i < _graphViews.Count; i++)
            {
                if (_isChanged[i])
                {
                    CurrentAsset.GraphViewDataList.Add(_graphViews[i].SaveGraph());

                    if (String.IsNullOrEmpty(((TextField)_sequenceTablesContainer.ElementAt(i).ElementAt(2)).value))
                    {
                        ((TextField)_sequenceTablesContainer.ElementAt(i).ElementAt(2)).value = "null";
                    }
                }
                else
                {
                    CurrentAsset.GraphViewDataList.Add(data[i]);
                }

                string name = ((TextField)_sequenceTablesContainer.ElementAt(i).ElementAt(2)).value;

                if (names.Contains(name))
                {
                    int repeatCount = 2;

                    while (names.Contains(name + repeatCount))
                    {
                        repeatCount++;
                    }

                    name += repeatCount.ToString();
                }

                names.Add(name);

                CurrentAsset.GraphViewDataList[i].GraphName = name;
                CurrentAsset.GraphViewStateList.Add(((Toggle)_sequenceTablesContainer.ElementAt(i).ElementAt(3))
                    .value);
            }

            CurrentAsset.StringParameterNames = new List<string>();
            CurrentAsset.StringParameterValues = new List<string>();

            for (int i = 0; i < _stringParametersContainer.childCount - 1; i++)
            {
                if (((TextField)_stringParametersContainer.ElementAt(i).ElementAt(1)).value == null)
                {
                    ((TextField)_stringParametersContainer.ElementAt(i).ElementAt(1)).value = "null";
                }

                string name = ((TextField)_stringParametersContainer.ElementAt(i).ElementAt(1)).value;

                if (CurrentAsset.StringParameterNames.Contains(name))
                {
                    int repeatCount = 2;

                    while (CurrentAsset.StringParameterNames.Contains(name + repeatCount))
                    {
                        repeatCount++;
                    }

                    name += repeatCount.ToString();
                }

                CurrentAsset.StringParameterNames.Add(name);
                CurrentAsset.StringParameterValues.Add(
                    ((TextField)_stringParametersContainer.ElementAt(i).ElementAt(2)).value);
            }

            CurrentAsset.FloatParameterNames = new List<string>();
            CurrentAsset.FloatParameterValues = new List<float>();

            for (int i = 0; i < _floatParametersContainer.childCount - 1; i++)
            {
                if (((TextField)_floatParametersContainer.ElementAt(i).ElementAt(1)).value == null)
                {
                    ((TextField)_floatParametersContainer.ElementAt(i).ElementAt(1)).value = "null";
                }

                string name = ((TextField)_floatParametersContainer.ElementAt(i).ElementAt(1)).value;

                if (CurrentAsset.FloatParameterNames.Contains(name))
                {
                    int repeatCount = 2;

                    while (CurrentAsset.FloatParameterNames.Contains(name + repeatCount))
                    {
                        repeatCount++;
                    }

                    name += repeatCount.ToString();
                }

                CurrentAsset.FloatParameterNames.Add(name);
                CurrentAsset.FloatParameterValues.Add(((FloatField)_floatParametersContainer.ElementAt(i).ElementAt(2))
                    .value);
            }

            CurrentAsset.IntParameterNames = new List<string>();
            CurrentAsset.IntParameterValues = new List<int>();

            for (int i = 0; i < _intParametersContainer.childCount - 1; i++)
            {
                if (((TextField)_intParametersContainer.ElementAt(i).ElementAt(1)).value == null)
                {
                    ((TextField)_intParametersContainer.ElementAt(i).ElementAt(1)).value = "null";
                }

                string name = ((TextField)_intParametersContainer.ElementAt(i).ElementAt(1)).value;

                if (CurrentAsset.IntParameterNames.Contains(name))
                {
                    int repeatCount = 2;

                    while (CurrentAsset.IntParameterNames.Contains(name + repeatCount))
                    {
                        repeatCount++;
                    }

                    name += repeatCount.ToString();
                }

                CurrentAsset.IntParameterNames.Add(name);
                CurrentAsset.IntParameterValues.Add(((IntegerField)_intParametersContainer.ElementAt(i).ElementAt(2))
                    .value);
            }

            CurrentAsset.BoolParameterNames = new List<string>();
            CurrentAsset.BoolParameterValues = new List<bool>();

            for (int i = 0; i < _boolParametersContainer.childCount - 1; i++)
            {
                if (((TextField)_boolParametersContainer.ElementAt(i).ElementAt(1)).value == null)
                {
                    ((TextField)_boolParametersContainer.ElementAt(i).ElementAt(1)).value = "null";
                }

                string name = ((TextField)_boolParametersContainer.ElementAt(i).ElementAt(1)).value;

                if (CurrentAsset.BoolParameterNames.Contains(name))
                {
                    int repeatCount = 2;

                    while (CurrentAsset.BoolParameterNames.Contains(name + repeatCount))
                    {
                        repeatCount++;
                    }

                    name += repeatCount.ToString();
                }

                CurrentAsset.BoolParameterNames.Add(name);
                CurrentAsset.BoolParameterValues.Add(
                    ((Toggle)_boolParametersContainer.ElementAt(i).ElementAt(2)).value);
            }

            EditorUtility.SetDirty(CurrentAsset);
            AssetDatabase.Refresh();

            _loadAsset();
        }

        private void _setSequenceManager()
        {
            if (CurrentAsset != null)
            {
                _saveAsset();
                //=
                List<SequenceTableData> sequenceTableData = new List<SequenceTableData>();
                for (int i = 0; i < _graphViews.Count; i++)
                {
                    sequenceTableData.Add(
                        _graphViews[i].CreateSequenceTable(CurrentAsset.GraphViewDataList[i].GraphName, CurrentAsset.GraphViewStateList[i]));
                }

                SequenceManagerData sequenceManagerData = CreateInstance<SequenceManagerData>();

                sequenceManagerData.SequenceTableData = sequenceTableData.ToArray();

                sequenceManagerData.StringParameterNames = CurrentAsset.StringParameterNames.ToArray();
                sequenceManagerData.StringParameterValues = CurrentAsset.StringParameterValues.ToArray();

                sequenceManagerData.IntParameterNames = CurrentAsset.IntParameterNames.ToArray();
                sequenceManagerData.IntParameterValues = CurrentAsset.IntParameterValues.ToArray();

                sequenceManagerData.FloatParameterNames = CurrentAsset.FloatParameterNames.ToArray();
                sequenceManagerData.FloatParameterValues = CurrentAsset.FloatParameterValues.ToArray();

                sequenceManagerData.BoolParameterNames = CurrentAsset.BoolParameterNames.ToArray();
                sequenceManagerData.BoolParameterValues = CurrentAsset.BoolParameterValues.ToArray();

                //==
                if (!AssetDatabase.IsValidFolder("Assets/Resources/SequenceManager"))
                {
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }
                    AssetDatabase.CreateFolder("Assets/Resources", "SequenceManager");
                }

                string[] assets = Directory.GetFiles(Application.dataPath + "/Resources/SequenceManager");
                List<string> validScenes = new List<string>();

                foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                {
                    validScenes.Add(scene.path.Remove(scene.path.IndexOf(".unity"), 6).Remove(0, scene.path.LastIndexOf('/') + 1));
                }

                for (int i = 0; i < assets.Length; i++)
                {
                    assets[i] = assets[i].Remove(assets[i].Length - 6, 6).Remove(0, Application.dataPath.Length + 27);

                    if (!validScenes.Contains(assets[i]))
                    {
                        AssetDatabase.DeleteAsset("Assets/Resources/SequenceManager/" + assets[i] + ".asset");
                    }
                }

                //==
                AssetDatabase.CreateAsset(
                    sequenceManagerData, "Assets/Resources/SequenceManager/" + EditorSceneManager.GetActiveScene().name + ".asset");

                return;
            }

            Debug.LogWarning("No SequenceManager asset found");
        }
    }
}
