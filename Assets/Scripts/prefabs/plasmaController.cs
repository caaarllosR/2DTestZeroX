using UnityEngine;
using System.Collections;

public class plasmaController : MonoBehaviour
{
    private playerController playerControl;
    private Camera cameraObj;
    private Animator anime;
    private BoxCollider2D boxColliderPlasma;
    private stateMachine machine;

    private Vector2 velocityPlasma;
    private Vector2 offSetPlasma2;
    private Vector2 sizePlasma2;
    private Vector2 offSetPlasma3;
    private Vector2 sizePlasma3;

    public int damagePlasma;
    private float speedPlasma;
    private float animSpeed;
    public float cameraPosXRight;
    public float cameraPosXLeft;
    public float cameraDist;

    // Use this for initialization
    void Start()
    {
        anime = GetComponent<Animator>();
        boxColliderPlasma = GetComponent<BoxCollider2D>();
        machine = FindObjectOfType<stateMachine>();
        playerControl = FindObjectOfType<playerController>();
        cameraObj = FindObjectOfType<Camera>();
        anime.SetFloat("charge", playerControl.charge);
        anime.SetBool("shoting", playerControl.shot);
        speedPlasma = 4 * playerControl.sideFace * playerControl.sliceFace;

        velocityPlasma = new Vector2(0.0f, 0.0f);
        offSetPlasma2.Set(0.22f, 0);
        sizePlasma2.Set(0.44f, 0.15f);
        offSetPlasma3.Set(0.205f, 0);
        sizePlasma3.Set(0.41f, 0.23f);
        playerControl.charge = 0.0f;
        transform.eulerAngles = playerControl.eulerTransSlice;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(machine.states == gameState.PAUSED))
        {
            velocityPlasma.Set(transform.position.x + speedPlasma * Time.deltaTime, transform.position.y);
            transform.position = velocityPlasma;
            cameraDist = cameraObj.ScreenToWorldPoint(new Vector3(cameraObj.pixelWidth, 0, 0)).x - cameraObj.transform.position.x;
            cameraPosXRight = cameraObj.ScreenToWorldPoint(new Vector3(cameraObj.pixelWidth, 0, 0)).x;
            cameraPosXLeft = cameraObj.transform.position.x - cameraDist;


            if (playerControl.sideFace > 0)
            {
                if (cameraPosXRight - transform.position.x < -1.0f)
                {
                    Destroy(gameObject);
                }
            }
            if (playerControl.sideFace < 0)
            {
                if (cameraPosXLeft - transform.position.x > 1.0f)
                {
                    Destroy(gameObject);
                }
            }

            //animação
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!(machine.states == gameState.PAUSED))
        {
            if (other.gameObject.layer == 14)
            {
                playerControl.attack = damagePlasma;
                other.gameObject.GetComponent<MonoBehaviour>().Invoke("damaged", 0);
            }
            Destroy(gameObject);
        }
    }

    void colliderPlasma2()
    {
        boxColliderPlasma.offset = offSetPlasma2;
        boxColliderPlasma.size = sizePlasma2;
    }

    void colliderPlasma3()
    {
        boxColliderPlasma.offset = offSetPlasma3;
        boxColliderPlasma.size = sizePlasma3;
    }

    void damagedPlasma1()
    {
        damagePlasma = 1;
    }

    void damagedPlasma2()
    {
        damagePlasma = 2;
    }

    void damagedPlasma3()
    {
        damagePlasma = 3;
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
