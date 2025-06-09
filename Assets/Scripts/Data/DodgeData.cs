using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data/Dodge Data")]
public class DodgeData : ScriptableObject {
    public float maxRoll = 720f;
    public float rollSpeed = 360f;
    public Vector3 dodgeScale = new(0.5f, 0.5f, 0.5f);
    public AudioData dodgeSFX;
}
