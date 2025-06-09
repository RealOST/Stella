using UnityEngine;

public class GameSetting : PersistentSingleton<GameSetting> {
    [Header("Audio Settings")]
    [SerializeField] private float masterVolume = 1f;  
    [SerializeField] private float musicVolume = 1f; 
    [SerializeField] private float sfxVolume = 1f;

    [Header("Gameplay Settings")]
    [SerializeField] private bool isShowOnHeadHealthBar = true;
    [SerializeField] private bool isHardcoreMode = false;
    [SerializeField] private float gameSpeed = 1f;

    public float MasterVolume {
        get => masterVolume;
        set {
            masterVolume = Mathf.Clamp01(value);
        }
    }

    public float MusicVolume {
        get => musicVolume;
        set {
            musicVolume = Mathf.Clamp01(value);
        }
    }

    public float SFXVolume {
        get => sfxVolume;
        set {
            sfxVolume = Mathf.Clamp01(value);
        }
    }

    public bool IsHardcoreMode {
        get => isHardcoreMode;
        set => isHardcoreMode = value;
    }
}