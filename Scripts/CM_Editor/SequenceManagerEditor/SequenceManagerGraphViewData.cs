using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Editor.SequenceManagerEditor
{
    [Serializable]
    public class SequenceManagerGraphViewData
    {
        public string GraphName;

        public List<string> Groups = new List<string>();
        public List<Vector2> GroupVector = new List<Vector2>();

        public List<SequenceNodeData> SequenceNodeDatas = new List<SequenceNodeData>();
        public List<ArgumentNodeData> ArgumentNodeDatas = new List<ArgumentNodeData>();
    }
}
