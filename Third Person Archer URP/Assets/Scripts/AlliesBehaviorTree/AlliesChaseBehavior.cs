using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlliesChaseBehavior : AlliesBehavior
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        alliesStats = animator.GetComponent<AlliesStatsSystem>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = alliesStats.runSpeed;
        agent.stoppingDistance = alliesStats.attackRange;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckClosestTarget(alliesStats.detectRange, animator);
        if (closestTargetTransform != null)
        {
            agent.SetDestination(closestTargetTransform.position);
            distanceToTarget = Vector3.Distance(closestTargetTransform.transform.position, animator.transform.position);
            if (distanceToTarget <= alliesStats.attackRange)
            {
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", true);
            }
            else
            {
                agent.SetDestination(closestTargetTransform.position);
                animator.SetBool("Attack", false);
                animator.SetBool("Chase", true);
            }
        }
        else
        {
            animator.SetBool("Chase", false);
            animator.SetBool("Patrol", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Chase", false);
    }
}
