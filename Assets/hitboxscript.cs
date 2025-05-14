using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxscript : MonoBehaviour


{
    private GameManager gm;
    public int damage, armorDamage;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(gm.armor > 0f)
            {
                gm.armor -= armorDamage;
                if (gm.armor < 0f)
                {
                    gm.armor = 0f;
                }
            }
            else
            {
                gm.health -= damage;
                gm.healthcooldown = 5f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
