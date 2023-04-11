using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState { idle, wait, walk, work, gather, build, sleep }
public enum SpecialSkill { None = -1, Hunting, Farming, Stargazing, AstronomicalObservation, Leading, Alchemy, Smelt, Refining, Navigation, OceanSailing, AerospaceResearch, GenerateElectricity }
public class Character : MonoBehaviour
{
    public CharacterState characterState = CharacterState.idle;
    public int targetBlock;
    private bool isCharacter = false;
    public float turnSpeed = 20f;
    public Quaternion targetQua, oldQua, nextQua, curQua;
    public float time;
    private Transform center;
    private Vector3 axisVec, targetVec, oldVec;
    public float angle;
    public Item item;
    public bool foundResource, goToBuild, finishBuilding;
    public GameObject resourceObject;
    public int characterNum;
    public GameObject buildingObject;
    public float age;
    public float ageSpeed;
    public int[] homeRange;
    private Color pixelColor;
    public SpecialSkill specialSkill = SpecialSkill.None;
    public MeshRenderer itemSprite;
    public bool goHome = false;
    public Building home;
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Planet").transform;
        itemSprite = item.transform.Find("ItemSprite").GetComponent<MeshRenderer>();
        if (item.itemType == ItemType.Character)
        {
            isCharacter = true;
            homeRange = BlockSystem.Instance.GetRandomNearBlock(center.position, BlockSystem.Instance.GetBlockNum(center.position, transform.rotation));
        }
        else
        {
            isCharacter = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        curQua = transform.rotation;
        pixelColor = GetColorSystem.Instance.GetColor(transform.rotation * Vector3.up);
        if (!isCharacter)
        {
            return;
        }


        AddAge();
        GoHome();

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
                    targetVec = targetQua * Vector3.up;
                    oldVec = oldQua * Vector3.up;
                    targetBlock = BlockSystem.Instance.GetBlockNum(center.position, targetQua);
                    //targetEuler = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                    //targetQua = Quaternion.Euler(targetEuler);
                    //targetBlock = BlockSystem.Instance.GetBlockNum(center.position, targetQua);
                }
                while (!RoadCanMove(targetBlock));
                //获取完毕后进行旋转方向的判定
                if (RoadCanMove(targetBlock))
                {
                    //nextQua = oldQua;
                    //angle = Quaternion.Angle(oldQua, targetQua);
                    //Debug.Log("Start" + targetQua + " " + nextQua + " " + transform.rotation + " " + transform.eulerAngles);
                    if (angle != 0)
                    {
                        turnSpeed = Mathf.Abs(turnSpeed) * angle / Mathf.Abs(angle);
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
                // if (ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(nextQua * Vector3.up), ColorSystem.Instance.colors[0]) < 0.1f)
                // {
                //     characterState = CharacterState.idle;
                //     nextQua = transform.rotation;
                //     time = 0;
                // }
                transform.rotation = nextQua;
                //当初始角度跟目标角度小于0.1,将目标角度赋值给初始角度,让旋转角度是我们需要的角度
                //if (Vector3.Angle(oldVec, targetVec) < 1f)
                if (time > angle / turnSpeed)
                {
                    time = 0;
                    transform.rotation = targetQua;
                    if (goHome)
                    {
                        characterState = CharacterState.sleep;
                    }
                    else if (foundResource)
                    {
                        characterState = CharacterState.gather;
                    }
                    else if (goToBuild)
                    {
                        characterState = CharacterState.build;
                    }
                    else
                    {
                        characterState = CharacterState.idle;
                    }
                }
                break;
            case CharacterState.work:
                break;
            case CharacterState.gather:
                time += Time.deltaTime;
                if (time > 5f)
                {
                    if (resourceObject != null)
                    {
                        ResourceSystem.Instance.GatherResource(this, resourceObject);
                        resourceObject = null;
                    }
                    foundResource = false;
                    characterState = CharacterState.idle;
                }
                break;
            case CharacterState.build:
                time += Time.deltaTime;
                if (time > 5f)
                {
                    age += 20f;
                    if (buildingObject != null)
                    {
                        buildingObject.transform.localScale = Vector3.one;
                        buildingObject.GetComponent<Building>().finishBuilding = true;
                    }
                    if (buildingObject.GetComponent<Building>().finishBuilding)
                    {
                        goToBuild = false;
                        characterState = CharacterState.idle;
                        buildingObject = null;
                    }
                }
                break;
            case CharacterState.sleep:
                if (goHome == false)
                {
                    characterState = CharacterState.idle;
                }
                break;
        }
        //Debug.DrawLine(Vector3.zero, oldVec, Color.green);
        //Debug.DrawLine(Vector3.zero, targetVec, Color.blue);
        // Debug.DrawLine(transform.position, transform.rotation * Vector3.up * 1000, Color.yellow);
        // Debug.DrawLine(Vector3.zero, axisVec * 1000, Color.red);
    }

    //判断目的地是否在角色活动范围内
    public bool RoadCanMove(int block)
    {
        if (System.Array.IndexOf(homeRange, block) == -1)
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

    //向给定四元数走过去
    public void WalkToTargetQua(Quaternion newTargetQua)
    {
        int newTargetBlock = BlockSystem.Instance.GetBlockNum(center.position, newTargetQua);
        if (RoadCanMove(newTargetBlock))
        {
            targetVec = newTargetQua * Vector3.up;
            oldVec = transform.rotation * Vector3.up;
            axisVec = Vector3.Cross(oldVec, targetVec);
            oldQua = transform.rotation;
            targetQua = newTargetQua;
            //Debug.Log("TargetQua:" + targetQua);
            angle = Vector3.Angle(oldVec, targetVec);
            if (angle != 0)
            {
                turnSpeed = Mathf.Abs(turnSpeed) * angle / Mathf.Abs(angle);
            }
            time = 0;
            characterState = CharacterState.wait;
        }
    }

    //角色死亡
    public void Die()
    {
        CharacterSystem.Instance.characters[characterNum] = null;
        Destroy(this.gameObject);
    }

    //增加年龄
    public void AddAge()
    {
        if (goHome)
        {
            ageSpeed = 0f;
        }
        else if ((ColorSystem.ColorExt.Difference(pixelColor, ColorSystem.Instance.colors[1]) < 0.01f)
         || (ColorSystem.ColorExt.Difference(pixelColor, ColorSystem.Instance.colors[3]) < 0.01f))
        {
            ageSpeed = 1f;
        }
        else
        {
            ageSpeed = 0.5f;
        }
        age += Time.deltaTime * ageSpeed;
        if ((age > CharacterSystem.Instance.maxAge) && (!goToBuild))
        {
            Die();
        }
    }

    //根据最近的居住建筑是否被光照判断是否回家
    public void GoHome()
    {
        if (goToBuild) { return; }
        home = BuildingSystem.Instance.FindNearHome(transform);
        if (home == null) { return; }
        if (home.GetComponent<LightProbes>().illumination == Illumination.day)
        {
            goHome = false;
        }
        else
        {
            if (goHome == false)
            {
                WalkToTargetQua(home.transform.rotation);
            }
            goHome = true;
        }
    }



}
