using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.SequenceManager
{
    [Serializable]
    public class SequenceTableData
    {
        public string TableName;
        public SequenceManagerElementData[] SequenceData;
        public SequenceManagerElementData[] ArgumentData;
        public bool IsTableActive;
        public string CurrentArgumentName;
    }
}