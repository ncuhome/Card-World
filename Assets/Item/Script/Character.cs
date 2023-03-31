using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState { idle, wait, walk, work, gather }
public class Character : MonoBehaviour
{
    public CharacterState characterState = CharacterState.idle;
    public int targetBlock;
    private bool isCharacter = false;
    public float turnSpeed = 20f;
    private Quaternion targetQua;
    private Quaternion oldQua;
    private float time;
    private Transform center;
    private Quaternion nextQua;
    private Vector3 axisVec;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Planet").transform;
        if (GetComponent<Item>().itemType == Item.ItemType.Character)
        {
            isCharacter = true;
        }
        else
        {
            isCharacter = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCharacter)
        {
            return;
        }

        switch (characterState)
        {
            //待机状态
            case CharacterState.idle:
                //自动获取可行的移动方向和距离
                do
                {
                    axisVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    angle = Random.Range(-60f, 60f);
                    oldQua = transform.rotation;
                    targetQua = Quaternion.AngleAxis(angle, axisVec) * oldQua;
                    targetBlock = BlockSystem.Instance.GetBlockNum(center.position, targetQua);
                    //targetEuler = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                    //targetQua = Quaternion.Euler(targetEuler);
                    //targetBlock = BlockSystem.Instance.GetBlockNum(center.position, targetQua);
                }
                while (!RoadCanMove());
                //获取完毕后进行旋转方向的判定
                if (RoadCanMove())
                {
                    //nextQua = oldQua;
                    //angle = Quaternion.Angle(oldQua, targetQua);
                    //Debug.Log("Start" + targetQua + " " + nextQua + " " + transform.rotation + " " + transform.eulerAngles);
                    if (turnSpeed * angle < 0)
                    {
                        turnSpeed = -turnSpeed;
                    }
                    time = 0;
                    characterState = CharacterState.wait;
                }
                break;
                //进入等待阶段
            case CharacterState.wait:
                time += Time.deltaTime;
                if (time > 0.3f)
                {
                    characterState = CharacterState.walk;
                    time = 0;
                }
                break;
                //进入行走阶段
            case CharacterState.walk:
                time += Time.deltaTime;
                //使用绕轴旋转
                nextQua = Quaternion.AngleAxis(turnSpeed * time, axisVec) * oldQua;
                // if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(nextQua.eulerAngles), ColorSystem.Instance.colors[0]) < 0.1f)
                // {
                //     characterState = CharacterState.idle;
                //     break;
                // }
                transform.rotation = nextQua;
                //当初始角度跟目标角度小于0.1,将目标角度赋值给初始角度,让旋转角度是我们需要的角度
                if (Quaternion.Angle(targetQua, transform.rotation) < 0.1)
                {
                    transform.rotation = targetQua;
                    characterState = CharacterState.idle;
                }
                break;
            case CharacterState.work:
                break;
            case CharacterState.gather:
                break;
        }
        //Debug.DrawLine(Vector3.zero, oldVec, Color.green);
        //Debug.DrawLine(Vector3.zero, targetVec, Color.blue);
        Debug.DrawLine(transform.position, transform.rotation * Vector3.up * 1000, Color.yellow);
        Debug.DrawLine(Vector3.zero, axisVec * 1000, Color.red);
    }

    public bool RoadCanMove()
    {
        if (targetBlock != GetComponent<Item>().blockNum)
        {
            //Debug.Log(GetComponent<Item>().blockNum + " " + targetBlock);
            return false;
        }
        // if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(targetEuler), ColorSystem.Instance.colors[0]) < 0.1f)
        // {
        //     return false;
        // }
        return true;
    }



}
