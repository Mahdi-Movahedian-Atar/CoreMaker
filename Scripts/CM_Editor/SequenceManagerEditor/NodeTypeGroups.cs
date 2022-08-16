using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CM.Editor.SequenceManagerEditor
{
    public class NodeTypeGroups
    {
        internal static Dictionary<string, Type> SequenceTypes = new Dictionary<string, Type>();
        internal static Dictionary<string, string> SequenceGroups = new Dictionary<string, string>();
        internal static Dictionary<string, Type> ArgumentTypes = new Dictionary<string, Type>();
        internal static Dictionary<string, string> ArgumentGroups = new Dictionary<string, string>();
        internal static List<string> Groups = new List<string>();

        internal static void Internalize()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    SequenceAttribute sequenceAttribute = type.GetCustomAttribute<SequenceAttribute>();
                    if (sequenceAttribute != null)
                    {
                        if (!SequenceTypes.ContainsKey(sequenceAttribute.Name))
                        {
                            SequenceTypes.Add(sequenceAttribute.Name, type);
                            SequenceGroups.Add(sequenceAttribute.Name, sequenceAttribute.GroupName);

                            if (!Groups.Contains(sequenceAttribute.GroupName))
                            {
                                Groups.Add(sequenceAttribute.GroupName);
                            }
                        }
                        else
                        {
                            Debug.LogWarning(
                                "Editor.SequenceManager: A sequence node with this name(" + sequenceAttribute.Name + ") already exists");
                        }
                        continue;
                    }

                    ArgumentAttribute argumentAttribute = type.GetCustomAttribute<ArgumentAttribute>();
                    if (argumentAttribute != null)
                    {
                        if (!ArgumentTypes.ContainsKey(argumentAttribute.Name))
                        {
                            ArgumentTypes.Add(argumentAttribute.Name, type);
                            ArgumentGroups.Add(argumentAttribute.Name, argumentAttribute.GroupName);

                            if (!Groups.Contains(argumentAttribute.GroupName))
                            {
                                Groups.Add(argumentAttribute.GroupName);
                            }
                        }
                        else
                        {
                            Debug.LogWarning(
                                "Editor.SequenceManager: An argument node with this name(" + argumentAttribute.Name + ") already exists");
                        }
                    }
                }
            }
            Groups.Sort();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ArgumentAttribute : Attribute
    {
        internal string Name;
        internal string GroupName;

        public ArgumentAttribute(string name, string groupName)
        {
            Name = name;
            GroupName = groupName;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SequenceAttribute : Attribute
    {
        internal string Name;
        internal string GroupName;

        public SequenceAttribute(string name, string groupName)
        {
            Name = name;
            GroupName = groupName;
        }
    }
}
