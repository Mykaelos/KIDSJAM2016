using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour {
    public const string MESSAGE_PLAYER_COUNT_CHANGED = "MESSAGE_PLAYER_COUNT_CHANGED";

    public GameObject CannonPrefab;
    public List<Sprite> CannonBarrelSprites = new List<Sprite>();
    public List<Sprite> CannonBaseSprites = new List<Sprite>();
    public List<Sprite> CannonBallSprites = new List<Sprite>();
    InputManager InputManager;
    List<CannonController> Cannons = new List<CannonController>();

    public int PlayersCount {
        get { return Cannons.Count; }
    }


    void Awake() {
        InputManager = GetComponent<InputManager>();
    }

    void Start() {
        InputManager.LocalMessenger.On(InputManager.PLAYER_JOINED, GenerateCannon);
        InputManager.LocalMessenger.On(InputManager.PLAYER_EXITED, RemoveCannon);
    }

    void GenerateCannon(object[] args) {
        InputData inputData = (InputData)args[0];

        // Create the new cannon at its location
        GameObject cannon = GameObject.Instantiate(CannonPrefab, Vector2.zero, Quaternion.identity);
        var cannonController = cannon.GetComponent<CannonController>();
        cannonController.Setup(inputData, CannonBarrelSprites.RandomElement(), CannonBaseSprites.RandomElement(), CannonBallSprites.RandomElement());
        Cannons.Add(cannonController);

        MoveCannonsToLocations();

        Messenger.Fire(MESSAGE_PLAYER_COUNT_CHANGED, new object[] { PlayersCount });
    }

    void RemoveCannon(object[] args) {
        InputData inputData = (InputData)args[0];

        var tempList = new List<CannonController>(Cannons);
        for (int i = 0; i < tempList.Count; i++) {
            if (tempList[i].GetPlayerSlot() == inputData.PlayerSlot) {
                Destroy(Cannons[i].gameObject);
                Cannons.RemoveAt(i);
                break;
            }
        }

        MoveCannonsToLocations();

        Messenger.Fire(MESSAGE_PLAYER_COUNT_CHANGED, new object[] { Cannons.Count });
    }

    void MoveCannonsToLocations() {
        // Determine new cannon locations based on the number of players in the game
        var screenRect = Camera.main.VisibleWorldRect();
        var playerLocations = new Vector2[PlayersCount];

        float sectionWidth = screenRect.width / (playerLocations.Length + 1f);
        float nextXLocation = screenRect.x + sectionWidth;

        for (int i = 0; i < playerLocations.Length; i++) {
            playerLocations[i] = new Vector2(nextXLocation, screenRect.y);

            nextXLocation += sectionWidth;
        }

        // Move existing cannons to their positions
        for (int i = 0; i < Cannons.Count; i++) {
            Cannons[i].transform.position = playerLocations[i];
        }
    }
}
