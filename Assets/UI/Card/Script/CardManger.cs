using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManger : MonoBehaviour
{
    [SerializeField]private GameObject[] AllCard = new GameObject[26];
    [SerializeField] private float[] cardProbability = new float[26];
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

    public void DealCard()  //发牌
    {
        //if (CardPack.cardPack.Count < 6)
        //{
            GameObject newCard;
            float allProbability = 0;
            for (int i = 0; i < cardProbability.Length; i++)
            {
                allProbability += cardProbability[i];
            }
            float randomNum = Random.Range(0f, allProbability);
            float frontProbability = 0;
            for (int i = 0; i < cardProbability.Length; i++)
            {
                if (frontProbability < randomNum && randomNum <= frontProbability + cardProbability[i])
                {
                    newCard = Instantiate(AllCard[i]);
                    newCard.transform.SetParent(GameObject.Find("Card pack").transform);  //设置父类为卡包
                    newCard.transform.localScale = Vector2.zero;
                    break;
                }
                frontProbability += cardProbability[i];
 
        }
            
        //if (CardPack.cardPack.Count < 6)
        //{
        //    GameObject newCard;
        //    int a = Random.Range(0, 2);
        //    if (a == 0)
        //    {
        //        newCard = Instantiate(AllCard[25]);
        //    }
        //    else
        //    {
        //        newCard = Instantiate(AllCard[25]);
        //    }
        //    newCard.transform.SetParent(GameObject.Find("Card pack").transform);  //设置父类为卡包
        //    newCard.transform.localScale = Vector2.zero;
        //}
    }

}
