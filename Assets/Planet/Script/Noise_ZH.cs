using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 柏林噪声地图生成
/// </summary>
public static class Noise_ZH
{

    /// <summary>
    /// 柏林噪声地图生成方法
    /// </summary>
    /// <param 地图宽度="_MapWidth"></param>
    /// <param 地图高度="_MapHeight"></param>
    /// <param 地图大小="_Scale"></param>
    /// <param 种子="_Seed"></param>
    /// <param 抵消="_Octaves"></param>
    /// <param 持续="_Persistance"></param>
    /// <param 空隙="_Lacunarity"></param>
    /// <param 偏移抵消="_Offset"></param>/// 
    /// <returns></returns>
    public static float[,] GenerateNoiseMap(int _MapWidth, int _MapHeight, float _Scale, int _Seed, int _Octaves, float _Persistance, float _Lacunarity, Vector2 _Offset)
    {
        //噪声地图
        float[,] _NoiseMap = new float[_MapWidth, _MapHeight];

        //地图种子随机
        System.Random _Prng = new System.Random(_Seed);

        //抵消
        Vector2[] _OctaveOffsets = new Vector2[_Octaves];
        for (int i = 0; i < _Octaves; i++)
        {
            float _OffsetX = _Prng.Next(-10000, 10000) + _Offset.x;
            float _OffsetY = _Prng.Next(-10000, 10000) + _Offset.y;
            _OctaveOffsets[i] = new Vector2(_OffsetX, _OffsetY);
        }

        //避免地图  不存在
        if (_Scale <= 0)
        {
            _Scale = 0.0001f;
        }

        //地图放大响应
        float _HalfWidth = _MapWidth / 2;
        float _HalfHeight = _MapHeight / 2;


        //最大噪声高度
        float _MaxNoiseHeight = float.MinValue;
        //最小噪声高度
        float _MinNoiseHeight = float.MaxValue;


        for (int y = 0; y < _MapHeight; y++)
        {
            for (int x = 0; x < _MapWidth; x++)
            {
                //振幅
                float _Amplitude = 1;
                //频率
                float _Frequency = 1;
                //噪波高度
                float _NoiseHeight = 0;

                for (int i = 0; i < _Octaves; i++)
                {

                    //地图单元取整
                    float _SampleX = (x - _HalfWidth) / _Scale * _Frequency + _OctaveOffsets[i].x;
                    float _SampleY = (y - _HalfHeight) / _Scale * _Frequency + _OctaveOffsets[i].y;

                    //根据传入参数 生成2D柏林噪声
                    float _PerlinValue = Mathf.PerlinNoise(_SampleX, _SampleY) * 2 - 1;
                    //噪波高度等于 柏林噪声 乘于 振幅
                    _NoiseHeight += _PerlinValue * _Amplitude;

                    //振幅变更
                    _Amplitude *= _Persistance;
                    //频率变更
                    _Frequency *= _Lacunarity;
                }

                //限值操作
                if (_NoiseHeight > _MaxNoiseHeight)
                {
                    _MaxNoiseHeight = _NoiseHeight;
                }
                else if (_NoiseHeight < _MinNoiseHeight)
                {
                    _MinNoiseHeight = _NoiseHeight;
                }

                //传入地图数据
                _NoiseMap[x, y] = _NoiseHeight;
            }
        }

        //地图左右边界置零并且各点插值降低
        for (int y = 0; y < _MapHeight; y++)
        {
            float _NoiseHeight = 0f;
            for (int x = Mathf.FloorToInt(_HalfWidth); x >= 0; x--)
            {
                _NoiseHeight = _NoiseMap[x, y] - (_NoiseMap[0, y] - _MinNoiseHeight) * Mathf.Exp((-x / _HalfWidth) * 10);
                if (_NoiseHeight > _MaxNoiseHeight)
                {
                    _NoiseHeight = _MaxNoiseHeight;
                }
                else if (_NoiseHeight < _MinNoiseHeight)
                {
                    _NoiseHeight = _MinNoiseHeight;
                }
                _NoiseMap[x, y] = _NoiseHeight;
            }
            for (int x = Mathf.FloorToInt(_HalfWidth); x < _MapWidth; x++)
            {
                _NoiseHeight = _NoiseMap[x, y] - (_NoiseMap[_MapWidth - 1, y] - _MinNoiseHeight) * Mathf.Exp((x / _HalfWidth - 2) * 10);
                if (_NoiseHeight > _MaxNoiseHeight)
                {
                    _NoiseHeight = _MaxNoiseHeight;
                }
                else if (_NoiseHeight < _MinNoiseHeight)
                {
                    _NoiseHeight = _MinNoiseHeight;
                }
                _NoiseMap[x, y] = _NoiseHeight;
            }
        }


        //地图上下边界置1，并且各点插值升高
        for (int x = 0; x < _MapWidth; x++)
        {
            float _NoiseHeight = 0f;
            for (int y = Mathf.FloorToInt(_HalfHeight); y < _MapHeight; y++)
            {
                _NoiseHeight = _NoiseMap[x, y] + (_MaxNoiseHeight - _NoiseMap[x, _MapHeight - 1]) * Mathf.Exp(((y + 200) / _HalfHeight - 2) * 5);
                if (_NoiseHeight > _MaxNoiseHeight)
                {
                    _NoiseHeight = _MaxNoiseHeight;
                }
                else if (_NoiseHeight < _MinNoiseHeight)
                {
                    _NoiseHeight = _MinNoiseHeight;
                }
                _NoiseMap[x, y] = _NoiseHeight;
            }
            for (int y = Mathf.FloorToInt(_HalfHeight); y >= 0; y--)
            {
                _NoiseHeight = _NoiseMap[x, y] + (_MaxNoiseHeight - _NoiseMap[x, 0]) * Mathf.Exp((-(y - 200) / _HalfHeight) * 5);
                if (_NoiseHeight > _MaxNoiseHeight)
                {
                    _NoiseHeight = _MaxNoiseHeight;
                }
                else if (_NoiseHeight < _MinNoiseHeight)
                {
                    _NoiseHeight = _MinNoiseHeight;
                }
                _NoiseMap[x, y] = _NoiseHeight;
            }

        }

        

        //噪声贴图 最大值最小值 限定输出
        //图像叠加
        for (int y = 0; y < _MapHeight; y++)
        {
            for (int x = 0; x < _MapWidth; x++)
            {
                _NoiseMap[x, y] = Mathf.InverseLerp(_MinNoiseHeight, _MaxNoiseHeight, _NoiseMap[x, y]);
            }
        }


        //返回地图数据
        return _NoiseMap;
    }

}