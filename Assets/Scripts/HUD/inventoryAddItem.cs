using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class inventoryAddItem : MonoBehaviour
{

    public inventoryScript theInventory;
    public playerInventory thePlayerInventory;
    private playerController thePlayerControl;

    public Texture blankIcon;
    public Texture theNewItem;
    public Texture theEqpItem;

    public int arrayGrid;
    public int inventoryNewItemAdd;
    public int inventoryItemEqp;
    public int splitID;

    public string[] splitItemImageID;
    public string[] splitItemName;
    public string[] splitItemAmount;
    public string[] splitItemDesc;

    public string itemImageID;
    public string eqp1ImageID;
    public string eqp2ImageID;
    public string itemName;
    public string itemAmount;
    public string itemDesc;
    public int saveLife;

    // Use this for initialization
    void Start()
    {
        theInventory = GetComponent<inventoryScript>();
        thePlayerInventory = GetComponent<playerInventory>();
        thePlayerControl = GetComponent<playerController>();

        arrayGrid = 0;
        inventoryNewItemAdd = -1;
        inventoryItemEqp = -1;

        loadInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void saveInventory()
    {
        if (theInventory.contentDope1[0].image != null)
        {
            eqp1ImageID = theInventory.contentDope1[0].image.name;
        }
        if (theInventory.contentDope2[0].image != null)
        {
            eqp2ImageID = theInventory.contentDope2[0].image.name;
        }
        saveLife = thePlayerControl.life;

        for (int i = 0; i < theInventory.grids.Length; i++)
        {
            if (theInventory.grids[i].image.name != "gridIcon")
            {
                if (thePlayerInventory.itemPlayerAmount[int.Parse(theInventory.grids[i].image.name)] > 0)
                {
                    itemImageID += theInventory.grids[i].image.name + "-";
                    itemName += theInventory.grids[i].tooltip + "-";
                    itemAmount += theInventory.grids[i].text + "-";
                    itemDesc += theInventory.gridsDesc[i] + "-";
                }
            }
        }
        PlayerPrefs.SetString("itemImageID", itemImageID);
        PlayerPrefs.SetString("itemName", itemName);
        PlayerPrefs.SetString("itemAmount", itemAmount);
        PlayerPrefs.SetString("itemDesc", itemDesc);
        PlayerPrefs.SetString("eqp1ImageID", eqp1ImageID);
        PlayerPrefs.SetString("eqp2ImageID", eqp2ImageID);
        PlayerPrefs.SetInt("saveLife", saveLife);
    }


    public void loadInventory()
    {
        itemImageID = PlayerPrefs.GetString("itemImageID");

        if (itemImageID != "")
        {
            itemName = PlayerPrefs.GetString("itemName");
            itemAmount = PlayerPrefs.GetString("itemAmount");
            itemDesc = PlayerPrefs.GetString("itemDesc");
            eqp1ImageID = PlayerPrefs.GetString("eqp1ImageID");
            eqp2ImageID = PlayerPrefs.GetString("eqp2ImageID");

            if (eqp1ImageID != "")
            {
                theInventory.contentDope1[0].image = (Texture)thePlayerInventory.itemTexture[int.Parse(PlayerPrefs.GetString("eqp1ImageID"))];
            }
            if (eqp2ImageID != "")
            {
                theInventory.contentDope2[0].image = (Texture)thePlayerInventory.itemTexture[int.Parse(PlayerPrefs.GetString("eqp2ImageID"))];
            }
            thePlayerControl.life = PlayerPrefs.GetInt("saveLife");
            splitItemImageID = itemImageID.Split('-');
            splitItemName = itemName.Split('-');
            splitItemAmount = itemAmount.Split('-');
            splitItemDesc = itemDesc.Split('-');

            for (int i = 0; i < splitItemImageID.Length; i++)
            {
                if (splitItemImageID[i] != "")
                {
                    splitID = int.Parse(splitItemImageID[i]);
                    theInventory.grids[i].image = (Texture)thePlayerInventory.itemTexture[splitID];
                    theInventory.grids[i].tooltip = splitItemName[i];
                    theInventory.grids[i].text = splitItemAmount[i];
                    theInventory.gridsDesc[i] = splitItemDesc[i];
                }
            }
            PlayerPrefs.DeleteAll();
        }
    }


    void equipDope()
    {
        if (inventoryItemEqp > -1)
        {
            theEqpItem = (Texture)thePlayerInventory.itemTexture[inventoryItemEqp];

            if (thePlayerInventory.itemType[inventoryItemEqp] == "active")
            {
                theInventory.contentDope1[0].image = theEqpItem;
            }
            else if (thePlayerInventory.itemType[inventoryItemEqp] == "passive")
            {
                theInventory.contentDope2[0].image = theEqpItem;
            }
            inventoryItemEqp = -1;
        }
    }


    void newItem()
    {
        if (inventoryNewItemAdd > -1)
        {
            theNewItem = (Texture)thePlayerInventory.itemTexture[inventoryNewItemAdd];
            if (arrayGrid < theInventory.grids.Length)
            {
                if (theInventory.grids[arrayGrid].image.name == theNewItem.name)
                {
                    theInventory.grids[arrayGrid].text = thePlayerInventory.itemPlayerAmount[inventoryNewItemAdd].ToString();
                    arrayGrid = 0;
                    inventoryNewItemAdd = -1;
                }
                else if (theInventory.grids[arrayGrid].image.name == blankIcon.name)
                {
                    theInventory.grids[arrayGrid].image = theNewItem;
                    theInventory.grids[arrayGrid].text = thePlayerInventory.itemPlayerAmount[inventoryNewItemAdd].ToString();
                    theInventory.grids[arrayGrid].tooltip = thePlayerInventory.itemID[inventoryNewItemAdd];
                    theInventory.gridsDesc[arrayGrid] = thePlayerInventory.itemDesc[inventoryNewItemAdd];
                    arrayGrid = 0;
                    inventoryNewItemAdd = -1;
                }
                else if (theInventory.grids[arrayGrid].image.name != blankIcon.name)
                {
                    arrayGrid += 1;
                }
            }
        }
    }
}
