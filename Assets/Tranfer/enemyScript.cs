using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    public int enemyHP = 3;
    private GameManager gm;
    public gunScript gun;
    public gameData data;

    public GameObject fire, ice, lightning;
    private ElementController element; 

    //Enemy AI
    public NavMeshAgent agent;
    public Transform player, LookAtPlayer;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttack;
    bool alreadyAttack;
    public GameObject bullet;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Transform enemyGun;

    private Animator anim;

    public bool isHitting = false;

    private Rigidbody rb;
    public float meleeForce;

    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

private void Update()
{
    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


    if(enemyHP > 0)
        {
            if (playerInSightRange && !playerInAttackRange) Chasing();
            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && playerInAttackRange) Attack();
        }

    if (enemyHP <= 0)
        {
            Debug.Log("enemy is dead");
            anim.SetBool("IsDead", true);
            StartCoroutine(waitingTime());
           
        }
    }

IEnumerator waitingTime()
    {
        yield return new WaitForSeconds(5);
        EnemyDie();
    }

    private void Chasing()
{
    agent.SetDestination(player.position);
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(LookAtPlayer);

        if(!alreadyAttack)
        {
            anim.SetBool("Shooting", true);
            Rigidbody rb = Instantiate(bullet, enemyGun.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);
            alreadyAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void ResetAttack()
    {
        alreadyAttack = false;
        anim.SetBool("Shooting", false);
    }

    private void Patrolling()
{
    if (!walkPointSet) SearchWalkPoint();
    if (walkPointSet) agent.SetDestination(walkPoint);
    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

private void SearchWalkPoint()
{
    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    float randomX = Random.Range(-walkPointRange, walkPointRange);

    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
}
void Start()
    {
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        element = GameObject.FindGameObjectWithTag("Player").GetComponent<ElementController>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(enemyHP);
        enemyHP -= damage;

        if((isHitting == true))
        {
            Vector3 dir = (transform.position - player.transform.position).normalized;
            rb.AddForce(dir * meleeForce, ForceMode.Impulse);

            if(Time.time > 0.25f)
            {
                rb.velocity = Vector3.zero;
                isHitting = false;
            }
        }
        else
        {

            if (element.element == 1)
            {
                fire.SetActive(false);
                ice.SetActive(false);
                lightning.SetActive(false);
            }

            if (element.element == 2)
            {
                fire.SetActive(true);
                ice.SetActive(false);
                lightning.SetActive(false);
            }

            if (element.element == 3)
            {
                fire.SetActive(false);
                ice.SetActive(true);
                lightning.SetActive(false);
            }

            if (element.element == 4)
            {
                fire.SetActive(false);
                ice.SetActive(false);
                lightning.SetActive(true);
            }
        }

    }

    void EnemyDie()
    {
        data.enemyDestroyed += 1;
        data.money += 10;
        Destroy(gameObject);
        GameManager.score += 100;
    }

}
