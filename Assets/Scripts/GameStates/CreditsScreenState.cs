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

        AudioManager.PlayMusic("MenuMusic", true, false, 0.5f);

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0.5f });
    }

    public override void EndFn() {
        CreditsScreenUIGroup.SetVisible(false);
    }
}
