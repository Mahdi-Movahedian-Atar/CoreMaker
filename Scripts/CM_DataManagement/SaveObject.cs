using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.DataManagement
{
    [System.Serializable]
    public class SaveObject
    {
        private object[] _saveObjects;

        public void AddArray(object[] objects)
        {
            _saveObjects = objects;

        }

        public object[] GetArray()
        {
            return _saveObjects;
        }
    }
}