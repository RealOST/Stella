using UnityEngine;

public class PlayerOverdrive : MonoBehaviour {
    // public static UnityAction on = delegate { };
    // public static UnityAction off = delegate { };

    [SerializeField] private GameObject triggerVFX;
    [SerializeField] private GameObject offVFX;
    [SerializeField] private GameObject engineVFXNormal;
    [SerializeField] private GameObject engineVFXOverdrive;
    [SerializeField] private AudioData onSFX;
    [SerializeField] private AudioData onSFX2;
    [SerializeField] private AudioData offSFX;
    [SerializeField] [Range(-3, 3)] private float audioPitch = 1f;

    private SmartParticleGroupStopper vFXStopper;

    private void Awake() {
        // on += On;
        // off += Off;
        EventBus.Subscribe<OverdriveEvent>(HandleOverdriveEvent);
        vFXStopper = engineVFXOverdrive.GetComponent<SmartParticleGroupStopper>();
    }

    private void OnDestroy() {
        // on -= On;
        // off -= Off;
        EventBus.Unsubscribe<OverdriveEvent>(HandleOverdriveEvent);
    }
    
    void HandleOverdriveEvent(OverdriveEvent evt) {
        if (evt.IsOn)
            On();
        else
            Off();
    }

    private void On() {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
        AudioManager.Instance.PlaySFX(onSFX2, audioPitch);
    }

    private void Off() {
        offVFX.SetActive(true);
        engineVFXNormal.SetActive(true);
        // engineVFXOverdrive.SetActive(false);
        vFXStopper.SmoothStopAndDisable();
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}