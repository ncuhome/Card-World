using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherParticleSystem : MonoBehaviour
{
    [SerializeField] private GameObject rainParticle;
    [SerializeField] private GameObject snowParticle;
    [SerializeField] private GameObject hotParticle;
    [SerializeField] private GameObject coldParticle;
    private GameObject particleSystemManger;
    public static WeatherParticleSystem instance;
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        particleSystemManger = this.gameObject;
        InvokeRepeating("RandomRain", 0f, 60);
        InvokeRepeating("RandomRain", 10f, 65);
        InvokeRepeating("RandomSnow", 20f, 70);
        InvokeRepeating("RandomHot", 5f, 75);
        InvokeRepeating("RandomCold", 8f, 76);
    }
    public void RandomWeather()   //随机区块生成天气(两个下雨，一个下雪)
    {

    }

    //指定区块指定下雨时间下雨（大雨，中雨，小雨随机）  randomNum 1为小雨 2为中雨 3为大雨
    public void Rain(int blockNum, float weatherTime, int randomNum)           
    {
        if (!BlockSystem.Instance.blocks[blockNum].isWeather)
        {
            BlockSystem.Instance.blocks[blockNum].isWeather = true;
            Vector3 targetEulerAngles = BlockSystem.Instance.ReturnBlockAngles(blockNum);
            GameObject weatherEffect = Instantiate(rainParticle, particleSystemManger.transform);
            weatherEffect.transform.localEulerAngles = targetEulerAngles;
            ParticleSystem rainParticleSystem = weatherEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = rainParticleSystem.main;
            rainParticleSystem.Stop(true);
            main.duration = weatherTime;   //改变粒子效果持续时间
            rainParticleSystem.Play(true);
            ParticleSystem.EmissionModule emission = rainParticleSystem.emission;
            emission.rateOverTime = 10 * randomNum;         //改变粒子发射速度
            rainParticleSystem.trigger.SetCollider(1, GameObject.Find("Planet").transform);
            int[] array = new int[1];
            array[0] = blockNum;
            StartCoroutine(BlockSystem.Instance.NauAffectBlock(0, (float)(0.05 * randomNum), 0, weatherTime + 3f, weatherEffect, array));  //对区块的水量等参数产生影响
        }
    }

    //指定区块指定下雪时间下雪
    public void Snow(int blockNum, float weatherTime)
    {
        if (!BlockSystem.Instance.blocks[blockNum].isWeather)
        {
            BlockSystem.Instance.blocks[blockNum].isWeather = true;
            Vector3 targetEulerAngles = BlockSystem.Instance.ReturnBlockAngles(blockNum);
            GameObject weatherEffect = Instantiate(snowParticle, particleSystemManger.transform);
            weatherEffect.transform.localEulerAngles = targetEulerAngles;
            ParticleSystem snowParticleSystem = weatherEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = snowParticleSystem.main;
            snowParticleSystem.Stop(true);
            main.duration = weatherTime;   //改变粒子效果持续时间
            snowParticleSystem.Play(true);
            snowParticleSystem.trigger.SetCollider(1, GameObject.Find("Planet").transform);
            int[] array = new int[1];
            array[0] = blockNum;
            StartCoroutine(BlockSystem.Instance.NauAffectBlock(-0.04f, 0.04f, 0, weatherTime + 4.5f, weatherEffect, array));  //对区块的水量等参数产生影响
        }
    }
    //指定区块指定酷暑时间
    public void Hot(int blockNum, float weatherTime)
    {
        if (!BlockSystem.Instance.blocks[blockNum].isWeather)
        {
            BlockSystem.Instance.blocks[blockNum].isWeather = true;
            Vector3 targetEulerAngles = BlockSystem.Instance.ReturnBlockAngles(blockNum);
            GameObject weatherEffect = Instantiate(hotParticle, particleSystemManger.transform);
            weatherEffect.transform.localEulerAngles = targetEulerAngles;
            ParticleSystem hotParticleSystem = weatherEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = hotParticleSystem.main;
            hotParticleSystem.Stop(true);
            main.duration = weatherTime;   //改变粒子效果持续时间
            hotParticleSystem.Play(true);
            int[] array = new int[1];
            array[0] = blockNum;
            StartCoroutine(BlockSystem.Instance.NauAffectBlock(0.04f, -0.1f, 0, weatherTime + 4.5f, weatherEffect, array));  //对区块的水量等参数产生影响
        }
    }

    //指定区块指定寒潮时间
    public void Cold(int blockNum, float weatherTime)
    {
        if (!BlockSystem.Instance.blocks[blockNum].isWeather)
        {
            BlockSystem.Instance.blocks[blockNum].isWeather = true;
            Vector3 targetEulerAngles = BlockSystem.Instance.ReturnBlockAngles(blockNum);
            GameObject weatherEffect = Instantiate(coldParticle, particleSystemManger.transform);
            weatherEffect.transform.localEulerAngles = targetEulerAngles;
            ParticleSystem coldParticleSystem = weatherEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = coldParticleSystem.main;
            coldParticleSystem.Stop(true);
            main.duration = weatherTime;   //改变粒子效果持续时间
            coldParticleSystem.Play(true);
            int[] array = new int[1];
            array[0] = blockNum;
            StartCoroutine(BlockSystem.Instance.NauAffectBlock(-0.05f, 0, 0, weatherTime + 4.5f, weatherEffect, array));  //对区块的水量等参数产生影响
        }
    }

    //随机区块下60s的雨
    public void RandomRain()
    {
        int randomBlock = Random.Range(0, 24);
        instance.Rain(randomBlock, 60, Random.Range(1, 3));
    }

    //随机区块下60s的雪
    public void RandomSnow()
    {
        int randomBlock = Random.Range(0, 24);
        instance.Snow(randomBlock, 60);
    }

    //随机区块产生60s的酷暑
    public void RandomHot()
    {
        int randomBlock = Random.Range(0, 24);
        instance.Hot(randomBlock, 60);
    }
    //随机区块产生60s的寒潮
    public void RandomCold()
    {
        int randomBlock = Random.Range(0, 24);
        instance.Cold(randomBlock, 60);
    }
}
