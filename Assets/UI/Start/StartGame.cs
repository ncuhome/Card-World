using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void Start()
    {
        Time.timeScale = 0f;
    }
    public void StartThisGame()
    {
        Debug.Log("start");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
