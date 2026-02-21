using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;

public class Fireball : MonoBehaviour
{
    bool wasCasting = false;

    void Update()
    {
        Debug.Log("bingus");
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool l_stickPressed);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool r_stickPressed);

        bool isCasting = (l_stickPressed || r_stickPressed) && (ps.lFireReady || ps.rFireReady);

        if (isCasting && !wasCasting)
        {
            if (ps.lFireReady)
                Debug.Log("Left fireball cast!");

            if (ps.rFireReady)
                Debug.Log("Right fireball cast!");
        }

        wasCasting = isCasting;
    }
}