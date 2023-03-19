using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图显示
/// 将噪声贴图转换为纹理 
/// </summary>
public class MapDisplay_ZH : MonoBehaviour
{
    [Header("纹理渲染")]
    public Renderer _TextureRender;
    [Header("网格数据")]
    public MeshFilter _MeshFilter;
    [Header("网格渲染")]
    public MeshRenderer _MeshRender;

    /// <summary>
    /// 噪声地图绘制
    /// </summary>
    /// <param 柏林噪声="_NoiseMap"></param>
    public void DrawNoiseMap(float[,] _NoiseMap)
    {
        //地图宽度
        int _Width = _NoiseMap.GetLength(0);
        //地图高度
        int _Height = _NoiseMap.GetLength(1);

        //根据传入值  确定渲染纹理长宽
        Texture2D _Texture = new Texture2D(_Width, _Height);

        //获取噪波数组中的所有值
        Color[] _ColourMap = new Color[_Width * _Height];


        //设置每个像素点的颜色
        for (int y = 0; y < _Width; y++)
        {
            for (int x = 0; x < _Height; x++)
            {
                //色彩取样
                // Color 是一维数组 噪声是二维数组
                //获取当前像素点所在位置 ： Y值 乘 地图宽度 再加上 X  就是当前像素在噪声地图中的位置
                //当前取样点的颜色 
                //由于只想要 黑白色域  所以使用 Color.black 和 Color.white
                _ColourMap[y * _Width + x] = Color.Lerp(Color.black, Color.white, _NoiseMap[x, y]);
            }
        }
        //色彩传递
        //纹理贴图赋值
        _Texture.SetPixels(_ColourMap);
        _Texture.Apply();

        //主纹理贴图赋值
        _TextureRender.sharedMaterial.mainTexture = _Texture;
        //地图大小赋值
        //_TextureRender.transform.localScale = new Vector3(_Width, 1, _Height);
    }

    /// <summary>
    /// Texture 地图绘制
    /// </summary>
    /// <param 图像="_Texture"></param>
    public void DrawTexture(Texture2D _Texture)
    {
        //渲染贴图赋予
        _TextureRender.sharedMaterial.mainTexture = _Texture;
        //渲染物体大小设置
        //_TextureRender.transform.localScale = new Vector3(_Texture.width, 1, _Texture.height);
    }
    /// <summary>
    /// 网格地形绘制
    /// </summary>
    /// <param 地形网格="_MeshData"></param>
    /// <param 地形贴图="_Texture2D"></param>
    internal void DrawMesh(MeshData _MeshData, Texture2D _Texture2D)
    {
        _MeshFilter.sharedMesh = _MeshData.CreteMesh();

        _MeshRender.sharedMaterial.mainTexture = _Texture2D;
    }
}
