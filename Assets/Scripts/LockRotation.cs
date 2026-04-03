using UnityEngine;

public class LockRotation : MonoBehaviour
{
    void LateUpdate()
    {
        // Get the parent's forward (stump facing direction) in world space
        Vector3 stumpForward = transform.parent.forward;

        // Recompose a rotation that keeps that forward but forces world up
        transform.rotation = Quaternion.LookRotation(stumpForward, Vector3.up);
    }
}