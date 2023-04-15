using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCharRes : MonoBehaviour
{
    public Text[] texts;

    private void Update()
    {
        if(!ShowResManger.change)
        {
            int buildingNum = 0;
            int peopleNum = 0;
            foreach (Building building in BuildingSystem.Instance.buildings)
            {
                if (building != null) buildingNum++;
            }
            foreach (Character character in CharacterSystem.Instance.characters)
            {
                if (character != null) peopleNum++;
            }
            texts[0].text = peopleNum.ToString();
            texts[1].text = buildingNum.ToString();
        }
        else
        {
            texts[0].text = texts[0].gameObject.name;
            texts[1].text = texts[1].gameObject.name;
        }
    }
}
