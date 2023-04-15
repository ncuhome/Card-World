using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 山地卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        BlockSystem.Instance.blocks[block[0]].temperature -= 1;
        BlockSystem.Instance.blocks[block[0]].livability += 2;
        ResourceSystem.Instance.RegenerationResource(ResourceType.Stone, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.Stone, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.Stone, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.Stone, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.Stone, block);
    }

}
