using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour {
    [SerializeField] protected AudioData defaultPickUpSFX;

    private int pickUpStateID = Animator.StringToHash("PickUp");

    protected AudioData pickUpSFX;

    private Animator animator;

    protected Player player;

    protected Text lootMessage;

    private void Awake() {
        animator = GetComponent<Animator>();

        player = FindObjectOfType<Player>();

        lootMessage = GetComponentInChildren<Text>(true);

        pickUpSFX = defaultPickUpSFX;
    }

    private void OnEnable() {
        StartCoroutine(MoveCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PickUp();
    }

    protected virtual void PickUp() {
        StopAllCoroutines();
        animator.Play(pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);
    }

    private IEnumerator MoveCoroutine() {
        float speed = 120f;
        float angle = Random.Range(0f, 360f); // 初始角度
        Vector3 centerPosition = transform.position; // 圆心初始位置
        float radius = 0.5f; // 圆周运动的半径
        float moveSpeed = 0.5f; // 圆心水平移动的速度

        while (true) {
            // 每次增加角度来实现圆周运动
            angle += speed * Time.deltaTime;

            // 计算新的位置，基于圆周的数学公式
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            // 让物体沿圆周轨迹运动，圆心持续向左移动
            transform.position = centerPosition + new Vector3(x, y, 0);

            // 圆心位置逐渐向左移动
            centerPosition.x -= moveSpeed * Time.deltaTime;

            yield return null;
        }
    }
}
