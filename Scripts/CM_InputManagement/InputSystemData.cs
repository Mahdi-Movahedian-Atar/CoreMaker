using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CM.DataManagement;
using CM.ApplicationManagement;
using UnityEngine;
using UnityEngine.Events;

namespace CM.InputManagement
{
    public class InputSystemData : DataManagerSettingMainObject, IDataManagerMainObject
    {
        private static bool _isSaved;
        private static SaveObject _thisSaveObject;

        #region Interface

        public string SaveName => "InputSetting";

        public bool IsSaved
        {
            get { return _isSaved; }
        }

        public SaveObject ThisSaveObject
        {
            get { return _thisSaveObject; }
        }

        #endregion

        public static InputTable[] DefaultInputTables;
        public static InputTable[] CurrentInputTables;

        public static InputTable SafeTable;
        //============================================================================================================
        protected override void OnSettingSave()
        {
            base.OnSettingSave();
            ApplicationManager.SetApplicationPackageState("CM.InputManagement", false);
            //Try to load data------------------------------------------------------------------------------------
            try
            {
                if (DefaultInputTables == null)
                {
                    DefaultInputTables = InputManager.CurrentInputManager.InputTables;
                    SafeTable = DefaultInputTables[0];
                }

                CurrentInputTables = InputManager.CurrentInputManager.InputTables;

                _thisSaveObject = new SaveObject();

                ThisSaveObject.AddArray(new object[1] { CurrentInputTables });

                if (WriteGameData.SaveSettingObject(ThisSaveObject, SaveName, CreateBinaryFormatter()))
                {
                    _isSaved = true;
                    ApplicationManager.SetApplicationPackageState("CM.InputManagement", true);
                    return;
                }
            }
            //If it failed---------------------------------------------------------------------------------------
            catch (Exception message)
            {
                Debug.LogWarning("InputManager : " + "Failed to save input data" + Environment.NewLine + message);

                throw;
            }

            ApplicationManager.SetApplicationPackageState("CM.InputManagement", true);

            _isSaved = false;
        }
        //------------------------------------------------------------------------------------------------------------
        protected override void OnSettingLoad()
        {
            base.OnSettingLoad();

            if (DefaultInputTables == null)
            {
                DefaultInputTables = InputManager.CurrentInputManager.InputTables;
                SafeTable = DefaultInputTables[0];
            }

            //Check if data is changed-----------------------------------------------------------------------------

            ApplicationManager.SetApplicationPackageState("CM.InputManagement", false);

            CurrentInputTables = InputManager.CurrentInputManager.InputTables;

            if (!IsSaved)
            {
                object saveObject = new object();

                //Try to load data---------------------------------------------------------------------------------
                try
                {
                    ReadGameData.LoadSettingObject(ref saveObject, SaveName, CreateBinaryFormatter());

                    _thisSaveObject = (SaveObject)saveObject;

                    CurrentInputTables = (InputTable[])ThisSaveObject.GetArray()[0];

                    for (int i = 0; i < DefaultInputTables.Length; i++)
                    {
                        CurrentInputTables[i].KeyNames = DefaultInputTables[i].KeyNames;

                        CurrentInputTables[i].KeyCodeEvent = DefaultInputTables[i].KeyCodeEvent;

                        CurrentInputTables[i].FirstAxesEvent = DefaultInputTables[i].FirstAxesEvent;
                        CurrentInputTables[i].SecondAxesEvent = DefaultInputTables[i].SecondAxesEvent;
                    }

                    //Finalize-------------------------------------------------------------------------------------
                    InputManager.CurrentInputManager.InputTables = CurrentInputTables;

                    InputManager.CurrentInputManager.InputTables[0] = SafeTable;

                    ApplicationManager.SetApplicationPackageState("CM.InputManagement", true);

                    _isSaved = false;
                }
                //If it failed------------------------------------------------------------------------------------
                catch (Exception message)
                {
                    Debug.LogWarning("InputManager : " + "Failed to load input data" + Environment.NewLine + message);

                    InputManager.CurrentInputManager.InputTables = DefaultInputTables;

                    OnSettingSave();

                    throw;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        protected override void OnSettingReset()
        {
            base.OnSettingReset();

            ApplicationManager.SetApplicationPackageState("CM.InputManagement", false);

            if (DefaultInputTables == null)
            {
                DefaultInputTables = InputManager.CurrentInputManager.InputTables;
                SafeTable = DefaultInputTables[0];
            }

            InputManager.CurrentInputManager.InputTables = DefaultInputTables;

            InputManager.CurrentInputManager.InputTables[0] = SafeTable;

            OnSettingSave();

            ApplicationManager.SetApplicationPackageState("InputSystem", true);
        }
        //============================================================================================================
        public BinaryFormatter CreateBinaryFormatter()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            SurrogateSelector surrogateSelector = new SurrogateSelector();

            InputTableSerializationSurrogate inputTableSerializationSurrogate = new InputTableSerializationSurrogate();

            surrogateSelector.AddSurrogate(typeof(InputTable), new StreamingContext(StreamingContextStates.All), inputTableSerializationSurrogate);

            binaryFormatter.SurrogateSelector = surrogateSelector;

            return binaryFormatter;
        }
    }
}