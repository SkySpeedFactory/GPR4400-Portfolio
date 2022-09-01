using UnityEngine;
using static Models;

public class WeaponController : MonoBehaviour
{
    private PlayerManager playerManager;

    [Header("References")]
    [SerializeField] Animator weaponAnimator;
    [SerializeField] WindManager windManager;

    [Header("Settings")]
    [SerializeField] WeaponSettingsModel settings;

    private bool isInitialized;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    private bool isGroundedTrigger;

    private float fallingDelay;

    [Header("Weapon Sway")]
    [SerializeField] Transform weaponSwayObject;
    [SerializeField] float swayAmountA = 1;
    [SerializeField] float swayAmountB = 2;
    [SerializeField] float swayScale = 600;
    [SerializeField] float swayLerpSpeed = 14;
    private float swayTime;
    private Vector3 swayPosition;


    [Header("Sights")]
    [SerializeField] UnityEngine.GameObject scopeGlass;
    [SerializeField] Transform sightTarget;
    [SerializeField] float sightOffset;
    [SerializeField] float aimingInTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;
    public bool isAimingIn;

    [Header("Shooting")]
    [SerializeField] UnityEngine.GameObject bulletPrefab;
    [SerializeField] UnityEngine.GameObject muzzleFlashPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    //public GameObject shellPrefab; -----------> Not Implented 
    //public Transform shellEjectionPort;
    [SerializeField] float bulletSpeed;
    [SerializeField] float fireRate;
    private float nextFire;
    [SerializeField] float gravity;
    [SerializeField] float bulletLifeTime;
    public bool isShooting;

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }
    public void Initialize(PlayerManager PlayerManager)
    {
        playerManager = PlayerManager;
        isInitialized = true;
    }

    private void FixedUpdate()
    {
        if (!isInitialized)
        {
            return;
        }
        CalculateWeaponRotation();
        SetWeaponAnimations();
        CalculateWeaponSway();
        CalculateAimingIn();
        CalculateShooting();
    }

    private void CalculateShooting()
    {
        if (isShooting && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            UnityEngine.GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);
            UnityEngine.GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);
            Bullet_762x51 bulletScript = bullet.GetComponent<Bullet_762x51>();
            if (bulletScript)
            {
                bulletScript.Initialize(bulletSpawnPoint, bulletSpeed, gravity, windManager.GetWind());
            }
            Destroy(bullet, bulletLifeTime);
            Destroy(muzzleFlash, 0.2f);
        }
    }

    private void CalculateAimingIn()
    {
        var targetPosition = transform.position;
        
        if (isAimingIn)
        {
            targetPosition = playerManager.cameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (playerManager.cameraHolder.transform.forward * sightOffset);
            scopeGlass.SetActive(false);
        }
        else scopeGlass.SetActive(true);

        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingInTime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
    }

    public void TriggerJump()
    {
        isGroundedTrigger = false;
        weaponAnimator.SetTrigger("Jump");
    }

    private void CalculateWeaponRotation()
    {
        //Could use a variable to make dynamic sway instead of 3
        targetWeaponRotation.y += (isAimingIn ? settings.SwayAmount / 3 : settings.SwayAmount) * (settings.SwayXInverted ? -playerManager.inputMouseLook.x : playerManager.inputMouseLook.x) * Time.deltaTime;
        targetWeaponRotation.x += (isAimingIn ? settings.SwayAmount / 3 : settings.SwayAmount) * (settings.SwayYInverted ? playerManager.inputMouseLook.y : -playerManager.inputMouseLook.y) * Time.deltaTime;//Invert Mouselook as Xmouse = Yrotation

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);
        targetWeaponRotation.z = isAimingIn ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothing);

        //Could use a variable to make dynamic sway instead of 3
        targetWeaponMovementRotation.z = (isAimingIn ? settings.MovementSwayX / 3 : settings.MovementSwayX) * (settings.MovementSwayXInverted ? -playerManager.inputMovement.x : playerManager.inputMovement.x);
        targetWeaponMovementRotation.x = (isAimingIn ? settings.MovementSwayY / 3 : settings.MovementSwayY) * (settings.MovementSwayYInverted ? -playerManager.inputMovement.y : playerManager.inputMovement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);

        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }

    private void SetWeaponAnimations()
    {
        if (isGroundedTrigger)
        {
            fallingDelay = 0;
        }
        else
        {
            fallingDelay = Time.deltaTime;
        }

        if (playerManager.isGrounded && !isGroundedTrigger && fallingDelay > 0.1f)
        {
            weaponAnimator.SetTrigger("Land");
            isGroundedTrigger = true;
        }
        else if (!playerManager.isGrounded && isGroundedTrigger)
        {
            weaponAnimator.SetTrigger("Falling");
            isGroundedTrigger = false;
        }

        weaponAnimator.SetBool("IsSprinting", playerManager.isSprinting);
        weaponAnimator.SetFloat("WeaponAnimationSpeed", playerManager.weaponAnimationSpeed);
    }

    private void CalculateWeaponSway()
    {
        var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAimingIn ? swayScale * 3 : swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;
        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }                
    }
    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time * Mathf.PI));
    }
}
