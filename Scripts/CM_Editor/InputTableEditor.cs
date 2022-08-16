using System.Collections;
using System.Collections.Generic;
using CM.InputManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace CM.Editor
{
    [CustomEditor(typeof(InputTable))]
    public class InputTableEditor : UnityEditor.Editor
    {
        private int _currentKeyLength;

        private int _keyLength;
        //----------------------------------------------------------------------------------------------------------------
        private List<bool> _states;

        private List<string> _keyNames = new List<string>();

        private List<KeyMode> _keyModes;

        private List<CMKeyCode> _keyBoardKeyCode;
        private List<CMKeyCode> _xBoxKeyCode;
        private List<CMKeyCode> _psKeyCode;

        private List<UnityEvent> _keyCodeEvents;
        //================================================================================================================
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InputTable inputTable = (InputTable)target;
            //------------------------------------------------------------------------------------------------------------
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("===================================================================================================");

            _keyLength = EditorGUILayout.IntField("length", _keyLength);

            if (GUILayout.Button("Override"))
            {
                _currentKeyLength = _keyLength;
            }

            EditorGUILayout.LabelField(_currentKeyLength.ToString());
            //------------------------------------------------------------------------------------------------------------

            if (_states == null)
            {
                _states = new List<bool>(_currentKeyLength);

                if (inputTable.KeyNames == null)
                {
                    _currentKeyLength = 1;
                    _keyLength = 1;

                    inputTable.KeyNames = new string[0];
                    inputTable.KeyModes = new KeyMode[0];
                    inputTable.KeyBoardKeyCode = new CMKeyCode[0];
                    inputTable.XBoxKeyCode = new CMKeyCode[0];
                    inputTable.PsKeyCode = new CMKeyCode[0];
                    inputTable.KeyCodeEvent = new UnityEvent[0];
                }
                else
                {
                    _currentKeyLength = inputTable.KeyNames.Length;
                    _keyLength = inputTable.KeyNames.Length;
                }
            }
            //------------------------------------------------------------------------------------------------------------
            if (_currentKeyLength != _states.Count)
            {
                for (int i = _states.Count; _currentKeyLength > _states.Count; i++)
                {
                    _states.Add(false);
                }

                for (int i = _states.Count; _currentKeyLength < _states.Count; i--)
                {
                    _states.RemoveAt(i - 1);
                }
            }
            //------------------------------------------------------------------------------------------------------------
            if (_keyNames.Count != _currentKeyLength)
            {
                _keyNames = new List<string>();
                _keyModes = new List<KeyMode>();
                _keyBoardKeyCode = new List<CMKeyCode>();
                _xBoxKeyCode = new List<CMKeyCode>();
                _psKeyCode = new List<CMKeyCode>();
                _keyCodeEvents = new List<UnityEvent>();

                for (int i = 0; i < _currentKeyLength; i++)
                {

                    if (inputTable.KeyNames.Length <= i)
                    {
                        _keyNames.Add("Null");
                        _keyModes.Add(KeyMode.Down);
                        _keyBoardKeyCode.Add(CMKeyCode.Mouse0);
                        _xBoxKeyCode.Add(CMKeyCode.JoystickButton0);
                        _psKeyCode.Add(CMKeyCode.JoystickButton0);
                        _keyCodeEvents.Add(new UnityEvent());
                    }
                    else
                    {
                        _keyNames.Add(inputTable.KeyNames[i]);
                        _keyModes.Add(inputTable.KeyModes[i]);
                        _keyBoardKeyCode.Add(inputTable.KeyBoardKeyCode[i]);
                        _xBoxKeyCode.Add(inputTable.XBoxKeyCode[i]);
                        _psKeyCode.Add(inputTable.PsKeyCode[i]);
                        _keyCodeEvents.Add(inputTable.KeyCodeEvent[i]);
                    }
                }
            }
            //------------------------------------------------------------------------------------------------------------
            if (inputTable.KeyCodeEvent == null)
            {
                inputTable.KeyCodeEvent = new UnityEvent[0];
            }

            for (int i = 0; i < _currentKeyLength; i++)
            {
                _states[i] = EditorGUILayout.Foldout(_states[i], _keyNames[i]);

                if (_states[i])
                {
                    _keyNames[i] = EditorGUILayout.TextField("name", _keyNames[i]);
                    _keyModes[i] = (KeyMode)EditorGUILayout.EnumPopup("KeyMode", _keyModes[i]);
                    _keyBoardKeyCode[i] = (CMKeyCode)EditorGUILayout.EnumPopup("Key Board", _keyBoardKeyCode[i]);
                    _xBoxKeyCode[i] = (CMKeyCode)EditorGUILayout.EnumPopup("XBox", _xBoxKeyCode[i]);
                    _psKeyCode[i] = (CMKeyCode)EditorGUILayout.EnumPopup("PL", _psKeyCode[i]);
                }
            }

            inputTable.KeyNames = _keyNames.ToArray();
            inputTable.KeyModes = _keyModes.ToArray();
            inputTable.KeyBoardKeyCode = _keyBoardKeyCode.ToArray();
            inputTable.XBoxKeyCode = _xBoxKeyCode.ToArray();
            inputTable.PsKeyCode = _psKeyCode.ToArray();
            inputTable.KeyCodeEvent = _keyCodeEvents.ToArray();
            //------------------------------------------------------------------------------------------------------------
            _currentKeyLength = inputTable.KeyNames.Length;

            EditorUtility.SetDirty(inputTable);
        }
    }
}