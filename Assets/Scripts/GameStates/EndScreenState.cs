using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenState : StateMachineState {
    InputManager InputManager;
    GameData GameData;

    CanvasGroup EndScreenUIGroup;
    Text DetailsText;


    public EndScreenState(InputManager inputManager, GameData gameData) {
        InputManager = inputManager;
        GameData = gameData;

        EndScreenUIGroup = GameObject.Find("Canvas/EndScreenUI").GetComponent<CanvasGroup>();
        DetailsText = GameObject.Find("Canvas/EndScreenUI/Details").GetComponent<Text>();
    }

    public override string CheckFn() {
        if (InputManager.DidAnyonePressStart()) {
            return ClassNameOf<TitleScreenState>();
        }

        return null;
    }

    public override void StartFn() {
        EndScreenUIGroup.SetVisible(true);

        AudioManager.MusicVolume = 0.3f; //TODO refactor this to be part of the PlayMusic method.
        AudioManager.PlayMusic("MenuMusic", true, false);

        //Collect all of the data
        DetailsText.text = string.Format(
            "Balloons Popped: {0} \nShots Fired: {1}\n\nThanks for playing!",
            GameData.BalloonsPopped.ToString("N0"),
            GameData.ShotsFired.ToString("N0"));

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0f });
        Messenger.Fire(SpawnController.MESSAGE_REMOVE_BALLOONS);
    }

    public override void EndFn() {
        EndScreenUIGroup.SetVisible(false);
    }
}
