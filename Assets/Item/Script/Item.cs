using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Character, Building, Resource }
    public Transform center = null;
    public ItemType itemType;
    public int blockNum = 0;
    public float latitude = 0f;
    public float longitude = 0f;
    public CapsuleCollider itemCollider;
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Planet").transform;
        blockNum = BlockSystem.Instance.GetBlockNum(center.position, this.transform.GetChild(0).position);
        latitude = BlockSystem.Instance.GetLatitude(center.position, this.transform.GetChild(0).position);
        longitude = BlockSystem.Instance.GetLongitude(center.position, this.transform.GetChild(0).position);


        //分配tag
        switch (itemType)
        {
            case ItemType.Character:
                ChangeAllTag(transform,"Character");
                break;
            case ItemType.Building:
                ChangeAllTag(transform,"Building");
                break;
            case ItemType.Resource:
                ChangeAllTag(transform,"Resource");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        blockNum = BlockSystem.Instance.GetBlockNum(center.position, this.transform.GetChild(0).position);
        latitude = BlockSystem.Instance.GetLatitude(center.position, this.transform.GetChild(0).position);
        longitude = BlockSystem.Instance.GetLongitude(center.position, this.transform.GetChild(0).position);
    }

    //递归改变所有子物体的tag
    private void ChangeAllTag(Transform currGameObject, string tag)
    {
        currGameObject.tag = tag;
        if (currGameObject.childCount == 0)
        {
            return;
        }
        for (int i = 0; i < currGameObject.childCount; i++)
        {
            ChangeAllTag(currGameObject.GetChild(i), tag);
        }
    }
}
