using System.Collections;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(CompareNumbersArgument), SequenceType.Argument)]
    public class CompareNumbersArgument : Argument
    {
        public enum ComparisonTypes
        {
            Equal,
            Greater,
            Lesser,
            GreaterEqual,
            LesserEqual
        }

        //================================================================================================================

        public ComparisonTypes Types;
        //----------------------------------------------------------------------------------------------------------------
        private string _firstParameterNumber;
        private float _firstNumber;
        private string _secondParameterNumber;
        private float _secondNumber;
        //----------------------------------------------------------------------------------------------------------------
        public string FirstParameterNumber
        {
            get
            {
                return _firstParameterNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _firstNumber = 0;
                }

                _firstParameterNumber = value;
            }
        }
        public float FirstNumber
        {
            get
            {
                return _firstNumber;
            }
            set
            {
                if (value != 0)
                {
                    _firstParameterNumber = null;
                }

                _firstNumber = value;
            }
        }
        public string SecondParameterNumber
        {
            get
            {
                return _secondParameterNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _secondNumber = 0;
                }

                _secondParameterNumber = value;
            }
        }
        public float SecondNumber
        {
            get
            {
                return _secondNumber;
            }
            set
            {
                if (value != 0)
                {
                    _secondParameterNumber = null;
                }

                _secondNumber = value;
            }
        }

        //================================================================================================================

        public override void EnterArgument()
        {
            if (!string.IsNullOrEmpty(_firstParameterNumber))
            {
                if (SequenceManager.CurrentSequenceManager.FloatParameters.ContainsKey(_firstParameterNumber))
                {
                    _firstNumber = SequenceManager.CurrentSequenceManager.FloatParameters[_firstParameterNumber];
                }
                else
                {
                    if (SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_firstParameterNumber))
                    {
                        _firstNumber = SequenceManager.CurrentSequenceManager.IntParameters[_firstParameterNumber];
                    }
                }
            }

            if (!string.IsNullOrEmpty(_secondParameterNumber))
            {
                if (SequenceManager.CurrentSequenceManager.FloatParameters.ContainsKey(_secondParameterNumber))
                {
                    _secondNumber = SequenceManager.CurrentSequenceManager.FloatParameters[_secondParameterNumber];
                }
                else
                {
                    if (SequenceManager.CurrentSequenceManager.IntParameters.ContainsKey(_secondParameterNumber))
                    {
                        _secondNumber = SequenceManager.CurrentSequenceManager.IntParameters[_secondParameterNumber];
                    }
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------
        public override IEnumerator Invoke()
        {
            switch (Types)
            {
                case ComparisonTypes.Equal:
                    yield return null;
                    if (_firstNumber == _secondNumber)
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
                    }
                    else
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[1]);
                    }
                    break;

                case ComparisonTypes.Greater:
                    yield return null;
                    if (_firstNumber > _secondNumber)
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
                    }
                    else
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[1]);
                    }
                    break;

                case ComparisonTypes.Lesser:
                    yield return null;
                    if (_firstNumber < _secondNumber)
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
                    }
                    else
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[1]);
                    }
                    break;

                case ComparisonTypes.GreaterEqual:
                    yield return null;
                    if (_firstNumber >= _secondNumber)
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
                    }
                    else
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[1]);
                    }
                    break;

                case ComparisonTypes.LesserEqual:
                    yield return null;
                    if (_firstNumber <= _secondNumber)
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
                    }
                    else
                    {
                        SequenceManager.UpdateTable(TableName, TargetSequenceName[1]);
                    }
                    break;
            }
            yield return null;
        }
        //----------------------------------------------------------------------------------------------------------------
        public override void ExitArgument()
        {
        }
        //----------------------------------------------------------------------------------------------------------------
        public override Argument MakeArgument(SequenceManagerElementData elementData)
        {
            CompareNumbersArgument compareNumbersArgument = new CompareNumbersArgument();

            if (elementData.Bools[0]) compareNumbersArgument._firstParameterNumber = elementData.Strings[0];
            else compareNumbersArgument._firstNumber = elementData.Floats[0];

            if (elementData.Bools[1]) compareNumbersArgument._firstParameterNumber = elementData.Strings[1];
            else compareNumbersArgument._firstNumber = elementData.Floats[1];

            compareNumbersArgument.Types = (ComparisonTypes)elementData.Ints[0];

            return compareNumbersArgument;
        }
    }
}  