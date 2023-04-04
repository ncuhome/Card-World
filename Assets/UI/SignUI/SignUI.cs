using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static System.Net.Mime.MediaTypeNames;
using System;

public class SignUI : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    public static SignUI instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        instance.tmpText = instance.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayText(string newText, float time, Color color)  //提醒玩家的UI的展示
    {
        StartCoroutine(FadeCoroutine(0.2f));
        instance.tmpText.text = newText;
        instance.tmpText.color = color;
        Invoke("SetTextNULL", time);
    }
    public void DisplayText(string newText, bool permanent, Color color)  //提醒玩家的UI的展示
    {
        if (permanent)
        {
            StartCoroutine(FadeCoroutine(0.2f));
            instance.tmpText.text = newText;
            instance.tmpText.color = color;
        }
    }
    public void SetTextNULL()
    {
        instance.tmpText.text = string.Empty;
    }

    IEnumerator FadeCoroutine(float fadeTime) //用协程做出淡入淡出效果
    {
        float waitTime = 0;
        while (waitTime < 1)
        {
            tmpText.fontMaterial.SetColor("_FaceColor", Color.Lerp(Color.clear, Color.white, waitTime));
            yield return null;
            waitTime += Time.deltaTime / fadeTime;
        }
    }
}
