using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName
{
    Army, Businessman, Farmer, Savages, Naked, City, NightCity, Pyramid, Shrub, Tree
}
public class CreateController : MonoBehaviour
{
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
        while (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f)
        {
            targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }

        CreateItem(itemName, targetEuler);
    }
    //指定类型和区块
    public void CreateItem(ItemName itemName, int[] blockNum)
    {
        Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        while ((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.01f)
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

        Item itemScript = item.GetComponent<Item>();
        Character character = item.GetComponent<Character>();
        switch (itemName)
        {
            case ItemName.Army:
            case ItemName.Businessman:
            case ItemName.Farmer:
            case ItemName.Savages:
            case ItemName.Naked:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.51f, 0);
                itemScript.itemType = ItemType.Character;
                character.characterNum = GetCharacterNum();
                CharacterSystem.Instance.characters[GetCharacterNum()] = character;
                item.name = "Character";
                break;
            case ItemName.City:
            case ItemName.NightCity:
                itemSprite.transform.localPosition = new Vector3(0, 0.54f, 0);
                itemScript.itemType = ItemType.Building;
                item.name = "Building";
                break;
            case ItemName.Pyramid:
                itemSprite.transform.localPosition = new Vector3(0, 0.53f, 0);
                itemScript.itemType = ItemType.Building;
                item.name = "Building";
                break;
            case ItemName.Shrub:
            case ItemName.Tree:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.515f, 0);
                itemScript.itemType = ItemType.Resource;
                ResourceSystem.Instance.resourceInBlock[BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler))]++;
                item.name = "Resource";
                break;
        }
    }

    //获取角色数组里的最小空位
    private int GetCharacterNum()
    {
        int i = 0;
        while (CharacterSystem.Instance.characters[i] != null)
        {
            i++;
        }
        return i;
    }
}
