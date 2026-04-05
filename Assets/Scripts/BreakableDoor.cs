using UnityEngine;
using static Unity.VisualScripting.Member;

public class BreakableDoor : MonoBehaviour
{
    public GameObject doorBreakStuff;
    // Destroy Self
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerFB"))
        {
            Instantiate(doorBreakStuff, transform.position + (Vector3.up * 1f), transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
