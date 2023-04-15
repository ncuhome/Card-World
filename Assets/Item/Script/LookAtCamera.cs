using UnityEngine;

/// <summary>
/// 挂在需要看向摄像机的场景物体上，使物体始终固定于某轴面向摄像机
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    [Header("面向的摄像机Camera")]
    public Camera cameraToLookAt;
    public bool lookAtCamera = true;

    public void Start()
    {
        //如果是水资源或者石油或者农田就不旋转

    }
    void Update()
    {
        if (transform.parent.GetComponent<Resources>().resourceType == ResourceType.Water || transform.parent.GetComponent<Resources>().resourceType == ResourceType.Oil
         || transform.parent.GetComponent<Building>().buildingType == BuildingType.OriginalFarmland || transform.parent.GetComponent<Building>().buildingType == BuildingType.Farm)
        {
            lookAtCamera = false;
        }
        else
        {
            lookAtCamera = true;
        }
        
        if (!lookAtCamera) { return; }
        //若cameraToLookAt为空，则自动选择主摄像机
        if (cameraToLookAt == null)
            cameraToLookAt = Camera.main;

        transform.LookAt(cameraToLookAt.transform.position);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
