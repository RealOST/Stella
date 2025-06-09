using System;
using UnityEngine;

public class Planet : MonoBehaviour {
    protected float paddingX;
    private float paddingY;
    private void Awake() {
        paddingX = 10f;
        paddingY = 10f;
        
        transform.position = Viewport.Instance.RandomPlanetSpawnPosition(paddingX, paddingY);
    }
}
