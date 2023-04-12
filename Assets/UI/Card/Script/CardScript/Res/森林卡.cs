using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 森林卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        for (int i = 0; i < 10; i++)
        {
            BlockSystem.Instance.blocks[block[0]].livability += 3;
            ResourceSystem.Instance.RegenerationResource(ResourceType.Wood, block);
        }
    }
}
