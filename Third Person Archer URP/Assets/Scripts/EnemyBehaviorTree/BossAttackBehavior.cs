using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttackBehavior : StateMachineBehaviour
{
    private EnemiesStatsSystem enemiesStats;
    private float distanceToTarget;
    private int skillChangeValue;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemiesStats = animator.GetComponent<EnemiesStatsSystem>();
        if (enemiesStats.closestTargetTransform != null)
        {
            skillChangeValue = SkillChange();
            if (skillChangeValue < 41)
            {
                animator.SetTrigger("BrutalClash");
            }
            //if (skillChangeValue < 102)
            //{
            //    animator.SetTrigger("BrutalClash");
            //}
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemiesStats.closestTargetTransform != null)
        {
            animator.transform.LookAt(enemiesStats.closestTargetTransform.transform);
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

    private int SkillChange()
    {
        int randomNumber = Random.Range(1, 101);
        return randomNumber;
    }
}
