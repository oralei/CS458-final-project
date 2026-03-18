using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public static LockOnSystem Instance { get; private set; }

    [Header("Cone Settings")]
    public float coneAngle = 20f;
    public float coneRange = 20f;

    // Exposed locked targets for each hand
    [HideInInspector] public Transform lockedTargetLeft;
    [HideInInspector] public Transform lockedTargetRight;

    [Header("Hand Transforms (for Gizmos)")]
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

    // Make sure this script is an Instance.
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // --- Gizmo Visualization ---
    void OnDrawGizmos()
    {
        if (leftHandTransform != null)
            DrawConeGizmo(leftHandTransform.position, leftHandTransform.rotation, lockedTargetLeft != null);

        if (rightHandTransform != null)
            DrawConeGizmo(rightHandTransform.position, rightHandTransform.rotation, lockedTargetRight != null);
    }

    void DrawConeGizmo(Vector3 origin, Quaternion handRot, bool isLocked)
    {
        Gizmos.color = isLocked ? Color.red : Color.yellow;
        Vector3 forward = handRot * Vector3.forward;

        Gizmos.DrawRay(origin, forward * coneRange);

        int segments = 16;
        for (int i = 0; i < segments; i++)
        {
            float angle = (360f / segments) * i;
            Quaternion rot = Quaternion.AngleAxis(angle, forward);
            Vector3 edge = rot * (Quaternion.AngleAxis(coneAngle, handRot * Vector3.right) * forward);
            Gizmos.DrawRay(origin, edge * coneRange);
        }
    }
}
