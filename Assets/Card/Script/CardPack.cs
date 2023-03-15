using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPack : MonoBehaviour //���Ʊ���
{
    public int cardMaximums = 10;
    public static List<MonoBehaviour> cardPack = new List<MonoBehaviour>();
    public static Vector2 AddCard(Card newCard)
    {
        Vector2 endVector;  //������������ƶ�����λ��
        cardPack.Add(newCard); //�򿨰��м����¿���
        for (int i = 0; i < cardPack.Count - 1; i++)
        {
            cardPack[i].gameObject.transform.DOLocalMoveX(cardPack[i].gameObject.transform.localPosition.x - Card.cardSizex / 2 , Card.secondTime);
        }
        if (cardPack.Count == 1)
        {
            endVector = Vector2.zero;
        }
        else
        {
            endVector = new Vector2(cardPack[cardPack.Count - 2].gameObject.transform.localPosition.x + Card.cardSizex/2, 0);
        }
        return endVector;
    }
}
