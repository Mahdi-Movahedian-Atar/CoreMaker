using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using CM.ApplicationManagement;
using CM.DataManagement;
using CM.InputManagement;
using UnityEngine;

public class Test4 : MonoBehaviour
{

    public BucketMassage BucketMassagea;
    void Start()
    {
        // DataManagerEventHandler.SaveSettingData("InputSystem");

        // DataManagerEventHandler.LoadSettingData();

        /*BucketSystem.SubscribeToBucket("a", "b", null, 2).AddListener(sssd);

        InputManager.CurrentInputManager.GetEvent("a", "a").AddListener(ddd);

        InputManager.CurrentInputManager.GetEvent("a", "b").AddListener(ddd);

        InputManager.CurrentInputManager.ChangeCurrentInputTable("a");

        InputManager.CurrentInputManager.ChangeKey("a", "a", CMKeyCode.Mouse1);

        InputManager.CurrentInputManager.ChangeMode("a", "a", KeyMode.Hold);*/
       
    }

    void Update()
    {

    }

    private void sssd(BucketMassage bucketMassage)
    {
        print(BucketMassagea.Actor + BucketMassagea.Act + BucketMassagea.Acted + BucketMassagea.ActCount + BucketMassagea.ToString());
    }

    private void ddd()
    {
        BucketSystem.TrowToBucket(BucketMassagea);
    }

}