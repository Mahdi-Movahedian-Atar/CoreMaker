using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CM.ApplicationManagement;
using CM.DataManagement;
using CM.InputManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class Test2 : MonoBehaviour
{
    private void Start()
    {
        /*var a1 = new WaitForBucketArgument();
        a1.ArgumentName = "a1";
        a1.TableName = "t1";
        a1.TargetSequenceName = new[] {"s1"};
        a1.ParameterActorFilter = "sk1";
        a1.ParameterActFilter = "sk1";
        a1.ActedFilter = "sk1";
        a1.ParameterActCountFilter = "sk1";
        a1.EventName = "sk1";

        SequenceManager.IntParameters.Add("sk1", 3);
        SequenceManager.StringParameters.Add("sk1", "sk1");

        var s1 = new PrintSequence();
        s1.Message =
            "++++++++++++++++------------------------++++++++++++++++++++++++++-----------------------========================";
        s1.SequenceName = "s1";
        s1.TableName = "t1";
        s1.TargetArgumentName = "a2";

        var a2 = new WaitForSecondsArgument();
        a2.seconds = 10;
        a2.ArgumentName = "a2";
        a2.TableName = "t1";
        a2.TargetSequenceName = new[] {"s2"};

        var s2 = new ChangeStringSequence();
        s2.Parameter = "sk2";
        s2.ParameterName = "sk1";
        s2.SequenceName = "s2";
        s2.TableName = "t1";
        s2.TargetArgumentName = "a1";

        var t1 = new SequenceTable();

        t1.Arguments = new Dictionary<string, Argument>();
        t1.Sequences = new Dictionary<string, Sequence>();

        t1.Arguments.Add("a1", a1);
        t1.Arguments.Add("a2", a2);

        t1.Sequences.Add("s1", s1);
        t1.Sequences.Add("s2", s2);

        t1.CurrentArgumentName = "a1";
        t1.IsTableActive = true;
        t1.TableName = "t1";

        SequenceManager.SequenceTables = new Dictionary<string, SequenceTable>();

        SequenceManager.SequenceTables.Add("t1", t1);

        SequenceManager.StartAllCurrentArguments();

        StartCoroutine(sss());
    }

    private IEnumerator sss()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            BucketSystem.TrowToBucket(new BucketMassage() {Act = "sk1", Acted = "sk1", Actor = "sk1", ActCount = 3});
        }*/
    }
}