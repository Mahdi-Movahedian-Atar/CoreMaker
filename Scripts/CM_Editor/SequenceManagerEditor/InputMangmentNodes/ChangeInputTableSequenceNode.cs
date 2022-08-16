using System;
using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.InputManagement;
using CM.SequenceManager;
using Codice.Client.BaseCommands;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("ChangeInputTableSequenceNode", "Input Management")]
    public class ChangeInputTableSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private TextField _table = SM_TextField(null);
        private Toggle _isItParameter = SM_Toggle(false);

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);

            if (sequenceNodeData.StringArray.Count != 0) { _table.value = sequenceNodeData.StringArray[0]; }

            if (sequenceNodeData.BoolArray.Count != 0) { _isItParameter = SM_Toggle(sequenceNodeData.BoolArray[0]); }

            _isItParameter.text = "Is it a parameter";

            extensionContainer.Add(_table);
            extensionContainer.Add(_isItParameter);

            RefreshExpandedState();

            NodeData = sequenceNodeData;
            sequenceNodeData.TypeName = "ChangeInputTableSequenceNode";
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
            NodeData.StringArray = new List<string> { _table.value };

            NodeData.BoolArray = new List<bool> { _isItParameter.value };

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(ChangeInputTableSequence).ToString(),
                Bools = new[] { _isItParameter.value },
                Strings = new[] { _table.value }
            };

            return sequenceManagerElementData;
        }
    }
}
