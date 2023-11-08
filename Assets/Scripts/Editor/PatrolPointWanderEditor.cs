using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatrolPointWander))]
public class PatrolPointWanderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PatrolPointWander script = (PatrolPointWander)target;

        if (GUILayout.Button("Save Selected Transforms"))
        {
            SaveTransformPositions(script);
        }
    }

    private void SaveTransformPositions(PatrolPointWander script)
    {
        if (Selection.transforms.Length > 0)
        {
            Vector3[] positions = new Vector3[Selection.transforms.Length];
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                positions[i] = Selection.transforms[i].position;
            }
            script.patrolPoints = positions;
            EditorUtility.SetDirty(script);
        }
    }
}
