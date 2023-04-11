using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollider : MonoBehaviour
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (item.character.foundResource || item.character.goToBuild || item.character.goHome) { return; }
        switch (transform.tag)
        {
            case "Character":
                if ((other.tag == "Resource") && (!other.transform.parent.parent.GetComponent<Resources>().isGathering))
                {
                    int targetBlock = BlockSystem.Instance.GetBlockNum(item.center.position, other.GetComponent<ItemCollider>().item.transform.rotation);
                    if (item.character.RoadCanMove(targetBlock))
                    {
                        item.character.foundResource = true;
                        other.transform.parent.parent.GetComponent<Resources>().isGathering = true;
                        item.character.resourceObject = other.GetComponent<ItemCollider>().item.gameObject;
                        //Debug.Log("resourceRotation:" + other.GetComponent<ItemCollider>().item.transform.rotation);
                        item.character.WalkToTargetQua(other.GetComponent<ItemCollider>().item.transform.rotation);
                    }
                }
                break;
            case "Building":
                break;
            case "Resource":
                break;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (item.character.foundResource || item.character.goToBuild || item.character.goHome) { return; }
        switch (transform.tag)
        {
            case "Character":
                if ((other.tag == "Resource") && (!other.transform.parent.parent.GetComponent<Resources>().isGathering))
                {
                    int targetBlock = BlockSystem.Instance.GetBlockNum(item.center.position, other.GetComponent<ItemCollider>().item.transform.rotation);
                    if (item.character.RoadCanMove(targetBlock))
                    {
                        item.character.foundResource = true;
                        other.transform.parent.parent.GetComponent<Resources>().isGathering = true;
                        item.character.resourceObject = other.GetComponent<ItemCollider>().item.gameObject;
                        //Debug.Log("resourceRotation:" + other.GetComponent<ItemCollider>().item.transform.rotation);
                        item.character.WalkToTargetQua(other.GetComponent<ItemCollider>().item.transform.rotation);
                    }
                }
                break;
            case "Building":
                break;
            case "Resource":
                break;
        }
    }
}
