using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeManger : MonoBehaviour
{
    public GameObject TechTreeImage;
    void Start()
    {
        
    }

    public void OpenCloseTechTree()  //关闭科技树
    {
        if (TechTreeImage.transform.localScale == Vector3.zero)
        {
            TechTreeImage.transform.localScale = Vector3.one;
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[4]);
        }
        else if (TechTreeImage.transform.localScale == Vector3.one)
        {
            TechTreeImage.transform.localScale = Vector3.zero;
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[4]);
        }
    }
}
