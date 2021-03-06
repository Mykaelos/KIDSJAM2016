﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayState : StateMachineState {
    InputManager InputManager;
    CannonManager CannonManager;
    GameData GameData;

    CanvasGroup GamePlayUIGroup;
    Text ClockText;
    Text ScoreText;

    Timer GameTimer;
    Timer DifficultyIncreaseTimer;
    float MinDifficulty = 1;
    float MaxDifficulty = 3;
    float CurrentDifficultyBase = 1;
    float DifficultyStep = 0.5f;


    public GamePlayState(InputManager inputManager, CannonManager cannonManager, GameData gameData) {
        InputManager = inputManager;
        CannonManager = cannonManager;
        GameData = gameData;

        GamePlayUIGroup = GameObject.Find("Canvas/GamePlayUI").GetComponent<CanvasGroup>();
        ClockText = GameObject.Find("Canvas/GamePlayUI/Clock").GetComponent<Text>();
        ScoreText = GameObject.Find("Canvas/GamePlayUI/Score").GetComponent<Text>();

        GameTimer = new Timer(146);
        DifficultyIncreaseTimer = new Timer(GameTimer.Delay / ((MaxDifficulty - MinDifficulty + DifficultyStep) / DifficultyStep));
    }

    public override string CheckFn() {
        if (GameTimer.Check()) {
            return ClassNameOf<EndScreenState>();
        }

        return null;
    }

    public override void UpdateFn() {
        UpdateClock();
        UpdateDifficulty();

        if (Application.isEditor) { // For testing in the editor.
            if (Input.GetKeyDown(KeyCode.S)) { // Hit S to quickly end the level.
                GameTimer.SetTimeRemaining(0);
            }
        }
    }

    public override void StartFn() {
        GamePlayUIGroup.SetVisible(true);

        //Reset everything
        GameTimer.Reset();
        UpdateClock();
        GameData.Reset();
        UpdateScore();
        CurrentDifficultyBase = MinDifficulty;
        SetDifficulty();

        AudioManager.PlayMusic("GamePlayMusic", true, false);

        Messenger.On(BalloonController.MESSAGE_BALLOON_POPPED, IncrementScore);
        Messenger.On(ExplosiveBalloonController.MESSAGE_EXPLOSIVE_BALLOON_POPPED, IncrementExplosiveBalloonsPopped);
        Messenger.On(CannonController.MESSAGE_SHOTS_FIRED, IncrementShots);
        Messenger.On(CannonManager.MESSAGE_PLAYER_COUNT_CHANGED, PlayerCountChanged);
    }

    public override void EndFn() {
        GamePlayUIGroup.SetVisible(false);
        SetSpawnRate(0);

        Messenger.Un(BalloonController.MESSAGE_BALLOON_POPPED, IncrementScore);
        Messenger.Un(ExplosiveBalloonController.MESSAGE_EXPLOSIVE_BALLOON_POPPED, IncrementExplosiveBalloonsPopped);
        Messenger.Un(CannonController.MESSAGE_SHOTS_FIRED, IncrementShots);
        Messenger.Un(CannonManager.MESSAGE_PLAYER_COUNT_CHANGED, PlayerCountChanged);
    }

    void PlayerCountChanged(object[] args = null) {
        int playersCount = (int)args[0];

        if (playersCount == 0) { // Everyone left the game.
            SwitchState(ClassNameOf<InstructionsScreenState>());
            return;
        }

        SetDifficulty();
    }

    void UpdateClock() {
        var timeSpan = TimeSpan.FromSeconds(GameTimer.DurationUntilNext());
        ClockText.text = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    void UpdateDifficulty() {
        if (DifficultyIncreaseTimer.Check()) {
            DifficultyIncreaseTimer.Reset();

            CurrentDifficultyBase += DifficultyStep;
            Debug.Log("CurrentDifficultyBase :" + CurrentDifficultyBase);
            SetDifficulty();

            Messenger.Fire(SpawnController.MESSAGE_SPAWN_EXPLOSIVE_BALLOON);
        }
    }

    void IncrementScore(object[] args = null) {
        GameData.BalloonsPopped++;
        UpdateScore();
    }

    void IncrementExplosiveBalloonsPopped(object[] args = null) {
        GameData.ExplosiveBalloonsPopped++;
    }

    void IncrementShots(object[] args = null) {
        GameData.ShotsFired++;
    }

    void UpdateScore() {
        ScoreText.text = GameData.BalloonsPopped.ToString("N0") + " Popped";
    }

    void SetDifficulty(object[] args = null) {
        // We'll determine the difficulty by the number of balloons being spawned. 
        // We definitely want this to scale with the number of players, otherwise the
        // game could potentially be too easy.

        float spawnRate = CurrentDifficultyBase * Math.Max(CannonManager.PlayersCount, 1);
        Debug.Log("spawnRate :" + spawnRate);

        SetSpawnRate(spawnRate);
    }

    void SetSpawnRate(float spawnsPerSecond) {
        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { spawnsPerSecond });
    }
}
