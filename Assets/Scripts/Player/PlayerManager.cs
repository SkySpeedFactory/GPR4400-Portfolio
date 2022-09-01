using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class PlayerManager : MonoBehaviour
{
    private CharacterController characterController;

    private PlayerControls playerControls;
    public Vector2 inputMovement { get; private set; }
    public Vector2 inputMouseLook { get; private set; }

    private Vector3 newPlayerRotation;
    private Vector3 newCameraRotation;

    [Header("References")]
    [SerializeField] UnityEngine.GameObject mainCamera;
    [SerializeField] Transform feetTransform;
    public Transform cameraHolder { get; private set; }

    [Header("Settings")]
    [SerializeField] PlayerSettingsModel playerSettings;
    [SerializeField] float mouseLookClampX = -80;
    [SerializeField] float mouseLookClampY = 80;
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask groundMask;

    [Header("Movement")]
    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;
    public bool isSprinting { get; private set; }

    [Header("Gravity")]
    [SerializeField] float gravityAmount;
    [SerializeField] float gravityMin;
    private float playerGravity;
    public bool isGrounded { get; private set; }
    public bool isFalling { get; private set; }

    [Header("Jumping")]
    [SerializeField] Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    [Header("Stance")]
    [SerializeField] PlayerStance playerStance;
    [SerializeField] float playerStanceSmoothing;
    [SerializeField] CharacterStance playerStandStance;
    [SerializeField] CharacterStance playerCrouchStance;
    [SerializeField] CharacterStance playerProneStance;
    private float stanceCheckErrorMargin = 0.05f;
    private float cameraHeight;
    private float cameraHeightVelocity;
    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;

    [Header("Weapon")]
    [SerializeField] WeaponController currentWeapon;
    public float weaponAnimationSpeed { get; private set; }

    [Header("Aiming In")]
    public bool isAimingIn;

    [Header("Shooting")]
    public bool isShooting;



    #region Awake
    private void Awake()
    {
        CursorLock();

        playerControls = new PlayerControls();
        cameraHolder = mainCamera.transform;
                
        playerControls.Player.Movement.performed += ctx => inputMovement = ctx.ReadValue<Vector2>();        
        playerControls.Player.MouseLook.performed += ctx => inputMouseLook = ctx.ReadValue<Vector2>();
        
        playerControls.Player.Jump.performed += ctx => Jump();        
        playerControls.Player.Crouch.performed += ctx => Crouch();
        playerControls.Player.Prone.performed += ctx => Prone();
        playerControls.Player.Sprint.performed += ctx => ToggleSprint();
        playerControls.Player.SprintReleased.performed += ctx => StopSprint();

        playerControls.Weapon.Fire2Pressed.performed += ctx => AimingInPressed();
        playerControls.Weapon.Fire2Released.performed += ctx => AimingInReleased();

        playerControls.Weapon.Fire1Pressed.performed += ctx => ShootingPressed();
        playerControls.Weapon.Fire1Released.performed += ctx => ShootingReleased();

        playerControls.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newPlayerRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();

        cameraHeight = cameraHolder.localPosition.y;

        if (currentWeapon)
        {
            currentWeapon.Initialize(this);
        }
    }
    #endregion

    private void FixedUpdate()
    {
        SetIsGrounded();
        SetIsFalling();
        CalculateMouseLook();
        CalculateMovement();
        CalculateJump();
        CalculateStance();
        CalculateAimingIn();
        CalculateShooting();
    }

    #region Cursor
    public void CursorLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Aiming
    private void AimingInPressed()
    {
        isAimingIn = true;
    }
    private void AimingInReleased()
    {
        isAimingIn = false;        
    }
    private void CalculateAimingIn()
    {
        if (!currentWeapon)
        {
            return;
        }
        currentWeapon.isAimingIn = isAimingIn;
    }
    #endregion

    #region Shooting
    public void ShootingPressed()
    {
        isShooting = true;
    }

    public void ShootingReleased()
    {
        isShooting = false;
    }
    private void CalculateShooting()
    {
        if (!currentWeapon)
        {
            return;
        }
        currentWeapon.isShooting = isShooting;
    }
    #endregion

    #region IsFalling / Isgrounded

    private void SetIsGrounded()
    {
        isGrounded = Physics.CheckSphere(feetTransform.position, playerSettings.isGroundedRadius, groundMask);
    }

    private void SetIsFalling()
    {
        isFalling = (!isGrounded && characterController.velocity.magnitude > playerSettings.isFallingSpeed);        
    }

    #endregion

    #region MouseLook and Movement
    private void CalculateMouseLook()
    {
        newPlayerRotation.y += (isAimingIn ? playerSettings.MouseLookXSensitivity * playerSettings.AimingSensitivityEffector : playerSettings.MouseLookXSensitivity) * (playerSettings.ViewXInverted ? -inputMouseLook.x : inputMouseLook.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newPlayerRotation);

        newCameraRotation.x += (isAimingIn ? playerSettings.MouseLookYSensitivity * playerSettings.AimingSensitivityEffector : playerSettings.MouseLookYSensitivity) * (playerSettings.ViewYInverted ? inputMouseLook.y : -inputMouseLook.y) * Time.deltaTime;//Invert Mouselook as Xmouse = Yrotation
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, mouseLookClampX, mouseLookClampY); // Clamp Mouse look

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }
    private void CalculateMovement()
    {
        if (inputMovement.y <= 0.2f)
        {
            isSprinting = false;
        }

        var verticalSpeed = playerSettings.WalkingForwardSpeed;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed;

        if (isSprinting)
        {
            verticalSpeed = playerSettings.RunningForwardSpeed;
            horizontalSpeed = playerSettings.RunningStrafeSpeed;
        }

        //Effectors
        if (isGrounded)
        {
            playerSettings.SpeedEffector = playerSettings.FallingSpeedEffector;
        }
        else if (playerStance == PlayerStance.Crouch)
        {
            playerSettings.SpeedEffector = playerSettings.CrouchSpeedEffector;
        }
        else if (playerStance == PlayerStance.Prone)
        {
            playerSettings.SpeedEffector = playerSettings.ProneSpeedEffector;
        }
        else if (isAimingIn)
        {
            playerSettings.SpeedEffector = playerSettings.AimingSpeedEffector;
        }
        else
        {
            playerSettings.SpeedEffector = 1;
        }

        weaponAnimationSpeed = characterController.velocity.magnitude / (playerSettings.WalkingForwardSpeed * playerSettings.SpeedEffector);
        if (weaponAnimationSpeed > 1f && weaponAnimationSpeed != 0f)
        {
            weaponAnimationSpeed = 1f;
        }
        verticalSpeed *= playerSettings.SpeedEffector;
        horizontalSpeed *= playerSettings.SpeedEffector;
        
        newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, new Vector3(horizontalSpeed * inputMovement.x * Time.deltaTime, 0, verticalSpeed * inputMovement.y * Time.deltaTime),ref newMovementSpeedVelocity, characterController.isGrounded ? playerSettings.MovementSmoothing : playerSettings.FallingSmoothing);
         var MovementSpeed = transform.TransformDirection(newMovementSpeed);

        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }
        
        if (playerGravity < -0.2f && isGrounded)
        {
            playerGravity = -1f;
        }
        
        //Player Gravity
        MovementSpeed.y += playerGravity * Time.deltaTime;
        //Player Jumping Force
        MovementSpeed += jumpingForce * Time.deltaTime;
        //Character Controller Move
        characterController.Move(MovementSpeed);               
    }
    #endregion

    #region Player Stance
    private void CalculateStance()
    {
        var currentStance = playerStandStance;

        if (playerStance == PlayerStance.Crouch)
        {
            currentStance = playerCrouchStance;
        }
        else if (playerStance == PlayerStance.Prone)
        {
            currentStance = playerProneStance;
        }
        
        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, currentStance.cameraHeight,ref cameraHeightVelocity, playerStanceSmoothing);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.stanceCollider.height, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.stanceCollider.center, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);

    }
    private bool StanceCheck(float stanceCheckHeight)
    {
        var start = new Vector3(feetTransform.position.x,feetTransform.position.y + characterController.radius + stanceCheckErrorMargin, feetTransform.position.z);
        var end = new Vector3(feetTransform.position.x, feetTransform.position.y - characterController.radius - stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);
        

        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }
    #endregion

    #region Jumping
    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
    }
    private void Jump()
    {
        Debug.Log("JumpPressed");

        if (!isGrounded || playerStance == PlayerStance.Prone)
        {
            return;
        }
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        jumpingForce = Vector3.up * playerSettings.JumpingHeight;
        playerGravity = 0;

        currentWeapon.TriggerJump();
    }
    #endregion

    #region Crouch and Prone
    private void Crouch()
    {
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        if (StanceCheck(playerCrouchStance.stanceCollider.height))
        {
            return;
        }
        playerStance = PlayerStance.Crouch;
    }

    private void Prone()
    {
        playerStance = PlayerStance.Prone;
    }
    #endregion

    #region Sprint
    private void ToggleSprint()
    {
        if (inputMovement.y <= 0.2f)
        {
            isSprinting = false;
            return;
        }
        isSprinting = !isSprinting;

    }
    private void StopSprint()
    {
        if (playerSettings.SprintingHold)
        {
            isSprinting = false;
        }        
    }
    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetTransform.position, playerSettings.isGroundedRadius);
    }

    #endregion
}
