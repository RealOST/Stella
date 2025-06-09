using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data/Player Data")]
public class PlayerData : CharacterData {
    public bool regenerateHealth = true;
    public float healthRegenerateTime;
    [Range(0f, 1f)] public float healthRegeneratePercent;
    public GameObject damagePrefab;

    [Header("---- INPUT ----")]
    public PlayerInput input;

    [Header("---- MOVE ----")]
    public float moveSpeed = 10f;

    public float accelerationTime = 3f;
    public float decelerationTime = 3f;
    public float moveRotationAngle = 50f;

    [Header("---- FIRE ----")]
    public GameObject[] projectiles;

    public GameObject projectileOverdrive;
    public ParticleSystem muzzleVFX;
    public Transform muzzleMiddle;
    public Transform muzzleTop;
    public Transform muzzleBottom;
    public AudioData projectileLaunchSFX;
    [Range(0, 4)] public int weaponPower = 0;
    public float fireInterval = 0.2f;

    [Header("---- DODGE ----")]
    public AudioData dodgeSFX;

    [Range(0, 100)] public int dodgeEnergyCost = 25;
    public float maxRoll = 720f;
    public float rollSpeed = 360f;
    public Vector3 dodgeScale = new(0.5f, 0.5f, 0.5f);

    [Header("---- OVERDRIVE ----")]
    public int overdriveDodgeFactor = 2;

    public float overdriveSpeedFactor = 1.2f;
    public float overdriveFireFactor = 1.2f;


    public readonly float SlowMotionDuration = 0.4f;
    public readonly float InvincibleTime = 1.5f;
    public float dodgeDuration;

    // public bool IsFullHealth => health == maxHealth;
    public bool IsFullPower => weaponPower == 4;
}