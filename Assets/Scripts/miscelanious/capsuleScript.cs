using UnityEngine;
using System.Collections;

public class capsuleScript : MonoBehaviour
{
    public bool isCapsuleCenter;
    public bool openCapsule;
    public float capsuleCenter;

    public BoxCollider2D capsuleCollider;

    public playerController thePlayer;
    private Animator capsuleAnime;
    private Renderer spriteRender;

    public bool testBool;

    // Use this for initialization
    void Start()
    {
        capsuleAnime = GetComponent<Animator>();
        spriteRender = GetComponent<Renderer>();

        thePlayer = FindObjectOfType<playerController>();
        capsuleCollider = GetComponent<BoxCollider2D>();

        capsuleCenter = transform.position.x + capsuleCollider.offset.x;
    }

    // Update is called once per frame
    void Update()
    {
        capsuleAnime.SetBool("isCapsuleCenter", isCapsuleCenter);
        capsuleAnime.SetBool("openCapsule", openCapsule);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            openCapsule = true;
        }
    }


    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            thePlayer.accelerateX = 0.0f;
            thePlayer.teleportCapsule = true;
            if (!isCapsuleCenter && Mathf.Abs(capsuleCenter - other.transform.position.x) < 0.05f)
            {
                other.transform.position = new Vector2(capsuleCenter, other.transform.position.y);
                spriteRender.sortingOrder = 3;
                isCapsuleCenter = true;
            }
            else if (!isCapsuleCenter)
            {
                if (other.transform.position.x < capsuleCenter)
                {
                    other.transform.localPosition = new Vector2(other.transform.position.x + 1.0f * Time.deltaTime, other.transform.position.y);
                }
                else if (other.transform.position.x > capsuleCenter)
                {
                    other.transform.localPosition = new Vector2(other.transform.position.x - 1.0f * Time.deltaTime, other.transform.position.y);
                }
            }
        }
    }
}
