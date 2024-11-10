using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float JUMP_FORCE_MULTIPLIER = -2f;

    [Header("References")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Jump & Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;
    //[SerializeField] private float jumpHeight = 2f;

    private float turnSmoothVelocity;
    private Vector3 velocity;
    private float moveSpeed;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = walkSpeed; // Initialize with walk speed
    }

    void Update()
    {
        HandleMovement();
        ApplyGravityAndJump();
        Interaction();
    }

    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerInteract.InteractLand();
        }

    }

    private void HandleMovement()
    {
        // Get input for movement direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Move if input is detected
        if (direction.magnitude > 0.1f)
        {
            UpdateMoveSpeed();
            RotateTowardsMovementDirection(direction);
            MoveCharacter(direction);
            playerAnimator.SetFloat("Speed", 1);
        }
        else
        {
            playerAnimator.SetFloat("Speed", 0.1f);
            playerAnimator.SetBool("IsRunning", false);
        }
    }

    private void UpdateMoveSpeed()
    {
        // Update move speed based on sprinting or walking
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, transitionSpeed * Time.deltaTime);
            playerAnimator.SetBool("IsRunning", true);

        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, transitionSpeed * Time.deltaTime);
            playerAnimator.SetBool("IsRunning", false);
        }
    }

    private void RotateTowardsMovementDirection(Vector3 direction)
    {
        // Calculate target rotation based on camera orientation
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        // Apply rotation
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void MoveCharacter(Vector3 direction)
    {
        // Calculate move direction relative to camera orientation
        Vector3 moveDirection = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f) * direction;
        moveDirection *= moveSpeed;

        // Move character controller
        characterController.Move(moveDirection * Time.deltaTime);

    }

    private void ApplyGravityAndJump()
    {
        // Reset vertical velocity if grounded
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jump if grounded and jump button is pressed

        /*if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * JUMP_FORCE_MULTIPLIER * gravity);
            playerAnimator.SetTrigger("IsJumping");
        }*/

        // Apply gravity to vertical velocity
        velocity.y += gravity * Time.deltaTime;

        // Move character controller based on vertical velocity
        characterController.Move(velocity * Time.deltaTime);
    }

}
