using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 冰川时代意外卡 : AccidentCard
{
    public override void BeUse()
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[5]);
        foreach (Block block in BlockSystem.Instance.blocks)
        {
            block.temperature -= 20;
        }
    }
}
