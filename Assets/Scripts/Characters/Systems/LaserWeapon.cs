using System.Collections;
using UnityEngine;

public class LaserWeapon : WeaponBase {
    [SerializeField]private LaserProjectile laserProjectile;
    
    protected override IEnumerator FireCoroutine() {
        while (true) {
            StartCoroutine(laserProjectile.FireLaser());
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
}
