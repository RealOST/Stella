using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile : MonoBehaviour {
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private AudioData[] hitSFX;
    [SerializeField] private float damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    // 解决子弹朝向和移动方向不一致，产生波纹、残影错位问题
    protected virtual void Awake() {
        // 同步旋转：让子弹朝向 moveDirection
        var angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected virtual void OnEnable() {
        StartCoroutine(MoveDirectly());
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<Character>(out var character)) {
            character.TakeDamage(damage);

            // var contactPoint = collision.GetContact(0);
            // PoolManager.Release(hitVFX, contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            PoolManager.Release(hitVFX, collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator MoveDirectly() {
        while (gameObject.activeSelf) {
            Move();
            yield return null;
        }
    }

    public void Move() => transform.Translate(moveSpeed * Time.deltaTime * Vector2.right);

    protected void SetTarget(GameObject target) => this.target = target;
}