using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState { idle, wait, walk, work }
public class Character : MonoBehaviour
{
    public CharacterState characterState = CharacterState.idle;
    public Vector3 targetEuler;
    public int targetBlock;
    public bool isCharacter = false;
    public float turnSpeed = 1f;
    public Quaternion targetQua;
    public Quaternion oldQua;
    private float time;
    private Transform center;
    public Quaternion nextQua;
    public Vector3 normalVec;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Planet").transform;
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
            case CharacterState.idle:
                do
                {
                    targetEuler = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                    oldQua = transform.rotation;
                    targetQua = Quaternion.Euler(targetEuler);
                    targetBlock = BlockSystem.Instance.GetBlockNum(center.position, targetQua);
                }
                while (!RoadCanMove());
                if (RoadCanMove())
                {
                    // turnSpeed = 20f / Quaternion.Angle(oldQua, targetQua);
                    //normalVec = Vector3.Cross(transform.eulerAngles, targetEuler);
                    //angle = Quaternion.Angle(oldQua, targetQua);
                    turnSpeed = 0.5f;
                    time = 0;
                    characterState = CharacterState.walk;
                }
                break;
            case CharacterState.wait:
                time += Time.deltaTime;
                if (time > 0.3f)
                {
                    characterState = CharacterState.walk;
                    time = 0;
                }
                break;
            case CharacterState.walk:
                time += Time.deltaTime;
                //  用 slerp 进行插值平滑的旋转
                nextQua = Quaternion.Slerp(oldQua, targetQua, turnSpeed * time);
                //nextQua = Quaternion.AngleAxis(20 * time, normalVec);
                // if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(nextQua.eulerAngles), ColorSystem.Instance.colors[0]) < 0.1f)
                // {
                //     characterState = CharacterState.idle;
                //     break;
                // }
                transform.rotation = nextQua;
                // 当初始角度跟目标角度小于1,将目标角度赋值给初始角度,让旋转角度是我们需要的角度
                if (Quaternion.Angle(targetQua, transform.rotation) < 0.1)
                {
                    transform.rotation = targetQua;
                    characterState = CharacterState.idle;
                }
                break;
            case CharacterState.work:
                break;
        }
        //Debug.DrawLine(Vector3.zero, oldQua.eulerAngles, Color.green);
        //Debug.DrawLine(Vector3.zero, targetEuler, Color.blue);
        //Debug.DrawLine(transform.position, transform.up * 1000, Color.yellow);
        //Debug.DrawLine(Vector3.zero, normalVec, Color.red);
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
