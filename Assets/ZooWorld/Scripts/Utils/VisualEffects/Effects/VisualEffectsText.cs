using System.Collections;
using UnityEngine;

public class VisualEffectsText : MonoBehaviour
{
    private TextMesh _textMesh;
    private float _duration;

    public void Initialize(float durationSeconds)
    {
        _duration = durationSeconds;
    }

    private void Start()
    {
        _textMesh = GetComponent<TextMesh>();
        StartCoroutine(FadeAndRise());
    }

    private IEnumerator FadeAndRise()
    {
        Color startColor = _textMesh.color;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _duration;
            
            transform.position += Vector3.up * Time.deltaTime * 0.5f;
            _textMesh.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }

        Destroy(gameObject);
    }
}