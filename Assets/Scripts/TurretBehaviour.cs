using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject playerTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, playerTarget.transform.position + (Vector3.up * 1f));
    }
}
