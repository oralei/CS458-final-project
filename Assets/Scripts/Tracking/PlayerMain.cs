using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public CapsuleCollider bodyHitBox;
    public SphereCollider headHitBox;
    public PlayerMain Instance;
    public float health = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
