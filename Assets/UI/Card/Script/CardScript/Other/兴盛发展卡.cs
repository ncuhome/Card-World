using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 兴盛发展卡 : Card 
{
    public override void BeUse()
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[5]);
        foreach (Character character in CharacterSystem.Instance.characters)
        {
            if(character != null)
            {
                if (Random.Range(0 ,9) == 5)
                {
                    CreateController.Instance.CreateItem(ItemType.Character, null, null,character.specialSkill);
                }
            }
        }
    }
}
