using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectileScript : MonoBehaviour
{
    private GameManager gm;
    public float damage, armorDamage;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Destroy(gameObject, 2f);    
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           if(gm.armor > 0f)
            {
                gm.armor -= armorDamage;
                if(gm.armor < 0f)
                {
                    gm.armor = 0f;
                }
            }
            else
            {
                gm.health -= damage;
                gm.healthcooldown = 5f;
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
