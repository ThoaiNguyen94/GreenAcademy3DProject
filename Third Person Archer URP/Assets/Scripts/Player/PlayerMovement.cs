using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerStatsSystem))]
public class PlayerMovement : MonoBehaviour
{
    #region Private Variables
    private CharacterController characterController;
    private PlayerStatsSystem   playerStats;
    private Transform           cameraTransform;
    private Animator            animator;
    private PlayerInput         playerInput;
    private InputAction         moveAction;
    private InputAction         runAction;
    private InputAction         jumpAction;
    private readonly float      gravityValue = 14f;
    private float               verticalVelocity;
    #endregion

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerStats         = GetComponent<PlayerStatsSystem>();
        animator            = GetComponent<Animator>();
        playerInput         = GetComponent<PlayerInput>();
        moveAction          = playerInput.actions["Move"];
        runAction           = playerInput.actions["Run"];
        jumpAction          = playerInput.actions["Jump"];
        cameraTransform     = Camera.main.transform;
    }

    private void Update()
    {
        Move();
        Run();
        RotatePlayerMovement();
        Jump();
    }

    private void Move()
    {
        if (!runAction.IsPressed())
        {
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 move = new(input.x, 0, input.y);
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0f;
            characterController.Move(move * Time.deltaTime * playerStats.moveSpeed);
            MovementAnimation(input.x, input.y);
            //playerStats.position = transform.position;
            //playerStats.rotation = transform.rotation.eulerAngles;
        }
    }

    private void Run()
    {
        if (runAction.IsPressed())
        {
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0f;
            characterController.Move(move * Time.deltaTime * playerStats.runSpeed);
            animator.SetBool("Run", true);
            MovementAnimation(input.x, input.y);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    private void MovementAnimation(float vertical, float horizontal)
    {
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Horizontal", horizontal);
    }

    private void RotatePlayerMovement()
    {
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y + 15, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, playerStats.rotationSpeed * Time.deltaTime);
    }
    
    private void Jump()
    {
        // Check character is grounded
        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravityValue * Time.deltaTime;
            Vector3 fallVector = new Vector3(0, verticalVelocity, 0);
            characterController.Move(fallVector * Time.deltaTime);
            animator.SetBool("Jump", false);
        }
        else
        {
            verticalVelocity = -1f * gravityValue * Time.deltaTime;
            if (jumpAction.IsPressed())
            {
                Vector3 jumpVector = new Vector3(0, playerStats.jumpForce, 0);
                characterController.Move(jumpVector * Time.deltaTime);
                animator.SetBool("Jump", true);
            }
        }
    }

    private void WalkSound()
    {
        FindObjectOfType<PlayerAudioManager>().PlayerSound("Walk");
    }

    private void RunSound()
    {
        FindObjectOfType<PlayerAudioManager>().PlayerSound("Run");
    }

}
