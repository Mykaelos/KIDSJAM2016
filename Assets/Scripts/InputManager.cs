//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : MonoBehaviour {
//    public GameObject CannonPrefab;


//    public InputData[] Players = new InputData[2];
//    public int PlayersCount = 0;



//    public InputData[] GamepadPlayers = new InputData[4];
//    public InputData KeyboardMousePlayers;


//    CannonController[] Cannons = new CannonController[2];



//    void Awake() {
//        var cannons = GameObject.FindGameObjectsWithTag("Cannon");

//        for(int i = 0; i < )
//    }



//    void Update() {
//        //listen for new players if needed

//        if (PlayersCount < Players.Length) {



//            for (int i = 0; i < 2; i++) {
//                CheckForGamepadJoin();
//            }
//        }


//    }

//    void CheckForGamepadJoin(int controllerNumber) {
//        if (Input.GetButtonDown("Fire1 p1")) {

//        }
//    }

//    void CheckForKeyboardMouseJoin() {
//        if (KeyboardMousePlayers == null && Input.GetKeyDown(KeyCode.Space) ) {
//            KeyboardMousePlayers = new InputData {
//                PlayerNumber = PlayersCount++,
//                InputControllerType = InputControllerType.KeyboardMouse
//            };
//        }
//    }

//    void GenerateCannon(InputData data) {
//        //set inputData on cannon, and setup inputinterface

//        //create cannon

//        var playerLocations = new Vector2[PlayersCount];

//        playerLocations



//        var bullet = GameObject.Instantiate(CannonPrefab, BarrelExit.position, Quaternion.identity);
//        bullet.transform.Rotate(Barrel.eulerAngles);
//        bullet.transform.SetParent(SpawnBucket, true);
//    }

//}

//public enum InputControllerType {
//    None,
//    KeyboardMouse,
//    Gamepad
//}

//public class InputData {
//    public int PlayerNumber; //0 or 1
//    public InputControllerType InputControllerType;
//}