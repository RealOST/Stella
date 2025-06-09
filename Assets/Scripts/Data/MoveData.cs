using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data/Move Data")]
public class MoveData : ScriptableObject {
    [Header("Basic")]
    public float baseSpeed = 5f;
    public float accelerationTime = 0.12f;
    public float decelerationTime = 0.2f;
    public float moveRotationAngle = 30f;

    [Header("Boundary")]
    public float paddingX = 0.1f;
    public float paddingY = 0.1f;

    [Header("Multiplier")]
    public float speedMultiplier = 1f;

    public float CurrentSpeed => baseSpeed * speedMultiplier;
}
