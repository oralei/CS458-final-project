using UnityEngine;

public class StateChanger : MonoBehaviour
{
    public WizardBehaviour w;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHead") && !triggered)
        {
            triggered = true;
            w.DisplayText();
        }
    }
}
