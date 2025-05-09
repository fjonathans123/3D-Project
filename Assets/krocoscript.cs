using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krocoscript : MonoBehaviour
{
    public int enemyType, damage, enemyHP;
    bool alreadyAttack;
    public bool isMelee;
    public GameObject HitBox;
    public Animator anim;
    private gameData data;

    public int MoneyDrop = 0;
    // Start is called before the first frame update
    void Start()
    {
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        if (isMelee)
        {
            if (enemyType == 0)
            {
                HitBox.SetActive(false);    
            }
        }
    }

    public void StartAttackingPlayer()
    {
        if(enemyType == 0)
        {
            Debug.Log("Enemy Damaged");
            HitBox.SetActive(true);
        }
    }

    public void resetAttack()
    {
        alreadyAttack = false;
        anim.SetBool("isAttacking", false);
    }
    // Update is called once per frame
    public void StopAttackingPlayer()
    {
        if(isMelee)
        {
            if (enemyType == 0)
            {
                HitBox.SetActive(false);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;

        if(enemyHP <- 0 )
        {
            Debug.Log("enemy Died");
            anim.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            StartCoroutine(waitForDestroy(10f));
        }
        else
        {
            anim.SetTrigger("damage");
        }
    }

    IEnumerator waitForDestroy(float timer)
    {
        yield return new WaitForSeconds(timer);
        data.enemyDestroyed += 1;
        data.money += MoneyDrop;
        Destroy(gameObject);
        GameManager.score += 10;
    }
}
