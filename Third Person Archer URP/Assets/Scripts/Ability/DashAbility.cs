using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DashAbility : CharacterAbility
{
    [SerializeField] private GameObject dashVFX;
    private GameObject dashObject;

    private PlayerStatsSystem      playerStats;
    private PlayerInput            playerInput;
    private InputAction            dashAction;
    private InputAction            runAction;

    private readonly float         dashVelocity = 4;

    protected override void Start()
    {
        playerStats = GetComponentInParent<PlayerStatsSystem>();
        animator    = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        dashAction  = playerInput.actions["Skill-1"];
        runAction   = playerInput.actions["Run"];

        
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
                    if (dashAction.IsPressed() && !runAction.IsPressed() && timer <= 0 && skillData.isUpgrade == true)
                    {
                        playerStats.currentMana -= skillData.manaCost;
                        playerStats.UpdateCurrentMana(playerStats.maxMana, playerStats.currentMana);
                        animator.SetTrigger("Dash");
                        DashActivate();
                        StartCoroutine(DashDeactivate());
                        timer = skillData.cooldownTime;
                        state = AbilityState.cooldown;
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

    private void DashActivate()
    {
        playerStats.moveSpeed *= dashVelocity;
        dashObject = Instantiate(dashVFX, transform.position, Quaternion.identity);
        Destroy(dashObject, 1);
    }

    private IEnumerator DashDeactivate()
    {
        yield return new WaitForSeconds(0.3f);
        playerStats.moveSpeed /= dashVelocity;
    }

    private void DashSound()
    {
        GetComponent<PlayerAudioManager>().PlayerSound("Dash");
    }
}
