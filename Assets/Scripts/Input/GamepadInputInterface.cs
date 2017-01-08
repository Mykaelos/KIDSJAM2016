using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadInputInterface : MonoBehaviour {

    public virtual float GetAimingAngle(Transform self = null) {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var z = Mathf.Atan2(vertical, horizontal) * 180 / Mathf.PI - 90;
        if (vertical == 0 && horizontal == 0) {
            z = 0; //Default it to 0 degrees, which is straight up.
        }

        return z;
    }

    public virtual bool GetFiringButton() {
        return Input.GetButton("Fire1 Gamepad");
    }
}
