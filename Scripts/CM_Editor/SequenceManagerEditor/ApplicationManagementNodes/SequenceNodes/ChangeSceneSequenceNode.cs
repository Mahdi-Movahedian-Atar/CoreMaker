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
    [Sequence("ChangeSceneSequenceNode", "Application Management")]
    public class ChangeSceneSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private VisualElement _sceneIndex = new VisualElement();
        private Toggle _sceneIndexParameter;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            if (sequenceNodeData.StringArray == null || sequenceNodeData.StringArray.Count != 1)
                sequenceNodeData.StringArray = new List<string>() { "null" };

            if (sequenceNodeData.IntArray == null || sequenceNodeData.IntArray.Count != 1)
                sequenceNodeData.IntArray = new List<int>() { 0 };

            if (sequenceNodeData.BoolArray == null || sequenceNodeData.BoolArray.Count != 1)
                sequenceNodeData.BoolArray = new List<bool>() { false };

            NodeData = sequenceNodeData;
            //-----------------------------------------------------------------------------------------------------------
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);
            //-----------------------------------------------------------------------------------------------------------
            _sceneIndexParameter = SM_Toggle(sequenceNodeData.BoolArray[0]);
            _sceneIndexParameter.RegisterValueChangedCallback(targed => { _set(); });
            _set();
            _sceneIndexParameter.text = "Is it a parameter";
            //-----------------------------------------------------------------------------------------------------------
            extensionContainer.Add(_sceneIndex);
            extensionContainer.Add(_sceneIndexParameter);

            RefreshExpandedState();

            sequenceNodeData.TypeName = "ChangeSceneSequenceNode";
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
                NodeData.StringArray = new List<string> { ((TextField)_sceneIndex[0]).value };
            }
            else
            {
                NodeData.IntArray = new List<int> { ((IntegerField)_sceneIndex[0]).value };
            }

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(ChangeSceneSequence).ToString(),
                Bools = NodeData.BoolArray.ToArray(),
                Strings = NodeData.StringArray.ToArray(),
                Ints = NodeData.IntArray.ToArray()
            };

            return sequenceManagerElementData;
        }

        private void _set()
        {
            _sceneIndex.Clear();
            NodeData.BoolArray[0] = _sceneIndexParameter.value;
            if (NodeData.BoolArray[0]) _sceneIndex.Add(SM_TextField(NodeData.StringArray[0]));
            else _sceneIndex.Add(SM_IntField(NodeData.IntArray[0]));
        }
    }
}
