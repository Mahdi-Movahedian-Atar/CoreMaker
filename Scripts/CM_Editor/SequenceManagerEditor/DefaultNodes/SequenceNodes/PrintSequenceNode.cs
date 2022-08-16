using System;
using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.SequenceManager;
using Codice.Client.BaseCommands;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("PrintSequenceNode", "Default")]
    public class PrintSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private TextField _print = SM_TextField(null);
        private Toggle _isItParameter = SM_Toggle(false);

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);

            if (sequenceNodeData.StringArray.Count != 0) { _print.value = sequenceNodeData.StringArray[0]; }

            if (sequenceNodeData.BoolArray.Count != 0) { _isItParameter = SM_Toggle(sequenceNodeData.BoolArray[0]); }

            _isItParameter.text = "Is it a parameter";

            extensionContainer.Add(_print);
            extensionContainer.Add(_isItParameter);

            RefreshExpandedState();

            NodeData = sequenceNodeData;
            sequenceNodeData.TypeName = "PrintSequenceNode";
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
            NodeData.StringArray = new List<string> { _print.value };

            NodeData.BoolArray = new List<bool> { _isItParameter.value };

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(PrintSequence).ToString(),
                Bools = new[] { _isItParameter.value },
                Strings = new[] { _print.value }
            };

            return sequenceManagerElementData;
        }
    }
}
