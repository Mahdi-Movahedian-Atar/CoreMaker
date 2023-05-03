using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.SequenceManager
{
    [Serializable]
    public class SequenceManagerData
    {
        public string Name;
        
        public SequenceTableData[] SequenceTableData;

        public string[] StringParameterValues;
        public string[] StringParameterNames;

        public int[] IntParameterValues;
        public string[] IntParameterNames;

        public float[] FloatParameterValues;
        public string[] FloatParameterNames;

        public bool[] BoolParameterValues;
        public string[] BoolParameterNames;

        public void SetSequenceManager()
        {
            SequenceManager sequenceManager = ScriptableObject.FindObjectOfType<SequenceManager>();

            sequenceManager.Name = Name;

            sequenceManager.SequenceTables = new Dictionary<string, SequenceTable>();
            sequenceManager.StringParameters = new Dictionary<string, string>();
            sequenceManager.IntParameters = new Dictionary<string, int>();
            sequenceManager.FloatParameters = new Dictionary<string, float>();
            sequenceManager.BoolParameters = new Dictionary<string, bool>();

            foreach (SequenceTableData tableData in SequenceTableData)
            {
                SequenceTable sequenceTable = new SequenceTable()
                {
                    TableName = tableData.TableName,
                    Sequences = new Dictionary<string, Sequence>(),
                    Arguments = new Dictionary<string, Argument>(),
                    IsTableActive = tableData.IsTableActive,
                    CurrentArgumentName = tableData.CurrentArgumentName
                };

                foreach (SequenceManagerElementData sequenceData in tableData.SequenceData)
                {
                    Sequence sequence = (Sequence)Activator.CreateInstance(SequenceManagerElementAttribute.SequenceTypes[sequenceData.Type]);
                    sequence = sequence.MakeSequence(sequenceData);
                    sequence.SequenceName = sequenceData.Name;
                    sequence.TableName = tableData.TableName;
                    if (sequenceData.TargetName != null && sequenceData.TargetName.Length != 0)
                    {
                        sequence.TargetArgumentName = sequenceData.TargetName[0];
                    }

                    sequenceTable.Sequences.Add(sequenceData.Name, sequence);
                }

                foreach (SequenceManagerElementData argumentData in tableData.ArgumentData)
                {
                    Argument argument = (Argument)Activator.CreateInstance(SequenceManagerElementAttribute.ArgumentTypes[argumentData.Type]);
                    argument = argument.MakeArgument(argumentData);
                    argument.ArgumentName = argumentData.Name;
                    argument.TableName = tableData.TableName;
                    argument.TargetSequenceName = argumentData.TargetName;

                    sequenceTable.Arguments.Add(argumentData.Name, argument);
                }

                sequenceManager.SequenceTables.Add(tableData.TableName, sequenceTable);
            }

            if (StringParameterNames != null)
            {
                for (int i = 0; i < StringParameterNames.Length; i++)
                {
                    sequenceManager.StringParameters.Add(StringParameterNames[i], StringParameterValues[i]);
                }
            }
            if (IntParameterNames != null)
            {
                for (int i = 0; i < IntParameterNames.Length; i++)
                {
                    sequenceManager.IntParameters.Add(IntParameterNames[i], IntParameterValues[i]);
                }
            }
            if (FloatParameterNames != null)
            {
                for (int i = 0; i < FloatParameterNames.Length; i++)
                {
                    sequenceManager.FloatParameters.Add(FloatParameterNames[i], FloatParameterValues[i]);
                }
            }
            if (BoolParameterNames != null)
            {
                for (int i = 0; i < BoolParameterNames.Length; i++)
                {
                    sequenceManager.BoolParameters.Add(BoolParameterNames[i], BoolParameterValues[i]);
                }
            }

            SequenceManager.CurrentSequenceManager = sequenceManager;
        }
    }
}
