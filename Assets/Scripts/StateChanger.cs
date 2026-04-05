using UnityEngine;

public class StateChanger : MonoBehaviour
{
    public WizardBehaviour w;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHead"))
        {
            w.DisplayText();

        }
    }
}
