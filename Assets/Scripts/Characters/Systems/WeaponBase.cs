using System.Collections;
using UnityEngine;

public class WeaponBase : MonoBehaviour,IInjectable {
    private WeaponData weaponData;
    [SerializeField] protected Transform[] muzzles;
    [SerializeField] private ParticleSystem muzzleVFX;
    [SerializeField] private int weaponPower;

    protected Coroutine fireRoutine;
    protected bool isOverdriving = false;
    protected bool isFiring = false;

    public int WeaponPower => weaponPower;
    
    protected WaitForSeconds waitForFireInterval => new(weaponData.normalFireInterval);
    protected WaitForSeconds waitForOverdriveFireInterval => new(weaponData.normalFireInterval / weaponData.overdriveFireFactor);

    public void Inject(PlayerContext context) {
        weaponData = context.WeaponData;
        weaponPower = weaponData.defaultWeaponPower;
    }
    
    public void StartFire() {
        if (isFiring) return;
        isFiring = true;
        muzzleVFX.Play(); 
        fireRoutine = StartCoroutine(FireCoroutine());
    }

    public void StopFire() {
        if (!isFiring) return;
        muzzleVFX.Stop();
        StopCoroutine(fireRoutine);
        isFiring = false;
    }

    public void SetOverdrive(bool value) => isOverdriving = value;

    protected virtual IEnumerator FireCoroutine() {
        while (true) {
            var pattern = weaponData.weaponPatterns[weaponPower];
            pattern.Execute(muzzles, ReleaseProjectile);

            AudioManager.Instance.PlayRandomSFX(weaponData.fireSFX);
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }

    protected virtual void ReleaseProjectile(int index, Transform muzzle) {
        var prefab = isOverdriving ? weaponData.overdriveProjectile : weaponData.projectiles[index];
        PoolManager.Release(prefab, muzzle.position);
    }
    
    public void PowerUp() => weaponPower = Mathf.Min(weaponPower + 1, weaponData.weaponPatterns.Length-1);
    public void PowerDown() => weaponPower = Mathf.Max(--weaponPower, 0);
    
}

