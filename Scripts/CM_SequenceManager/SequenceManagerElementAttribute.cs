using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CM.SequenceManager
{
    public enum SequenceType { Sequence, Argument }

    [AttributeUsage(AttributeTargets.Class)]
    public class SequenceManagerElementAttribute : Attribute
    {
        private SequenceType _sequenceType;
        private Type _type;

        internal static Dictionary<string, Type> SequenceTypes = new Dictionary<string, Type>();
        internal static Dictionary<string, Type> ArgumentTypes = new Dictionary<string, Type>();

        public SequenceManagerElementAttribute(Type thisType, SequenceType type)
        {
            _sequenceType = type;
            _type = thisType;
        }

        internal static void Internalize()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    SequenceManagerElementAttribute sequenceAttribute = type.GetCustomAttribute<SequenceManagerElementAttribute>();

                    if (sequenceAttribute != null)
                    {
                        if (sequenceAttribute._sequenceType == SequenceType.Sequence)
                        {
                            if (!SequenceTypes.ContainsKey(sequenceAttribute._type.ToString()))
                                SequenceTypes.Add(sequenceAttribute._type.ToString(), sequenceAttribute._type);
                            
                            continue;
                        }
                        if (!ArgumentTypes.ContainsKey(sequenceAttribute._type.ToString()))
                            ArgumentTypes.Add(sequenceAttribute._type.ToString(), sequenceAttribute._type);
                    }
                }
            }
        }
    }
}
