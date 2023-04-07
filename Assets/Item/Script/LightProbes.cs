using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Illumination { day, night }
public class LightProbes : MonoBehaviour
{
    public Illumination illumination = Illumination.day;
    public float eulerAngles;
    public Transform lightTransform;
    // Start is called before the first frame update
    void Start()
    {
        lightTransform = GameObject.Find("Directional Light Center").transform;
    }

    // Update is called once per frame
    void Update()
    {
        eulerAngles = Vector3.Angle(transform.rotation * Vector3.up,lightTransform.rotation * Vector3.up);
        if (eulerAngles > 90f)
        {
            illumination = Illumination.night;
        }
        else
        {
            illumination = Illumination.day;
        }
    }
}
