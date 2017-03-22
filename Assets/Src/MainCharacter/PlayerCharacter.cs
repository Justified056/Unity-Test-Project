using System.Collections;
using System.Collections.Generic;
using DataManagement.DataReader;
using DataManagement.DataType;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Transform legArmorGO;
    private Transform chestArmorGO;
    private Transform headArmorGO;
    private Transform weaponGO;
    private Transform offhandGO;

    private Sprite playerSprite;
    private Sprite legArmorSprite;
    private Sprite chestArmorSprite;
    private Sprite headArmorSprite;
    private Sprite weaponSprite;
    private Sprite offhandSprite; // this could be another weapon, shield, potion, flask....etc.

    void LoadChildSprites(ref List<ItemData> dataList)
    {
        playerSprite = Resources.Load<Sprite>("Characters/Player_Character_Sprite") as Sprite;
        chestArmorSprite = Resources.Load<Sprite>(dataList.Find(id => id.itemType.typeOfItem == "CHESTARMOR").itemFilepath) as Sprite;
        legArmorSprite = Resources.Load<Sprite>(dataList.Find(id => id.itemType.typeOfItem == "LEGARMOR").itemFilepath) as Sprite;
        headArmorSprite = Resources.Load<Sprite>(dataList.Find(id => id.itemType.typeOfItem == "HEADARMOR").itemFilepath) as Sprite;
        weaponSprite = Resources.Load<Sprite>(dataList.Find(id => id.itemType.typeOfItem == "WEAPON").itemFilepath) as Sprite;
        offhandSprite = Resources.Load<Sprite>(dataList.Find(id => id.itemType.typeOfItem == "OFFHAND").itemFilepath) as Sprite;
    }

    void SetChildTransformPositions()
    {
        Transform[] childGameObjects = GetComponentsInChildren<Transform>(true);

        legArmorGO = System.Array.Find(childGameObjects, ct => ct.name == "PlayerLegArmor");
        chestArmorGO = System.Array.Find(childGameObjects, ct => ct.name == "PlayerChestArmor");
        headArmorGO = System.Array.Find(childGameObjects, ct => ct.name == "PlayerHeadArmor");
        weaponGO = System.Array.Find(childGameObjects, ct => ct.name == "PlayerMainHand");
        offhandGO = System.Array.Find(childGameObjects, ct => ct.name == "PlayerOffHand");

        Vector3 legArmorPosition = new Vector3(0.0f, -0.05f, 0.0f);
        Vector3 chestArmorPosition = new Vector3(0.0f, -0.02f, 0.0f);
        Vector3 headArmorPosition = new Vector3(0.0f, 0.03f, 0.0f);
        Vector3 weaponPosition = new Vector3(-0.06f, -0.01f, 0.0f);
        Vector3 offhandPosition = new Vector3(0.06f, -0.03f, 0.0f);

        legArmorGO.localPosition = legArmorPosition;
        chestArmorGO.localPosition = chestArmorPosition;
        headArmorGO.localPosition = headArmorPosition;
        weaponGO.localPosition = weaponPosition;
        offhandGO.localPosition = offhandPosition;
    }

    void SetChildSprites()
    {
        SpriteRenderer[] childSR = GetComponentsInChildren<SpriteRenderer>(true);

        System.Array.Find(childSR, cs => cs.name == "PlayerLegArmor").sprite = legArmorSprite;
        System.Array.Find(childSR, cs => cs.name == "PlayerChestArmor").sprite = chestArmorSprite;
        System.Array.Find(childSR, cs => cs.name == "PlayerHeadArmor").sprite = headArmorSprite;
        System.Array.Find(childSR, cs => cs.name == "PlayerMainHand").sprite = weaponSprite;
        System.Array.Find(childSR, cs => cs.name == "PlayerOffHand").sprite = offhandSprite;
    }
    
    void Awake()
    {
        string itemDataPath = Application.persistentDataPath + "/PlayerInventory.txt";
        TDReader itemDataReader = new TDReader(itemDataPath);
        List<ItemData> itemDataList = itemDataReader.getItemData();

        LoadChildSprites(ref itemDataList);
        SetChildTransformPositions();
    }


	void Start ()
    {
        if (playerSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = playerSprite;
        }

        Camera playerCamera = this.GetComponent<Camera>();

        if(playerCamera != null)
        {
            // playerCamera.targetTexture = playerSprite;
        }

        SetChildSprites();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
