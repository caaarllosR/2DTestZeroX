using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class swtchingLevel2 : MonoBehaviour
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


    void loadLevel()
    {
        addingNewItem.Invoke("saveInventory", 0);
        SceneManager.LoadScene("faseTeste1");
    }
}
