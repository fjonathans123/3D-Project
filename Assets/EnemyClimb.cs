using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyClimb : MonoBehaviour
{
    public float ClimbRange = 1.5f;
    public float maxClimbHeight = 2f;
    public LayerMask climbMask;
    public Transform climbCheckOrigin;

    private NavMeshAgent agent;
    private Animator anim;
    public bool isClimbing;

    private float climbCooldown = 1f;
    private float lastClimbTime = -10f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanClimb(out Vector3 climbPoint)
    {
        climbPoint = Vector3.zero;
        if (isClimbing || Time.time < lastClimbTime + climbCooldown)
            return false;

        RaycastHit hit;
        if(Physics.Raycast(climbCheckOrigin.position, transform.forward, out hit, ClimbRange, climbMask))
        {
            float heightDifference = hit.collider.bounds.max.y - transform.position.y;

            if(heightDifference <= maxClimbHeight)
            {
                Transform climbTarget = hit.collider.transform.Find("CLimbPoint");
                if (climbTarget != null)
                {
                    climbPoint = climbTarget.position;
                    return true;
                }
            }
        }

        return false;
    }

    public void StartClimb(Vector3 targetPosition, Animator animator)
    {
        if (!isClimbing)
        {
            lastClimbTime = Time.time;
            animator.SetBool("IsClimbing", true);
            StartCoroutine(enemyClimb(targetPosition, animator));
        }
    }

    IEnumerator enemyClimb (Vector3 targetPosition, Animator animator)
    {
        isClimbing = true;
        agent.isStopped = true;
        agent.updatePosition = false;

        yield return new WaitForSeconds(0.5f);

        float climbDuration = 1.0f;
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while(elapsed < climbDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / climbDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(0.2f);
        agent.Warp(targetPosition);
        agent.updatePosition = true;
        agent.isStopped = false;

        isClimbing = false;
        animator.SetBool("IsClimbing", false);
    }
}
