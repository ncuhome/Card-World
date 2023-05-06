using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    public static PopulationController Instance = null;
    public float CivilizationScale = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CivilizationScale = 1 - Mathf.Exp(-CharacterSystem.Instance.GetPopulation()); //公式写在这里，当前公式为一个示例，意为 1-（e的-角色数）
        // 想要获取角色数，使用 CharacterSystem.Instance.GetPopulation()
        // 想要获取建筑数量，使用 BuildingSystem.Instance.GetBuildingNums()
        // 想要获取居住建筑数量，使用 BuildingSystem.Instance.GetHomeNums()
        
        //提供一些数学工具，都在 Mathf 类中，使用方法为 Mathf.XXX  
        //Sin Cos Tan Asin Acos Atan Atan2  如 Mathf.Sin(1)
        //PI常量 Mathf.PI = 3.14
        //Pow 次方 使用方法为 Mathf.Pow(f,p),其中f与p都为float类型，返回f的p次幂，例如 Mathf.Pow(2f,1.2f) , Mathf.Pow(3.5f,7.5f)
        //Sqrt 开方 使用方法为 Mathf.Sqrt(f), 返回 f 的开方
        //Exp e的幂次 使用方法为 Mathf.Exp(f), 返回 e的f次幂
        //Ceil 和 Floor 对实数向上取整和向下取整
        //Max 和 Min 区二数中的最大或者最小返回
    }
}
