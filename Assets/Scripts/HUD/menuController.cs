using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class menuController : MonoBehaviour
{
    public inventoryAddItem addingNewItem;
    public Text stText;
    public Text extText;
    public Button stButton;
    public Button extButton;
    public int selectOpt;
    public float timeJoyVertical;
    public bool changeButton;

    // Use this for initialization
    void Start()
    {
        selectOpt = 1;
        stButton.Select();
        addingNewItem = GetComponent<inventoryAddItem>();
    }

    // Update is called once per frame
    void Update()
    {
        selectButton();
        stText.fontSize = (int)(stButton.GetComponent<RectTransform>().sizeDelta.x * Screen.width / 80);
        extText.fontSize = (int)(stButton.GetComponent<RectTransform>().sizeDelta.x * Screen.width / 80);
    }

    
    public void btStartGame()
    {
        PlayerPrefs.SetString("itemImageID", "");
        //PlayerPrefs.SetString("itemName", "");
        //PlayerPrefs.SetString("itemAmount", "");
        //PlayerPrefs.SetString("itemDesc", "");
        //PlayerPrefs.Save();
        SceneManager.LoadScene("faseTeste1");
    }


    void selectButton()
    {
        if (Input.GetAxisRaw("Vertical") >= 1f || Input.GetAxisRaw("Vertical") <= -1f)
        {
            if (selectOpt == 1)
            {
                if (!changeButton)
                {
                    selectOpt -= 1;
                    stButton.Select();
                    changeButton = true;
                }
                else
                {
                    timeJoyVertical += Time.deltaTime;
                }
            }
            else if (selectOpt == 0)
            {
                if (!changeButton)
                {
                    selectOpt += 1;
                    extButton.Select();
                    changeButton = true;
                }
                else
                {
                    timeJoyVertical += Time.deltaTime;
                }
            }

            if (timeJoyVertical >= 0.2f && changeButton)
            {
                changeButton = false;
                timeJoyVertical = 0;
            }
        }

        else if (Input.GetAxisRaw("Vertical") == 0f)
        {
            changeButton = false;
            timeJoyVertical = 0;
        }
    }

    public void btExitGame()
    {
        Application.Quit();
    }
}
