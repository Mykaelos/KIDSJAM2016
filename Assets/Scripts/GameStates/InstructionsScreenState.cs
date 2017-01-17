using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScreenState : StateMachineState {
    InputManager InputManager;

    CanvasGroup InstructionsScreenUIGroup;


    public InstructionsScreenState(InputManager inputManager) {
        InputManager = inputManager;

        InstructionsScreenUIGroup = GameObject.Find("Canvas/InstructionsScreenUI").GetComponent<CanvasGroup>();
    }

    public override string CheckFn() {
        if (InputManager.DidAnyonePressStart()) {
            return ClassNameOf<GamePlayState>();
        }

        return null;
    }

    public override void StartFn() {
        InstructionsScreenUIGroup.SetVisible(true);

        AudioManager.PlayMusic("MenuMusic", true, false, 0.5f);

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0.5f });
    }

    public override void EndFn() {
        InstructionsScreenUIGroup.SetVisible(false);
    }
}
