using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManger : MonoBehaviour
{
    public GameObject normalCard;
    public GameObject redCard;
    public static CardManger instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void DealCard()  //����
    {
        if (CardPack.cardPack.Count < 6)
        {
            GameObject newCard;
            int a = Random.Range(0, 2);
            if (a == 0)
            {
                newCard = Instantiate(this.normalCard);
            }
            else
            {
                newCard = Instantiate(this.redCard);
            }
            newCard.transform.SetParent(GameObject.Find("Card pack").transform);  //���ø���Ϊ����
            newCard.transform.localScale = Vector2.zero;
        }
    }

}
