using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 矿石卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        BlockSystem.Instance.blocks[block[0]].livability -= 5;
        int randomNum = Random.Range(3, 12);
        ResourceSystem.Instance.RegenerationResource(ResourceSystem.Instance.resourceDatas[randomNum].resourceType, block);
    }
}
