using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackstate : StateMachineBehaviour
{
    Transform player;
    public float attackRange = 13;

    [SerializeField] private dragonScript dragon;
    [SerializeField] private krocoscript kroco;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("playeranim").transform;
        dragon = animator.GetComponent<dragonScript>();
        kroco = animator.GetComponent<krocoscript>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > attackRange)
        {
            animator.SetBool("IsAttacking", false);
        }
      }

// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
      {
        if (dragon != null)
        {
            dragon.StopAttackingPlayer();
        }

        if (kroco != null)
        {
            kroco.StopAttackingPlayer();

        }
    }

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
