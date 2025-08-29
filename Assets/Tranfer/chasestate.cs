using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chasestate : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    public float chaseRange = 13;
    public float attackRange = 2.5f;
    public float chaseSpeed;

    public float rotationSpeed;

    EnemyClimb climb;

    public LayerMask climbMask;

    private float climbCooldown = 1f;
    private float lastClimbTime = -10f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        climb = animator.GetComponent<EnemyClimb>();
        player = GameObject.FindGameObjectWithTag("playeranim").transform;
        agent.speed = chaseSpeed;
        agent.isStopped = false;
        //agent.speed = 3.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 climbPoint;
        if (climb != null && !climb.isClimbing && climb.CanClimb(out climbPoint))
        {
            climb.StartClimb(climbPoint, animator);
            return;
        }
        float distance = Vector3.Distance(player.position, animator.transform.position);


        if (distance > chaseRange)
        {
            if (animator.GetBool("IsChasing"))
                animator.SetBool("IsChasing", false);
            if (animator.GetBool("IsPatroling"))
                animator.SetBool("IsPatroling", false);
        }

        agent.SetDestination(player.position);

        Vector3 dirToPlayer = (player.position - animator.transform.position).normalized;

        if (dirToPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dirToPlayer);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        if (distance < attackRange)
        {
            if (!animator.GetBool("IsAttacking"))
                animator.SetBool("IsAttacking", true);
            if (animator.GetBool("IsChasing"))
                animator.SetBool("IsChasing", false);
        }
    }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
        agent.isStopped = true;
    }

    
    //    if(climb!= null && climb.checkClimb())
    //    {
    //        return;
    //    }

    //    agent.SetDestination(player.position);
    //    float distance = Vector3.Distance(player.position, animator.transform.position);
    //    if (distance > chaseRange)
    //    {
    //        animator.SetBool("IsChasing", false);
    //    }
    //    if (distance < attackRange)
    //    {
    //        animator.SetBool("IsAttacking", true);
    //        animator.SetBool("IsChasing", false);
    //    }

    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state


    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
