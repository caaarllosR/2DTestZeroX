using UnityEngine;
using System.Collections;

public class inventoryScript : MonoBehaviour
{

    private Vector2 windowPosition;
    private Vector2 windowSize;
    private Vector2 gridPosition;
    private Vector2 gridSize;
    private Vector2 eqpDopePosition1;
    private Vector2 eqoDopePosition2;
    private Vector2 eqpWeaponPosition;
    private Vector2 eqpSize;

    private Rect eqpButton;

    private int itemID;
    private int weaponID;
    private int gridLineValue;

    public int testGridPer5;
    public int gridValue;
    public int indexGrid;
    public int indexGridY;
    public int pressDopeEqp1;
    public int pressDopeEqp2;
    public int pressWeaponEqp;
    public int selectedGridEqp;
    public int previousGridEqp;

    public float timeJoyVertical;
    public float timeJoyHorizontal;

    public bool dope1DescOnOff;
    public bool dope2DescOnOff;
    public bool weaponDescOnOff;
    public bool gridDescOnOff;
    public bool changeGridY;
    public bool changeGridX;
    public bool inventoryOn;
    public bool selectL;
    public bool selectR;
    public bool changeGridEqp;

    public string slotDope1;
    public string slotDope2;

    public string[] gridsDesc;

    private GUIStyle style;
    private GUIStyle styleLife;

    public GUIContent buttonDopeEqp;
    public GUIContent[] grids;
    public GUIContent[] contentDope1;
    public GUIContent[] contentDope2;
    public GUIContent[] contentWeapon;

    public Texture invetoryWindow;

    private playerController thePlayerControl;
    private inventoryAddItem addingNewItem;
    private playerInventory thePlayerInventory;

    // Use this for initialization
    void Start()
    {
        pressDopeEqp1 = -1;
        pressDopeEqp2 = -1;
        pressWeaponEqp = -1;
        gridValue = -1;
        gridLineValue = 5;
        indexGrid = grids.Length - 1;
        indexGridY = (grids.Length / 5) - 1;
        style = new GUIStyle();
        style.normal.textColor = Color.white;

        styleLife = new GUIStyle();
        styleLife.font = (Font)Resources.Load("Fonts/BIONIC");
        styleLife.normal.textColor = Color.green;
        styleLife.alignment = TextAnchor.LowerRight;
        windowPosition = new Vector2(0, 0);

        addingNewItem = GetComponent<inventoryAddItem>();
        thePlayerInventory = GetComponent<playerInventory>();
        thePlayerControl = GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        windowSize = new Vector2(Screen.width, 5000);
        eqpButton = new Rect(Screen.width / 1.18f, Screen.height / 1.35f, Screen.width / 8f, Screen.height / 8f);
        gridPosition = new Vector2(Screen.width / 1.725f, Screen.height / 12f);
        gridSize = new Vector2(Screen.width / 2.50f, Screen.height / 1.6f);

        eqpDopePosition1 = new Vector2(Screen.width / 13.725f, Screen.height / 1.83f);
        eqoDopePosition2 = new Vector2(Screen.width / 2.705f, Screen.height / 1.83f);
        eqpWeaponPosition = new Vector2(Screen.width / 13.725f, Screen.height / 3.25f);
        eqpSize = new Vector2(Screen.width / 6.54f, Screen.height / 11f);

        if (Input.GetButtonDown("SelectL") && !selectL)
        {
            changeGridEqp = false;
            selectL = true;
            selectR = false;
            pressDopeEqp1 = 0;
            pressDopeEqp2 = -1;
            pressWeaponEqp = -1;
            gridValue = -1;
        }
        else if (Input.GetButtonDown("SelectR") && !selectR)
        {
            changeGridX = false;
            changeGridY = false;
            selectR = true;
            selectL = false;
            pressDopeEqp1 = -1;
            pressDopeEqp2 = -1;
            pressWeaponEqp = -1;
            gridValue = 0;
        }


        if (Input.GetButtonDown("Select"))
        {
            if (inventoryOn)
            {
                inventoryOn = false;
                selectL = false;
                selectR = false;
                pressDopeEqp1 = -1;
                pressDopeEqp2 = -1;
                pressWeaponEqp = -1;
                gridValue = -1;
            }
            else if (!inventoryOn)
            {
                inventoryOn = true;
                changeGridX = false;
                changeGridY = false;
                selectR = true;
                selectL = false;
                pressDopeEqp1 = -1;
                pressDopeEqp2 = -1;
                pressWeaponEqp = -1;
                gridValue = 0;
            }
        }

        selectEqpGrid();
        selectEquipDope();
        addingNewItem.Invoke("newItem", 0);
        addingNewItem.Invoke("equipDope", 0);
    }


    void selectEquipDope()
    {
        if (inventoryOn)
        {
            if (selectR)
            {
                if (Input.GetAxisRaw("Vertical") <= -0.85f)
                {
                    changeGridX = false;
                    timeJoyHorizontal = 0;

                    if (!changeGridY)
                    {
                        if (gridValue / 5 < indexGridY)
                        {
                            gridValue += 5;
                            changeGridY = true;
                        }
                        else
                        {
                            gridValue -= indexGrid - 4;
                            changeGridY = true;
                        }
                    }
                    else
                    {
                        timeJoyVertical += Time.deltaTime;
                    }

                    if (timeJoyVertical >= 0.2f && changeGridY)
                    {
                        changeGridY = false;
                        timeJoyVertical = 0;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") >= 0.85f)
                {
                    changeGridX = false;
                    timeJoyHorizontal = 0;

                    if (!changeGridY)
                    {
                        if (gridValue / 5 == 0)
                        {
                            gridValue += indexGrid - 4;
                            changeGridY = true;
                        }
                        else if (gridValue / 5 <= indexGridY)
                        {
                            gridValue -= 5;
                            changeGridY = true;
                        }
                    }
                    else
                    {
                        timeJoyVertical += Time.deltaTime;
                    }

                    if (timeJoyVertical >= 0.2f && changeGridY)
                    {
                        changeGridY = false;
                        timeJoyVertical = 0;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") >= 0.85f)
                {
                    changeGridY = false;
                    timeJoyVertical = 0;

                    if (!changeGridX)
                    {
                        if (gridValue < indexGrid)
                        {
                            gridValue += 1;
                            changeGridX = true;
                        }
                        else
                        {
                            gridValue = 0;
                            changeGridX = true;
                        }
                    }
                    else
                    {
                        timeJoyHorizontal += Time.deltaTime;
                    }

                    if (timeJoyHorizontal >= 0.2f && changeGridX)
                    {
                        changeGridX = false;
                        timeJoyHorizontal = 0;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") <= -0.85f)
                {
                    changeGridY = false;
                    timeJoyVertical = 0;

                    if (!changeGridX)
                    {
                        if (gridValue == 0)
                        {
                            gridValue = indexGrid;
                            changeGridX = true;
                        }
                        else if (gridValue <= indexGrid)
                        {
                            gridValue -= 1;
                            changeGridX = true;
                        }
                    }
                    else
                    {
                        timeJoyHorizontal += Time.deltaTime;
                    }

                    if (timeJoyHorizontal >= 0.2f && changeGridX)
                    {
                        changeGridX = false;
                        timeJoyHorizontal = 0;
                    }
                }

                if (Input.GetAxisRaw("Vertical") == 0f && Input.GetAxisRaw("Horizontal") == 0f)
                {
                    changeGridY = false;
                    changeGridX = false;
                    timeJoyVertical = 0;
                    timeJoyHorizontal = 0;
                }
            }
        }
    }


    void selectEqpGrid()
    {
        if (inventoryOn)
        {
            if (selectL)
            {
                if (Input.GetAxisRaw("Vertical") >= 0.85f)
                {
                    if (selectedGridEqp == 0)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp = 2;
                            previousGridEqp = 0;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyVertical += Time.deltaTime;
                        }
                    }
                    if (selectedGridEqp == 1)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp = 2;
                            previousGridEqp = 1;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyVertical += Time.deltaTime;
                        }
                    }
                    if (selectedGridEqp == 2)
                    {
                        if (!changeGridEqp)
                        {
                            if (previousGridEqp == 1)
                            {
                                selectedGridEqp = 0;
                            }
                            else
                            {
                                selectedGridEqp = 1;
                            }
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyVertical += Time.deltaTime;
                        }
                    }

                    if (timeJoyVertical >= 0.2f && changeGridEqp)
                    {
                        changeGridEqp = false;
                        timeJoyVertical = 0;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") <= -0.85f)
                {
                    if (selectedGridEqp == 0)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp = 2;
                            previousGridEqp = 0;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyVertical += Time.deltaTime;
                        }
                    }
                    if (selectedGridEqp == 1)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp = 2;
                            previousGridEqp = 1;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyVertical += Time.deltaTime;
                        }
                    }
                    if (selectedGridEqp == 2)
                    {
                        if (!changeGridEqp)
                        {
                            if (previousGridEqp == 0)
                            {
                                selectedGridEqp = 0;
                            }
                            else
                            {
                                selectedGridEqp = 1;
                            }
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyVertical += Time.deltaTime;
                        }
                    }

                    if (timeJoyVertical >= 0.2f && changeGridEqp)
                    {
                        changeGridEqp = false;
                        timeJoyVertical = 0;
                    }
                }

                else if (Input.GetAxisRaw("Horizontal") >= 0.85f)
                {
                    if (selectedGridEqp < 2)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp += 1;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyHorizontal += Time.deltaTime;
                        }
                    }
                    if (selectedGridEqp == 2)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp = 0;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyHorizontal += Time.deltaTime;
                        }
                    }

                    if (timeJoyHorizontal >= 0.2f && changeGridEqp)
                    {
                        changeGridEqp = false;
                        timeJoyHorizontal = 0;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") <= -0.85f)
                {
                    if (selectedGridEqp > 0)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp -= 1;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyHorizontal += Time.deltaTime;
                        }
                    }
                    if (selectedGridEqp == 0)
                    {
                        if (!changeGridEqp)
                        {
                            selectedGridEqp = 2;
                            changeGridEqp = true;
                        }
                        else
                        {
                            timeJoyHorizontal += Time.deltaTime;
                        }
                    }

                    if (timeJoyHorizontal >= 0.2f && changeGridEqp)
                    {
                        changeGridEqp = false;
                        timeJoyHorizontal = 0;
                    }
                }

                if (selectedGridEqp == 0)
                {
                    pressDopeEqp1 = 0;
                    pressDopeEqp2 = -1;
                    pressWeaponEqp = -1;
                }
                else if(selectedGridEqp == 1)
                {
                    pressDopeEqp1 = -1;
                    pressDopeEqp2 = 0;
                    pressWeaponEqp = -1;
                }
                else if (selectedGridEqp == 2)
                {
                    pressDopeEqp1 = -1;
                    pressDopeEqp2 = -1;
                    pressWeaponEqp = 0;
                }

                if (Input.GetAxisRaw("Vertical") == 0f && Input.GetAxisRaw("Horizontal") == 0f)
                {
                    changeGridEqp = false;
                    timeJoyVertical = 0;
                    timeJoyHorizontal = 0;
                }
            }
        }
    }


    void OnGUI()
    {
        if (inventoryOn)
        {
            GUI.BeginGroup(new Rect(windowPosition.x, windowPosition.y, windowSize.x, windowSize.y), invetoryWindow);
            GUI.EndGroup();
            pressDopeEqp1 = GUI.SelectionGrid(new Rect(eqpDopePosition1.x, eqpDopePosition1.y, eqpSize.x, eqpSize.y), pressDopeEqp1, contentDope1, 1);
            pressDopeEqp2 = GUI.SelectionGrid(new Rect(eqoDopePosition2.x, eqoDopePosition2.y, eqpSize.x, eqpSize.y), pressDopeEqp2, contentDope2, 1);
            pressWeaponEqp = GUI.SelectionGrid(new Rect(eqpWeaponPosition.x, eqpWeaponPosition.y, eqpSize.x, eqpSize.y), pressWeaponEqp, contentWeapon, 1);
            gridValue = GUI.SelectionGrid(new Rect(gridPosition.x, gridPosition.y, gridSize.x, gridSize.y), gridValue, grids, gridLineValue);
            styleLife.fontSize = (int)(Screen.width / 35.05f);
            GUI.Label(new Rect(Screen.width / 3.35f, Screen.height / 4.3f, styleLife.fontSize, styleLife.fontSize), thePlayerControl.life.ToString(), styleLife);
            GUI.Label(new Rect(Screen.width / 2.45f, Screen.height / 4.3f, styleLife.fontSize, styleLife.fontSize), "16", styleLife);

            if (pressDopeEqp1 >= 0 && !dope1DescOnOff)
            {
                dope1DescOnOff = true;
                dope2DescOnOff = false;
                weaponDescOnOff = false;
                gridDescOnOff = false;
                selectR = false;
                selectL = true;
                pressDopeEqp2 = -1;
                pressWeaponEqp = -1;
                gridValue = -1;
            }
            if (pressDopeEqp2 >= 0 && !dope2DescOnOff)
            {
                dope2DescOnOff = true;
                dope1DescOnOff = false;
                weaponDescOnOff = false;
                gridDescOnOff = false;
                selectR = false;
                selectL = true;
                pressDopeEqp1 = -1;
                pressWeaponEqp = -1;
                gridValue = -1;
            }
            if (pressWeaponEqp >= 0 && !weaponDescOnOff)
            {
                addingNewItem.Invoke("saveInventory", 0);
                weaponDescOnOff = true;
                dope1DescOnOff = false;
                dope2DescOnOff = false;
                gridDescOnOff = false;
                selectR = false;
                selectL = true;
                pressDopeEqp1 = -1;
                pressDopeEqp2 = -1;
                gridValue = -1;
            }
            if (gridValue >= 0 && !gridDescOnOff)
            {
                gridDescOnOff = true;
                dope2DescOnOff = false;
                dope1DescOnOff = false;
                weaponDescOnOff = false;
                selectL = false;
                selectR = true;
                pressDopeEqp1 = -1;
                pressDopeEqp2 = -1;
                pressWeaponEqp = -1;
            }

            if (pressDopeEqp1 >= 0 && contentDope1[0].image != null)
            {
                pressDopeEqp2 = -1;
                pressWeaponEqp = -1;
                gridValue = -1;
                itemID = int.Parse(contentDope1[0].image.name);
                style.fontSize = (int)(Screen.width / 30.05f);
                GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.35f, 200, 40), "Name: " + thePlayerInventory.itemID[itemID] + "   Amount: " + thePlayerInventory.itemPlayerAmount[itemID] + "   Type: " + thePlayerInventory.itemType[itemID], style);
                GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.15f, 100, 100), thePlayerInventory.itemDesc[itemID], style);
            }
            else if (pressDopeEqp2 >= 0 && contentDope2[0].image != null)
            {
                pressDopeEqp1 = -1;
                pressWeaponEqp = -1;
                gridValue = -1;
                itemID = int.Parse(contentDope2[0].image.name);
                style.fontSize = (int)(Screen.width / 30.05f);
                GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.35f, 200, 40), "Name: " + thePlayerInventory.itemID[itemID] + "   Amount: " + thePlayerInventory.itemPlayerAmount[itemID] + "   Type: " + thePlayerInventory.itemType[itemID], style);
                GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.15f, 100, 100), thePlayerInventory.itemDesc[itemID], style);
            }
            else if (pressWeaponEqp >= 0 && contentWeapon[0].image != null)
            {
                pressDopeEqp1 = -1;
                pressDopeEqp2 = -1;
                gridValue = -1;
                weaponID = int.Parse(contentWeapon[0].image.name);
                style.fontSize = (int)(Screen.width / 30.05f);
                GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.35f, 200, 40), "Name: " + thePlayerInventory.weaponID[weaponID], style);
                GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.15f, 100, 100), thePlayerInventory.weaponDesc[weaponID], style);
            }

            if (gridValue != -1)
            {
                pressDopeEqp1 = -1;
                pressDopeEqp2 = -1;
                pressWeaponEqp = -1;

                if (grids[gridValue].image.name != "gridIcon")
                {
                    itemID = int.Parse(grids[gridValue].image.name);
                    style.fontSize = (int)(Screen.width / 30.05f);
                    GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.35f, 200, 40), "Name: " + grids[gridValue].tooltip + "   Amount: " + grids[gridValue].text + "   Type: " + thePlayerInventory.itemType[itemID], style);
                    GUI.Label(new Rect(Screen.width / 30.05f, Screen.height / 1.15f, 100, 100), gridsDesc[gridValue], style);

                    if (contentDope1[0].image != null)
                    {
                        slotDope1 = contentDope1[0].image.name;
                    }
                    if (contentDope2[0].image != null)
                    {
                        slotDope2 = contentDope2[0].image.name;
                    }

                    if (slotDope1 != grids[gridValue].image.name && slotDope2 != grids[gridValue].image.name)
                    {
                        if (GUI.Button(new Rect(eqpButton.x, eqpButton.y, eqpButton.width, eqpButton.height), buttonDopeEqp) || Input.GetButton("Equip"))
                        {
                            addingNewItem.inventoryItemEqp = itemID;
                        }
                    }
                }
            }
        }
    }
}
