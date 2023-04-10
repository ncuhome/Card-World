using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterData
{
    public string name;
    public SpecialSkill specialSkill;
    public Era era;
    public Material characterMaterial;
}
public class CharacterSystem : MonoBehaviour
{
    public static CharacterSystem Instance = null;
    public Character[] characters = new Character[100];
    public CharacterData[] characterDatas = new CharacterData[20];
    public float maxAge = 50f;
    public Material[] eraMaterials = new Material[3];
    public Material[] specialMaterials = new Material[10];
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
    }

    // Update is called once per frame
    void Update()
    {
        switch (EraSystem.Instance.era)
        {
            case Era.AncientEra:
                maxAge = 50f;
                break;
            case Era.ClassicalEra:
                maxAge = 75f;
                break;
            case Era.IndustrialEra:
                maxAge = 100f;
                break;
        }
    }

    //判定是否能进行建筑，并且返回作为建造者的角色下标
    public int[] GetBuilders(int size)
    {
        int[] builders;
        for (int i = 0; i < 24; i++)
        {
            int num = 0;
            builders = new int[4] { -1, -1, -1, -1 };
            for (int j = 0; j < 100; j++)
            {
                if ((characters[j] != null) && (BuildingSystem.Instance.buildingInBlock[i] < BuildingSystem.Instance.maxBuildingInBlock) && (characters[j].item.blockNum == i)
                 && (characters[j].foundResource == false) && (characters[j].goToBuild == false) && (characters[j].goHome == false))
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
