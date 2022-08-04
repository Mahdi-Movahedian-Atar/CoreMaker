using System.Collections;
using System.Collections.Generic;
using CM.ApplicationManagement;
using UnityEngine;

public class SM_Test : MonoBehaviour
{
    private bool sss = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sss)
        {
            _ss();
            sss = false;
        }
    }

    private void _ss()
    {
        SequenceManager.StartAllCurrentArguments();
    }
}
