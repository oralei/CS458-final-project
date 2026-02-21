using UnityEngine;
using UnityEngine.XR;

public class CalibrationManager : MonoBehaviour
{
    bool wasPressed = false;

    void Update()
    {
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        rightController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool pressed);

        if (pressed && !wasPressed)
        {
            OnCalibrate();
        }
        wasPressed = pressed;
    }

    void OnCalibrate()
    {
        var ps = PlayerTransformState.Instance;
        float rightDist = Vector2.Distance(
            new Vector2(ps.RightHandPosition.x, ps.RightHandPosition.z),
            new Vector2(ps.HeadPosition.x, ps.HeadPosition.z)
        );
        float leftDist = Vector2.Distance(
            new Vector2(ps.LeftHandPosition.x, ps.LeftHandPosition.z),
            new Vector2(ps.HeadPosition.x, ps.HeadPosition.z)
        );
        ps.CalibratedArmLength = Mathf.Max(rightDist, leftDist);
        Debug.Log($"Arm calibrated: {ps.CalibratedArmLength:F2}m");
    }
}