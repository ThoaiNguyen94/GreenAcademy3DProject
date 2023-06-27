using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseBehavior : StateMachineBehaviour
{
    private EnemiesStatsSystem enemiesStats;
    private NavMeshAgent agent;
    private float distanceToTarget;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemiesStats = animator.GetComponent<EnemiesStatsSystem>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = enemiesStats.runSpeed;
        agent.stoppingDistance = enemiesStats.attackRange;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemiesStats.closestTargetTransform != null)
        {
            agent.SetDestination(enemiesStats.closestTargetTransform.position);
            distanceToTarget = Vector3.Distance(enemiesStats.closestTargetTransform.position, animator.transform.position);
            if (distanceToTarget <= enemiesStats.attackRange)
            {
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", true);
            }
            else
            {
                agent.SetDestination(enemiesStats.closestTargetTransform.position);
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
