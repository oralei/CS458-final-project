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
    public ParticleSystem lSparks;
    public ParticleSystem rSparks;

    void Update()
    {
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
            SpellMaterialGlow.Instance.SetLeft(SpellMaterialGlow.Instance.fireRed, 0.125f);
            lSparks.Play();
            GameObject fb = Instantiate(fbObj, lSpawn.transform.position, lSpawn.transform.rotation);

            // Wire up homing target from the lock-on system
            FireballProjectile projectile = fb.GetComponent<FireballProjectile>();
            if (projectile != null && LockOnSystem.Instance != null)
                projectile.homingTarget = LockOnSystem.Instance.lockedTargetLeft;
        }

        if (isCastingRight && !wasCastingRight)
        {
            Debug.Log("Right fireball cast!");
            SpellMaterialGlow.Instance.SetRight(SpellMaterialGlow.Instance.fireRed, 0.125f);
            rSparks.Play();
            GameObject fb = Instantiate(fbObj, rSpawn.transform.position, rSpawn.transform.rotation);

            FireballProjectile projectile = fb.GetComponent<FireballProjectile>();
            if (projectile != null && LockOnSystem.Instance != null)
                projectile.homingTarget = LockOnSystem.Instance.lockedTargetRight;
        }

        wasCastingLeft = isCastingLeft;
        wasCastingRight = isCastingRight;
    }
}