using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SingleStrikeAbility : CharacterAbility
{
    [SerializeField] private GameObject singleStrikeVFX;

    #region Private Variables
    private PlayerStatsSystem playerStats;
    private PlayerMovement    playerMovement;
    private PlayerInput       playerInput;
    private InputAction       singleStrikeAction;
    private InputAction       runAction;
    private MeleeAttack       meleeAttack;
    private GameObject        normalSlashHit;
    #endregion

    protected override void Start()
    {
        playerStats        = GetComponent<PlayerStatsSystem>();
        playerMovement     = GetComponent<PlayerMovement>();
        animator           = GetComponent<Animator>();
        playerInput        = GetComponent<PlayerInput>();
        meleeAttack        = GetComponent<MeleeAttack>();

        singleStrikeAction = playerInput.actions["Skill-2"];
        runAction          = playerInput.actions["Run"];
        
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
                    if (singleStrikeAction.IsPressed() && !runAction.IsPressed() && timer <= 0 && skillData.isUpgrade == true)
                    {
                        playerStats.currentMana -= skillData.manaCost;
                        playerStats.UpdateCurrentMana(playerStats.maxMana, playerStats.currentMana);
                        animator.SetTrigger("SingleStrike");
                        StartCoroutine(DisableMeleeAttack());
                        StartCoroutine(DisableMovement());
                        state = AbilityState.cooldown;
                        timer = skillData.cooldownTime;
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

    private void SingleStrikeAttack()
    {
        Vector3 spherePosition = (transform.position + offsetSphere) + (transform.forward * (skillData.skillRange / 2));
        WeaponSystem weaponStats = playerStats.GetComponent<WeaponSystem>();
        int totalDamage = Mathf.FloorToInt(playerStats.attackDamage * skillData.multipleAttack) + weaponStats.weaponDamage;
        Collider[] colliders = Physics.OverlapSphere(spherePosition, skillData.skillRange / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemies"))
            {
                collider.GetComponent<EnemiesStatsSystem>().TakeDamage(totalDamage);
            }
        }
        Vector3 forwardOffest = transform.forward;
        Vector3 offset = new(0f, 1f, 0f);
        normalSlashHit = Instantiate(singleStrikeVFX, transform.position + offset + forwardOffest, Quaternion.identity);
        Destroy(normalSlashHit, 1f);
    }

    private IEnumerator DisableMovement()
    {
        playerMovement.enabled = false;
        yield return new WaitForSeconds(1.08f);
        playerMovement.enabled = true;
    }

    private IEnumerator DisableMeleeAttack()
    {
        meleeAttack.enabled = false;
        yield return new WaitForSeconds(1.3f);
        meleeAttack.enabled = true;
    }

    private void SingleStrikeSound()
    {
        GetComponent<PlayerAudioManager>().PlayerSound("SingleStrike");
    }
}
