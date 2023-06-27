using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemiesStatsSystem : EnemiesStatsSystem
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform projectileParent;

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

    public void EnemyTriggerRangeDamage()
    {
        Transform target = closestTargetTransform;
        if(target != null)
        {
            Vector3 upperTarget = new(target.position.x, target.position.y + 1, target.position.z);
            Ray ray = new()
            {
                origin = barrelTransform.position,
                direction = upperTarget - barrelTransform.position
            };
            Quaternion bulletDirection = Quaternion.Euler(ray.direction);
            GameObject projectile = Instantiate(projectilePrefab, ray.origin, bulletDirection, projectileParent);
            EnemyProjectileController enemyProjectile = projectile.GetComponent<EnemyProjectileController>();
            if (Physics.Raycast(ray.origin, ray.direction, attackRange))
            {
                enemyProjectile.target = ray.origin + ray.direction * attackRange * 10;
                enemyProjectile.hit = true;
            }
            GetComponent<EnemiesAudioManager>().EnemiesSound("Attack");
        }
    }
}
