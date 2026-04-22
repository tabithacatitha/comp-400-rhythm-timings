using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;
using System.IO;

public class DataRecorder
{
    private static DataRecorder instance;
    public static DataRecorder Instance {
        get {
            if (instance == null) instance = new();
            return instance;
        }
    }

    private Dataset dataset;

    private DataRecorder()
    {
        LoadFile("Assets/ScriptableObjects/data.asset");
    }

    public void AddData(string tablename, float x, float y)
    {
        List<DataValue> table = dataset.GetOrCreateTable(tablename);

        table.Add(new DataValue(x, y));
        EditorUtility.SetDirty(dataset);
    }

    // data is stored with 2 values, x and y
    private void LoadFile(string filepath)
    {
        dataset = (Dataset) AssetDatabase.LoadAssetAtPath(filepath, typeof(Dataset));
        if (dataset == null) {
            dataset = (Dataset) ScriptableObject.CreateInstance(typeof(Dataset));
            Save();
        };
    }

    private void SaveFile(string filepath)
    {
        AssetDatabase.CreateAsset(dataset, filepath);
    }

    public void Save()
    {
        SaveFile("Assets/ScriptableObjects/data.asset");
    }

    public void OutputToCSV(string tablename)
    {
        List<DataValue> table = dataset.GetTable(tablename);
        if (table == null)
        {
            Debug.LogWarning("failed to fetch "+tablename+" as it does not exist");
            return;
        }
        string[] lines = table.Select(x => x.ToCSVLine()).ToArray();
        File.WriteAllLines("./csv/"+tablename+".csv", lines);
        Debug.Log("written "+tablename+" to csv!");
    }
}


// using this class as a reference (this is my code from a different project)
// public class DefenseSplineEditor : Editor
// {
//     // from https://discussions.unity.com/t/create-a-button-in-the-inspector/22432/2 and also whatever I did for star3-valley.CellEditor
//     public override void OnInspectorGUI() {
//         DefenseSpline defenseSpline = target as DefenseSpline;

//         GUILayout.FlexibleSpace();
//         GUILayout.BeginVertical();
//         defenseSpline.Filename = EditorGUILayout.TextField("File Name",defenseSpline.Filename);
//         string filepath = DefenseSpline.path + "/" + defenseSpline.Filename + ".asset";
//         if(GUILayout.Button("Load Spline From Asset") && IsFilenameValid(defenseSpline.Filename))
//         {
//             Undo.RecordObject(defenseSpline.DefensePath, "Load Spline From Asset");
//             defenseSpline.DefensePath = (DefensePath) AssetDatabase.LoadAssetAtPath(filepath, typeof(DefensePath));
//             defenseSpline.PathToSpline();
//         }
//         GUILayout.FlexibleSpace();
//         if(GUILayout.Button("Save Spline To Asset") && IsFilenameValid(defenseSpline.Filename))
//         {
//             if (defenseSpline.DefensePath == null) {
//                 defenseSpline.DefensePath = (DefensePath)CreateInstance(typeof(DefensePath));
//                 defenseSpline.DefensePath.name = "Temporary DefensePath";
//             }
//             defenseSpline.SplineToPath();
//             AssetDatabase.CreateAsset(defenseSpline.DefensePath, filepath);
//             Debug.Log(string.Format("Asset {0} created or modified",filepath));
//         }
//         GUILayout.EndVertical();
//         GUILayout.FlexibleSpace();
//         DrawDefaultInspector();
//    }

//    private bool IsFilenameValid(string filename)
//     {
//         if (filename == "") {
//             Debug.LogError("Could not save asset to empty defenseSpline.Filename.");
//             return false;
//         }
//         return true;
//     }
// }