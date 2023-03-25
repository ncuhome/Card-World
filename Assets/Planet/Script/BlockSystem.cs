using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    public static BlockSystem Instance = null;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    //求纬度（返回-90到90的float）
    private float GetLatitude(Transform center, Transform point)
    {
        float radius = Vector3.Distance(center.transform.position, point.transform.position);
        return Mathf.Asin((point.position.y - center.position.y) / radius) * Mathf.Rad2Deg;//求纬度，并转成弧度
    }

    //求经度（返回-180到180的float）
    private float GetLongitude(Transform center, Transform point)
    {
        return Mathf.Atan2(center.position.z, point.position.x) * Mathf.Rad2Deg;//求经度
    }

    public int GetBlockNum(Transform center, Transform point)
    {
        float latitude = GetLatitude(center, point);
        float longitude = GetLongitude(center, point);
        int blockX = (int)Mathf.Floor(longitude / 60) + 3;
        int blockY = (int)Mathf.Floor(latitude / 45) + 2;
        return ((blockY * 6) + blockX);
    }
}
