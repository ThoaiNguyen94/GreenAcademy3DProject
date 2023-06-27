using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RoundKickAbility : CharacterAbility
{
    [SerializeField] private GameObject kickBackVFX;

    #region Private Variables
    private PlayerStatsSystem playerStats;
    private PlayerMovement    playerMovement;
    private PlayerInput       playerInput;
    private InputAction       roundKickAction;
    private InputAction       runAction;
    private GameObject        kickBackHit;
    private readonly float knockBackDistance = 4;
    private readonly float knockBackDuration = 1;
    #endregion

    protected override void Start()
    {
        playerStats    = GetComponent<PlayerStatsSystem>();
        playerMovement = GetComponent<PlayerMovement>();
        animator       = GetComponent<Animator>();
        playerInput    = GetComponent<PlayerInput>();

        roundKickAction = playerInput.actions["Skill-2"];
        runAction       = playerInput.actions["Run"];

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
                    if (roundKickAction.IsPressed() && !runAction.IsPressed() && timer <= 0 && skillData.isUpgrade == true)
                    {
                        playerStats.currentMana -= skillData.manaCost;
                        playerStats.UpdateCurrentMana(playerStats.maxMana, playerStats.currentMana);
                        animator.SetTrigger("RoundKick");
                        StartCoroutine(DisableMovement());
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

    // Use in animation event
    private void RoundKickAttack()
    {
        Vector3 spherePosition = (transform.position + offsetSphere) + (transform.forward * (skillData.skillRange / 2));
        int totalDamage = Mathf.FloorToInt(playerStats.attackDamage * skillData.multipleAttack);
        Collider[] colliders = Physics.OverlapSphere(spherePosition, skillData.skillRange / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemies"))
            {
                KnockBack(collider.transform, knockBackDistance, knockBackDuration);
                collider.GetComponent<EnemiesStatsSystem>().TakeDamage(totalDamage);
            }
        }
        Vector3 forwardOffest = transform.forward;
        Vector3 offset = new(0f, 1f, 0f);
        kickBackHit = Instantiate(kickBackVFX, transform.position + offset + forwardOffest, Quaternion.identity);
        Destroy(kickBackHit, 1f);
        KickBackSound();
    }

    private IEnumerator DisableMovement()
    {
        playerMovement.enabled = false;
        yield return new WaitForSeconds(0.91f);
        playerMovement.enabled = true;
    }

    private void KickBackSound()
    {
        GetComponent<PlayerAudioManager>().PlayerSound("RoundKick");
    }

    private void KnockBack(Transform enemyTransform, float distance, float duration)
    {
        Vector3 direction = enemyTransform.position - transform.position;
        direction.y = 0f; // Set y component to zero to prevent enemy from flying upwards

        StartCoroutine(MoveEnemy(enemyTransform, direction.normalized * distance, duration));
    }

    private IEnumerator MoveEnemy(Transform enemyTransform, Vector3 direction, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = enemyTransform.position;

        while (elapsedTime < duration) // loop as long as elapsedTime less than duration
        {
            enemyTransform.Translate(direction * Time.deltaTime / duration, Space.World); // move enemy by Translate with direction and (Time.deltaTime / duration) as speed in world space
            elapsedTime += Time.deltaTime;
            yield return null; // move then wait to the next frame
        }

        enemyTransform.position = startingPos + direction; // Ensure that the enemy reaches its final position
    }

}
