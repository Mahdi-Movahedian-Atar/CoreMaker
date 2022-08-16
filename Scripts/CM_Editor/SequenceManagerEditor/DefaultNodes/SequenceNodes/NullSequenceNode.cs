using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.SequenceManager;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("NullSequenceNode", "Default")]
    public class NullSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);

            RefreshExpandedState();

            NodeData = sequenceNodeData;
            NodeData.TypeName = "NullSequenceNode";
        }

        public override void SetNode( SequenceManagerGraphView sequenceManagerGraphView)
        {
            if (NodeData.Port != -1)
            {
                sequenceManagerGraphView.Add(_outputPort.ConnectTo((Port)sequenceManagerGraphView.ArgumentParentNodes[NodeData.Port]
                    .inputContainer[0]));
            }
        }

        public override SequenceNodeData SaveNode()
        {
            NodeData.Vector = GetPosition().position;

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            return new SequenceManagerElementData { Type = typeof(NullSequence).ToString() };
        }
    }
}
