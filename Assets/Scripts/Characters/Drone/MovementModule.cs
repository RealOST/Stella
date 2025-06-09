using System.Collections;
using UnityEngine;

namespace Characters.Drone {
    public class MovementModule : MonoBehaviour {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float accelerationTime = 0.2f;
        [SerializeField] private float decelerationTime = 0.2f;
        [SerializeField] private PlayerInput input;

        private Rigidbody2D rb;
        private Coroutine moveCoroutine;
        private Vector2 moveDirection;

        private void Awake()
        {
            rb = GetComponentInParent<Rigidbody2D>();

            // 自动绑定 InputModule 的事件（只在同 GameObject 或父物体查找）
            // input = GetComponent<PlayerInput>() ?? GetComponentInParent<PlayerInput>();
            if (input != null)
            {
                input.onMove += Move;
                input.onStopMove += StopMove;
            }
            else
            {
                Debug.LogWarning("未找到输入模块，移动模块未绑定控制事件！");
            }
        }

        public void Move(Vector2 input)
        {
            moveDirection = input.normalized;
            StartMoveCoroutine(moveDirection * moveSpeed, accelerationTime);
        }

        public void StopMove()
        {
            StartMoveCoroutine(Vector2.zero, decelerationTime);
        }

        private void StartMoveCoroutine(Vector2 targetVelocity, float time)
        {
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCoroutine(targetVelocity, time));
        }

        private IEnumerator MoveCoroutine(Vector2 targetVelocity, float duration)
        {
            float t = 0f;
            Vector2 startVelocity = rb.velocity;

            while (t < 1f)
            {
                t += Time.fixedDeltaTime / duration;
                rb.velocity = Vector2.Lerp(startVelocity, targetVelocity, t);
                yield return new WaitForFixedUpdate();
            }

            rb.velocity = targetVelocity;
        }
        
        private void Update() {
            // 限制位置在屏幕范围内
            // transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);
        }
    }
}