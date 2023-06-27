using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlliesPatrolBehavior : AlliesBehavior
{
    private Transform enemyBaseTransform;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        alliesStats = animator.GetComponent<AlliesStatsSystem>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = alliesStats.moveSpeed;
        enemyBaseTransform = GameObject.FindGameObjectWithTag("EnemyBase").transform;
        agent.SetDestination(enemyBaseTransform.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckClosestTarget(alliesStats.detectRange, animator);
        if (closestTargetTransform != null)
        {
            animator.SetBool("Patrol", false);
            animator.SetBool("Chase", true);
        }
        else
        {
            agent.SetDestination(enemyBaseTransform.position);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Patrol", false);
    }
}
