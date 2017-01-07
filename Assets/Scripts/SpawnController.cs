using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    public float BalloonsPerSecond = 2;

    public GameObject BalloonPrefab;

    Timer BalloonSpawnTimer;
    Rect ScreenRect;
    Transform SpawnBucket;


    void Awake() {
        SpawnBucket = GameObject.Find("SpawnBucket").transform;
        BalloonSpawnTimer = new Timer();

        // We're finding the screen's edges so that we can figured out a good place to spawn the balloons.
        // Added some padding on the x axes so we can use this rect again for the balloons knowing they're
        // out of bounds. Also moved the baloons up a bit on the y axes so that they don't fly right into 
        // the cannons.
        var screenRect = Camera.main.VisibleWorldRect();
        ScreenRect = new Rect(screenRect.xMin - 2, screenRect.y + 2, screenRect.width + 4, screenRect.height - 2);
    }

    void Update() {
        if (BalloonSpawnTimer.Check(1f / BalloonsPerSecond)) {
            BalloonSpawnTimer.Reset();

            SpawnBalloon();
        }
    }

    void SpawnBalloon() {
        var balloon = GameObject.Instantiate<GameObject>(BalloonPrefab);
        var halfWidth = ScreenRect.width / 2f;

        var location = ScreenRect.RandomPoint();
        float direction = location.x < 0 ? 1 : -1;
        location.x += (halfWidth * -direction);

        balloon.transform.position = location;
        balloon.transform.SetParent(SpawnBucket, true);

        balloon.GetComponent<BalloonController>().Setup(new Vector2(direction, 0), ScreenRect);
    }

    public void RemoveAllBalloons() {
        foreach (Transform child in SpawnBucket) {
            Destroy(child.gameObject);
        }
    }
}
