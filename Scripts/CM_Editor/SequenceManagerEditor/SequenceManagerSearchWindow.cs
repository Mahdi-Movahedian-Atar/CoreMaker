using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CM.Editor.SequenceManagerEditor
{
    public class SequenceManagerSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private SequenceManagerGraphView _graphView;
        private SequenceManagerMenuItem _menuItem;
        private Texture2D _emptyIcon;

        public void Initialize(SequenceManagerGraphView sequenceManagerGraph, SequenceManagerMenuItem sequenceManagerMenuItem)
        {
            _graphView = sequenceManagerGraph;
            _menuItem = sequenceManagerMenuItem;

            _emptyIcon = new Texture2D(1, 1);
            _emptyIcon.SetPixel(0, 0, Color.clear);
            _emptyIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>();

            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("Create Elements")));

            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("Sequences"), 1));

            foreach (string group in NodeTypeGroups.Groups)
            {
                if (NodeTypeGroups.SequenceGroups.ContainsValue(group))
                {
                    searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent(group), 2));

                    foreach (string sequence in NodeTypeGroups.SequenceGroups.Keys)
                    {
                        if (NodeTypeGroups.SequenceGroups[sequence] == group)
                        {
                            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent(sequence, _emptyIcon))
                            { userData = new[] { "Sequence", sequence }, level = 3 });
                        }
                    }
                }

            }

            searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("Arguments"), 1));

            foreach (string group in NodeTypeGroups.Groups)
            {
                if (NodeTypeGroups.ArgumentGroups.ContainsValue(group))
                {
                    searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent(group), 2));

                    foreach (string argument in NodeTypeGroups.ArgumentGroups.Keys)
                    {
                        if (NodeTypeGroups.ArgumentGroups[argument] == group)
                        {
                            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent(argument, _emptyIcon))
                                { userData = new[] { "Argument", argument }, level = 3 });
                        }
                    }
                }
            }

            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent("Group", _emptyIcon))
            {
                userData = new[] { "Group" },
                level = 1
            });

            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent("SetStartNode", _emptyIcon))
            {
                userData = new[] { "CreateStartNode" },
                level = 1
            });

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition =
                _graphView.contentViewContainer.WorldToLocal(_menuItem.rootVisualElement.ChangeCoordinatesTo(
                    _menuItem.rootVisualElement.parent, context.screenMousePosition - _menuItem.position.position));

            switch (((string[])searchTreeEntry.userData)[0])
            {
                case "Sequence":
                    SequenceNodeData sequenceNodeData = new SequenceNodeData();
                    sequenceNodeData.NodeName = ((string[])searchTreeEntry.userData)[1];
                    sequenceNodeData.TypeName = ((string[])searchTreeEntry.userData)[1];
                    sequenceNodeData.Vector = localMousePosition;

                    _graphView.CreateSequenceNode(sequenceNodeData);

                    return true;

                case "Argument":
                    ArgumentNodeData argumentNodeData = new ArgumentNodeData();
                    argumentNodeData.NodeName = ((string[])searchTreeEntry.userData)[1];
                    argumentNodeData.TypeName = ((string[])searchTreeEntry.userData)[1];
                    argumentNodeData.Vector = localMousePosition;

                    _graphView.CreateArgumentNode(argumentNodeData);

                    return true;

                case "Group":
                    _graphView.CreateGroup("NullGroup", localMousePosition);

                    return true;

                case "CreateStartNode":
                    ArgumentNodeData startNodeData = new ArgumentNodeData();
                    startNodeData.NodeName = "StartArgumentNode";
                    startNodeData.TypeName = "StartArgumentNode";
                    startNodeData.Vector = localMousePosition;

                    _graphView.CreateStartNode(startNodeData);

                    return true;

                default:
                    return false;
            }
        }
    }
}
