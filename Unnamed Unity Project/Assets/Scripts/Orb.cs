using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Orb : MonoBehaviour {

    public float speed;

    private Rigidbody2D myRigidBody;

    private Vector2 direction;

    // Use this for initialization
    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        myRigidBody.velocity = direction * speed;
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
