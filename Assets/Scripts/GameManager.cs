using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gameState = 0;
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
