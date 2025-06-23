using UnityEngine;

public static class VisualEffects
{
    public static void Create3DText(string text, Vector3 position, Color color,float duration, Quaternion rotation = default)
    {
        var go = new GameObject("3DText");
        go.transform.position = position;
        go.transform.rotation = rotation;

        var textMesh = go.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.characterSize = 0.2f;
        textMesh.fontSize = 64;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = color;

        var renderer = go.GetComponent<MeshRenderer>();
        renderer.sortingOrder = 10;

        var fx = go.AddComponent<VisualEffectsText>();
        fx.Initialize(duration);
    }
}
