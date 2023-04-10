using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Resource, Building, Character }
public class CreateController : MonoBehaviour
{
    public static CreateController Instance = null;
    public GameObject itemPrefab;
    public Transform itemParent;
    public MeshRenderer itemSprite;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance CreateController");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //TestCreate();
    }

    // Update is called once per frame
    void Update()
    {

    }



    // //指定类型，坐标随机
    public void CreateItem(ItemType itemType, ResourceType? resourceType, BuildingType? buildingType, SpecialSkill? characterSkill)
    {
        Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        while (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f)
        {
            targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }

        CreateItem(itemType, resourceType, buildingType, characterSkill, targetEuler);
    }
    // //指定类型和区块
    public void CreateItem(ItemType itemType, ResourceType? resourceType, BuildingType? buildingType, SpecialSkill? characterSkill, int[] blockNum)
    {
        Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        while ((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f)
                || (System.Array.IndexOf(blockNum, BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler))) == -1))
        {
            targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
        CreateItem(itemType, resourceType, buildingType, characterSkill, targetEuler);
    }
    //指定类型和坐标
    public void CreateItem(ItemType itemType, ResourceType? resourceType, BuildingType? buildingType, SpecialSkill? characterSkill, Vector3 targetEuler)
    {
        GameObject item = Instantiate(itemPrefab);
        item.transform.SetParent(itemParent);
        item.transform.position = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.transform.eulerAngles = targetEuler;
        itemSprite = item.transform.Find("ItemSprite").GetComponent<MeshRenderer>();

        Item itemScript = item.GetComponent<Item>();
        Character character = item.GetComponent<Character>();

        switch (itemType)
        {
            case ItemType.Resource:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.515f, 0);
                itemScript.itemType = ItemType.Resource;
                itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[0];
                item.name = ResourceSystem.Instance.resourceDatas[(int)resourceType].name;
                break;
            case ItemType.Building:
                itemSprite.transform.localPosition = new Vector3(0, 0.54f, 0);
                itemScript.itemType = ItemType.Building;
                itemSprite.material = BuildingSystem.Instance.buildingDatas[(int)buildingType].buildingMaterial;
                item.name = BuildingSystem.Instance.buildingDatas[(int)buildingType].name;
                break;
            case ItemType.Character:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.51f, 0);
                itemScript.itemType = ItemType.Character;
                character.characterNum = GetCharacterNum();
                CharacterSystem.Instance.characters[GetCharacterNum()] = character;
                itemSprite.material = CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).characterMaterial;
                item.name = CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).name;
                break;
        }
    }

    //获取角色数组里的最小空位
    private int GetCharacterNum()
    {
        int i = 0;
        while (CharacterSystem.Instance.characters[i] != null)
        {
            i++;
        }
        return i;
    }


    public void TestCreate()
    {
        int num = Random.Range(0, 24);
        for (int i = 0; i <= 3; i++)
        {
            CreateController.Instance.CreateItem(ItemType.Character, null, null, SpecialSkill.None, new int[] { num });
        }
        CreateController.Instance.CreateItem(ItemType.Building, null, BuildingType.Cave, null, new int[] { num });
        BuildingSystem.Instance.buildings[0] = GameObject.Find("Items").transform.GetChild(GameObject.Find("Items").transform.childCount - 1).GetComponent<Building>();
        BuildingSystem.Instance.buildingInBlock[num]++;
        BuildingSystem.Instance.buildings[0].stopGenerate = true;
    }
}
