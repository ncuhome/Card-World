using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 油田卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        Debug.Log("森林卡生成树木");
        ResourceSystem.Instance.RegenerationResource(ResourceType.Oil, block);
    }

}
