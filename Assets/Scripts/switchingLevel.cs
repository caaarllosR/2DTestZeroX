using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class switchingLevel : MonoBehaviour
{

    public bool testBool;
    public inventoryAddItem addingNewItem;

    // Use this for initialization
    void Start()
    {
        addingNewItem = FindObjectOfType<inventoryAddItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay2D(Collision2D other)
    {
        testBool = true;
        if (other.gameObject.layer == 8)
        {
            addingNewItem.Invoke("saveInventory", 0);
            SceneManager.LoadScene("faseTeste2");
        }
    }

}
