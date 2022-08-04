using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CM.InputManagement;
using UnityEngine;
using UnityEngine.Events;

namespace CM.InputManagement
{
    public class InputTableSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            InputTable inputTable = (InputTable)obj;

            info.AddValue("FirstXInvert", inputTable.FirstXInvert);
            info.AddValue("FirstYInvert", inputTable.FirstYInvert);
            info.AddValue("SecondXInvert", inputTable.SecondXInvert);
            info.AddValue("SecondYInvert", inputTable.SecondYInvert);

            info.AddValue("FirstXSensitivity" , (double)inputTable.FirstXSensitivity);
            info.AddValue("FirstYSensitivity", (double)inputTable.FirstYSensitivity);
            info.AddValue("SecondXSensitivity", (double)inputTable.SecondXSensitivity);
            info.AddValue("SecondYSensitivity", (double)inputTable.SecondYSensitivity);

            info.AddValue("length", inputTable.KeyNames.Length);

            for (int i = 0; i < inputTable.KeyModes.Length; i++)
            {
                info.AddValue("keyMode" + i, inputTable.KeyModes[i].GetHashCode());
            }

            for (int i = 0; i < inputTable.KeyBoardKeyCode.Length; i++)
            {
                info.AddValue("keyBoard" + i, inputTable.KeyBoardKeyCode[i].GetHashCode());
            }

            for (int i = 0; i < inputTable.XBoxKeyCode.Length; i++)
            {
                info.AddValue("Xbox" + i, inputTable.XBoxKeyCode[i].GetHashCode());
            }

            for (int i = 0; i < inputTable.PsKeyCode.Length; i++)
            {
                info.AddValue("Ps" + i, inputTable.PsKeyCode[i].GetHashCode());
            }
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            InputTable inputTable = (InputTable)obj;

            int length = info.GetInt32("length");
            
            inputTable = new InputTable
            {
                FirstXInvert = info.GetBoolean("FirstXInvert"),
                FirstYInvert = info.GetBoolean("FirstYInvert"),
                SecondXInvert = info.GetBoolean("SecondXInvert"),
                SecondYInvert = info.GetBoolean("SecondYInvert"),

                FirstXSensitivity = (float)info.GetDouble("FirstXSensitivity"),
                FirstYSensitivity = (float)info.GetDouble("FirstYSensitivity"),
                SecondXSensitivity = (float)info.GetDouble("SecondXSensitivity"),
                SecondYSensitivity = (float)info.GetDouble("SecondYSensitivity"),

                KeyNames = new string[length],

                KeyModes = new KeyMode[length],

                KeyBoardKeyCode = new CMKeyCode[length],
                XBoxKeyCode = new CMKeyCode[length],
                PsKeyCode = new CMKeyCode[length],

                KeyCodeEvent = new UnityEvent[length],

                FirstAxesEvent = new UnityEvent<float , float>(),
                SecondAxesEvent = new UnityEvent<float ,float>(),
            };

            for (int i = 0; i < length; i++)
            {
                inputTable.KeyModes[i] = (KeyMode)info.GetInt32("keyMode" + i);
            }

            for (int i = 0; i < length; i++)
            {
                inputTable.KeyBoardKeyCode[i] = (CMKeyCode)info.GetInt32(inputTable.KeyNames[i] + "keyBoard" + i);
            }

            for (int i = 0; i < length; i++)
            {
                inputTable.XBoxKeyCode[i] = (CMKeyCode)info.GetInt32(inputTable.KeyNames[i] + "Xbox" + i);
            }

            for (int i = 0; i < length; i++)
            {
                inputTable.PsKeyCode[i] = (CMKeyCode)info.GetInt32(inputTable.KeyNames[i] + "Ps" + i);
            }

            obj = inputTable;

            return obj;
        }
    }
}