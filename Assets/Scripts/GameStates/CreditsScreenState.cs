using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreenState : StateMachineState {
    InputManager InputManager;

    CanvasGroup CreditsScreenUIGroup;


    public CreditsScreenState(InputManager inputManager) {
        InputManager = inputManager;

        CreditsScreenUIGroup = GameObject.Find("Canvas/CreditsScreenUI").GetComponent<CanvasGroup>();
    }

    public override string CheckFn() {
        if (InputManager.DidAnyonePressStart()) {
            return ClassNameOf<InstructionsScreenState>();
        }

        return null;
    }

    public override void StartFn() {
        CreditsScreenUIGroup.SetVisible(true);

        AudioManager.MusicVolume = 0.3f; //TODO refactor this to be part of the PlayMusic method.
        AudioManager.PlayMusic("MenuMusic", true, false);

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0.5f });
    }

    public override void EndFn() {
        CreditsScreenUIGroup.SetVisible(false);
    }
}
