using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CM.ApplicationManagement;

namespace CM.DataManagement
{
    public class DataManagerStartUp : MonoBehaviour
    {
        public void Awake()
        {
            PrimaryApplicationStartUp.AddStartUp("DataManager", 0).AddListener(_startUp);
        }

        private void _startUp()
        {
            ApplicationManager.SetApplicationPartState("DataManager", false);

            if (!ReadGameData.ReadDefaultPath())
            {
                WriteGameData.CreateDefaultPath();
            }

            ReadGameData.ReadPlayerProfiles();

            ReadGameData.ReadSaveSlots();

            ApplicationManager.SetApplicationPartState("DataManager", true);
        }
    }
}