using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    InputInterface InputInterface = new InputInterface();

    public GameObject BulletPrefab;
    Transform SpawnBucket;

    Transform Barrel;
    Transform BarrelExit;

    Timer RefireTimer;
    public float FireRate = 3; //bullets per second
    bool HeldFire = false;


    public void SetInputInterface(InputInterface inputInterface) {
        InputInterface = inputInterface;
    }

    void Awake() {
        SpawnBucket = GameObject.Find("SpawnBucket").transform;
        Barrel = transform.Find("Barrel");
        BarrelExit = Barrel.Find("BarrelExit");
        RefireTimer = new Timer();

        InputInterface = new KeyboardMouseInputInterface(); //TODO Remove
    }

    void Update() {
        UpdateAim();
        UpdateFire();
    }

    void UpdateAim() {
        Barrel.eulerAngles = new Vector3(0, 0, InputInterface.GetAimingAngle(transform));
    }

    void UpdateFire() {
        if (InputInterface.GetFiringButton()) {
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
        bullet.transform.SetParent(SpawnBucket, true);
    }
}
