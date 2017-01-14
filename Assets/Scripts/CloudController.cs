using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {
    public Vector3 Velocity;
    Rect BoundsRect;


    void Start() {
        var rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = Velocity * Random.Range(0.8f, 1.2f);

        // Need to make sure the BoundsRect is big enough that the cloud can completely leave the screen, 
        // and be repositioned to the right of the screen without being visible yet.
        var spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        var spriteWidth = spriteRenderer.GetWorldSpaceSize().x;
        var screenRect = Camera.main.VisibleWorldRect();
        BoundsRect = new Rect(screenRect.xMin - spriteWidth, screenRect.y, screenRect.width + 2 * spriteWidth, screenRect.height);
    }

    void Update() {
        if (transform.position.x < BoundsRect.x) { // Out of left Bounds
            transform.SetX(BoundsRect.x + BoundsRect.width); // Move it to the right bounds so it can loop over again.
        }
    }
}
