using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 雪卡 : RangeUsageCard
{

    public override IEnumerator SelectBlock()
    {
        while (isSelect == true)
        {
            CardManger.instance.dividingLine.SetActive(true);
            //Debug.Log("现在指着区块" + BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition()));
            if (Input.GetMouseButtonDown(0))
            {
                if (MouseOnSphere.instance.ReturnMousePosition() != Vector3.zero)
                {
                    CardManger.instance.dividingLine.SetActive(false);
                    int[] block = new int[1];
                    block[0] = BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition());
                    AffectBlock(block);
                    SignUI.instance.SetTextNULL();
                    CardPack.canBeDrag = true;  //其他卡牌能被拖动
                    canBeDrag = true;
                    isSelect = false;
                }
            }
            yield return null;
        }
    }
    public override void AffectBlock(int[] block)
    {
        StartCoroutine(BlockSystem.Instance.NauAffectBlock(-1f, 0.5f, 0, 10f, this.gameObject, block));
    }
}
