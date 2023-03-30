using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform center = null;
    public int blockNum = 0;
    public float latitude = 0f;
    public float longitude = 0f;
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Planet").transform;
        blockNum = BlockSystem.Instance.GetBlockNum(center.position, this.transform.GetChild(0).position);
        latitude = BlockSystem.Instance.GetLatitude(center.position, this.transform.GetChild(0).position);
        longitude = BlockSystem.Instance.GetLongitude(center.position, this.transform.GetChild(0).position);
    }

    // Update is called once per frame
    void Update()
    {
        blockNum = BlockSystem.Instance.GetBlockNum(center.position, this.transform.GetChild(0).position);
        latitude = BlockSystem.Instance.GetLatitude(center.position, this.transform.GetChild(0).position);
        longitude = BlockSystem.Instance.GetLongitude(center.position, this.transform.GetChild(0).position);
    }
}
