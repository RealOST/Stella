using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyProjectile : Projectile {
    protected override void Awake() {
        base.Awake();
        // if (moveDirection != Vector2.left) transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
    }
}