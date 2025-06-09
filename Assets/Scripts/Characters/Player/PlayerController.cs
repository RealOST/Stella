using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("---- SYSTEM DATA ----")]
    [SerializeField] private MoveData moveData;
    [SerializeField] private DodgeData dodgeData;
    [SerializeField] private MissileData missileData;
    [SerializeField] private WeaponData bulletWeaponData;
    [SerializeField] private WeaponData crossWeaponData;

    [Header("---- SYSTEM REFS ----")]
    [SerializeField] private WeaponBase equippedWeapon;
    private MovementSystem movementSystem;
    private DodgeSystem dodgeSystem;
    private MissileSystem missileSystem;
    
    [Header("---- INPUT ----")]
    [SerializeField] private PlayerInput input;
    
    [Header("---- FIRE ----")]
    [SerializeField] private Transform muzzleMiddle;
    [SerializeField] private Transform muzzleTop;
    [SerializeField] private Transform muzzleBottom;
    
    private readonly float SlowMotionDuration = 0.4f;
    public Vector2 LastDirection => movementSystem.MoveDirection;
    public int WeaponPower => equippedWeapon.WeaponPower;
    
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    
    #region UNITY EVENT FUNCTIONS
    
    private void GetComponents() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        movementSystem = GetComponent<MovementSystem>();
        dodgeSystem = GetComponent<DodgeSystem>();
        missileSystem = GetComponent<MissileSystem>();
    }

    private void InjectData() {
        var context = new PlayerContext {
            MoveData = moveData,
            DodgeData = dodgeData,
            MissileData = missileData,
            WeaponData = bulletWeaponData,
            Rigidbody2D = rigidbody,
            Collider2D = collider,
        };
        
        foreach (var system in GetComponents<IInjectable>()) {
            system.Inject(context);
        }
    }

    private void Awake() {
        GetComponents();
        
        // dodgeSystem.Init(rigidbody,collider);

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        moveData.paddingX = size.x / 2f;
        moveData.paddingY = size.y / 2f;

        rigidbody.gravityScale = 0f;
        
        InjectData();
    }
    
    private void OnEnable() {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        // PlayerOverdrive.on += OverdriveOn;
        // PlayerOverdrive.off += OverdriveOff;
        EventBus.Subscribe<OverdriveEvent>(HandleOverdriveEvent);
    }


    private void OnDisable() {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;

        // PlayerOverdrive.on -= OverdriveOn;
        // PlayerOverdrive.off -= OverdriveOff;
        EventBus.Unsubscribe<OverdriveEvent>(HandleOverdriveEvent);
    }
    
    #endregion
    
    #region MOVE

    // 运动控制入口方法
    public void Move(Vector2 moveInput) {
        movementSystem.Move(moveInput);
    }

    // 运动状态终止方法
    private void StopMove() {
        movementSystem.StopMove();
    }

    #endregion

    #region FIRE

    // 武器激活入口
    private void Fire() {
        equippedWeapon.StartFire();
    }

    // 武器停火控制
    private void StopFire() {

        equippedWeapon.StopFire();
    }

    #endregion

    #region DODGE

    private void Dodge() {
        dodgeSystem.Dodge();
    }

    #endregion

    #region OVERDRIVE

    private void Overdrive() {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        // PlayerOverdrive.on.Invoke();
        EventBus.Publish(new OverdriveEvent { IsOn = true });
    }
    
    private void HandleOverdriveEvent(OverdriveEvent evt) {
        if (evt.IsOn)
            OverdriveOn();
        else
            OverdriveOff();
    }

    private void OverdriveOn() {
        equippedWeapon.SetOverdrive(true);
        movementSystem.SpeedUp();
        TimeController.Instance.BulletTime(SlowMotionDuration, SlowMotionDuration);
    }

    private void OverdriveOff() {
        equippedWeapon.PowerUp();
        equippedWeapon.SetOverdrive(false);
        movementSystem.ResetSpeed();
    }

    #endregion

    #region MISSILE

    private void LaunchMissile() {
        missileSystem.Launch(muzzleMiddle);
    }

    public void PickUpMissile() {
        missileSystem.PickUp();
    }

    #endregion

    #region WEAPON POWER

    public void PowerUp() {
        equippedWeapon.PowerUp();
    }

    public void PowerDown() {
        equippedWeapon.PowerDown();
    }

    #endregion
}

public struct PlayerContext {
    public MoveData MoveData;
    public DodgeData DodgeData;
    public MissileData MissileData;
    public WeaponData WeaponData;

    public Rigidbody2D Rigidbody2D;
    public Collider2D Collider2D;
}
