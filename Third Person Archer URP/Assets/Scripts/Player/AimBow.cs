using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class AimBow : MonoBehaviour
{
    #region Private Variables
    private WeaponSystem weaponSystem;
    private Transform    crossHairTarget;
    private PlayerInput  playerInput;
    private InputAction  aimAction;
    private InputAction  attackAction;
    private InputAction  runAction;
    private Animator     animator;
    private float        timer = 0;
    #endregion

    #region Serialize Private Variables
    [SerializeField] private GameObject arrowPrefab;
    private Transform  barrelTransform;
    [SerializeField] private Transform  arrowParent;
    #endregion

    private PlayerStatsSystem playerStats;

    public Ray ray;

    // Start is called before the first frame update
    void Awake()
    {
        animator        = GetComponent<Animator>();
        playerStats     = GetComponent<PlayerStatsSystem>();
        playerInput     = GetComponent<PlayerInput>();
        weaponSystem    = GetComponent<WeaponSystem>();
        aimAction       = playerInput.actions["Aim"];
        attackAction    = playerInput.actions["Attack"];
        runAction       = playerInput.actions["Run"];
        crossHairTarget = GameObject.FindGameObjectWithTag("CrossHairTarget").transform;
    }

    private void Start()
    {
        barrelTransform = GameObject.FindGameObjectWithTag("Barrel").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        AimBowAnimation();
    }

    private void AimBowAnimation()
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
        attackAction.performed += _ => ShootArrow();
    }

    private void OnDisable()
    {
        attackAction.performed -= _ => ShootArrow();
    }

    private void ShootArrow()
    {
        if (aimAction.IsPressed() && Time.time >= timer)
        {
            if (barrelTransform.position != null && crossHairTarget.position != null)
            {
                ray.origin = barrelTransform.position;
                ray.direction = crossHairTarget.position - barrelTransform.position;
            }   
            Quaternion arrowDirection = Quaternion.Euler(ray.direction);
            GameObject arrow = Instantiate(arrowPrefab, ray.origin, arrowDirection, arrowParent);
            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            if (Physics.Raycast(ray.origin, ray.direction, playerStats.attackRange))
            {
                arrowController.target = ray.origin + ray.direction * playerStats.attackRange;
                arrowController.hit = true;
            }
            else
            {
                arrowController.target = ray.origin + ray.direction * playerStats.attackRange;
                arrowController.hit = false;
            }
            animator.SetTrigger("Shoot");
            timer = Time.time + weaponSystem.weaponFireRate;
        }
    }

    private void PullStringSound()
    {
        GetComponent<PlayerAudioManager>().PlayerSound("PullStringBow");
    }

    private void FireArrowSound()
    {
        GetComponent<PlayerAudioManager>().PlayerSound("FireArrow");
    }
}
