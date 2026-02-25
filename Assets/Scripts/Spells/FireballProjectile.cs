using UnityEngine;
using System.Collections;

public class FireballProjectile : MonoBehaviour
{
    public float lifetime = 4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * 6.5f;

        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
