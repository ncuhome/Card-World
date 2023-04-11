using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuildingData
{
    public string name;
    public BuildingType buildingType;
    public Era era;
    public ResourceData[] targetResource = new ResourceData[14];
    public bool isSpecialBuilding;
    public bool isHomeBuilding;
    public Material buildingMaterial;
    public bool canBuild;
}
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance = null;
    public int size;
    public BuildingData[] buildingDatas = new BuildingData[30];
    public int[] builders;
    public Building[] buildings = new Building[120];
    public int[] buildingInBlock = new int[24];
    public int maxBuildingInBlock;
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
        BuildingData[] buildingsCanBeBuild = new BuildingData[30];
        int buildingsCanBeBuildNum = 0;
        for (int i = 0; i < 30; i++)
        {
            buildingDatas[i].canBuild = true;
            if (EraSystem.Instance.era != buildingDatas[i].era) { buildingDatas[i].canBuild = false; }
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
        if (buildingsCanBeBuildNum == 0) { return; }
        int buildingRandomNum = Random.Range(0, buildingsCanBeBuildNum);
        builders = CharacterSystem.Instance.GetBuilders(size);
        if (builders != null)
        {
            Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            while ((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f)
                || (CharacterSystem.Instance.characters[builders[0]].item.blockNum != BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler))))
            {
                targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            }
            int buildingNum = GetBuildingNum();
            CreateController.Instance.CreateItem(ItemType.Building, null, buildingsCanBeBuild[buildingRandomNum].buildingType, null, targetEuler);
            buildings[buildingNum] = GameObject.Find("Items").transform.GetChild(GameObject.Find("Items").transform.childCount - 1).GetComponent<Building>();
            buildings[buildingNum].transform.localScale = Vector3.zero;
            buildings[buildingNum].finishBuilding = false;
            buildingInBlock[CharacterSystem.Instance.characters[builders[0]].item.blockNum]++;

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


    private int GetBuildingNum()
    {
        int i = 0;
        while (buildings[i] != null)
        {
            i++;
        }
        return i;
    }

    public Building FindNearBuilding(Transform itemTransform)
    {
        Building nearBuilding = null;
        float minAngle = 3600f;
        foreach (Building building in buildings)
        {
            if (building == null) { continue; }
            float angle = Vector3.Angle(itemTransform.rotation * Vector3.up, building.transform.rotation * Vector3.up);
            if (angle < minAngle)
            {
                minAngle = angle;
                nearBuilding = building;
            }
        }
        return nearBuilding;
    }

    public void EndOfCivilization()
    {
        // for (int i = 0; i < 120; i++)
        // {
        //     if (buildings[i] == null) { continue; }
        //     if (buildings[i].specialBuilding == SpecialBuilding.none)
        //     {
        //         Destroy(buildings[i].gameObject);
        //         buildings[i] = null;
        //     }
        //     else
        //     {
        //         if (Random.Range(0f,1f) < 0.3f)
        //         {
        //             Destroy(buildings[i].gameObject);
        //             buildings[i] = null;
        //         }
        //     }
        // }
    }

}
