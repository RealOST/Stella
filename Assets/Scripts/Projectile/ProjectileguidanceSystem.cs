using System.Collections;
using UnityEngine;

public class ProjectileguidanceSystem : MonoBehaviour {
    [SerializeField] private Projectile projectile;
    [SerializeField] private float minBallisticAngle = -50f;
    [SerializeField] private float maxBallisticAngle = 50f;

    private float ballisticAngle;

    private Vector3 targetDirection;

    public IEnumerator HomingCoroutine(GameObject target) {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
        while (gameObject.activeSelf) {
            if (target.activeSelf) {
                // 获取从当前物体指向目标的方向向量（已经归一化）
                var targetDirection = (target.transform.position - transform.position).normalized;

                // 计算该向量的角度（注意是以Vector2.right为基准旋转角）
                var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

                // 设置当前物体的旋转，让它“面朝”目标方向 + 一定角度偏移（弹道抖动）
                transform.rotation = Quaternion.Euler(0f, 0f, angle + ballisticAngle);
                projectile.Move();
            }
            else {
                projectile.Move();
            }

            yield return null;
        }
    }
}