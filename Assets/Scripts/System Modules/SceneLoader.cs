using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader> {
    [SerializeField] private Image transitionImage;
    [SerializeField] private float fadeTime = 3.5f;

    private Color color;
    private bool isLoading = false; // 🔐 加载锁

    private const string GAMEPLAY = "Gameplay";
    private const string MAIN_MENU = "MainMenu";
    private const string SCORING = "Scoring";
    
    private IEnumerator LoadCoroutine(string sceneName) {
        if (isLoading) yield break; // 防止重复加载
        isLoading = true;
        
        Debug.Log("👀 开始加载"+sceneName);
        // Load new scene in background
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        // Set this scene inactive
        loadingOperation.allowSceneActivation = false;
        Debug.Log("⏳ 正在加载"+sceneName);

        transitionImage.gameObject.SetActive(true);

        // Fade out
        while (color.a < 1f) {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        // TODO
        while (loadingOperation.progress < 0.9f) {
            yield return null;
        }
        Debug.Log("✅ 加载完成，准备激活"+sceneName);

        // Activate the new scene
        loadingOperation.allowSceneActivation = true;

        // Fade in
        while (color.a > 0f) {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        transitionImage.gameObject.SetActive(false);
        isLoading = false; // 释放加载锁
    }

    public void LoadGameplayScene() {
        if (!isLoading)
        StartCoroutine(LoadCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene() {
        if (!isLoading)
        StartCoroutine(LoadCoroutine(MAIN_MENU));
    }

    public void LoadScoringScene() {
        if (!isLoading)
        StartCoroutine(LoadCoroutine(SCORING));
    }
}