using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimController : MonoBehaviour
{
    public InputActionReference gripInput;
    public InputActionReference triggerInput;

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
    }
}
