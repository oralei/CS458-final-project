using UnityEngine;

public class MissleLauncher : MonoBehaviour
{
    public GameObject missle;
    public ParticleSystem sparks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SummonMissle", 1.25f, 1.25f);
    }

    void Update()
    {

    }

    void SummonMissle()
    {
        GameObject spawnBlock = Instantiate(missle, transform.position, transform.rotation);
        sparks.Play(spawnBlock);

        Rigidbody rb = spawnBlock.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * 6.5f;
    }
}
