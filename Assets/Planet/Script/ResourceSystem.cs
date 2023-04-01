using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{
    private float time;
    public int[] resourceInBlock = new int[25];
    public static ResourceSystem Instance = null;
    public float regenerationDuration;
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
        resourceInBlock = new int[25];
        for (int i = 0; i <= 12; i++)
        {
            CreateController.Instance.CreateItem(CreateController.ItemName.Shrub);
        }
        for (int i = 0; i <= 12; i++)
        {
            CreateController.Instance.CreateItem(CreateController.ItemName.Tree);
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

    public int MinResourceBlock()
    {
        int minResource = 10000;
        int minNum = 0;
        for (int i = 1; i <= 24; i++)
        {
            if (minResource > resourceInBlock[i])
            {
                minResource = resourceInBlock[i];
                minNum = i;
            }
        }
        return minNum;
    }

    public void GatherResource(GameObject resourceObject)
    {
        int blockNum = resourceObject.GetComponent<Item>().blockNum;
        resourceInBlock[blockNum]--;
        DeleteResource(resourceObject);
        RegenerationResource(new int[] { blockNum });
    }

    public void DeleteResource(GameObject resourceObject)
    {
        Destroy(resourceObject);
    }

    public void RegenerationResource(int[] blockNum)
    {
        Debug.Log("RegenerationResource: Block[] " + blockNum);
        CreateController.Instance.CreateItem((CreateController.ItemName)Random.Range(8, 10), blockNum);
    }
}
