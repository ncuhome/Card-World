using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechNode : MonoBehaviour
{
    public string nodeName;  //该科技的名字

    public bool unlock = false;  //是否已经解锁

    [SerializeField]private List<TechNode> frontTechnology = null;
    public bool DetectUnlocked()  //检查该科技是否已经解锁
    {
        if (unlock)  //已经解锁
        {
            return true;
        }
        else        //没有解锁
        {
            return false;
        }
    }
    public void UnlockIt() //点击解锁这个科技
    {
        int count = 0;
        for (int i = 0; i < frontTechnology.Count; i++)
        {
            if (frontTechnology[i].unlock)
            {
                count++;
            }
            else
            {
                SignUI.instance.DisplayText("前置科技" + frontTechnology[i].nodeName + "未解锁", 3f, Color.red);
            }
        }

        if (count == frontTechnology.Count) //前置科技都被解锁
        {
            this.unlock = true;
            this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 0.5f);
            SignUI.instance.DisplayText("你已经解锁" + this.nodeName, 2.5f, Color.blue);
        }                                          
    }
    public void ImmediateUnlockIt()
    {
        this.unlock = true;
        this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 0.5f);
        SignUI.instance.DisplayText("你已经解锁" + this.nodeName, 2.5f, Color.blue);
    }
    //void Start()
    //{
    //    nodeName = this.gameObject.name; //名字为搭载脚本的物体的名字
    //}

    void Update()
    {
        
    }
}
