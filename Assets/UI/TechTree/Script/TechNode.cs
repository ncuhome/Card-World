using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechNode : MonoBehaviour
{
    public string nodeName;  //�ÿƼ�������

    public bool unlock;  //�Ƿ��Ѿ�����

    public bool DetectUnlocked()
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

    }
    void Start()
    {
        nodeName = this.gameObject.name; //����Ϊ���ؽű������������
    }

    void Update()
    {
        
    }
}
