using UnityEngine;
using UnityEngine.UI;

public class RandomBG : MonoBehaviour {
    [SerializeField] private Image background;
    [SerializeField] private Sprite[] backgroundImages;

    private void OnEnable() {
        ShowRandomBackground();
    }

    private void ShowRandomBackground() {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }
}