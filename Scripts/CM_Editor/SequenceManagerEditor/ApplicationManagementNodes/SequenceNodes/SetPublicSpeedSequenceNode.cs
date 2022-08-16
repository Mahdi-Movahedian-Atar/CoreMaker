using System;
using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.SequenceManager;
using Codice.Client.BaseCommands;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("SetPublicSpeedSequenceNode", "Application Management")]
    public class SetPublicSpeedSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private VisualElement _ChangeValue = new VisualElement();
        private Toggle _isItParameter;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            if (sequenceNodeData.StringArray == null || sequenceNodeData.StringArray.Count != 1)
                sequenceNodeData.StringArray = new List<string>() { "null" };

            if (sequenceNodeData.FloatArray == null || sequenceNodeData.FloatArray.Count != 1)
                sequenceNodeData.FloatArray = new List<float>() { 0 };

            if (sequenceNodeData.BoolArray == null || sequenceNodeData.BoolArray.Count != 1)
                sequenceNodeData.BoolArray = new List<bool>() { false };

            NodeData = sequenceNodeData;
            //-----------------------------------------------------------------------------------------------------------
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);
            //-----------------------------------------------------------------------------------------------------------
            _isItParameter = SM_Toggle(sequenceNodeData.BoolArray[0]);
            _isItParameter.RegisterValueChangedCallback(targed => { _set(); });
            _set();
            _isItParameter.text = "Is it a parameter";
            //-----------------------------------------------------------------------------------------------------------
            extensionContainer.Add(_ChangeValue);
            extensionContainer.Add(_isItParameter);

            RefreshExpandedState();

            sequenceNodeData.TypeName = "SetPublicSpeedSequenceNode";
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
            if (NodeData.BoolArray[0])
            {
                NodeData.StringArray = new List<string> { ((TextField)_ChangeValue[0]).value };
            }
            else
            {
                NodeData.FloatArray = new List<float> { ((FloatField)_ChangeValue[0]).value };
            }

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(SetPublicSpeedSequence).ToString(),
                Bools = NodeData.BoolArray.ToArray(),
                Strings = NodeData.StringArray.ToArray(),
                Floats = NodeData.FloatArray.ToArray()
            };

            return sequenceManagerElementData;
        }

        private void _set()
        {
            _ChangeValue.Clear();
            NodeData.BoolArray[0] = _isItParameter.value;
            if (NodeData.BoolArray[0]) _ChangeValue.Add(SM_TextField(NodeData.StringArray[0]));
            else _ChangeValue.Add(SM_FloatField(NodeData.FloatArray[0]));
        }
    }
}
