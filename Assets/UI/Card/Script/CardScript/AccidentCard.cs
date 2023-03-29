using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AccidentCard : Card
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (Vector2.Distance(this.transform.position, cardTrash.transform.position) < 350)  //ÔÚÀ¬»øÍ°·¶Î§ÄÚ
        {
            SignUI.instance.DisplayText("You can't broke it", 1f, Color.red);
            CardPack.SortCard();
        }
        else if (this.GetComponent<RectTransform>().anchoredPosition.y >= CardPack.cardPackHigh + Card.cardSizey + 50)
        {
            this.canBeDrag = false;
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
