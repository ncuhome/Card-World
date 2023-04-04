using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private bool isBuilding;
    public bool finishBuilding;
    private float time;
    private int population;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Item>().itemType == Item.ItemType.Building)
        {
            isBuilding = true;
            population = Random.Range(1,5);
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
        time += Time.deltaTime;
        if (time > 60f / population)
        {
            time = 0;
            CreateController.Instance.CreateItem(CreateController.ItemName.Naked,transform.eulerAngles);
        }
    }
}
