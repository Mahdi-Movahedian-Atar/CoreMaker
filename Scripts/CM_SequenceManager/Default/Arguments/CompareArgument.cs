using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(CompareArgument<int>), SequenceType.Argument)]
    public class CompareArgument<T> : Argument
    {
        private int _type;
        //----------------------------------------------------------------------------------------------------------------
        private string _firstParameterElement;
        private T _firstElement;
        private string _secondParameterElement;
        private T _secondElement;
        //----------------------------------------------------------------------------------------------------------------
        public string FirstParameterElement
        {
            get
            {
                return _firstParameterElement;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _firstElement = default(T);
                }

                _firstParameterElement = value;

                switch (typeof(T).ToString())
                {
                    case "System.Int32":
                        _type = 0;
                        break;

                    case "System.Single":
                        _type = 1;
                        break;

                    case "System.String":
                        _type = 2;
                        break;

                    case "System.Boolean":
                        _type = 3;
                        break;

                    default:
                        _type = 4;
                        break;
                }
            }
        }
        public T FirstElement
        {
            get
            {
                return _firstElement;
            }
            set
            {
                if (value != null)
                {
                    _firstParameterElement = null;
                }

                _firstElement = value;

                switch (typeof(T).ToString())
                {
                    case "System.Int32":
                        _type = 0;
                        break;

                    case "System.Single":
                        _type = 1;
                        break;

                    case "System.String":
                        _type = 2;
                        break;

                    case "System.Boolean":
                        _type = 3;
                        break;

                    default:
                        _type = 4;
                        break;
                }
            }
        }
        public string SecondParameterElement
        {
            get
            {
                return _secondParameterElement;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _secondElement = default(T);
                }

                _secondParameterElement = value;

                switch (typeof(T).ToString())
                {
                    case "System.Int32":
                        _type = 0;
                        break;

                    case "System.Single":
                        _type = 1;
                        break;

                    case "System.String":
                        _type = 2;
                        break;

                    case "System.Boolean":
                        _type = 3;
                        break;

                    default:
                        _type = 4;
                        break;
                }
            }
        }
        public T SecondElement
        {
            get
            {
                return _secondElement;
            }
            set
            {
                if (value != null)
                {
                    _secondParameterElement = null;
                }

                _secondElement = value;

                switch (typeof(T).ToString())
                {
                    case "System.Int32":
                        _type = 0;
                        break;

                    case "System.Single":
                        _type = 1;
                        break;

                    case "System.String":
                        _type = 2;
                        break;

                    case "System.Boolean":
                        _type = 3;
                        break;

                    default:
                        _type = 4;
                        break;
                }
            }
        }

        //================================================================================================================

        public override void EnterArgument()
        {
            switch (_type)
            {
                case 0:
                    if (_firstParameterElement != null && SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_firstParameterElement))
                    {
                        _firstElement = (T)(object)SequenceManager.CurrentSequenceManager.IntParameters[_firstParameterElement];
                    }
                    if (_secondParameterElement != null && SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_secondParameterElement))
                    {
                        _secondElement = (T)(object)SequenceManager.CurrentSequenceManager.IntParameters[_secondParameterElement];
                    }
                    break;

                case 1:
                    if (_firstParameterElement != null && SequenceManager.CurrentSequenceManager.FloatParameters.ContainsKey(_firstParameterElement))
                    {
                        _firstElement = (T)(object)SequenceManager.CurrentSequenceManager.FloatParameters[_firstParameterElement];
                    }
                    if (_secondParameterElement != null && SequenceManager.CurrentSequenceManager.FloatParameters.ContainsKey(_secondParameterElement))
                    {
                        _secondElement = (T)(object)SequenceManager.CurrentSequenceManager.FloatParameters[_secondParameterElement];
                    }
                    break;

                case 2:
                    if (_firstParameterElement != null && SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_firstParameterElement))
                    {
                        _firstElement = (T)(object)SequenceManager.CurrentSequenceManager.StringParameters[_firstParameterElement];
                    }
                    if (_secondParameterElement != null && SequenceManager.CurrentSequenceManager.StringParameters.ContainsKey(_secondParameterElement))
                    {
                        _secondElement = (T)(object)SequenceManager.CurrentSequenceManager.StringParameters[_secondParameterElement];
                    }
                    break;

                case 3:
                    if (_firstParameterElement != null && SequenceManager.CurrentSequenceManager.BoolParameters.ContainsKey(_firstParameterElement))
                    {
                        _firstElement = (T)(object)SequenceManager.CurrentSequenceManager.BoolParameters[_firstParameterElement];
                    }
                    if (_secondParameterElement != null && SequenceManager.CurrentSequenceManager.BoolParameters.ContainsKey(_secondParameterElement))
                    {
                        _secondElement = (T)(object)SequenceManager.CurrentSequenceManager.BoolParameters[_secondParameterElement];
                    }
                    break;

                default:
                    Debug.Log("SequenceManager : CompareArgument " + ArgumentName + " : No type has found");
                    break;
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public override IEnumerator Invoke()
        {
            yield return null;
            if (EqualityComparer<T>.Default.Equals(_firstElement, _secondElement))
            {
                SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
            }
            else
            {
                SequenceManager.UpdateTable(TableName, TargetSequenceName[1]);
            }
            yield break;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override void ExitArgument()
        {
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Argument MakeArgument(SequenceManagerElementData elementData)
        {
            switch (elementData.Ints[0])
            {
                case 0:
                    CompareArgument<int> intArgument = new CompareArgument<int>();
                    intArgument._type = 0;

                    if (!elementData.Bools[0])
                    {
                        intArgument._firstElement = elementData.Ints[1];
                    }
                    else
                    {
                        intArgument._firstParameterElement = elementData.Strings[0];
                    }

                    if (!elementData.Bools[1])
                    {
                        intArgument._secondElement = elementData.Ints[2];
                    }
                    else
                    {
                        intArgument._secondParameterElement = elementData.Strings[1];
                    }

                    return intArgument;

                case 1:
                    CompareArgument<float> floatArgument = new CompareArgument<float>();
                    floatArgument._type = 1;

                    if (!elementData.Bools[0])
                    {
                        floatArgument._firstElement = elementData.Floats[0];
                    }
                    else
                    {
                        floatArgument._firstParameterElement = elementData.Strings[0];
                    }

                    if (!elementData.Bools[1])
                    {
                        floatArgument._secondElement = elementData.Floats[1];
                    }
                    else
                    {
                        floatArgument._secondParameterElement = elementData.Strings[1];
                    }

                    return floatArgument;

                case 2:
                    CompareArgument<string> stringArgument = new CompareArgument<string>();
                    stringArgument._type = 2;

                    if (!elementData.Bools[0])
                    {
                        stringArgument._firstElement = elementData.Strings[0];
                    }
                    else
                    {
                        stringArgument._firstParameterElement = elementData.Strings[0];
                    }

                    if (!elementData.Bools[1])
                    {
                        stringArgument._secondElement = elementData.Strings[1];
                    }
                    else
                    {
                        stringArgument._secondParameterElement = elementData.Strings[1];
                    }

                    return stringArgument;

                case 3:
                    CompareArgument<bool> boolArgument = new CompareArgument<bool>();
                    boolArgument._type = 3;

                    if (!elementData.Bools[0])
                    {
                        boolArgument._firstElement = elementData.Bools[2];
                    }
                    else
                    {
                        boolArgument._firstParameterElement = elementData.Strings[0];
                    }

                    if (!elementData.Bools[1])
                    {
                        boolArgument._secondElement = elementData.Bools[3];
                    }
                    else
                    {
                        boolArgument._secondParameterElement = elementData.Strings[1];
                    }

                    return boolArgument;


                default:
                    return null;
            }
        }
    }
}