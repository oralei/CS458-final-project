using UnityEngine;

public class MissleLauncher : MonoBehaviour
{
    public GameObject missle;
    public ParticleSystem sparks;
    public float speed = 6.5f;
    public float fireRate = 1.25f;
    public bool isHoming;
    public AudioSource AudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        InvokeRepeating("SummonMissle", Random.Range(0f, fireRate), fireRate);
    }

    void Update()
    {

    }

    void SummonMissle()
    {
        GameObject spawnBlock = Instantiate(missle, transform.position + transform.forward * 0.1f, transform.rotation);
        sparks.Play(spawnBlock);
        if (AudioSource != null)
            AudioSource.Play();

        EnemyMagicMissle missleScript = spawnBlock.GetComponent<EnemyMagicMissle>();
        missleScript.isHoming = isHoming; // or true

        Rigidbody rb = spawnBlock.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Debug.Log("Turret Shot!");
    }
}
