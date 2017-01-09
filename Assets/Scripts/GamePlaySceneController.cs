using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneController : MonoBehaviour {
    public static int BalloonsPopped = 0;
    public static int ShotsFired = 0;


    void Awake() {
        AudioManager.AddTracks(new Dictionary<string, string> {
            { "MenuMusic", "Ponies_and_Balloons" },
            { "GamePlayMusic", "Racing_The_Clock" }
        });
    }

    void Start() {
        StateMachine.Initialize(gameObject, new List<StateMachineState> {
            new TitleScreenState(),
            new GamePlayState(),
            new EndScreenState()
        });
    }

}
