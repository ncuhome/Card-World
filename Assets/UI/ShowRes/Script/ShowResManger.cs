using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowResManger : MonoBehaviour
{
    public TextMeshProUGUI[] resTextMesh;

    private GameObject foldRes;
    void Update()
    {
        for (int i = 0; i < resTextMesh.Length; i++)
        {
            resTextMesh[i].text = ResourceSystem.Instance.resourceDatas[i].resourceNum.ToString();
        }
    }
    private void Start()
    {
        foldRes = GameObject.Find("OtherRes");
        foldRes.SetActive(false);
    }
    public void FoldButton()
    {
        if (foldRes.activeInHierarchy == false)
        {
            foldRes.SetActive(true);
        }
        else if (foldRes.activeInHierarchy == true)
        {
            foldRes.SetActive(false);
        }
    }
}
