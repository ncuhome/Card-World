using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 繁荣科技卡 : Card //随机解锁一个可以被解锁的科技
{
    public override void BeUse()
    {
        TechNode[] allTechNode;
        List<TechNode> canBeUnlock = new List<TechNode>();
        if (EraSystem.Instance.era == Era.AncientEra)
        {
            allTechNode = TechTree.instance.ancientEraTech;
        }
        else if (EraSystem.Instance.era == Era.ClassicalEra)
        {
            allTechNode = TechTree.instance.classicalEraTech;
        }
        else
        {
            allTechNode = TechTree.instance.industrialEraTech;
        }
        foreach (TechNode tech in allTechNode)
        {
            Debug.Log(tech);
            if (tech.CanBeUnlocked() == true && tech.unlock == false )
            {
                canBeUnlock.Add(tech);
            }
        }
        int random = Random.Range(0, canBeUnlock.Count); //解锁随机一个科技
        canBeUnlock[random].ImmediateUnlockIt();
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[5]);
    }
}
