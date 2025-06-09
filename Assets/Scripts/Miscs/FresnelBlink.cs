using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FresnelBlink : MonoBehaviour {
    [Header("Blink Settings")]
    public float blinkSpeed = 6f; // 闪烁频率（越大越快）

    public float duration = 1.5f; // 总持续时间（秒）
    public string fresnelProperty = "_FresnelPower"; // Shader中Fresnel控制变量名

    private float timer;
    private Renderer rend;
    private MaterialPropertyBlock propBlock;
    private bool isBlinking = false;

    private void Awake() {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    private void OnEnable() {
        timer = 0f;
        isBlinking = true;
    }

    private void Update() {
        if (!isBlinking) return;

        timer += Time.deltaTime;

        // 使用 sin 控制闪烁 (0~1)
        var blinkValue = Mathf.Sin(Time.time * blinkSpeed) * 0.5f + 0.5f;

        rend.GetPropertyBlock(propBlock);
        propBlock.SetFloat(fresnelProperty, blinkValue);
        rend.SetPropertyBlock(propBlock);

        if (timer >= duration) {
            // 保证完整一个周期后再关闭
            isBlinking = false;
            // 最后一次设置为0
            propBlock.SetFloat(fresnelProperty, 0f);
            rend.SetPropertyBlock(propBlock);
            gameObject.SetActive(false); // 或者调用 ReturnToPool()
        }
    }
}