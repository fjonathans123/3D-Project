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

        Vector3 distanceToPlayer = (player.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, distanceToPlayer);
        if (dot < 0.5f)
            return false; 

        RaycastHit hit;
        if(Physics.Raycast(climbCheckOrigin.position, transform.forward, out hit, ClimbRange, climbMask))
        {
            lastWallHit = hit.point;
            Vector3 climbCheckStart = hit.point + Vector3.up * maxClimbHeight;

            RaycastHit climbHit;
            if(Physics.Raycast(climbCheckStart, Vector3.down, out climbHit, maxClimbHeight + 1f, climbMask))
            {
                lastLedgeHit = climbHit.point;
                float heightDifference = climbHit.point.y - transform.position.y;
                if(heightDifference <= maxClimbHeight && heightDifference > 0.2f)
                {
                    climbPoint = climbHit.point + transform.forward * 2f;
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
            //transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / climbDuration);

            float t = elapsed / climbDuration;
            float heightOffset = Mathf.Sin(Mathf.PI * t) * 0.5f;
            Vector3 position = Vector3.Lerp(startPosition, targetPosition, t);
            position.y += heightOffset;
            transform.position = position;

            if(player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0f;
                if(direction.sqrMagnitude > 0.001f)
                {
                    Quaternion lookRot = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        lookRot,
                        Time.deltaTime *5f);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Vector3 finalposition = targetPosition + transform.forward * .5f;
        transform.position = finalposition;

        yield return new WaitForSeconds(0.2f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(finalposition, out hit,1f, NavMesh.AllAreas))
        {
            finalposition = hit.position;
        }
        agent.Warp(finalposition);
        NavMeshHit hit2;
        if (NavMesh.SamplePosition(finalposition, out hit2, 1f, NavMesh.AllAreas))
        {
            agent.Warp(hit2.position);
        }
        else
        {
            Debug.LogWarning("[Enemy Climb] no valid navmesh , retrying recovery");
            isClimbing = false;
            yield break;
        }

        yield return null;
        agent.ResetPath();
        agent.updatePosition = true;
        agent.isStopped = false;

        isClimbing = false;
        animator.SetBool("IsClimbing", false);
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
