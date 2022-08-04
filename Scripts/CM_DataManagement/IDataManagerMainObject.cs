using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace CM.DataManagement
{
    public interface IDataManagerMainObject
    {
        string SaveName { get; }

        bool IsSaved { get; }

        SaveObject ThisSaveObject { get; }
        
        BinaryFormatter CreateBinaryFormatter();
    }
}