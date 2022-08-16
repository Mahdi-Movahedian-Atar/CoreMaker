using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Editor
{
    [CreateAssetMenu, Serializable]
    public class ApplicationManagerDefaultAsset : ScriptableObject
    {
        public float Speed = 1;

        public string[] Packages;
    }
}
