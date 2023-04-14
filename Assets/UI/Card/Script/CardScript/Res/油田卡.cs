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
        BlockSystem.Instance.blocks[block[0]].livability -= 5;
        CreateController.Instance.CreateItem(ItemType.Building, null, BuildingType.OilVent, null, block);
    }

}
