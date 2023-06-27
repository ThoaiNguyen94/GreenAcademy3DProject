using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliesAttackBehavior : AlliesBehavior
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        alliesStats = animator.GetComponent<AlliesStatsSystem>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckClosestTarget(alliesStats.detectRange, animator);
        if (closestTargetTransform != null)
        {
            animator.transform.LookAt(closestTargetTransform.transform);
            distanceToTarget = Vector3.Distance(closestTargetTransform.transform.position, animator.transform.position);
            if (distanceToTarget > alliesStats.attackRange)
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
