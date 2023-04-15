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
    public int characterNum;
    public int preCharacterNum;
    public bool techAbility;
}
public class CharacterSystem : MonoBehaviour
{
    public static CharacterSystem Instance = null;
    public Character[] characters = new Character[100];
    public CharacterData[] characterDatas = new CharacterData[20];
    public float maxAge = 50f;
    public int population;
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
                maxAge = 60f;
                break;
            case Era.ClassicalEra:
                maxAge = 85f;
                break;
            case Era.IndustrialEra:
                maxAge = 110f;
                break;
        }

        EndOfCivilization();
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
                 && (characters[j].foundResource == false) && (characters[j].goToBuild == false) && (characters[j].goHome == false) && (characters[j].stayInBuilding == false))
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

    //根据时代和特殊能力获取对应角色
    public CharacterData GetCharacter(Era era, SpecialSkill? characterSkill)
    {
        foreach (CharacterData characterData in characterDatas)
        {
            if ((characterData.era == era) && (characterData.specialSkill == characterSkill))
            {
                return characterData;
            }
        }
        return null;
    }

    public int GetPopulation()
    {
        population = 0;
        foreach (Character character in characters)
        {
            if (character != null) { population++; }
        }
        return population;
    }

    //获取角色数组里的最小空位
    public int GetCharacterNum()
    {
        int i = 0;
        while (characters[i] != null)
        {
            i++;
        }
        return i;
    }

    //文明进阶
    public void CivilizationProgresses()
    {
        foreach (Character character in characters)
        {
            if (character == null) { continue; }
            CharacterData[] charactersCanProgress = new CharacterData[20];
            int charactersCanProgressNum = 0;
            for (int i = 0; i < 20; i++)
            {
                if (characterDatas[i].preCharacterNum == GetCharacter(EraSystem.Instance.era, character.specialSkill).characterNum)
                {
                    charactersCanProgress[charactersCanProgressNum] = characterDatas[i];
                    charactersCanProgressNum++;
                }
            }
            if (charactersCanProgressNum != 0)
            {
                int num = Random.Range(0, charactersCanProgressNum);
                character.specialSkill = charactersCanProgress[num].specialSkill;
                character.itemSprite.material = charactersCanProgress[num].characterMaterial;
                character.name = charactersCanProgress[num].name;
                character.itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                switch (CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era + 1, character.specialSkill).characterNum)
                {
                    case 12:
                    case 13:
                    case 18:
                        character.itemSprite.transform.localPosition = new Vector3(0, 0.515f, 0);
                        break;
                    case 17:
                        character.itemSprite.transform.localPosition = new Vector3(0, 0.514f, 0);
                        break;
                }
                break;
            }
            else
            {
                character.specialSkill = SpecialSkill.None;
                character.itemSprite.material = GetCharacter(EraSystem.Instance.era, SpecialSkill.None).characterMaterial;
                character.name = GetCharacter(EraSystem.Instance.era, SpecialSkill.None).name;
                character.itemSprite.transform.localPosition = new Vector3(0, 0.518f, 0);
                if (GetCharacter(EraSystem.Instance.era, SpecialSkill.None).characterNum == 13)
                {
                    character.itemSprite.transform.localPosition = new Vector3(0, 0.515f, 0);
                }
            }
        }
    }

    //根据概率和时代获取随机的技能
    public SpecialSkill GetRandomSkill()
    {
        float random = Random.value;
        switch (EraSystem.Instance.era)
        {
            case Era.AncientEra:
                if (random < 0.5f) { return SpecialSkill.None; }
                if (random < 0.65f) { return SpecialSkill.Hunting; }
                if (random < 0.8f) { return SpecialSkill.Farming; }
                if (random < 0.9f) { return SpecialSkill.Stargazing; }
                return SpecialSkill.Leading;
            case Era.ClassicalEra:
                if (random < 0.3f) { return SpecialSkill.None; }
                if (random < 0.45f) { return SpecialSkill.Alchemy; }
                if (random < 0.55f) { return SpecialSkill.Leading; }
                if (random < 0.6f) { return SpecialSkill.OceanSailing; }
                if (random < 0.65f) { return SpecialSkill.Navigation; }
                if (random < 0.8f) { return SpecialSkill.Farming; }
                if (random < 0.85f) { return SpecialSkill.AstronomicalObservation; }
                return SpecialSkill.Smelt;
            case Era.IndustrialEra:
                if (random < 0.4f) { return SpecialSkill.None; }
                if (random < 0.44f) { return SpecialSkill.OceanSailing; }
                if (random < 0.52f) { return SpecialSkill.Navigation; }
                if (random < 0.68f) { return SpecialSkill.Refining; }
                if (random < 0.8f) { return SpecialSkill.GenerateElectricity; }
                if (random < 0.95f) { return SpecialSkill.Farming; }
                return SpecialSkill.AerospaceResearch;
        }
        return SpecialSkill.None;
    }

    public void EndOfCivilization()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != null) { return; }
        }
        BuildingSystem.Instance.EndOfCivilization();
        GameObject.Find("FailPanel").transform.localScale = Vector3.one; //显示失败UI
        Debug.Log("End Of Civilization");
    }

}
