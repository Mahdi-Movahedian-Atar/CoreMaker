using System.Collections;
using System.Collections.Generic;
using CM.SequenceManager;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("SetArgumentSequenceNode", "Default")]
    public class SetArgumentSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private TextField _tableName;
        private TextField _targetArgument;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            if (sequenceNodeData.StringArray == null || sequenceNodeData.StringArray.Count != 2)
                sequenceNodeData.StringArray = new List<string>() { "null", "null" };

            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);

            _tableName = SM_TextField(sequenceNodeData.StringArray[0]);
            _targetArgument = SM_TextField(sequenceNodeData.StringArray[1]);


            extensionContainer.Add(SM_Label("Table Name"));
            extensionContainer.Add(_tableName);
            extensionContainer.Add(SM_Label("Argument Name"));
            extensionContainer.Add(_targetArgument);

            RefreshExpandedState();

            NodeData = sequenceNodeData;
            sequenceNodeData.TypeName = "SetArgumentSequenceNode";
        }

        public override void SetNode(SequenceManagerGraphView sequenceManagerGraphView)
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
            NodeData.StringArray = new List<string> { _tableName.value, _targetArgument.value };

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(SetArgumentSequence).ToString(),
                Strings = NodeData.StringArray.ToArray()
            };

            return sequenceManagerElementData;
        }
    }
}
