using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDescription : MonoBehaviour, IPointerEnterHandler
{
    Card parentCard;
    public void Start()
    {
        parentCard = gameObject.transform.parent.gameObject.GetComponent<Card>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentCard.canBeDrag && CardPack.canBeDrag && !parentCard.isDrag)
        {
            parentCard.mouseOnCard = false;
            parentCard.mouseTimer = 0;
            parentCard.descriptionPanel.transform.DOLocalMoveX(0, 0.1f);
            parentCard.transform.DOScale(new Vector2(1f, 1f), 0.1f);      //变小
            parentCard.canBeDrag = false;
            parentCard.transform.DOLocalMoveY(CardPack.cardPackHigh, 0.1f)
                .OnComplete(() => { parentCard.canBeDrag = true; });  //向下移动
        }
    }

}
