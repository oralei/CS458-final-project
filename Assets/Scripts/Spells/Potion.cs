using UnityEngine;
using UnityEngine.XR;

public class Potion : MonoBehaviour
{
    private bool lastLPressed;
    private bool lastRPressed;
    public int potionsRemaining = 5;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        /*var glow = SpellMaterialGlow.Instance;
        if (glow != null)
        {
            glow.SetLeft(ps.lPotionReady ? glow.bluePotion : glow.whiteIdle);
            glow.SetRight(ps.rPotionReady ? glow.bluePotion : glow.whiteIdle);
        }*/

        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool lPressed);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rPressed);

        if (ps.lPotionReady)
        {
            if (lPressed && !lastLPressed)
                DrinkPotion();
        }

        if (ps.rPotionReady)
        {
            if (rPressed && !lastRPressed)
                DrinkPotion();
        }

        lastLPressed = lPressed; 
        lastRPressed = rPressed;
    }

    void DrinkPotion()
    {
        PlayerMain.Instance.HealPlayer(20f);
        potionsRemaining--;
    }
}
