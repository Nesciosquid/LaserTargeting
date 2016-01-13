using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LaserHolderController))]
public class LaserHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LaserHolderController myHolder = (LaserHolderController)target;
        if (GUILayout.Button("Scan"))
        {
            myHolder.Scan();
        }
        BuildGameObjectList(myHolder);
    }

    private void BuildGameObjectList(LaserHolderController holder)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Detected Objects");
        foreach (var targetObject in holder.GetDetectedObjects().Keys)
        {
            var name = targetObject.name;
            if (GUILayout.Button(name))
            {
                holder.PointAtGameObject(targetObject);
            }
        }
        GUILayout.EndVertical();
    }
}
