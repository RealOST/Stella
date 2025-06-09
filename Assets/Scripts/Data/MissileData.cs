using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data/Missile Data")]
public class MissileData : ScriptableObject {
    public int defaultAmount = 5;
    public float cooldownTime = 1f;
    public GameObject missilePrefab = null;
    public AudioData launchSFX = null;
}
