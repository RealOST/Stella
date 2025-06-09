using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character {
    #region FIELDS
    [SerializeField] private StatsBar_HUD statsBar_HUD;
    [SerializeField] private bool regenerateHealth = true;
    [SerializeField] private float healthRegenerateTime;
    [SerializeField] [Range(0f, 1f)] private float healthRegeneratePercent;
    [SerializeField] private GameObject damageFlicker;

    [Header("---- INPUT ----")]
    [SerializeField] private PlayerInput input;

    [Header("---- FIRE ----")]
    [SerializeField] private Transform muzzleMiddle;
    [SerializeField] private Transform muzzleTop;
    [SerializeField] private Transform muzzleBottom;

    private readonly float SlowMotionDuration = 0.4f;
    private readonly float InvincibleTime = 1.5f;

    private Vector2 moveDirection;
    private Vector2 previousVelocity;

    private Quaternion previousRotation;

    private WaitForSeconds waitHealthRegenerateTime => new(healthRegenerateTime);
    private WaitForSeconds waitInvincibleTime => new(InvincibleTime);

    private Coroutine moveCoroutine;
    private Coroutine healthRegenerateCoroutine;

    private new Rigidbody2D rigidbody;

    private new Collider2D collider;

    [SerializeField]private PlayerController playerController;

    #endregion

    #region PROPERTIES

    public bool IsFullHealth => health == maxHealth;
    public bool IsFullPower => playerController.WeaponPower == 4;

    #endregion

    #region UNITY EVENT FUNCTIONS

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Start() {
        statsBar_HUD.Initialize(health, maxHealth);

        input.EnableGameplayInput();
    }

    #endregion

    #region HEALTH

    // 增强型受击处理方法
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage); // 执行基础生命值计算
        // GetComponent<Shield>().showShield();
        playerController.PowerDown(); // 武器等级衰退策略
        statsBar_HUD.UpdateStats(health, maxHealth); // HUD组件实时更新
        
        DamagedVFX();

        // 存活状态检测
        if (gameObject.activeSelf) {
            // movementSystem.Move(movementSystem.MoveDirection); // 受击惯性保持
            playerController.Move(playerController.LastDirection);
            StartCoroutine(InvincibleCoroutine()); // 激活无敌状态

            // 生命恢复系统重启机制
            if (regenerateHealth) {
                if (healthRegenerateCoroutine != null) StopCoroutine(healthRegenerateCoroutine); // 中断现有恢复进程

                healthRegenerateCoroutine =
                    StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    public override void RestoreHealth(float value) {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void Die() {
        // GameManager.onGameOver.Invoke();
        // GameManager.GameState = GameState.GameOver;
        EventBus.Publish(new GameOverEvent());
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    private IEnumerator InvincibleCoroutine() {
        collider.isTrigger = true;
        damageFlicker.SetActive(true);

        yield return waitInvincibleTime;

        collider.isTrigger = false;
        damageFlicker.SetActive(false);
    }

    private void DamagedVFX() {
        // Camera.main.DOShakePosition(0.1f, 0.1f, 10, 90, true); // 视觉震动反馈（使用DOTween插件）
        Camera.main.DOShakePosition(0.2f, 0.12f, 10, 90, true).SetUpdate(true); // 视觉震动反馈（使用DOTween插件）
        var timescale = Time.timeScale;
        Time.timeScale = 0f;
        DOVirtual.DelayedCall(0.12f, () => { Time.timeScale = timescale; }, true);
        TimeController.Instance.BulletTime(SlowMotionDuration * 2); // 受击后时间减速
    }

    #endregion
    
    public void PowerUp() {
        playerController.PowerUp();
    }
    
    public void PickUpMissile() {
        playerController.PickUpMissile();
    }

    [SerializeField]private GameObject drone;
    public void GetDrone() {
        drone.SetActive(true);
    }
}