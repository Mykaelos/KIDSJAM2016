using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneController : MonoBehaviour {
    public GameData GameData = new GameData();

    InputManager InputManager;

    void Awake() {
        AudioManager.AddTracks(new Dictionary<string, string> {
            { "MenuMusic", "Ponies_and_Balloons" },
            { "GamePlayMusic", "Racing_The_Clock" }
        });

        InputManager = GetComponent<InputManager>();

    }

    void Start() {
        StateMachine.Initialize(gameObject, new List<StateMachineState> {
            new CreditsScreenState(InputManager),
            new TitleScreenState(InputManager),
            new GamePlayState(InputManager, GameData),
            new EndScreenState(InputManager, GameData)
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
