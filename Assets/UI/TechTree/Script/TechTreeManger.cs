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
            TechTreeImage.transform.localScale = new Vector3(1, 0.8f, 1);
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[4]);
        }
        else if (TechTreeImage.transform.localScale == new Vector3(1, 0.8f, 1))
        {
            TechTreeImage.transform.localScale = Vector3.zero;
            AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[4]);
        }
    }
}
