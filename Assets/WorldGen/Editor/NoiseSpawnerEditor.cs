using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseSpawner), true)]
public class NoseSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NoiseSpawner t = (NoiseSpawner)target;
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Noise Preview", EditorStyles.boldLabel);
        if (t.noiseTexture!= null)
        {
            Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth - 100, EditorGUIUtility.singleLineHeight * 10);
            EditorGUI.DrawTextureTransparent(rect, t.noiseTexture, ScaleMode.ScaleToFit);
        }
    }
}