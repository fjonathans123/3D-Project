using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armorpickup : MonoBehaviour
{
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.armor += 10f;

            if (gm.armor > (gm.Maxarmor + (gm.Maxarmor / 2)))
            {
                gm.armor = (gm.Maxarmor + (gm.Maxarmor / 2)); 
            }
            Destroy(gameObject);
        }
    }
}
