using System.Collections.Generic;
using UnityEngine;

public class SpellMaterialGlow : MonoBehaviour
{
    Material material;
    GameObject glowPart;

    SpellColor blue = new SpellColor();
    SpellColor red = new SpellColor();
    SpellColor green = new SpellColor();
    SpellColor white = new SpellColor(Color.white, Color.white, 5f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpellColor(SpellColor spellColor)
    {
        material.SetColor("_MainColor", spellColor.baseColor);
        material.SetColor("_Emission", spellColor.emissionColor);
        material.SetFloat("_Intensity", spellColor.emissionIntensity);
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