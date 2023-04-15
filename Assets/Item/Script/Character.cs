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
    public bool goHome, stayInBuilding = false;
    public Building home, targetBuilding;
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Planet").transform;
        itemSprite = item.transform.Find("ItemSpriteCenter").Find("ItemSprite").GetComponent<MeshRenderer>();
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
        ExpandScope();
        StartWork();
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
                    else if (stayInBuilding)
                    {
                        characterState = CharacterState.work;
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
                if (stayInBuilding == false)
                {
                    characterState = CharacterState.idle;
                }
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
                    age += BuildingSystem.Instance.builderAge;
                    if (buildingObject != null)
                    {
                        buildingObject.transform.localScale = Vector3.one;
                        buildingObject.GetComponent<Building>().finishBuilding = true;
                    }
                    if ((buildingObject == null) || (buildingObject.GetComponent<Building>().finishBuilding))
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
        if (CharacterSystem.Instance.GetCharacter(EraSystem.Instance.era, specialSkill).techAbility && (Random.value > 0.7f))
        {
            {
                TechNode[] allTechNode;
                List<TechNode> canBeUnlock = new List<TechNode>();
                if (EraSystem.Instance.era == Era.AncientEra)
                {
                    allTechNode = TechTree.instance.ancientEraTech;
                }
                else if (EraSystem.Instance.era == Era.ClassicalEra)
                {
                    allTechNode = TechTree.instance.classicalEraTech;
                }
                else
                {
                    allTechNode = TechTree.instance.industrialEraTech;
                }
                foreach (TechNode tech in allTechNode)
                {
                    Debug.Log(tech);
                    if (tech.CanBeUnlocked() == true && tech.unlock == false)
                    {
                        Debug.Log(tech + "second");
                        canBeUnlock.Add(tech);
                    }
                }
                int random = Random.Range(0, canBeUnlock.Count); //解锁随机一个科技
                canBeUnlock[random].ImmediateUnlockIt();
                AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[5]);
            }
        }

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
            ageSpeed = 1f + (BlockSystem.Instance.blocks[item.blockNum].water * BlockSystem.Instance.blocks[item.blockNum].temperature * 8f) / (BlockSystem.Instance.blocks[item.blockNum].water + BlockSystem.Instance.blocks[item.blockNum].temperature) / BlockSystem.Instance.blocks[item.blockNum].livability - 1;
        }
        else
        {
            ageSpeed = 0.5f + (BlockSystem.Instance.blocks[item.blockNum].water * BlockSystem.Instance.blocks[item.blockNum].temperature * 8f) / (BlockSystem.Instance.blocks[item.blockNum].water + BlockSystem.Instance.blocks[item.blockNum].temperature) / BlockSystem.Instance.blocks[item.blockNum].livability - 1;
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
        //if (EraSystem.Instance.era == Era.IndustrialEra) { return; }
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
                if (goToBuild || foundResource) { return; }
                WalkToTargetQua(home.transform.rotation);
                goHome = true;
                stayInBuilding = false;
            }
        }
    }

    public void Stay(BuildingType buildingType)
    {
        targetBuilding = BuildingSystem.Instance.FindNearBuildingWithType(transform, buildingType);
        if (targetBuilding == null)
        {
            //stayInBuilding = false;
            return;
        }
        if (stayInBuilding == false)
        {
            if (goToBuild || goHome || foundResource) { return; }
            stayInBuilding = true;
            WalkToTargetQua(targetBuilding.transform.rotation);
        }
    }

    public void StartWork()
    {
        switch (specialSkill)
        {
            case SpecialSkill.None:
                break;
            case SpecialSkill.Hunting:
                Stay(BuildingType.SheepPen);
                Stay(BuildingType.FishingFacility);
                break;
            case SpecialSkill.Farming:
                Stay(BuildingType.OriginalFarmland);
                Stay(BuildingType.Farm);
                Stay(BuildingType.ModernFarm);
                break;
            case SpecialSkill.Stargazing:
                Stay(BuildingType.Stonehenge);
                break;
            case SpecialSkill.AstronomicalObservation:
                Stay(BuildingType.Tower);
                break;
            case SpecialSkill.Leading:
                break;
            case SpecialSkill.Alchemy:
                Stay(BuildingType.AlchemyInstitute);
                break;
            case SpecialSkill.Smelt:
                Stay(BuildingType.SmallMetalWorkshop);
                break;
            case SpecialSkill.Refining:
                Stay(BuildingType.MetalSmelter);
                break;
            case SpecialSkill.Navigation:
                break;
            case SpecialSkill.OceanSailing:
                break;
            case SpecialSkill.AerospaceResearch:
                Stay(BuildingType.RocketLaunchBase);
                break;
            case SpecialSkill.GenerateElectricity:
                Stay(BuildingType.NuclearPowerPlant);
                break;

        }
    }

    public void ExpandScope()
    {
        foreach (int blockNum in homeRange)
        {
            if (BuildingSystem.Instance.buildingInBlock[blockNum] == BuildingSystem.Instance.maxBuildingInBlock)
            {
                List<int> homeList = new List<int>(homeRange);
                int[] nearBlock = BlockSystem.Instance.GetNearBlock(center.position, blockNum);
                int block1 = -1, block2 = -1;
                int min1 = 50, min2 = 50;
                for (int i = 0; i < nearBlock.Length; i++)
                {
                    if (System.Array.IndexOf(homeRange, nearBlock[i]) != -1) { continue; }
                    if (BuildingSystem.Instance.buildingInBlock[nearBlock[i]] < min1)
                    {
                        min2 = min1;
                        block2 = block1;
                        min1 = BuildingSystem.Instance.buildingInBlock[nearBlock[i]];
                        block1 = nearBlock[i];
                    }
                    else if (BuildingSystem.Instance.buildingInBlock[nearBlock[i]] < min2)
                    {
                        block2 = nearBlock[i];
                        min2 = BuildingSystem.Instance.buildingInBlock[nearBlock[i]];
                    }
                }
                if (block1 != -1)
                {
                    homeList.Add(block1);
                }
                if (block2 != -1)
                {
                    homeList.Add(block2);
                }
                homeRange = homeList.ToArray();
            }
        }
    }




}
