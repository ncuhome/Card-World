using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManger : MonoBehaviour
{
    private GameObject CardPack;
    public GameObject normalCard;
    public GameObject redCard;
    private void Start()
    {
        CardPack = GameObject.Find("Card pack");
    }
        
    public void DealCard()  //发牌
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
        newCard.transform.SetParent(GameObject.Find("Card pack").transform);  //设置父类为卡包
        newCard.transform.localScale = Vector2.zero;
    }
}
