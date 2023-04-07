using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public static int cardSizex = 100;     //���ƿ��

    public static int cardSizey = 150;     //���Ƹ߶�

    public static float firstTime = 0.5f;  //�鿨��һ�׶�ʱ�� ���չʾ

    public static float secondTime = 0.4f; //�鿨�ڶ��׶�ʱ�� ��С���뿨��

    public static GameObject cardTrash;    //����Ͱ�����п����ж���һ������

    public float mouseTimer;               //���ͣ����ʱ��

    public bool mouseOnCard;               //����Ƿ��ڿ�����
    protected bool canBeDrag;
    private Canvas cardCanvas;
    [SerializeField] private GameObject descriptionPanel;
    public void Start() //���Ʊ�����������ִ��
    {
        cardTrash = GameObject.Find("TrashCan");
        Drawcards();
    }
    private void Update()
    {
        if (mouseOnCard == true)
        {
            mouseTimer += Time.deltaTime;
            if (mouseTimer > 0.5f)
            {
                descriptionPanel.transform.DOLocalMoveX(cardSizex, 0.1f);
            }
        }
    }
    public virtual void BeUse()  //ʹ�ÿ��ƺ���
    {

    }
    public void Drawcards()
    {
        canBeDrag = false;
        this.transform.localPosition = new Vector2(900, 0);
        Sequence quence = DOTween.Sequence(); //������������
        quence.Append(transform.DOLocalMove(new Vector2(0, 540), firstTime));
        quence.Join(transform.DOScale(new Vector2(3f, 3f), firstTime));
        quence.Join(transform.DORotate(new Vector2(15, -90), firstTime).From());
        quence.Append(transform.DOPunchPosition(new Vector2(5, 10), 0.2f, 1, 0.1f)); //������
        quence.AppendInterval(0.6f).OnComplete(() => {
            CardPack.AddCard(this); transform.DOScale(new Vector2(1f, 1f), secondTime)
            .OnComplete(() => { canBeDrag = true; });
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            mouseOnCard = true;
            this.transform.SetAsLastSibling(); //��ʾ�����п��Ƶ�������
            transform.DOScale(new Vector2(1.75f, 1.75f), 0.1f);  //���
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            mouseOnCard = false;
            mouseTimer = 0;
            descriptionPanel.transform.DOLocalMoveX(0, 0.1f);
            transform.DOScale(new Vector2(1f, 1f), 0.1f);      //��С
        }

    }

    public void OnDrag(PointerEventData eventData) //������קЧ��
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            mouseOnCard = false;
            this.GetComponent<RectTransform>().anchoredPosition +=
                eventData.delta / this.transform.parent.transform.parent.GetComponent<Canvas>().scaleFactor;
        }


    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 350)  //������Ͱ��Χ��
            {
                this.transform.DOMove(cardTrash.transform.position, 0.5f).OnComplete(() => { CardPack.DeleteCard(this); Destroy(this.gameObject); });
            }
            else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)
            {
                canBeDrag = false;
                CardPack.DeleteCard(this);
                transform.DOLocalMove(new Vector2(0, 540), 0.5f);
                transform.DOScale(new Vector2(2f, 2f), 0.5f).
                    OnComplete(() => { Destroy(this.transform.GetChild(0).gameObject); this.transform.GetChild(1).GetComponent<Image>().DOFade(0, 0.2f).OnComplete(() => { Destroy(this.gameObject); }); });
                BeUse();
            }
            else
            {
                CardPack.SortCard();
            }
        }
    }

}

public class AccidentCard : Card
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 350)  //������Ͱ��Χ��
            {
                SignUI.instance.DisplayText("You can't broke it", 1f, Color.red);
                CardPack.SortCard();
            }
            else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)  //��ʹ��
            {
                this.canBeDrag = false;
                CardPack.DeleteCard(this);
                transform.DOLocalMove(new Vector2(0, 540), 0.5f);
                transform.DOScale(new Vector2(2f, 2f), 0.5f).
                    OnComplete(() => { this.GetComponent<Image>().DOFade(0, 0.2f).OnComplete(() => { Destroy(this); }); });
                BeUse();
            }
            else
            {
                CardPack.SortCard();
            }
        }
    }
}

public class RangeUsageCard : Card, IPointerUpHandler //��Χʹ�õĿ���
{
    public bool isSelect = false;
    public override void BeUse()
    {
        isSelect = true;
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {

        if (isSelect)
        {
            Debug.Log(BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition()));
            isSelect = false;
        }
    }
}

public class AccidentRangeUsageCard : AccidentCard, IAffectBlock //��Χʹ�õ�������
{
    public bool isSelect = false;
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 350)  //������Ͱ��Χ��
            {
                SignUI.instance.DisplayText("���޷��ݻ��������⿨", 1f, Color.red);
                CardPack.SortCard();
            }
            else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)  //��ʹ��
            {
                this.canBeDrag = false;
                CardPack.DeleteCard(this);
                transform.DOLocalMove(new Vector2(0, 540), 0.5f);
                transform.DOScale(new Vector2(2f, 2f), 0.5f).
                    OnComplete(() => { Destroy(this.transform.GetChild(0).gameObject); this.transform.GetChild(1).GetComponent<Image>().DOFade(0, 0.2f).OnComplete(() => { CardPack.DeleteCard(this); }); });
                BeUse();
            }
            else
            {
                CardPack.SortCard();
            }
        }
    }


    public virtual void AffectBlock(int block) //����������ú���
    {
        Debug.Log(block);
    }

    IEnumerator SelectBlock()
    {
        while (isSelect == true)
        {
            Debug.Log("����ָ������" + BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition()));
            if (Input.GetMouseButtonDown(0))
            {
                if (MouseOnSphere.instance.ReturnMousePosition() != Vector3.zero)
                {
                    AffectBlock(BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition()));
                    SignUI.instance.SetTextNULL();
                    CardPack.canBeDrag = true;  //���������ܱ��϶�
                    Destroy(this.gameObject);
                }
            }
            yield return null;
        }
    }

    public override void BeUse()
    {
        isSelect = true;
        SignUI.instance.DisplayText("ѡ����Ҫ���õ�����", true, Color.red);
        CardPack.canBeDrag = false;  //�������Ʋ��ܱ��϶�
        StartCoroutine(SelectBlock());
    }

    //private void Update()
    //{
    //    if (isSelect == true)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            AffectBlock(BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition()));
    //        }
    //    }
    //}
}

interface IAffectBlock
{
    public void AffectBlock(int block);

}

