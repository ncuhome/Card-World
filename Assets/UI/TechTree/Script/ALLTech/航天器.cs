using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 航天器 : TechNode
{
    public static 航天器 instance;
    void Start()
    {
        nodeName = this.gameObject.name; //名字为搭载脚本的物体的名字
        if (instance == null)
        {
            instance = this;
        }
    }
}
