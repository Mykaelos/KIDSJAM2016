using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneController : MonoBehaviour {

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
