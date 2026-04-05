using UnityEngine;
using System.Collections;

public class FireballProjectile : MonoBehaviour
{
    public float lifetime = 4f;
    public float homingStrength = 25f;  // how aggressively it steers
    [HideInInspector] public Transform homingTarget;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * 6.5f;

        StartCoroutine(DestroySelf());
    }

    // Update is called based on framerate
    void FixedUpdate()
    {
        if (homingTarget == null) return;

        // Proportional Navigation: steer toward target
        Vector3 dirToTarget = (homingTarget.position - transform.position).normalized;
        float speed = rb.linearVelocity.magnitude;

        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity.normalized,
            dirToTarget,
            homingStrength * Time.fixedDeltaTime
        ) * speed;

        // Face the direction of travel
        if (rb.linearVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    // I used OnTriggerEnter, as OnCollisionEnter was not working well
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh == null){
                Destroy(other.gameObject);
            }
            else{
                eh.TakeDamage(25f);
            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("World"))
        {
            Destroy(gameObject);
        }
    }
}
