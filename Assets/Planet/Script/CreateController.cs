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
        TestCreate();
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
        Resources resources = item.GetComponent<Resources>();
        Building building = item.GetComponent<Building>();

        switch (itemType)
        {
            case ItemType.Resource:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.505f, 0);
                itemScript.itemType = ItemType.Resource;
                itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[0];
                item.name = ResourceSystem.Instance.resourceDatas[(int)resourceType].name;
                resources.resourceType = (ResourceType)resourceType;
                switch (resourceType)
                {
                    //分地形生成木头资源
                    case ResourceType.Wood:
                        if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[1]) < 0.01f)
                        {
                            itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[0];
                        }
                        else if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[2]) < 0.01f)
                        {
                            int randomWood = Random.Range(1, 4);
                            itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[randomWood];
                        }
                        else if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[3]) < 0.01f)
                        {
                            itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[4];
                        }
                        break;
                    //随机生成水资源
                    case ResourceType.Water:
                        int randomWater = Random.Range(0, 2);
                        itemSprite.transform.localPosition = new Vector3(0, 0.5f, 0);
                        itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[randomWater];
                        if (randomWater == 0)
                        {
                            itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 2, itemSprite.transform.localScale.z, itemSprite.transform.localScale.y);
                        }
                        else
                        {
                            itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x, itemSprite.transform.localScale.z, itemSprite.transform.localScale.y);
                        }
                        break;
                }
                break;
            case ItemType.Building:
                itemSprite.transform.localPosition = new Vector3(0, 0.52f, 0);
                itemScript.itemType = ItemType.Building;
                itemSprite.material = BuildingSystem.Instance.buildingDatas[(int)buildingType].buildingMaterial;
                item.name = BuildingSystem.Instance.buildingDatas[(int)buildingType].name;
                break;
            case ItemType.Character:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.505f, 0);
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

    //调试用初始化
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
