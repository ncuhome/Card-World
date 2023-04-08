using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpecialBuilding { none = -1 }
public class Building : MonoBehaviour
{
    private bool isBuilding;
    public bool finishBuilding;
    private float time;
    public int population;
    public MeshRenderer itemSprite;
    public SpecialBuilding specialBuilding = SpecialBuilding.none;
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
        if (specialBuilding == SpecialBuilding.none)
        {
            itemSprite.material = BuildingSystem.Instance.eraMaterials[(int)EraSystem.Instance.era];
        }
        else
        {
            itemSprite.material = BuildingSystem.Instance.specialMaterials[(int)specialBuilding];
        }
    }
}
