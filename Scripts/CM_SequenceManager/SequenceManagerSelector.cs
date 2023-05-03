using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CM.SequenceManager
{
    [Serializable]
    public class SequenceManagerSelector : ScriptableObject
    {
        [SerializeField] private string[] _scenes;
        [SerializeField] private SequenceManagerData[] _sequenceManagers;

        public Dictionary<string, SequenceManagerData> Selector
        {
            get
            {
                Dictionary<string, SequenceManagerData> dictionary = new Dictionary<string, SequenceManagerData>();

                for (int i = 0; i < _scenes.Length; i++)
                {
                    dictionary.Add(_scenes[i], _sequenceManagers[i]);
                }

                return dictionary;
            }
        }

        public void Setup([NotNull] string[] scenes, [NotNull] SequenceManagerData[] sequenceManagers)
        {
            if (scenes.Length != sequenceManagers.Length)
            {
                Debug.LogError("SequenceManager : correct list of scenes or SequenceManagers is not given");
                return;
            }

            _scenes = scenes;
            _sequenceManagers = sequenceManagers;
        }
    }
}