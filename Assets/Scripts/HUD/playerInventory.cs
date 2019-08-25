using UnityEngine;
using System.Collections;

public class playerInventory : MonoBehaviour
{
    public string[] itemID = new string[4];
    public string[] itemDesc = new string[4];
    public string[] itemType = new string[4];
    public int[] itemPlayerAmount = new int[4];

    public string[] weaponID = new string[2];
    public string[] weaponDesc = new string[2];
    public Texture[] weaponTexture;

    public Texture[] itemTexture;

    // Use this for initialization
    void Start()
    {
        itemID = new string[] {"dope1", "dope2", "dope3", "dope4"};
        itemType = new string[] {"active", "active", "passive", "passive"};
        itemDesc = new string[] {"blablablabla", "blablablabla", "blablablabla", "blablablabla" };
        itemPlayerAmount = new int[] { 0, 0, 0, 0 };
        itemTexture = new Texture[itemID.Length];
        itemTexture = Resources.LoadAll<Texture>("Item_Icons");

        weaponID = new string[] { "plasma", "golpes ninja"};
        weaponDesc = new string[] { "arma de plasma usada para pulverizar seus oponentes...", "Baladas e filmes ninja..."};
        weaponTexture = new Texture[weaponID.Length];
        weaponTexture = Resources.LoadAll<Texture>("Weapon_Icons");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
