using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public Vector3 targetEuler;
    public Quaternion oldQua;
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        targetEuler = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
        oldQua = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(targetEuler);
        transform.rotation = Quaternion.Slerp(oldQua, targetRotation, Time.deltaTime * rotateSpeed);
    }
}
