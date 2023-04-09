using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Cave, GrassHouse, SheepPen, FishingFacility, OriginalFarmland, SmallDock, Stonehenge, PotteryTurntable, TheFire, School,
    StoneHouse, LongHouse, UrbanHousing, Pier, Tower, Farm, AlchemyInstitute, Castle, LargeFleet, SmallMetalWorkshop,
    ConcreteBuilding, Villa, CourtyardHouse, Observatory, ModernFarm, RocketLaunchBase, MetalSmelter, NuclearPowerPlant,SteamCastle,GliderAirports
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
        SwitchImage();
        if (stopGenerate) { return; }
        time += Time.deltaTime;
        if (time > 60f / population)
        {
            time = 0;
            CreateController.Instance.CreateItem(ItemName.Naked, transform.eulerAngles);
            generatePopulation++;
        }
        if (generatePopulation > population)
        {
            stopGenerate = true;
        }
    }

    public void SwitchImage()
    {

    }
}
