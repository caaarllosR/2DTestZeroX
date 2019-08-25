using UnityEngine;
using System.Collections;

public class zeroEnemyPlasmaController : MonoBehaviour
{
    private stateMachine machine;
    private Animator anime;
    private playerController playerControl;
    private zeroEnemyController zeroEnemyControl;
    private Vector2 velocityPlasma;
    private Camera cameraObj;

    public float speedPlasma;
    public float animSpeed;
    public float cameraPosXRight;
    public float cameraPosXLeft;
    public float cameraDist;

    // Use this for initialization
    void Start()
    {
        anime = GetComponent<Animator>();
        machine = FindObjectOfType<stateMachine>();
        zeroEnemyControl = FindObjectOfType<zeroEnemyController>();
        playerControl = FindObjectOfType<playerController>();
        velocityPlasma = new Vector2(0.0f, 0.0f);
        cameraObj = FindObjectOfType<Camera>();
        speedPlasma = 4 * zeroEnemyControl.sideFace;
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

       
            if (zeroEnemyControl.sideFace > 0)
            {
                if (cameraPosXRight - transform.position.x < -1.0f)
                {
                    Destroy(gameObject);
                }
            }
            if (zeroEnemyControl.sideFace < 0)
            {
                if (cameraPosXLeft - transform.position.x > 1.0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!(machine.states == gameState.PAUSED))
        {
            if (!playerControl.isImune && !playerControl.imuneDodge)
            {
                if (other.gameObject.layer == 8)
                {
                    playerControl.life = playerControl.life - 1;
                    if (playerControl.sideFace + zeroEnemyControl.sideFace != 0.0f)
                    {
                        if (other.GetComponent<Transform>().transform.eulerAngles.y < 20.0f)
                        {
                            other.GetComponent<Transform>().transform.eulerAngles = playerControl.eulerAnglesLeft;
                            playerControl.sideFace = playerControl.sideFace * (-1);
                        }
                        else
                        {
                            other.GetComponent<Transform>().transform.eulerAngles = playerControl.eulerAnglesRight;
                            playerControl.sideFace = playerControl.sideFace * (-1);
                        }
                    }
                    playerControl.damageEffect = true;
                }
                Destroy(gameObject);
            }
            else if (playerControl.isImune || playerControl.imuneDodge)
            {
                if (!(other.gameObject.layer == 8))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
