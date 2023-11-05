using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PerlinNoiseTest))]
public class PerlinNoiseTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PerlinNoiseTest pnt = (PerlinNoiseTest)target;

        if (GUILayout.Button("Generate Map"))
        {
            pnt.SetTexture(true);
        }

    }


}
