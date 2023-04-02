using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem : MonoBehaviour
{
    public static CharacterSystem Instance = null;
    public Character[] characters = new Character[100];
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance CharacterSystem");
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

    public int[] GetBuilders(int size, int targetResource)
    {
        int[] builders;
        for (int i = 0; i < 24; i++)
        {
            int num = 0;
            builders = new int[4];
            for (int j = 0; j < 100; j++)
            {
                if ((characters[j] != null) && (characters[j].item.blockNum == i) && (characters[j].resourceNum >= targetResource) && (characters[j].goToBuild == false))
                {
                    builders[num] = j;
                    num++;
                }
                if (num >= size)
                {
                    return builders;
                }
            }
        }
        return null;
    }
}
