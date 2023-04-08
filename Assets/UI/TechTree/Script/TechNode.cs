using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechNode : MonoBehaviour
{
    public string nodeName;  //�ÿƼ�������

    public bool unlock = false;  //�Ƿ��Ѿ�����

    [SerializeField]private List<TechNode> frontTechnology = null;
    public bool DetectUnlocked()  //���ÿƼ��Ƿ��Ѿ�����
    {
        if (unlock)  //�Ѿ�����
        {
            return true;
        }
        else        //û�н���
        {
            return false;
        }
    }
    public void UnlockIt() //��������Ƽ�
    {
        int count = 0;
        for (int i = 0; i < frontTechnology.Count; i++)
        {
            if (frontTechnology[i].unlock)
            {
                count++;
            }
            else
            {
                SignUI.instance.DisplayText("ǰ�ÿƼ�" + frontTechnology[i].nodeName + "δ����", 3f, Color.red);
            }
        }

        if (count == frontTechnology.Count) //ǰ�ÿƼ���������
        {
            this.unlock = true;
            this.gameObject.GetComponent<Image>().color = new Color(255, 0, 0, 0.5f);
            SignUI.instance.DisplayText("���Ѿ�����" + this.nodeName, 3f, Color.blue);
        }
    }
    //void Start()
    //{
    //    nodeName = this.gameObject.name; //����Ϊ���ؽű������������
    //}

    void Update()
    {
        
    }
}
