using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public float temperature = 20f;//温度

    public float water = 20;      //水量

    public float livability = 50; //宜居度
}

public class BlockSystem : MonoBehaviour
{
    public static BlockSystem Instance = null;

    public Block[] blocks = new Block[24];
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //求纬度（返回-90到90的float）
    public float GetLatitude(Vector3 centerPos, Vector3 pointPos)
    {
        float radius = Vector3.Distance(centerPos, pointPos);
        return Mathf.Asin((pointPos.y - centerPos.y) / radius) * Mathf.Rad2Deg;//求纬度，并转成弧度
    }

    //求经度（返回-180到180的float）
    public float GetLongitude(Vector3 centerPos, Vector3 pointPos)
    {
        return Mathf.Atan2(pointPos.z - centerPos.z, pointPos.x - centerPos.x) * Mathf.Rad2Deg;//求经度
    }

    //获取区块编号（1-24）
    public int GetBlockNum(Vector3 centerPos, Vector3 pointPos)
    {
        float latitude = GetLatitude(centerPos, pointPos);
        float longitude = GetLongitude(centerPos, pointPos);
        int blockX = (int)Mathf.Floor(longitude / 60) + 3;
        int blockY = (int)Mathf.Floor(latitude / 45) + 2;
        return ((blockY * 6) + blockX);
    }
    //用四元数代表方向向量获取区块坐标
    public int GetBlockNum(Vector3 centerPos, Quaternion pointQua)
    {
        Vector3 pointPos = (pointQua * Vector3.up).normalized;
        return GetBlockNum(centerPos, pointPos);
    }

    //获取当前区块的周围区块（包括自身
    public int[] GetNearBlock(Vector3 centerPos, int blockNum)
    {
        int[] nearBlock = new int[] { };
        int blockY = blockNum / 6;
        int blockX = blockNum - blockY * 6;
        if (blockY == 0)
        {
            nearBlock = new int[4] { blockY * 6 + (blockX - 1) % 6, blockNum, blockY * 6 + (blockX + 1) % 6, blockNum + 6 };
        }
        else if ((blockY == 1) || (blockY == 2))
        {
            nearBlock = new int[5] { blockNum - 6, blockY * 6 + (blockX - 1) % 6, blockNum, blockY * 6 + (blockX + 1) % 6, blockNum + 6 };
        }
        else
        {
            nearBlock = new int[4] { blockNum - 6, blockY * 6 + (blockX - 1) % 6, blockNum, blockY * 6 + (blockX + 1) % 6 };
        }
        return nearBlock;
    }

    public int[] GetRandomNearBlock(Vector3 centerPos, int blockNum)
    {
        int[] nearBlock = new int[] { };
        int[] randomNearBlock = new int[] { };
        int blockY = blockNum / 6;
        int blockX = blockNum - blockY * 6;

        if (blockY == 0)
        {
            nearBlock = new int[3] { blockY * 6 + (blockX - 1) % 6, blockY * 6 + (blockX + 1) % 6, blockNum + 6 };
            randomNearBlock = new int[2] { blockNum, nearBlock[Random.Range(0, 3)] };
        }
        else if ((blockY == 1) || (blockY == 2))
        {
            nearBlock = new int[4] { blockNum - 6, blockY * 6 + (blockX - 1) % 6, blockY * 6 + (blockX + 1) % 6, blockNum + 6 };
            randomNearBlock = new int[2] { blockNum, nearBlock[Random.Range(0, 4)] };
        }
        else
        {
            nearBlock = new int[3] { blockNum - 6, blockY * 6 + (blockX - 1) % 6, blockY * 6 + (blockX + 1) % 6 };
            randomNearBlock = new int[2] { blockNum, nearBlock[Random.Range(0, 3)] };
        }
        return randomNearBlock;
    }
    public IEnumerator NauAffectBlock(float tem, float water, float livability, float theTime, GameObject thisCard,int[] block)
    {
        float allTime = 0;
        float time = 0;
        while (true)
        {
            
            Debug.Log("正在完成效果");
            allTime += Time.deltaTime;
            time += Time.deltaTime;
            if (time > 1)
            {
                BlockSystem.Instance.blocks[block[0]].temperature += tem;
                BlockSystem.Instance.blocks[block[0]].water += water;
                BlockSystem.Instance.blocks[block[0]].livability += livability;
                time -= 1;
            }
            if (allTime > theTime)
            {
                break;
            }
            yield return null;
        }
        Destroy(thisCard);
    }
}
