using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneController : MonoBehaviour {
    public GameData GameData = new GameData();

    InputManager InputManager;
    CannonManager CannonManager;


    void Awake() {
        AudioManager.AddTracks(new Dictionary<string, string> {
            { "MenuMusic", "Ponies_and_Balloons" },
            { "GamePlayMusic", "Racing_The_Clock" }
        });

        InputManager = GetComponent<InputManager>();
        CannonManager = GetComponent<CannonManager>();
    }

    void Start() {
        StateMachine.Initialize(gameObject, new List<StateMachineState> {
            new CreditsScreenState(InputManager),
            new InstructionsScreenState(InputManager),
            new GamePlayState(InputManager, CannonManager, GameData),
            new EndScreenState(InputManager, GameData)
        });
    }

    void Update() {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.P)) { // For testing purposes =)
            Messenger.Fire(SpawnController.MESSAGE_SPAWN_EXPLOSIVE_BALLOON);
        }
    }
}

public class GameData {
    public int BalloonsPopped = 0;
    public int ShotsFired = 0;
    public int ExplosiveBalloonsPopped = 0;


    public GameData() {
        Reset();
    }

    public void Reset() {
        BalloonsPopped = 0;
        ShotsFired = 0;
        ExplosiveBalloonsPopped = 0;
    }
}
