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
            PrimaryApplicationStartUp.AddStartUp("CM.DataManagement").AddListener(_startUp);
        }

        private void _startUp()
        {
            ApplicationManager.SetApplicationPackageState("CM.DataManagement", false);

            if (!ReadGameData.ReadDefaultPath())
            {
                WriteGameData.CreateDefaultPath();
            }

            ReadGameData.ReadPlayerProfiles();

            ReadGameData.ReadSaveSlots();

            ApplicationManager.SetApplicationPackageState("CM.DataManagement", true);
        }
    }
}