using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscUI : MonoBehaviour
{
    public GameObject Esc;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Esc.activeInHierarchy == true)
            {
                Esc.SetActive(false);
                Time.timeScale = 1f;
            }
            else if (Esc.activeInHierarchy == false)
            {
                Esc.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
    public void Quit()    //退出游戏
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        //打包时不能使用
        //UnityEditor.EditorApplication.isPlaying = false;
        //测试时不能执行，打包后可以执行
        Application.Quit();
    }

    public void ReLoad() //重新加载场景
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        Debug.Log("Relodad");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Esc.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        AudioManger.instance.effetPlaySound(AudioManger.instance.audioClips[7]);
        Debug.Log("Continue");
        Esc.SetActive(false);
        Time.timeScale = 1f;
    }
}
