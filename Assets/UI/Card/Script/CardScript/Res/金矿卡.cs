using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 金矿卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        ResourceSystem.Instance.RegenerationResource(ResourceType.GoldOre, block);
    }

}
