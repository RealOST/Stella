using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : PersistentSingleton<GameManager> {
    protected override void Awake() {
        base.Awake();
        EventBus.Subscribe<GameOverEvent>(_ =>  GameState = GameState.GameOver);
    }

    public static GameState GameState {
        get => Instance.gameState;
        set => Instance.gameState = value;
    }

    [SerializeField] private GameState gameState = GameState.Playing;
}

public enum GameState {
    Playing,
    Paused,
    GameOver,
    Scoring
}