using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CM.ApplicationManagement;
using CM.DataManagement;
using PlasticGui.Configuration.CloudEdition;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CM.SequenceManager
{
    public class SequenceManagerSaveData : DataManagerMainObject
    {
        private static bool _isSaved;
        public static bool IsSaved => _isSaved;

        private static SaveObject _thisSaveObject = new SaveObject();
        public static SaveObject ThisSaveObject => _thisSaveObject;

        public static string SaveName => "CM.SequenceManager" + SequenceManager.CurrentSequenceManager.Name;

        private string[] _currentStringParameters;
        private int[] _currentIntParameters;
        private float[] _currentFloatParameters;
        private bool[] _currentBoolParameters;
        private string[] _currentCurrentArguments;
        private bool[] _currentStates;

        protected override void OnSave()
        {
            if (_currentCurrentArguments == null) { _setCurrentData(); }

            ApplicationManager.SetApplicationPackageState("CM.SequenceManager", false);
            try
            {
                string[] stringParameters = new string[SequenceManager.CurrentSequenceManager.StringParameters.Count];
                SequenceManager.CurrentSequenceManager.StringParameters.Values.CopyTo(stringParameters, 0);

                int[] intParameters = new int[SequenceManager.CurrentSequenceManager.IntParameters.Count];
                SequenceManager.CurrentSequenceManager.IntParameters.Values.CopyTo(intParameters, 0);

                float[] floatParameters = new float[SequenceManager.CurrentSequenceManager.FloatParameters.Count];
                SequenceManager.CurrentSequenceManager.FloatParameters.Values.CopyTo(floatParameters, 0);

                bool[] boolParameters = new bool[SequenceManager.CurrentSequenceManager.BoolParameters.Count];
                SequenceManager.CurrentSequenceManager.BoolParameters.Values.CopyTo(boolParameters, 0);

                string[] currentArguments = new string[SequenceManager.CurrentSequenceManager.SequenceTables.Count];
                bool[] states = new bool[SequenceManager.CurrentSequenceManager.SequenceTables.Count];
                for (int i = 0; i < currentArguments.Length; i++)
                {
                    currentArguments[i] = SequenceManager.CurrentSequenceManager.SequenceTables.ElementAt(i).Value
                        .CurrentArgumentName;
                    states[i] = SequenceManager.CurrentSequenceManager.SequenceTables.ElementAt(i).Value.IsTableActive;
                }

                _thisSaveObject.AddArray(new object[6] { stringParameters, intParameters, floatParameters, boolParameters, currentArguments, states });

                if (WriteGameData.SaveObject(ThisSaveObject, SaveName, CreateBinaryFormatter()))
                {
                    _setCurrentData();
                    _isSaved = true;
                    ApplicationManager.SetApplicationPackageState("CM.SequenceManager", true);
                    return;
                }
            }
            catch (Exception message)
            {
                Debug.LogWarning("SequenceManager : " + "Failed to save data" + Environment.NewLine + message);

                throw;
            }

            _isSaved = false;

            ApplicationManager.SetApplicationPackageState("CM.SequenceManager", true);
        }

        protected override void OnLoad()
        {
            ApplicationManager.SetApplicationPackageState("CM.SequenceManager", false);
            string[] keys;
            if (_currentCurrentArguments == null) { _setCurrentData(); }

            if (!IsSaved)
            {
                try
                {
                    object saveObject = new object();
                    if (ReadGameData.LoadObject(ref saveObject, SaveName, CreateBinaryFormatter()))
                    {
                        _thisSaveObject = (SaveObject)saveObject;

                        string[] stringParameters = (string[])_thisSaveObject.GetArray()[0];

                        int[] intParameters = (int[])_thisSaveObject.GetArray()[1];

                        float[] floatParameters = (float[])_thisSaveObject.GetArray()[2];

                        bool[] boolParameters = (bool[])_thisSaveObject.GetArray()[3];

                        string[] currentArguments = (string[])_thisSaveObject.GetArray()[4];

                        bool[] states = (bool[])_thisSaveObject.GetArray()[5];

                        keys = new string[SequenceManager.CurrentSequenceManager.StringParameters.Count];
                        SequenceManager.CurrentSequenceManager.StringParameters.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            SequenceManager.CurrentSequenceManager.StringParameters[keys[i]] = stringParameters[i];
                        }

                        keys = new string[SequenceManager.CurrentSequenceManager.IntParameters.Count];
                        SequenceManager.CurrentSequenceManager.IntParameters.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            SequenceManager.CurrentSequenceManager.IntParameters[keys[i]] = intParameters[i];
                        }

                        keys = new string[SequenceManager.CurrentSequenceManager.FloatParameters.Count];
                        SequenceManager.CurrentSequenceManager.FloatParameters.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            SequenceManager.CurrentSequenceManager.FloatParameters[keys[i]] = floatParameters[i];
                        }

                        keys = new string[SequenceManager.CurrentSequenceManager.BoolParameters.Count];
                        SequenceManager.CurrentSequenceManager.BoolParameters.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            SequenceManager.CurrentSequenceManager.BoolParameters[keys[i]] = boolParameters[i];
                        }

                        keys = new string[SequenceManager.CurrentSequenceManager.SequenceTables.Count];
                        SequenceManager.CurrentSequenceManager.SequenceTables.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            SequenceManager.CurrentSequenceManager.SequenceTables[keys[i]].CurrentArgumentName = currentArguments[i];
                            SequenceManager.CurrentSequenceManager.SequenceTables[keys[i]].IsTableActive = states[i];
                        }

                        _setCurrentData();
                        _isSaved = true;
                        ApplicationManager.SetApplicationPackageState("CM.SequenceManager", true);
                        return;
                    }
                }
                catch (Exception message)
                {
                    Debug.LogWarning("SequenceManager : " + "Failed to load data" + Environment.NewLine + message);
                    throw;
                }
            }
            _isSaved = false;

            keys = new string[SequenceManager.CurrentSequenceManager.StringParameters.Count];
            SequenceManager.CurrentSequenceManager.StringParameters.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                SequenceManager.CurrentSequenceManager.StringParameters[keys[i]] = _currentStringParameters[i];
            }

            keys = new string[SequenceManager.CurrentSequenceManager.IntParameters.Count];
            SequenceManager.CurrentSequenceManager.IntParameters.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                SequenceManager.CurrentSequenceManager.IntParameters[keys[i]] = _currentIntParameters[i];
            }

            keys = new string[SequenceManager.CurrentSequenceManager.FloatParameters.Count];
            SequenceManager.CurrentSequenceManager.FloatParameters.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                SequenceManager.CurrentSequenceManager.FloatParameters[keys[i]] = _currentFloatParameters[i];
            }

            keys = new string[SequenceManager.CurrentSequenceManager.BoolParameters.Count];
            SequenceManager.CurrentSequenceManager.BoolParameters.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                SequenceManager.CurrentSequenceManager.BoolParameters[keys[i]] = _currentBoolParameters[i];
            }

            keys = new string[SequenceManager.CurrentSequenceManager.SequenceTables.Count];
            SequenceManager.CurrentSequenceManager.SequenceTables.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                SequenceManager.CurrentSequenceManager.SequenceTables[keys[i]].CurrentArgumentName = _currentCurrentArguments[i];
                SequenceManager.CurrentSequenceManager.SequenceTables[keys[i]].IsTableActive = _currentStates[i];
            }

            ApplicationManager.SetApplicationPackageState("CM.SequenceManager", true);
        }

        private void _setCurrentData()
        {
            _currentStringParameters = new string[SequenceManager.CurrentSequenceManager.StringParameters.Count];
            SequenceManager.CurrentSequenceManager.StringParameters.Values.CopyTo(_currentStringParameters, 0);

            _currentIntParameters = new int[SequenceManager.CurrentSequenceManager.IntParameters.Count];
            SequenceManager.CurrentSequenceManager.IntParameters.Values.CopyTo(_currentIntParameters, 0);

            _currentFloatParameters = new float[SequenceManager.CurrentSequenceManager.FloatParameters.Count];
            SequenceManager.CurrentSequenceManager.FloatParameters.Values.CopyTo(_currentFloatParameters, 0);

            _currentBoolParameters = new bool[SequenceManager.CurrentSequenceManager.BoolParameters.Count];
            SequenceManager.CurrentSequenceManager.BoolParameters.Values.CopyTo(_currentBoolParameters, 0);

            _currentCurrentArguments = new string[SequenceManager.CurrentSequenceManager.SequenceTables.Count];
            _currentStates = new bool[SequenceManager.CurrentSequenceManager.SequenceTables.Count];
            for (int i = 0; i < _currentCurrentArguments.Length; i++)
            {
                _currentCurrentArguments[i] = SequenceManager.CurrentSequenceManager.SequenceTables.ElementAt(i).Value
                    .CurrentArgumentName;
                _currentStates[i] = SequenceManager.CurrentSequenceManager.SequenceTables.ElementAt(i).Value.IsTableActive;
            }
        }

        public BinaryFormatter CreateBinaryFormatter()
        {
            return new BinaryFormatter();
        }
    }
}
