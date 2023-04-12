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
    private float time;
    public int population;
    public MeshRenderer itemSprite;
    public BuildingType buildingType = BuildingType.Cave;
    public bool stopGenerate = false;
    public int generatePopulation;
    public GameObject oil;
    public float oilTime;
    // Start is called before the first frame update
    void Start()
    {
        itemSprite = transform.Find("ItemSprite").GetComponent<MeshRenderer>();
        if (GetComponent<Item>().itemType == ItemType.Building)
        {
            isBuilding = true;
            population = Random.Range(1, 5);
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



    }

    void GeneratePopulation()
    {
        if (stopGenerate) { return; }
        time += Time.deltaTime;
        if (time > 60f / population)
        {
            time = 0;
            CreateController.Instance.CreateItem(ItemType.Character, null, null, SpecialSkill.None, transform.eulerAngles);
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
            }
        }
    }
}
