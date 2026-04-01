using UnityEngine;
using UnityEngine.XR;

public class Shield : MonoBehaviour
{
    public GameObject leftShield;
    public GameObject rightShield;

    public bool leftShieldActive = false;
    public bool rightShieldActive = false;

    bool leftButtonPrev = false;
    bool rightButtonPrev = false;

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

        // Activate on press if gesture is ready, toggle off on press again
        if (leftPressed)
        {
            if (!leftShieldActive && ps.lShieldReady)
                leftShieldActive = true;
            else if (leftShieldActive)
                leftShieldActive = false;
        }

        if (rightPressed)
        {
            if (!rightShieldActive && ps.rShieldReady)
                rightShieldActive = true;
            else if (rightShieldActive)
                rightShieldActive = false;
        }

        // Force off if fist is released (arm position no longer matters)
        if (!ps.LeftFist) leftShieldActive = false;
        if (!ps.RightFist) rightShieldActive = false;

        ActivateShield(leftShield, leftShieldActive);
        ActivateShield(rightShield, rightShieldActive);

        leftButtonPrev = leftButton;
        rightButtonPrev = rightButton;
    }

    void ActivateShield(GameObject shieldObject, bool castHand)
    {
        // Check if shield is already active to save SetActive call for performance.
        if (shieldObject.activeSelf != castHand)
            shieldObject.SetActive(castHand);
    }
}
