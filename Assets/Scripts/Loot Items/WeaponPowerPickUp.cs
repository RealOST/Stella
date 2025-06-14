using UnityEngine;

public class WeaponPowerPickUp : LootItem {
    [SerializeField] private AudioData fullPowerPickUpSFX;
    [SerializeField] private int fullPowerScoreBonus = 200;

    protected override void PickUp() {
        if (player.IsFullPower) {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = $"SCORE + {fullPowerScoreBonus}";
            ScoreManager.Instance.AddScore(fullPowerScoreBonus);
        }
        else {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = "POWER UP!";
            player.PowerUp();
        }

        base.PickUp();
    }
}