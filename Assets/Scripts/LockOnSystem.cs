using UnityEngine;

/*
Class: LockOnSystem

Breakdown:
- Cone Settings to adjust the cone angle and range (editable in Inspector)
- Dwell Settings to adjust how long it takes to lock on to a target, and how fast to unlock (editable in Inspector)
- UpdateHand(), ResetHand(), GetBestTarget()
- Layer Mask to only select specific targets (for performance/optimization)
*/
public class LockOnSystem : MonoBehaviour
{
    public static LockOnSystem Instance { get; private set; }

    [Header("Cone Settings")]
    public float coneAngle = 20f;
    public float coneRange = 20f;

    [Header("Dwell Settings")]
    public float dwellThreshold = 0.8f;
    public float dwellDecayRate = 1.5f;

    [Header("Targeting")]
    [SerializeField] private LayerMask targetLayer;

    [Header("Hand Transforms (for Gizmos)")]
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

    // Exposed locked targets for each hand
    [HideInInspector] public Transform lockedTargetLeft;
    [HideInInspector] public Transform lockedTargetRight;

    private float dwellTimerLeft = 0f;
    private float dwellTimerRight = 0f;

    // Separate candidates for left or right hand.
    [HideInInspector] public Transform candidateLeft;
    [HideInInspector] public Transform candidateRight;

    private Transform previousCandidateLeft;
    private Transform previousCandidateRight;

    // Make sure this script is an Instance.
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    /*
    Update() Functionality

    Breakdown:
    - We reference the PlayerTransformState instance so we can get left and right hand position (controllers)
    - If the controller is in the FireReady (finger gun and extended arm), then we can start the lock on logic UpdateHand().
      - Else, we call the ResetHand() function
    */
    void Update()
    {
        var ps = PlayerTransformState.Instance;
        if (ps == null) return;

        // LEFT hand finger gun AND extended arm
        if (ps.lFireReady)
        {
            UpdateHand("LEFT", ps.LeftHandPosition, ps.LeftHandRotation,
                ref candidateLeft, ref previousCandidateLeft,
                ref dwellTimerLeft, ref lockedTargetLeft);
        }
        else{
            ResetHand(ref candidateLeft, ref previousCandidateLeft, ref dwellTimerLeft, ref lockedTargetLeft);
        }

        // RIGHT hand finger gun AND extended arm
        if (ps.rFireReady)
        {
            UpdateHand("RIGHT", ps.RightHandPosition, ps.RightHandRotation,
                ref candidateRight, ref previousCandidateRight,
                ref dwellTimerRight, ref lockedTargetRight);
        }
        else{
            ResetHand(ref candidateRight, ref previousCandidateRight, ref dwellTimerRight, ref lockedTargetRight);
        }
    }

    // ResetHand() Breakdown: Essentially, just reset all the values for the lock on.
    void ResetHand(ref Transform candidate, ref Transform previousCandidate, ref float dwellTimer, ref Transform locked)
    {
        candidate = null;
        previousCandidate = null;
        dwellTimer = 0f;
        locked = null;
    }

    /*
    UpdateHand() Functionality - Handles Dwell timing:

    Breakdown:
    - Parameters: Depending on which hand, we have a string for labelling, position Vector3, rotation, 
                  candidate, previous candidate, current dwell timer, and locked target
    - Calls the GetBestTarget() function.
    */
    void UpdateHand(
        string hand,
        Vector3 handPos,
        Quaternion handRot,
        ref Transform candidate,
        ref Transform previousCandidate,
        ref float dwellTimer,
        ref Transform locked)
    {
        candidate = GetBestTarget(handPos, handRot * Vector3.forward);

        // Require SAME candidate to build dwell
        if (candidate != null && candidate == previousCandidate)
        {
            dwellTimer += Time.deltaTime;

            if (dwellTimer >= dwellThreshold)
                locked = candidate;
        }
        else
        {
            // decay instead of instant reset
            dwellTimer = Mathf.Max(0f, dwellTimer - dwellDecayRate * Time.deltaTime);
        }

        // Lose lock if fully decayed
        if (dwellTimer <= 0f)
            locked = null;

        previousCandidate = candidate;
    }

    /*
    GetBestTarget() Functionality - Core Lock-On System Logic:

    Breakdown:
    - Broad to narrow filter system
      - Filter 1: Sphere overlap (all degrees of freedom, but range of cone)
      - Filter 2: Cone angle calculation and comparison for each object within Filter 1
      - Filter 3: Raycast to prevent lock on through walls.
    - Target scoring based on angle and distance
    */
    Transform GetBestTarget(Vector3 origin, Vector3 forward)
    {
        // Filter 1: Find every collider within a sphere of radius coneRange centered at origin (the hand position), but only on the specified targetLayer.
        // Culls anything too far away before doing any math. If nothing is hit, it returns early with null.
        // The sphere is used as a cheap approximation.
        Collider[] hits = Physics.OverlapSphere(origin, coneRange, targetLayer);

        Transform best = null;
        float bestScore = float.MinValue; // The smallest possible float value (-3.4E28)

        // We take each object within the sphere overlap
        foreach (Collider c in hits)
        {
            Transform t = c.transform;

            // Filter 2: Is the target within the cone?
            Vector3 toTarget = t.position - origin;    // Direction of hand to target
            float dist = toTarget.magnitude;           // Distance to target
            Vector3 dir = toTarget.normalized;         // Normalized direction

            float angle = Vector3.Angle(forward, dir); // Angle from centre ray to target

            if (angle > coneAngle) continue;           // Makes sure that the target is within the cone angle.

            // Filter 3: Line of sight check
            if (Physics.Raycast(origin, dir, out RaycastHit hit, coneRange))
            {
                if (hit.transform != t)
                    continue;
            }

            // --- Scoring system (tweak weights as needed) ----
            // Targets that pass all three filters get a score.
            // Both terms subtract from the score, meaning lower angle and shorter distance both make the score less negative (i.e. higher).
            // The weights control priority:
            float score = 0f;

            score -= angle * 2f;   // Angle is weighted more heavily, so centrality in the cone matters more than distance
            score -= dist * 0.5f;  // Prioritize target's closer to the player (hand)

            // If this target's score is better, set it to be best target.
            if (score > bestScore)
            {
                bestScore = score;
                best = t;
            }

            if (angle > coneAngle)
            {
                Debug.Log($"[LockOnSystem] {t.name} outside cone ({angle:F1}° > {coneAngle}°) — skipped.");
                continue;
            }

            if (hit.transform != t)
            {
                Debug.Log($"[LockOnSystem] {t.name} blocked by {hit.transform.name} — LoS failed.");
                continue;
            }
        }

        // Return the Transform of the best calculated/selected target.
        return best;
    }

    // --- Gizmo Visualization ---
    // Simply draws a cone based on hand position, cone shape, and lock on state
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
