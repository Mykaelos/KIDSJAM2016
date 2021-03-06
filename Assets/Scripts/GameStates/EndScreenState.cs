﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenState : StateMachineState {
    InputManager InputManager;
    GameData GameData;

    CanvasGroup EndScreenUIGroup;
    Text NumbersText;

    EventChain EventChain;


    public EndScreenState(InputManager inputManager, GameData gameData) {
        InputManager = inputManager;
        GameData = gameData;

        EndScreenUIGroup = GameObject.Find("Canvas/EndScreenUI").GetComponent<CanvasGroup>();
        NumbersText = GameObject.Find("Canvas/EndScreenUI/Numbers").GetComponent<Text>();
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
        NumbersText.text = string.Format(
            "{0}\n{1}\n{2}",
            GameData.BalloonsPopped.ToString("N0"),
            GameData.ShotsFired.ToString("N0"),
            GameData.ExplosiveBalloonsPopped.ToString("N0"));

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
                AudioManager.PlayMusic("MenuMusic", true, false, 0.5f);
            })
        });
    }

    public override void EndFn() {
        EndScreenUIGroup.SetVisible(false);
    }
}
