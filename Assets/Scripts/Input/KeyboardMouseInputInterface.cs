using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMouseInputInterface : InputInterface {

    public override float GetAimingAngle(Transform self = null) {
        if (self == null) {
            return 0;
        }
        Vector3 mouseLoc = Input.mousePosition;
        Vector3 target = Camera.main.ScreenToWorldPoint(mouseLoc);
        var zRotation = (Mathf.Atan2(target.y - self.position.y, target.x - self.position.x) * 180 / Mathf.PI - 90);

        return zRotation;
    }

    public override bool GetFiringButton() {
        return Input.GetButton("Fire1 KeyboardMouse");
    }
}
