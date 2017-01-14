using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayState : StateMachineState {
    CanvasGroup GamePlayUIGroup;
    Text ClockText;
    Text ScoreText;

    Timer GameTimer;
    bool EveryoneLeftGame = false;
    GameData GameData;


    public GamePlayState(GameData gameData) {
        Name = GetType().Name;
        GamePlayUIGroup = GameObject.Find("Canvas/GamePlayUI").GetComponent<CanvasGroup>();
        ClockText = GameObject.Find("Canvas/GamePlayUI/Clock").GetComponent<Text>();
        ScoreText = GameObject.Find("Canvas/GamePlayUI/Score").GetComponent<Text>();

        GameTimer = new Timer(146);
        GameData = gameData;

        // Not the ideal way to do it, but I don't have time to refactor the StateMachine
        // to use proper inheritance for this game jam.
        Check = CheckFn;
        Update = UpdateFn;
        Start = StartFn;
        End = EndFn;
    }

    string CheckFn() {
        //check to see if the timer is finished
        //also could check to see if everyone has left the game?

        if (EveryoneLeftGame) {
            return "TitleScreenState";
        }

        if (GameTimer.Check()) {
            return "EndScreenState";
        }

        return null;
    }

    void UpdateFn() {
        //update the timer
        //update the balloon's popped score?

        UpdateClock();

        if (Application.isEditor) { // For testing in the editor.
            if (Input.GetKeyDown(KeyCode.S)) { // Hit S to quickly end the level.
                GameTimer.SetTimeRemaining(0);
            }
        }
    }

    void StartFn() {
        GamePlayUIGroup.SetVisible(true);

        GameTimer.Reset();
        UpdateClock();
        GameData.Reset();
        EveryoneLeftGame = false;
        UpdateScore();

        AudioManager.MusicVolume = 0.7f; //TODO refactor this to be part of the PlayMusic method.
        AudioManager.PlayMusic("GamePlayMusic", true, false);

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 2f * InputManager.Cannons.Count });

        Messenger.On(BalloonController.MESSAGE_BALLOON_POPPED, IncrementScore);
        Messenger.On(CannonController.MESSAGE_SHOTS_FIRED, IncrementShots);
        Messenger.On(InputManager.MESSAGE_EVERYONE_LEFT_GAME, OnEveryoneLeftGame);
        Messenger.On(InputManager.MESSAGE_PLAYER_COUNT_CHANGED, PlayerCountChanged);
    }

    void EndFn() {
        GamePlayUIGroup.SetVisible(false);
        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0f });

        Messenger.Un(BalloonController.MESSAGE_BALLOON_POPPED, IncrementScore);
        Messenger.Un(CannonController.MESSAGE_SHOTS_FIRED, IncrementShots);
        Messenger.Un(InputManager.MESSAGE_EVERYONE_LEFT_GAME, OnEveryoneLeftGame);
        Messenger.Un(InputManager.MESSAGE_PLAYER_COUNT_CHANGED, PlayerCountChanged);
    }

    void PlayerCountChanged(object[] args = null) {
        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 2f * InputManager.Cannons.Count });
    }

    void OnEveryoneLeftGame(object[] args = null) {
        EveryoneLeftGame = true;
    }

    void UpdateClock() {
        var timeSpan = TimeSpan.FromSeconds(GameTimer.DurationUntilNext());
        ClockText.text = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    void IncrementScore(object[] args = null) {
        GameData.BalloonsPopped++;
        UpdateScore();
    }

    void IncrementShots(object[] args = null) {
        GameData.ShotsFired++;
    }

    void UpdateScore() {
        ScoreText.text = GameData.BalloonsPopped.ToString("N0") + " Popped";
    }
}
