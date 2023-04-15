using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TechTreeDrag : MonoBehaviour , IDragHandler
{
    //屏幕的宽和高
    public float width;
    public float height;
    private Canvas techTreeUI;
    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.transform.localPosition.x > 690)
        {
            if(eventData.delta.x < 0)
            {
                gameObject.transform.localPosition +=
            new Vector3(eventData.delta.x , 0, 0);
            }
        }
        else if (gameObject.transform.localPosition.x <= 0)
        {
            if (eventData.delta.x > 0)
            {
                gameObject.transform.localPosition +=
            new Vector3(eventData.delta.x, 0, 0);
            }
        }
        else
        {
            this.GetComponent<RectTransform>().localPosition +=
            new Vector3(eventData.delta.x, 0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        techTreeUI = GameObject.Find("TechTreeUIPanel").GetComponent<Canvas>();
        width = GameObject.Find("TechTreeUIPanel").GetComponent<RectTransform>().rect.width;
        height = GameObject.Find("TechTreeUIPanel").GetComponent<RectTransform>().rect.height;
        gameObject.transform.localPosition = 
            new Vector3(700, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
