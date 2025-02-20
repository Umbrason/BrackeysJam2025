using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

[CustomEditor(typeof(Attractee))]
public class AttracteeEditor : Editor
{
    SerializedProperty layerMask;

    void OnEnable()
    {
        layerMask = serializedObject.FindProperty("attractionLayerMask");
    }

    enum Layers
    {
        None = 0,
        Zero = 1 << 0,
        One = 1 << 1,
        Two = 1 << 2,
        Three = 1 << 3,
        Four = 1 << 4,
        Five = 1 << 5,
        Six = 1 << 6,
        Seven = 1 << 7
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mask = layerMask.intValue;
        mask = (int)(object)EditorGUILayout.EnumFlagsField((Layers)mask);
        layerMask.intValue = mask;
        serializedObject.ApplyModifiedProperties();
    }
}
