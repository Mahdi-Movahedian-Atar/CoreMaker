using System;
using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using CM.SequenceManager;
using Codice.Client.BaseCommands;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("TrowToBucketSequenceNode", "Application Management")]
    public class TrowToBucketSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private TextField _act;
        private Toggle _isActAParameter;

        private TextField _acted;
        private Toggle _isActedAParameter;

        private TextField _actor;
        private Toggle _isActorAParameter;

        private TextField _massage;
        private Toggle _isMassageAParameter;

        private VisualElement _actCount = new VisualElement();
        private Toggle _isActCountParameter;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            if (sequenceNodeData.StringArray == null || sequenceNodeData.StringArray.Count != 5)
                sequenceNodeData.StringArray = new List<string>() { "", "", "", "", "null" };

            if (sequenceNodeData.IntArray == null || sequenceNodeData.IntArray.Count != 1)
                sequenceNodeData.IntArray = new List<int>() { 1 };

            if (sequenceNodeData.BoolArray == null || sequenceNodeData.BoolArray.Count != 5)
                sequenceNodeData.BoolArray = new List<bool>() { false, false, false, false, false };

            NodeData = sequenceNodeData;
            //-----------------------------------------------------------------------------------------------------------
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);
            //-----------------------------------------------------------------------------------------------------------
            _act = SM_TextField(sequenceNodeData.StringArray[0]);
            _isActAParameter = SM_Toggle(sequenceNodeData.BoolArray[0]);
            _isActAParameter.text = "Is it act parameter";

            Foldout actFoldout = SM_Foldout("Act");
            actFoldout.Add(_act);
            actFoldout.Add(_isActAParameter);
            //-----------------------------------------------------------------------------------------------------------
            _acted = SM_TextField(sequenceNodeData.StringArray[1]);
            _isActedAParameter = SM_Toggle(sequenceNodeData.BoolArray[1]);
            _isActedAParameter.text = "Is it acted parameter";

            Foldout actedFoldout = SM_Foldout("Acted");
            actedFoldout.Add(_acted);
            actedFoldout.Add(_isActedAParameter);
            //-----------------------------------------------------------------------------------------------------------
            _actor = SM_TextField(sequenceNodeData.StringArray[2]);
            _isActorAParameter = SM_Toggle(sequenceNodeData.BoolArray[2]);
            _isActorAParameter.text = "Is it actor parameter";

            Foldout actorFoldout = SM_Foldout("Actor");
            actorFoldout.Add(_actor);
            actorFoldout.Add(_isActorAParameter);
            //-----------------------------------------------------------------------------------------------------------
            _massage = SM_TextField(sequenceNodeData.StringArray[3]);
            _isMassageAParameter = SM_Toggle(sequenceNodeData.BoolArray[3]);
            _isMassageAParameter.text = "Is it massage parameter";

            Foldout massageFoldout = SM_Foldout("Massage");
            massageFoldout.Add(_massage);
            massageFoldout.Add(_isMassageAParameter);
            //-----------------------------------------------------------------------------------------------------------
            _isActCountParameter = SM_Toggle(sequenceNodeData.BoolArray[4]);
            _isActCountParameter.RegisterValueChangedCallback(targed => { _set(); });
            _set();
            _isActCountParameter.text = "Is act count a parameter";

            Foldout actCountFoldout = SM_Foldout("Act Count");
            actCountFoldout.Add(_actCount);
            actCountFoldout.Add(_isActCountParameter);
            //-----------------------------------------------------------------------------------------------------------
            extensionContainer.Add(actFoldout);
            extensionContainer.Add(actedFoldout);
            extensionContainer.Add(actorFoldout);
            extensionContainer.Add(massageFoldout);
            extensionContainer.Add(actCountFoldout);

            RefreshExpandedState();

            sequenceNodeData.TypeName = "TrowToBucketSequenceNode";
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
            NodeData.BoolArray[0] = _isActCountParameter.value;
            NodeData.BoolArray[1] = _isActedAParameter.value;
            NodeData.BoolArray[2] = _isActorAParameter.value;
            NodeData.BoolArray[3] = _isMassageAParameter.value;

            NodeData.StringArray[0] = _act.value;
            NodeData.StringArray[1] = _acted.value;
            NodeData.StringArray[2] = _actor.value;
            NodeData.StringArray[3] = _massage.value;

            if (NodeData.BoolArray[4]) NodeData.StringArray[4] = ((TextField)_actCount[0]).value;
            else NodeData.IntArray = new List<int> { ((IntegerField)_actCount[0]).value };
            
            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            SequenceManagerElementData sequenceManagerElementData = new SequenceManagerElementData()
            {
                Type = typeof(TrowToBucketSequence).ToString(),
                Bools = NodeData.BoolArray.ToArray(),
                Strings = NodeData.StringArray.ToArray(),
                Ints = NodeData.IntArray.ToArray()
            };

            return sequenceManagerElementData;
        }

        private void _set()
        {
            _actCount.Clear();
            NodeData.BoolArray[4] = _isActCountParameter.value;
            if (NodeData.BoolArray[4]) _actCount.Add(SM_TextField(NodeData.StringArray[4]));
            else _actCount.Add(SM_IntField(NodeData.IntArray[0]));
        }
    }
}