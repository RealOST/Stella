using System.Collections;
using UnityEngine;

public class MissileSystem : MonoBehaviour,IInjectable {
    private MissileData missileData;

    private bool isReady = true;

    private int amount;

    public void Inject(PlayerContext context) {
        missileData = context.MissileData;
        amount = missileData.defaultAmount;
    }

    private void Start() {
        MissileDisplay.UpdateAmountText(amount);
    }

    public void PickUp() {
        amount++;
        MissileDisplay.UpdateAmountText(amount);

        if (amount == 1) {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    }

    public void Launch(Transform muzzleTransform) {
        if (amount == 0 || !isReady) return; // TODO: Add SFX && UI VFX here 

        isReady = false;
        PoolManager.Release(missileData.missilePrefab, muzzleTransform.position);
        AudioManager.Instance.PlayRandomSFX(missileData.launchSFX);
        amount--;
        MissileDisplay.UpdateAmountText(amount);

        if (amount == 0)
            MissileDisplay.UpdateCooldownImage(1f);
        else
            StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine() {
        var cooldownValue = missileData.cooldownTime;

        while (cooldownValue > 0f) {
            MissileDisplay.UpdateCooldownImage(cooldownValue / missileData.cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;
        }

        isReady = true;
    }

}