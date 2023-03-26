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
    }

    // Update is called once per frame
    void Update()
    {
        blockNum = BlockSystem.Instance.GetBlockNum(center, this.transform.GetChild(0));
        latitude = BlockSystem.Instance.GetLatitude(center, this.transform.GetChild(0));
        longitude = BlockSystem.Instance.GetLongitude(center, this.transform.GetChild(0));
    }
}
