using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Editor.SequenceManagerEditor
{
    [CreateAssetMenu , Serializable]
    public class SequenceManagerAsset : ScriptableObject
    {
        public List<SequenceManagerGraphViewData> GraphViewDataList;
        public List<bool> GraphViewStateList;

        public List<string> StringParameterNames;
        public List<string> StringParameterValues;

        public List<string> IntParameterNames;
        public List<int> IntParameterValues;

        public List<string> FloatParameterNames;
        public List<float> FloatParameterValues;

        public List<string> BoolParameterNames;
        public List<bool> BoolParameterValues;
    }
}
