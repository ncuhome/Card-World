using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType { Wood, Stone, Water, Porphyry, Flint, Clay, GypsumStone, Granite, IronOre, GoldOre, BronzeOre, CoalMine, TitaniumOre, Oil }
public class Resources : MonoBehaviour
{
    private bool isResource;
    public bool isGathering;
    public bool canBeGathered = true;
    public bool isNature;
    public ResourceType resourceType = ResourceType.Wood;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Item>().itemType == ItemType.Resource)
        {
            isResource = true;
        }
        else
        {
            isResource = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isResource) { return; }
        if ((resourceType == ResourceType.Oil) && (EraSystem.Instance.era != Era.IndustrialEra))
        {
            canBeGathered = false;
        }
    }
}
