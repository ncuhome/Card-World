using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetColorSystem : MonoBehaviour
{
    public static GetColorSystem Instance = null;
    public GameObject target = null;
    private void Awake()
    {
        //生成实例
        if (GetColorSystem.Instance == null)
        {
            GetColorSystem.Instance = this;
            Debug.Log("Instance GetColorSystem");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Planet");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //输入指定的旋转方向获取对应的球面点颜色
    public Color GetColor(Transform point)
    {
        RaycastHit hitInfo;
        Ray rayForward = new Ray(target.transform.position, point.up);
        Ray rayBack = Reverse(rayForward, 3000);
        if (Physics.Raycast(rayBack, out hitInfo))
        {
            //Debug.DrawLine(transform.position, transform.position + transform.up * 100, Color.green);
            //Debug.Log(hitInfo.collider.gameObject.name);
            Vector2 uv = hitInfo.textureCoord;
            Renderer rendere = target.GetComponent<MeshRenderer>();
            Material material = rendere.material;
            Texture2D texture = material.mainTexture as Texture2D;
            int width = texture.width;
            int height = texture.height;
            int pixelx = (int)(uv.x * width);
            int pixely = (int)(uv.y * height);

            //print(pixelx + " " + pixely);
            Color color = texture.GetPixel(pixelx, pixely);
            //print("------" + color.r + "--" + color.g + "--" + color.b);
            return color;
        }
        else
        {
            return Color.white;
        }
    }

    public Ray Reverse(Ray ray, float distance)
    {
        return new Ray(ray.origin + ray.direction * distance, -ray.direction);
    }
}
