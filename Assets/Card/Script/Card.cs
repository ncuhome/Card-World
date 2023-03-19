using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public static int cardSizex = 100;     //卡牌宽度

    public static int cardSizey = 150;     //卡牌高度

    public static float firstTime = 0.5f;  //抽卡第一阶段时长 变大并展示

    public static float secondTime = 1.5f; //抽卡第二阶段时长 缩小进入卡槽

    public Sequence pointerSequence;

    private bool canBeDrag;
    [SerializeField] private Canvas cardCanvas;
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
        quence.AppendInterval(0.6f).OnComplete(() => { CardPack.AddCard(this); transform.DOScale(new Vector2(1f, 1f), secondTime); });   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.SetAsLastSibling(); //显示在所有卡牌的最上面
        this.pointerSequence.Append(transform.DOScale(new Vector2(1.5f, 1.5f), 0.1f));  //变大
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.pointerSequence.Append(transform.DOScale(new Vector2(1f, 1f), 0.1f));      //变小
    }

    public void OnDrag(PointerEventData eventData) //卡牌拖拽效果
    {
        this.GetComponent<RectTransform>().anchoredPosition += 
            eventData.delta / this.transform.parent.transform.parent.GetComponent<Canvas>().scaleFactor;  
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)
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
