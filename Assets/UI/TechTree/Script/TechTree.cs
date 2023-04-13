using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TechTree : MonoBehaviour
{
    public TechNode[] allTechNode = new TechNode[27];

    public static TechTree instance;

    public TechNode[] ancientEraTech;

    public TechNode[] classicalEraTech;

    public TechNode[] industrialEraTech;
    void Start()
    {
        if (instance ==  null)
        {
            instance = this;
        }
    }
    public bool GetWhetherUnlocked(string techName)  //检查该科技是否解锁
    {
        foreach(TechNode node in allTechNode)
        {
            if(node.nodeName == techName)
            {
                if (node.unlock)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        throw new NullReferenceException("没有找到该科技,请检查你输入的科技名字是否正确" + techName);
    }
    public int ConfirmEra() //判断属于那个时代
    {
        int count = 0;
        foreach (TechNode tech in classicalEraTech) //上一个时代科技全部解锁则进入下一时代
        {
            if (tech.unlock)
            {
                count++;
            }
        }
        if (count == classicalEraTech.Length)
        {
            if (EraSystem.Instance.era == Era.ClassicalEra)
            {
                EraSignUI.instance.DisplayText("你进入了工业时代！", 5f, Color.red);
                AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[2]);
                BuildingSystem.Instance.CivilizationProgresses();
                CharacterSystem.Instance.CivilizationProgresses();
            }
            EraSystem.Instance.era = Era.IndustrialEra;
            return 2; //第三个时代
        }
        count = 0;
        foreach (TechNode tech in ancientEraTech)
        {
            if (tech.unlock)
            {
                count++;
            }
        }
        if (count == ancientEraTech.Length)
        {
            if (EraSystem.Instance.era == Era.AncientEra)
            {
                EraSignUI.instance.DisplayText("你进入了古典时代！", 5f, Color.red);
                AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[2]);
                BuildingSystem.Instance.CivilizationProgresses();
                CharacterSystem.Instance.CivilizationProgresses();
            }
            EraSystem.Instance.era = Era.ClassicalEra;
            return 1; //第二个时代
        }

        EraSystem.Instance.era = Era.AncientEra;
        return 0; //第一个时代
    }
}
