using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BalloonController : MonoBehaviour {
    public Vector2 BalloonVelocity;
    public GameObject PopPrefab;
    BalloonData BalloonData;

    SpriteRenderer SpriteRenderer;
    Rigidbody2D Rigidbody2D;

    Rect BalloonOutOfBoundsRect;
    bool HasMadeItInsideScreen = false;


    public void Setup(Vector2 velocity, Rect balloonOutOfBoundsRect, BalloonData balloonData) {
        BalloonVelocity = velocity;
        BalloonOutOfBoundsRect = balloonOutOfBoundsRect;
        BalloonData = balloonData;
    }

    void Awake() {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer.sortingOrder = Random.Range(1, 32000);
    }

    void Start() {
        Rigidbody2D.velocity = BalloonVelocity * Random.Range(0.5f, 1.5f);
        SpriteRenderer.sprite = BalloonData.Sprite;
        transform.localScale *= Random.Range(0.7f, 1.4f);
    }

    void Update() {
        CheckForRemoval();
    }

    void CheckForRemoval() {
        // Check to see if the balloon has made it into the screen yet.
        if (!HasMadeItInsideScreen && BalloonOutOfBoundsRect.Contains(transform.position)) {
            HasMadeItInsideScreen = true;
        }
        // Check to see if the balloon is now exiting the screen after being inside of it.
        if (HasMadeItInsideScreen && !BalloonOutOfBoundsRect.Contains(transform.position)) {
            Destroy(gameObject);
        }
    }

    public void Pop() {
        var popPrefab = GameObject.Instantiate<GameObject>(PopPrefab);
        popPrefab.transform.position = transform.position;

        popPrefab.GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.0f);
        var particleSystemMain = popPrefab.GetComponent<ParticleSystem>().main;
        particleSystemMain.startColor = BalloonData.PopColor;

        Destroy(popPrefab, 1);
        Destroy(gameObject);
    }
}

[System.Serializable]
public class BalloonData {
    public Sprite Sprite;
    public Color PopColor;
}
