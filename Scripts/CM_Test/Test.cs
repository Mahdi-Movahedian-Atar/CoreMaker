using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using CM.DataManagement;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Test 
{
    public IEnumerator SssEnumerator()
    {
        while (true)
        {
            Debug.Log("________________________W__________________________________________________________________________________________");

            yield return null;
        }
    }
}