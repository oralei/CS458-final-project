using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform playerTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTarget = PlayerMain.Instance.transform;
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Log("Turret Spawned!");
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, playerTarget.position + (Vector3.up * 1f));
    }

    void OnDestroy()
    {
        Debug.Log("Turret was destroyed: " + gameObject.name);
    }
}
