using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BalloonController : MonoBehaviour {
    public Vector2 BalloonVelocity;
    public GameObject PopPrefab;

    SpriteRenderer SpriteRenderer;
    Rigidbody2D Rigidbody2D;


    public void Setup(Vector2 velocity, Rect screenRect) {
        BalloonVelocity = velocity;
        ScreenRect = screenRect;
    }

    void Awake() {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer.sortingOrder = Random.Range(1, 32000);
    }

    void Start() {
        Rigidbody2D.velocity = BalloonVelocity * Random.Range(0.5f, 1.5f);
    }

    void Update() {
        CheckForRemoval();
    }

    Rect ScreenRect;
    bool HasMadeItInsideScreen = false;

    void CheckForRemoval() {
        // Check to see if the balloon has made it into the screen yet.
        if (!HasMadeItInsideScreen && ScreenRect.Contains(transform.position)) {
            HasMadeItInsideScreen = true;
        }
        // Check to see if the balloon is now exiting the screen after being inside of it.
        if (HasMadeItInsideScreen && !ScreenRect.Contains(transform.position)) {
            Destroy(gameObject);
        }
    }

    public void Pop() {
        var popPrefab = GameObject.Instantiate<GameObject>(PopPrefab);
        popPrefab.transform.position = transform.position;

        popPrefab.GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.0f);
        //popPrefab.GetComponent<ParticleSystem>().startColor = SpriteRenderer.color;

        Destroy(popPrefab, 1);

        Destroy(gameObject);
    }
}
