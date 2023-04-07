using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 驯化 : TechNode
{
    public static 驯化 instance;
    private void Start()
    {
        nodeName = this.gameObject.name; //名字为搭载脚本的物体的名字
        if (instance == null)
        {
            instance = this;
        }

    }
}
