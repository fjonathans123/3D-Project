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

    private Vector3 lastWallHit;
    private Vector3 lastLedgeHit;
    public Transform player;

    private Vector3 lastPosition;
    private float stuckTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        StuckChecker();
        if(!isClimbing && player != null)
        {
            if(!agent.hasPath || agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Vector3 climbPoint;
                if(CanClimb(out climbPoint))
                {
                    StartClimb(climbPoint, anim);
                }
            }
        }
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
                }
                else
                {
                    climbPoint = hit.point + transform.forward * 1.2f;
                    climbPoint.y = transform.position.y;
                }
                return true;
            }
        }

        return false;
    }

    public void StartClimb(Vector3 targetPosition, Animator animator)
    {
        if (!isClimbing)
        {
            lastClimbTime = Time.time;
            animator.SetTrigger("IsClimbing");
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
            //transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / climbDuration);

            float t = elapsed / climbDuration;
            float heightOffset = Mathf.Sin(Mathf.PI * t) * 0.5f;
            Vector3 position = Vector3.Lerp(startPosition, targetPosition, t);
            position.y += heightOffset;
            transform.position = position;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(0.2f);
        agent.Warp(targetPosition);
        agent.updatePosition = true;
        agent.isStopped = false;

        isClimbing = false;
        animator.SetTrigger("IsClimbing");
    }

    private void StuckChecker()
    {
        if (isClimbing || !agent.enabled) return;

        float distanceMove = (transform.position - lastPosition).magnitude;
        if(agent.hasPath && !agent.pathPending && distanceMove < 0.05f && agent.velocity.sqrMagnitude < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if(stuckTimer > 1.0f)
            {
                NavMeshHit navhit;
                if(NavMesh.SamplePosition(transform.position, out navhit, 2f, NavMesh.AllAreas))
                {
                    agent.Warp(navhit.position);
                    if(!agent.pathPending && !agent.hasPath)
                    {
                        agent.SetDestination(player.position);  
                    }
                    agent.ResetPath();
                    agent.isStopped = false;

                    if(player != null)
                    {
                        agent.SetDestination(player.position);
                    }
                }
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;

        }
        lastPosition = transform.position;
    }
}
