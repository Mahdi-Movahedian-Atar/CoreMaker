using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncTest<T> : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FuncTest(){}

    public FuncTest(out T a)
    {
        a = (T) new object();
    }
}

public class a
{
    public FuncTest<int> func;
}