using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseOnSphere : MonoBehaviour //��ȡ��������ϵ�λ��
{
    public static MouseOnSphere instance;

    // ������������ʾ���ζ���
    public GameObject sphere;

    // ����һ��������������ʾ����Ͷ����İ뾶
    private float planetRadius;

    // ����һ��������������ʾ����Ͷ�����������
    private float maxDistance = 100f;

    // ����һ�����ڻ���Բ��LineRenderer���
    private LineRenderer lineRenderer;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        sphere = GameObject.Find("Planet");
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        planetRadius = 0.5f * sphere.transform.localScale.x;
    }
    public Vector3 ReturnMousePosition()  //��������������ϵ�����
    {
        // ��ȡ�������Ļ�ϵ�λ������
        Vector3 mousePos = Input.mousePosition;

        // ������λ������ת��Ϊһ������
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // ����һ�������������洢��ײ��Ϣ
        RaycastHit hit;

        // �����ߵ���㷢��һ������Ͷ���壬������κ���ײ���ཻ���ͽ���ײ��Ϣ��ֵ�� hit ������������ true
        if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance))
        {
            // �����ײ����ײ���Ƿ������ζ������ײ��
            if (hit.collider == sphere.GetComponent<SphereCollider>())
            {
                // ��ȡ���ζ����ϵ���ײ������
                Vector3 hitPoint = hit.point;
                Debug.Log("��������Ϊ" + hitPoint);
                return hitPoint;
            }
        }
        Debug.Log("��������û��������");
        return Vector3.zero;  //���û��������
    }
}
