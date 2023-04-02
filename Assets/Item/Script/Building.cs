using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private bool isBuilding;
    public bool finishBuilding;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Item>().itemType == Item.ItemType.Building)
        {
            isBuilding = true;
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
    }
}
