using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointTrigger : MonoBehaviour
{
    private playeMovement checkpoint;
    // Start is called before the first frame update
    void Start()
    {
        checkpoint = GameObject.FindGameObjectWithTag("Player").GetComponent<playeMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpoint.PlayerPos = other.transform.position;
        }
    }
}
