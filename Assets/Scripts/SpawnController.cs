using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    public const string MESSAGE_SPAWN_EXPLOSIVE_BALLOON = "MESSAGE_SPAWN_EXPLOSIVE_BALLOON";
    public const string MESSAGE_SET_BALLOONS_PER_SECOND = "MESSAGE_SET_BALLOONS_PER_SECOND";
    public const string MESSAGE_REMOVE_BALLOONS = "MESSAGE_REMOVE_BALLOONS";
    public const string MESSAGE_REMOVE_BALLOONS_FROM_POINT = "MESSAGE_REMOVE_BALLOONS_FROM_POINT";

    public BalloonData[] BalloonData = new BalloonData[4];

    public BalloonData ExplosiveBalloonData;

    public float BalloonsPerSecond = 2;

    public GameObject BalloonPrefab;

    Timer BalloonSpawnTimer;
    Rect ScreenRect;
    Rect BalloonOutOfBoundsRect;
    Rect SpawnRect;
    Transform SpawnBucket;


    void Awake() {
        SpawnBucket = GameObject.Find("SpawnBucket").transform;
        BalloonSpawnTimer = new Timer();

        // We're finding the screen's edges so that we can figure out a good place to spawn the balloons, and when to remove 
        // the balloons that don't get popped.
        ScreenRect = Camera.main.VisibleWorldRect();
        // Added some padding on the x axes so that the balloons are removed when completely of the screen.
        BalloonOutOfBoundsRect = new Rect(ScreenRect.xMin - 2, ScreenRect.y - 2, ScreenRect.width + 4, ScreenRect.height + 4);

        float spawnWidth = 2;
        float spawnStart = -spawnWidth / 2f;
        // Adjusted the starting y axis so that they don't fly right into the cannons.
        SpawnRect = new Rect(spawnStart, ScreenRect.y + 2, spawnWidth, ScreenRect.height - 3);

        Messenger.On(MESSAGE_SPAWN_EXPLOSIVE_BALLOON, SpawnExplosiveBalloon);
        Messenger.On(MESSAGE_SET_BALLOONS_PER_SECOND, SetSpawnRate);
        Messenger.On(MESSAGE_REMOVE_BALLOONS, RemoveAllBalloons);
        Messenger.On(MESSAGE_REMOVE_BALLOONS_FROM_POINT, RemoveAllBalloonsFromPoint);
    }

    void OnDestory() {
        Messenger.Un(MESSAGE_SPAWN_EXPLOSIVE_BALLOON, SpawnExplosiveBalloon);
        Messenger.Un(MESSAGE_SET_BALLOONS_PER_SECOND, SetSpawnRate);
        Messenger.Un(MESSAGE_REMOVE_BALLOONS, RemoveAllBalloons);
        Messenger.Un(MESSAGE_REMOVE_BALLOONS_FROM_POINT, RemoveAllBalloonsFromPoint);
    }

    void Update() {
        if (BalloonsPerSecond > 0 && BalloonSpawnTimer.Check(1f / (BalloonsPerSecond))) {
            BalloonSpawnTimer.Reset();

            if (Random.value < 0.01f) {
                SpawnExplosiveBalloon();
            }
            else {
                SpawnBalloon();
            }

        }
    }

    void SetSpawnRate(object[] args = null) {
        if (args != null && args.Length > 0) {
            BalloonsPerSecond = (float)args[0];
        }
    }

    GameObject SpawnBalloon() {
        // I want to equally distribute the balloons to one side or the other. Essentially, I'm splitting the spawnRect 
        // in half and sliding it to the edge of the screen. This way I only have to deal with one rect and not having
        // choose which one.
        var halfScreenWidth = ScreenRect.width / 2f + 1f; // Adding one for some padding for the balloon, so it doesn't spawn on screen.

        var location = SpawnRect.RandomPoint();
        float direction = location.x < 0 ? 1 : -1;
        location.x += (halfScreenWidth * -direction);

        var balloon = GameObject.Instantiate<GameObject>(BalloonPrefab);
        balloon.transform.position = location;
        balloon.transform.SetParent(SpawnBucket, true);

        balloon.GetComponent<BalloonController>().Setup(new Vector2(direction, 0), BalloonOutOfBoundsRect, BalloonData.RandomElement());

        return balloon;
    }

    void SpawnExplosiveBalloon(object[] args = null) {
        var balloon = SpawnBalloon();
        balloon.AddComponent<ExplosiveBalloonController>();
        balloon.GetComponent<BalloonController>().SetBalloonData(ExplosiveBalloonData);
    }

    public void RemoveAllBalloons(object[] args = null) {
        foreach (Transform child in SpawnBucket) {
            var balloonController = child.GetComponent<BalloonController>();
            if (balloonController != null) {
                RemoveBalloonAfterDelay(balloonController, Random.Range(0f, 0.2f));
            }
        }
    }

    public void RemoveAllBalloonsFromPoint(object[] args) {
        Vector3 startingPoint = (Vector3)args[0];

        foreach (Transform child in SpawnBucket) {
            var balloonController = child.GetComponent<BalloonController>();
            if (balloonController != null) {
                var distance = Vector3.Distance(startingPoint, child.position);
                var duration = distance * (0.2f / 10f); // ~0.2 seconds per 10 distance units

                RemoveBalloonAfterDelay(balloonController, duration);
            }
        }
    }

    public void RemoveBalloonAfterDelay(BalloonController balloon, float delay) {
        WaitUntil.Seconds(delay, delegate () {
            if (balloon != null) {
                balloon.Pop();
            }
        });
    }
}
