using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CM.ApplicationManagement;
using CM.SequenceManager;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Argument("ForwardArgumentNode", "Default")]
    public class ForwardArgumentNode : ArgumentParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        public override void CreateNode(ArgumentNodeData argumentNodeData)
        {
            _inputPort = SM_Port(this, "Input", PortType.ArgumentInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.ArgumentOutput);
            outputContainer.Add(_outputPort);

            RefreshExpandedState();

            NodeData = argumentNodeData;
            NodeData.TypeName = "ForwardArgumentNode";
        }

        public override void SetNode(SequenceManagerGraphView sequenceManagerGraphView)
        {
            if (NodeData.Ports.Count != 0 && NodeData.Ports[0] != -1)
            {
                sequenceManagerGraphView.Add(_outputPort.ConnectTo((Port)sequenceManagerGraphView.SequenceParentNodes[NodeData.Ports[0]]
                    .inputContainer[0]));
            }
        }

        public override ArgumentNodeData SaveNode()
        {
            NodeData.Vector = GetPosition().position;

            return NodeData;
        }

        public override SequenceManagerElementData CreateArgument()
        {
            return new SequenceManagerElementData { Type = typeof(ForwardArgument).ToString() };
        }
    }
}
