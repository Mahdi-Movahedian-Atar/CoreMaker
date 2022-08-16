using System.Collections;
using System.Collections.Generic;
using CM.SequenceManager;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("ChangeParameterNode", "Default")]
    public class ChangeParameterNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private TextField _parameter;

        private VisualElement _element = new VisualElement();
        private Toggle _isAParameter;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            if (sequenceNodeData.IntArray == null || sequenceNodeData.IntArray.Count != 2)
            { sequenceNodeData.IntArray = new List<int>() { 0, 0 }; }

            if (sequenceNodeData.FloatArray == null || sequenceNodeData.FloatArray.Count != 1)
            { sequenceNodeData.FloatArray = new List<float>() { 0 }; }

            if (sequenceNodeData.StringArray == null || sequenceNodeData.StringArray.Count != 2)
            { sequenceNodeData.StringArray = new List<string>() { "null", "null" }; }

            if (sequenceNodeData.BoolArray == null || sequenceNodeData.BoolArray.Count != 2)
            { sequenceNodeData.BoolArray = new List<bool>() { false, false }; }

            NodeData = sequenceNodeData;
            NodeData.TypeName = "ChangeParameterNode";
            //------------------------------------------------------------------------------------------------------------------------------------
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);
            //------------------------------------------------------------------------------------------------------------------------------------
            _parameter = SM_TextField(sequenceNodeData.StringArray[0]);

            _isAParameter = SM_Toggle(sequenceNodeData.BoolArray[0]);
            _isAParameter.text = "Is it a parameter";
            _isAParameter.RegisterValueChangedCallback(target => { _set(); });
            _set();
            //------------------------------------------------------------------------------------------------------------------------------------
            extensionContainer
                .Add(SM_DropdownField(new List<string>() { "Int Parameter", "Float Parameter", "String Parameter", "Bool Parameter" }, sequenceNodeData.IntArray[0], (index =>
                {
                    switch (index)
                    {
                        case "Int Parameter":
                            sequenceNodeData.IntArray[0] = 0;
                            break;
                        case "Float Parameter":
                            sequenceNodeData.IntArray[0] = 1;
                            break;
                        case "String Parameter":
                            sequenceNodeData.IntArray[0] = 2;
                            break;
                        case "Bool Parameter":
                            sequenceNodeData.IntArray[0] = 3;
                            break;
                    }

                    _set();
                    return index;
                })));
            extensionContainer.Add(SM_Label("Parameter"));
            extensionContainer.Add(_parameter);
            extensionContainer.Add(SM_Label("Overwrite value"));
            extensionContainer.Add(_element);
            extensionContainer.Add(_isAParameter);

            RefreshExpandedState();
        }

        public override void SetNode(SequenceManagerGraphView sequenceManagerGraphView)
        {
            if (NodeData.Port != -1)
            {
                sequenceManagerGraphView.Add(_outputPort.ConnectTo((Port)sequenceManagerGraphView
                    .ArgumentParentNodes[NodeData.Port]
                    .inputContainer[0]));
            }
        }

        public override SequenceNodeData SaveNode()
        {
            NodeData.Vector = GetPosition().position;
            NodeData.StringArray[0] = _parameter.value;

            switch (NodeData.IntArray[0])
            {
                case 0:
                    if (!NodeData.BoolArray[0]) NodeData.IntArray[1] = ((IntegerField)_element[0]).value;
                    else NodeData.StringArray[1] = ((TextField)_element[0]).value;

                    break;

                case 1:
                    if (!NodeData.BoolArray[0]) NodeData.FloatArray[0] = ((FloatField)_element[0]).value;
                    else NodeData.StringArray[1] = ((TextField)_element[0]).value;

                    break;

                case 2:
                    NodeData.StringArray[1] = ((TextField)_element[0]).value;

                    break;

                case 3:
                    if (!NodeData.BoolArray[0]) NodeData.BoolArray[1] = ((Toggle)_element[0]).value;
                    else NodeData.StringArray[1] = ((TextField)_element[0]).value;

                    break;
            }

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            switch (NodeData.IntArray[0])
            {
                case 0:
                    SequenceManagerElementData changeIntSequence = new SequenceManagerElementData()
                    {
                        Type = typeof(ChangeIntSequence).ToString(),
                        Ints = new[] { NodeData.IntArray[1] },
                        Strings = NodeData.StringArray.ToArray(),
                        Bools = new[] { NodeData.BoolArray[0] }
                    };

                    return changeIntSequence;

                case 1:
                    SequenceManagerElementData changeFloatSequence = new SequenceManagerElementData()
                    {
                        Type = typeof(ChangeFloatSequence).ToString(),
                        Floats = new[] { NodeData.FloatArray[0] },
                        Strings = NodeData.StringArray.ToArray(),
                        Bools = new[] { NodeData.BoolArray[0] }
                    };

                    return changeFloatSequence;

                case 2:
                    SequenceManagerElementData changeStringSequence = new SequenceManagerElementData()
                    {
                        Type = typeof(ChangeStringSequence).ToString(),
                        Strings = NodeData.StringArray.ToArray(),
                        Bools = new[] { NodeData.BoolArray[0] }
                    };

                    return changeStringSequence;

                default:
                    SequenceManagerElementData changeBoolSequence = new SequenceManagerElementData()
                    {
                        Type = typeof(ChangeBoolSequence).ToString(),
                        Strings = NodeData.StringArray.ToArray(),
                        Bools = NodeData.BoolArray.ToArray()
                    };

                    return changeBoolSequence;
            }
        }

        private void _set()
        {
            if (_isAParameter.value)
            {
                _element.Clear();
                _element.Add(SM_TextField(NodeData.StringArray[1]));
                NodeData.BoolArray[0] = true;
            }
            else
            {
                _element.Clear();
                switch (NodeData.IntArray[0])
                {
                    case 0:
                        _element.Add(SM_IntField(NodeData.IntArray[1]));
                        break;
                    case 1:
                        _element.Add(SM_FloatField(NodeData.FloatArray[0]));
                        break;
                    case 2:
                        _element.Add(SM_TextField(NodeData.StringArray[1]));
                        break;
                    case 3:
                        _element.Add(SM_Toggle(NodeData.BoolArray[1]));
                        break;
                }
                NodeData.BoolArray[0] = false;
            }
        }
    }
}