﻿using System.Collections;
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
    // Start is called before the first frame update
    void Start()
    {
        itemSprite = transform.Find("ItemSprite").GetComponent<MeshRenderer>();
        if (GetComponent<Item>().itemType == ItemType.Building)
        {
            isBuilding = true;
            switch (EraSystem.Instance.era)
            {
                case Era.AncientEra:
                    population = Random.Range(1, 5);
                    break;
                case Era.ClassicalEra:
                    population = Random.Range(5, 8);
                    break;
                case Era.IndustrialEra:
                    population = Random.Range(6, 10);
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
        GeneratePopulation();
        InstantiateOil();
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
    }

    void GeneratePopulation()
    {
        if ((buildingType == BuildingType.Cave) && (CharacterSystem.Instance.GetPopulation() < 8))
        {
            time += Time.deltaTime;
            if (time > 20f)
            {
                time = 0;
                CreateController.Instance.CreateItem(ItemType.Character, null, null, CharacterSystem.Instance.GetRandomSkill(), transform.eulerAngles);
            }
        }
        if ((buildingType == BuildingType.Cave) && (CharacterSystem.Instance.GetPopulation() < BuildingSystem.Instance.GetBuildingNums()))
        {
            Debug.Log("建筑多于人数 ");
            secondTime += Time.deltaTime;
            if (secondTime > 5f)
            {
                secondTime = 0;
                CreateController.Instance.CreateItem(ItemType.Character, null, null, CharacterSystem.Instance.GetRandomSkill(), transform.eulerAngles);
            }
        }
        if (stopGenerate) { return; }
        time += Time.deltaTime;
        if ((time > 60f / population) && (CharacterSystem.Instance.GetPopulation() < 50))
        {
            time = 0;
            CreateController.Instance.CreateItem(ItemType.Character, null, null, CharacterSystem.Instance.GetRandomSkill(), transform.eulerAngles);
            generatePopulation++;
        }
        if (generatePopulation > population)
        {
            stopGenerate = true;
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
}
