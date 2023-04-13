using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public static int cardSizex = 100;     //卡牌宽度

    public static int cardSizey = 150;     //卡牌高度

    public static float firstTime = 0.5f;  //抽卡第一阶段时长 变大并展示

    public static float secondTime = 0.3f; //抽卡第二阶段时长 缩小进入卡槽

    public static GameObject cardTrash;    //垃圾桶在所有卡牌中都是一个对象

    private float mouseTimer;               //鼠标停留的时间

    private bool mouseOnCard;               //鼠标是否在卡牌上

    private int moveY = 50;                     //鼠标在卡牌上时卡牌上移高度
    protected bool canBeDrag;
    private Canvas cardCanvas;
    [SerializeField] private GameObject descriptionPanel;
    public void Start() //卡牌被创建后立即执行
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
    public virtual void BeUse()  //使用卡牌函数
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[5]);
    }
    public void Drawcards()
    {
        canBeDrag = false;
        this.transform.localPosition = new Vector2(900, 0);
        Sequence quence = DOTween.Sequence(); //声明动画容器
        quence.Append(transform.DOLocalMove(new Vector2(0, 540), firstTime));
        quence.Join(transform.DOScale(new Vector2(3f, 3f), firstTime));
        quence.Join(transform.DORotate(new Vector2(15, -90), firstTime).From());
        quence.Append(transform.DOPunchPosition(new Vector2(5, 10), 0.2f, 1, 0.1f)); //卡牌震动
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
            this.transform.SetAsLastSibling(); //显示在所有卡牌的最上面
            transform.DOScale(new Vector2(1.75f, 1.75f), 0.1f);  //变大
            transform.DOLocalMoveY(transform.position.y + moveY, 0.1f);  //向上移动
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            mouseOnCard = false;
            mouseTimer = 0;
            descriptionPanel.transform.DOLocalMoveX(0, 0.1f);
            transform.DOScale(new Vector2(1f, 1f), 0.1f);      //变小
            canBeDrag = false;
            transform.DOLocalMoveY(CardPack.cardPackHigh, 0.1f)
                .OnComplete(() => { canBeDrag = true; });  //向下移动
        }

    }

    public void OnDrag(PointerEventData eventData) //卡牌拖拽效果
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
            if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 200)  //在垃圾桶范围内
            {
                this.transform.DOMove(cardTrash.transform.position, 0.5f).OnComplete(() => { CardPack.DeleteCard(this); Destroy(this.gameObject); });
                AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[6]);
            }
            else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)
            {
                canBeDrag = false;
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

}

public class AccidentCard : Card
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 350)  //在垃圾桶范围内
            {
                SignUI.instance.DisplayText("You can't broke it", 1f, Color.red);
                CardPack.SortCard();
            }
            else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)  //在使用
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

public class RangeUsageCard : Card, IAffectBlock //范围使用的卡牌
{
    public bool isSelect = false;

    public virtual void AffectBlock(int[] block) //对区块的作用函数
    {
        Debug.Log("对区块" + block);
    }

    public virtual IEnumerator SelectBlock()
    {
        while (isSelect == true)
        {
            CardPack.canBeDrag = false;
            //Debug.Log("现在指着区块" + BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition()));
            if (Input.GetMouseButtonDown(0))
            {
                if (MouseOnSphere.instance.ReturnMousePosition() != Vector3.zero)
                {
                    int[] block = new int[1]; 
                    block[0] = BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition());
                    AffectBlock(block);
                    SignUI.instance.SetTextNULL();
                    CardPack.canBeDrag = true;  //其他卡牌能被拖动
                    Destroy(this.gameObject);
                }
            }
            yield return null;
        }
    }

    public override void BeUse()
    {
        isSelect = true;
        SignUI.instance.DisplayText("选择你要作用的区块", true, Color.red);
        CardPack.canBeDrag = false;  //其他卡牌不能被拖动
        StartCoroutine(SelectBlock()); 
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[5]);
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

public class AccidentRangeUsageCard : AccidentCard, IAffectBlock //范围使用的意外牌
{
    public bool isSelect = false;
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (canBeDrag && CardPack.canBeDrag)
        {
            if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 350)  //在垃圾桶范围内
            {
                SignUI.instance.DisplayText("你无法摧毁这张意外卡", 1f, Color.red);
                CardPack.SortCard();
            }
            else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)  //在使用
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


    public virtual void AffectBlock(int[] block) //对区块的作用函数
    {
        Debug.Log(block);
    }

    IEnumerator SelectBlock()
    {
        
        while (isSelect == true)
        {
            CardPack.canBeDrag = false;
            if (Input.GetMouseButtonDown(0))
            {
                if (MouseOnSphere.instance.ReturnMousePosition() != Vector3.zero)
                {
                    int[] block = new int[1];
                    block[0] = BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition());
                    AffectBlock(block);
                    SignUI.instance.SetTextNULL();
                    CardPack.canBeDrag = true;  //其他卡牌能被拖动
                    Destroy(this.gameObject);
                }
            }
            yield return null;
        }
    }

    public override void BeUse()
    {
        isSelect = true;
        SignUI.instance.DisplayText("选择你要作用的区块", true, Color.red);
        CardPack.canBeDrag = false;  //其他卡牌不能被拖动
        StartCoroutine(SelectBlock());
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[5]);
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
    public void AffectBlock(int[] block);

}

