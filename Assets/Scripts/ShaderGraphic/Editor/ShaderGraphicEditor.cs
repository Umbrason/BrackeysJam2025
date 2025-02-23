using System.Reflection;
using UnityEditor;

//[CustomEditor(typeof(ShaderGraphic))]
[CanEditMultipleObjects]
public class ShaderGraphicEditor : Editor
{
    void OnEnable()
    { 

    }
    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
