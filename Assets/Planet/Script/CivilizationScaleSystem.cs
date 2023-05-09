using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationScaleSystem : MonoBehaviour
{
    public static CivilizationScaleSystem Instance = null;
    public int maxCivilizationScale = 120;
    public int civilizationScale = 0;
    public bool civilizationScaleOverFlow = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance CivilizationScaleSystem");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        civilizationScale = CharacterSystem.Instance.GetPopulation() + BuildingSystem.Instance.GetBuildingNums() * 3 + BuildingSystem.Instance.GetPioneeredBlockNum() * 10;
        if (civilizationScale > maxCivilizationScale)
        {
            civilizationScaleOverFlow = true;
        }
        else
        {
            civilizationScaleOverFlow = false;
        }
    }
}
