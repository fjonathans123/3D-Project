using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMelee : MonoBehaviour
{
    private GameManager gm;
    private dragonScript dragon;

    public float meleeDMG;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dragon = GetComponentInParent<dragonScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dragon);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dragon.isPhaseTwo)
            {
                gm.armor -= meleeDMG * 2;
            }
            
            else

            {
                gm.armor -= meleeDMG;
            }
            if (gm.armor < 0f)
            {
                gm.armor = 0f;
            }

        }
        else
        {
            if (dragon.isPhaseTwo)
            {
                gm.health -= meleeDMG * 3f;
            }
            else
            {
                gm.health -= meleeDMG * 2f;
            }
            gm.healthcooldown = 5f;
            Debug.Log("damage");
        }
    }
}
