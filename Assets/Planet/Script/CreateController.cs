﻿using DG.Tweening;
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
    public static float createTime = 0.5f; //物体从0变大的动画时间
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
    //生成自然资源 //指定类型，坐标随机
    public void CreateItem(ItemType itemType, ResourceType? resourceType, BuildingType? buildingType, SpecialSkill? characterSkill, bool nature)
    {
        Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        while (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f)
        {
            targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }

        CreateItem(itemType, resourceType, buildingType, characterSkill, targetEuler, nature);
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
    // //指定类型和区块创建自然资源
    public void CreateItem(ItemType itemType, ResourceType? resourceType, BuildingType? buildingType, SpecialSkill? characterSkill, int[] blockNum, bool nature)
    {
        Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        while ((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f)
                || (System.Array.IndexOf(blockNum, BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler))) == -1))
        {
            targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
        CreateItem(itemType, resourceType, buildingType, characterSkill, targetEuler, nature);
    }
    //指定类型和坐标
    public void CreateItem(ItemType itemType, ResourceType? resourceType, BuildingType? buildingType, SpecialSkill? characterSkill, Vector3 targetEuler)
    {
        GameObject item = Instantiate(itemPrefab);
        item.transform.SetParent(itemParent);
        item.transform.position = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.transform.eulerAngles = targetEuler;
        itemSprite = item.transform.Find("ItemSpriteCenter").Find("ItemSprite").GetComponent<MeshRenderer>();

        Item itemScript = item.GetComponent<Item>();
        Character character = item.GetComponent<Character>();
        Resources resources = item.GetComponent<Resources>();
        Building building = item.GetComponent<Building>();

        switch (itemType)
        {
            case ItemType.Resource:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
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
                        itemSprite.transform.localPosition = new Vector3(0, 0.499f, 0);
                        itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[randomWater];
                        if (randomWater == 0)
                        {
                            itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 2, itemSprite.transform.localScale.y, itemSprite.transform.localScale.z);
                            itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        break;
                    case ResourceType.Oil:
                        itemSprite.transform.localPosition = new Vector3(0, 0.499f, 0);
                        itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        break;
                    case ResourceType.GypsumStone:
                        itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                        break;
                }
                break;
            case ItemType.Building:
                itemSprite.transform.localPosition = new Vector3(0, 0.521f, 0);
                itemScript.itemType = ItemType.Building;
                itemSprite.material = BuildingSystem.Instance.buildingDatas[(int)buildingType].buildingMaterial;
                building.buildingType = (BuildingType)buildingType;
                item.name = BuildingSystem.Instance.buildingDatas[(int)buildingType].name;
                switch (buildingType)
                {
                    case BuildingType.OriginalFarmland:
                        itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        itemSprite.transform.localPosition = new Vector3(0, 0.5f, 0);
                        break;
                    case BuildingType.PotteryTurntable:
                    case BuildingType.UrbanHousing:
                    case BuildingType.LargeFleet:
                    case BuildingType.SmallMetalWorkshop:
                    case BuildingType.ModernFarm:
                        itemSprite.transform.localPosition = new Vector3(0, 0.516f, 0);
                        break;
                    case BuildingType.StoneHouse:
                        itemSprite.transform.localPosition = new Vector3(0, 0.51f, 0);
                        break;
                    case BuildingType.LongHouse:
                        itemSprite.transform.localPosition = new Vector3(0, 0.513f, 0);
                        break;
                    case BuildingType.AlchemyInstitute:
                    case BuildingType.ConcreteBuilding:
                        itemSprite.transform.localPosition = new Vector3(0, 0.52f, 0);
                        break;
                    case BuildingType.Villa:
                        itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                        break;
                    case BuildingType.CourtyardHouse:
                    case BuildingType.Observatory:
                    case BuildingType.GliderAirports:
                        itemSprite.transform.localPosition = new Vector3(0, 0.517f, 0);
                        break;
                    case BuildingType.MetalSmelter:
                        itemSprite.transform.localPosition = new Vector3(0, 0.519f, 0);
                        break;
                    case BuildingType.NuclearPowerPlant:
                        itemSprite.transform.localPosition = new Vector3(0, 0.514f, 0);
                        break;
                    case BuildingType.Magma:
                        itemSprite.transform.localPosition = new Vector3(0, 0.499f, 0);
                        itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        break;
                }
                break;
            case ItemType.Character:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                itemScript.itemType = ItemType.Character;
                character.characterNum = CharacterSystem.Instance.GetCharacterNum();
                character.specialSkill = (SpecialSkill)characterSkill;
                CharacterSystem.Instance.characters[CharacterSystem.Instance.GetCharacterNum()] = character;
                itemSprite.material = CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).characterMaterial;
                item.name = CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).name;
                switch (CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).characterNum)
                {
                    case 12:
                    case 13:
                    case 18:
                        itemSprite.transform.localPosition = new Vector3(0, 0.515f, 0);
                        break;
                    case 17:
                        itemSprite.transform.localPosition = new Vector3(0, 0.514f, 0);
                        break;
                }
                break;
        }
        itemSprite.transform.DOScale(new Vector3(0, 0, 0), createTime).From();
    }
    //指定类型和坐标创建自然资源
    public void CreateItem(ItemType itemType, ResourceType? resourceType, BuildingType? buildingType, SpecialSkill? characterSkill, Vector3 targetEuler, bool nature)
    {
        GameObject item = Instantiate(itemPrefab);
        item.transform.SetParent(itemParent);
        item.transform.position = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.transform.eulerAngles = targetEuler;
        itemSprite = item.transform.Find("ItemSpriteCenter").Find("ItemSprite").GetComponent<MeshRenderer>();

        Item itemScript = item.GetComponent<Item>();
        Character character = item.GetComponent<Character>();
        Resources resources = item.GetComponent<Resources>();
        Building building = item.GetComponent<Building>();

        switch (itemType)
        {
            case ItemType.Resource:

                item.GetComponent<Resources>().isNature = nature;

                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.521f, 0);
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
                        itemSprite.transform.localPosition = new Vector3(0, 0.499f, 0);
                        itemSprite.material = ResourceSystem.Instance.resourceDatas[(int)resourceType].resourceMaterials[randomWater];
                        if (randomWater == 0)
                        {
                            itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 2, itemSprite.transform.localScale.y, itemSprite.transform.localScale.z);
                            itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        break;
                    case ResourceType.Oil:
                        itemSprite.transform.localPosition = new Vector3(0, 0.499f, 0);
                        itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        break;
                    case ResourceType.GypsumStone:
                        itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                        break;
                }
                break;
            case ItemType.Building:
                itemSprite.transform.localPosition = new Vector3(0, 0.52f, 0);
                itemScript.itemType = ItemType.Building;
                itemSprite.material = BuildingSystem.Instance.buildingDatas[(int)buildingType].buildingMaterial;
                building.buildingType = (BuildingType)buildingType;
                item.name = BuildingSystem.Instance.buildingDatas[(int)buildingType].name;
                switch (buildingType)
                {
                    case BuildingType.OriginalFarmland:
                        itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        itemSprite.transform.localPosition = new Vector3(0, 0.5f, 0);
                        break;
                }
                switch (buildingType)
                {
                    case BuildingType.OriginalFarmland:
                        itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        itemSprite.transform.localPosition = new Vector3(0, 0.5f, 0);
                        break;
                    case BuildingType.PotteryTurntable:
                    case BuildingType.UrbanHousing:
                    case BuildingType.LargeFleet:
                    case BuildingType.SmallMetalWorkshop:
                    case BuildingType.ModernFarm:
                        itemSprite.transform.localPosition = new Vector3(0, 0.516f, 0);
                        break;
                    case BuildingType.StoneHouse:
                        itemSprite.transform.localPosition = new Vector3(0, 0.51f, 0);
                        break;
                    case BuildingType.LongHouse:
                        itemSprite.transform.localPosition = new Vector3(0, 0.513f, 0);
                        break;
                    case BuildingType.AlchemyInstitute:
                    case BuildingType.ConcreteBuilding:
                        itemSprite.transform.localPosition = new Vector3(0, 0.52f, 0);
                        break;
                    case BuildingType.Villa:
                        itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                        break;
                    case BuildingType.CourtyardHouse:
                    case BuildingType.Observatory:
                    case BuildingType.GliderAirports:
                        itemSprite.transform.localPosition = new Vector3(0, 0.517f, 0);
                        break;
                    case BuildingType.MetalSmelter:
                        itemSprite.transform.localPosition = new Vector3(0, 0.519f, 0);
                        break;
                    case BuildingType.NuclearPowerPlant:
                        itemSprite.transform.localPosition = new Vector3(0, 0.514f, 0);
                        break;
                }
                break;
            case ItemType.Character:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                itemScript.itemType = ItemType.Character;
                character.characterNum = CharacterSystem.Instance.GetCharacterNum();
                character.specialSkill = (SpecialSkill)characterSkill;
                CharacterSystem.Instance.characters[CharacterSystem.Instance.GetCharacterNum()] = character;
                itemSprite.material = CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).characterMaterial;
                item.name = CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).name;
                switch (CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, characterSkill).characterNum)
                {
                    case 12:
                    case 13:
                    case 18:
                        itemSprite.transform.localPosition = new Vector3(0, 0.515f, 0);
                        break;
                    case 17:
                        itemSprite.transform.localPosition = new Vector3(0, 0.514f, 0);
                        break;
                }
                break;
        }
        itemSprite.transform.DOScale(new Vector3(0, 0, 0), createTime).From();
    }
    //调试用初始化
    public void TestCreate()
    {
        int num = 13;
        for (int i = 0; i <= 3; i++)
        {
            CreateController.Instance.CreateItem(ItemType.Character, null, null, SpecialSkill.None, new int[] { num });
        }
        CreateController.Instance.CreateItem(ItemType.Character, null, null, SpecialSkill.Hunting, new int[] { num });
        CreateController.Instance.CreateItem(ItemType.Character, null, null, SpecialSkill.Farming, new int[] { num });
        CreateController.Instance.CreateItem(ItemType.Building, null, BuildingType.Cave, null, new int[] { num });
        BuildingSystem.Instance.buildings[0] = GameObject.Find("Items").transform.GetChild(GameObject.Find("Items").transform.childCount - 1).GetComponent<Building>();
        BuildingSystem.Instance.buildingInBlock[num]++;
        BuildingSystem.Instance.buildings[0].stopGenerate = true;
    }
}
