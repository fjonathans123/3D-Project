using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dragonScript : MonoBehaviour
{
    public int DragonHP = 50;
    public Animator anim;
    private gameData data;
    public float lowHP;
    public bool isHitting = false;
    private Rigidbody rb;
    public float meleeForce;
    public GameObject fire, ice, lightning;
    private ElementController elements;

    NavMeshAgent agent;

    public float slowMotionSpeed;
    public float normalspeed = 1f;
    public bool isFrozen = false;
    public bool isstun = false; 
    private float originalAnimSpeed;

    public GameObject hitbox;

    public int DragonType;
    bool alreadyAttack;
    public GameObject bullet;
    public Transform enemyRangeGun;
    public float timeBetweenAttack;

    public FreezeEffect freeze;
    public Enemydefeatscript defeat;

    private void Start()
    {
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        elements = GameObject.FindGameObjectWithTag("Player").GetComponent<ElementController>();
        agent = GetComponent<NavMeshAgent>();
        normalspeed = agent.speed;
        originalAnimSpeed = anim.speed;
        lowHP = DragonHP * 0.25f;
        lowHP = DragonHP * 0.25f;

        hitbox.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        DragonHP -= damage;
        if (isHitting == true)
        {
            if (DragonHP <= 0)
            {
                anim.SetTrigger("die");
                GetComponent<Collider>().enabled = false;
                defeat.EnemyDefeated();
                StartCoroutine(waitforDestroy(10f));
            }
            else
            {
                anim.SetTrigger("damage");
            }
        }
        else
        {
            if (DragonHP <= 0)
            {
                anim.SetTrigger("die");
                GetComponent<Collider>().enabled = false;
                defeat.EnemyDefeated();
                StartCoroutine(waitforDestroy(10f));
            }
            else
            {
                anim.SetTrigger("damage");
                if (elements.element == 1)
                {
                    fire.SetActive(false);
                    ice.SetActive(false);
                    lightning.SetActive(false);
                }

                if (elements.element == 2)
                {
                    StartCoroutine(DPSDuration(6));
                }

                if (elements.element == 3)
                {
                    freeze.isFrozen = true;
                    StartCoroutine(DPSDuration(6));
                }

                if (elements.element == 4)
                {
                    StartCoroutine(DPSDuration(6));
                }
            }

            
        }
            }
      
       void fireDPS()
    {
            fire.SetActive(true);
            ice.SetActive(false);
            lightning.SetActive(false);
            Debug.Log("Hit By Fire damage 1");
     
    }

    void iceDPS()
    {
        fire.SetActive(false);
        ice.SetActive(true);
        lightning.SetActive(false);
        DragonHP -= 1;
        if (!isFrozen)
        {
            agent.speed = normalspeed * slowMotionSpeed;
            anim.speed = originalAnimSpeed * slowMotionSpeed; 
            isFrozen = true;
        }
    }

    void electricityDPS()
    {
            fire.SetActive(false);
            ice.SetActive(false);
            lightning.SetActive(true);
            Debug.Log("Hit By Fire damage 1");
        DragonHP -= 1;
        if (!isstun)
        {
            agent.speed = 0;
            isstun = false;
        }
        hideElement();

    }

    IEnumerator DPSDuration(float timer)
    {
        if (elements.element == 2)
        {
            InvokeRepeating("fireDPS", 1.5f, 2.0f);
            yield return new WaitForSeconds(timer);
            CancelInvoke("fireDPS");
            hideElement();
        }

        if (elements.element == 3)
        {
            InvokeRepeating("iceDPS", 1.5f, 2.0f);
            yield return new WaitForSeconds(timer);
            CancelInvoke("iceDPS");
            if (isFrozen)
            {
                agent.speed = normalspeed;
                anim.speed = originalAnimSpeed;
                isFrozen = false; 
            }
            freeze.isFrozen = false;
            hideElement();
        }

        if (elements.element == 4)
        {
            InvokeRepeating("electricityDPS", 1.5f, 2.0f);
            anim.SetBool("isstun", true);
            yield return new WaitForSeconds(timer);
            CancelInvoke("electricityDPS");
            anim.SetBool("isstun", false);
            hideElement();
            if (isstun)
            {
                agent.speed = normalspeed;
                isstun = false;
            }
        }

    }

    void hideElement()
    {
        fire.SetActive(false);
        ice.SetActive(false);
        lightning.SetActive(false);
    }

    IEnumerator waitforDestroy(float timer)
    {
        yield return new WaitForSeconds(timer);
        data.enemyDestroyed += 1;
        data.money += 100;
        Destroy(gameObject);
        GameManager.score += 100;
    }


    public void StartAttackingPlayer()
    {
        //int 0 = darat, int 1 = udara 

        if(DragonType == 0)
        {
            hitbox.SetActive(false);
        }
        else if (DragonType == 1)
        {
            if (!alreadyAttack)
            {
                Rigidbody rb = Instantiate(bullet, enemyRangeGun.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 6f, ForceMode.Impulse);
                rb.AddForce(transform.up * -1.5f, ForceMode.Impulse);
                alreadyAttack = true;
                //Invoke(nameof(ResetAttack), timeBetweenAttack);
            }
        }
        
}

    private void ResetAttack()
    {
        alreadyAttack = false;
        anim.SetBool("IsAttacking", false);
    }

    public void StopAttackingPlayer()
    {
       if (DragonType == 0)
        {
            hitbox.SetActive(false);
        }
    }
}

