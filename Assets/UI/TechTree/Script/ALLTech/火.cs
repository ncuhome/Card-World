using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 火 : TechNode
{
    public static 火 instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        instance.nodeName = gameObject.name; //名字为搭载脚本的物体的名字
    }
}
