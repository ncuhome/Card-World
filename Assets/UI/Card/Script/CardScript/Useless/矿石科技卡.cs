using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 矿石科技卡 : Card
{
    public override void BeUse()
    {
        采矿.instance.ImmediateUnlockIt();
    }
}
