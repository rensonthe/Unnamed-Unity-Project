using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Orb : MonoBehaviour {

    public float speed;
    private float orbDuration = 2f;

    private Rigidbody2D myRigidBody;

    private Vector2 direction;

    // Use this for initialization
    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        orbDuration -= Time.deltaTime;
        if (orbDuration <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        myRigidBody.velocity = direction * speed;
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
        }
    }
}
