using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public const string PLAYER_JOINED = "PLAYER_JOINED";
    public const string PLAYER_EXITED = "PLAYER_EXITED";

    private static string[] StartButtonNames = new string[] {
        "Start KeyboardMouse",
        "Start Gamepad0",
        "Start Gamepad1",
        "Start Gamepad2",
        "Start Gamepad3",
    };

    public LocalMessenger LocalMessenger = new LocalMessenger();

    InputData[] PlayerSlots = new InputData[5];
    InputData[] GamepadPlayers = new InputData[4];
    InputData KeyboardMousePlayers;


    public bool DidAnyonePressStart() {
        for (int i = 0; i < StartButtonNames.Length; i++) {
            if (Input.GetButtonDown(StartButtonNames[i])) {
                return true;
            }
        }

        return false;
    }

    void Update() {
        for (int i = 0; i < GamepadPlayers.Length; i++) {
            CheckForGamepadJoin(i);
            CheckForGamepadExit(i);
        }

        CheckForKeyboardMouseJoin();
        CheckForKeyboardMouseExit();
    }

    void CheckForGamepadJoin(int controllerNumber) {
        if (GamepadPlayers[controllerNumber] == null && Input.GetButtonDown("Fire1 Gamepad" + controllerNumber.ToString())) {
            var slotNumber = GetNextFreePlayerSlot();

            var inputData = new InputData {
                PlayerSlot = slotNumber,
                InputControllerType = InputControllerType.Gamepad,
                GamepadNumber = controllerNumber
            };

            GamepadPlayers[controllerNumber] = inputData;
            PlayerSlots[slotNumber] = inputData;

            LocalMessenger.Fire(PLAYER_JOINED, new object[] { inputData });
        }
    }

    void CheckForGamepadExit(int controllerNumber) {
        if (GamepadPlayers[controllerNumber] != null && Input.GetButtonDown("Back Gamepad" + controllerNumber.ToString())) {
            var inputData = GamepadPlayers[controllerNumber];

            PlayerSlots[inputData.PlayerSlot] = null;
            GamepadPlayers[controllerNumber] = null;

            LocalMessenger.Fire(PLAYER_EXITED, new object[] { inputData });
        }
    }

    void CheckForKeyboardMouseJoin() {
        if (KeyboardMousePlayers == null && Input.GetButtonDown("Fire1 KeyboardMouse")) {
            var slotNumber = GetNextFreePlayerSlot();

            var inputData = new InputData {
                PlayerSlot = slotNumber,
                InputControllerType = InputControllerType.KeyboardMouse
            };

            KeyboardMousePlayers = inputData;
            PlayerSlots[slotNumber] = inputData;

            LocalMessenger.Fire(PLAYER_JOINED, new object[] { inputData });
        }
    }

    void CheckForKeyboardMouseExit() {
        if (KeyboardMousePlayers != null && Input.GetButtonDown("Back KeyboardMouse")) {
            var inputData = KeyboardMousePlayers;

            PlayerSlots[inputData.PlayerSlot] = null;
            KeyboardMousePlayers = null;

            LocalMessenger.Fire(PLAYER_EXITED, new object[] { inputData });
        }
    }

    int GetNextFreePlayerSlot() {
        for (int i = 0; i < PlayerSlots.Length; i++) {
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
