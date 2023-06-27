using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackBehavior : StateMachineBehaviour
{
    private EnemiesStatsSystem enemiesStats;
    private float distanceToTarget;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemiesStats = animator.GetComponent<EnemiesStatsSystem>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemiesStats.closestTargetTransform != null)
        {
            animator.transform.LookAt(enemiesStats.closestTargetTransform);
            distanceToTarget = Vector3.Distance(enemiesStats.closestTargetTransform.position, animator.transform.position);
            if (distanceToTarget > enemiesStats.attackRange)
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Chase", true);
            }
        }
        else
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Patrol", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
}
