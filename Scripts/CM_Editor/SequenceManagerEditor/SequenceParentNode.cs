using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.SequenceManager;
using Codice.Client.Common.TreeGrouper;
using UnityEngine;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace CM.Editor.SequenceManagerEditor
{
    public abstract class SequenceParentNode : Node
    {
        public SequenceNodeData NodeData;

        public abstract void CreateNode(SequenceNodeData sequenceNodeData);
        public abstract void SetNode(SequenceManagerGraphView sequenceManagerGraphView);
        public abstract SequenceNodeData SaveNode();
        public abstract SequenceManagerElementData CreateSequence();
    }
}
