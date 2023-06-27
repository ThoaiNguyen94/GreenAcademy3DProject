using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBossStatsSystem : EnemiesStatsSystem
{
    [SerializeField] private GameObject bloodSprayPrefab;
    [SerializeField] private Transform  bloodSprayParent;
    [SerializeField] private Transform  bloodSpraySpawn;
    
    private GameObject bloodSprayVFX;
    private bool       canAttack = false;

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

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackState"))
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        timerCheckTarget += Time.deltaTime;
        if (timerCheckTarget >= timeCheckTarget)
        {
            timerCheckTarget = 0;
            closestTargetTransform = CheckClosestTarget(detectRange);
        }
    }

    private void MeleeAttackHit()
    {
        Vector3 offsetSphere = new(0f, 1.7f, 0f);
        Vector3 spherePosition = (transform.position + offsetSphere) + (transform.forward * (attackRange / 2));
        //StartCoroutine(DisableMovement());
        int totalDamage = attackDamage;
        Collider[] colliders = Physics.OverlapSphere(spherePosition, attackRange / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && canAttack)
            {
                if (collider.GetComponent<PlayerStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<PlayerStatsSystem>().TakeDamage(totalDamage);
                    Vector3 bloodSpray = new(0, 0.8f, 0);
                    bloodSprayVFX = Instantiate(bloodSprayPrefab, collider.transform.position + bloodSpray, Quaternion.identity, bloodSprayParent);
                    Destroy(bloodSprayVFX, 2f);
                    GetComponent<EnemiesAudioManager>().EnemiesSound("Attack");
                }
            }
            if (collider.CompareTag("Allies") && canAttack)
            {
                if (collider.GetComponent<AlliesStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<AlliesStatsSystem>().TakeDamage(totalDamage);
                    Vector3 bloodSpray = new(0, 0.8f, 0);
                    bloodSprayVFX = Instantiate(bloodSprayPrefab, collider.transform.position + bloodSpray, Quaternion.identity, bloodSprayParent);
                    Destroy(bloodSprayVFX, 2f);
                    GetComponent<EnemiesAudioManager>().EnemiesSound("Attack");
                }
            }
        }
    }

    //private readonly Vector3 offSphere = new(0f, 0.8f, 0f);
    //// Draw a sphere to debug attack range in scene
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 6f);
    //}
}
