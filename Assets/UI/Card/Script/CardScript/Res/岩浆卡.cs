using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 岩浆卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        BlockSystem.Instance.blocks[block[0]].temperature += 20;
        BlockSystem.Instance.blocks[block[0]].livability -= 10;
        CreateController.Instance.CreateItem(ItemType.Building, null, BuildingType.Magma, null, block);
    }
}
