using System;
using System.Collections;
using System.Collections.Generic;
using CM.SequenceManager;
using CM.DataManagement;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;
using Object = System.Object;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("SaveAllSettingDataSequenceNode", "Data Management")]
    public class SaveAllSettingDataSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private ObjectField _objectType;
        private TextField _type;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);

            RefreshExpandedState();

            NodeData = sequenceNodeData;
            sequenceNodeData.TypeName = "SaveAllSettingDataSequenceNode";
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
            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            return new SequenceManagerElementData() { Type = typeof(SaveAllSettingDataSequence).ToString() };
        }
    }
}
