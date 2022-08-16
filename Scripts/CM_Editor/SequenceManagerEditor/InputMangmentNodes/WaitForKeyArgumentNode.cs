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
    [Argument("WaitForKeyArgumentNode", "Input Management")]
    public class WaitForKeyArgumentNode : ArgumentParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private TextField _table = SM_TextField(null);
        private Toggle _parameterTable;

        private TextField _keyCodeName = SM_TextField(null);
        private Toggle _parameterKeyCodeName = SM_Toggle(false);

        public override void CreateNode(ArgumentNodeData argumentNodeData)
        {
            if (argumentNodeData.StringArray == null || argumentNodeData.StringArray.Count != 2)
                argumentNodeData.StringArray = new List<string>() { "null", "null" };

            if (argumentNodeData.BoolArray == null || argumentNodeData.BoolArray.Count != 2)
                argumentNodeData.BoolArray = new List<bool>() { false, false};

            NodeData = argumentNodeData;
            //-----------------------------------------------------------------------------------------------------------
            _inputPort = SM_Port(this, "Input", PortType.ArgumentInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.ArgumentOutput);
            outputContainer.Add(_outputPort);
            //-----------------------------------------------------------------------------------------------------------
            _table = SM_TextField(argumentNodeData.StringArray[0]);

            _parameterTable = SM_Toggle(argumentNodeData.BoolArray[0]);

            _parameterTable.text = "Is it a parameter";
            //-----------------------------------------------------------------------------------------------------------
            _keyCodeName = SM_TextField(argumentNodeData.StringArray[1]);

            _parameterKeyCodeName = SM_Toggle(argumentNodeData.BoolArray[1]);

            _parameterTable.text = "Is it a parameter";
            //-----------------------------------------------------------------------------------------------------------
            extensionContainer.Add(SM_Label("Table"));
            extensionContainer.Add(_table);
            extensionContainer.Add(_parameterTable);
            extensionContainer.Add(SM_Label("Key"));
            extensionContainer.Add(_keyCodeName);
            extensionContainer.Add(_parameterKeyCodeName);
            
            RefreshExpandedState();

            argumentNodeData.TypeName = "WaitForKeyArgumentNode";
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
            NodeData.StringArray = new List<string> { _table.value , _keyCodeName.value };

            NodeData.BoolArray = new List<bool> { _parameterTable.value , _parameterKeyCodeName.value };

            return NodeData;
        }

        public override SequenceManagerElementData CreateArgument()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(WaitForKeyArgument).ToString(),
                Bools = new[] { _parameterTable.value , _parameterKeyCodeName.value },
                Strings = new[] { _table.value , _keyCodeName.value }
            };

            return sequenceManagerElementData;
        }
    }
}
