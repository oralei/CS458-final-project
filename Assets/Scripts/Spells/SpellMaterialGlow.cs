using System.Collections;
using UnityEngine;

public class SpellMaterialGlow : MonoBehaviour
{
    public static SpellMaterialGlow Instance;

    public Renderer[] lRenderers;
    public Renderer[] rRenderers;

    public SpellColor fireRed = new SpellColor(new Color(1f, 0.18f, 0f), new Color(1f, 0.16f, 0f), 5f);
    public SpellColor greenShield = new SpellColor(new Color(0.75f, 1f, 0.76f), new Color(0f, 1f, 0.03f), 4f);
    public SpellColor bluePotion = new SpellColor(new Color(0f, 0.77f, 1f), new Color(0f, 0.27f, 1f), 6f);
    public SpellColor whiteIdle = new SpellColor(Color.white, Color.white, 5f);

    Coroutine lRoutine;
    Coroutine rRoutine;

    void Awake() { Instance = this; }

    public void SetLeft(SpellColor c, float duration = 0f)
    {
        if (lRoutine != null) StopCoroutine(lRoutine);
        foreach (var r in lRenderers) SetSpellColor(r, c);
        if (duration > 0f)
            lRoutine = StartCoroutine(RevertAfter(lRenderers, true, duration));
    }

    public void SetRight(SpellColor c, float duration = 0f)
    {
        if (rRoutine != null) StopCoroutine(rRoutine);
        foreach (var r in rRenderers) SetSpellColor(r, c);
        if (duration > 0f)
            rRoutine = StartCoroutine(RevertAfter(rRenderers, false, duration));
    }

    IEnumerator RevertAfter(Renderer[] renderers, bool isLeft, float duration)
    {
        yield return new WaitForSeconds(duration);
        foreach (var r in renderers) SetSpellColor(r, whiteIdle);
        if (isLeft) lRoutine = null;
        else rRoutine = null;
    }

    public void SetSpellColor(Renderer r, SpellColor spellColor)
    {
        r.material.SetColor("_MainColor", spellColor.baseColor);
        r.material.SetColor("_Emission", spellColor.emissionColor);
        r.material.SetFloat("_Intensity", spellColor.emissionIntensity);
    }
}

[System.Serializable]
public struct SpellColor
{
    public Color baseColor;
    public Color emissionColor;
    public float emissionIntensity;

    public SpellColor(Color baseColor, Color emissionColor, float emissionIntensity)
    {
        this.baseColor = baseColor;
        this.emissionColor = emissionColor;
        this.emissionIntensity = emissionIntensity;
    }
}