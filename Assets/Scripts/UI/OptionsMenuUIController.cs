using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionsMenuUIController : MonoBehaviour {
    [Header("==== PlAYER INPUT ====")]
    [SerializeField] private PlayerInput playerInput;

    [Header("==== CANVAS ====")]
    [SerializeField] private Canvas mainMenuCanvas;

    [SerializeField] private Canvas optionsMenuCanvas;

    [Header("==== BUTTONS ====")]
    [SerializeField] private Button btn_GamePlay;

    [SerializeField] private Button btn_Video;
    [SerializeField] private Button btn_Audio;

    [SerializeField] private Button buttonStart;

    [Header("==== AUDIO DATA ====")]
    [SerializeField] private AudioData cancelSFX;


    [Header("==== HighLights ====")]
    public GameObject lineGameplay;

    public GameObject lineVideo;
    public GameObject lineAudio;

    [Header("==== Panels ====")]
    public GameObject PanelGameplay;

    public GameObject PanelVideo;
    public GameObject PanelAudio;


    private int currentTabIndex = 0; // 当前选中的Tab：0=Gameplay, 1=Video, 2=Audio
    private GameObject[] tabPanels;
    private GameObject[] tabHighlight;
    private GameObject[] firstOptionInPanels; // 每个面板第一个可选的子选项
    public GameObject fullScreenMask;

    private void OnEnable() {
        // playerInput.onPause += Pause;
        // playerInput.onUnpause += Unpause;
        playerInput.onTabSwitch += TabSwitch;
        playerInput.onExit += Cancel;
    }

    private void OnDisable() {
        // playerInput.onPause -= Pause;
        // playerInput.onUnpause -= Unpause;
        playerInput.onTabSwitch -= TabSwitch;
        playerInput.onExit -= Cancel;
    }

    private void Start() {
        playerInput.EnableGameplayInput();

        tabPanels = new GameObject[] { PanelGameplay, PanelVideo, PanelAudio };
        tabHighlight = new GameObject[] { lineGameplay, lineVideo, lineAudio };

        firstOptionInPanels = new GameObject[] {
            PanelGameplay.transform.GetChild(0).gameObject,
            PanelVideo.transform.GetChild(0).gameObject,
            PanelAudio.transform.GetChild(0).gameObject
        };

        SelectCurrentTab();
    }

    private void TabSwitch(float value) {
        if (value < 0)
            currentTabIndex = (currentTabIndex - 1 + tabPanels.Length) % tabPanels.Length;
        else if (value > 0) currentTabIndex = (currentTabIndex + 1) % tabPanels.Length;
        SelectCurrentTab();
    }

    private void SelectCurrentTab() {
        DisablePanels();
        tabPanels[currentTabIndex].SetActive(true);
        tabHighlight[currentTabIndex].SetActive(true);

        UIInput.Instance.SelectUI(firstOptionInPanels[currentTabIndex].GetComponent<Selectable>());
    }

    private void Cancel() {
        fullScreenMask.SetActive(false);
        mainMenuCanvas.enabled = true;
        optionsMenuCanvas.enabled = false;
        AudioManager.Instance.PlaySFX(cancelSFX);
        UIInput.Instance.SelectUI(buttonStart);
    }


    public void GamePlayPanel() {
        currentTabIndex = 0;
        SelectCurrentTab();
    }

    public void VideoPanel() {
        currentTabIndex = 1;
        SelectCurrentTab();
    }

    public void AudioPanel() {
        currentTabIndex = 2;
        SelectCurrentTab();
    }

    private void DisablePanels() {
        foreach (var panel in tabPanels) panel.SetActive(false);
        foreach (var line in tabHighlight) line.SetActive(false);
    }
}