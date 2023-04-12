using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Era { AncientEra, ClassicalEra, IndustrialEra }
public class EraSystem : MonoBehaviour
{
    public static EraSystem Instance = null;
    public Era era = Era.AncientEra;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance EraSystem");
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
}
