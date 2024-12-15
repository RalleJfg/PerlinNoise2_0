using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyScript : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
