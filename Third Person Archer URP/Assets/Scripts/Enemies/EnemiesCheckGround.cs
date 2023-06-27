using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemiesCheckGround : MonoBehaviour
{
    #region Private Variables
    private CharacterController characterController;
    private float verticalVelocity;
    private float gravityValue = 14f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravityValue * Time.deltaTime;
            Vector3 fallVector = new Vector3(0, verticalVelocity, 0);
            characterController.Move(fallVector * Time.deltaTime);
        }
        else
        {
            verticalVelocity = -1f * gravityValue * Time.deltaTime;
        }
    }
}
