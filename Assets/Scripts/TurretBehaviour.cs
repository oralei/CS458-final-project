using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public bool isTracker = false;
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
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);

            Vector3 targetPos = isTracker
                ? PlayerMain.Instance.ChestAimPoint.position
                : transform.position + transform.forward * 100f;

            Vector3 direction = targetPos - transform.position;
            float distance = direction.magnitude;

            int worldLayer = LayerMask.GetMask("World");

            if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, worldLayer))
                lineRenderer.SetPosition(1, hit.point);
            else
                lineRenderer.SetPosition(1, targetPos);
        }
    }

    void OnDestroy()
    {
        Debug.Log("Turret was destroyed: " + gameObject.name);
    }
}
