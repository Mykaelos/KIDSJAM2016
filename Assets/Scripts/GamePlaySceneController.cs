using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneController : MonoBehaviour {

    CanvasGroup TitleScreenUIGroup;

    void Awake() {
        TitleScreenUIGroup = GameObject.Find("Canvas/TitleScreenUI").GetComponent<CanvasGroup>();
    }


    void Start() {

        StateMachine.Initialize(gameObject, new List<StateMachineState> {
            new StateMachineState("TitleScreen", 
                //maybe have a few random balloons floating around?
                //maybe an ai cannon for fun?
                delegate {
                    /* check for input, move to gameplay*/
                    return null;
                },
                null,
                delegate {
                    /*Show title UI*/
                    TitleScreenUIGroup.SetVisible(false);
                },
                delegate {
                    /*hide title UI*/
                    TitleScreenUIGroup.SetVisible(false);
                }
            ),
            new StateMachineState("GamePlay",
                delegate { return null; /* check timer, move to finished*/},
                null,
                delegate { /*show gameplay ui, start timer*/ },
                delegate { /*hide title UI*/ }
            ),
            new StateMachineState("EndScreen",
                delegate { return null; /* check for input, move to gameplay or title screen*/},
                null,
                delegate { /*show final results, maybe stats too*/ },
                delegate { /*hide UI*/ }
            )
        }, "TitleScreen");


    }

}
