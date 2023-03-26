using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSystem : MonoBehaviour
{
    public static ColorSystem Instance = null;
    public Color[] colors = new Color[4];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public class ColorExt
    {
        public static float Difference(Color c1, Color c2)
        {
            c1 *= 255; c2 *= 255;
            var averageR = (c1.r + c2.r) * 0.5f;
            return Mathf.Sqrt((2 + averageR / 255f) * Mathf.Pow(c1.r - c2.r, 2) + 4 * Mathf.Pow(c1.g - c2.g, 2) + (2 + (255 - averageR) / 255f) * Mathf.Pow(c1.b - c2.b, 2)) / (3 * 255f);
        }
    }
}
