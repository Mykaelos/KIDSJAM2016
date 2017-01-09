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
    int Score = 0;


    public GamePlayState() {
        Name = GetType().Name;
        GamePlayUIGroup = GameObject.Find("Canvas/GamePlayUI").GetComponent<CanvasGroup>();
        ClockText = GameObject.Find("Canvas/GamePlayUI/Clock").GetComponent<Text>();
        ScoreText = GameObject.Find("Canvas/GamePlayUI/Score").GetComponent<Text>();

        GameTimer = new Timer(180);

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

        if (GameTimer.Check()) {
            return "EndScreenState";
        }

        return null;
    }

    void UpdateFn() {
        //update the timer
        //update the balloon's popped score?

        UpdateClock();
    }

    void StartFn() {
        GamePlayUIGroup.SetVisible(true);

        GameTimer.Reset();
        UpdateClock();
        Score = 0;
        UpdateScore();

        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 2f });

        Messenger.On(BalloonController.MESSAGE_BALLOON_POPPED, IncrementScore);
    }

    void EndFn() {
        GamePlayUIGroup.SetVisible(false);
        Messenger.Fire(SpawnController.MESSAGE_SET_BALLOONS_PER_SECOND, new object[] { 0f });

        Messenger.Un(BalloonController.MESSAGE_BALLOON_POPPED, IncrementScore);
    }

    void UpdateClock() {
        var timeSpan = TimeSpan.FromSeconds(GameTimer.DurationUntilNext());
        ClockText.text = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    void IncrementScore(object[] args = null) {
        Score++;
        UpdateScore();
    }

    void UpdateScore() {
        ScoreText.text = Score.ToString("N0") + " Popped";
    }
}
