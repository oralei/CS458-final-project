using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR;

public class ShieldCollide : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioClip parrySound;
    public AudioSource source;
    public bool isLeft;

    private InputDevice leftController;
    private InputDevice rightController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Destroy Enemy Projectiles
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProj"))
        {
            if (isLeft && leftController != null)
                leftController.SendHapticImpulse(0, 0.85f, 0.35f);
            else if (!isLeft && rightController != null)
                rightController.SendHapticImpulse(0, 0.85f, 0.35f);

            Debug.Log("Blocked!");
            source.clip = hitSound;
            source.pitch = (Random.Range(0.6f, 1.4f));
            source.Play();

            Destroy(other.gameObject);
        }
    }
}
