using System.Collections;
using System.Collections.Generic;
using CM.DataManagement;
using CM.SequenceManager;
using PlasticGui.Configuration.CloudEdition;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    [Sequence("SaveSettingDataSequenceNode", "Data Management")]
    public class SaveSettingDataSequenceNode : SequenceParentNode
    {
        private Port _inputPort;
        private Port _outputPort;

        private ObjectField _objectType;
        private TextField _type;

        public override void CreateNode(SequenceNodeData sequenceNodeData)
        {
            if (sequenceNodeData.StringArray == null || sequenceNodeData.StringArray.Count != 1)
                sequenceNodeData.StringArray = new List<string>() { "null" };

            _inputPort = SM_Port(this, "Input", PortType.SequenceInput);
            inputContainer.Add(_inputPort);

            _outputPort = SM_Port(this, "Output", PortType.SequenceOutput);
            outputContainer.Add(_outputPort);

            _objectType = SM_ObjectField(null, null, typeof(DataManagerSettingMainObject));
            _objectType.RegisterValueChangedCallback(target =>
            {
                _type.value = _objectType.value.ToString();
                _type.value = _type.value.Remove(_type.value.IndexOf(')'), _type.value.Length - _type.value.IndexOf(')'));
                _type.value = _type.value.Remove(0, _type.value.IndexOf('(') + 1);
            });

            _type = SM_TextField(sequenceNodeData.StringArray[0]);

            extensionContainer.Add(SM_Label("Save GameObject"));
            extensionContainer.Add(_objectType);
            extensionContainer.Add(SM_Label("Save types name"));
            extensionContainer.Add(_type);

            RefreshExpandedState();

            NodeData = sequenceNodeData;
            sequenceNodeData.TypeName = "SaveSettingDataSequenceNode";
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
            NodeData.Vector = GetPosition().position;

            NodeData.StringArray = new List<string> { _type.value };

            return NodeData;
        }

        public override SequenceManagerElementData CreateSequence()
        {
            return new SequenceManagerElementData()
            {
                Strings = new[] { _type.value },
                Type = typeof(SaveSettingDataSequence).ToString()
            };
        }
    }
}
