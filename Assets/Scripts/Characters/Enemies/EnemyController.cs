using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyController : MonoBehaviour {
    [Header("---- MOVE ----")]
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private float moveRotationAngle = 25f;

    [Header("---- FIRE ----")]
    [SerializeField] protected GameObject[] projectiles;

    [SerializeField] protected AudioData[] projectileLaunchSFX;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected ParticleSystem muzzleVFX;
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;
    [SerializeField] protected float continuousFireInterval = 0.1f;
    [SerializeField] protected float continuousFireDuration = 1.5f;
    [SerializeField] protected bool useContinuousFire = false;

    protected float paddingX;
    private float paddingY;

    protected Vector3 targetPosition;

    protected WaitForSeconds waitForFireInterval => new(maxFireInterval);
    protected WaitForSeconds waitForContinuousFireInterval => new(continuousFireInterval);
    private WaitForFixedUpdate waitForFixedUpdate = new();

    protected virtual void Awake() {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    protected virtual void OnEnable() {
        StartCoroutine(RandomlyMovingCoroutine());
        StartCoroutine(RandomlyFireCoroutine());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private IEnumerator RandomlyMovingCoroutine() {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf) {
            // if has not arrived targetPosition
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime) {
                // keep moving to targetPosition
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                transform.rotation =
                    Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle,
                        Vector3.right);
            }
            else {
                // set a new targetPosition
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return waitForFixedUpdate;
        }
    }

    protected virtual IEnumerator RandomlyFireCoroutine() {
        while (gameObject.activeSelf) {
            yield return
                new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval)); //this will wait for a certain time

            if (useContinuousFire)
                yield return StartCoroutine(ContinuousFireCoroutine());
            else
                SingleFire();
        }
    }

    protected virtual void SingleFire() {
        foreach (var projectile in projectiles) PoolManager.Release(projectile, muzzle.position);

        AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
        muzzleVFX.Play();
    }

    protected virtual IEnumerator ContinuousFireCoroutine() {
        muzzleVFX.Play();
        var fireTimer = 0f;

        while (fireTimer < continuousFireDuration) {
            SingleFire();

            fireTimer += continuousFireInterval;
            yield return waitForContinuousFireInterval;
        }

        muzzleVFX.Stop();
    }
}