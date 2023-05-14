using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResourceData
{
    public string name;
    public ResourceType resourceType;
    public int resourceNum;
    public Material[] resourceMaterials;
}
public class ResourceSystem : MonoBehaviour
{
    private float time;
    //public int[] resourceInBlock = new int[24];
    public static ResourceSystem Instance = null;
    public float regenerationDuration;
    public ResourceData[] resourceDatas = new ResourceData[14];
    public GameObject center;
    public int grainNum = 0;
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
        ResourceInitialization();
    }

    // Update is called once per frame
    void Update()
    {
        //InstantiateByTime();
    }

    //初始化资源
    public void ResourceInitialization()
    {
        time = 0;
        for (int i = 0; i < 12; i++)
        {
            int resourceNumInBlock = 6;
            for (int j = 0; j < resourceNumInBlock; j++)
            {
                float resourceTypeNum = Random.Range(0f, 1f);
                if (resourceTypeNum < 0.5f)
                {
                    CreateController.Instance.CreateItem(ItemType.Resource, ResourceType.Wood, null, null, new int[] { i }, true);
                }
                else if (resourceTypeNum < 0.8f)
                {
                    CreateController.Instance.CreateItem(ItemType.Resource, ResourceType.Stone, null, null, new int[] { i }, true);
                }
                else
                {
                    CreateController.Instance.CreateItem(ItemType.Resource, ResourceType.Water, null, null, new int[] { i }, true);
                }
            }
        }
        for (int i = 3; i < 13; i++)
        {
            Debug.Log((ResourceType)i);
            CreateController.Instance.CreateItem(ItemType.Resource, (ResourceType)i, null, null);
        }
    }
    //测试用，按时间生成资源
    public void InstantiateByTime()
    {
        time += Time.deltaTime;
        if (time > regenerationDuration)
        {
            time = 0;
            RegenerationResource(ResourceType.Wood);
            RegenerationResource(ResourceType.Stone);
            RegenerationResource(ResourceType.Water);
        }
    }

    //获取资源最少的区块
    // public int MinResourceBlock()
    // {
    //     int minResource = 10000;
    //     int minNum = 0;
    //     for (int i = 0; i < 24; i++)
    //     {
    //         if (minResource > resourceInBlock[i])
    //         {
    //             minResource = resourceInBlock[i];
    //             minNum = i;
    //         }
    //     }
    //     return minNum;
    // }

    //获取资源
    public void GatherResource(Character character, GameObject resourceObject)
    {
        int blockNum = resourceObject.GetComponent<Item>().blockNum;
        //resourceInBlock[blockNum]--;
        resourceDatas[(int)resourceObject.GetComponent<Resources>().resourceType].resourceNum++;
        DeleteResource(resourceObject);
        if (resourceObject.GetComponent<Resources>().isNature)
        {
            CreateController.Instance.CreateItem(ItemType.Resource, resourceObject.GetComponent<Resources>().resourceType, null, null, true);
        }
    }

    //删除资源
    public void DeleteResource(GameObject resourceObject)
    {
        Destroy(resourceObject);
    }

    //延迟重新添加资源
    public IEnumerator DelayRegeneration(ResourceType resourceType, float time)
    {
        yield return new WaitForSeconds(time);
        RegenerationResource(resourceType);
    }
    //延迟重新添加自然资源
    public IEnumerator DelayRegeneration(ResourceType resourceType, float time, bool nature)
    {
        yield return new WaitForSeconds(time);
        RegenerationResource(resourceType, nature);
    }

    //在指定区块生成指定资源
    public void RegenerationResource(ResourceType resourceType, int[] blockNum)
    {
        //Debug.Log("RegenerationResource: Block[] " + blockNum);
        CreateController.Instance.CreateItem(ItemType.Resource, resourceType, null, null, blockNum);
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[8]);
    }
    //在指定区块生成指定自然资源
    public void RegenerationResource(ResourceType resourceType, int[] blockNum, bool nature)
    {
        //Debug.Log("RegenerationResource: Block[] " + blockNum);
        CreateController.Instance.CreateItem(ItemType.Resource, resourceType, null, null, blockNum, nature);
    }
    //在随机区块生成指定资源
    public void RegenerationResource(ResourceType resourceType)
    {
        //Debug.Log("RegenerationResource: Block[] " + blockNum);
        CreateController.Instance.CreateItem(ItemType.Resource, resourceType, null, null);
    }
    //在随机区块生成指定自然资源
    public void RegenerationResource(ResourceType resourceType, bool nature)
    {
        //Debug.Log("RegenerationResource: Block[] " + blockNum);
        CreateController.Instance.CreateItem(ItemType.Resource, resourceType, null, null, nature);
    }


}
