using UnityEngine;
using System.Collections;

public class zeroEnemyController : MonoBehaviour
{

    private Animator anime;
    private Transform groundTransform;
    private Transform zeroEnemyTransform;
    public Transform firePoint;
    public GameObject plasma1;
    private LayerMask whatIsGround;
    private Renderer spriteRender;
    private playerController playerControl;
    private plasmaController playerPlasma;
    private stateMachine machine;

    public int life;
    public float timeDash;
    public float speedPlayerX;
    public float accelerateY;
    public float accelerateX;
    public float sideFace;
    public float animSpeed;
    private float AnimInstantNormTime;
    public float gravityStore;
    public float zeroEnemyPositionY;
    public float ladderTop;
    public float ladderCenter;
    public float timeShot;

    public int damageBody;

    public float testNormTime;
    public float testSizeSprite;
    public bool testCenter;

    public Vector2 velocityPlayer;
    private Vector2 boxCastSize;
    private Vector2 boxCastDirection;
    public Vector2 eulerAnglesRight;
    public Vector2 eulerAnglesLeft;

    private Vector3 shotPosition;
    public Vector3 zeroEnemyPosition;

    public bool onLadder;
    public bool animeGrounded;
    public bool airMove;
    public bool runShot;
    public bool dash;
    public bool dashed;
    public bool jump;
    public bool run;
    public bool climbGrab;
    public bool climbLadder;
    public bool climbDown;
    public bool climbStand;
    public bool climbUp;
    public bool grounded;
    public bool down;
    public bool down2;
    public bool shot;
    public bool stand;
    public bool dashShot;
    public bool standShot;

    // Use this for initialization
    void Start()
    {
        life = 3;
        damageBody = 1;
        machine = FindObjectOfType<stateMachine>();
        playerControl = FindObjectOfType<playerController>();
        playerPlasma = FindObjectOfType<plasmaController>();
        anime = GetComponent<Animator>();
        zeroEnemyTransform = GetComponent<Transform>();
        whatIsGround = LayerMask.GetMask("Ground", "Wall", "Platform");
        groundTransform = transform.Find("GroundCheck");
        spriteRender = GetComponent<Renderer>();

        boxCastSize = new Vector2(0.22f, 0.0025f);
        boxCastDirection = new Vector2(0.0f, 0.0f);
        velocityPlayer = new Vector2(0.0f, 0.0f);
        eulerAnglesRight = new Vector2(0.0f, 0.0f);
        eulerAnglesLeft = new Vector2(0.0f, 180.0f);
        zeroEnemyPosition = zeroEnemyTransform.position;

        speedPlayerX = 1.5f;
        accelerateX = 0.0f;
        accelerateY = 0.0f;
        sideFace = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(machine.states == gameState.PAUSED))
        {
            zeroEnemyPosition = zeroEnemyTransform.position;
            grounded = !climbDown && Physics2D.BoxCast(groundTransform.position, boxCastSize, 0f, boxCastDirection, 0f, whatIsGround);

            testNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            testSizeSprite = transform.position.y + spriteRender.bounds.size.y;

            stand = true;
            shot = true;
            side();

            zeroCshootingStanding();

            checkLife();
            //animações
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!(machine.states == gameState.PAUSED))
        {
            if (other.gameObject.layer == 8)
            {
                playerControl.sideFaceEnemy = sideFace;
            }
        }
    }

    void checkLife()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void damagedPlayer()
    {
        playerControl.life = playerControl.life - 1;
    }

    void damaged()
    {
        life = life - playerControl.attack;
    }

    void side()
    {
        if (playerControl.playerPosition.x > zeroEnemyPosition.x)
        {
            transform.transform.eulerAngles = eulerAnglesRight;
            sideFace = 1.0f;
        }
        else if (playerControl.playerPosition.x < zeroEnemyPosition.x)
        {
            transform.transform.eulerAngles = eulerAnglesLeft;
            sideFace = -1.0f;
        }
    }

    void zeroCshootingStanding()
    {
        if (stand && shot && standShot == false)
        {
            anime.Play("zeroCShootStand", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Instantiate(plasma1, firePoint.position, firePoint.rotation);
            AnimInstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            standShot = true;
        }

        else if (anime.GetCurrentAnimatorStateInfo(0).IsName("zeroCShootStand") && standShot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - AnimInstantNormTime) >= 4.0f)
        {
            standShot = false;
            anime.Play("zeroCShootStand", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("zeroCShootStand") || !stand)
        {
            standShot = false;
        }
    }


    void paused()
    {
        animSpeed = anime.speed;
        anime.speed = 0;
    }


    void unPaused()
    {
        anime.speed = animSpeed;
    }
}
