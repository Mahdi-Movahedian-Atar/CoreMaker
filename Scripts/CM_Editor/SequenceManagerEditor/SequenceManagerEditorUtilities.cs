using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace CM.Editor.SequenceManagerEditor
{
    public enum PortType
    {
        ArgumentInput,
        ArgumentOutput,
        SequenceInput,
        SequenceOutput
    }

    public class SequenceManagerEditorUtilities
    {
        private static StyleSheet _styleSheet = (StyleSheet)AssetDatabase.LoadAssetAtPath
            ("Packages/com.core.coremaker/Scripts/CM_Editor/SequenceManagerEditor/StyleSheets/SequenceManagerUtilityStyleSheet.uss", typeof(StyleSheet));

        internal static TextField SM_TextField(string value)
        {
            TextField textField = new TextField() { value = value };

            textField.styleSheets.Add(_styleSheet);
            textField.AddToClassList("SM_TextField");

            return textField;
        }

        internal static IntegerField SM_IntField(int value)
        {
            IntegerField intField = new IntegerField() { value = value };

            intField.styleSheets.Add(_styleSheet);
            intField.AddToClassList("SM_IntField");

            return intField;
        }

        internal static FloatField SM_FloatField(float value)
        {
            FloatField floatField = new FloatField() { value = value };

            floatField.styleSheets.Add(_styleSheet);
            floatField.AddToClassList("SM_FloatField");

            return floatField;
        }

        internal static Toggle SM_Toggle(bool value)
        {
            Toggle toggle = new Toggle() { value = value };
            toggle.RegisterValueChangedCallback(target =>
            {
                if (toggle.value) toggle.ElementAt(0).style.backgroundColor = new Color(0, .5f, 0);
                else toggle.ElementAt(0).style.backgroundColor = Color.red;
            });

            if (toggle.value) toggle.ElementAt(0).style.backgroundColor = new Color(0, .5f, 0);
            else toggle.ElementAt(0).style.backgroundColor = Color.red;

            toggle.styleSheets.Add(_styleSheet);
            toggle.AddToClassList("SM_Toggle");

            return toggle;
        }

        internal static ObjectField SM_ObjectField(string label, Object value, Type type)
        {
            ObjectField objectField = new ObjectField(label)
            {
                objectType = type,
                value = value
            };

            objectField.styleSheets.Add(_styleSheet);
            objectField.AddToClassList("SM_ObjectField");

            return objectField;
        }

        internal static Label SM_Label(string value)
        {
            Label label = new Label(value);

            label.styleSheets.Add(_styleSheet);
            label.AddToClassList("SM_Label");

            return label;
        }

        internal static Button SM_Button(string label, Action clickEvent)
        {
            Button button = new Button(clickEvent)
            {
                text = label
            };

            button.styleSheets.Add(_styleSheet);
            button.AddToClassList("SM_Button");

            return button;
        }

        internal static DropdownField SM_DropdownField(List<string> choices, int index, Func<string, string> funk)
        {
            DropdownField dropdownField = new DropdownField(choices, index, funk);

            dropdownField.styleSheets.Add(_styleSheet);
            dropdownField.AddToClassList("SM_DropdownField");

            return dropdownField;
        }

        internal static ScrollView SM_ScrollView(ScrollViewMode scrollViewMode)
        {
            ScrollView scrollView = new ScrollView(scrollViewMode);

            scrollView.styleSheets.Add(_styleSheet);
            scrollView.AddToClassList("SM_ScrollView");

            return scrollView;
        }

        internal static Foldout SM_Foldout(string label)
        {
            Foldout foldout = new Foldout() { text = label, value = false };

            foldout.styleSheets.Add(_styleSheet);
            foldout.AddToClassList("SM_Foldout");

            return foldout;
        }

        internal static Port SM_Port(Node node, string label, PortType portType)
        {
            Port port;

            switch (portType)
            {
                case PortType.ArgumentInput:
                    port = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                        typeof(ArgumentParentNode));
                    port.portName = label;
                    port.portColor = Color.red;
                    break;

                case PortType.ArgumentOutput:
                    port = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                        typeof(SequenceParentNode));
                    port.portName = label;
                    port.portColor = Color.green;
                    break;

                case PortType.SequenceInput:
                    port = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                        typeof(SequenceParentNode));
                    port.portName = label;
                    port.portColor = Color.green;
                    break;

                case PortType.SequenceOutput:
                    port = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                        typeof(ArgumentParentNode));
                    port.portName = label;
                    port.portColor = Color.red;
                    break;

                default: return null;
            }

            port.ElementAt(1).style.color = Color.white;

            return port;
        }
    }
}
