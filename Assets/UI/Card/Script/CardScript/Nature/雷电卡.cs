using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 雷电卡 : RangeUsageCard
{
    public override void AffectBlock(int[] block)
    {
        foreach (Character character in CharacterSystem.Instance.characters)
        {
            if (character != null)
            {
                if (block[0] == BlockSystem.Instance.GetBlockNum(Vector3.zero, character.transform.rotation))
                {
                    Debug.Log("检测到小人");
                    if (Random.Range(0, 9) == 5) //十分之一概率劈死
                    {
                        character.Die();
                    }
                }
            }
        }
    }

}
