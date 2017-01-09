using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneController : MonoBehaviour {

    void Start() {
        StateMachine.Initialize(gameObject, new List<StateMachineState> {
            new TitleScreenState(),
            new GamePlayState(),
            new EndScreenState()
        });
    }

}
