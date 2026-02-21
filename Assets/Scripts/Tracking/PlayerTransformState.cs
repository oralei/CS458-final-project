using UnityEngine;

public class PlayerTransformState : MonoBehaviour
{
    public static PlayerTransformState Instance { get; private set; }

    [Header("Head")]
    public Vector3 HeadPosition;
    public Quaternion HeadRotation;
    public Vector3 HeadForward;

    [Header("Hands")]
    public Vector3 LeftHandPosition;
    public Vector3 RightHandPosition;
    public Quaternion LeftHandRotation;
    public Quaternion RightHandRotation;

    [Header("Input")]
    public float LeftGrip, RightGrip;
    public float LeftTrigger, RightTrigger;
    public bool LeftPrimaryButton, RightPrimaryButton;

    [HideInInspector] public float CalibratedArmLength = -1f; // -1 means uncalibrated

    // Gestures - Left
    public bool LeftFist => LeftGrip >= 0.9f && LeftTrigger >= 0.9f;
    public bool LeftFingerGun => LeftGrip >= 0.9f && LeftTrigger < 0.2f;

    public bool LeftArmExtended
    {
        get
        {
            if (CalibratedArmLength < 0) return false;
            float current = Vector2.Distance(
                new Vector2(LeftHandPosition.x, LeftHandPosition.z),
                new Vector2(HeadPosition.x, HeadPosition.z)
            );
            return current >= CalibratedArmLength * 0.85f;
        }
    }

    public bool lFireReady => LeftFingerGun && LeftArmExtended;

    // Gestures - Right
    public bool RightFist => RightGrip >= 0.9f && RightTrigger >= 0.9f;
    public bool RightFingerGun => RightGrip >= 0.9f && RightTrigger < 0.2f;

    public bool RightArmExtended
    {
        get
        {
            if (CalibratedArmLength < 0) return false;
            float current = Vector2.Distance(
                new Vector2(RightHandPosition.x, RightHandPosition.z),
                new Vector2(HeadPosition.x, HeadPosition.z)
            );
            return current >= CalibratedArmLength * 0.85f;
        }
    }

    public bool rFireReady => RightFingerGun && RightArmExtended;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
