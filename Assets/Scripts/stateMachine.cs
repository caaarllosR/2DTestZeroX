using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum gameState
{
    STARTGAME,
    MENU,
    INGAME,
    PAUSED,
    DEAD,
    WIN,
    LOSE,
    GAMEOVER
}


public class stateMachine : MonoBehaviour
{
    public bool pause;

    public float animSpeed;

    public GameObject[] gameObjects;
    public playerController thePlayer;
    public Rigidbody2D thePlayerRigidbody;
    public Animator thePlayerAnimator;

    public gameState states;

    // Use this for initialization
    void Start()
    {
        thePlayer = FindObjectOfType<playerController>();
        thePlayerAnimator = thePlayer.GetComponent<Animator>();
        thePlayerRigidbody = thePlayer.GetComponent<Rigidbody2D>();
        pause = false;
        states = gameState.INGAME;
    }

    // Update is called once per frame
    void Update()
    {
        GameState();
    }

    private void GameState()
    {
        switch (states)
        {
            case gameState.STARTGAME:
                {
           
                }
                break;
            case gameState.MENU:
                {

                }
                break;
            case gameState.INGAME:
                {
                    if (Input.GetButtonDown("Select") && !pause)
                    {
                        pause = true;
                        gameObjects = GameObject.FindGameObjectsWithTag("animated");
                        foreach (GameObject gb in gameObjects)
                        {
                            gb.gameObject.GetComponent<MonoBehaviour>().Invoke("paused", 0);
                        }
                        states = gameState.PAUSED;
                    }
                }
                break;
            case gameState.PAUSED:
                {
                    if (Input.GetButtonDown("Select") && pause)
                    {
                        pause = false;
                        gameObjects = GameObject.FindGameObjectsWithTag("animated");
                        foreach (GameObject gb in gameObjects)
                        {
                            gb.gameObject.GetComponent<MonoBehaviour>().Invoke("unPaused", 0);
                        }
                       
                            states = gameState.INGAME;
                        
                    }
                }
                break;
            case gameState.DEAD:
                {

                }
                break;
            case gameState.WIN:
                {

                }
                break;
            case gameState.LOSE:
                {

                }
                break;
            case gameState.GAMEOVER:
                {

                }
                break;
        }
    }

    private gameState CurrentGameState
    {
        get
        {
            return states;
        }
        set
        {
            states = value;
        }
    }

}
