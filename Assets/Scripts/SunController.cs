using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour {
    Timer RotationTimer;
    public float RotationDelay = 1;


    void Awake() {
        RotationTimer = new Timer();
    }

    void Update() {
        if (RotationTimer.Check(RotationDelay)) {
            RotationTimer.Reset();

            transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
        }
    }
}
