using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public stateMachine machine;
    public GameObject Plasma;
    public Sprite[] playerLife;
    public Image imgLife;
    public stateMachine statesMachine;
    private Animator anime;
    private BoxCollider2D boxPlayer;
    private ParticleSystem particleCharge;
    private ParticleSystem.MainModule particleChargeMain;
    private ParticleSystem.EmissionModule emissionModule;
    private Rigidbody2D playerRigidbody;
    private Transform groundTransform;
    private Transform playerTransform;
    private Transform firePoint;
    private Transform topCheckTransform;
    private Transform topCheckTransform2;
    private Transform bottomCheckTransform;
    private Transform backCollisionTransform;
    private Transform frontCollisionTransform;
    private LayerMask whatIsGround;
    private LayerMask layerCheckObstacule;
    private LayerMask layerTopCheck;
    private Renderer spriteRender;

    public int attack;
    public int life;

    public float boxOffsetY;
    public float boxSizeY;
    public float boxOffsetYPer2;
    public float boxSizeYPer2;
    public float charge;
    public float timeSliceJump;
    public float timeDash;
    public float timeBackDash;
    public float timeKnowBack;
    public float timeImune;
    public float speedPlayerX;
    public float accelerateY;
    public float accelerateX;
    public float sideFace;
    public float sideFaceEnemy;
    public float sliceFace;
    private float timeDodge;
    public float animSpeed;
    private float animIstantNormTime;
    public float gravityStore;
    public float playerPositionY;
    public float playerPositionX;
    public float ladderTop;
    public float ladderCenter;
    public float deltaX;
    private float gravityPause;

    public Vector2 climbCenterPosition;
    public Vector2 eulerTransSlice;
    public Vector2 forceJump;
    public Vector2 frontCollisionBoxSize;
    public Vector2 backCollisionBoxSize;
    public Vector2 velocityPlayer;
    private Vector2 groundedBoxSize;
    private Vector2 groundedBoxDirection;
    public Vector2 eulerAnglesRight;
    public Vector2 eulerAnglesLeft;

    private Vector3 shotPosition;
    public Vector3 playerPosition;

    public bool isDown;
    public bool damageEffect;
    public bool enemyDamage;
    public bool sliceWall;
    public bool isImune;
    public bool isKnowBack;
    public bool jumpSliceWall;
    public bool isClimbCenter;
    public bool obstacule;
    public bool topCheckCollision;
    public bool topCheckCollision2;
    public bool frontCheckCollision;
    public bool bottomCheckCollision;
    public bool backObstacle;
    public bool frontObstacle;
    public bool onLadder;
    public bool animeGrounded;
    public bool airMove;
    public bool runShot;
    public bool dash;
    public bool dashed;
    public bool backDash;
    public bool backDashed;
    public bool dodge;
    public bool jump;
    public bool jumpShot;
    public bool run;
    public bool walkDown;
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
    public bool backDashShot;
    public bool standShot;
    public bool fallingShot;
    public bool fallingShot2;
    private bool climbShot;
    private bool sliceShot;
    private bool sliceShot2;
    private bool sliceWall2;
    public bool imuneDodge;
    private bool jumpLadder;
    private bool touchCapsule;
    public bool teleportCapsule;


    // Use this for initialization
    void Start()
    {
        life = 16;
        boxOffsetY = 0.28f;
        boxSizeY = 0.5f;
        boxOffsetYPer2 = 0.14f;
        boxSizeYPer2 = 0.25f;

        anime = GetComponent<Animator>();
        boxPlayer = GetComponent<BoxCollider2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        spriteRender = GetComponent<Renderer>();
        particleCharge = gameObject.GetComponentInChildren<ParticleSystem>();
        particleChargeMain = particleCharge.main;
        emissionModule = particleCharge.emission;
        emissionModule.enabled = false;
        machine = FindObjectOfType<stateMachine>();

        groundTransform = transform.Find("GroundCheck");
        whatIsGround = LayerMask.GetMask("Ground", "Wall", "Platform");

        firePoint = transform.Find("FirePoint");
        layerTopCheck = LayerMask.GetMask("Ground", "Wall", "Door");
        layerCheckObstacule = LayerMask.GetMask("Ground", "Wall", "Door");
        topCheckTransform = transform.Find("TopCheckCollisiion");
        topCheckTransform2 = transform.Find("TopCheckCollisiion2");
        bottomCheckTransform = transform.Find("BottonCheckCollisiion");
        backCollisionTransform = transform.Find("BackCollision");
        frontCollisionTransform = transform.Find("FrontCollision");

        backCollisionBoxSize = new Vector2(0.01f, 0.01f);
        frontCollisionBoxSize = new Vector2(0.01f, 0.01f);
        groundedBoxDirection = new Vector2(0.0f, 0.0f);
        groundedBoxSize = new Vector2(0.22f, 0.0025f);
        velocityPlayer = new Vector2(0.0f, 0.0f);
        eulerAnglesRight = new Vector2(0.0f, 0.0f);
        eulerAnglesLeft = new Vector2(0.0f, 180.0f);

        playerPosition = playerTransform.position;

        topCheckCollision = false;
        climbStand = true;
        imgLife.sprite = playerLife[life];

        speedPlayerX = 1.5f;
        accelerateX = 0.0f;
        accelerateY = 0.0f;
        sideFace = 1.0f;
        gravityStore = playerRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            //When a key is pressed down it see if it was the escape key if it was it will execute the code
            Application.Quit(); // Quits the game
        }

        if (!(machine.states == gameState.PAUSED) && !teleportCapsule)
        {
            grounded = !climbDown && Physics2D.BoxCast(groundTransform.position, groundedBoxSize, 0f, groundedBoxDirection, 0f, whatIsGround);
            backObstacle = Physics2D.BoxCast(backCollisionTransform.position, backCollisionBoxSize, 0f, groundedBoxDirection, 0f, layerCheckObstacule);
            topCheckCollision = !isDown && Physics2D.CircleCast(topCheckTransform.position, 0.002f, groundedBoxDirection, 0f, layerTopCheck); //OverlapCircle(  layerTopCheck);
            frontCheckCollision = Physics2D.BoxCast(frontCollisionTransform.position, frontCollisionBoxSize, 0f, groundedBoxDirection, 0f, layerCheckObstacule);
            frontObstacle = topCheckCollision || Physics2D.BoxCast(frontCollisionTransform.position, frontCollisionBoxSize, 0f, groundedBoxDirection, 0f, layerCheckObstacule);
            bottomCheckCollision = Physics2D.CircleCast(bottomCheckTransform.position, 0.002f, groundedBoxDirection, 0f, layerTopCheck); //OverlapCircle(  layerTopCheck);

            shot = Input.GetButtonDown("Shoot") || !Input.GetButton("Shoot") && charge > 0.3;

            imunized();

            if (!damageEffect && !anime.GetCurrentAnimatorStateInfo(0).IsName("playerDamaged3"))
            {
                if (Input.GetButtonDown("Jump") && climbLadder)
                {
                    jumpLadder = true;
                }
                else if (jumpLadder && grounded)
                {
                    jumpLadder = false;
                }

                if (sliceWall || sliceWall2)
                {
                    sliceFace = -1.0f;
                    if (sideFace > 0.0f)
                    {
                        eulerTransSlice = new Vector2(0, 180);
                    }
                    else
                    {
                        eulerTransSlice = new Vector2(0, 0);
                    }
                }
                else
                {
                    eulerTransSlice = transform.eulerAngles;
                    sliceFace = 1.0f;
                }

                if (grounded)
                {
                    dodging();              
                }
                else
                {
                    dodge = false;
                    imuneDodge = false;
                }

                if (!dodge)
                {
                    shootingSlicing1();
                    shootingSlicing2();
                    shootingJumping();
                    slicingWall();
                    chargeShot();
                    standing();
                    shootingStanding();
                    shootingRuning();
                    shootingDashing();
                    shootingBackDashing();
                    collisionObstacule();

                    if (grounded)
                    {
                        sliceWall = false;
                        sliceWall2 = false;
                        sliceShot = false;
                        sliceShot2 = false;
                        fallingShot = false;
                        fallingShot2 = false;
                        airMove = false;
                        down = false;
                        down2 = false;
                        if (!dash && !jump)
                        {
                            timeDash = 0.0f;
                            dashed = false;
                        }
                        else if (dash)
                        {
                            run = false;
                        }

                        if (!backDash && !jump)
                        {
                            timeBackDash = 0.0f;
                            backDashed = false;
                        }
                        else if (backDash)
                        {
                            run = false;
                        }

                        if (!dash && !backDash && !climbLadder)
                        {
                            if (!obstacule)
                            {
                                running();
                            }
                            else
                            {
                                run = false;
                            }
                        }
                    }
                    else if (!grounded)
                    {
                        timeDash = 0.0f;
                        dash = false;
                        timeBackDash = 0.0f;
                        backDash = false;
                        run = false;

                        if (!sliceWall && !sliceWall2)
                        {
                            shootingFalling1();
                            shootingFalling2();
                        }

                        if (!climbLadder && !obstacule && !jumpSliceWall)
                        {
                            airMoving();
                        }
                        else
                        {
                            airMove = false;
                        }
                        if (!climbLadder && !climbDown && !climbUp)
                        {
                            falling();
                        }
                    }

                    climbingLadder();

                    if (climbLadder)
                    {
                        shootClimber();
                        dashed = false;
                        backDashed = false;
                        if (!climbDown && !climbUp && isClimbCenter)
                        {
                            if (!(climbLadder && dashed) && !(climbLadder && backDashed))
                            {
                                jumping();
                            }
                        }
                    }
                    else if (!climbLadder)
                    {
                        climbShot = false;

                        if (!climbDown && !climbUp)
                        {
                            if (!(climbLadder && dashed) && !(climbLadder && backDashed))
                            {
                                jumping();
                            }
                        }
                    }
                    if (climbGrab || climbDown || climbUp)
                    {
                        airMove = false;
                        jump = false;
                        down = false;
                        down2 = false;
                        climbGrab = false;
                    }
                    if (jump && climbLadder)
                    {
                        climbStand = true;
                        climbLadder = false;
                        isClimbCenter = false;
                        climbUp = false;
                        climbDown = false;
                        climbStand = false;
                        playerRigidbody.gravityScale = gravityStore;
                    }
                    else if (!climbLadder && !climbDown && !climbUp)
                    {
                        if (!frontObstacle)
                        {
                            if (!backDash && !anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootBackDash") && !anime.GetCurrentAnimatorStateInfo(0).IsName("playerBackDash"))
                            {
                                dashing();
                            }
                        }
                        else
                        {
                            dash = false;
                            dashed = false;
                        }

                        if (!backObstacle)
                        {
                            if (!dash && !anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootDash") && !anime.GetCurrentAnimatorStateInfo(0).IsName("playerDash"))
                            {
                                backDashing();
                            }
                        }
                        else
                        {
                            backDash = false;
                            backDashed = false;
                        }
                    }
                    else
                    {
                        dash = false;
                        dashed = false;
                        backDash = false;
                        backDashed = false;
                    }

                    if (airMove)
                    {
                        climbStand = false;
                    }
                }
            }
            else
            {
                playerRigidbody.isKinematic = false;
                accelerateY = 0;
                checklifePlayer();
                sliceWall2 = false;
                fallingShot = false;
                jumpShot = false;
                run = false;
                standShot = false;
                airMove = false;
                runShot = false;
                dashShot = false;
                backDashShot = false;
                down = false;
                down2 = false;
                jump = false;
                dash = false;
                dashed = false;
                backDash = false;
                backDashed = false;
                stand = false;
                sliceWall = false;
                climbDown = false;
                climbShot = false;
                climbGrab = false;
                climbUp = false;
                climbStand = false;
                climbLadder = false;
                climbStand = false;
                isClimbCenter = false;
                charge = 0.0f;
                emissionModule.enabled = false;
                isImune = true;
                playerRigidbody.gravityScale = 1.0f;
                knowBack();

                if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerDamaged"))
                {
                    enemyDamage = true;
                }
                if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerDamaged3") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= anime.GetCurrentAnimatorStateInfo(0).length)
                {
                    damageEffect = false;
                }
            }

            if (!isDown)
            {
                isDown = grounded && (Input.GetAxisRaw("Vertical") >= 1f) && !jump && !climbDown;
            }
            else
            {
                isDown = grounded && (Input.GetAxisRaw("VerticalDown") >= 1f) && !jump && !climbDown;
            }

            //animações

            anime.SetFloat("velocityY", playerRigidbody.velocity.y);
            anime.SetBool("sliceWall", sliceWall);
            anime.SetBool("sliceWall2", sliceWall2);
            anime.SetBool("damageEffect", damageEffect);
            anime.SetBool("enemyDamage", enemyDamage);
            anime.SetBool("jumpSliceWall", jumpSliceWall);
            anime.SetBool("run", run && !jump);
            anime.SetBool("stand", stand);
            anime.SetBool("dodge", dodge);
            anime.SetBool("runShot", runShot);
            anime.SetBool("standShot", standShot);
            anime.SetBool("jumpShot", jumpShot);
            anime.SetBool("grounded", grounded);
            anime.SetBool("dash", dash && grounded);
            anime.SetBool("backDash", backDash && grounded);
            anime.SetBool("shot", shot);
            anime.SetBool("dashShot", dashShot);
            anime.SetBool("backDashShot", backDashShot);
            anime.SetBool("jump", jump);
            anime.SetBool("down", down);
            anime.SetBool("down2", down2);
            anime.SetBool("isDown", isDown);
            anime.SetBool("ladderUp", climbLadder);
            anime.SetBool("fallingShot", fallingShot);
            anime.SetBool("fallingShot2", fallingShot2);
            anime.SetBool("sliceShot", sliceShot);
            anime.SetBool("sliceShot2", sliceShot2);
            anime.SetBool("shotClimber", climbShot);
            anime.SetBool("ladderStand", climbStand);
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "capsule")
        {
            touchCapsule = true;
        }

        if (!(machine.states == gameState.PAUSED))
        {
            if (!isImune && !imuneDodge)
            {
                if (other.gameObject.layer == 14)
                {
                    if (!dodge)
                    {
                        other.gameObject.GetComponent<MonoBehaviour>().Invoke("damagedPlayer", 0);
                        if (sideFace + sideFaceEnemy != 0.0f)
                        {
                            if (transform.eulerAngles.y < 20.0f)
                            {
                                transform.eulerAngles = eulerAnglesLeft;
                                sideFace *= (-1);
                            }
                            else
                            {
                                transform.eulerAngles = eulerAnglesRight;
                                sideFace *= (-1);
                            }
                        }
                        damageEffect = true;
                    }
                }
            }
        }
    }


    void paused()
    {
        gravityPause = playerRigidbody.gravityScale;
        playerRigidbody.isKinematic = true;
        playerRigidbody.gravityScale = 0f;
        particleCharge.Pause();
        animSpeed = anime.speed;
        anime.speed = 0;
    }


    void unPaused()
    {
        anime.speed = animSpeed;
        particleCharge.Play();
        playerRigidbody.isKinematic = false;
        playerRigidbody.gravityScale = gravityPause;
    }


    void chargeShot()
    {
        if (Input.GetButton("Shoot") && charge < 2.0f)
        {
            charge += Time.deltaTime;
            if (charge > 0.29 && charge < 1.5)
            {
                emissionModule.enabled = true;
                particleChargeMain.startColor = new Color(0f, 1f, 0f, 1f);
            }
            else if (charge > 1.49)
            {
                particleChargeMain.startColor = new Color(1, 0, 1, 1f);
            }
        }
        else if (charge < 0.3)
        {
            charge = 0.0f;
            emissionModule.enabled = false;
        }       
    }


    void imunized()
    {
        if (isImune)
        {
            if (timeImune >= 0.8f)
            {
                timeImune = 0.0f;
                timeKnowBack = 0.0f;
                isImune = false;
                isKnowBack = false;
                enemyDamage = false;
            }
            else
            {
                timeImune += Time.deltaTime;
            }
        }
    }


    void knowBack()
    {
        if (!isKnowBack)
        {
            playerRigidbody.velocity = new Vector2(0.0f, 2.0f);
            timeKnowBack = 0.0f;
            isKnowBack = true;
        }
        else if (timeKnowBack <= 0.1f && isKnowBack)
        {
            playerTransform.position = new Vector2(playerTransform.position.x + 2.0f * Time.deltaTime * -sideFace, playerTransform.position.y);
            timeKnowBack += Time.deltaTime;
        }
    }


    void checklifePlayer()
    {
        imgLife.sprite = playerLife[life];
    }


    void standing()
    {
        if ((Input.anyKeyDown && !shot) || playerRigidbody.velocity.x != 0 || playerRigidbody.velocity.y != 0 || run || jumpSliceWall || jump || down || down2 || dash || backDash || climbLadder || climbDown || climbUp)
        {
            stand = false;
        }
        else
        {
            stand = true;
        }
    }


    void shootingStanding()
    {
        if (stand && shot)
        {
            anime.Play("playerShootStand", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.05f * sideFace, firePoint.position.y + 0.035f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            standShot = true;
        }
        else if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootStand") && standShot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 1.0f)
        {
            standShot = false;
            anime.Play("playerStand", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootStand") || !stand)
        {
            standShot = false;
        }       
    }


    void shootingFalling1()
    {
        if (down && shot && fallingShot == false)
        {
            anime.Play("playerShootFalling", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            fallingShot = true;
        }
        else if (down && anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootFalling") && fallingShot)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.18f)
            {
                anime.Play("playerShootFalling", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.6f)
            {
                anime.Play("playerFalling", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                fallingShot = false;
            }
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootFalling") || !down)
        {
            fallingShot = false;
        }
    }


    void shootingFalling2()
    {
        if (down2 && shot && fallingShot2 == false)
        {
            anime.Play("playerShootFalling2", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            fallingShot2 = true;
        }
        else if (down2 && anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootFalling2") && fallingShot2)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.18f)
            {
                anime.Play("playerShootFalling2", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.6f)
            {
                anime.Play("playerFalling2", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                fallingShot2 = false;
            }
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootFalling2") || !down2)
        {
            fallingShot2 = false;
        }
    }


    void shootingSlicing1()
    {
        if (sliceWall && shot && sliceShot == false)
        {
            anime.Play("playerShootSliceWall1", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.48f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            sliceShot = true;
        }
        else if (sliceWall && anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootSliceWall1") && sliceShot)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.18f)
            {
                anime.Play("playerShootSliceWall1", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.48f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.6f)
            {
                anime.Play("playerSliceWall", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                sliceShot = false;
            }
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootSliceWall1") || !sliceWall || grounded)
        {
            sliceShot = false;
        }
    }


    void shootingSlicing2()
    {
        if (sliceWall2 && shot && sliceShot2 == false)
        {
            anime.Play("playerShootSliceWall2", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.48f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            sliceShot2 = true;
        }
        else if (sliceWall2 && anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootSliceWall2") && sliceShot2)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.18f)
            {
                anime.Play("playerShootSliceWall2", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.48f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.6f)
            {
                anime.Play("playerSliceWall", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                sliceShot2 = false;
            }
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootSliceWall2") || !sliceWall2 || grounded)
        {
            sliceShot2 = false;
        }
    }


    void shootingRuning()
    {
        if (run && shot && runShot == false)
        {
            anime.Play("playerShootRun", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            runShot = true;
        }
        else if (run && anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootRun") && runShot)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.18f)
            {
                anime.Play("playerShootRun", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.6f)
            {
                anime.Play("playerRunning", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                runShot = false;
            }
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootRun") || !run)
        {
            runShot = false;
        }
    }


    void shootingJumping()
    {
        if (jump && shot && jumpShot == false)
        {
            anime.Play("playerShootJump", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            jumpShot = true;
        }
        else if (jump && anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootJump") && jumpShot)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.18f)
            {
                anime.Play("playerShootJump", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.08f * sideFace, firePoint.position.y + 0.032f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.6f)
            {
                anime.Play("playerJumping", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                jumpShot = false;
            }
        }
        else if (!anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootJump") || !jump)
        {
            jumpShot = false;
        }
    }


    void shootingDashing()
    {
        if (dash && shot && dashShot == false)
        {
            anime.Play("playerShootDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x + 0.12f * sideFace, firePoint.position.y - 0.0585f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            dashShot = true;
        }
        else if (dash && dashShot)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.35f)
            {
                anime.Play("playerShootDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x + 0.12f * sideFace, firePoint.position.y - 0.0585f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.50f)
            {
                anime.Play("playerDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                dashShot = false;

            }
        }
        else if (!dash && dashShot)
        {
            anime.Play("playerDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            dashShot = false;
        }
    }


    void shootingBackDashing()
    {
        if (backDash && shot && backDashShot == false)
        {
            anime.Play("playerShootBackDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            shotPosition.Set(firePoint.position.x - 0.3f * sideFace, firePoint.position.y + 0.12f, firePoint.position.z);
            Instantiate(Plasma, shotPosition, firePoint.rotation);
            animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            backDashShot = true;
        }
        else if (backDash && backDashShot)
        {
            if (shot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.08f)
            {
                anime.Play("playerShootBackDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.3f * sideFace, firePoint.position.y + 0.12f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else if ((anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.32f)
            {
                anime.Play("playerBackDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                backDashShot = false;

            }
        }
        else if (!backDash && backDashShot)
        {
            anime.Play("playerBackDash", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            backDashShot = false;
        }
    }


    void dodging()
    {
        if (Input.GetButtonDown("Dodge1") && !dodge)
        {
            dodge = true;

            imuneDodge = true;
            anime.Play("playerDodge", 0, 0);
            timeDodge = 0.0f;
        }
        else if (dodge)
        {
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerDodge") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime <= anime.GetCurrentAnimatorStateInfo(0).length)
            {
                playerTransform.position = new Vector2(playerTransform.position.x + 2.0f * Time.deltaTime * sideFace, playerTransform.position.y);
            }
            else if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerDodge") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime > anime.GetCurrentAnimatorStateInfo(0).length)
            {
                anime.Play("playerStand", 0, 0);
            }
            else if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerStand") && timeDodge <= 0.2f)
            {
                imuneDodge = false;
                timeDodge += Time.deltaTime;
            }
            else if (timeDodge > 0.2f)
            {
                dodge = false;
            }
        }
    }


    void slicingWall()
    {
        if (!obstacule || grounded || !topCheckCollision || (playerRigidbody.velocity.y >= 0))
        {
            sliceWall = false;
            sliceWall2 = false;
        }
        if (!grounded && Input.GetButtonDown("Jump") && !jumpSliceWall && bottomCheckCollision && (Mathf.Abs(Input.GetAxisRaw("Running") + sideFace) > 1.0f))
        {
            playerRigidbody.velocity = new Vector2(0.0f, 3.0f);
            timeSliceJump = 0.0f;
            jumpSliceWall = true;
            jump = true;
        }
        else if (timeSliceJump <= 0.1f && jumpSliceWall)
        {
            playerTransform.position = new Vector2(playerTransform.position.x + 2.0f * Time.deltaTime * -sideFace, playerTransform.position.y);
            timeSliceJump += Time.deltaTime;
            if (timeSliceJump > 0.1f)
            {
                jumpSliceWall = false;
            }
        }
        else if (!grounded && !jumpSliceWall && topCheckCollision && (Mathf.Abs(Input.GetAxisRaw("Running") + sideFace) > 1.0f) && playerRigidbody.velocity.y <= 0)
        {
            if (playerRigidbody.velocity.y < 0)
            {
                sliceWall2 = true;
                sliceWall = false;
            }
            else
            {
                sliceWall = true;
                sliceWall2 = false;
            }
            velocityPlayer.Set(0.0f, playerRigidbody.velocity.y / 1.5f);
            playerRigidbody.velocity = velocityPlayer;
        }
    }


    void dashing()
    {
        if (Input.GetButtonDown("Dash") && grounded)
        {
            dashed = true;
            dash = true;
            accelerateX = 2.0f;
            velocityPlayer.Set(accelerateX * speedPlayerX * sideFace * Time.deltaTime + playerTransform.position.x, playerTransform.position.y);
            playerTransform.localPosition = velocityPlayer;
        }

        if (dash)
        {
            timeDash += Time.deltaTime;
            if (grounded && !Input.GetButton("Dash") || (timeDash >= 0.6f))
            {
                timeDash = 0.0f;
                dash = false;
                accelerateX = 0.0f;
            }
            else if (!grounded && (Input.GetAxisRaw("Running") > 0.3 || Input.GetAxisRaw("Running") < -0.3))
            {
                timeDash = 0.0f;
                dash = false;
                accelerateX = 1.5f;
            }
            else if (!grounded && Input.GetAxisRaw("Running") == 0)
            {
                timeDash = 0.0f;
                dash = false;
                accelerateX = 0.0f;
            }
            velocityPlayer.Set(accelerateX * speedPlayerX * sideFace * Time.deltaTime + playerTransform.position.x, playerTransform.position.y);
            playerTransform.localPosition = velocityPlayer;
        }
    }

    void backDashing()
    {
        if (Input.GetButtonDown("BackDash") && grounded)
        {
            backDashed = true;
            backDash = true;
            accelerateX = -2.0f;
            velocityPlayer.Set(accelerateX * speedPlayerX * sideFace * Time.deltaTime + playerTransform.position.x, playerTransform.position.y);
            playerTransform.localPosition = velocityPlayer;
        }

        if (backDash)
        {
            timeBackDash += Time.deltaTime;
            if (grounded && !Input.GetButton("BackDash") || (timeBackDash >= 0.6f))
            {
                timeBackDash = 0.0f;
                backDash = false;
                accelerateX = 0.0f;
            }
            else if (!grounded && (Input.GetAxisRaw("Running") > 0.3 || Input.GetAxisRaw("Running") < -0.3))
            {
                timeBackDash = 0.0f;
                backDash = false;
                accelerateX = 1.5f;
            }
            else if (!grounded && Input.GetAxisRaw("Running") == 0)
            {
                timeBackDash = 0.0f;
                backDash = false;
                accelerateX = 0.0f;
            }
            velocityPlayer.Set(accelerateX * speedPlayerX * sideFace * Time.deltaTime + playerTransform.position.x, playerTransform.position.y);
            playerTransform.localPosition = velocityPlayer;
        }
    }


    void jumping()
    {
        if (Input.GetButtonDown("Jump") && (grounded || climbLadder))
        {
            jump = true;
            forceJump.Set(0.0f, 200.0f);
            playerRigidbody.AddForce(forceJump);
        }

        if (jump)
        {
            if (Input.GetButtonUp("Jump") && playerRigidbody.velocity.y > 2.0f)
            {
                playerRigidbody.velocity = new Vector2(0.0f, 2.0f);
            }
        }
    }


    void collisionObstacule()
    {
        if (frontObstacle && (Mathf.Abs(Input.GetAxisRaw("Running") + sideFace) > 1.0f))
        {
            obstacule = true;
        }
        else
        {
            obstacule = false;
        }
    }


    void climbingLadder()
    {
        if (climbDown)
        {
            climbStand = false;
            if (playerPositionY - transform.position.y >= 0.6f)
            {
                climbDown = false;
                playerRigidbody.isKinematic = false;
            }
            transform.localPosition = new Vector2(transform.position.x, transform.position.y - 1.0f * Time.deltaTime);
        }
        else if (climbUp)
        {
            climbStand = false;
            if (transform.position.y >= ladderTop)
            {
                climbUp = false;
                climbLadder = false;
                isClimbCenter = false;
                playerRigidbody.gravityScale = gravityStore;
            }
            transform.localPosition = new Vector2(transform.position.x, transform.position.y + 1.0f * Time.deltaTime);
        }
        if (climbLadder && onLadder)
        {
            if (!climbShot)
            {
                side();
            }
            if (!isClimbCenter && Mathf.Abs(ladderCenter - transform.position.x) < 0.01f)
            {
                accelerateX = 0.0f;
                playerTransform.position = new Vector2(climbCenterPosition.x, playerTransform.position.y);
                isClimbCenter = true;
            }
            else if ((climbDown || climbLadder) && Mathf.Abs(ladderCenter - transform.position.x) >= 0.01f && !isClimbCenter)
            {
                if (transform.position.x < ladderCenter)
                {
                    transform.localPosition = new Vector2(transform.position.x + 1.0f * Time.deltaTime, transform.position.y);
                }
                else if (transform.position.x > ladderCenter)
                {
                    transform.localPosition = new Vector2(transform.position.x - 1.0f * Time.deltaTime, transform.position.y);
                }
            }
            else if (!climbDown && !climbUp && isClimbCenter)
            {
                if (onLadder && (Input.GetButton("Up") || Input.GetAxisRaw("Vertical") <= -0.1f) && !grounded && ((transform.position.y + spriteRender.bounds.size.y / 2.0f) >= ladderTop))
                {
                    climbUp = true;
                    climbStand = true;
                    playerRigidbody.velocity = new Vector2(0.0f, 0.0f);
                    forceJump.Set(0.0f, 0.0f);
                    playerRigidbody.AddForce(forceJump);
                    playerPositionY = transform.position.y;
                }
                else if ((onLadder && (Input.GetButton("Down") || Input.GetAxisRaw("Vertical") >= 0.1f) && grounded && transform.position.y < ladderTop))
                {
                    climbStand = true;
                    climbLadder = false;
                    isClimbCenter = false;
                    playerRigidbody.gravityScale = gravityStore;
                }
                else if (climbLadder && !climbUp && !climbShot)
                {
                    playerRigidbody.velocity = new Vector2(0.0f, 0.0f);
                    forceJump.Set(0.0f, 0.0f);
                    playerRigidbody.AddForce(forceJump);
                    if ((Input.GetAxis("Up") >= 0.2f && Input.GetButton("Up")) || (Input.GetAxisRaw("Vertical") == -1.0f))
                    {
                        accelerateY = 1.0f;
                        climbStand = false;
                    }
                    else if ((Input.GetAxis("Down") <= -0.2f && Input.GetButton("Down")) || (Input.GetAxisRaw("Vertical") == 1.0f))
                    {
                        accelerateY = -1.0f;
                        climbStand = false;
                    }
                    else if (Input.GetAxisRaw("Up") == 0 && Input.GetAxisRaw("Down") == 0 && Input.GetAxisRaw("Vertical") == 0)
                    {
                        accelerateY = 0.0f;
                        climbStand = true;
                    }
                    velocityPlayer.Set(accelerateX * speedPlayerX * sideFace * Time.deltaTime + playerTransform.position.x, playerTransform.position.y + accelerateY * Time.deltaTime);
                    playerTransform.localPosition = velocityPlayer;
                }
            }
        }
        else if (!climbLadder && !climbDown && !climbUp)
        {
            if (!climbLadder && onLadder && (Input.GetButtonDown("Up") || (Input.GetAxis("Vertical") == -1.0f)) && !jumpLadder && (transform.position.y + spriteRender.bounds.size.y / 2.0f) < ladderTop)
            {
                playerRigidbody.velocity = new Vector2(0.0f, 0.0f);
                forceJump.Set(0.0f, 0.0f);
                playerRigidbody.AddForce(forceJump);
                climbGrab = true;
                climbLadder = true;
                isClimbCenter = false;
                playerRigidbody.gravityScale = 0.0f;
                playerTransform.localPosition = new Vector2(playerTransform.position.x, playerTransform.position.y + 0.05f);
            }

            else if (!climbDown && onLadder && (Input.GetButton("Down") || Input.GetAxisRaw("Vertical") == 1.0f) && grounded && (transform.position.y + spriteRender.bounds.size.y / 2.0f) >= ladderTop)
            {
                playerRigidbody.velocity = new Vector2(0.0f, 0.0f);
                playerRigidbody.isKinematic = true;
                playerRigidbody.gravityScale = 0.0f;
                climbDown = true;
                climbLadder = true;
                isClimbCenter = false;
                playerPositionY = transform.position.y;
            }
            else
            {
                climbStand = false;
                climbLadder = false;
                isClimbCenter = false;
                playerRigidbody.gravityScale = gravityStore;
            }
        }
        else if (climbLadder && !onLadder)
        {
            climbGrab = false;
            climbShot = false;
            climbDown = false;
            climbUp = false;
            climbStand = false;
            climbLadder = false;
            isClimbCenter = false;
            playerRigidbody.gravityScale = gravityStore;
        }
    }


    void falling()
    {
        if (playerRigidbody.velocity.y < 0.0f && !down && !down2)
        {
            jump = false;
            down = true;
            accelerateY = 0.0f;
        }
        else if (playerRigidbody.velocity.y < 0.0f && down && !down2)
        {
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerFalling") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= anime.GetCurrentAnimatorStateInfo(0).length)
            {
                down = false;
                down2 = true;
            }
        }
        else if (playerRigidbody.velocity.y >= 0.0f)
        {
            down = false;
            down2 = false;
        }
    }


    void shootClimber()
    {
            if (shot && climbLadder && isClimbCenter && !climbDown && !climbUp)
            {
                anime.Play("playerShootClimb", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
                shotPosition.Set(firePoint.position.x - 0.15f * sideFace, firePoint.position.y + 0.135f, firePoint.position.z);
                Instantiate(Plasma, shotPosition, firePoint.rotation);
                animIstantNormTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
                climbShot = true;
                accelerateY = 0.0f;
            }
            else if (anime.GetCurrentAnimatorStateInfo(0).IsName("playerShootClimb") && climbShot && (anime.GetCurrentAnimatorStateInfo(0).normalizedTime - animIstantNormTime) >= 0.2f)
            {
                climbShot = false;
                anime.Play("playerLadderUpStand", 0, anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
    }


    void side()
    {
        if (Input.GetAxisRaw("Running") > 0.0)
        {
            transform.transform.eulerAngles = eulerAnglesRight;
            sideFace = 1.0f;
        }
        else if (Input.GetAxis("Running") < 0.0)
        {
            transform.transform.eulerAngles = eulerAnglesLeft;
            sideFace = -1.0f;
        }
    }


    void airMoving()
    {
        side();
        playerPositionX = playerTransform.position.x;

        if (Input.GetAxisRaw("Running") > 0.3 || Input.GetAxisRaw("Running") < -0.3)
        {
            if (backDashed && Input.GetAxisRaw("Running") < -0.3)
            {
                accelerateX = 2.0f;
            }
            else if (dashed)
            {
                accelerateX = 2.0f;
            }
            else
            {
                accelerateX = 1.0f;
            }

            airMove = true;

            velocityPlayer.Set(accelerateX * speedPlayerX * sideFace * Time.deltaTime + playerTransform.position.x, playerTransform.position.y);
            playerTransform.localPosition = velocityPlayer;
        }
        else if (Input.GetAxisRaw("Running") == 0)
        {
            accelerateX = 0.0f;
            airMove = false;
        }

    }

    void running()
    {
        side();
        run = Mathf.Abs(Input.GetAxisRaw("Running")) > 0.3f;

        if (run)
        {
            if (Input.GetAxisRaw("Running") > 0.3f)
            {
                if (isDown)
                {
                    accelerateX = 0.5f;
                }
                else
                {
                    accelerateX = 1.0f;
                }
                run = true;
            }
            else if (Input.GetAxisRaw("Running") < -0.3f)
            {
                if (isDown)
                {
                    accelerateX = 0.5f;
                }
                else
                {
                    accelerateX = 1.0f;
                }
                run = true;
            }

            velocityPlayer.Set(accelerateX * speedPlayerX * sideFace * Time.deltaTime + playerTransform.position.x, playerTransform.position.y);
            playerTransform.localPosition = velocityPlayer;
        }
        else
        {
            accelerateX = 0.0f;
            run = false;
        }
    }
}