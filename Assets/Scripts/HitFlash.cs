using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [Header("References")]
    public Renderer flashRenderer; // drag your sphere renderer here

    [Header("Settings")]
    public float flashDuration = 0.3f;

    private Material _mat;

    void Awake()
    {
        // Get a unique instance so we don't modify the shared asset
        _mat = flashRenderer.material;
        _mat.color = new Color(0f, 0f, 0f, 0f); // start black (invisible on the sphere)
    }

    public void TriggerFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashDuration;
            float r = Mathf.Lerp(1f, 0f, t * t);
            _mat.color = new Color(r, 0f, 0f, 0f); // alpha stays full, red fades to black
            yield return null;
        }

        _mat.color = new Color(0f, 0f, 0f, 0f); // ensure it ends at black
    }

    private void SetAlpha(float a)
    {
        Color c = _mat.color;
        c.a = a;
        _mat.color = c;
    }
}