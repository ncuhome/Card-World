using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static int cardSizex = 100;
    public static int cardSizey = 150;
    public static float firstTime = 0.5f;  //抽卡第一阶段时长
    public static float secondTime = 1.5f; //抽卡第二阶段时长

    private void Start() //卡牌被创建后立即执行
    {
        Drawcards();
    }
    public virtual void BeUse()  //使用卡牌函数
    {

    }
    public void Drawcards()
    {
        this.transform.localPosition = new Vector2(900, 0);
        Sequence quence = DOTween.Sequence(); //声明动画容器
        quence.Append(transform.DOLocalMove(new Vector2(0,540) , firstTime));
        quence.Join(transform.DOScale(new Vector2(3f, 3f), firstTime));
        quence.Join(transform.DORotate(new Vector2(15, -90), firstTime).From());
        quence.Append(transform.DOPunchPosition(new Vector2(5, 10), 0.2f, 1, 0.1f)); //卡牌震动
        quence.AppendInterval(0.6f);                                                 //暂停
        quence.Append(transform.DOLocalMove(CardPack.AddCard(this), secondTime));    //回到卡槽
        quence.Join(transform.DOScale(new Vector2(1f, 1f), secondTime));
    }

}
