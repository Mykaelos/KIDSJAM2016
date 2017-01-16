using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayState : StateMachineState {
    InputManager InputManager;
    GameData GameData;

    CanvasGroup GamePlayUIGroup;
    Text ClockText;
    Text ScoreText;

    Timer GameTimer;


    public GamePlayState(InputManager inputManager, GameData gameData) {
        InputManager = inputManager;
        GameData = gameData;

        GamePlayUIGroup = GameObject.Find("Canvas/GamePlayUI").GetComponent<CanvasGroup>();
        ClockText = GameObject.Find("Canvas/GamePlayUI/Clock").GetComponent<Text>();
        ScoreText = GameObject.Find("Canvas/GamePlayUI/Score").GetComponent<Text>();

        GameTimer = new Timer(146);
    }

    public override string CheckFn() {
        if (GameTimer.Check()) {
            return ClassNameOf<EndScreenState>();
        }

        return null;
    }

    public override void UpdateFn() {
        UpdateClock();

        if (Application.isEditor) { // For testing in the editor.
            if (Input.GetKeyDown(KeyCode.S)) { // Hit S to quickly end the level.
                GameTimer.SetTimeRemaining(0);
            }
        }
    }

    public override void StartFn() {
        GamePlayUIGroup.SetVisible(true);

        GameTimer.Reset();
        UpdateClock();
        GameData.Reset();
        UpdateScore();
        UpdateDifficulty();

        AudioManager.MusicVolume = 0.7f; //TODO refactor this to be part of the PlayMusic method.
        AudioManager.PlayMusic("GamePlayMusic", true, false);

        Messenger.On(BulletController.MESSAGE_BALLOON_POPPED, IncrementScore);
        Messenger.On(CannonController.MESSAGE_SHOTS_FIRED, IncrementShots);
        Messenger.On(InputManager.MESSAGE_PLAYER_COUNT_CHANGED, PlayerCountChanged);
    }

    public override void EndFn() {
        GamePlayUIGroup.SetVisible(false);
        SetSpawnRate(0);

        Messenger.Un(BulletController.MESSAGE_BALLOON_POPPED, IncrementScore);
        Messenger.Un(CannonController.MESSAGE_SHOTS_FIRED, IncrementShots);
        Messenger.Un(InputManager.MESSAGE_PLAYER_COUNT_CHANGED, PlayerCountChanged);
    }

    void PlayerCountChanged(object[] args = null) {
        int playersCount = (int)args[0];

        if (playersCount == 0) { // Everyone left the game.
            SwitchState(ClassNameOf<TitleScreenState>());
            return;
        }

        UpdateDifficulty();
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

    void UpdateDifficulty(object[] args = null) {
        // We'll determine the difficulty by the number of balloons being spawned. 
        // We definitely want this to scale with the number of players, otherwise the
        // game could potentially be too easy.

        float spawnRate = 1f * InputManager.PlayersCount;

        SetSpawnRate(spawnRate);
    }

    void SetSpawnRate(float spawnsPerSecond) {
        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { spawnsPerSecond });
    }
}
