using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandTracker : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

    [Header("Left Hand Input")]
    [SerializeField] InputActionReference leftGrip;
    [SerializeField] InputActionReference leftTrigger;
    [SerializeField] InputActionReference leftPrimary;   // X button

    [Header("Right Hand Input")]
    [SerializeField] InputActionReference rightGrip;
    [SerializeField] InputActionReference rightTrigger;
    [SerializeField] InputActionReference rightPrimary;  // A button

    void Update()
    {
        var ps = PlayerTransformState.Instance;

        // Head
        ps.HeadPosition = headTransform.position;
        ps.HeadRotation = headTransform.rotation;
        ps.HeadForward = headTransform.forward;

        // Hand transforms
        ps.LeftHandPosition = leftHandTransform.position;
        ps.RightHandPosition = rightHandTransform.position;
        ps.LeftHandRotation = leftHandTransform.rotation;
        ps.RightHandRotation = rightHandTransform.rotation;

        // Input
        ps.LeftGrip = leftGrip.action.ReadValue<float>();
        ps.LeftTrigger = leftTrigger.action.ReadValue<float>();
        ps.LeftPrimaryButton = leftPrimary.action.ReadValue<float>() > 0.5f;

        ps.RightGrip = rightGrip.action.ReadValue<float>();
        ps.RightTrigger = rightTrigger.action.ReadValue<float>();
        ps.RightPrimaryButton = rightPrimary.action.ReadValue<float>() > 0.5f;
    }
}