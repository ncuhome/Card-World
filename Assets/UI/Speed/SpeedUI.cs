using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{
    public void Speed1()
    {
        Time.timeScale = 1f;
    }
    public void Speed2()
    {
        Time.timeScale = 2f;
    }
    public void Speed4()
    {
        Time.timeScale = 4f;
    }
}
