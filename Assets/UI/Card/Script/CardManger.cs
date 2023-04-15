using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManger : MonoBehaviour
{
    [SerializeField]private GameObject[] AllCard = new GameObject[26];
    [SerializeField] private float[] cardProbability = new float[26];
    public GameObject textCard;
    public GameObject redCard;
    public static CardManger instance;
    private bool haveDealCard;
    private float time;
    public GameObject dividingLine;
    private void Start()
    {
        Time.timeScale = 1f;
        //开局送一张科技卡
        GameObject newCard = Instantiate(AllCard[25]);
        newCard.transform.SetParent(GameObject.Find("Card pack").transform);  //设置父类为卡包
        newCard.transform.localScale = Vector2.zero;

        dividingLine = GameObject.Find("DividingLine");
        dividingLine.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        time += Time.deltaTime;
    }

    public void DealCard()  //发牌
    {
        //AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[1]);
        //if (CardPack.cardPack.Count < 6 && time > Card.firstTime + 0.8f)
        //{
        //    time = 0;
        //    GameObject newCard;
        //    float allProbability = 0;
        //    for (int i = 0; i < cardProbability.Length; i++)
        //    {
        //        allProbability += cardProbability[i];
        //    }
        //    float randomNum = Random.Range(0f, allProbability);
        //    float frontProbability = 0;
        //    for (int i = 0; i < cardProbability.Length; i++)
        //    {
        //        if (frontProbability < randomNum && randomNum <= frontProbability + cardProbability[i])
        //        {
        //            newCard = Instantiate(AllCard[i]);
        //            newCard.transform.SetParent(GameObject.Find("Card pack").transform);  //设置父类为卡包
        //            newCard.transform.localScale = Vector2.zero;
        //            break;
        //        }
        //        frontProbability += cardProbability[i];
        //    }
        //}
        //else if (time <= 0.5f)
        //{
        //    SignUI.instance.DisplayText("抽卡间隔过短", 1f, Color.red);
        //}
        //else
        //{
        //    SignUI.instance.DisplayText("你只能持有最多6张牌", 1.5f, Color.red);
        //}

        if (CardPack.cardPack.Count < 6)
        {
            GameObject newCard;
            newCard = Instantiate(textCard);
            newCard.transform.SetParent(GameObject.Find("Card pack").transform);  //设置父类为卡包
            newCard.transform.localScale = Vector2.zero;
        }
    }
}
