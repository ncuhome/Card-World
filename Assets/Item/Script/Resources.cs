using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    private bool isResource;
    public bool isGathering;
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
        if (!isResource) {return;}
    }
}
