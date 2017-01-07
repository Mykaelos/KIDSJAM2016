using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    public GameObject BulletPrefab;

    Transform Barrel;
    Transform BarrelExit;

    Timer RefireTimer;
    public float FireRate = 3; //bullets per second
    bool HeldFire = false;

    void Awake() {
        Barrel = transform.Find("Barrel");
        BarrelExit = Barrel.Find("BarrelExit");
        RefireTimer = new Timer();
    }

    void Update() {
        UpdateAim();
        UpdateFire();
    }

    void UpdateAim() {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var z = Mathf.Atan2(vertical, horizontal) * 180 / Mathf.PI - 90;
        if (vertical == 0 && horizontal == 0) {
            z = 0; //Default it to 0 degrees, which is straight up.
        }

        Barrel.eulerAngles = new Vector3(0, 0, z);
    }

    void UpdateFire() {
        if (Input.GetButton("Fire1")) {
            if (RefireTimer.Check(1f / FireRate) || !HeldFire) {
                RefireTimer.Reset();
                Fire();
            }
            HeldFire = true;
        }
        else {
            HeldFire = false;
        }
    }

    void Fire() {
        var bullet = GameObject.Instantiate(BulletPrefab, BarrelExit.position, Quaternion.identity);
        bullet.transform.Rotate(Barrel.eulerAngles); 
    }
}
