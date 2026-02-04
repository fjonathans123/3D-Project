using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterScript : MonoBehaviour
{
    public bool isWater = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
        {
        if (other.CompareTag("Water"))
        {
            isWater = true;
        }
    }

    // Update is called once per frame
     private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isWater = false;
        }
    }
}
