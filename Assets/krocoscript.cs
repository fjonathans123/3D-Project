using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class krocoscript : MonoBehaviour
{
    public int enemyType, damage, enemyHP;
    bool alreadyAttack;
    public bool isMelee;
    public GameObject HitBox;
    public Animator anim;
    private gameData data;
    public GameObject fire, lightning;
    private ElementController element;
    public FreezeEffect[] freeze;

    public bool isFrozen = false;
    NavMeshAgent agent;
    public float normalSpeed = 1f;
    private float originalAnimSpeed;
    public float slowMotionSpeed;
    public GameObject bullet;
    public Transform enemyRangeBullet;
    public float bulletHeight, bulletRange;

    private Coroutine currentDPSCoroutine;

    public int MoneyDrop = 0;
    // Start is called before the first frame update
    void Start()
    {
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        element = GameObject.FindGameObjectWithTag("Player").GetComponent<ElementController>();
        agent = GetComponent<NavMeshAgent>();
        normalSpeed = agent.speed;
        originalAnimSpeed = anim.speed;
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

        else if (enemyType == 1)
        {
            if (!isMelee)
            {
                Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                Vector3 direction = (playerPosition - enemyRangeBullet.position).normalized;
                Rigidbody rb = Instantiate(bullet, enemyRangeBullet.transform.position, Quaternion.LookRotation(direction)).GetComponent<Rigidbody>();
                rb.AddForce(direction * bulletRange, ForceMode.Impulse);
                rb.AddForce(Vector3.up * bulletHeight, ForceMode.Impulse);

                alreadyAttack = true;
            }
        }
    }

    public void resetAttack()
    {
        alreadyAttack = false;
        //anim.SetBool("IsAttacking", false);
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

        if(enemyHP <= 0 )
        {
            Debug.Log("enemy Died");
            anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            StartCoroutine(waitForDestroy(10f));
        }
        else
        {
            anim.SetTrigger("Damage");
            if(element.element == 1)
            {
                fire.SetActive(false);
                lightning.SetActive(false);
                SetFreezeAllMaterial(false);
            }
            if (element.element == 2)
            {
                if (currentDPSCoroutine != null)
                {
                    StopCoroutine(currentDPSCoroutine);
                    CancelInvoke("fireDPS");
                }
                currentDPSCoroutine = StartCoroutine(DPSDuration(6));
            }
            if (element.element == 3)
            {
                SetFreezeAllMaterial(true);
                if (currentDPSCoroutine != null)
                {
                    StopCoroutine(currentDPSCoroutine);
                    CancelInvoke("iceDPS");
                }
                currentDPSCoroutine = StartCoroutine(DPSDuration(6));
            }
            if (element.element == 4)
            {
                StartCoroutine(DPSDuration(6));
            }
        }
    }
    void iceDPS()
    {
        fire.SetActive(false);
        lightning.SetActive(false);
        enemyHP -= 1;
        if(!isFrozen)
        {
            agent.speed = normalSpeed * slowMotionSpeed;
            anim.speed = originalAnimSpeed * slowMotionSpeed;
            isFrozen = true;
        }
    }

    void SetFreezeAllMaterial(bool value)
    {
        if (freeze != null)
        {
            foreach (var ice in freeze)
            {
                if(ice != null)
                {
                    ice.isFrozen = value;
                }
            }
        }
    }

    void lightningDPS()
    {
        fire.SetActive(false);
        lightning.SetActive(true);
        SetFreezeAllMaterial(false);
        enemyHP -= 3;
    }

    IEnumerator DPSDuration(float timer)
    {
        if(element.element == 2)
        {
            CancelInvoke("fireDPS");
            InvokeRepeating("fireDPS", 1.5f, 2.0f);
            yield return new WaitForSeconds(timer);
            CancelInvoke("fireDPS");
            hideElement();
            currentDPSCoroutine = null;

        }
        if (element.element == 3)
        {
            CancelInvoke("iceDPS");
            InvokeRepeating("iceDPS", 1.5f, 2.0f);
            yield return new WaitForSeconds(timer);
            CancelInvoke("iceDPS");
            if (isFrozen)
            {
                agent.speed = normalSpeed;
                anim.speed = originalAnimSpeed;
                isFrozen = false;
            }
            hideElement();
            currentDPSCoroutine = null;
        }
        if (element.element == 4)
        {
            InvokeRepeating("lightningDPS", 1.5f, 2.0f);
            agent.speed = 0;
            yield return new WaitForSeconds(timer);
            CancelInvoke("lightningDPS");
            agent.speed = normalSpeed;
            hideElement();
        }
    }

    void hideElement()
    {
        fire.SetActive(false);
        lightning.SetActive(false);
        SetFreezeAllMaterial(false);
    }

    void fireDPS()
    {
        fire.SetActive(true);
        lightning.SetActive(false);
        SetFreezeAllMaterial(false);
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
