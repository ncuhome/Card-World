using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowBlockRes : MonoBehaviour
{
    public Text[] BlockResText;

    public GameObject[] res;

    public int nowBlock; //现在所指的区块

    public bool change;
    void Update()
    {
        change = ShowResManger.change;
        if (change == true)
        {
            BlockResText[0].text = res[0].name;
            BlockResText[1].text = res[1].name;
            BlockResText[2].text = res[2].name;
        }
        else
        {
            BlockResText[0].text = BlockSystem.Instance.blocks[nowBlock].temperature.ToString();
            BlockResText[1].text = BlockSystem.Instance.blocks[nowBlock].water.ToString();
            BlockResText[2].text = BlockSystem.Instance.blocks[nowBlock].livability.ToString();
        }   
        if (Input.GetMouseButtonDown(0))
        {
            if (MouseOnSphere.instance.ReturnMousePosition() != Vector3.zero)
            {
                nowBlock = BlockSystem.Instance.GetBlockNum(MouseOnSphere.instance.sphere.transform.position, MouseOnSphere.instance.ReturnMousePosition());
                Debug.Log("点击到了" + nowBlock + "区块");
            }
        }
    }
}
