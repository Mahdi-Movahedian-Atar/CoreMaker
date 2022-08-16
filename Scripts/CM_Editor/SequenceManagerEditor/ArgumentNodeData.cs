using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Editor.SequenceManagerEditor
{
    [Serializable]
    public class ArgumentNodeData
    {
        public string TypeName;
        public string NodeName = "ArgumentName";
        public int NodeIndex;
        public int GroupIndex = -1;

        public Vector2 Vector;
        public List<int> Ports = new List<int>();

        public List<string> StringArray = new List<string>();
        public List<int> IntArray = new List<int>();
        public List<float> FloatArray = new List<float>();
        public List<bool> BoolArray = new List<bool>();
    }
}
