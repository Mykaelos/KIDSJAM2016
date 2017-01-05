using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

    Transform Barrel;



    void Awake() {
        Barrel = transform.Find("Barrel");
    }


    void Update() {
        //var x = Input.GetAxis("Horizontal") * 60;
        //var y = Input.GetAxis("Vertical")* 45;
        //Barrel.localEulerAngles = new Vector3(x, 0, y);

        var z = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI - 90;
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
            z = 0;
        }

        Barrel.eulerAngles = new Vector3(0, 0, z);


    }





}
