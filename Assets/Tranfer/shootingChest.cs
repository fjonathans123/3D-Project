using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingChest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            //Debug.Log("treasure opened");
            GetComponent<lootDrop>().InstantiateLoot(transform.position);
            Destroy(gameObject);
        }
    }
}
