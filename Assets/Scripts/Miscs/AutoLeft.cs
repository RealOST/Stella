using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLeft : MonoBehaviour {
    // Start is called before the first frame update

    private float speed = 0.05f;
    private void Start() {
    }

    // Update is called once per frame
    private void FixedUpdate() {
        var p = transform.position;
        p.x -= speed;
        transform.position = p;
    }
}