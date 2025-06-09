using System.Collections;
using UnityEngine;

public class MovementSystem : MonoBehaviour,IInjectable {
    private MoveData moveData;
    [SerializeField] private float overdriveSpeedFactor = 1.2f;

    private new Rigidbody2D rigidbody2D;
    private Vector2 moveDirection;
    private Coroutine moveCoroutine;
    private Vector2 previousVelocity;
    private Quaternion previousRotation;
    private float t;

    public Vector2 MoveDirection => moveDirection;

    public void Inject(PlayerContext context) {
        moveData = context.MoveData;
        rigidbody2D = context.Rigidbody2D;
    }

    // private void Awake() {
    //     rigidbody2D = GetComponentInParent<Rigidbody2D>();
    //
    // }

    public void Move(Vector2 input) {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        moveDirection = input.normalized;

        moveCoroutine = StartCoroutine(MoveCoroutine(
            moveData.accelerationTime,
            moveDirection * moveData.CurrentSpeed,
            Quaternion.AngleAxis(moveData.moveRotationAngle * input.y, Vector3.right)
        ));
    }

    public void StopMove() {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveDirection = Vector2.zero;

        moveCoroutine = StartCoroutine(MoveCoroutine(
            moveData.decelerationTime,
            moveDirection,
            Quaternion.identity
        ));
    }

    private IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation) {
        t = 0f;
        previousVelocity = rigidbody2D.velocity;
        previousRotation = transform.rotation;

        while (t < 1f) {
            t += Time.fixedDeltaTime / time;
            rigidbody2D.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);
            yield return new WaitForFixedUpdate();
        }
    }

    private void Update() {
        // 屏幕边界限制
        transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, moveData.paddingX, moveData.paddingY);
    }
    
    public void SpeedUp() => moveData.speedMultiplier = overdriveSpeedFactor;
    public void ResetSpeed() => moveData.speedMultiplier = 1f;

}
