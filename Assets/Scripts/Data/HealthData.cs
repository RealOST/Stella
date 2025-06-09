using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data/Health Data")]
public class HealthData : ScriptableObject {
    public float maxHealth = 100f;
    public float regenPercent = 0.05f;
}