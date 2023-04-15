using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuildingData //构建可序列化的数据类
{
    public string name;
    public BuildingType buildingType;
    public Era era; //需求时代
    public ResourceData[] targetResource = new ResourceData[14];
    public bool isSpecialBuilding;
    public bool isHomeBuilding;
    public Material buildingMaterial;
    public bool canBuild; //当前是否可以建造该种建筑
    public string[] needTech;
    public BuildingType[] preBuildings;
}
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance = null;
    public int size; //需求人数
    public int builderAge;
    public BuildingData[] buildingDatas = new BuildingData[30]; //构建建筑的数据库
    public int[] builders;
    public Building[] buildings = new Building[120];
    public int[] buildingInBlock = new int[24];
    public int maxBuildingInBlock;
    public int guarantees;
    // Start is called before the first frame update
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Instance BuildingSystem");
            Instance = this;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (EraSystem.Instance.era)
        {
            case Era.AncientEra:
                builderAge = 0;
                size = 2;
                break;
            case Era.ClassicalEra:
                builderAge = 20;
                size = 4;
                break;
            case Era.IndustrialEra:
                builderAge = 40;
                size = 5;
                break;
        }

        //寻找可建造的建筑
        BuildingData[] buildingsCanBeBuild = new BuildingData[30];
        int buildingsCanBeBuildNum = 0;
        for (int i = 0; i < 30; i++)
        {
            buildingDatas[i].canBuild = true;
            if (EraSystem.Instance.era != buildingDatas[i].era) { buildingDatas[i].canBuild = false; }

            foreach (string techNode in buildingDatas[i].needTech)
            {
                if (techNode == null) { continue; }
                if (!TechTree.instance.GetWhetherUnlocked(techNode))
                {
                    buildingDatas[i].canBuild = false;
                }
            }

            for (int j = 0; j < 13; j++)
            {
                if (ResourceSystem.Instance.resourceDatas[j].resourceNum < buildingDatas[i].targetResource[j].resourceNum) { buildingDatas[i].canBuild = false; }
            }
            if (buildingDatas[i].canBuild)
            {
                buildingsCanBeBuild[buildingsCanBeBuildNum] = buildingDatas[i];
                buildingsCanBeBuildNum++;
            }
        }

        //如果有可建造的建筑，就开始寻找工人
        if (buildingsCanBeBuildNum == 0) { return; }
        builders = CharacterSystem.Instance.GetBuilders(size);

        //获取了工人则开始建造
        if (builders != null)
        {
            int buildingRandomNum = Random.Range(0, buildingsCanBeBuildNum);

            //保底出居住建筑
            // if (!buildingsCanBeBuild[buildingRandomNum].isHomeBuilding)
            // {
            //     guarantees++;
            //     if (guarantees > 2f)
            //     {
            //         while (!buildingsCanBeBuild[buildingRandomNum].isHomeBuilding)
            //         {
            //             buildingRandomNum = Random.Range(0, buildingsCanBeBuildNum);
            //         }
            //         guarantees = 0;
            //     }
            // }
            // else
            // {
            //     guarantees = 0;
            // }


            Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            while (((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f) && (buildingsCanBeBuild[buildingRandomNum].buildingType != BuildingType.LargeFleet))
                || ((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) > 0.01f) && (buildingsCanBeBuild[buildingRandomNum].buildingType == BuildingType.LargeFleet))
                || (CharacterSystem.Instance.characters[builders[0]].item.blockNum != BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler)))
                || (FindNearAngle(Quaternion.Euler(targetEuler)) < 2f))
            {
                targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            }
            int buildingNum = GetBuildingNum();
            CreateController.Instance.CreateItem(ItemType.Building, null, buildingsCanBeBuild[buildingRandomNum].buildingType, null, targetEuler);
            BuildingSystem.Instance.buildings[buildingNum] = GameObject.Find("Items").transform.GetChild(GameObject.Find("Items").transform.childCount - 1).GetComponent<Building>();
            buildings[buildingNum].transform.localScale = Vector3.zero;
            buildings[buildingNum].finishBuilding = false;
            buildingInBlock[CharacterSystem.Instance.characters[builders[0]].item.blockNum]++;
            if (!buildingDatas[(int)buildings[buildingNum].buildingType].isHomeBuilding) { buildings[buildingNum].stopGenerate = true; }

            //减少建造需求的对应资源
            for (int j = 0; j < 13; j++)
            {
                ResourceSystem.Instance.resourceDatas[j].resourceNum -= buildingsCanBeBuild[buildingRandomNum].targetResource[j].resourceNum;
            }
            foreach (int builderNum in builders)
            {
                if (builderNum < 0) { continue; }
                CharacterSystem.Instance.characters[builderNum].goToBuild = true;
                CharacterSystem.Instance.characters[builderNum].WalkToTargetQua(Quaternion.Euler(targetEuler));
                CharacterSystem.Instance.characters[builderNum].buildingObject = buildings[buildingNum].gameObject;
            }
        }
    }

    //获取最小的建筑空位
    public int GetBuildingNum()
    {
        int i = 0;
        while (buildings[i] != null)
        {
            i++;
        }
        return i;
    }

    public float FindNearAngle(Quaternion itemQua)
    {
        float minAngle = 3600f;
        foreach (Building building in buildings)
        {
            if (building == null) { continue; }
            //Debug.Log(building.name + " " + building.buildingType + " " + buildingDatas[(int)building.buildingType].isHomeBuilding);
            float angle = Vector3.Angle(itemQua * Vector3.up, building.transform.rotation * Vector3.up);
            if (angle < minAngle)
            {
                minAngle = angle;
            }
        }
        return minAngle;
    }

    //寻找最近的居住建筑
    public Building FindNearHome(Transform itemTransform)
    {
        Building nearHome = null;
        float minAngle = 3600f;
        foreach (Building building in buildings)
        {
            if (building == null) { continue; }
            //Debug.Log(building.name + " " + building.buildingType + " " + buildingDatas[(int)building.buildingType].isHomeBuilding);
            if (!buildingDatas[(int)building.buildingType].isHomeBuilding) { continue; }
            float angle = Vector3.Angle(itemTransform.rotation * Vector3.up, building.transform.rotation * Vector3.up);
            if (angle < minAngle)
            {
                minAngle = angle;
                nearHome = building;
            }
        }
        return nearHome;
    }

    public Building FindNearBuildingWithType(Transform itemTransform, BuildingType buildingType)
    {
        Building nearBuilding = null;
        float minAngle = 3600f;
        foreach (Building building in buildings)
        {
            if (building == null) { continue; }
            //Debug.Log(building.name + " " + building.buildingType + " " + buildingDatas[(int)building.buildingType].isHomeBuilding);
            if (building.buildingType != buildingType) { continue; }
            float angle = Vector3.Angle(itemTransform.rotation * Vector3.up, building.transform.rotation * Vector3.up);
            if (angle < minAngle)
            {
                minAngle = angle;
                nearBuilding = building;
            }
        }
        return nearBuilding;
    }

    //文明终结
    public void EndOfCivilization()
    {
        for (int i = 0; i < 120; i++)
        {
            if (buildings[i] == null) { continue; }
            if (!buildingDatas[(int)buildings[i].buildingType].isSpecialBuilding)
            {
                Destroy(buildings[i].gameObject);
                buildings[i] = null;
            }
            else
            {
                if (Random.Range(0f, 1f) > 0.3f)
                {
                    Destroy(buildings[i].gameObject);
                    buildings[i] = null;
                }
            }
        }
    }

    //文明进阶
    public void CivilizationProgresses()
    {
        foreach (Building building in buildings)
        {
            if (building == null) { continue; }
            if (buildingDatas[(int)building.buildingType].isSpecialBuilding) { continue; }
            if (Random.value < 0.3f)
            {
                Destroy(building.gameObject);
            }
            BuildingData[] buildingsCanProgress = new BuildingData[30];
            int buildingsCanProgressNum = 0;
            for (int i = 0; i < 30; i++)
            {
                if (System.Array.IndexOf(buildingDatas[i].preBuildings, building.buildingType) != -1)
                {
                    buildingsCanProgress[buildingsCanProgressNum] = buildingDatas[i];
                    buildingsCanProgressNum++;
                }
            }
            if (buildingsCanProgressNum != 0)
            {
                int num = Random.Range(0, buildingsCanProgressNum);
                building.itemSprite.material = buildingsCanProgress[num].buildingMaterial;
                switch (buildingsCanProgress[num].buildingType)
                {
                    case BuildingType.Farm:
                        building.itemSprite.transform.localEulerAngles = new Vector3(90, 0, 0);
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.505f, 0);
                        break;
                    case BuildingType.OriginalFarmland:
                        building.itemSprite.transform.localEulerAngles = new Vector3(0, 0, 0);
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.5f, 0);
                        break;
                    case BuildingType.PotteryTurntable:
                    case BuildingType.UrbanHousing:
                    case BuildingType.LargeFleet:
                    case BuildingType.SmallMetalWorkshop:
                    case BuildingType.ModernFarm:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.516f, 0);
                        break;
                    case BuildingType.StoneHouse:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.51f, 0);
                        break;
                    case BuildingType.LongHouse:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.513f, 0);
                        break;
                    case BuildingType.AlchemyInstitute:
                    case BuildingType.ConcreteBuilding:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.52f, 0);
                        break;
                    case BuildingType.Villa:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                        break;
                    case BuildingType.CourtyardHouse:
                    case BuildingType.Observatory:
                    case BuildingType.GliderAirports:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.517f, 0);
                        break;
                    case BuildingType.MetalSmelter:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.519f, 0);
                        break;
                    case BuildingType.NuclearPowerPlant:
                        building.itemSprite.transform.localPosition = new Vector3(0, 0.514f, 0);
                        break;
                }
                building.buildingType = buildingsCanProgress[num].buildingType;
                building.name = buildingsCanProgress[num].name;
            }
        }
    }

    public int GetBuildingNums()  //得到目前建筑数量
    {
        int buildingNum = 0;
        foreach (Building building in Instance.buildings)
        {
            if (building != null) buildingNum++;
        }
        return buildingNum;
    }
}
