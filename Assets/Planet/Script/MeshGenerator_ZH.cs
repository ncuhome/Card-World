using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网格生成器
/// </summary>
public static class MeshGenerator_ZH
{
    /// <summary>
    /// 地形网格生成
    /// </summary>
    /// <param 高度地图="_HeightMap"></param>
    public static MeshData GenerateTerrainMesh(float[,] _HeightMap)
    {
        //宽度
        int _Width = _HeightMap.GetLength(0);
        //高度
        int _Height = _HeightMap.GetLength(1);

        //网格划分
        float _TopLeftX = (_Width - 1) / -2f;
        float _TopLeftZ = (_Height - 1) / -2f;

        //网格数据
        MeshData _MeshData = new MeshData(_Width, _Height);

        //三角形 绘制序号
        int _VertexIndex = 0;

        //网格数据填充
        for (int y = 0; y < _Height; y++)
        {
            for (int x = 0; x < _Width; x++)
            {
                //顶点数据填充
                _MeshData._Vertices[_VertexIndex] = new Vector3(_TopLeftX + x, _HeightMap[x, y], _TopLeftZ + y);
                //UV 数据填充
                _MeshData._UVs[_VertexIndex] = new Vector2(x / (float)_Width, y / (float)_Height);

                //剔除 行 列 最边缘 的顶点  不计入网格渲染计算
                if (x < _Width - 1 && y < _Height - 1)
                {
                    _MeshData.AddTrianle(_VertexIndex, _VertexIndex + _Width + 1, _VertexIndex + _Width);
                    _MeshData.AddTrianle(_VertexIndex + _Width + 1, _VertexIndex, _VertexIndex + 1);
                }
                //序号增加
                _VertexIndex++;
            }
        }

        return _MeshData;
    }
}

/// <summary>
/// 网格数据
/// </summary>
public class MeshData
{
    //顶点数据
    public Vector3[] _Vertices;
    //绘制序列
    public int[] _Triangles;

    //UV 数据
    public Vector2[] _UVs;

     //三角 序号
    int _TriangleIndex;

    public MeshData(int _MeshWidth,int _MeshHeight)
    {
        //网格顶点数据
        _Vertices = new Vector3[_MeshWidth * _MeshHeight];

        //UV 数据
        _UVs = new Vector2[_MeshWidth * _MeshHeight];

        //三角形绘制序列
        _Triangles = new int[(_MeshWidth - 1) * (_MeshHeight - 1) * 6];
    }
    /// <summary>
    /// 网格三角形绘制
    /// </summary>
    /// <param 顶点A="_A"></param>
    /// <param 顶点B="_B"></param>
    /// <param 顶点C="_C"></param>
    public void AddTrianle(int _A,int _B,int _C)
    {
        //例如：
        // 1 2 3
        // 4 5 6
        // 7 8 9
        // 绘制 逻辑为  124 245 235 356 457 578 568 689  右手定则  逆时针 法线方向朝上
        //注意 绘制的的方向要统一   统一顺时针绘制  或者逆时针绘制
        _Triangles[_TriangleIndex] = _C;
        _Triangles[_TriangleIndex+1] = _B;
        _Triangles[_TriangleIndex+2] = _A;
        _TriangleIndex += 3;
    }

    /// <summary>
    /// 新建网格
    /// </summary>
    /// <returns></returns>
    public Mesh CreteMesh()
    {
        //网格数据
        Mesh _Mesh = new Mesh();
        //顶点数据赋予
        _Mesh.vertices = _Vertices;
        //三角形序列赋予
        _Mesh.triangles = _Triangles;
        //UV 数据 赋予
        _Mesh.uv = _UVs;
        //使用默认法线
        _Mesh.RecalculateNormals();
        //数据返回
        return _Mesh;
    }
}
