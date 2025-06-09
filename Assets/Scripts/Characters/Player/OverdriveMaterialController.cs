using UnityEngine;

public class OverdriveMaterialController : MonoBehaviour
{
    [SerializeField] Material overdriveMaterial;
    
    Material defaultMaterial;

    new Renderer renderer;
    
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterial = renderer.material;
    }
    
    void OnEnable()
    {
        // PlayerOverdrive.on += PlayerOverdriveOn;
        // PlayerOverdrive.off += PlayerOverdriveOff;
        EventBus.Subscribe<OverdriveEvent>(HandleOverdriveEvent);
    }

    void OnDisable()
    {
        // PlayerOverdrive.on -= PlayerOverdriveOn;
        // PlayerOverdrive.off -= PlayerOverdriveOff;     
        EventBus.Unsubscribe<OverdriveEvent>(HandleOverdriveEvent);
    }

    void HandleOverdriveEvent(OverdriveEvent evt) {
        if (evt.IsOn)
            PlayerOverdriveOn();
        else
            PlayerOverdriveOff();
    }

    void PlayerOverdriveOn() => renderer.material = overdriveMaterial;

    void PlayerOverdriveOff() => renderer.material = defaultMaterial;
}