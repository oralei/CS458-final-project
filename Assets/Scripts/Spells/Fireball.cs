using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;
using System.Collections;
using UnityEngine.UI;

public class Fireball : MonoBehaviour
{
    bool wasCastingLeft = false;
    bool wasCastingRight = false;
    public GameObject fbObj;
    public GameObject lSpawn;
    public GameObject rSpawn;
    public ParticleSystem lSparks;
    public ParticleSystem rSparks;

    [SerializeField] private AudioClip[] clipArray;
    public AudioSource lSound;
    public AudioSource rSound;

    public float leftCoolDown = 1.5f;
    public float rightCoolDown = 1.5f;

    private bool leftCanFire = true;
    private bool rightCanFire= true;
    // UI
    public Image leftCooldownImage;
    public Image rightCooldownImage;

    private float leftCooldownTimer = 0f;
    private float rightCooldownTimer = 0f;

    void Start()
    {
        if (leftCooldownImage != null)
            leftCooldownImage.fillAmount = 1f;

        if (rightCooldownImage != null)
            rightCooldownImage.fillAmount = 1f;
    }
    void Update()
    {
        UpdateCooldownUI();
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool l_stickPressed);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool r_stickPressed);

        bool isCastingLeft = l_stickPressed && ps.lFireReady;
        bool isCastingRight = r_stickPressed && ps.rFireReady;

        if (isCastingLeft && !wasCastingLeft && leftCanFire)
        {
            Debug.Log("Left fireball cast!");
            StartCoroutine(Cooldown(leftCoolDown, true));

            lSparks.Play();
            lSound.PlayOneShot(clipArray[Random.Range(0, clipArray.Length - 1)]);

            GameObject fb = Instantiate(fbObj, lSpawn.transform.position, lSpawn.transform.rotation);

            leftController.SendHapticImpulse(0, 0.3f, 0.2f);

            // Wire up homing target from the lock-on system
            FireballProjectile projectile = fb.GetComponent<FireballProjectile>();
            if (projectile != null && LockOnSystem.Instance != null)
                projectile.homingTarget = LockOnSystem.Instance.lockedTargetLeft;
        }

        if (isCastingRight && !wasCastingRight && rightCanFire)
        {
            Debug.Log("Right fireball cast!");
            StartCoroutine(Cooldown(rightCoolDown, false));

            rSparks.Play();
            rSound.PlayOneShot(clipArray[Random.Range(0, clipArray.Length - 1)]);
            GameObject fb = Instantiate(fbObj, rSpawn.transform.position, rSpawn.transform.rotation);

            rightController.SendHapticImpulse(0, 0.3f, 0.2f);

            FireballProjectile projectile = fb.GetComponent<FireballProjectile>();
            if (projectile != null && LockOnSystem.Instance != null)
                projectile.homingTarget = LockOnSystem.Instance.lockedTargetRight;
        }

        wasCastingLeft = isCastingLeft;
        wasCastingRight = isCastingRight;
    }
    IEnumerator Cooldown(float cooldown, bool isLeft)
    {
        if (isLeft)
        {
            leftCanFire = false;
            leftCooldownTimer = cooldown;
        }
        else
        {
            rightCanFire = false;
            rightCooldownTimer = cooldown;
        }

        yield return new WaitForSeconds(cooldown);

        if (isLeft) leftCanFire = true;
        else rightCanFire = true;
    }
    void UpdateCooldownUI()
    {
        if (leftCooldownTimer > 0f)
        {
            leftCooldownTimer -= Time.deltaTime;
            if (leftCooldownImage != null)
                leftCooldownImage.fillAmount = 1f - (leftCooldownTimer / leftCoolDown);
        }
        else
        {
            leftCooldownTimer = 0f;
            if (leftCooldownImage != null)
                leftCooldownImage.fillAmount = 1f;
        }

        if (rightCooldownTimer > 0f)
        {
            rightCooldownTimer -= Time.deltaTime;
            if (rightCooldownImage != null)
                rightCooldownImage.fillAmount = 1f - (rightCooldownTimer / rightCoolDown);
        }
        else
        {
            rightCooldownTimer = 0f;
            if (rightCooldownImage != null)
                rightCooldownImage.fillAmount = 1f;
        }
    }

}