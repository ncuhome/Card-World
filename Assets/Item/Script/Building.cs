using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Cave, GrassHouse, SheepPen, FishingFacility, OriginalFarmland, SmallDock, Stonehenge, PotteryTurntable, TheFire, School,
    StoneHouse, LongHouse, UrbanHousing, Pier, Tower, Farm, AlchemyInstitute, Castle, LargeFleet, SmallMetalWorkshop,
    ConcreteBuilding, Villa, CourtyardHouse, Observatory, ModernFarm, RocketLaunchBase, MetalSmelter, NuclearPowerPlant, SteamCastle, GliderAirports,
    OilVent
}
public class Building : MonoBehaviour
{
    private bool isBuilding;
    public bool finishBuilding;
    private float time, secondTime;
    public int population;
    public MeshRenderer itemSprite;
    public BuildingType buildingType = BuildingType.Cave;
    public bool stopGenerate = false;
    public int generatePopulation;
    public GameObject oil;
    public float oilTime;
    public GameObject farm;
    public GameObject lake;
    public int maintenanceTimes;
    public int maxProduceTimes;
    public int maxMaintenanceTimes;
    public int produceTimes;
    public int produceGrainNum = 5;
    public bool waitForMaintenance = false;
    public float generateMultiplier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        itemSprite = transform.Find("ItemSpriteCenter").Find("ItemSprite").GetComponent<MeshRenderer>();
        if (GetComponent<Item>().itemType == ItemType.Building)
        {
            isBuilding = true;
            switch (EraSystem.Instance.era)
            {
                case Era.AncientEra:
                    population = UnityEngine.Random.Range(1, 5);
                    break;
                case Era.ClassicalEra:
                    population = UnityEngine.Random.Range(5, 8);
                    break;
                case Era.IndustrialEra:
                    population = UnityEngine.Random.Range(6, 10);
                    break;
            }
        }
        else
        {
            isBuilding = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBuilding) { return; }
        if ((buildingType == BuildingType.FishingFacility) || (buildingType == BuildingType.SmallDock))
        {
            lake.SetActive(true);
        }
        else
        {
            lake.SetActive(false);
        }
        if (buildingType == BuildingType.Farm)
        {
            farm.SetActive(true);
        }
        else
        {
            farm.SetActive(false);
        }

        if (!stopGenerate)
        {
            maxProduceTimes = 2;
            maxMaintenanceTimes = 2;
        }
        if (BuildingSystem.Instance.buildingDatas[(int)buildingType].isGrainBuilding)
        {
            maxProduceTimes = 10;
            if ((buildingType == BuildingType.FishingFacility)||(buildingType == BuildingType.Pier))
            {
                maxMaintenanceTimes = 3;
            }
            else
            {
                maxMaintenanceTimes = 4;
            }
        }

        if (CivilizationScaleSystem.Instance.civilizationScaleOverFlow)
        {
            generateMultiplier = 1.5f;
        }
        else
        {
            generateMultiplier = 1f;
        }
        GeneratePopulation();
        InstantiateOil();
        ProduceGrain();
        WaitForMaintenance();
    }

    void GeneratePopulation()
    {
        if ((buildingType == BuildingType.Cave) && (CharacterSystem.Instance.GetPopulation() < 8))
        {
            time += Time.deltaTime;
            if (time > 20f * generateMultiplier)
            {
                time = 0;
                CreateController.Instance.CreateItem(ItemType.Character, null, null, CharacterSystem.Instance.GetRandomSkill(), transform.eulerAngles);
            }
        }
        if ((buildingType == BuildingType.Cave) && (CharacterSystem.Instance.GetPopulation() < BuildingSystem.Instance.GetBuildingNums()))
        {
            Debug.Log("建筑多于人数 ");
            secondTime += Time.deltaTime;
            if (secondTime > 2f - 0.01 * BuildingSystem.Instance.GetBuildingNums())
            {
                secondTime = 0;
                CreateController.Instance.CreateItem(ItemType.Character, null, null, CharacterSystem.Instance.GetRandomSkill(), transform.eulerAngles);
            }
        }
        if (stopGenerate) { return; }
        if (waitForMaintenance) { return; }
        time += Time.deltaTime;
        if ((time > 60f * generateMultiplier / (population- Convert.ToInt32(CivilizationScaleSystem.Instance.civilizationScaleOverFlow))) && (CharacterSystem.Instance.GetPopulation() < 50))
        {
            time = 0;
            CreateController.Instance.CreateItem(ItemType.Character, null, null, CharacterSystem.Instance.GetRandomSkill(), transform.eulerAngles);
            generatePopulation++;
        }
        if (generatePopulation > (population- Convert.ToInt32(CivilizationScaleSystem.Instance.civilizationScaleOverFlow)))
        {
            stopGenerate = true;
            produceTimes++;
        }
    }

    void InstantiateOil()
    {
        if (buildingType != BuildingType.OilVent) { return; }
        if (oil == null)
        {
            oilTime += Time.deltaTime;
            if (oilTime > 20f)
            {
                CreateController.Instance.CreateItem(ItemType.Resource, ResourceType.Oil, null, null, transform.eulerAngles);
                oil = transform.parent.GetChild(transform.parent.childCount - 1).gameObject;
                oilTime = 0;
            }
        }
    }

    void ProduceGrain()
    {
        if (!BuildingSystem.Instance.buildingDatas[(int)buildingType].isGrainBuilding) { return; }
        if (waitForMaintenance) { return; }
        time += Time.deltaTime;
        if (time > 10f)
        {
            ResourceSystem.Instance.grainNum += produceGrainNum;
            produceTimes++;
            time = 0f;
        }
    }

    void WaitForMaintenance()
    {
        if (produceTimes >= maxProduceTimes)
        {
            waitForMaintenance = true;
            if ((maintenanceTimes > maxMaintenanceTimes)&&(BuildingSystem.Instance.buildingDatas[(int)buildingType].isGrainBuilding))
            {
                BuildingSystem.Instance.buildingInBlock[BlockSystem.Instance.GetBlockNum(Vector3.zero,transform.position)] --;
                DestroyImmediate(this.gameObject);
            }
            for (int j = 0; j < 13; j++)
            {
                if (ResourceSystem.Instance.resourceDatas[j].resourceNum < (BuildingSystem.Instance.buildingDatas[(int)buildingType].targetResource[j].resourceNum + Convert.ToInt32(CivilizationScaleSystem.Instance.civilizationScaleOverFlow)))
                {
                    waitForMaintenance = false;
                    produceTimes = 0;
                    maintenanceTimes ++;
                }
            }
        }

    }
}
