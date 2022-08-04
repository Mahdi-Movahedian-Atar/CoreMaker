using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CM.DataManagement;
using CM.ApplicationManagement;

namespace CM.InputManagement
{
    [RequireComponent(typeof(InputSystemData),typeof(InputManager))]
    public class InputSystemStartUp : MonoBehaviour
    {
        public void Awake()
        {
            PrimaryApplicationStartUp.AddStartUp("InputSystem", 1).AddListener(_startUp);
        }
        private void _startUp()
        {
            if (!gameObject.TryGetComponent(typeof(InputManager), out _))
            {
                gameObject.AddComponent<InputManager>();
            }

            if (!gameObject.TryGetComponent(typeof(InputSystemData) , out _))
            {
                gameObject.AddComponent<InputSystemData>();
            }
            
            DataManagerEventHandler.LoadSettingData(typeof(InputSystemData));
        }
    }
}