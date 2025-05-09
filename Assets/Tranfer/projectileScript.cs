using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);    
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemies"))
        {
            Destroy(gameObject);
        }
    }
}
