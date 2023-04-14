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
    public int refreshTime = 60;  //资源的刷新时间
    private float time;
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
        if (this.GetComponent<Item>().itemType == ItemType.Resource )
        {
            time += Time.deltaTime;
            if (time > refreshTime)
            {
                ResourceSystem.Instance.RegenerationResource(resourceType, isNature);
                ResourceSystem.Instance.DeleteResource(this.gameObject);
                time = 0;
            }
        }
        if (!isResource) { return; }

        if ((resourceType == ResourceType.Oil) && (EraSystem.Instance.era != Era.IndustrialEra))
        {
            canBeGathered = false;
        }
    }
}
