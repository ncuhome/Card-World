using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static System.Net.Mime.MediaTypeNames;
using System;
using Text = UnityEngine.UI.Text;

public class EraSignUI : MonoBehaviour
{
    private Text text;
    public static EraSignUI instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        instance.text = instance.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayText(string newText, float time, Color color)  //提醒玩家的UI的展示
    {
        StartCoroutine(FadeCoroutine(0.2f));
        instance.text.text = newText;
        instance.text.color = color;
        Invoke("SetTextNULL", time);
    }
    public void DisplayText(string newText, bool permanent, Color color)  //提醒玩家的UI的展示(长期展示)
    {
        if (permanent)
        {
            StartCoroutine(FadeCoroutine(0.2f));
            instance.text.text = newText;
            instance.text.color = color;
        }
    }
    public void SetTextNULL()
    {
        instance.text.text = string.Empty;
    }

    IEnumerator FadeCoroutine(float fadeTime) //用协程做出淡入淡出效果
    {
        float waitTime = 0;
        while (waitTime < 1)
        {
            instance.text.color = new Color(instance.text.color.r, instance.text.color.g, instance.text.color.b, waitTime);
            yield return null;
            waitTime += Time.deltaTime / fadeTime;
        }
    }
}
