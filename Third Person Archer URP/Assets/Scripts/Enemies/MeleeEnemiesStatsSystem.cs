using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemiesStatsSystem : EnemiesStatsSystem
{
    [SerializeField] private GameObject bloodSprayPrefab;
    [SerializeField] private Transform bloodSprayParent;
    [SerializeField] private Transform bloodSpraySpawn;

    private GameObject bloodSprayVFX;

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

    private void EnemiesMeleeAttack()
    {
        Vector3 offsetSphere = new(0f, 0.8f, 0f);
        Vector3 spherePosition = (transform.position + offsetSphere) + (transform.forward * (attackRange / 2));
        int totalDamage = Mathf.FloorToInt(attackDamage);
        Collider[] colliders = Physics.OverlapSphere(spherePosition, attackRange / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if(collider.GetComponent<PlayerStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<PlayerStatsSystem>().TakeDamage(totalDamage);
                    bloodSprayVFX = Instantiate(bloodSprayPrefab, bloodSpraySpawn.position, Quaternion.identity, bloodSprayParent);
                    Destroy(bloodSprayVFX, 2f);
                }
            }
            else if (collider.CompareTag("Allies"))
            {
                if (collider.GetComponent<AlliesStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<AlliesStatsSystem>().TakeDamage(totalDamage);
                    bloodSprayVFX = Instantiate(bloodSprayPrefab, bloodSpraySpawn.position, Quaternion.identity, bloodSprayParent);
                    Destroy(bloodSprayVFX, 2f);
                }
            }
        }
    }

    //private readonly Vector3 offSphere = new(0f, 0.8f, 0f);
    //// Draw a sphere to debug attack range in scene
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere((transform.position + offSphere) + transform.forward * 2 / 2, 2 / 2);
    //}
}
