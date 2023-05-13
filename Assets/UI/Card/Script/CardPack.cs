using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPack : MonoBehaviour //卡牌背包
{
    public int cardMaximums = 6;
    public static float cardPackHigh;
    public static List<Card> cardPack = new List<Card>();
    public static bool canBeDrag = true;  //全局控制卡牌是否可以被拖到，如果正在使用时无法拖动
    private void Start()
    {
        cardPackHigh = this.transform.position.y;
    }
    public static void AddCard(Card newCard)
    {
        cardPack.Add(newCard); //向卡包列表中加入新卡牌
        SortCard();
    }
    public static void DeleteCard(Card card)
    {
        cardPack.Remove(card);
        SortCard();
    }
    public static void SortCard()
    {
        canBeDrag = false;
        float firstPos = -Card.cardSizex / 2 * (cardPack.Count - 1);
        //float rot = 5f;
        //if (cardPack.Count % 2 == 0)
        //{
        //    cardPack[cardPack.Count / 2].gameObject.transform.DORotate(Vector3.zero, Card.secondTime)
        //        .OnComplete(() => { canBeDrag = true; }); ;
        //    cardPack[cardPack.Count / 2 - 1].gameObject.transform.DORotate(Vector3.zero, Card.secondTime)
        //        .OnComplete(() => { canBeDrag = true; }); ;
        //    for (int i = 0; i < cardPack.Count / 2 - 1; i++)
        //    {
        //        cardPack[i].gameObject.transform.DORotate(new Vector3(0, 0, 0 + (cardPack.Count / 2 - 1 - i)*rot), Card.secondTime)
        //        .OnComplete(() => { canBeDrag = true; }); ;
        //    }
        //    for (int i = cardPack.Count / 2 + 1; i < cardPack.Count; i++)
        //    {
        //        cardPack[i].gameObject.transform.DORotate(new Vector3(0, 0, 0 - (i - cardPack.Count / 2) * rot), Card.secondTime)
        //        .OnComplete(() => { canBeDrag = true; }); ;
        //    }
        //}
        for (int i = 0; i < cardPack.Count; i++)
        {
            if (!cardPack[i].isDrag)
            {
                cardPack[i].gameObject.transform.DOLocalMove(new Vector2(firstPos + i * Card.cardSizex, 0), Card.secondTime)
                    .OnComplete(() => { canBeDrag = true; });
            }
            else
            {
                canBeDrag = true;
            }
        }
    }
}
