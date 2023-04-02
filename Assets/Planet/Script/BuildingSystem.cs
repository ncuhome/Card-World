using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance = null;
    public int size;
    public int targetResource;
    public int[] builders;
    private Vector3 targetEuler;
    private float buildProgress;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        builders = CharacterSystem.Instance.GetBuilders(size, targetResource);
        if (builders != null)
        {
            buildProgress = 0;
            Vector3 targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            while ((ColorSystem.ColorExt.Difference(GetColorSystem.Instance.GetColor(Quaternion.Euler(targetEuler) * Vector3.up), ColorSystem.Instance.colors[0]) < 0.1f)
                || (CharacterSystem.Instance.characters[builders[0]].item.blockNum != BlockSystem.Instance.GetBlockNum(Vector3.zero, Quaternion.Euler(targetEuler))))
            {
                targetEuler = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            }
            foreach (int builderNum in builders)
            {
                CharacterSystem.Instance.characters[builderNum].goToBuild = true;
                CharacterSystem.Instance.characters[builderNum].WalkToTargetEuler(targetEuler);
            }
        }
    }

    public void Build(Character builder)
    {
        builder.resourceNum -= targetResource;
        //寿命减少（预留
        buildProgress += 1f / size;
        if (buildProgress >= 1)
        {
            CreateController.Instance.CreateItem(CreateController.ItemName.City, builder.transform.eulerAngles);
        }
    }
}
