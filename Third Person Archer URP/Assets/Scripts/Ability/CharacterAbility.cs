using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbility : MonoBehaviour
{
    [SerializeField] protected SkillData skillData;
    [SerializeField] protected Image skillCooldownImage;
    [SerializeField] protected Image skillNoManaImage;

    protected float timer;
    protected Animator animator;

    // Melee use only
    protected readonly Vector3 offsetSphere = new(0f, 0.8f, 0f);

    protected enum AbilityState
    {
        ready,
        cooldown
    }

    protected AbilityState state = AbilityState.ready;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        //if (skill condition)
        //{
        //    switch (state)
        //    {
        //        case AbilityState.ready:
        //            //active skill
        //            break;

        //        case AbilityState.cooldown:
        //            if (timer > 0)
        //            {
        //                timer -= Time.deltaTime;
        //                skillCooldown.fillAmount -= 1 / skillCooldownTime * Time.deltaTime;
        //                skillCooldown.enabled = true;
        //            }
        //            else
        //            {
        //                state = AbilityState.ready;
        //                skillCooldown.fillAmount = 1;
        //                skillCooldown.enabled = false;
        //            }
        //            break;
        //    }
        //}
        //else
        //{
        //    skillCooldown.enabled = true;
        //}
    }
}
