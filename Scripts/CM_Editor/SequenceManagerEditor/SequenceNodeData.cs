using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Editor.SequenceManagerEditor
{
    [Serializable]
    public class SequenceNodeData
    {
        public string TypeName;
        public string NodeName = "SequenceNode";
        public int NodeIndex;
        public int GroupIndex = -1;

        public Vector2 Vector;
        public int Port = -1;

        public List<string> StringArray = new List<string>();
        public List<int> IntArray = new List<int>();
        public List<float> FloatArray = new List<float>();
        public List<bool> BoolArray = new List<bool>();
    }
}
