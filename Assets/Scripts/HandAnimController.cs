using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimController : MonoBehaviour
{
    public InputActionReference gripInput;
    public InputActionReference triggerInput;

    [SerializeField] private bool isLeftHand;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!animator) return;

        float grip = gripInput.action.ReadValue<float>();
        float trigger = triggerInput.action.ReadValue<float>();

        animator.SetFloat("Grip", grip);
        animator.SetFloat("Trigger", trigger);

        var ps = PlayerTransformState.Instance;
        if (ps != null)
        {
            bool drinking = isLeftHand ? ps.lPotionReady : ps.rPotionReady;
            animator.SetBool("isDrinking", drinking);
        }
    }
}
