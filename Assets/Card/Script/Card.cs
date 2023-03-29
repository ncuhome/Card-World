using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public static int cardSizex = 100;     //���ƿ��

    public static int cardSizey = 150;     //���Ƹ߶�

    public static float firstTime = 0.5f;  //�鿨��һ�׶�ʱ�� ���չʾ

    public static float secondTime = 1.5f; //�鿨�ڶ��׶�ʱ�� ��С���뿨��

    public static GameObject cardTrash;
    private bool canBeDrag;
    [SerializeField] private Canvas cardCanvas;
    private void Start() //���Ʊ�����������ִ��
    {
        cardTrash = GameObject.Find("TrashCan");
        Drawcards(); 
    }
    public virtual void BeUse()  //ʹ�ÿ��ƺ���
    {
        
    }
    public void Drawcards()
    {
        canBeDrag = false;
        this.transform.localPosition = new Vector2(900, 0);
        Sequence quence = DOTween.Sequence(); //������������
        quence.Append(transform.DOLocalMove(new Vector2(0,540) , firstTime));
        quence.Join(transform.DOScale(new Vector2(3f, 3f), firstTime));
        quence.Join(transform.DORotate(new Vector2(15, -90), firstTime).From());
        quence.Append(transform.DOPunchPosition(new Vector2(5, 10), 0.2f, 1, 0.1f)); //������
        quence.AppendInterval(0.6f).OnComplete(() => { CardPack.AddCard(this); transform.DOScale(new Vector2(1f, 1f), secondTime)
            .OnComplete(() => { canBeDrag = true; }); });   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canBeDrag)
        {
            this.transform.SetAsLastSibling(); //��ʾ�����п��Ƶ�������
            transform.DOScale(new Vector2(1.5f, 1.5f), 0.1f);  //���
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canBeDrag)
        {
            transform.DOScale(new Vector2(1f, 1f), 0.1f);      //��С
        }
        
    }

    public void OnDrag(PointerEventData eventData) //������קЧ��
    {
        if (canBeDrag)
        {
            this.GetComponent<RectTransform>().anchoredPosition += 
                eventData.delta / this.transform.parent.transform.parent.GetComponent<Canvas>().scaleFactor; 
        }
        
         
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 350)
        {
            this.transform.DOMove(cardTrash.transform.position, 0.5f).OnComplete(() => { CardPack.DeleteCard(this); Destroy(this.gameObject); });
        }
        else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)
        {
            canBeDrag = false;
            CardPack.DeleteCard(this);
            transform.DOLocalMove(new Vector2(0, 540), 0.5f);
            transform.DOScale(new Vector2(2f, 2f), 0.5f).
                OnComplete(() => { this.GetComponent<Image>().DOFade(0, 0.2f).OnComplete(() => { Destroy(this); }); });
        }
        else
        {
            CardPack.SortCard();
        }
    }

}
