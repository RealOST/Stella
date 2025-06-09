using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile {
    protected override void Awake() {
        base.Awake();
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    protected override void OnEnable() {
        StartCoroutine(MoveDirectionCoroutine());
        base.OnEnable();
    }

    private IEnumerator MoveDirectionCoroutine() {
        yield return null;
        if (target.activeSelf) {
            moveDirection = (target.transform.position - transform.position).normalized;

            // 通过 moveDirection 设定旋转，让 transform.right 指向目标
            var angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}