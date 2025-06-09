using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data/Base Data")]
public class CharacterData : ScriptableObject {
    [Header("---- DEATH ----")]
    public GameObject deathVFX; // 死亡特效预制体引用

    public AudioData[] deathSFX; // 死亡音效数据集

    [Header("---- HEALTH ----")]
    public float maxHealth; // 最大生命值  

    public bool showOnHeadHealthBar = true;
}