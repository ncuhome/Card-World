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

    public bool speed; //是否加速
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
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        }
        else if (foldRes.activeInHierarchy == true)
        {
            foldRes.SetActive(false);
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        }
    }
    public void ShowResName()
    {
        if (change)
        {
            change = false;
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        }
        else if (!change)
        {
            change = true;
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        }
    }

    public void Speed()
    {
        if (speed == false)
        {
            Time.timeScale = 10f;
            speed = true;
        }
        else
        {
            Time.timeScale = 1f;
            speed = false;
        }
    }
}
