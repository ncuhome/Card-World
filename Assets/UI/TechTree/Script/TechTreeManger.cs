using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeManger : MonoBehaviour
{
    private GameObject TechTreePanel;
    void Start()
    {
        TechTreePanel = GameObject.Find("TechTreeUIPanel");
        TechTreePanel.SetActive(false);
    }

    public void OpenCloseTechTree()  //关闭科技树
    {
        if (TechTreePanel.activeSelf == true)
        {
            TechTreePanel.SetActive(false);
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[4]);
        }
        else
        {
            TechTreePanel.SetActive(true);
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[4]);
        }
    }
}
