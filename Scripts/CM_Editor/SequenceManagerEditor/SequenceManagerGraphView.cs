using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CM.ApplicationManagement;
using CM.SequenceManager;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
using static CM.Editor.SequenceManagerEditor.SequenceManagerEditorUtilities;

namespace CM.Editor.SequenceManagerEditor
{
    public class SequenceManagerGraphView : GraphView
    {
        public List<SequenceParentNode> SequenceParentNodes;
        public List<ArgumentParentNode> ArgumentParentNodes;
        public List<Group> Groups = new List<Group>();

        public SequenceManagerGraphView(SequenceManagerGraphViewData graphViewData)
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            gridBackground.styleSheets.Add((StyleSheet)AssetDatabase.LoadAssetAtPath
                ("Packages/com.core.coremaker/Scripts/CM_Editor/SequenceManagerEditor/StyleSheets/SequenceManagerStyleSheet.uss", typeof(StyleSheet)));
            Insert(0, gridBackground);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());

            elementsAddedToGroup = (group, elements) =>
            {
                for (int i = 0; i < Groups.Count; i++)
                {
                    if (group.title == Groups[i].title)
                    {
                        foreach (GraphElement element in elements)
                        {
                            if (element is SequenceParentNode sequenceParentNodes)
                            {
                                sequenceParentNodes.NodeData.GroupIndex = i;
                                continue;
                            }

                            if (element is ArgumentParentNode argumentParentNode)
                            {
                                argumentParentNode.NodeData.GroupIndex = i;
                            }
                        }
                        return;
                    }
                }
            };
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (element is SequenceParentNode sequenceParentNodes)
                    {
                        sequenceParentNodes.NodeData.GroupIndex = -1;
                        continue;
                    }

                    if (element is ArgumentParentNode argumentParentNode)
                    {
                        argumentParentNode.NodeData.GroupIndex = -1;
                    }
                }
            };
            deleteSelection = (operationName, user) =>
            {
                List<int> deletedGroups = new List<int>();
                List<SequenceParentNode> deletedSequenceNodes = new List<SequenceParentNode>();
                List<ArgumentParentNode> deletedArgumentNodes = new List<ArgumentParentNode>();
                List<Edge> deletedEdges = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    switch (selectedElement)
                    {
                        case Group group:
                            deletedGroups.Add(Groups.IndexOf(group));
                            break;

                        case SequenceParentNode sequenceParentNode:
                            deletedSequenceNodes.Add(sequenceParentNode);
                            break;

                        case ArgumentParentNode argumentParentNode:
                            if (argumentParentNode.NodeData.NodeIndex != 0)
                                deletedArgumentNodes.Add(argumentParentNode);
                            break;

                        case Edge edge:
                            deletedEdges.Add(edge);
                            break;
                    }
                }

                DeleteElements(deletedEdges);
                foreach (Edge edge in edges.ToArray())
                {
                    if (deletedSequenceNodes.Contains(edge.input.node) || deletedSequenceNodes.Contains(edge.output.node) ||
                        deletedArgumentNodes.Contains(edge.input.node) || deletedArgumentNodes.Contains(edge.output.node))
                    {
                        edge.input.Disconnect(edge);
                        edge.output.DisconnectAll();
                        RemoveElement(edge);
                    }
                }

                foreach (SequenceParentNode deletedSequenceNode in deletedSequenceNodes)
                {
                    RemoveElement(deletedSequenceNode);
                    SequenceParentNodes.Remove(deletedSequenceNode);
                }
                for (int i = 0; i < SequenceParentNodes.Count; i++)
                {
                    SequenceParentNodes[i].NodeData.NodeIndex = i;
                }

                foreach (ArgumentParentNode deletedArgumentNode in deletedArgumentNodes)
                {
                    RemoveElement(deletedArgumentNode);
                    ArgumentParentNodes.Remove(deletedArgumentNode);
                }
                for (int i = 0; i < ArgumentParentNodes.Count; i++)
                {
                    ArgumentParentNodes[i].NodeData.NodeIndex = i;
                }

                foreach (int deletedGroup in deletedGroups)
                {
                    foreach (GraphElement groupElement in Groups[deletedGroup].containedElements)
                    {
                        if (groupElement is SequenceParentNode sequenceParentNodes)
                        {
                            sequenceParentNodes.NodeData.GroupIndex = -1;
                            continue;
                        }

                        if (groupElement is ArgumentParentNode argumentParentNode)
                        {
                            argumentParentNode.NodeData.GroupIndex = -1;
                        }
                    }

                    RemoveElement(Groups[deletedGroup]);
                    Groups.RemoveAt(deletedGroup);
                }
            };

            SequenceParentNodes = new List<SequenceParentNode>();
            ArgumentParentNodes = new List<ArgumentParentNode>();

            for (int i = 0; i < graphViewData.Groups.Count; i++)
            {
                CreateGroup(graphViewData.Groups[i], graphViewData.GroupVector[i]);
            }

            foreach (SequenceNodeData data in graphViewData.SequenceNodeDatas)
            {
                CreateSequenceNode(data);
            }
            foreach (ArgumentNodeData data in graphViewData.ArgumentNodeDatas)
            {
                if (data.TypeName == "StartArgumentNode")
                {
                    CreateStartNode(data);
                    continue;
                }
                CreateArgumentNode(data);
            }

            foreach (SequenceParentNode sequenceParentNode in SequenceParentNodes)
            {
                sequenceParentNode.SetNode(this);
            }
            foreach (ArgumentParentNode argumentParentNode in ArgumentParentNodes)
            {
                argumentParentNode.SetNode(this);
            }

            if (ArgumentParentNodes.Count == 0)
            {
                ArgumentNodeData startNodeData = new ArgumentNodeData();
                startNodeData.NodeName = "StartArgumentNode";
                startNodeData.TypeName = "StartArgumentNode";
                startNodeData.Vector = Vector2.zero;
                CreateStartNode(startNodeData);
            }
        }

        public SequenceManagerGraphViewData SaveGraph()
        {
            SequenceManagerGraphViewData sequenceManagerGraphView = new SequenceManagerGraphViewData
            {
                Groups = new List<string>(),
                GroupVector = new ListStack<Vector2>(),
                SequenceNodeDatas = new List<SequenceNodeData>(),
                ArgumentNodeDatas = new List<ArgumentNodeData>()
            };

            List<string> titles = new List<string>();

            foreach (Group group in Groups)
            {
                string title = group.title;

                if (titles.Contains(title))
                {
                    int repeatCount = 2;

                    while (titles.Contains(title + repeatCount))
                    {
                        repeatCount++;
                    }

                    title += repeatCount.ToString();
                }

                titles.Add(title);

                sequenceManagerGraphView.Groups.Add(title);
                sequenceManagerGraphView.GroupVector.Add(group.GetPosition().position);
            }

            titles = new List<string>();
            sequenceManagerGraphView.ArgumentNodeDatas = new List<ArgumentNodeData>();
            foreach (ArgumentParentNode argumentParentNode in ArgumentParentNodes)
            {
                sequenceManagerGraphView.ArgumentNodeDatas.Add(argumentParentNode.SaveNode());

                if (sequenceManagerGraphView.ArgumentNodeDatas.Count != 1)
                {
                    sequenceManagerGraphView.ArgumentNodeDatas.Last().NodeName =
                        ((TextField) argumentParentNode.extensionContainer[1]).value;
                }

                sequenceManagerGraphView.ArgumentNodeDatas.Last().Vector = argumentParentNode.GetPosition().position;
                sequenceManagerGraphView.ArgumentNodeDatas.Last().GroupIndex = argumentParentNode.NodeData.GroupIndex;
                sequenceManagerGraphView.ArgumentNodeDatas.Last().Ports = new List<int>();

                string name = sequenceManagerGraphView.ArgumentNodeDatas.Last().NodeName;
                if (titles.Contains(name))
                {
                    int repeatCount = 2;

                    while (titles.Contains(name + repeatCount))
                    {
                        repeatCount++;
                    }

                    name += repeatCount.ToString();
                }
                titles.Add(name);
                sequenceManagerGraphView.ArgumentNodeDatas.Last().NodeName = name;
            }
            titles = new List<string>();
            sequenceManagerGraphView.SequenceNodeDatas = new List<SequenceNodeData>();
            foreach (SequenceParentNode sequenceParentNode in SequenceParentNodes)
            {
                sequenceManagerGraphView.SequenceNodeDatas.Add(sequenceParentNode.SaveNode());
                sequenceManagerGraphView.SequenceNodeDatas.Last().NodeName =
                    ((TextField)sequenceParentNode.extensionContainer[1]).value;
                sequenceManagerGraphView.SequenceNodeDatas.Last().Vector = sequenceParentNode.GetPosition().position;
                sequenceManagerGraphView.SequenceNodeDatas.Last().GroupIndex = sequenceParentNode.NodeData.GroupIndex;
                sequenceManagerGraphView.SequenceNodeDatas.Last().Port = -1;

                string name = sequenceManagerGraphView.SequenceNodeDatas.Last().NodeName;
                if (titles.Contains(name))
                {
                    int repeatCount = 2;

                    while (titles.Contains(name + repeatCount))
                    {
                        repeatCount++;
                    }

                    name += repeatCount.ToString();
                }
                titles.Add(name);
                sequenceManagerGraphView.SequenceNodeDatas.Last().NodeName = name;
            }

            foreach (Edge edge in edges.ToList())
            {
                if (edge.output.node is SequenceParentNode sequenceParentNode)
                {
                    sequenceManagerGraphView.SequenceNodeDatas[sequenceParentNode.NodeData.NodeIndex].Port =
                        ((ArgumentParentNode)edge.input.node).NodeData.NodeIndex;
                    continue;
                }

                if (edge.output.node is ArgumentParentNode argumentParentNode)
                {
                    if (sequenceManagerGraphView.ArgumentNodeDatas[argumentParentNode.NodeData.NodeIndex].Ports.Count == 0)
                    {
                        sequenceManagerGraphView.ArgumentNodeDatas[argumentParentNode.NodeData.NodeIndex].Ports 
                            = Enumerable.Repeat(-1, argumentParentNode.outputContainer.childCount).ToList();
                    }

                    sequenceManagerGraphView.ArgumentNodeDatas[argumentParentNode.NodeData.NodeIndex]
                        .Ports[argumentParentNode.outputContainer.IndexOf(edge.output)]
                        = (((SequenceParentNode)edge.input.node).NodeData.NodeIndex);
                };
            }

            return sequenceManagerGraphView;
        }

        public SequenceTableData CreateSequenceTable(string tableName, bool isTableActive)
        {
            List<SequenceManagerElementData> sequenceData = new List<SequenceManagerElementData>();
            List<SequenceManagerElementData> argumentData = new List<SequenceManagerElementData>();

            foreach (SequenceParentNode sequenceParentNode in SequenceParentNodes)
            {
                SequenceManagerElementData elementData = sequenceParentNode.CreateSequence();

                elementData.Name = sequenceParentNode.NodeData.NodeName;
                if (sequenceParentNode.NodeData.Port != -1)
                {
                    elementData.TargetName = new[] { ArgumentParentNodes[sequenceParentNode.NodeData.Port].NodeData.NodeName };
                }

                sequenceData.Add(elementData);
            }
            foreach (ArgumentParentNode argumentParentNode in ArgumentParentNodes)
            {
                SequenceManagerElementData elementData = argumentParentNode.CreateArgument();

                elementData.Name = argumentParentNode.NodeData.NodeName;

                List<string> portNames = new List<string>();
                foreach (int index in argumentParentNode.NodeData.Ports) { portNames.Add(SequenceParentNodes[index].NodeData.NodeName); }
                elementData.TargetName = portNames.ToArray();

                argumentData.Add(elementData);
            }

            SequenceTableData sequenceTableData = new SequenceTableData
            {
                TableName = tableName,
                IsTableActive = isTableActive,
                CurrentArgumentName = ArgumentParentNodes[0].NodeData.NodeName,
                SequenceData = sequenceData.ToArray(),
                ArgumentData = argumentData.ToArray()
            };

            return sequenceTableData;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            foreach (var port in ports)
            {
                if (startPort.portType != port.portType)
                {
                    continue;
                }

                if (startPort == port)
                {
                    continue;
                }

                if (startPort.node == port.node)
                {
                    continue;
                }

                if (startPort.direction == port.direction)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }

        public void CreateGroup(string groupName, Vector2 location)
        {
            Group group = new Group() { title = groupName };

            group.SetPosition(new Rect(location, Vector2.zero));

            group.style.backgroundColor = new StyleColor(new Color(0, 0, .44f, 1));
            group.ElementAt(0).ElementAt(0).ElementAt(0).ElementAt(0).style.color = Color.white;

            group.style.borderBottomWidth = 2;
            group.style.borderRightWidth = 2;
            group.style.borderLeftWidth = 2;
            group.style.borderTopWidth = 2;

            group.style.borderTopRightRadius = 0;
            group.style.borderTopLeftRadius = 0;
            group.style.borderBottomRightRadius = 0;
            group.style.borderBottomLeftRadius = 0;

            group.style.borderBottomColor = new StyleColor(Color.white);
            group.style.borderRightColor = new StyleColor(Color.white);
            group.style.borderLeftColor = new StyleColor(Color.white);
            group.style.borderTopColor = new StyleColor(Color.white);

            Groups.Add(group);

            AddElement(Groups.Last());

            foreach (GraphElement selected in selection)
            {
                if (selected is SequenceParentNode)
                {
                    SequenceParentNode node = (SequenceParentNode)selected;
                    group.AddElement(node);
                }

                if (selected is ArgumentParentNode)
                {
                    ArgumentParentNode node = (ArgumentParentNode)selected;
                    group.AddElement(node);
                }
            }
        }

        public void CreateSequenceNode(SequenceNodeData sequenceNodeData)
        {
            SequenceParentNode sequenceNode = (SequenceParentNode)Activator.CreateInstance(NodeTypeGroups.SequenceTypes[sequenceNodeData.TypeName]);

            sequenceNode.SetPosition(new Rect(sequenceNodeData.Vector, Vector2.zero));

            sequenceNode.titleContainer.style.backgroundColor = new StyleColor(new Color(0, 0.1f, 0.3f, 1));
            sequenceNode.titleContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.titleContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.titleContainer.style.borderTopColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.titleContainer.style.height = 17;
            sequenceNode.titleContainer.style.borderLeftWidth = 2;
            sequenceNode.titleContainer.style.borderRightWidth = 2;
            sequenceNode.titleContainer.style.borderTopWidth = 2;

            sequenceNode.inputContainer.style.backgroundColor = new StyleColor(new Color(0.43f, 0.49f, 0.65f, 1));
            sequenceNode.outputContainer.style.backgroundColor = new StyleColor(new Color(0.3f, 0.35f, 0.5f, 1));
            sequenceNode.inputContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.outputContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.inputContainer.style.borderLeftWidth = 2;
            sequenceNode.outputContainer.style.borderRightWidth = 2;

            sequenceNode.extensionContainer.style.borderBottomColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.extensionContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.extensionContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            sequenceNode.extensionContainer.style.borderBottomWidth = 2;
            sequenceNode.extensionContainer.style.borderLeftWidth = 2;
            sequenceNode.extensionContainer.style.borderRightWidth = 2;

            sequenceNode.extensionContainer.Add(SM_Label("Nodes Name"));
            sequenceNode.extensionContainer.Add(SM_TextField(sequenceNodeData.NodeName));

            sequenceNode.CreateNode(sequenceNodeData);
            sequenceNode.AddManipulator(new Dragger());
            sequenceNode.titleContainer.Add(SM_Label(sequenceNodeData.TypeName));

            sequenceNode.NodeData.NodeIndex = SequenceParentNodes.Count;
            SequenceParentNodes.Add(sequenceNode);

            AddElement(SequenceParentNodes.Last());
            if (sequenceNodeData.GroupIndex != -1)
            {
                Groups[sequenceNodeData.GroupIndex].AddElement(sequenceNode);
            }
        }

        public void CreateArgumentNode(ArgumentNodeData argumentNodeData)
        {
            ArgumentParentNode argumentNode = (ArgumentParentNode)Activator.CreateInstance(NodeTypeGroups.ArgumentTypes[argumentNodeData.TypeName]);

            argumentNode.SetPosition(new Rect(argumentNodeData.Vector, Vector2.zero));

            argumentNode.titleContainer.style.backgroundColor = new StyleColor(new Color(0, 0.1f, 0.3f, 1));
            argumentNode.titleContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.titleContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.titleContainer.style.borderTopColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.titleContainer.style.height = 17;
            argumentNode.titleContainer.style.borderLeftWidth = 2;
            argumentNode.titleContainer.style.borderRightWidth = 2;
            argumentNode.titleContainer.style.borderTopWidth = 2;

            argumentNode.inputContainer.style.backgroundColor = new StyleColor(new Color(0.43f, 0.49f, 0.65f, 1));
            argumentNode.outputContainer.style.backgroundColor = new StyleColor(new Color(0.3f, 0.35f, 0.5f, 1));
            argumentNode.inputContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.outputContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.inputContainer.style.borderLeftWidth = 2;
            argumentNode.outputContainer.style.borderRightWidth = 2;

            argumentNode.extensionContainer.style.borderBottomColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.extensionContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.extensionContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            argumentNode.extensionContainer.style.borderBottomWidth = 2;
            argumentNode.extensionContainer.style.borderLeftWidth = 2;
            argumentNode.extensionContainer.style.borderRightWidth = 2;

            argumentNode.extensionContainer.Add(SM_Label("Nodes Name"));
            argumentNode.extensionContainer.Add(SM_TextField(argumentNodeData.NodeName));

            argumentNode.CreateNode(argumentNodeData);
            argumentNode.AddManipulator(new Dragger());
            argumentNode.titleContainer.Add(SM_Label(argumentNodeData.TypeName));

            argumentNode.NodeData.NodeIndex = ArgumentParentNodes.Count;
            ArgumentParentNodes.Add(argumentNode);

            AddElement(ArgumentParentNodes.Last());
            if (argumentNodeData.GroupIndex != -1)
            {
                Groups[argumentNodeData.GroupIndex].AddElement(argumentNode);
            }
        }

        public void CreateStartNode(ArgumentNodeData argumentNodeData)
        {
            if (ArgumentParentNodes.Count != 0)
            {
                ArgumentParentNodes[0].SetPosition(new Rect(argumentNodeData.Vector, Vector2.zero));
                return;
            }
            ArgumentParentNode startNode = new StartArgumentNode();

            startNode.SetPosition(new Rect(argumentNodeData.Vector, Vector2.zero));

            startNode.titleContainer.style.backgroundColor = new StyleColor(new Color(0, 0.2980392f, 0.007843138f, 1));
            startNode.titleContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            startNode.titleContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            startNode.titleContainer.style.borderTopColor = new StyleColor(new Color(1f, 1, 1, 1));
            startNode.titleContainer.style.borderBottomColor = new StyleColor(new Color(0, 0.2980392f, 0.007843138f, 1));
            startNode.titleContainer.style.height = 17;
            startNode.titleContainer.style.borderLeftWidth = 2;
            startNode.titleContainer.style.borderRightWidth = 2;
            startNode.titleContainer.style.borderTopWidth = 2;
            startNode.titleContainer.style.borderBottomWidth = 1;

            startNode.inputContainer.style.backgroundColor = new StyleColor(new Color(0.4313726f, 0.6509804f, 0.4742354f, 1));
            startNode.outputContainer.style.backgroundColor = new StyleColor(new Color(0.2980392f, 0.5019608f, 0.3358727f, 1));
            startNode.inputContainer.style.borderLeftColor = new StyleColor(new Color(1f, 1, 1, 1));
            startNode.inputContainer.style.borderBottomColor = new StyleColor(new Color(1f, 1, 1, 1));
            startNode.inputContainer.style.borderRightColor = new StyleColor(new Color(0.4313726f, 0.6509804f, 0.4742354f, 1));
            startNode.inputContainer.style.borderTopColor = new StyleColor(new Color(0.4313726f, 0.6509804f, 0.4742354f, 1));
            startNode.outputContainer.style.borderRightColor = new StyleColor(new Color(1f, 1, 1, 1));
            startNode.outputContainer.style.borderBottomColor = new StyleColor(new Color(1f, 1, 1, 1));
            startNode.outputContainer.style.borderLeftColor = new StyleColor(new Color(0.2980392f, 0.5019608f, 0.3358727f, 1));
            startNode.outputContainer.style.borderTopColor = new StyleColor(new Color(0.2980392f, 0.5019608f, 0.3358727f, 1));
            startNode.inputContainer.style.borderLeftWidth = 2;
            startNode.inputContainer.style.borderBottomWidth = 2;
            startNode.inputContainer.style.borderRightWidth = 1;
            startNode.inputContainer.style.borderTopWidth = 1;
            startNode.outputContainer.style.borderRightWidth = 2;
            startNode.outputContainer.style.borderBottomWidth = 2;
            startNode.outputContainer.style.borderLeftWidth = 1;
            startNode.outputContainer.style.borderTopWidth = 1;

            startNode.CreateNode(argumentNodeData);
            startNode.AddManipulator(new Dragger());
            startNode.titleContainer.Add(SM_Label(argumentNodeData.TypeName));

            startNode.NodeData.NodeIndex = 0;
            ArgumentParentNodes.Insert(0, startNode);

            AddElement(ArgumentParentNodes[0]);
            if (argumentNodeData.GroupIndex != -1)
            {
                Groups[argumentNodeData.GroupIndex].AddElement(startNode);
            }
        }
    }
}