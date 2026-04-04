using UnityEngine;
using System.Collections;


public class KillSelf : MonoBehaviour
{
    public float time;
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
