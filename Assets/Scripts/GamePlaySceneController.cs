using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneController : MonoBehaviour {
    public GameData GameData = new GameData();


    void Awake() {
        AudioManager.AddTracks(new Dictionary<string, string> {
            { "MenuMusic", "Ponies_and_Balloons" },
            { "GamePlayMusic", "Racing_The_Clock" }
        });
    }

    void Start() {
        StateMachine.Initialize(gameObject, new List<StateMachineState> {
            new TitleScreenState(),
            new GamePlayState(GameData),
            new EndScreenState(GameData)
        });
    }
}

public class GameData {
    public int BalloonsPopped = 0;
    public int ShotsFired = 0;


    public GameData() {
        Reset();
    }

    public void Reset() {
        BalloonsPopped = 0;
        ShotsFired = 0;
    }
}
