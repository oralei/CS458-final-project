using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class Potion : MonoBehaviour
{
    private bool lastLPressed;
    private bool lastRPressed;
    public int potionsRemaining = 5;
    [SerializeField] TextMeshProUGUI potsText;

    public GameObject leftPotionObj;
    public GameObject rightPotionObj;

    public float potionCoolDown = 1;
    private bool leftCanDrink = true;
    private bool rightCanDrink = true;

    public AudioClip emptySound;
    public AudioClip drinkSound;
    public AudioSource lSound;
    public AudioSource rSound;

    public static Potion Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

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
        if (leftPotionObj != null && potionsRemaining != 0)
        {
            bool showLeft = ps.lPotionReady && leftCanDrink && potionsRemaining > 0; ;
            leftPotionObj.SetActive(showLeft);
        }

        // Right potion visibility
        if (rightPotionObj != null && potionsRemaining != 0)
        {
            bool showRight = ps.rPotionReady && rightCanDrink && potionsRemaining > 0; ;
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
        if (potionsRemaining <= 0)
        {
            if (isLeft)
                lSound.PlayOneShot(emptySound);
            else
                rSound.PlayOneShot(emptySound);

            return;
        }

        if (isLeft)
            lSound.PlayOneShot(drinkSound);
        else
            rSound.PlayOneShot(drinkSound);

        StartCoroutine(Cooldown(potionCoolDown, isLeft));
        PlayerMain.Instance.HealPlayer(20f);
        potionsRemaining--;
        potsText.text = "Potions: " + potionsRemaining;
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
