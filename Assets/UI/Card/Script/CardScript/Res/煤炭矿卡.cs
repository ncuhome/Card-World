﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 煤炭矿卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        ResourceSystem.Instance.RegenerationResource(ResourceType.CoalMine, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.CoalMine, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.CoalMine, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.CoalMine, block);
        ResourceSystem.Instance.RegenerationResource(ResourceType.CoalMine, block);
        BlockSystem.Instance.blocks[block[0]].water -= 3;
        BlockSystem.Instance.blocks[block[0]].livability -= 3;
    }

}
