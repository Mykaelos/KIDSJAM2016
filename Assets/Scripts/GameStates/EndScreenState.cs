using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenState : StateMachineState {
    CanvasGroup EndScreenUIGroup;
    Text DetailsText;

    string[] StartButtonNames = new string[] {
        "Start KeyboardMouse",
        "Start Gamepad0",
        "Start Gamepad1",
        "Start Gamepad2",
        "Start Gamepad3",
    };


    public EndScreenState() {
        Name = GetType().Name;
        EndScreenUIGroup = GameObject.Find("Canvas/EndScreenUI").GetComponent<CanvasGroup>();
        DetailsText = GameObject.Find("Canvas/EndScreenUI/Details").GetComponent<Text>();

        // Not the ideal way to do it, but I don't have time to refactor the StateMachine
        // to use proper inheritance for this game jam.
        Check = CheckFn;
        Start = StartFn;
        End = EndFn;
    }

    string CheckFn() {
        for (int i = 0; i < StartButtonNames.Length; i++) {
            if (Input.GetButtonDown(StartButtonNames[i])) {
                return "TitleScreenState";
            }
        }

        return null;
    }

    void StartFn() {
        EndScreenUIGroup.SetVisible(true);

        //Collect all of the data

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0f });
    }

    void EndFn() {
        EndScreenUIGroup.SetVisible(false);
    }
}
