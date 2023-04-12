using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowResManger : MonoBehaviour
{
    public Text[] resText;

    public GameObject[] gameObjects;

    private GameObject foldRes;

    public static bool change = false;
    void Update()
    {
        for (int i = 0; i < resText.Length; i++)
        {
            if (!change)
            {
                resText[i].text = ResourceSystem.Instance.resourceDatas[i].resourceNum.ToString();
            }
            else
            {
                resText[i].text = gameObjects[i].name;
            }
        }
    }
    private void Start()
    {
        foldRes = GameObject.Find("OtherRes");
        foldRes.SetActive(false);
    }
    public void FoldButton()
    {
        if (foldRes.activeInHierarchy == false)
        {
            foldRes.SetActive(true);
        }
        else if (foldRes.activeInHierarchy == true)
        {
            foldRes.SetActive(false);
        }
    }
    public void ShowResName()
    {
        if (change)
        {
            change = false;
        }
        else if (!change)
        {
            change = true;
        }
    }
}
