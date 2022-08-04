using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.SequenceManager
{
    public class SequenceTable
    {
        internal string TableName;
        //================================================================================================================
        internal Dictionary<string, Sequence> Sequences;
        internal Dictionary<string, Argument> Arguments;
        //================================================================================================================
        internal bool IsTableActive = false;
        internal bool IsTableHalted = true;
        //================================================================================================================
        internal string CurrentArgumentName;
    }
}