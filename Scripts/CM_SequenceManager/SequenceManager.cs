using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CM.SequenceManager
{
    public class SequenceManager : MonoBehaviour
    {
        #region Singleton
        private static SequenceManager _currentSequenceManager;
        public static SequenceManager CurrentSequenceManager
        {
            get
            {
                return _currentSequenceManager;
            }
            set
            {
                if (value != null)
                {
                    _currentSequenceManager = value;
                }
            }
        }
        #endregion

        //================================================================================================================
        public string Name;

        public Dictionary<string, SequenceTable> SequenceTables;
        //----------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> StringParameters;
        public Dictionary<string, float> FloatParameters;
        public Dictionary<string, int> IntParameters;
        public Dictionary<string, bool> BoolParameters;

        //================================================================================================================

        void Awake()
        {
            _currentSequenceManager = FindObjectOfType<SequenceManager>();
        }

        //================================================================================================================

        public static void UpdateTable(string tableName, string targetName)
        {
            CurrentSequenceManager._updateTable(tableName, targetName);
        }
        private void _updateTable(string tableName, string targetName)
        {
            if (!SequenceTables[tableName].IsTableHalted)
            {
                _haltCurrentArgument(tableName);

                if (!String.IsNullOrEmpty(targetName))
                {
                    StartCoroutine(SequenceTables[tableName].Sequences[targetName].Invoke());

                    SequenceTables[tableName].CurrentArgumentName =
                        SequenceTables[tableName].Sequences[targetName].TargetArgumentName;

                    _startCurrentArgument(tableName);
                    return;
                }

                SequenceTables[tableName].IsTableActive = false;
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public static void SetArgument(string tableName, string targetName)
        {
            CurrentSequenceManager._SetArgument(tableName, targetName);
        }
        private void _SetArgument(string tableName, string targetName)
        {
            if (String.IsNullOrEmpty(tableName) || !SequenceTables.ContainsKey(tableName))
            {
                Debug.LogWarning("SequenceManager : Invalid table - " + tableName);
                return;
            }
            if (!String.IsNullOrEmpty(targetName) && !SequenceTables[tableName].Arguments.ContainsKey(targetName))
            {
                Debug.LogWarning("SequenceManager : Invalid argument - " + targetName);
                return;
            }

            SequenceTables[tableName].Arguments[SequenceTables[tableName].CurrentArgumentName].ExitArgument();

            if (SequenceTables[tableName].IsTableHalted)
            {
                if (!String.IsNullOrEmpty(targetName))
                {
                    SequenceTables[tableName].CurrentArgumentName = targetName;
                    _haltCurrentArgument(tableName);

                    return;
                }
            }
            else
            {
                _haltCurrentArgument(tableName);

                if (!String.IsNullOrEmpty(targetName))
                {
                    SequenceTables[tableName].CurrentArgumentName = targetName;
                    _startCurrentArgument(tableName);

                    return;
                }

                SequenceTables[tableName].IsTableActive = false;
            }

            _haltCurrentArgument(tableName);

            if (!String.IsNullOrEmpty(targetName))
            {

                StartCoroutine(SequenceTables[tableName].Sequences[targetName].Invoke());

                SequenceTables[tableName].CurrentArgumentName =
                    SequenceTables[tableName].Sequences[targetName].TargetArgumentName;

                _startCurrentArgument(tableName);
                return;
            }

            SequenceTables[tableName].IsTableActive = false;

            StartCoroutine(SequenceTables[tableName].Sequences[targetName].Invoke());
        }
        //----------------------------------------------------------------------------------------------------------------
        public static void StartCurrentArgument(string tableName)
        {
            CurrentSequenceManager._startCurrentArgument(tableName);
        }
        private void _startCurrentArgument(string tableName)
        {
            if (SequenceTables[tableName].IsTableActive)
            {
                if (!String.IsNullOrEmpty(SequenceTables[tableName].CurrentArgumentName))
                {
                    SequenceTables[tableName].IsTableHalted = false;
                    SequenceTables[tableName].Arguments[SequenceTables[tableName].CurrentArgumentName].EnterArgument();
                    StartCoroutine(SequenceTables[tableName].Arguments[SequenceTables[tableName].CurrentArgumentName].Invoke());
                    return;
                }
                SequenceTables[tableName].IsTableActive = false;
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public static void HaltCurrentArgument(string tableName)
        {
            CurrentSequenceManager._haltCurrentArgument(tableName);
        }
        private void _haltCurrentArgument(string tableName)
        {
            string currentArgument = SequenceTables[tableName].CurrentArgumentName;

            SequenceTables[tableName].IsTableHalted = true;
            if (!string.IsNullOrEmpty(currentArgument))
            {
                SequenceTables[tableName].Arguments[currentArgument].ExitArgument();
                StopCoroutine(SequenceTables[tableName].Arguments[currentArgument].Invoke());
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public static void StartAllCurrentArguments()
        {
            CurrentSequenceManager._startAllCurrentArguments();
        }
        private void _startAllCurrentArguments()
        {
            foreach (var sequenceTable in SequenceTables)
            {
                if (sequenceTable.Value.IsTableActive)
                {
                    sequenceTable.Value.IsTableHalted = false;
                    sequenceTable.Value.Arguments[sequenceTable.Value.CurrentArgumentName].EnterArgument();
                    StartCoroutine(sequenceTable.Value.Arguments[sequenceTable.Value.CurrentArgumentName].Invoke());
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public static void HaltAllCurrentArguments()
        {
            CurrentSequenceManager._haltAllCurrentArguments();
        }
        private void _haltAllCurrentArguments()
        {
            foreach (var sequenceTable in SequenceTables)
            {
                if (sequenceTable.Value.IsTableActive)
                {
                    sequenceTable.Value.IsTableHalted = true;
                    sequenceTable.Value.Arguments[sequenceTable.Value.CurrentArgumentName].ExitArgument();
                    StopCoroutine(sequenceTable.Value.Arguments[sequenceTable.Value.CurrentArgumentName].Invoke());
                }
            }
        }
    }
}