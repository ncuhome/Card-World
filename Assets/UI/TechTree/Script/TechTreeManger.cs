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

    public void OpenCloseTechTree()  //�رտƼ���
    {
        if (TechTreePanel.activeSelf == true)
        {
            TechTreePanel.SetActive(false);
        }
        else
        {
            TechTreePanel.SetActive(true);
        }
    }
}
