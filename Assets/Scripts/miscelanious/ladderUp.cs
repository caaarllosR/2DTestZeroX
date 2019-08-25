using UnityEngine;
using System.Collections;

public class ladderUp : MonoBehaviour
{
    private playerController playerControl;
    public BoxCollider2D ladderCollider;
    private Transform centerLadderPosition;
    private SpriteRenderer ladderSprite;

    // Use this for initialization
    void Start()
    {
        playerControl = FindObjectOfType<playerController>();
        ladderCollider = GetComponent<BoxCollider2D>();
        centerLadderPosition = GetComponent<Transform>();
        ladderSprite = GetComponent<SpriteRenderer>();
        playerControl.climbCenterPosition = centerLadderPosition.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            playerControl.climbCenterPosition = centerLadderPosition.position;
            playerControl.ladderTop = transform.position.y + (ladderSprite.bounds.size.y / 2.0f);
            playerControl.ladderCenter = transform.position.x + ladderCollider.offset.x;
            playerControl.onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            playerControl.onLadder = false;
        }
    }

}
