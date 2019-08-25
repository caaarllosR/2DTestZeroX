using UnityEngine;
using System.Collections;

public class collections : MonoBehaviour
{

    public playerInventory thePlayerInventory;
    public playerController thePlqyerControl;
    public SpriteRenderer spriteItemID;
    public inventoryAddItem addingNewItem;
    public int itemID;

    // Use this for initialization
    void Start()
    {
        thePlayerInventory = GetComponent<playerInventory>();
        thePlqyerControl = GetComponent<playerController>();
        addingNewItem = GetComponent<inventoryAddItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {      
        if (col.gameObject.layer == 16 && !thePlqyerControl.dodge)
        {
            spriteItemID = col.gameObject.GetComponent<SpriteRenderer>();
            itemID = int.Parse(spriteItemID.sprite.name);
            thePlayerInventory.itemPlayerAmount[itemID] += 1;
            addingNewItem.inventoryNewItemAdd = itemID;
            Destroy(col.gameObject);
        }
    }
}
