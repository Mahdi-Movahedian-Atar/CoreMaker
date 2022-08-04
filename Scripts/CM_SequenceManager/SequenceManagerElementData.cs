using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.SequenceManager
{
    [Serializable]
    public class SequenceManagerElementData
    {
        public string Type;
        public string Name;
        public string[] TargetName;

        public string[] Strings;
        public int[] Ints;
        public float[] Floats;
        public bool[] Bools;
    }
}
