using UnityEngine;
using TMPro;

public class PlayerMain : MonoBehaviour
{
    public BoxCollider bodyHitBox;
    public SphereCollider headHitBox;
    public static PlayerMain Instance;
    [SerializeField] TextMeshProUGUI healthText;
    public float health = 100f;
    public float chestHeightOffset = 0.7f;

    public Transform HeadAimPoint;
    public Transform ChestAimPoint;

    [SerializeField] private HitFlash _hitFlash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(float damage, Collider hitCollider = null)
    {
        bool headShot = false;
        if (hitCollider != null && hitCollider == headHitBox)
        {
            damage = damage * 2f;
            headShot = true;
        }

        if (headShot)
            Debug.Log("Player headshot for " + damage + " damage!");
        else
            Debug.Log("Player hit for " + damage + " damage!");

        health = Mathf.Clamp(health - damage, 0, 100);
        healthText.text = "Health: " + health;
        _hitFlash.TriggerFlash();
    }

    public Vector3 GetChestPosition()
    {
        Vector3 chestOffset = HeadAimPoint.position + (Vector3.down * chestHeightOffset); // down a bit

        return chestOffset;
    }

    public void HealPlayer(float healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, 100);
        healthText.text = "Health: " + health;
    }
}
