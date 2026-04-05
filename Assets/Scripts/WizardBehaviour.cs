using UnityEngine;
using TMPro;

public class WizardBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogue;
    public GameObject wizardModel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText()
    {
        switch (GameManager.Instance.gameState)
        {
            case 0:
                wizardModel.SetActive(true);
                dialogue.text = "Hoho! My spell worked! I finally managed to summon a soul from another world into these gloves. Now I can finally figure out how they work.";
                GameManager.Instance.gameState++;
                break;
            case 1:
                wizardModel.SetActive(true);
                dialogue.text = "But first, you'll have to get through this door. If you only had some way to break it open...";
                GameManager.Instance.gameState++;
                break;
            default:
                
                break;
        }
    }
}
