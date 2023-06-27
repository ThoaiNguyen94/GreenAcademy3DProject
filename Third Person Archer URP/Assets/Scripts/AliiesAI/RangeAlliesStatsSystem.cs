using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAlliesStatsSystem : AlliesStatsSystem
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform  barrelTransform;
    [SerializeField] private Transform  projectileParent;

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

    public void AlliesTriggerRangeDamage()
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
            AlliesProjectileController alliesProjectile = projectile.GetComponent<AlliesProjectileController>();
            if (Physics.Raycast(ray.origin, ray.direction, attackRange))
            {
                alliesProjectile.target = ray.origin + ray.direction * attackRange * 10;
                alliesProjectile.hit = true;
            }
            GetComponent<PlayerAudioManager>().PlayerSound("Attack");
        }
        
    }
}
