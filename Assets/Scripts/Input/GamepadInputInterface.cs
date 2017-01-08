using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadInputInterface : InputInterface {
    private int GamepadNumber;

    private string HorizontalAxesName;
    private string VerticalAxesName;
    private string FireButtonName;


    public GamepadInputInterface(int gamepadNumber) {
        GamepadNumber = gamepadNumber;

        // Saving a bit of memory so I don't recreate the strings every button check.
        HorizontalAxesName = "Horizontal Gamepad" + GamepadNumber.ToString();
        VerticalAxesName = "Vertical Gamepad" + GamepadNumber.ToString();
        FireButtonName = "Fire1 Gamepad" + GamepadNumber.ToString();
    }

    public override float GetAimingAngle(Transform self = null) {
        var horizontal = Input.GetAxis(HorizontalAxesName);
        var vertical = Input.GetAxis(VerticalAxesName);

        var z = Mathf.Atan2(vertical, horizontal) * 180 / Mathf.PI - 90;
        if (vertical == 0 && horizontal == 0) {
            z = 0; //Default it to 0 degrees, which is straight up.
        }

        return z;
    }

    public override bool GetFiringButton() {
        return Input.GetButton(FireButtonName);
    }
}
