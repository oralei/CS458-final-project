using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;

public class Fireball : MonoBehaviour
{
    bool wasCastingLeft = false;
    bool wasCastingRight = false;
    public GameObject fbObj;
    public GameObject lSpawn;
    public GameObject rSpawn;

    void Update()
    {
        Debug.Log("bingus");
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool l_stickPressed);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool r_stickPressed);

        bool isCastingLeft = l_stickPressed && ps.lFireReady;
        bool isCastingRight = r_stickPressed && ps.rFireReady;

        if (isCastingLeft && !wasCastingLeft)
        {
            Debug.Log("Left fireball cast!");
            Instantiate(fbObj, lSpawn.transform.position, lSpawn.transform.rotation);
        }

        if (isCastingRight && !wasCastingRight)
        {
            Debug.Log("Right fireball cast!");
            Instantiate(fbObj, rSpawn.transform.position, rSpawn.transform.rotation);
        }

        wasCastingLeft = isCastingLeft;
        wasCastingRight = isCastingRight;
    }
}