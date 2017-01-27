using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float Speed = 10;
    public float Duration = 2;
    Rigidbody2D Rigidbody;


    void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        var sprite = transform.Find("Sprite");
        sprite.Rotate(Vector3.forward, Random.Range(0f, 360f));
    }

    public void Start() {
        Rigidbody.AddForce(transform.up * Speed, ForceMode2D.Impulse);
    }

    public void Update() {
        if ((Duration -= Time.deltaTime) < 0) {
            OnDestroy();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        BalloonController balloonController = collision.gameObject.GetComponent<BalloonController>();
        if (balloonController != null) {
            balloonController.Pop();

            OnDestroy();
        }
    }

    public virtual void OnDestroy() {
        Destroy(this.gameObject);
    }
}
