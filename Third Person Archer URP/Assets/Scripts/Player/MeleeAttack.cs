using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameObject bloodSprayVFX;

    #region Private Variables
    private PlayerStatsSystem playerStats;
    private GameObject        bloodSpray;
    private WeaponSystem      weaponSystem;
    private Animator          animator;
    private PlayerInput       playerInput;
    private InputAction       aimAction;
    private InputAction       attackAction;
    private InputAction       runAction;
    private float             timer = 0;
    private readonly Vector3  offsetSphere = new(0f, 0.8f, 0f);
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        playerStats  = GetComponent<PlayerStatsSystem>();
        animator     = GetComponent<Animator>();
        weaponSystem = GetComponent<WeaponSystem>();
        playerInput  = GetComponent<PlayerInput>();
        aimAction    = playerInput.actions["Aim"];
        attackAction = playerInput.actions["Attack"];
        runAction    = playerInput.actions["Run"];
    }

    void Update()
    {
        AimAnimation();
    }

    private void AimAnimation()
    {
        if (aimAction.IsPressed() && !runAction.IsPressed())
        {
            animator.SetBool("Aim", true);
        }
        else
        {
            animator.SetBool("Aim", false);
        }
    }

    private void OnEnable()
    {
        attackAction.performed += _ => NormalSlash();
    }

    private void OnDisable()
    {
        attackAction.performed -= _ => NormalSlash();
    }

    private void NormalSlash()
    {
        if (Time.time >= timer)
        {
            animator.SetTrigger("Shoot");
            StartCoroutine(DisableMovement());
            timer = Time.time + playerStats.weaponFireRate;
        }
    }

    private void Attack()
    {
        Vector3 spherePosition = (transform.position + offsetSphere) + (transform.forward * (playerStats.attackRange / 2));
        int totalDamage = Mathf.FloorToInt(playerStats.attackDamage + weaponSystem.weaponDamage);
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, playerStats.attackRange / 2);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemies"))
            {
                Vector3 offset = new(0f, 1f, 0f);
                collider.GetComponent<EnemiesStatsSystem>().TakeDamage(totalDamage);
                bloodSpray = Instantiate(bloodSprayVFX, collider.transform.position + offset, collider.transform.rotation);
                Destroy(bloodSpray, 1f);
            }
        }
    }

    private void NormalSlashSound()
    {
        GetComponent<PlayerAudioManager>().PlayerSound("NormalSlash");
    }

    private IEnumerator DisableMovement()
    {
        GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(1.29f);
        GetComponent<PlayerMovement>().enabled = true;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position + offsetSphere + transform.forward * 1f, 1f); // radius = 1
    //}
}
