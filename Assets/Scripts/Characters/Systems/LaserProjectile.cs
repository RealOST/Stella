using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserProjectile : MonoBehaviour {
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 0.5f;
    [SerializeField] private LayerMask obstacleMask; // 可阻挡的层级
    [SerializeField] private LayerMask targetMask;   // 可伤害的敌人层级
    [SerializeField] private GameObject hitVFX;

    private LineRenderer lineRenderer;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate() {
        OnSpawned(transform.position,transform.parent.rotation);
    }

    public void OnSpawned(Vector3 position, Quaternion rotation) {
        // transform.SetPositionAndRotation(position, rotation);
        StartCoroutine(FireLaser());
    }

    public IEnumerator FireLaser() {
        Vector3 start = transform.position;
        Vector3 dir = transform.forward;
        Vector3 end = start + dir * maxDistance;

        RaycastHit2D hit = Physics2D.Raycast(start, dir, maxDistance, obstacleMask | targetMask);
        if (hit.collider != null) {
            end = hit.point;

            if (hit.collider.TryGetComponent<Character>(out var character)) {
                character.TakeDamage(damage);
                // 播放VFX/SFX
                PoolManager.Release(hitVFX, end, Quaternion.LookRotation(hit.normal));
            }
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        yield return new WaitForSeconds(lifetime);

        gameObject.transform.parent.gameObject.SetActive(false);
    }
}