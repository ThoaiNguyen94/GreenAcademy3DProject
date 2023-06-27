using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerShotAbility : CharacterAbility
{
    #region Serialize Private Varibles
    [SerializeField] private GameObject powerArrowPrefab;
    #endregion

    #region Private Variables
    private PlayerStatsSystem playerStats;
    private PlayerInput       playerInput;
    private InputAction       aimAction;
    private InputAction       runAction;
    private InputAction       powerShotAction;
    private Transform         barrelTransform;
    private Transform         crossHairTarget;
    private Transform         arrowParent;
    #endregion

    public Ray ray;

    protected override void Start()
    {
        playerStats = GetComponent<PlayerStatsSystem>();
        animator    = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        aimAction       = playerInput.actions["Aim"];
        runAction       = playerInput.actions["Run"];
        powerShotAction = playerInput.actions["Skill-3"];

        barrelTransform = GameObject.FindGameObjectWithTag("Barrel").transform;
        crossHairTarget = GameObject.FindGameObjectWithTag("CrossHairTarget").transform;
        arrowParent     = GameObject.FindGameObjectWithTag("ArrowParent").transform;

        skillCooldownImage.enabled = false;
        skillCooldownImage.fillAmount = 1;
        skillNoManaImage.enabled = false;
    }

    protected override void Update()
    {
        // Cooldown time still decrease independence with mana
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            skillCooldownImage.enabled = true;
            skillCooldownImage.fillAmount -= (1 / skillData.cooldownTime) * Time.deltaTime;
        }
        else
        {
            skillCooldownImage.enabled = false;
            skillCooldownImage.fillAmount = 1;
        }

        // Perform skill when enough mana and cooldown time <= 0
        if (playerStats.currentMana >= skillData.manaCost)
        {
            skillNoManaImage.enabled = false;
            switch (state)
            {
                case AbilityState.ready:
                    if(aimAction.IsPressed() && !runAction.IsPressed() && timer <= 0 && skillData.isUpgrade == true)
                    {
                        if (powerShotAction.IsPressed())
                        {
                            playerStats.currentMana -= skillData.manaCost;
                            playerStats.UpdateCurrentMana(playerStats.maxMana, playerStats.currentMana);
                            animator.SetTrigger("PowerShot");
                            PowerShotActivate();
                            StartCoroutine(PowerShotDeactivate());
                            timer = skillData.cooldownTime;
                            state = AbilityState.cooldown;
                        }
                    }
                    break;

                case AbilityState.cooldown:
                    state = AbilityState.ready;
                    break;
            }
        }
        else
        {
            skillNoManaImage.enabled = true;
        }
    }

    private void PowerShotActivate()
    {
        playerStats.attackDamage *= Mathf.FloorToInt(skillData.multipleAttack);
        if (barrelTransform.position != null && crossHairTarget.position != null)
        {
            ray.origin = barrelTransform.position;
            ray.direction = crossHairTarget.position - barrelTransform.position;
        }
        Quaternion arrowDirection = Quaternion.Euler(ray.direction);

        GameObject powerArrow1 = Instantiate(powerArrowPrefab, ray.origin + Vector3.forward, arrowDirection, arrowParent);
        GameObject powerArrow2 = Instantiate(powerArrowPrefab, ray.origin + Vector3.right, arrowDirection, arrowParent);
        GameObject powerArrow3 = Instantiate(powerArrowPrefab, ray.origin + Vector3.right + Vector3.right, arrowDirection, arrowParent);
        GameObject powerArrow4 = Instantiate(powerArrowPrefab, ray.origin + Vector3.left, arrowDirection, arrowParent);
        GameObject powerArrow5 = Instantiate(powerArrowPrefab, ray.origin + Vector3.left + Vector3.left, arrowDirection, arrowParent);

        PowerArrowController powerArrowController1 = powerArrow1.GetComponent<PowerArrowController>();
        PowerArrowController powerArrowController2 = powerArrow2.GetComponent<PowerArrowController>();
        PowerArrowController powerArrowController3 = powerArrow3.GetComponent<PowerArrowController>();
        PowerArrowController powerArrowController4 = powerArrow4.GetComponent<PowerArrowController>();
        PowerArrowController powerArrowController5 = powerArrow5.GetComponent<PowerArrowController>();

        if (Physics.Raycast(ray.origin, ray.direction, skillData.skillRange))
        {
            // Arrow 1
            powerArrowController1.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController1.hit = true;

            // Arrow 2
            powerArrowController2.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController2.hit = true;

            // Arrow 3
            powerArrowController3.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController3.hit = true;

            // Arrow 4
            powerArrowController4.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController4.hit = true;

            // Arrow 5
            powerArrowController5.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController5.hit = true;
        }
        else
        {
            // Arrow 1
            powerArrowController1.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController1.hit = false;

            // Arrow 2
            powerArrowController2.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController2.hit = false;

            // Arrow 3
            powerArrowController3.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController3.hit = false;

            // Arrow 4
            powerArrowController4.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController4.hit = false;

            // Arrow 5
            powerArrowController5.target = ray.origin + ray.direction * skillData.skillRange * 10;
            powerArrowController5.hit = false;
        }
    }

    private IEnumerator PowerShotDeactivate()
    {
        yield return new WaitForSeconds(1);
        playerStats.attackDamage /= Mathf.FloorToInt(skillData.multipleAttack);
    }

    private void PowerShotSound()
    {
        GetComponent<PlayerAudioManager>().PlayerSound("FirePowerArrow");
    }
}
