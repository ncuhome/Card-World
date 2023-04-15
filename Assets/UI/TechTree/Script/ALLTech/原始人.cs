using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 原始人 : TechNode
{
    public static 原始人 instance;
    public override void ImmediateUnlockIt()
    {
        this.unlock = true;
        this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 0.5f);
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[3]);
    }
    private void Start()
    {
        nodeName = this.gameObject.name; //名字为搭载脚本的物体的名字
        if(instance == null)
        {
            instance = this;
        }
        原始人.instance.ImmediateUnlockIt();
    }
}
