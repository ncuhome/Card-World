using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TechTreeDrag : MonoBehaviour , IDragHandler
{
    //ÆÁÄ»µÄ¿íºÍ¸ß
    public float width;
    public float height;
    private Canvas techTreeUI;
    public void OnDrag(PointerEventData eventData)
    {
        if (this.GetComponent<RectTransform>().localPosition.x >= (gameObject.GetComponent<RectTransform>().rect.width - width) / 2)
        {
            if(eventData.delta.x < 0)
            {
                this.GetComponent<RectTransform>().localPosition +=
            new Vector3(eventData.delta.x / techTreeUI.scaleFactor, 0, 0);
            }
        }
        else if (this.GetComponent<RectTransform>().localPosition.x <= -(gameObject.GetComponent<RectTransform>().rect.width - width) / 2)
        {
            if (eventData.delta.x > 0)
            {
                this.GetComponent<RectTransform>().localPosition +=
            new Vector3(eventData.delta.x / techTreeUI.scaleFactor, 0, 0);
            }
        }
        else
        {
            this.GetComponent<RectTransform>().localPosition +=
            new Vector3(eventData.delta.x / techTreeUI.scaleFactor, 0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        techTreeUI = GameObject.Find("TechTreeUIPanel").GetComponent<Canvas>();
        width = GameObject.Find("TechTreeUIPanel").GetComponent<RectTransform>().rect.width;
        height = GameObject.Find("TechTreeUIPanel").GetComponent<RectTransform>().rect.height;
        gameObject.GetComponent<RectTransform>().localPosition = 
            new Vector3((gameObject.GetComponent<RectTransform>().rect.width - width) / 2, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
