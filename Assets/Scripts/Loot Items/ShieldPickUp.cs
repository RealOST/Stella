using UnityEngine;

public class ShieldPickUp : LootItem {
    [SerializeField] private AudioData fullHealthPickUpSFX;
    [SerializeField] private int fullHealthScoreBonus = 200;
    [SerializeField] private float shieldBonus = 20f;

    protected override void PickUp() {
        if (player.IsFullHealth) {
            pickUpSFX = fullHealthPickUpSFX;
            lootMessage.text = $"SCORE + {fullHealthScoreBonus}";
            ScoreManager.Instance.AddScore(fullHealthScoreBonus);
        }
        else {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = $"SHIELD + {shieldBonus}";
            player.RestoreHealth(shieldBonus);
        }

        base.PickUp();
    }
}