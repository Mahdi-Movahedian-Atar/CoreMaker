using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.SequenceManager;
using Codice.Client.Common.TreeGrouper;
using UnityEngine;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace CM.Editor.SequenceManagerEditor
{
    public abstract class ArgumentParentNode : Node
    {
        public ArgumentNodeData NodeData;

        public abstract void CreateNode(ArgumentNodeData argumentNodeData);
        public abstract void SetNode(SequenceManagerGraphView sequenceManagerGraphView);
        public abstract ArgumentNodeData SaveNode();
        public abstract SequenceManagerElementData CreateArgument();
    }
}
