using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenState : StateMachineState {
    CanvasGroup TitleScreenUIGroup;

    string[] StartButtonNames = new string[] {
        "Start KeyboardMouse",
        "Start Gamepad0",
        "Start Gamepad1",
        "Start Gamepad2",
        "Start Gamepad3",
    };


    public TitleScreenState() {
        Name = GetType().Name;
        TitleScreenUIGroup = GameObject.Find("Canvas/TitleScreenUI").GetComponent<CanvasGroup>();

        // Not the ideal way to do it, but I don't have time to refactor the StateMachine
        // to use proper inheritance for this game jam.
        Check = CheckFn;
        Start = StartFn;
        End = EndFn;
    }

    string CheckFn() {
        for(int i = 0; i < StartButtonNames.Length; i++) {
            if (Input.GetButtonDown(StartButtonNames[i])) {
                return "GamePlayState";
            }
        }

        return null;
    }

    void StartFn() {
        TitleScreenUIGroup.SetVisible(true);

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0.5f });
    }

    void EndFn() {
        TitleScreenUIGroup.SetVisible(false);
    }
}
