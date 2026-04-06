using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float eHealth = 100f;
    public GameObject impact;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage, Collider hitCollider = null)
    {
        Debug.Log(gameObject.name + " hit for " + damage + " damage!");
        eHealth = Mathf.Clamp(eHealth - damage, 0, 100);

        Instantiate(impact);

        if (eHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
