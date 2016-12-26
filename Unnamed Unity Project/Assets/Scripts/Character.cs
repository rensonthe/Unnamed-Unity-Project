using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour {

    public GameObject Orb;

    public float moveSpeed = 3;

    public Transform rangedPos;

    public int health;

    public int currentHealth;

    public EdgeCollider2D swordCollider;

    public List<string> damageSources;

    public abstract bool IsDead { get; }

    protected bool facingRight;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    public virtual void Start()
    {
        currentHealth = health;
        facingRight = true;

        MyAnimator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void SpawnOrb(int value)
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
            tmp.GetComponent<Orb>().Initialize(Vector2.left);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
            tmp.GetComponent<Orb>().Initialize(Vector2.right);
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
