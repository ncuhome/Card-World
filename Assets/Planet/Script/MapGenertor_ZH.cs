using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  地图管理
/// </summary>
public class MapGenertor_ZH : MonoBehaviour
{
    /// <summary>
    /// 地图显示类型枚举
    /// </summary>
    public enum DrawMap { NoiseMap, ColourMap, MeshMap }

    [Header("地图绘制类型")]
    public DrawMap _DrawMap = DrawMap.NoiseMap;

    [Header("地图宽度")]
    public int _MapWidth;
    [Header("地图高度")]
    public int _MapHeight;
    [Header("地图大小")]
    public float _NoiseScale;

    [Header("八度")]
    public int _Octaves;
    [Header("持续")]
    [Range(0, 1)]
    public float _Persistance;
    [Header("空隙")]
    public float _Lacunarity;
    [Header("地图种子")]
    public int _Seed;
    [Header("偏移")]
    public Vector2 _Offset;

    [Header("地形区域")]
    public TerrainType[] _Regions;


    [Header("自动更新布尔")]
    public bool _AutoUpdate;

    /// <summary>
    /// 地图生成 调用
    /// </summary>
    public void GenerateMap()
    {
        //参数传递 
        float[,] _NoiseMap = Noise_ZH.GenerateNoiseMap(_MapWidth, _MapHeight, _NoiseScale, _Seed, _Octaves, _Persistance, _Lacunarity, _Offset);

        //色彩地图
        Color[] _ColourMap = new Color[_MapWidth * _MapHeight];

        //根据地形高度进行色彩赋予
        for (int y = 0; y < _MapHeight; y++)
        {
            for (int x = 0; x < _MapWidth; x++)
            {
                //获取当前地图高度值
                float _CurrentHeight = _NoiseMap[x, y];
                //进行地形区域判定
                for (int i = 0; i < _Regions.Length; i++)
                {
                    if (_CurrentHeight <= _Regions[i]._Height)
                    {
                        //色彩赋值
                        _ColourMap[y * _MapWidth + x] = _Regions[i]._Colour;
                        break;
                    }
                }

            }
        }

        //查找 MapDisplay_ZH 搭载物体
        MapDisplay_ZH _Display = FindObjectOfType<MapDisplay_ZH>();

        //根据不同类型进行不同地图绘制
        if (_DrawMap == DrawMap.NoiseMap)
        {
            //进行柏林噪声地形绘制
            _Display.DrawTexture(TextureGenerator_ZH.TextureFromHeightMap(_NoiseMap));
        }
        else if (_DrawMap == DrawMap.ColourMap)
        {
            //进行色彩贴图地形绘制
            _Display.DrawTexture(TextureGenerator_ZH.TextureFromColourMap(_ColourMap, _MapWidth, _MapHeight));
        }
        else if (_DrawMap == DrawMap.MeshMap)
        {
            //进行网格地形绘制
            _Display.DrawMesh(MeshGenerator_ZH.GenerateTerrainMesh(_NoiseMap), TextureGenerator_ZH.TextureFromColourMap(_ColourMap, _MapWidth, _MapHeight));
        }

    }

    public void Start()
    {
        _Seed = Random.Range(-1000,1000);
        GenerateMap();
    }

    /// <summary>
    /// 当脚本被加载或检查器在值被修改时调用此函数时(仅在编辑器调用中)
    /// </summary>
    private void OnValidate()
    {
        //保证初始化显示
        if (_MapWidth < 1)
        {
            _MapWidth = 1;
        }
        if (_MapHeight < 1)
        {
            _MapHeight = 1;
        }
        if (_Lacunarity < 1)
        {
            _Lacunarity = 1;
        }
        if (_Octaves < 0)
        {
            _Octaves = 0;
        }
    }
}

//序列化
[System.Serializable]
/// <summary>
/// 地形类型
/// </summary>
public struct TerrainType
{
    //名称
    public string _Name;
    //高度
    public float _Height;
    //颜色
    public Color _Colour;
}


