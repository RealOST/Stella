using UnityEngine;
using System.Collections;

public class SmartParticleGroupStopper : MonoBehaviour {
    private ParticleSystem[] particleSystems;
    private float maxLifetime = 0f;

    private void Awake() {
        // 获取当前 GameObject 及其所有子对象上的粒子系统
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        if (particleSystems.Length == 0) Debug.LogWarning("SmartParticleGroupStopper 没有找到任何子粒子系统！");
    }

    public void SmoothStopAndDisable() {
        if (particleSystems.Length == 0) return;

        maxLifetime = 0f;

        foreach (var ps in particleSystems) {
            // 停止发射但允许现有粒子自然消散
            ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            // 找到最长的生命周期
            var lifetime = ps.main.startLifetime.constantMax;
            if (lifetime > maxLifetime)
                maxLifetime = lifetime;
        }

        StartCoroutine(WaitAndDisable(maxLifetime));
    }

    private IEnumerator WaitAndDisable(float delay) {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // 或者对象池回收
    }
}