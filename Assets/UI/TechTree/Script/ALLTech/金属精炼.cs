using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 金属精炼 : TechNode
{
    public static 金属精炼 instance;
    void Start()
    {
        nodeName = this.gameObject.name; //名字为搭载脚本的物体的名字
        if (instance == null)
        {
            instance = this;
        }
    }
}
