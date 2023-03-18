using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPix : MonoBehaviour
{
    public GameObject target = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, transform.position + transform.up * 100, Color.green);
    }

    public Color GetColor()
    {
        target = GameObject.Find("Planet");

        RaycastHit hitInfo;
        Ray rayForward = new Ray(transform.position, transform.up);
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
