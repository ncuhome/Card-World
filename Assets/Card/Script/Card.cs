using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static int cardSizex = 100;
    public static int cardSizey = 150;
    public static float firstTime = 0.5f;  //�鿨��һ�׶�ʱ��
    public static float secondTime = 1.5f; //�鿨�ڶ��׶�ʱ��

    private void Start() //���Ʊ�����������ִ��
    {
        Drawcards();
    }
    public virtual void BeUse()  //ʹ�ÿ��ƺ���
    {

    }
    public void Drawcards()
    {
        this.transform.localPosition = new Vector2(900, 0);
        Sequence quence = DOTween.Sequence(); //������������
        quence.Append(transform.DOLocalMove(new Vector2(0,540) , firstTime));
        quence.Join(transform.DOScale(new Vector2(3f, 3f), firstTime));
        quence.Join(transform.DORotate(new Vector2(15, -90), firstTime).From());
        quence.Append(transform.DOPunchPosition(new Vector2(5, 10), 0.2f, 1, 0.1f)); //������
        quence.AppendInterval(0.6f);                                                 //��ͣ
        quence.Append(transform.DOLocalMove(CardPack.AddCard(this), secondTime));    //�ص�����
        quence.Join(transform.DOScale(new Vector2(1f, 1f), secondTime));
    }

}
