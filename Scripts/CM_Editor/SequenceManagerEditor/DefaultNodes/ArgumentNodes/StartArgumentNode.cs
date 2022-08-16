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
    public class StartArgumentNode : ArgumentParentNode
    {
        private Port _outputPort;

        public override void CreateNode(ArgumentNodeData argumentNodeData)
        {
            _outputPort = SM_Port(this, "Output", PortType.ArgumentOutput);
            outputContainer.Add(_outputPort);

            NodeData = argumentNodeData;
            NodeData.TypeName = "StartArgumentNode";
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
