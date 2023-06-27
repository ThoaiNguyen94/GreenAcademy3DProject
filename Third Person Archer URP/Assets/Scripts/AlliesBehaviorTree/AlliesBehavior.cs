using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlliesBehavior : StateMachineBehaviour
{
    protected AlliesStatsSystem alliesStats;
    protected NavMeshAgent agent;
    protected Transform closestTargetTransform;
    protected float distanceToTarget;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        alliesStats = animator.GetComponent<AlliesStatsSystem>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckClosestTarget(alliesStats.detectRange, animator);
    }

    protected Transform CheckClosestTarget(float detectRange, Animator animator)
    {
        closestTargetTransform = null;

        Collider[] colliders = Physics.OverlapSphere(animator.transform.position, detectRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemies"))
            {
                Transform target1 = colliders[i].transform;
                float distanceToTarget1 = Vector3.Distance(target1.position, animator.transform.position);
                closestTargetTransform = target1;

                // Check the next collider and compare the distance
                for (int j = 1; j < colliders.Length; j++)
                {
                    if (colliders[j].CompareTag("Enemies"))
                    {
                        Transform target2 = colliders[j].transform;
                        float distanceToTarget2 = Vector3.Distance(target2.position, animator.transform.position);
                        if (distanceToTarget1 > distanceToTarget2)
                        {
                            closestTargetTransform = target2;
                        }
                    }
                }
            }
        }
        return closestTargetTransform;
    }
}
