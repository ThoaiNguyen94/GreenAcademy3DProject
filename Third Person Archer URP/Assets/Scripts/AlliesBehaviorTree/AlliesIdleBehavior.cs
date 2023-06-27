using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliesIdleBehavior : AlliesBehavior
{
    private float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 0.5)
        {
            animator.SetBool("Patrol", true);
        }
    }
}
