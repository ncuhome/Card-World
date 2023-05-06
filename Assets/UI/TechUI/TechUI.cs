using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUI : MonoBehaviour
{
    public GameObject techUI;
    public bool open = true;
    public float time;
    private void Start()
    {
        
    }
    private void Update()
    {
       
         
    }
    public void OpenCloseTechUI()
    {
        techUI.SetActive(false);
    }
}
