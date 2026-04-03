using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public GameObject leftShield;
    public GameObject rightShield;

    public bool leftShieldActive = false;
    public bool rightShieldActive = false;

    bool leftButtonPrev = false;
    bool rightButtonPrev = false;

    [Header("Duration")]
    public float shieldDuration = 3f;

    [Header("Cooldown")]
    public float shieldCooldown = 2.25f;

    // Per-hand timers
    private float leftDurationTimer = 0f;
    private float rightDurationTimer = 0f;

    private float leftCooldownTimer = 0f;
    private float rightCooldownTimer = 0f;

    private bool leftCanActivate = true;
    private bool rightCanActivate = true;

    [Header("UI")]
    public Image leftDurationImage;
    public Image rightDurationImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public static Shield Instance { get; private set; }
    void Awake() 
    { 
        Instance = this; 
    }

    // Update is called once per frame
    void Update()
    {
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool leftButton);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rightButton);

        // Button just pressed this frame (rising edge)
        bool leftPressed = leftButton && !leftButtonPrev;
        bool rightPressed = rightButton && !rightButtonPrev;

        // --- Activate / manual toggle off ---
        if (leftPressed)
        {
            if (!leftShieldActive && ps.lShieldReady && leftCanActivate)
                ActivateShieldState(true);
            else if (leftShieldActive)
                DeactivateShieldState(true);
        }

        if (rightPressed)
        {
            if (!rightShieldActive && ps.rShieldReady && rightCanActivate)
                ActivateShieldState(false);
            else if (rightShieldActive)
                DeactivateShieldState(false);
        }

        // --- Force off if fist released ---
        if (!ps.LeftFist && leftShieldActive) DeactivateShieldState(true);
        if (!ps.RightFist && rightShieldActive) DeactivateShieldState(false);

        // --- Tick duration timers ---
        if (leftShieldActive)
        {
            leftDurationTimer -= Time.deltaTime;
            if (leftDurationTimer <= 0f)
                DeactivateShieldState(true);
        }

        if (rightShieldActive)
        {
            rightDurationTimer -= Time.deltaTime;
            if (rightDurationTimer <= 0f)
                DeactivateShieldState(false);
        }

        // --- Tick cooldown timers ---
        if (!leftCanActivate)
        {
            leftCooldownTimer -= Time.deltaTime;
            if (leftCooldownTimer <= 0f)
            {
                leftCooldownTimer = 0f;
                leftCanActivate = true;
            }
        }

        if (!rightCanActivate)
        {
            rightCooldownTimer -= Time.deltaTime;
            if (rightCooldownTimer <= 0f)
            {
                rightCooldownTimer = 0f;
                rightCanActivate = true;
            }
        }

        UpdateUI();

        ActivateShieldObject(leftShield, leftShieldActive);
        ActivateShieldObject(rightShield, rightShieldActive);

        leftButtonPrev = leftButton;
        rightButtonPrev = rightButton;
    }

    void ActivateShieldState(bool isLeft)
    {
        if (isLeft)
        {
            leftShieldActive = true;
            leftDurationTimer = shieldDuration;
        }
        else
        {
            rightShieldActive = true;
            rightDurationTimer = shieldDuration;
        }
    }

    void ActivateShieldObject(GameObject shieldObject, bool state)
    {
        if (shieldObject.activeSelf != state)
            shieldObject.SetActive(state);
    }

    // Called on manual toggle-off, fist release, or expiry
    void DeactivateShieldState(bool isLeft)
    {
        if (isLeft)
        {
            leftShieldActive = false;
            leftDurationTimer = 0f;
            leftCanActivate = false;
            leftCooldownTimer = shieldCooldown;
        }
        else
        {
            rightShieldActive = false;
            rightDurationTimer = 0f;
            rightCanActivate = false;
            rightCooldownTimer = shieldCooldown;
        }
    }

    void UpdateUI()
    {
        // Active: 1 to 0 (draining). Cooldown: 0 to 1 (refilling). Idle (ready): 1.
        if (leftDurationImage != null)
        {
            if (leftShieldActive)
                leftDurationImage.fillAmount = leftDurationTimer / shieldDuration;
            else if (!leftCanActivate)
                leftDurationImage.fillAmount = 1f - (leftCooldownTimer / shieldCooldown);
            else
                leftDurationImage.fillAmount = 1f;
        }

        if (rightDurationImage != null)
        {
            if (rightShieldActive)
                rightDurationImage.fillAmount = rightDurationTimer / shieldDuration;
            else if (!rightCanActivate)
                rightDurationImage.fillAmount = 1f - (rightCooldownTimer / shieldCooldown);
            else
                rightDurationImage.fillAmount = 1f;
        }
    }
}
