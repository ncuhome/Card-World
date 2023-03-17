using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateController : MonoBehaviour
{
    public enum ItemName
    {
        Army, Businessman, Farmer, Savages, Naked, City, NightCity, Pyramid, Shrub, Tree
    }
    public GameObject itemPrefab;
    public Material[] materials = new Material[10];
    public Transform itemParent;
    public MeshRenderer itemSprite;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 20; i++)
        {
            CreateItem();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateItem()
    {
        ItemName itemName = (ItemName)Random.Range(0, 9);
        CreateItem(itemName);
    }

    public void CreateItem(ItemName itemName)
    {
        GameObject item = Instantiate(itemPrefab);
        item.transform.SetParent(itemParent);
        item.transform.position = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.transform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
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
                break;
            case ItemName.City:
            case ItemName.NightCity:
                itemSprite.transform.localPosition = new Vector3(0, 0.58f, 0);
                break;
            case ItemName.Pyramid:
                itemSprite.transform.localPosition = new Vector3(0, 0.565f, 0);
                break;
            case ItemName.Shrub:
            case ItemName.Tree:
                itemSprite.transform.localScale = new Vector3(itemSprite.transform.localScale.x * 0.5f, itemSprite.transform.localScale.y * 0.5f, itemSprite.transform.localScale.z);
                itemSprite.transform.localPosition = new Vector3(0, 0.53f, 0);
                break;
        }
    }
}
