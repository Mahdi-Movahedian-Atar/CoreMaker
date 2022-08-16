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
    [Argument("CompareNumbersArgumentNode", "Default")]
    public class CompareNumbersArgumentNode : ArgumentParentNode
    {
        private Port _inputPort;
        private Port _trueOutputPort;
        private Port _falseOutputPort;

        private VisualElement _firstElement = new VisualElement();
        private Toggle _isFirstAParameter;

        private VisualElement _secondElement = new VisualElement();
        private Toggle _isSecondAParameter;

        public override void CreateNode(ArgumentNodeData argumentNodeData)
        {
            if (argumentNodeData.IntArray == null || argumentNodeData.IntArray.Count != 1)
            { argumentNodeData.IntArray = new List<int>() { 0 }; }

            if (argumentNodeData.FloatArray == null || argumentNodeData.FloatArray.Count != 2)
            { argumentNodeData.FloatArray = new List<float>() { 0, 0 }; }

            if (argumentNodeData.StringArray == null || argumentNodeData.StringArray.Count != 2)
            { argumentNodeData.StringArray = new List<string>() { "null", "null" }; }

            if (argumentNodeData.BoolArray == null || argumentNodeData.BoolArray.Count != 2)
            { argumentNodeData.BoolArray = new List<bool>() { false, false }; }

            NodeData = argumentNodeData;
            NodeData.TypeName = "CompareNumbersArgumentNode";
            //------------------------------------------------------------------------------------------------------------------------------------
            _inputPort = SM_Port(this, "Input", PortType.ArgumentInput);
            inputContainer.Add(_inputPort);

            _trueOutputPort = SM_Port(this, "True", PortType.ArgumentOutput);
            _falseOutputPort = SM_Port(this, "False", PortType.ArgumentOutput);
            outputContainer.Add(_trueOutputPort);
            outputContainer.Add(_falseOutputPort);
            //------------------------------------------------------------------------------------------------------------------------------------
            _isFirstAParameter = SM_Toggle(argumentNodeData.BoolArray[0]);
            _isFirstAParameter.text = "Is it a parameter";
            _isFirstAParameter.RegisterValueChangedCallback(target => { _setFirst(); });
            _setFirst();

            _isSecondAParameter = SM_Toggle(argumentNodeData.BoolArray[1]);
            _isSecondAParameter.text = "Is it a parameter";
            _isSecondAParameter.RegisterValueChangedCallback(target => { _setSecond(); });
            _setSecond();
            //------------------------------------------------------------------------------------------------------------------------------------
            extensionContainer.Add(SM_DropdownField(
                new List<string>() { "( = )", "( < )", "( > )", "( <= )", "( >= )" },
                argumentNodeData.IntArray[0],
                (index =>
               {
                   switch (index)
                   {
                       case "( = )":
                           argumentNodeData.IntArray[0] = 0;
                           break;
                       case "( < )":
                           argumentNodeData.IntArray[0] = 1;
                           break;
                       case "( > )":
                           argumentNodeData.IntArray[0] = 2;
                           break;
                       case "( <= )":
                           argumentNodeData.IntArray[0] = 3;
                           break;
                       case "( >= )":
                           argumentNodeData.IntArray[0] = 4;
                           break;
                   }

                   _setFirst();
                   _setSecond();
                   return index;
               })));
            extensionContainer.Add(SM_Label("FirstElement"));
            extensionContainer.Add(_firstElement);
            extensionContainer.Add(_isFirstAParameter);
            extensionContainer.Add(SM_Label("SecondElement"));
            extensionContainer.Add(_secondElement);
            extensionContainer.Add(_isSecondAParameter);

            RefreshExpandedState();
        }

        public override void SetNode(SequenceManagerGraphView sequenceManagerGraphView)
        {
            if (NodeData.Ports.Count != 0)
            {
                if (NodeData.Ports[0] != -1)
                {
                    sequenceManagerGraphView.Add(_trueOutputPort.ConnectTo((Port)sequenceManagerGraphView.SequenceParentNodes[NodeData.Ports[0]]
                        .inputContainer[0]));
                }

                if (NodeData.Ports[1] != -1)
                {
                    sequenceManagerGraphView.Add(_falseOutputPort.ConnectTo((Port)sequenceManagerGraphView.SequenceParentNodes[NodeData.Ports[1]]
                        .inputContainer[0]));
                }
            }
        }

        public override ArgumentNodeData SaveNode()
        {
            NodeData.Vector = GetPosition().position;

            if (NodeData.BoolArray[0]) NodeData.StringArray[0] = ((TextField)_firstElement[0]).value;
            else NodeData.FloatArray[0] = ((FloatField)_firstElement[0]).value;

            if (NodeData.BoolArray[1]) NodeData.StringArray[1] = ((TextField)_secondElement[0]).value;
            else NodeData.FloatArray[1] = ((FloatField)_secondElement[0]).value;

            return NodeData;
        }

        public override SequenceManagerElementData CreateArgument()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(CompareNumbersArgument).ToString(),
                Ints = NodeData.IntArray.ToArray(),
                Floats = NodeData.FloatArray.ToArray(),
                Strings = NodeData.StringArray.ToArray(),
                Bools = NodeData.BoolArray.ToArray()
            };

            return sequenceManagerElementData;
        }

        private void _setFirst()
        {
            if (_isFirstAParameter.value)
            {
                _firstElement.Clear();
                _firstElement.Add(SM_TextField(NodeData.StringArray[0]));
                NodeData.BoolArray[0] = true;
            }
            else
            {
                _firstElement.Clear();
                _firstElement.Add(SM_FloatField(NodeData.FloatArray[0]));
                NodeData.BoolArray[0] = false;
            }
        }
        private void _setSecond()
        {
            if (_isSecondAParameter.value)
            {
                _secondElement.Clear();
                _secondElement.Add(SM_TextField(NodeData.StringArray[1]));
                NodeData.BoolArray[1] = true;
            }
            else
            {
                _secondElement.Clear();
                _secondElement.Add(SM_FloatField(NodeData.FloatArray[1]));
                NodeData.BoolArray[1] = false;
            }
        }
    }
}
