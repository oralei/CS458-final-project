using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = PlayerMain.Instance.transform;
    }

    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target.position);
    }
}
