using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class WizardBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogue;
    public GameObject wizardModel;
    public ParticleSystem p;
    public AudioSource a;
    public AudioSource dialogueSound;

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
                wizardModel.SetActive(true); a.Play(); p.Play();
                dialogue.text = "Hoho! My spell worked! I finally managed to summon a soul from another world into these gloves. Now I can finally figure out how they work.";
                GameManager.Instance.gameState++;
                StartCoroutine("Deactivate");
                break;
            case 1:
                wizardModel.SetActive(true);
                dialogue.text = "But first, you'll have to get through this door. If you only had some way to break it open...";
                GameManager.Instance.gameState++;
                StartCoroutine("Deactivate");
                break;
            case 2:
                wizardModel.SetActive(true);
                dialogue.text = "Hoho! That is incredible! Fire magic? Out of your fingertip? But beware... I've set up defensive constructs to test your limits!";
                GameManager.Instance.gameState++;
                StartCoroutine("Deactivate");
                break;
            case 3:
                wizardModel.SetActive(true);
                dialogue.text = "Hmmm... looks like you've taken a hit. Maybe drinking a potion would help ease your pain...";
                GameManager.Instance.gameState++;
                StartCoroutine("Deactivate");
                break;
            case 4:
                wizardModel.SetActive(true);
                dialogue.text = "Easy there! Those are some dangerous missles incoming! Don't want to get hurt eh? I wonder if there's a way you can block those attacks...";
                GameManager.Instance.gameState++;
                StartCoroutine("Deactivate");
                break;
            default:
                Debug.Log("Hit default, gameState = " + GameManager.Instance.gameState);
                break;
        }
        dialogueSound.Play();
    }

    IEnumerator Deactivate()
    {
        Debug.Log("gameState: " + GameManager.Instance.gameState);
        yield return new WaitForSeconds(10);

        wizardModel.SetActive(false);
    }
}
