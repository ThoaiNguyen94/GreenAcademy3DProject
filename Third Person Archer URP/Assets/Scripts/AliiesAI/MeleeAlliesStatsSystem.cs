using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAlliesStatsSystem : AlliesStatsSystem
{
    [SerializeField] private GameObject bloodSprayPrefab;
    [SerializeField] private Transform  bloodSprayParent;
    [SerializeField] private Transform  bloodSpraySpawn;

    private NavMeshAgent agent;
    private GameObject   bloodSprayVFX;
    //private bool         canAttack = false;
    //private bool         attackable = false;

    protected override void Update()
    {
        if (currentHealth == maxHealth)
        {
            healthCanvas.enabled = false;
        }
        else if (currentHealth < maxHealth)
        {
            healthCanvas.enabled = true;
        }

        timerCheckTarget += Time.deltaTime;
        if (timerCheckTarget >= timeCheckTarget)
        {
            timerCheckTarget = 0;
            closestTargetTransform = CheckClosestTarget(detectRange);
        }
    }


    private IEnumerator DisableMovement()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(1.2f);
        agent.enabled = true;
    }

    private void MeleeAttack()
    {
        Vector3 offsetSphere = new(0f, 0.8f, 0f);
        Vector3 spherePosition = (transform.position + offsetSphere) + (transform.forward * (attackRange / 2));
        int totalDamage = Mathf.FloorToInt(attackDamage);
        Collider[] colliders = Physics.OverlapSphere(spherePosition, attackRange / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemies"))
            {
                if(collider.GetComponent<EnemiesStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<EnemiesStatsSystem>().TakeDamage(totalDamage);
                    bloodSprayVFX = Instantiate(bloodSprayPrefab, bloodSpraySpawn.position, Quaternion.identity, bloodSprayParent);
                    Destroy(bloodSprayVFX, 2f);
                }
            }
        }
    }
}
