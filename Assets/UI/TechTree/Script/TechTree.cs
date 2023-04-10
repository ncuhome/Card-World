using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TechTree : MonoBehaviour
{
    public TechNode[] allTechNode = new TechNode[27];

    public static TechTree instance;
    void Start()
    {
        if (instance ==  null)
        {
            instance = this;
        }
    }
    public bool GetWhetherUnlocked(string techName)  //检查该科技是否解锁
    {
        foreach(TechNode node in allTechNode)
        {
            if(node.nodeName == techName)
            {
                if (node.unlock)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        throw new NullReferenceException("没有找到该科技,请检查你输入的科技名字是否正确");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
