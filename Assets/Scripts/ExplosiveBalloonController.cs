using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBalloonController : MonoBehaviour {
    public const string MESSAGE_EXPLOSIVE_BALLOON_POPPED = "MESSAGE_EXPLOSIVE_BALLOON_POPPED";


    void Awake() {
        var balloonController = GetComponent<BalloonController>();
        balloonController.LocalMessenger.On(BalloonController.MESSAGE_POP, delegate(object[] args) {
            Messenger.Fire(SpawnController.MESSAGE_REMOVE_BALLOONS_FROM_POINT, new object[] { transform.position });
            Messenger.Fire(MESSAGE_EXPLOSIVE_BALLOON_POPPED);
        });
    }
}
