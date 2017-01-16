using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenState : StateMachineState {
    InputManager InputManager;
    GameData GameData;

    CanvasGroup EndScreenUIGroup;
    Text DetailsText;

    EventChain EventChain;


    public EndScreenState(InputManager inputManager, GameData gameData) {
        InputManager = inputManager;
        GameData = gameData;

        EndScreenUIGroup = GameObject.Find("Canvas/EndScreenUI").GetComponent<CanvasGroup>();
        DetailsText = GameObject.Find("Canvas/EndScreenUI/Details").GetComponent<Text>();
    }

    public override string CheckFn() {
        if (InputManager.DidAnyonePressStart()) {
            if(EventChain != null) {
                EventChain.Stop();
            }

            return ClassNameOf<CreditsScreenState>();
        }

        return null;
    }

    public override void StartFn() {
        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0f });

        //Collect all of the data
        DetailsText.text = string.Format(
            "Balloons Popped: {0} \nShots Fired: {1}\n\nThanks for playing!",
            GameData.BalloonsPopped.ToString("N0"),
            GameData.ShotsFired.ToString("N0"));

        EventChain = EventChain.Begin(new List<EventLink> {
            new CallFunctionLink(delegate(object[] args) {
                Messenger.Fire(SpawnController.MESSAGE_REMOVE_BALLOONS);
                AudioManager.StopMusic();
            }),
            new WaitLink(2f),
            new CallFunctionLink(delegate(object[] args) {
                AudioManager.PlaySound("Yay", 1, 1.5f);
                EndScreenUIGroup.SetVisible(true);
            }),
            new WaitLink(2.5f),
            new CallFunctionLink(delegate(object[] args) {
                AudioManager.MusicVolume = 0.3f; //TODO refactor this to be part of the PlayMusic method.
                AudioManager.PlayMusic("MenuMusic", true, false);
            }),
        });
    }

    public override void EndFn() {
        EndScreenUIGroup.SetVisible(false);
    }
}
