using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class Potion : MonoBehaviour
{
    private bool lastLPressed;
    private bool lastRPressed;
    public int potionsRemaining = 5;

    public GameObject leftPotionObj;
    public GameObject rightPotionObj;

    public float potionCoolDown = 1;
    private bool leftCanDrink = true;
    private bool rightCanDrink = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        // Left potion visibility
        if (leftPotionObj != null)
        {
            bool showLeft = ps.lPotionReady && leftCanDrink;
            leftPotionObj.SetActive(showLeft);
        }

        // Right potion visibility
        if (rightPotionObj != null)
        {
            bool showRight = ps.rPotionReady && rightCanDrink;
            rightPotionObj.SetActive(showRight);
        }

        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool lPressed);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rPressed);

        if (ps.lPotionReady)
        {
            if (lPressed && !lastLPressed && leftCanDrink)
                DrinkPotion(true);
        }

        if (ps.rPotionReady)
        {
            if (rPressed && !lastRPressed && rightCanDrink)
                DrinkPotion(false);
        }

        lastLPressed = lPressed; 
        lastRPressed = rPressed;
    }

    void DrinkPotion(bool isLeft)
    {
        if (potionsRemaining <= 0) return;

        StartCoroutine(Cooldown(potionCoolDown, isLeft));
        PlayerMain.Instance.HealPlayer(20f);
        potionsRemaining--;
    }

    IEnumerator Cooldown(float cooldown, bool isLeft)
    {
        if (isLeft) leftCanDrink = false;
        else rightCanDrink = false;

        yield return new WaitForSeconds(cooldown);

        if (isLeft) leftCanDrink = true;
        else rightCanDrink = true;
    }
}
