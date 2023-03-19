using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 纹理生成器
/// </summary>
public static class TextureGenerator_ZH
{
    /// <summary>
    /// 颜色地图的纹理
    /// </summary>
    /// <param 色彩地图="_ColourMap"></param>
    /// <param 宽度="_Width"></param>
    /// <param 高度="_Height"></param>
    /// <returns></returns>
    public static Texture2D TextureFromColourMap(Color[] _ColourMap, int _Width, int _Height)
    {
        Texture2D _Texture = new Texture2D(_Width, _Height);

        //绘制模式 变更
        //点过滤-纹理像素变得块状近距离
        //边缘块状显示
        //_Texture.filterMode = FilterMode.Point;
        _Texture.filterMode = FilterMode.Point;

        //纹理坐标缠绕模式
        //将纹理夹紧到边缘的最后一个像素
        _Texture.wrapMode = TextureWrapMode.Clamp;

        //色彩传递
        //纹理贴图赋值
        _Texture.SetPixels(_ColourMap);

        //必须要应用一下不然不响应
        _Texture.Apply();

        return _Texture;
    }

    /// <summary>
    /// 高度贴图纹理
    /// </summary>
    /// <param 高度贴图="_HeightMap"></param>
    /// <returns></returns>
    public static Texture2D TextureFromHeightMap(float[,] _HeightMap)
    {
        //地图宽度
        int _Width = _HeightMap.GetLength(0);
        //地图高度
        int _Height = _HeightMap.GetLength(1);

        //根据传入值  确定渲染纹理长宽
        //Texture2D _Texture = new Texture2D(_Width, _Height);

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
                _ColourMap[y * _Width + x] = Color.Lerp(Color.black, Color.white, _HeightMap[x, y]);
            }
        }
        //色彩传递
        //纹理贴图赋值
        //_Texture.SetPixels(_ColourMap);
        //_Texture.Apply();


        return TextureFromColourMap(_ColourMap, _Width, _Height);
    }
}
