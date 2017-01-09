using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public delegate void DeadEventHandler();

public class PlayerController : Character {

    [SerializeField]
    private Stat healthStat;

    public float healVal;
    public ParticleSystem healthEffect;
    public GameObject playerSoul;
    public Transform[] groundPoints;
    public LayerMask whatIsGround;
    public float jumpForce;
    public float groundRadius;
    public bool airControl;
    public bool trig = false;

    private Enemy enemy;
    private float attackTimer;
    private float attackCooldown = 2f;
    private bool canThrow = true;

    public bool immortalty;
    private bool immortal = false;
    public float immortalTime;

    private SpriteRenderer spriteRenderer;
    private bool isCreated;
    private Camera cam;
    private Vector3 mousePosition;
    public float alphaLevel = 1;
    public float totalTime = 0;

    public event DeadEventHandler Dead;

    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }     

    public Rigidbody2D MyRigidBody { get; set; }

    public bool Jump { get; set; }

    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if(healthStat.CurrentVal <= 0)
            {
                OnDead();
            }
            
            return healthStat.CurrentVal <= 0;
        }
    }

    public Vector2 startPos;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidBody = GetComponent<Rigidbody2D>();
        healthStat.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            HandleInput();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Orb != null)
                {
                    transform.position = FindObjectOfType<PlayerOrb>().transform.position;
                }
            }
        }
    }

    void FixedUpdate() {

        if (!IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            HandleMovement(horizontal);

            Flip(horizontal);

            HandleLayers();

            attackTimer += Time.deltaTime;
        }

    }

    public void OnDead()
    {
        if(Dead != null)
        {
            Dead();
        }
    }

    void HandleMovement(float horizontal)
    {        
        if (MyRigidBody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        if (!Attack && (OnGround || airControl))
        {
            MyRigidBody.velocity = new Vector2(horizontal * moveSpeed, MyRigidBody.velocity.y);
        }
        if(Jump && MyRigidBody.velocity.y == 0)
        {
            MyRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    public void HandleInput()
    {
        if (trig == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MyAnimator.SetTrigger("attack");
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MyAnimator.SetTrigger("jump");
            }
            if (Input.GetMouseButtonDown(1))
            {
                OrbAttack();
            }
        }
    }

    void Flip(float horizontal)
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        horizontal = mousePosition.x - transform.position.x;
        if ((horizontal > 0 && !facingRight && !Attack) || (horizontal < 0 && facingRight && !Attack))
        {
            ChangeDirection();
        }
    }

    private bool IsGrounded()
    {
        if(MyRigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void SpawnOrb(int value)
    {
        {
            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
                tmp.GetComponent<PlayerOrb>().Initialize(Vector2.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
                tmp.GetComponent<PlayerOrb>().Initialize(Vector2.left);
            }
        }
    }

    private void OrbAttack()
    {
        if (attackTimer >= attackCooldown)
        {
            canThrow = true;
            attackTimer = 0;
        }

        if (canThrow)
        {
            canThrow = false;
            MyAnimator.SetTrigger("throw");
        }
    }

    public void SetActive()
    {
        trig = false;
        moveSpeed = 3;      
    }

    public void SetInactive()
    {
        moveSpeed = 0;
        trig = true;
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(.2f);

            spriteRenderer.enabled = true;

            yield return new WaitForSeconds(.2f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal && immortalty)
        {
            healthStat.CurrentVal -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
            }
        }
        else
        {
            healthStat.CurrentVal -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
            }
        }
    }

    public override void Death()
    {
        StartCoroutine("Checkpoint");
        MyRigidBody.velocity = Vector2.zero;

        if (!isCreated)
        {
            Instantiate(playerSoul, transform.position, Quaternion.identity);
            isCreated = true;
        }

        //if(alphaLevel >= 0)
        //{
        //    totalTime += Time.deltaTime;

        //    if (totalTime >= .09)
        //    {
        //        alphaLevel -= .025f;
        //        totalTime = 0;
        //    }

        //    GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alphaLevel);
        //}
    }

    private IEnumerator NewLevel()
    {
        float fadeTime = GameObject.Find("_BG").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("scene_00");

        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
    }

    private IEnumerator Checkpoint()
    {
        yield return new WaitForSeconds(5);
        transform.position = startPos;
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        StopCoroutine("Checkpoint");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Health")
        {
            if(healthStat.CurrentVal != healthStat.MaxVal)
            {
                healthStat.CurrentVal += healVal;
                Destroy(Instantiate(healthEffect.gameObject, transform.position, Quaternion.identity) as GameObject, healthEffect.startLifetime);
                Destroy(other.gameObject);
            }
        }
    }
}
