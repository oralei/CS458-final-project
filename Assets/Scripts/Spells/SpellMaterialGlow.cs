using System.Collections;
using UnityEngine;

public class SpellMaterialGlow : MonoBehaviour
{
    public static SpellMaterialGlow Instance;

    public Renderer[] lRenderers;
    public Renderer[] rRenderers;

    public SpellColor fireRed = new SpellColor(new Color(1f, 0.18f, 0f), new Color(1f, 0.16f, 0f), 5f);
    public SpellColor greenPotion = new SpellColor(new Color(0.75f, 1f, 0.76f), new Color(0f, 1f, 0.03f), 4f);
    public SpellColor blueShield = new SpellColor(new Color(0f, 0.77f, 1f), new Color(0f, 0.27f, 1f), 6f);
    public SpellColor whiteIdle = new SpellColor(Color.white, Color.white, 5f);

    Coroutine lRoutine;
    Coroutine rRoutine;

    public float lLockTimer = 0f;
    public float rLockTimer = 0f;

    void Awake() { Instance = this; }

    void Update()
    {
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        UpdateHandGlow(ps, true);
        UpdateHandGlow(ps, false);
    }

    void UpdateHandGlow(PlayerTransformState ps, bool isLeft)
    {
        SpellColor color = whiteIdle;
        bool shieldActive = Shield.Instance != null && (isLeft ? Shield.Instance.leftShieldActive : Shield.Instance.rightShieldActive);

        if (isLeft)
        {
            if (ps.lFireReady) color = fireRed;
            else if (shieldActive) color = blueShield;
            else if (ps.lPotionReady) color = greenPotion;
        }
        else
        {
            if (ps.rFireReady) color = fireRed;
            else if (shieldActive) color = blueShield;
            else if (ps.rPotionReady) color = greenPotion;
        }

        foreach (var r in (isLeft ? lRenderers : rRenderers))
            SetSpellColor(r, color);
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