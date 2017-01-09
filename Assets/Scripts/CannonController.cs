using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    InputInterface InputInterface = new InputInterface();
    InputData InputData;

    public GameObject BulletPrefab;
    Transform SpawnBucket;

    Transform Barrel;
    Transform BarrelExit;
    AudioSource AudioSource;

    Timer RefireTimer;
    public float FireRate = 3; //bullets per second
    bool HeldFire = false;


    public void Setup(InputData inputData) {
        InputData = inputData;

        if (InputData.InputControllerType == InputControllerType.KeyboardMouse) {
            InputInterface = new KeyboardMouseInputInterface();
        }
        else if (InputData.InputControllerType == InputControllerType.Gamepad) {
            InputInterface = new GamepadInputInterface(InputData.GamepadNumber);
        }
    }

    public int GetPlayerSlot() {
        return InputData.PlayerSlot;
    }

    void Awake() {
        SpawnBucket = GameObject.Find("SpawnBucket").transform;
        Barrel = transform.Find("Barrel");
        BarrelExit = Barrel.Find("BarrelExit");
        AudioSource = GetComponent<AudioSource>();
        RefireTimer = new Timer();
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

        // An ok hack for now. AudioManager needs to be refactored to not use the Resources folder.
        AudioManager.PlaySound(AudioSource, Random.Range(0.95f, 1.05f), 0.8f);
    }
}

/* Ideas:
- Could add a player number or controller name to the cannon to help tell who is who.
- Could change the color of each cannon, maybe randomly or from a pile of options.




*/