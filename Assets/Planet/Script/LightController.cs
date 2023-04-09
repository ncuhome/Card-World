using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public float time = 0;
    public Transform lightCenter;
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1 / rotateSpeed)
        {
            time -= 1 / rotateSpeed;
        }
        lightCenter.eulerAngles = new Vector3(0, time * rotateSpeed * 360, -90);
    }
}
