using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace CM.InputManagement
{

    public enum ControllerMode
    {
        KeyBoard,
        XBox,
        Ps
    }

    //================================================================================================================

    public enum AxesType
    {
        FirstAxes,
        SecondAxes,
    }

    //================================================================================================================

    public class InputManager : MonoBehaviour
    {
        #region Singleton

        private static InputManager _currentInputManager;

        public static InputManager CurrentInputManager
        {
            get
            {
                if (_currentInputManager == null)
                {
                    _currentInputManager = new InputManager();
                }

                return _currentInputManager;
            }
        }

        #endregion

        #region InputTables

        public InputTable[] InputTables;

        public string[] InputTableNames;

        #endregion

        #region KeyCodesIndexes

        private int[] _downKeyCodeIndex;
        private int[] _holdKeyCodeIndex;
        private int[] _upKeyCodeIndex;

        #endregion
        //------------------------------------------------------------------------------------------------------------
        public int CurrentInputTableIndex;

        private ControllerMode[] _controllersModes;

        private ControllerMode _currentControllerMode;
        //============================================================================================================
        void Awake()
        {
            _currentInputManager = this;

            _controllersModes = new ControllerMode[0];

            _currentControllerMode = ControllerMode.KeyBoard;

            CurrentInputTableIndex = 0;

            SetKeys();
        }
        //------------------------------------------------------------------------------------------------------------
        void Start()
        {
        }
        //------------------------------------------------------------------------------------------------------------
        void Update()
        {
            InputTable currentInputTable = InputTables[CurrentInputTableIndex];

            //Set events----------------------------------------------------------------------------------------------

            switch (_currentControllerMode)
            {
                //For key board---------------------------------------------------------------------------------------
                case ControllerMode.KeyBoard:
                    //Set axes----------------------------------------------------------------------------------------
                    if (!currentInputTable.FirstXInvert)
                    {

                        if (!currentInputTable.FirstYInvert)
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstKeyBoardXAxes") * currentInputTable.FirstXSensitivity, Input.GetAxis("FirstKeyBoardYAxes") * currentInputTable.FirstYSensitivity);
                        }
                        else
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstKeyBoardXAxes") * currentInputTable.FirstXSensitivity, Input.GetAxis("FirstKeyBoardYAxes") * currentInputTable.FirstYSensitivity * -1);
                        }
                    }
                    else
                    {
                        if (!currentInputTable.FirstYInvert)
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstKeyBoardXAxes") * currentInputTable.FirstXSensitivity * -1, Input.GetAxis("FirstKeyBoardYAxes") * currentInputTable.FirstYSensitivity);
                        }
                        else
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstKeyBoardXAxes") * currentInputTable.FirstXSensitivity * -1, Input.GetAxis("FirstKeyBoardYAxes") * currentInputTable.FirstYSensitivity * -1);
                        }
                    }

                    if (!currentInputTable.SecondXInvert)
                    {
                        if (!currentInputTable.SecondYInvert)
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondKeyBoardXAxes") * currentInputTable.SecondXSensitivity, Input.GetAxis("SecondKeyBoardYAxes") * currentInputTable.SecondYSensitivity);
                        }
                        else
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondKeyBoardXAxes") * currentInputTable.SecondXSensitivity, Input.GetAxis("SecondKeyBoardYAxes") * currentInputTable.SecondYSensitivity * -1);
                        }
                    }
                    else
                    {
                        if (!currentInputTable.SecondYInvert)
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondKeyBoardXAxes") * currentInputTable.SecondXSensitivity * -1, Input.GetAxis("SecondKeyBoardYAxes") * currentInputTable.SecondYSensitivity);
                        }
                        else
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondKeyBoardXAxes") * currentInputTable.SecondXSensitivity * -1, Input.GetAxis("SecondKeyBoardYAxes") * currentInputTable.SecondYSensitivity * -1);
                        }
                    }
                    //Set keys----------------------------------------------------------------------------------------
                    foreach (var index in _holdKeyCodeIndex)
                    {
                        if (GetKey(currentInputTable.KeyBoardKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                        }
                    }

                    foreach (var index in _downKeyCodeIndex)
                    {
                        if (GetKeyDown(currentInputTable.KeyBoardKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                            return;
                        }
                    }

                    foreach (var index in _upKeyCodeIndex)
                    {
                        if (GetKeyUp(currentInputTable.KeyBoardKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                            return;
                        }
                    }

                    break;

                //For xBox--------------------------------------------------------------------------------------------
                case ControllerMode.XBox:
                    //Set axes----------------------------------------------------------------------------------------
                    if (!currentInputTable.FirstXInvert)
                    {
                        if (!currentInputTable.FirstYInvert)
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity);
                        }
                        else
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity * -1);
                        }
                    }
                    else
                    {
                        if (!currentInputTable.FirstYInvert)
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity * -1, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity);
                        }
                        else
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity * -1, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity * -1);
                        }
                    }

                    if (!currentInputTable.SecondXInvert)
                    {
                        if (!currentInputTable.SecondYInvert)
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity);
                        }
                        else
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity * -1);
                        }
                    }
                    else
                    {
                        if (!currentInputTable.SecondYInvert)
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity * -1, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity);
                        }
                        else
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity * -1, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity * -1);
                        }
                    }
                    //Set keys----------------------------------------------------------------------------------------
                    foreach (var index in _holdKeyCodeIndex)
                    {
                        if (GetKey(currentInputTable.XBoxKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                        }
                    }

                    foreach (var index in _downKeyCodeIndex)
                    {
                        if (GetKeyDown(currentInputTable.XBoxKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                            return;
                        }
                    }

                    foreach (var index in _upKeyCodeIndex)
                    {
                        if (GetKeyUp(currentInputTable.XBoxKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                            return;
                        }
                    }

                    break;

                //For ps4---------------------------------------------------------------------------------------------
                case ControllerMode.Ps:
                    //Set axes----------------------------------------------------------------------------------------
                    if (!currentInputTable.FirstXInvert)
                    {
                        if (!currentInputTable.FirstYInvert)
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity);
                        }
                        else
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity * -1);
                        }
                    }
                    else
                    {
                        if (!currentInputTable.FirstYInvert)
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity * -1, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity);
                        }
                        else
                        {
                            currentInputTable.FirstAxesEvent.Invoke(Input.GetAxis("FirstJoyStickXAxes") * currentInputTable.FirstXSensitivity * -1, Input.GetAxis("FirstJoyStickYAxes") * currentInputTable.FirstYSensitivity * -1);
                        }
                    }

                    if (!currentInputTable.SecondXInvert)
                    {
                        if (!currentInputTable.SecondYInvert)
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity);
                        }
                        else
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity * -1);
                        }
                    }
                    else
                    {
                        if (!currentInputTable.SecondYInvert)
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity * -1, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity);
                        }
                        else
                        {
                            currentInputTable.SecondAxesEvent.Invoke(Input.GetAxis("SecondJoyStickXAxes") * currentInputTable.SecondXSensitivity * -1, Input.GetAxis("SecondJoyStickYAxes") * currentInputTable.SecondYSensitivity * -1);
                        }
                    }
                    //Set keys----------------------------------------------------------------------------------------
                    foreach (var index in _holdKeyCodeIndex)
                    {
                        if (GetKey(currentInputTable.PsKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                        }
                    }

                    foreach (var index in _downKeyCodeIndex)
                    {
                        if (GetKeyDown(currentInputTable.PsKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                            return;
                        }
                    }

                    foreach (var index in _upKeyCodeIndex)
                    {
                        if (GetKeyUp(currentInputTable.PsKeyCode[index]))
                        {
                            currentInputTable.KeyCodeEvent[index].Invoke();
                            return;
                        }
                    }

                    break;

            }

            //Look if controller has changed--------------------------------------------------------------------------

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _currentControllerMode = ControllerMode.KeyBoard;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    _currentControllerMode = _controllersModes[0];
                }

                if (Input.GetKeyDown(KeyCode.Joystick2Button0))
                {
                    _currentControllerMode = _controllersModes[1];
                }

                if (Input.GetKeyDown(KeyCode.Joystick3Button0))
                {
                    _currentControllerMode = _controllersModes[2];
                }

                if (Input.GetKeyDown(KeyCode.Joystick4Button0))
                {
                    _currentControllerMode = _controllersModes[3];
                }
            }

            //Look if a controller has added--------------------------------------------------------------------------

            string[] joysticks = Input.GetJoystickNames();

            if (joysticks.Length != _controllersModes.Length)
            {
                _controllersModes = new ControllerMode[joysticks.Length];

                for (int i = 0; i < joysticks.Length; i++)
                {
                    if (joysticks[i].ToLower().Contains("play"))
                    {
                        _controllersModes[i] = ControllerMode.Ps;
                    }
                    else
                    {
                        _controllersModes[i] = ControllerMode.XBox;
                    }
                }
            }
        }
        //============================================================================================================
        public static bool GetKey(CMKeyCode keyCode)
        {
            float scroll = Input.GetAxis("Scroll");

            if (keyCode == CMKeyCode.ScrollUp)
            {
                if (scroll > 0)
                {
                    return true;
                }
                return false;
            }

            if (keyCode == CMKeyCode.ScrollDown)
            {
                if (scroll < 0)
                {
                    return true;
                }
                return false;
            }
            //--------------------------------------------------------------------------------------------------------
            float joyStick1 = Input.GetAxis("JoyStickKeys1");

            if (keyCode == CMKeyCode.JoystickButton20)
            {
                if (joyStick1 > 1)
                {
                    return true;
                }
                return false;
            }

            if (keyCode == CMKeyCode.JoystickButton21)
            {
                if (joyStick1 < 1)
                {
                    return true;
                }
                return false;
            }
            //--------------------------------------------------------------------------------------------------------
            float joyStick2 = Input.GetAxis("JoyStickKeys2");

            if (keyCode == CMKeyCode.JoystickButton22)
            {
                if (joyStick2 > 1)
                {
                    return true;
                }
                return false;
            }

            if (keyCode == CMKeyCode.JoystickButton23)
            {
                if (joyStick2 < 1)
                {
                    return true;
                }
                return false;
            }
            //--------------------------------------------------------------------------------------------------------
            float joyStick3 = Input.GetAxis("JoyStickKeys3");

            if (keyCode == CMKeyCode.JoystickButton24)
            {
                if (joyStick3 > 1)
                {
                    return true;
                }
                return false;
            }

            if (keyCode == CMKeyCode.JoystickButton25)
            {
                if (joyStick3 < 1)
                {
                    return true;
                }
                return false;
            }
            //--------------------------------------------------------------------------------------------------------
            return Input.GetKey((KeyCode)keyCode.GetHashCode());
        }
        //------------------------------------------------------------------------------------------------------------
        public static bool GetKeyDown(CMKeyCode keyCode)
        {
            if (keyCode == CMKeyCode.ScrollUp || keyCode == CMKeyCode.ScrollDown || keyCode == CMKeyCode.JoystickButton20 || keyCode == CMKeyCode.JoystickButton21 ||
                keyCode == CMKeyCode.JoystickButton22 || keyCode == CMKeyCode.JoystickButton23 || keyCode == CMKeyCode.JoystickButton24 || keyCode == CMKeyCode.JoystickButton25)
            {
                return false;
            }
            //--------------------------------------------------------------------------------------------------------
            return Input.GetKeyDown((KeyCode)keyCode.GetHashCode());
        }
        //------------------------------------------------------------------------------------------------------------
        public static bool GetKeyUp(CMKeyCode keyCode)
        {
            if (keyCode == CMKeyCode.ScrollUp || keyCode == CMKeyCode.ScrollDown || keyCode == CMKeyCode.JoystickButton20 || keyCode == CMKeyCode.JoystickButton21 ||
                keyCode == CMKeyCode.JoystickButton22 || keyCode == CMKeyCode.JoystickButton23 || keyCode == CMKeyCode.JoystickButton24 || keyCode == CMKeyCode.JoystickButton25)
            {
                return false;
            }
            //--------------------------------------------------------------------------------------------------------
            return Input.GetKeyUp((KeyCode)keyCode.GetHashCode());
        }
        //============================================================================================================
        public UnityEvent GetEvent(string inputTableName, string keyName)
        {
            //Search for table----------------------------------------------------------------------------------------
            for (int i = 0; i <= InputTableNames.Length; i++)
            {
                if (InputTableNames[i] == inputTableName)
                {
                    InputTable inputTable = InputTables[i];
                    //Search for key name-----------------------------------------------------------------------------
                    for (int j = 0; j < inputTable.KeyNames.Length; j++)
                    {
                        if (inputTable.KeyNames[j] == keyName)
                        {
                            return InputTables[i].KeyCodeEvent[j];
                        }
                    }
                    //If it failed------------------------------------------------------------------------------------
                    Debug.LogError("InputManager : " + "Failed to find key codes event");

                    return null;
                }
            }
            //If it failed--------------------------------------------------------------------------------------------
            Debug.LogError("InputManager : " + "Failed to find input table");

            return null;
        }
        //------------------------------------------------------------------------------------------------------------
        public UnityEvent<float, float> GetAxesEvent(string inputTableName, AxesType axesType)
        {
            //Search for table----------------------------------------------------------------------------------------
            for (int i = 0; i <= inputTableName.Length; i++)
            {
                if (inputTableName == InputTableNames[i])
                {
                    //Search for axes types---------------------------------------------------------------------------
                    switch (axesType)
                    {
                        case AxesType.FirstAxes:
                            return InputTables[CurrentInputTableIndex].FirstAxesEvent;

                        case AxesType.SecondAxes:
                            return InputTables[CurrentInputTableIndex].SecondAxesEvent;
                    }
                }
            }
            //If it failed--------------------------------------------------------------------------------------------

            Debug.LogError("InputManager : " + "Failed to find input table");

            return null;
        }
        //------------------------------------------------------------------------------------------------------------
        public void ChangeCurrentInputTable(string inputTableName)
        {
            for (int i = 0; i < InputTableNames.Length; i++)
            {
                if (InputTableNames[i] == inputTableName)
                {
                    CurrentInputTableIndex = i;

                    SetKeys();

                    return;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------
        public void ChangeKey(string inputTableName, string keyName, CMKeyCode keyCode)
        {
            //Search for table----------------------------------------------------------------------------------------
            for (int i = 0; i < InputTableNames.Length; i++)
            {
                if (InputTableNames[i] == inputTableName)
                {
                    InputTable inputTable = InputTables[i];
                    //Search for key name-----------------------------------------------------------------------------
                    for (int j = 0; j < inputTable.KeyNames.Length; j++)
                    {
                        if (inputTable.KeyNames[j] == keyName)
                        {
                            if (!keyCode.ToString().ToLower().Contains("joystick"))
                            {
                                InputTables[i].KeyBoardKeyCode[j] = keyCode;

                                Debug.Log("InputManager : " + "Keyboard key code has changed");

                                return;
                            }

                            if (_currentControllerMode == ControllerMode.XBox)
                            {
                                InputTables[i].XBoxKeyCode[j] = keyCode;

                                Debug.Log("InputManager : " + "Xbox key code has changed");

                                return;
                            }

                            if (_currentControllerMode == ControllerMode.Ps)
                            {
                                InputTables[i].PsKeyCode[j] = keyCode;

                                Debug.Log("InputManager : " + "Ps key code has changed");

                                return;
                            }

                            return;
                        }
                    }

                    //If it failed------------------------------------------------------------------------------------
                    Debug.LogError("InputManager : " + "Failed to find key code");
                }
            }
            Debug.LogError("InputManager : " + "Failed to find input table");
        }
        //------------------------------------------------------------------------------------------------------------
        public void ChangeMode(string inputTableName, string keyName, KeyMode keyMode)
        {
            //Search for table----------------------------------------------------------------------------------------
            for (int i = 0; i < InputTableNames.Length; i++)
            {
                if (InputTableNames[i] == inputTableName)
                {
                    InputTable inputTable = InputTables[i];
                    //Search for key name-----------------------------------------------------------------------------
                    for (int j = 0; j < inputTable.KeyNames.Length; j++)
                    {
                        if (inputTable.KeyNames[j] == keyName)
                        {
                            {
                                InputTables[i].KeyModes[j] = keyMode;

                                Debug.Log("InputManager : " + "Key mode has changed");

                                return;
                            }
                        }
                    }
                    //If it failed------------------------------------------------------------------------------------
                    Debug.LogError("InputManager : " + "Failed to find key code");

                    return;
                }
            }
            Debug.LogError("InputManager : " + "Failed to find input table");
        }
        //------------------------------------------------------------------------------------------------------------
        public void SetKeys()
        {
            List<int> downKeyCodes = new List<int>();
            List<int> holdKeyCodes = new List<int>();
            List<int> upKeyCodes = new List<int>();
            
            for (int i = 0; i < InputTables[CurrentInputTableIndex].KeyModes.Length; i++)
            {
                switch (InputTables[CurrentInputTableIndex].KeyModes[i])
                {
                    case KeyMode.Down:
                        downKeyCodes.Add(i);
                        break;

                    case KeyMode.Hold:
                        holdKeyCodes.Add(i);
                        break;

                    case KeyMode.Up:
                        upKeyCodes.Add(i);
                        break;
                }
            }

            _downKeyCodeIndex = downKeyCodes.ToArray();
            _holdKeyCodeIndex = holdKeyCodes.ToArray();
            _upKeyCodeIndex = upKeyCodes.ToArray();
        }
    }
}