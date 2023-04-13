using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseOnSphere : MonoBehaviour //读取鼠标在球上的位置
{
    public static MouseOnSphere instance;

    // 声明变量，表示球形对象
    public GameObject sphere;

    // 声明一个公共变量，表示球形投射体的半径
    private float planetRadius;

    // 声明一个公共变量，表示球形投射体的最大距离
    private float maxDistance = 100f;

    // 定义一个用于绘制圆的LineRenderer组件
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
    public Vector3 ReturnMousePosition()  //返回鼠标在球体上的坐标
    {
        // 获取鼠标在屏幕上的位置坐标
        Vector3 mousePos = Input.mousePosition;

        // 将鼠标的位置坐标转换为一条射线
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // 声明一个变量，用来存储碰撞信息
        RaycastHit hit;

        // 从射线的起点发射一个球形投射体，如果与任何碰撞器相交，就将碰撞信息赋值给 hit 变量，并返回 true
        if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance))
        {
            // 检查碰撞的碰撞器是否是球形对象的碰撞器
            if (hit.collider == sphere.GetComponent<MeshCollider>())
            {
                // 获取球形对象上的碰撞点坐标
                Vector3 hitPoint = hit.point;
                Debug.Log("鼠标的坐标为" + hitPoint);
                return hitPoint;
            }
        }
        Debug.Log("鼠标的坐标没在星球上");
        return Vector3.zero;  //鼠标没碰到球体
    }
}
