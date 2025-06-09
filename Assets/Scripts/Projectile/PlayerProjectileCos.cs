using System.Collections;
using UnityEngine;

public class PlayerProjectileCos : PlayerProjectile {
    [SerializeField] private float amplitude = 1f;   // 波峰高度
    [SerializeField] private float frequency = 1f;   // 波动频率
    
    private float elapsedTime;

    protected override void OnEnable() {
        StartCoroutine(MoveSinusoidal());
    }

    private IEnumerator MoveSinusoidal() {
        elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (gameObject.activeSelf) {
            elapsedTime += Time.deltaTime;
        
            // x轴匀速移动，y轴按正弦波摆动
            float x = elapsedTime * moveSpeed;
            float y = Mathf.Cos(x * frequency) * amplitude;

            transform.position = startPosition + new Vector3(x, y, 0f);

            yield return null;
        }
    }
}