using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalScript : MonoBehaviour
{
    public GameObject finishCanvas;
    public int coinCollected = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coinCollected >= 5)
            {
                finishCanvas.SetActive(true);
            }

        }
    }
}
