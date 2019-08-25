using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour
{
    private playerController thePlayer;
    // Use this for initialization
    void Start()
    {
        thePlayer = FindObjectOfType<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(thePlayer.transform.position.x, 1.0f, -10);
        //transform.position = new Vector3(0.004f, 1.1f, -10);
        //transform.position = new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -10);
    }
}

