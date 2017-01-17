using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour {
    Timer RotationTimer;
    public float RotationDelay = 1;


    void Awake() {
        RotationTimer = new Timer();

        var screenRect = Camera.main.VisibleWorldRect();
        transform.position = new Vector3(screenRect.x + screenRect.width - 1, screenRect.y + screenRect.height - 1);
    }

    void Update() {
        if (RotationTimer.Check(RotationDelay)) {
            RotationTimer.Reset();

            transform.Rotate(Vector3.forward, Random.Range(-360f, 360f));
        }
    }
}
