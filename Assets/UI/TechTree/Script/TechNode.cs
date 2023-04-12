
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechNode : MonoBehaviour
{
    public string nodeName;  //该科技的名字

    public bool unlock = false;  //是否已经解锁

    [SerializeField]public List<TechNode> frontTechnology = null;
    public bool DetectUnlocked()  //检查该科技是否已经解锁
    {
        if (unlock)  //已经解锁
        {
            return true;
        }
        else         //没有解锁
        {
            return false;
        }
    }
    public bool CanBeUnlocked()  //检查该科技的前置科技是否全部被解锁
    {
        int count = 0;
        if (this.frontTechnology == null)
        {
            return true;
        }
        foreach(TechNode node in this.frontTechnology)
        {
            if (node.unlock == true)
            {
                count++;
            }
        }
        if (count == frontTechnology.Count)
        {
            return true;
        }
        else
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
        TechTree.instance.ConfirmEra();
    }
    public void ImmediateUnlockIt()
    {
        this.unlock = true;
        this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 0.5f);
        SignUI.instance.DisplayText("你已经解锁" + this.nodeName, 2.5f, Color.blue);
        TechTree.instance.ConfirmEra();
    }

    void Update()
    {
        
    }
}
