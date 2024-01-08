using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGen))]
public class WorldGenEditor : Editor
{
    private Texture2D turfPreview = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldGen t = (WorldGen)target;
        if (GUILayout.Button("Generate World"))
        {
            t.GenerateWorld();
            turfPreview = new Texture2D(t.MapSize.x, t.MapSize.y);
            for (int y = 0; y < t.MapSize.y; y++)
            {
                for (int x = 0; x < t.MapSize.x; x++)
                {
                    Color turfPreviewColor = t.TurfGrid[x, y].PreviewColor;
                    turfPreview.SetPixel(x, y, turfPreviewColor);
                }
            }
            turfPreview.Apply();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Turf Preview", EditorStyles.boldLabel);
        if (turfPreview != null)
        {
            Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth - 100, EditorGUIUtility.singleLineHeight * 10);
            EditorGUI.DrawTextureTransparent(rect, turfPreview, ScaleMode.ScaleToFit);
        }
    }
}