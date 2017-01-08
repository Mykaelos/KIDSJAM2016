using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInterface {

    public virtual float GetAimingAngle(Transform self = null) {        
        return 0;
    }

    public virtual bool GetFiringButton() {
        return false;
    }
}
