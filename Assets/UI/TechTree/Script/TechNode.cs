using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechNode : MonoBehaviour
{
    public string nodeName;  //该科技的名字

    public bool unlock;  //是否已经解锁

    public bool DetectUnlocked()
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
    public void UnlockIt() //解锁这个科技
    {

    }
    void Start()
    {
        nodeName = this.gameObject.name; //名字为搭载脚本的物体的名字
    }

    void Update()
    {
        
    }
}
