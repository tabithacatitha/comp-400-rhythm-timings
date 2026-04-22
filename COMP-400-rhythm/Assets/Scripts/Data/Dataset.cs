using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dataset : ScriptableObject
{
    [SerializeField] public List<string> keys = new();
    // public Dictionary<string, List<DataValue>> data = new();
    [SerializeField] public List<List<int>> test = new();
    [SerializeField] public List<DataValueList> data = new();

    public List<DataValue> GetTable(string tablename)
    {
        int index = keys.IndexOf(tablename);
        if (index == -1) return null;
        return data[index].list;
    }

    public List<DataValue> GetOrCreateTable(string tablename)
    {
        List<DataValue> table = GetTable(tablename);
        if (table == null)
        {
            keys.Add(tablename);
            DataValueList dataValueList = new DataValueList();
            table = dataValueList.list;
            data.Add(dataValueList);
        }

        return table;
    }
}