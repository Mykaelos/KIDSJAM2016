using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public GameObject CannonPrefab;
    
    public InputData[] GamepadPlayers = new InputData[4];
    public InputData KeyboardMousePlayers;

    public int PlayersCount = 0;
    public GameObject[] PlayerSlots = new GameObject[5];

    List<CannonController> Cannons = new List<CannonController>();


    void Update() {
        // Listen for new players wanting to join
        if (PlayersCount < PlayerSlots.Length) {
            for (int i = 0; i < GamepadPlayers.Length; i++) {
                CheckForGamepadJoin(i);
            }

            CheckForKeyboardMouseJoin();
        }
    }

    void CheckForGamepadJoin(int controllerNumber) {
        if (GamepadPlayers[controllerNumber] == null && Input.GetButtonDown("Fire1 Gamepad" + controllerNumber.ToString())) {
            PlayersCount++;
            var slotNumber = GetNextFreePlayerSlot();

            GamepadPlayers[controllerNumber] = new InputData {
                PlayerSlot = slotNumber,
                InputControllerType = InputControllerType.Gamepad,
                GamepadNumber = controllerNumber
            };

            var cannon = GenerateCannon(GamepadPlayers[controllerNumber]);
            PlayerSlots[slotNumber] = cannon;
        }

        if (GamepadPlayers[controllerNumber] != null && Input.GetButtonDown("Back Gamepad" + controllerNumber.ToString())) {
            RemoveCannon(GamepadPlayers[controllerNumber]);
            GamepadPlayers[controllerNumber] = null;
        }
    }

    void CheckForKeyboardMouseJoin() {
        if (KeyboardMousePlayers == null && Input.GetButtonDown("Fire1 KeyboardMouse")) {
            PlayersCount++;
            var slotNumber = GetNextFreePlayerSlot();

            KeyboardMousePlayers = new InputData {
                PlayerSlot = slotNumber,
                InputControllerType = InputControllerType.KeyboardMouse
            };

            var cannon = GenerateCannon(KeyboardMousePlayers);
            PlayerSlots[slotNumber] = cannon;
        }

        if (KeyboardMousePlayers != null && Input.GetButtonDown("Back KeyboardMouse")) {
            RemoveCannon(KeyboardMousePlayers);
            KeyboardMousePlayers = null;
        }
    }

    GameObject GenerateCannon(InputData data) {
        // Create the new cannon at its location
        GameObject cannon = GameObject.Instantiate(CannonPrefab, Vector2.zero, Quaternion.identity);
        var cannonController = cannon.GetComponent<CannonController>();
        cannonController.Setup(data);
        Cannons.Add(cannonController);

        MoveCannonsToLocations();

        return cannon;
    }

    void RemoveCannon(InputData data) {
        var tempList = new List<CannonController>(Cannons);
        for (int i = 0; i < tempList.Count; i++) {
            if (tempList[i].GetPlayerSlot() == data.PlayerSlot) {
                Destroy(Cannons[i].gameObject);
                Cannons.RemoveAt(i);
                PlayerSlots[data.PlayerSlot] = null;
                PlayersCount--;
                break;
            }
        }

        MoveCannonsToLocations();
    }

    void MoveCannonsToLocations() {
        // Determine new cannon locations based on the number of players in the game
        var screenRect = Camera.main.VisibleWorldRect();
        var playerLocations = new Vector2[PlayersCount];

        float sectionWidth = screenRect.width / (playerLocations.Length + 1f);
        float nextXLocation = screenRect.x + sectionWidth;

        for (int i = 0; i < playerLocations.Length; i++) {
            playerLocations[i] = new Vector2(nextXLocation, screenRect.y);

            nextXLocation += sectionWidth;
        }

        // Move existing cannons to their positions
        for (int i = 0; i < Cannons.Count; i++) {
            Cannons[i].transform.position = playerLocations[i];
        }
    }

    int GetNextFreePlayerSlot() {
        for(int i = 0; i < PlayerSlots.Length; i++) {
            if (PlayerSlots[i] == null) {
                return i;
            }
        }

        return -1;
    }
}

public enum InputControllerType {
    None,
    KeyboardMouse,
    Gamepad
}

public class InputData {
    public int PlayerSlot;
    public int GamepadNumber;
    public InputControllerType InputControllerType;
}
