using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem : MonoBehaviour
{
    public static CharacterSystem Instance = null;
    public Character[] characters = new Character[100];
    public float maxAge = 50f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance CharacterSystem");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TestCreate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TestCreate()
    {
        int num = Random.Range(0, 24);
        for (int i = 0; i <= 3; i++)
        {
            CreateController.Instance.CreateItem(CreateController.ItemName.Naked, new int[] { num });
        }
    }
    //判定是否能进行建筑，并且返回作为建造者的角色下标
    public int[] GetBuilders(int size, int targetResource)
    {
        int[] builders;
        for (int i = 0; i < 24; i++)
        {
            int num = 0;
            builders = new int[4] { -1, -1, -1, -1 };
            for (int j = 0; j < 100; j++)
            {
                if ((characters[j] != null) && (BuildingSystem.Instance.buildingInBlock[i] < BuildingSystem.Instance.maxBuildingInBlock) && (characters[j].item.blockNum == i)
                 && (characters[j].characterState != CharacterState.gather) && (ResourceSystem.Instance.resourceNum >= targetResource) && (characters[j].goToBuild == false))
                {
                    builders[num] = j;
                    num++;
                    if (num >= size)
                    {
                        return builders;
                    }
                }
            }
        }
        return null;
    }
}
