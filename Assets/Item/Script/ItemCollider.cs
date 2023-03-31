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
        if (item.character.foundResource) { return; }
        switch (transform.tag)
        {
            case "Character":
                if (other.tag == "Resource")
                {
                    item.character.resourceObject = other.GetComponent<ItemCollider>().item.gameObject;
                    item.character.WalkToTargetEuler(other.GetComponent<ItemCollider>().item.transform.eulerAngles);
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
        if (item.character.foundResource) { return; }
        switch (transform.tag)
        {
            case "Character":
                if (other.tag == "Resource")
                {
                    item.character.resourceObject = other.GetComponent<ItemCollider>().item.gameObject;
                    item.character.WalkToTargetEuler(other.GetComponent<ItemCollider>().item.transform.eulerAngles);
                }
                break;
            case "Building":
                break;
            case "Resource":
                break;
        }
    }
}
