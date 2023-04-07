using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 天文观测 : TechNode
{
    public static 天文观测 instance;
    private void Start()
    {
        nodeName = this.gameObject.name; //名字为搭载脚本的物体的名字
        if (instance == null)
        {
            instance = this;
        }
    }
}
