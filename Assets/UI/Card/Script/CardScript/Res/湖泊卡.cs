using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 湖泊卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        BlockSystem.Instance.blocks[block[0]].water += 5;
        BlockSystem.Instance.blocks[block[0]].livability += 2;
        for (int i = 0; i < 10; i++)
        {
            ResourceSystem.Instance.RegenerationResource(ResourceType.Water, block);
        }
        
    }

}
