using UnityEngine;
using System.Collections;

public class MeshTrail : MonoBehaviour {
    [Header("基础配置")]
    [SerializeField] private GameObject meshTrailPrefab;

    [SerializeField] private GameObject model;

    [Header("参数配置")]
    [SerializeField] private float fadeDuration = 0.5f; // 每个残影褪色时间

    [SerializeField] private float spawnInterval = 0.1f; // 生成间隔

    private bool isTrailEnabled = false;
    private Coroutine autoSpawnRoutine;

    private int trailRequestCount = 0;

    private void OnEnable() {
        DodgeSystem.OnDodgeStarted += EnableTrail;
        DodgeSystem.OnDodgeEnded += DisableTrail;
        // PlayerOverdrive.on += EnableTrail;
        // PlayerOverdrive.off += DisableTrail;
        EventBus.Subscribe<OverdriveEvent>(HandleOverdriveEvent);
    }

    private void OnDisable() {
        DodgeSystem.OnDodgeStarted -= EnableTrail;
        DodgeSystem.OnDodgeEnded -= DisableTrail;
        // PlayerOverdrive.on -= EnableTrail;
        // PlayerOverdrive.off -= DisableTrail;
        EventBus.Unsubscribe<OverdriveEvent>(HandleOverdriveEvent);
    }
    
    void HandleOverdriveEvent(OverdriveEvent evt) {
        if (evt.IsOn) 
            EnableTrail();
        else 
            DisableTrail();
    }


    /// <summary>
    /// 启用自动残影生成
    /// </summary>
    public void EnableTrail() {
        trailRequestCount++;
        if (trailRequestCount == 1) {
            isTrailEnabled = true;
            autoSpawnRoutine = StartCoroutine(AutoSpawn());
        }
    }

    /// <summary>
    /// 关闭自动残影生成
    /// </summary>
    public void DisableTrail() {
        trailRequestCount--;
        trailRequestCount = Mathf.Max(trailRequestCount, 0); // 防止负数
        if (trailRequestCount == 0) {
            isTrailEnabled = false;
            if (autoSpawnRoutine != null)
                StopCoroutine(autoSpawnRoutine);
        }
    }

    /// <summary>
    /// 手动生成一个残影
    /// </summary>
    public void SpawnOnce() {
        StartCoroutine(SpawnAndFade());
    }

    /// <summary>
    /// 在指定时间内自动生成残影
    /// </summary>
    public void AutoSpawnTrail(float duration) {
        if (autoSpawnRoutine != null)
            StopCoroutine(autoSpawnRoutine);
        StartCoroutine(AutoSpawnForDuration(duration));
    }

    private IEnumerator AutoSpawn() {
        while (isTrailEnabled) {
            SpawnOnce();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator AutoSpawnForDuration(float duration) {
        var elapsed = 0f;
        while (elapsed < duration) {
            SpawnOnce();
            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }
    }

    private IEnumerator SpawnAndFade() {
        var mt = PoolManager.Release(meshTrailPrefab);

        mt.transform.position = model.transform.position;
        mt.transform.rotation = model.transform.rotation;
        mt.transform.localScale = model.transform.lossyScale;

        var fade = mt.GetComponent<MeshTrailFade>();
        fade.Play(fadeDuration);
        yield return null;
    }
}