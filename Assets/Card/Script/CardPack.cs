using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPack : MonoBehaviour //卡牌背包
{
    public int cardMaximums = 10;
    public static float cardPackHigh;
    public static List<Card> cardPack = new List<Card>();
    private void Awake()
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
        float first = -Card.cardSizex / 2 * (cardPack.Count - 1);
        for (int i = 0; i < cardPack.Count; i++)
        {
            cardPack[i].gameObject.transform.DOLocalMove(new Vector2(first + i * Card.cardSizex, 0), Card.secondTime);
        }
    }
}
