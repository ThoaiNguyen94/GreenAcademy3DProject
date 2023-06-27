using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolBehavior : StateMachineBehaviour
{
    private EnemiesStatsSystem enemiesStats;
    private NavMeshAgent agent;
    private Transform playerBaseTransform;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemiesStats = animator.GetComponent<EnemiesStatsSystem>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = enemiesStats.moveSpeed;
        playerBaseTransform = GameObject.FindGameObjectWithTag("PlayerBase").transform;
        agent.SetDestination(playerBaseTransform.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemiesStats.closestTargetTransform != null)
        {
            animator.SetBool("Patrol", false);
            animator.SetBool("Chase", true);
        }
        else
        {
            agent.SetDestination(playerBaseTransform.position);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Patrol", false);
    }
}
