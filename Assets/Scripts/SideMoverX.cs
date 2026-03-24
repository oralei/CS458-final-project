using UnityEngine;

public class SideMoverX : MonoBehaviour
{
    public float moveDistance = 3f;   // How far left/right from start
    public float speed = 2f;          // Movement speed

    private Rigidbody rb;
    private Vector3 startPosition;
    private int direction = 1; // 1 = right, -1 = left

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Calculate target movement
        Vector3 movement = Vector3.right * direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Check distance from start
        float offset = rb.position.x - startPosition.x;

        if (Mathf.Abs(offset) >= moveDistance)
        {
            direction *= -1; // Flip direction
        }
    }
}
