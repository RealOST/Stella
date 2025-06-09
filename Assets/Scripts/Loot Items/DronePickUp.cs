using UnityEngine;

public class DronePickUp : LootItem {
    protected override void PickUp() {
        player.GetDrone();
        base.PickUp();
    }
}
