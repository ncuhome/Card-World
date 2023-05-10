using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 寒潮卡 : RangeUsageCard
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
                    int[] block = new int[1];
                    CardManger.instance.dividingLine.SetActive(false);
                    block[0] = BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition());
                    if (!BlockSystem.Instance.blocks[block[0]].isWeather)
                    {
                        AffectBlock(block);
                        SignUI.instance.SetTextNULL();
                        CardPack.canBeDrag = true;  //其他卡牌能被拖动
                        canBeDrag = true;
                        isSelect = false;
                    }
                    else
                    {
                        SignUI.instance.DisplayText("该区块已经存在天气效果", 3f, Color.blue);
                    }
                }
            }
            yield return null;
        }
    }
    public override void AffectBlock(int[] block)
    {
        WeatherParticleSystem.instance.Cold(block[0], 60);
        Destroy(gameObject);
    }

}
