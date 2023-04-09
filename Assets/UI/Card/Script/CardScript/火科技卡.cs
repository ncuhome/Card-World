using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 火科技卡 : Card
{
    public override void BeUse()
    {
        火.instance.ImmediateUnlockIt();
    }
}
