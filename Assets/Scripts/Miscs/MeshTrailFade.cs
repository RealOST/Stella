using UnityEngine;
using System.Collections;

public class MeshTrailFade : MonoBehaviour {
    private MeshRenderer meshRenderer;
    private Material trailMaterial;
    private Color originalColor;
    private float fadeDuration = 0.5f;

    public void Play(float duration) {
        fadeDuration = duration;
        StartCoroutine(FadeAndDisable());
    }

    private void OnEnable() {
        meshRenderer = GetComponent<MeshRenderer>();
        // 创建一个独立材质实例（否则会影响共享材质）
        trailMaterial = new Material(meshRenderer.sharedMaterial);
        meshRenderer.material = trailMaterial;

        // 保存原始颜色，并确保透明度为1（可见）
        originalColor = trailMaterial.color;
        originalColor.a = 1f;
        trailMaterial.color = originalColor;
    }

    private IEnumerator FadeAndDisable() {
        var elapsed = 0f;
        while (elapsed < fadeDuration) {
            var alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / fadeDuration);
            var c = originalColor;
            c.a = alpha;
            trailMaterial.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false); // 回收对象
    }
}