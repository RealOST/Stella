using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon Data/Base Data")]
public class WeaponData : ScriptableObject {
    public GameObject[] projectiles;
    public GameObject overdriveProjectile;

    public int defaultWeaponPower = 1;
    public WeaponPatternData[] weaponPatterns;
    
    public float normalFireInterval = 0.12f;
    public float overdriveFireFactor = 1.2f;

    public AudioData fireSFX;
}
