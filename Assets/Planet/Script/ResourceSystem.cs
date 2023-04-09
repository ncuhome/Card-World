﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{
    private float time;
    public int[] resourceInBlock = new int[24];
    public static ResourceSystem Instance = null;
    public float regenerationDuration;
    public int resourceNum;
    public GameObject center;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance ResourceSystem");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        resourceInBlock = new int[24];
        for (int i = 0; i <= 12; i++)
        {
            CreateController.Instance.CreateItem(ItemName.Shrub);
        }
        for (int i = 0; i <= 12; i++)
        {
            CreateController.Instance.CreateItem(ItemName.Tree);
        }

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > regenerationDuration)
        {
            time = 0;
            RegenerationResource(new int[1] { MinResourceBlock() });
        }
    }

    //获取资源最少的区块
    public int MinResourceBlock()
    {
        int minResource = 10000;
        int minNum = 0;
        for (int i = 0; i < 24; i++)
        {
            if (minResource > resourceInBlock[i])
            {
                minResource = resourceInBlock[i];
                minNum = i;
            }
        }
        return minNum;
    }

    //获取资源
    public void GatherResource(Character character,GameObject resourceObject)
    {
        int blockNum = resourceObject.GetComponent<Item>().blockNum;
        resourceInBlock[blockNum]--;
        resourceNum ++;
        DeleteResource(resourceObject);
        RegenerationResource(BlockSystem.Instance.GetNearBlock(center.transform.position,blockNum));
    }

    //删除资源
    public void DeleteResource(GameObject resourceObject)
    {
        Destroy(resourceObject);
    }

    public void RegenerationResource(int[] blockNum)
    {
        //Debug.Log("RegenerationResource: Block[] " + blockNum);
        CreateController.Instance.CreateItem((ItemName)Random.Range(8, 10), blockNum);
    }

    
}
