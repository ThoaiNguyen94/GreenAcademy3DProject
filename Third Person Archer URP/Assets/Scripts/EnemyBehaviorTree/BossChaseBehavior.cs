using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossChaseBehavior : StateMachineBehaviour
{
    private EnemiesStatsSystem enemiesStats;
    private NavMeshAgent agent;
    private float distanceToTarget;
    private int skillChangeValue;
    private readonly float jumpAttackRange = 12;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemiesStats = animator.GetComponent<EnemiesStatsSystem>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = enemiesStats.runSpeed;
        agent.stoppingDistance = enemiesStats.attackRange;
        if (enemiesStats.closestTargetTransform != null)
        {
            skillChangeValue = SkillChange();
            if (skillChangeValue < 31)
            {
                if (distanceToTarget < jumpAttackRange)
                {
                    animator.SetTrigger("JumpAttack");
                }
            }
        }
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

    private int SkillChange()
    {
        int randomNumber = Random.Range(1, 101);
        return randomNumber;
    }
}
