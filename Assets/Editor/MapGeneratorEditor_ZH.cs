using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// 菜单编辑
/// </summary>
[CustomEditor(typeof(MapGenertor_ZH))]
public class MapGeneratorEditor_ZH : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenertor_ZH _MapGen = (MapGenertor_ZH)target;

        //绘制内置检查器
        //如果发现变更
        if (DrawDefaultInspector())
        {
            //如果更新布尔为 Ture  就持续更新地图
            if (_MapGen._AutoUpdate)
            {
                _MapGen.GenerateMap();
            }
        }

        //在 MapGenertor_ZH 上创建更新按钮
        if (GUILayout.Button("Generate"))
        {
            _MapGen.GenerateMap();
        }
    }
}

