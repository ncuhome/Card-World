using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateController : MonoBehaviour
{
    public enum ItemName
    {
        Army, Businessman, Farmer, Savages, Naked, City, NightCity, Pyramid, Shrub, Tree
    }
    public enum ItemClass
    {
        Character, Building, Resource
    }
    public static CreateController Instance = null;
    public GameObject itemPrefab;
    public Material[] materials = new Material[10];
    public Transform itemParent;
    public MeshRenderer itemSprite;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance CreateController");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 0; i++)
        {
            CreateItem(ItemName.Naked);
        }

        
       
        //CreateItem(ItemName.Farmer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //随机创建
    public void CreateItem()
    {
        ItemName itemName = (ItemName)Random.Range(0, 9);
        CreateItem(itemName);
    }


    //指定类型，坐标随机
    public void CreateItem(ItemName itemName)
    {
        Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        while (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.1f)
        {
            targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }

        CreateItem(itemName, targetEuler);
    }
    //指定类型和区块
    public void CreateItem(ItemName itemName, int[] blockNum)
    {
        Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        while ((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.1f) 
                || (System.Array.IndexOf(blockNum, BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler))) == -1))
        {
            targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
        CreateItem(itemName, targetEuler);
    }
    //指定类型和坐标
    public void CreateItem(ItemName itemName, Vector3 targetEuler)
    {
        GameObject item = Instantiate(itemPrefab);
        item.transform.SetParent(itemParent);
        item.transform.position = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.transform.eulerAngles = targetEuler;
        itemSprite = item.transform.Find("ItemSprite").GetComponent<MeshRenderer>();
        itemSprite.material = materials[(int)itemName];
        switch (itemName)
        {
            case ItemName.Army:
            case ItemName.Businessman:
            case ItemName.Farmer:
            case ItemName.Savages:
            case ItemName.Naked:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.53f, 0);
                item.GetComponent<Item>().itemType = Item.ItemType.Character;
                break;
            case ItemName.City:
            case ItemName.NightCity:
                itemSprite.transform.localPosition = new Vector3(0, 0.58f, 0);
                item.GetComponent<Item>().itemType = Item.ItemType.Building;
                break;
            case ItemName.Pyramid:
                itemSprite.transform.localPosition = new Vector3(0, 0.565f, 0);
                item.GetComponent<Item>().itemType = Item.ItemType.Building;
                break;
            case ItemName.Shrub:
            case ItemName.Tree:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.53f, 0);
                item.GetComponent<Item>().itemType = Item.ItemType.Resource;
                ResourceSystem.Instance.resourceInBlock[BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler))] ++;
                break;
        }
    }
}
