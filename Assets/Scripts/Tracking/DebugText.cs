using TMPro;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugText;
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;

    string ColorValue(float value)
    {
        string hex = value >= 0.9f ? "#00FF00" : value >= 0.5f ? "#FFFF00" : "white";
        return $"<color={hex}>{value:F2}</color>";
    }

    string ColorValue(bool value)
    {
        return value ? "<color=#00FF00>True</color>" : "False";
    }

    string GetLockOnStatus(Transform locked, Transform candidate)
    {
        if (locked != null)
            return $"<color=#FF4400>LOCKED</color> ? {locked.name}";
        if (candidate != null)
            return $"<color=#FFFF00>Searching...</color> ? {candidate.name}";
        return "Idle";
    }

    void Update()
    {
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        var los = LockOnSystem.Instance;

        float calibrated = ps.CalibratedArmLength;
        float rightArmCurrent = Vector2.Distance(
            new Vector2(ps.RightHandPosition.x, ps.RightHandPosition.z),
            new Vector2(ps.HeadPosition.x, ps.HeadPosition.z)
        );

        debugText.text =
            $"=== HEAD ===\n" +
            $"Pos: {ps.HeadPosition:F2}\n" +
            $"Fwd: {ps.HeadForward:F2}\n\n";

        leftText.text =
            $"=== LEFT HAND ===\n" +
            $"Pos: {ps.LeftHandPosition:F2}\n" +
            $"Grip: {ColorValue(ps.LeftGrip)}  Trigger: {ColorValue(ps.LeftTrigger)}\n" +
            $"Fist: {ColorValue(ps.LeftFist)}  FingerGun: {ColorValue(ps.LeftFingerGun)}\n\n" +

            $"=== LEFT ARM EXTENSION ===\n" +
            $"Calibrated Length: {(calibrated < 0 ? "<color=#FF0000>Uncalibrated</color>" : $"{calibrated:F2}m")}\n" +
            $"Current Length: {rightArmCurrent:F2}m\n" +
            $"Arm Extended: {ColorValue(ps.LeftArmExtended)}\n\n" +

            $"=== LEFT LOCK ON ===\n" +
            (los != null ? GetLockOnStatus(los.lockedTargetLeft, los.candidateLeft) : "No LockOnSystem");

        rightText.text =
            $"=== RIGHT HAND ===\n" +
            $"Pos: {ps.RightHandPosition:F2}\n" +
            $"Grip: {ColorValue(ps.RightGrip)}  Trigger: {ColorValue(ps.RightTrigger)}\n" +
            $"Fist: {ColorValue(ps.RightFist)}  FingerGun: {ColorValue(ps.RightFingerGun)}\n\n" +

            $"=== RIGHT ARM EXTENSION ===\n" +
            $"Calibrated Length: {(calibrated < 0 ? "<color=#FF0000>Uncalibrated</color>" : $"{calibrated:F2}m")}\n" +
            $"Current Length: {rightArmCurrent:F2}m\n" +
            $"Arm Extended: {ColorValue(ps.RightArmExtended)}\n\n" +

            $"=== RIGHT LOCK ON ===\n" +
            (los != null ? GetLockOnStatus(los.lockedTargetRight, los.candidateRight) : "No LockOnSystem");
    }
}