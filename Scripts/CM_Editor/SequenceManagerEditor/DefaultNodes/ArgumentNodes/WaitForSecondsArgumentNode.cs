using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.SequenceManager;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Argument("WaitForSecondsArgumentNode", "Default")]
    public class WaitForSecondsArgumentNode : ArgumentParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private VisualElement _element = new VisualElement();
        private Toggle _isAParameter;

        public override void CreateNode(ArgumentNodeData argumentNodeData)
        {
            if (argumentNodeData.StringArray == null || argumentNodeData.StringArray.Count != 1)
                argumentNodeData.StringArray = new List<string>() { "null" };

            if (argumentNodeData.BoolArray == null || argumentNodeData.BoolArray.Count != 1)
                argumentNodeData.BoolArray = new List<bool>() { false };

            if (argumentNodeData.IntArray == null || argumentNodeData.IntArray.Count != 1)
                argumentNodeData.IntArray = new List<int>() { 1 };

            NodeData = argumentNodeData;

            _inputPort = SM_Port(this, "Input", PortType.ArgumentInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.ArgumentOutput);
            outputContainer.Add(_outputPort);

            _isAParameter = SM_Toggle(argumentNodeData.BoolArray[0]);
            _isAParameter.text = "Is it a parameter";
            _isAParameter.RegisterValueChangedCallback(target => { _setElement(); });
            _setElement();

            extensionContainer.Add(SM_Label(null));
            extensionContainer.Add(_element);
            extensionContainer.Add(_isAParameter);

            RefreshExpandedState();

            NodeData = argumentNodeData;
            NodeData.TypeName = "WaitForSecondsArgumentNode";
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

            if (!NodeData.BoolArray[0]) NodeData.IntArray = new List<int> { ((IntegerField)_element[0]).value };
            else NodeData.StringArray = new List<string> { ((TextField)_element[0]).value };

            return NodeData;
        }

        public override SequenceManagerElementData CreateArgument()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData { Type = typeof(WaitForSecondsArgument).ToString() };

            if (NodeData.BoolArray[0]) sequenceManagerElementData.Strings = NodeData.StringArray.ToArray();
            else sequenceManagerElementData.Ints = NodeData.IntArray.ToArray();

            return sequenceManagerElementData;
        }

        private void _setElement()
        {
            if (_isAParameter.value)
            {
                _element.Clear();
                _element.Add(SM_TextField(NodeData.StringArray[0]));
                NodeData.BoolArray[0] = true;
            }
            else
            {
                _element.Clear();
                _element.Add(SM_IntField(NodeData.IntArray[0]));
                NodeData.BoolArray[0] = false;
            }
        }
    }
}
