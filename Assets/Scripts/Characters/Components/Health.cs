using UnityEngine;

[System.Serializable]
public class Health : IDamageable {
    public float Max { get; private set; }
    public float Current { get; private set; }

    public bool IsDead => Current <= 0;

    public void Initialize(HealthData data) {
        Max = data.maxHealth;
        Current = Max;
    }

    public void TakeDamage(float amount) {
        if (IsDead) return;
        Current = Mathf.Max(0, Current - amount);
    }

    public void Heal(float amount) {
        Current = Mathf.Min(Max, Current + amount);
    }
}